//#region MODAL

var modalElement = document.getElementById(tipoGastoEditConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region EDIT

$("#" + tipoGastoEditConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + tipoGastoEditConfig.idForm).valid()) {

        Matvi.confirm(tipoGastoEditConfig.title, "¿Confirma que desea guardar la información del tipo de gasto?", function (resp) {
            if (resp == true) {
                $("#" + tipoGastoEditConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Edit(response) {
    //debugger;
    Matvi.alert(response.messageType, tipoGastoEditConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion