//#region MODAL

var modalElement = document.getElementById(window.cobroCreateConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region SELECT2

$(document).ready(function () {
    Matvi.initSelect2(
        '#Cobro_CtClie_IdFld',
        1,
        '#' + window.cobroCreateConfig.idModal,
        '/Ventas/Api/Cliente/select'
    );
    Matvi.setInitialSelect2Value('#Cobro_CtClie_IdFld');

    var valInicial = $('#Cobro_CtClie_IdFld').val();
    if (valInicial) {
        cargarSaldoPendiente(valInicial);
    }
});

//#endregion

//#region CHANGE CtClie_IdFld

$("#Cobro_CtClie_IdFld").on("select2:select select2:clear", function () {
    var idCliente = $(this).val();

    if (!idCliente) {
        $("#txtSaldoPendienteCliente").val("0.00");
        recalcular();
        return;
    }

    cargarSaldoPendiente(idCliente);
});

//#endregion

//#region FUNCTION cargarSaldoPendiente

function cargarSaldoPendiente(idCliente) {
    $.ajax({
        url: '/Finanzas/CorteCaja/CrearCobro?handler=SaldoPendiente',
        type: 'GET',
        data: { idCliente: idCliente },
        success: function (saldo) {
            $("#txtSaldoPendienteCliente").val(Matvi.formatMiles(parseFloat(saldo) || 0));
            recalcular();
        },
        error: function () {
            $("#txtSaldoPendienteCliente").val("0.00");
            recalcular();
        }
    });
}

//#endregion

//#region INPUT MontoFld

$(document).on("input", "#Cobro_MontoFld", function () {
    recalcular();
});

//#endregion

//#region FUNCTION recalcular

function recalcular() {
    let saldo = parseFloat($("#txtSaldoPendienteCliente").val().replace(/,/g, '')) || 0;
    let monto = parseFloat($("#Cobro_MontoFld").val()) || 0;

    let restante = monto >= saldo ? 0 : saldo - monto;

    $("#txtSaldoRestante").val(Matvi.formatMiles(restante));
    $("#txtSaldoRestante").toggleClass("border border-info", restante > 0);
}

//#endregion

//#region GUARDAR

$("#btnGuardar").on("click", function () {
    if ($("#" + window.cobroCreateConfig.idForm).valid()) {
        Matvi.confirm(window.cobroCreateConfig.title, "¿Confirma que desea registrar el cobro?", function (resp) {
            if (resp == true) {
                $("#" + window.cobroCreateConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO

function Success_Create(response) {
    Matvi.alert(response.messageType, window.cobroCreateConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion