
//#region MODAL

var modal = bootstrap.Modal.getOrCreateInstance(document.getElementById(window.rolConfig.idModal));

$(function () {
    modal.toggle();
});

var modalElement = document.getElementById(window.rolConfig.idModal);
modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region EDIT

$("#" + window.rolConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#"+ window.rolConfig.idForm).valid()) {

        Matvi.confirm(window.rolConfig.title, "¿Confirma que desea guardar la información de del rol?", function (resp) {
            if (resp == true) {
                $("#" + window.rolConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Edit(response) {
    //debugger;
    Matvi.alert(response.messageType, window.rolConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion