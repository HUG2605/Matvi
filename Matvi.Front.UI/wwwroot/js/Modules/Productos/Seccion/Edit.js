//#region MODAL

var modalElement = document.getElementById(window.seccionEditConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region EDIT

$("#" + window.seccionEditConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.seccionEditConfig.idForm).valid()) {
        Matvi.confirm(window.seccionEditConfig.title, "¿Confirma que desea guardar la información de la sección?", function (resp) {
            if (resp == true) {
                $("#" + window.seccionEditConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Edit(response) {
    Matvi.alert(response.messageType, window.seccionEditConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion