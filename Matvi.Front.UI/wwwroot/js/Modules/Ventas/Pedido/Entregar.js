//#region MODAL

var modalElement = document.getElementById(window.entregarConfig.idModal);
var modal = bootstrap.Modal.getInstance(modalElement)
    ?? new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('shown.bs.modal', (event) => {

    new MVStepper('.mv-steps-vertical', {
        formSelector: '#' + window.entregarConfig.idForm,
        buttonSelectors: {
            previous: '#btnEntregarAnterior',
            next: '#btnEntregarSiguiente',
            submit: '#btnRegistrarEntrega'
        },
        onStepChange: (stepIndex) => {
            // Cambio de paso
        },
        validateStep: (stepIndex) => {
            // Solo validamos el paso 2 (Productos)
            if (stepIndex === 1) {
                return validarProductos();
            }
            return true;
        }
    });

    actualizarRechazoUI();
});

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    if (modal) {
        modal.dispose();
        modal = null;
    }
});

$(document).on('input', '.txt-rechazar', function () {
    const $tr = $(this).closest('tr');
    const custodia = parseFloat($tr.find('[data-custodia]').first().attr('data-custodia')) || 0;
    const rechazar = parseFloat($(this).val()) || 0;
    const $entregar = $tr.find('.txt-entregar');
    if ($entregar.length > 0 && rechazar >= 0 && rechazar <= custodia) {
        $entregar.val(custodia - rechazar);
    }
    actualizarRechazoUI();
});

function actualizarRechazoUI() {
    let totalRechazo = 0;
    $('.txt-rechazar').each(function () {
        totalRechazo += parseFloat($(this).val()) || 0;
    });
    if (totalRechazo > 0) {
        $('#lblMotivoRechazo').addClass('mv-form-label-required');
    } else {
        $('#lblMotivoRechazo').removeClass('mv-form-label-required');
    }
}

//#endregion

//#region VALIDACIÓN DE CANTIDADES

function validarLinea($tr) {
    const custodia = parseFloat($tr.find('[data-custodia]').first().attr('data-custodia')) || 0;
    const entregar = parseFloat($tr.find('.txt-entregar').val()) || 0;
    const rechazar = parseFloat($tr.find('.txt-rechazar').val()) || 0;

    if (entregar < 0 || rechazar < 0) return 'Las cantidades no pueden ser negativas.';
    if (entregar + rechazar > custodia) {
        return 'Entregar/rechazar excede la custodia disponible (' + custodia + ').';
    }
    return null;
}

function validarProductos() {
    let hayCantidad = false;
    let hayRechazo = false;
    let error = null;

    $('#tblEntregar tbody tr').each(function () {
        if ($(this).find('.txt-entregar').length === 0) return;

        const $tr = $(this);
        const entregar = parseFloat($tr.find('.txt-entregar').val()) || 0;
        const rechazar = parseFloat($tr.find('.txt-rechazar').val()) || 0;

        if (entregar > 0 || rechazar > 0) hayCantidad = true;
        if (rechazar > 0) hayRechazo = true;

        error = validarLinea($tr);
        if (error) return false;
    });

    if (error) {
        Matvi.alert(Matvi.alertType.Danger, window.entregarConfig.confirmTitle, error);
        return false;
    }
    if (!hayCantidad) {
        Matvi.alert(Matvi.alertType.Danger, window.entregarConfig.confirmTitle,
            'Debe capturar cantidades entregadas o rechazadas en al menos una línea.');
        return false;
    }

    const motivoRechazo = $('#txtMotivoRechazo').val().trim();
    if (hayRechazo && !motivoRechazo) {
        Matvi.alert(Matvi.alertType.Danger, window.entregarConfig.confirmTitle,
            'Debe indicar el motivo de rechazo en el campo de texto.');
        $('#txtMotivoRechazo').focus();
        return false;
    }

    return true;
}

//#endregion

//#region REGISTRAR ENTREGA

function construirPayload() {
    const lineas = [];
    const idPedido = parseInt($('#hdnIdPedido').val(), 10);
    const motivoGeneral = $('#txtMotivoRechazo').val().trim() || null;

    $('#tblEntregar tbody tr').each(function () {
        if ($(this).find('.txt-entregar').length === 0) return;

        const $tr = $(this);
        const idDetalle = parseInt($tr.find('.txt-entregar').attr('data-id'), 10);
        const entregar = parseFloat($tr.find('.txt-entregar').val()) || 0;
        const rechazar = parseFloat($tr.find('.txt-rechazar').val()) || 0;

        if (entregar === 0 && rechazar === 0) return;

        lineas.push({
            IdOpPedidoDetalle: idDetalle,
            CantidadEntregar: entregar,
            CantidadRechazar: rechazar,
            NotasRechazo: rechazar > 0 ? motivoGeneral : null
        });
    });

    return {
        Id: idPedido,
        Lineas: lineas,
        MontoCobro: null,
        IdCtMedioPago: null,
        IdCtCuentaBancaria: null,
        NotasCobro: null
    };
}

$('#btnRegistrarEntrega').on('click', function () {
    if (!validarProductos()) return;

    Matvi.confirm(
        window.entregarConfig.confirmTitle,
        '¿Confirma que el pedido fue entregado con las cantidades indicadas?',
        function (resp) {
            if (resp !== true) return;

            const payload = construirPayload();

            $.ajax({
                url: $('#' + window.entregarConfig.idForm).attr('action')
                    || window.location.href.split('?')[0],
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(payload),
                headers: {
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                beforeSend: function () {
                    Matvi.blockUI();
                },
                complete: function () {
                    Matvi.unblockUI();
                },
                success: function (response) {
                    handleEntregarResponse(response);
                },
                error: function (xhr) {
                    Matvi.alert(
                        Matvi.alertType.Danger,
                        window.entregarConfig.confirmTitle,
                        'Error al registrar la entrega: ' + (xhr.responseText || xhr.statusText)
                    );
                }
            });
        }
    );
});

function handleEntregarResponse(response) {
    Matvi.alert(
        response.messageType,
        window.entregarConfig.confirmTitle,
        response.message,
        function () {
            if (response.messageType === Matvi.alertType.Success) {
                window.location.reload();
            }
        }
    );
}

function Success_Entregar(response) {
    handleEntregarResponse(response);
}

//#endregion