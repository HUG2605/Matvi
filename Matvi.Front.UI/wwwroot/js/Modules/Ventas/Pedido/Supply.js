//#region MODAL

var modalElement = document.getElementById(window.supplyConfig.idModal);
var modal = bootstrap.Modal.getInstance(modalElement)
    ?? new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('shown.bs.modal', (event) => {

    new MVStepper('.mv-steps-vertical', {
        formSelector: '#' + window.supplyConfig.idForm,
        buttonSelectors: {
            previous: '#btnAnterior',
            next: '#btnSiguiente',
            submit: '#btnSurtir'
        },
        onStepChange: (stepIndex) => {
            // Al entrar al paso 3, autoseleccionar la modalidad según entregaDomicilio
            if (stepIndex === 2) {
                preseleccionarModalidad();
            }
        }
    });

    // Inicializar Select2 de repartidor
    Matvi.initSelect2(
        '#slcRepartidor',
        1,
        '#' + window.supplyConfig.idModal,
        '/Seguridad/Api/Usuario/repartidor-select'
    );

    // Cambio de radio: mostrar/ocultar bloques
    $('input[name="TipoSurtidoRadio"]').on('change', function () {
        aplicarVisibilidadModalidad();
    });

    // El confirmar-checkbox habilita el botón Surtir cuando es entrega directa
    $('#chkConfirmacionDirecto').on('change', function () {
        actualizarBotonSurtirHabilitado();
    });

    // El select de repartidor habilita el botón Surtir cuando hay valor
    $('#slcRepartidor').on('change', function () {
        actualizarBotonSurtirHabilitado();
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

//#region VISIBILIDAD DE MODALIDAD

function preseleccionarModalidad() {
    const rbRepartidor = $('#rbRepartidor');
    const rbDirecto = $('#rbDirecto');

    if (rbRepartidor.is(':checked') || rbDirecto.is(':checked')) {
        // El usuario ya eligió, respetamos su selección
        aplicarVisibilidadModalidad();
        return;
    }

    // Default según entrega a domicilio
    if (window.supplyConfig.entregaDomicilio) {
        rbRepartidor.prop('checked', true).trigger('change');
    } else {
        rbDirecto.prop('checked', true).trigger('change');
    }
}

function aplicarVisibilidadModalidad() {
    const tipo = $('input[name="TipoSurtidoRadio"]:checked').val();
    const $bloqueRepartidor = $('#bloqueRepartidor');
    const $bloqueDirecto = $('#bloqueDirecto');

    if (tipo === '1') {
        $bloqueRepartidor.removeClass('d-none');
        $bloqueDirecto.addClass('d-none');
        // Si el checkbox de confirmación estaba marcado, desmarcarlo
        $('#chkConfirmacionDirecto').prop('checked', false);
    } else if (tipo === '2') {
        $bloqueRepartidor.addClass('d-none');
        $bloqueDirecto.removeClass('d-none');
        // Limpiar select de repartidor
        $('#slcRepartidor').val(null).trigger('change');
    } else {
        $bloqueRepartidor.addClass('d-none');
        $bloqueDirecto.addClass('d-none');
    }

    actualizarBotonSurtirHabilitado();
}

function actualizarBotonSurtirHabilitado() {
    const tipo = $('input[name="TipoSurtidoRadio"]:checked').val();
    const btn = $('#btnSurtir');

    if (tipo === '1') {
        // Debe tener repartidor seleccionado
        const idRepartidor = $('#slcRepartidor').val();
        btn.prop('disabled', !idRepartidor);
    } else if (tipo === '2') {
        // Debe tener checkbox confirmado
        btn.prop('disabled', !$('#chkConfirmacionDirecto').is(':checked'));
    } else {
        btn.prop('disabled', true);
    }
}

//#endregion

//#region VALIDACIÓN DE PASO 3

// El MVStepper.validateCurrentStep() recorre inputs/selects dentro del collapse
// activo. Cuando la modalidad A está activa, el select de repartidor está visible
// y jQuery Validate lo evalúa (su name="IdCtUsuarioRepartidor" coincide con el DTO).
// Cuando es B, el checkbox htm5 "required" exige marcado.
// Aquí solo reforzamos la regla para el select, ya que data-val="true" no se
// generó en el .cshtml.

$(function () {
    $.validator.addMethod("requiereRepartidor", function (value, element) {
        const tipo = $('input[name="TipoSurtidoRadio"]:checked').val();
        if (tipo !== '1') return true; // No aplica si la modalidad no es REPARTIDOR
        return value && value.toString().trim() !== '';
    }, "Debe seleccionar un repartidor.");

    $.validator.addMethod("requiereConfirmacion", function (value, element) {
        const tipo = $('input[name="TipoSurtidoRadio"]:checked').val();
        if (tipo !== '2') return true; // No aplica si la modalidad no es DIRECTO
        return element.checked === true;
    }, "Debe confirmar la entrega directa.");

    $('#slcRepartidor').rules('add', { requiereRepartidor: true });
    $('#chkConfirmacionDirecto').rules('add', { requiereConfirmacion: true });
});

//#endregion

//#region SURTIR

function construirLineasSurtir() {
    const lineas = [];
    $('#tblSupplyProductos tbody tr').each(function () {
        const $input = $(this).find('.txt-surtir');
        if ($input.length === 0 || $input.prop('disabled')) return;

        const idDetalle = parseInt($input.attr('data-id'), 10);
        const cant = parseFloat($input.val()) || 0;
        if (cant > 0) {
            lineas.push({
                IdOpPedidoDetalle: idDetalle,
                CantidadASurtir: cant
            });
        }
    });
    return lineas;
}

$('#' + window.supplyConfig.idForm).on('submit', function (e) {
    e.preventDefault();

    const lineas = construirLineasSurtir();
    if (lineas.length === 0) {
        Matvi.alert(
            Matvi.alertType.Danger,
            window.supplyConfig.confirmTitle,
            'Debes ingresar una cantidad a surtir mayor a cero en al menos un producto.'
        );
        return false;
    }

    const idPedido = parseInt($('#hdnIdPedido').val(), 10);
    const tipoSurtido = parseInt($('input[name="TipoSurtidoRadio"]:checked').val(), 10);
    const idRepartidor = $('#slcRepartidor').val();

    const payload = {
        Id: idPedido,
        TipoSurtido: tipoSurtido,
        IdCtUsuarioRepartidor: tipoSurtido === 1 ? parseInt(idRepartidor, 10) : null,
        Lineas: lineas
    };

    $.ajax({
        url: $(this).attr('action') || window.location.href.split('?')[0],
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
            handleSupplyResponse(response);
        },
        error: function (xhr) {
            Matvi.alert(
                Matvi.alertType.Danger,
                window.supplyConfig.confirmTitle,
                'Error al surtir el pedido: ' + (xhr.responseText || xhr.statusText)
            );
        }
    });

    return false;
});

function handleSupplyResponse(response) {
    Matvi.alert(
        response.messageType,
        window.supplyConfig.confirmTitle,
        response.message,
        function () {
            if (response.messageType === Matvi.alertType.Success) {
                window.location.reload();
            }
        }
    );
}

function Success_Supply(response) {
    handleSupplyResponse(response);
}

//#endregion

//#region CONFIRMACIÓN DE Surtir

$('#btnSurtir').on('click', function () {
    const form = $('#' + window.supplyConfig.idForm);
    if (!form.valid()) return;

    const lineas = construirLineasSurtir();
    if (lineas.length === 0) {
        Matvi.alert(
            Matvi.alertType.Danger,
            window.supplyConfig.confirmTitle,
            'Debes ingresar una cantidad a surtir mayor a cero en al menos un producto.'
        );
        return;
    }

    Matvi.confirm(
        window.supplyConfig.confirmTitle,
        '¿Confirma que desea registrar el surtido con las cantidades indicadas?',
        function (resp) {
            if (resp === true) {
                form.trigger('submit');
            }
        }
    );
});

//#endregion
