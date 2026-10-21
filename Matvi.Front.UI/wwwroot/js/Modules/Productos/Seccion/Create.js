//#region MODAL

var modalElement = document.getElementById(window.seccionCreateConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region CREATE

$("#" + window.seccionCreateConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.seccionCreateConfig.idForm).valid()) {
        Matvi.confirm(window.seccionCreateConfig.title, "¿Confirma que desea guardar la información de la sección?", function (resp) {
            if (resp == true) {
                $("#" + window.seccionCreateConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Create(response) {
    Matvi.alert(response.messageType, window.seccionCreateConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion