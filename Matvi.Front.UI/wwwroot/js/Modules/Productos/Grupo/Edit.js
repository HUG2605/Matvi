//#region MODAL

var modalElement = document.getElementById(window.grupoEditConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region EDIT

$("#" + window.grupoEditConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.grupoEditConfig.idForm).valid()) {
        Matvi.confirm(window.grupoEditConfig.title, "¿Confirma que desea guardar la información del grupo?", function (resp) {
            if (resp == true) {
                $("#" + window.grupoEditConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Edit(response) {
    Matvi.alert(response.messageType, window.grupoEditConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion