//#region MODAL

var modalElement = document.getElementById(window.gastoEditConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', () => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region GUARDAR

$("#btnGuardar").on("click", function () {
    if ($("#" + window.gastoEditConfig.idForm).valid()) {
        Matvi.confirm(window.gastoEditConfig.title, "¿Confirma que desea guardar la información del gasto?", function (resp) {
            if (resp == true) {
                $("#" + window.gastoEditConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO

function Success_Edit(response) {
    Matvi.alert(response.messageType, window.gastoEditConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion