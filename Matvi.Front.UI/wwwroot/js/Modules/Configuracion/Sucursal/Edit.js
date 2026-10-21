//#region MODAL

var modalElement = document.getElementById(window.sucursalConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region EDIT

$("#" + window.sucursalConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.sucursalConfig.idForm).valid()) {

        Matvi.confirm(window.sucursalConfig.title, "¿Confirma que desea guardar la información de la sucursal?", function (resp) {
            if (resp == true) {
                $("#" + window.sucursalConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Edit(response) {
    //debugger;
    Matvi.alert(response.messageType, window.sucursalConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion
