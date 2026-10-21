//#region MODAL

var modalElement = document.getElementById(window.tipoGastoCreateConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region CREATE

$("#" + window.tipoGastoCreateConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.tipoGastoCreateConfig.idForm).valid()) {

        Matvi.confirm(window.tipoGastoCreateConfig.title, "¿Confirma que desea guardar la información del tipo de gasto?", function (resp) {
            if (resp == true) {
                $("#" + window.tipoGastoCreateConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Create(response) {
    //debugger;
    Matvi.alert(response.messageType, window.tipoGastoCreateConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion