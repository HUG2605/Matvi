//#region MODAL Y STEPPER

var modalElement = document.getElementById(window.cancelarConfig.idModal);
var modal = bootstrap.Modal.getInstance(modalElement)
    ?? new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('shown.bs.modal', (event) => {

    new MVStepper('.mv-steps-vertical', {
        formSelector: '#' + window.cancelarConfig.idForm,
        buttonSelectors: {
            previous: '#btnCancelarAnterior',
            next: '#btnCancelarSiguiente',
            submit: '#btnRegistrarCancelacion'
        },
        onStepChange: (stepIndex) => {
            // Cambio de paso
        },
        validateStep: (stepIndex) => {
            if (stepIndex === 1) {
                return validarProductosYMotivo();
            }
            return true;
        }
    });

});

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    if (modal) {
        modal.dispose();
        modal = null;
    }
});

//#endregion

//#region VALIDACIONES

function validarProductosYMotivo() {
    const motivo = $('#txtMotivoCancelacion').val().trim();
    if (!motivo) {
        $('#txtMotivoCancelacion').addClass('is-invalid');
        $('#txtMotivoCancelacion-error').text('El motivo de cancelación es obligatorio.');
        Matvi.alert(
            Matvi.alertType.Warning,
            'Cancelar pedido',
            'Debe ingresar el motivo de la cancelación.'
        );
        $('#txtMotivoCancelacion').focus();
        return false;
    } else {
        $('#txtMotivoCancelacion').removeClass('is-invalid');
        $('#txtMotivoCancelacion-error').text('');
    }

    let sumaACancelar = 0;
    let hayError = false;
    let mensajeError = '';

    $('.txt-cancelar').each(function () {
        const val = parseFloat($(this).val()) || 0;
        const max = parseFloat($(this).data('cancelable')) || 0;

        if (val < 0) {
            hayError = true;
            mensajeError = 'Las cantidades no pueden ser negativas.';
            return false;
        }
        if (val > max) {
            hayError = true;
            mensajeError = `La cantidad ${val} supera el límite cancelable (${max}).`;
            return false;
        }
        sumaACancelar += val;
    });

    if (hayError) {
        Matvi.alert(
            Matvi.alertType.Warning,
            'Cancelar pedido',
            mensajeError
        );
        return false;
    }

    if (sumaACancelar <= 0) {
        Matvi.alert(
            Matvi.alertType.Warning,
            'Cancelar pedido',
            'Debe indicar una cantidad mayor a cero a cancelar en al menos un producto.'
        );
        return false;
    }

    return true;
}

function construirPayload() {
    const lineas = [];
    $('.txt-cancelar').each(function () {
        const id = parseInt($(this).attr('data-id'), 10);
        const cant = parseFloat($(this).val()) || 0;
        if (cant > 0) {
            lineas.push({
                IdOpPedidoDetalle: id,
                CantidadACancelar: cant
            });
        }
    });

    return {
        Id: parseInt(window.cancelarConfig.idPedido, 10),
        Motivo: $('#txtMotivoCancelacion').val().trim(),
        IdOpCorteCaja: window.cancelarConfig.idCajaActiva || null,
        Lineas: lineas
    };
}

//#endregion

//#region ENVÍO AJAX

$('#' + window.cancelarConfig.idForm).on('submit', function (e) {
    e.preventDefault();
    $('#btnRegistrarCancelacion').trigger('click');
});

$('#btnRegistrarCancelacion').on('click', function () {
    if (!validarProductosYMotivo()) return;

    Matvi.confirm(
        'Confirmar cancelación',
        '¿Está seguro de proceder con la cancelación de la mercancía seleccionada? Esta acción liberará el inventario reservado.',
        function (resp) {
            if (resp !== true) return;

            const payload = construirPayload();
            const urlPost = $('#' + window.cancelarConfig.idForm).attr('action')
                || window.cancelarConfig.urlPost
                || window.location.href.split('?')[0];

            $.ajax({
                url: urlPost,
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
                    handleCancelarResponse(response);
                },
                error: function (xhr) {
                    Matvi.alert(
                        Matvi.alertType.Danger,
                        'Cancelar pedido',
                        'Error al registrar la cancelación: ' + (xhr.responseText || xhr.statusText)
                    );
                }
            });
        }
    );
});

function handleCancelarResponse(response) {
    Matvi.alert(
        response.messageType,
        'Cancelar pedido',
        response.message,
        function () {
            if (response.messageType === Matvi.alertType.Success) {
                if (modal) {
                    modal.hide();
                }
                window.location.reload();
            }
        }
    );
}

function Success_Cancelar(response) {
    handleCancelarResponse(response);
}

//#endregion
