//#region MODAL

var modalElement = document.getElementById(window.pagoCreateConfig.idModal);
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
        '#Pago_CtProv_IdFld',
        1,
        '#' + window.pagoCreateConfig.idModal,
        '/Compras/Api/Proveedor/select'
    );
});

//#endregion

//#region CHANGE CtProv_IdFld

$("#Pago_CtProv_IdFld").on("select2:select select2:clear", function () {
    var idProveedor = $(this).val();

    if (!idProveedor) {
        $("#txtSaldoVencido").val("0.00");
        recalcular();
        return;
    }

    cargarSaldoVencido(idProveedor);
});

//#endregion

//#region FUNCTION cargarSaldoVencido

function cargarSaldoVencido(idProveedor) {
    $.ajax({
        url: '/Finanzas/CorteCaja/CrearPago?handler=SaldoVencido',
        type: 'GET',
        data: { idProveedor: idProveedor },
        success: function (saldo) {
            $("#txtSaldoVencido").val(Matvi.formatMiles(parseFloat(saldo) || 0));
            recalcular();
        },
        error: function () {
            $("#txtSaldoVencido").val("0.00");
            recalcular();
        }
    });
}

//#endregion

//#region INPUT MontoFld

$(document).on("input", "#Pago_MontoFld", function () {
    recalcular();
});

//#endregion

//#region FUNCTION recalcular

function recalcular() {
    let saldo = parseFloat($("#txtSaldoVencido").val().replace(/,/g, '')) || 0;
    let monto = parseFloat($("#Pago_MontoFld").val()) || 0;

    let pendiente = monto >= saldo ? 0 : saldo - monto;

    $("#txtSaldoPendiente").val(Matvi.formatMiles(pendiente));
    $("#txtSaldoPendiente").toggleClass("border border-info", pendiente > 0);
}

//#endregion

//#region GUARDAR

$("#btnGuardar").on("click", function () {
    if ($("#" + window.pagoCreateConfig.idForm).valid()) {
        Matvi.confirm(window.pagoCreateConfig.title, "¿Confirma que desea registrar el pago?", function (resp) {
            if (resp == true) {
                $("#" + window.pagoCreateConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO

function Success_Create(response) {
    Matvi.alert(response.messageType, window.pagoCreateConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion