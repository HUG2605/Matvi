//#region MODAL

var modalElement = document.getElementById(window.cajaCerrarConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region GUARDAR

$("#btnGuardar").on("click", function () {
    if ($("#" + window.cajaCerrarConfig.idForm).valid()) {
        Matvi.confirm(window.cajaCerrarConfig.title, "¿Confirma que desea cerrar la caja?", function (resp) {
            if (resp == true) {
                $("#" + window.cajaCerrarConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO

function Success_Cerrar(response) {
    if (response.isSuccessful) {
        const d = response.data;
        const detalle =
            `Cobros: <strong>$ ${Matvi.formatMiles(parseFloat(d.totalCobros) || 0)}</strong><br>` +
            `Gastos: <strong>$ ${Matvi.formatMiles(parseFloat(d.totalGastos) || 0)}</strong><br>` +
            `Pagos: <strong>$ ${Matvi.formatMiles(parseFloat(d.totalPagos) || 0)}</strong><br>` +
            `Fondo final (sistema): <strong>$ ${Matvi.formatMiles(parseFloat(d.fondoFinalSistema) || 0)}</strong><br>` +
            `Fondo final (contado): <strong>$ ${Matvi.formatMiles(parseFloat(d.fondoFinalContado) || 0)}</strong><br>` +
            `Diferencia: <strong>$ ${Matvi.formatMiles(parseFloat(d.diferencia) || 0)}</strong>`;

        Matvi.alert(response.messageType, window.cajaCerrarConfig.title, `${response.message}`, function () {
            Matvi.confirm('Resumen del cierre', detalle, function (resp) {
                window.location.reload();
            });
        });
    } else {
        Matvi.alert(response.messageType, window.cajaCerrarConfig.title, response.message, function () {
            window.location.reload();
        });
    }
}

//#endregion