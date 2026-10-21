
//#region MODAL

var modalElement = document.getElementById(window.empresaConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region EDIT

$("#" + window.empresaConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.empresaConfig.idForm).valid()) {

        Matvi.confirm(window.empresaConfig.title, "¿Confirma que desea guardar la información de la empresa?", function (resp) {
            if (resp == true) {
                $("#" + window.empresaConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Edit(response) {
    //debugger;
    Matvi.alert(response.messageType, window.empresaConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion
