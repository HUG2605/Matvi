//#region MODAL

var modalElement = document.getElementById(window.grupoCreateConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region CREATE

$("#" + window.grupoCreateConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.grupoCreateConfig.idForm).valid()) {
        Matvi.confirm(window.grupoCreateConfig.title, "¿Confirma que desea guardar la información del grupo?", function (resp) {
            if (resp == true) {
                $("#" + window.grupoCreateConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Create(response) {
    Matvi.alert(response.messageType, window.grupoCreateConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion