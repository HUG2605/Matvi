//#region MODAL

var modalElement = document.getElementById(window.proveedorEditConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region EDIT

$("#" + window.proveedorEditConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.proveedorEditConfig.idForm).valid()) {
        Matvi.confirm(window.proveedorEditConfig.title, "¿Confirma que desea guardar la información del proveedor?", function (resp) {
            if (resp == true) {
                $("#" + window.proveedorEditConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Edit(response) {
    Matvi.alert(response.messageType, window.proveedorEditConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion