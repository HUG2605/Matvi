//#region MODAL

var modalElement = document.getElementById(window.proveedorCreateConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region CREATE

$("#" + window.proveedorCreateConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.proveedorCreateConfig.idForm).valid()) {
        Matvi.confirm(window.proveedorCreateConfig.title, "¿Confirma que desea guardar la información del proveedor?", function (resp) {
            if (resp == true) {
                $("#" + window.proveedorCreateConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Create(response) {
    Matvi.alert(response.messageType, window.proveedorCreateConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion