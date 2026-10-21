//#region MODAL

var modalElement = document.getElementById(window.inventarioEditConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region EDIT

$("#" + window.inventarioEditConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.inventarioEditConfig.idForm).valid()) {

        Matvi.confirm(window.inventarioEditConfig.title, "¿Confirma que desea guardar la información del producto en la sucursal?", function (resp) {
            if (resp == true) {
                $("#" + window.inventarioEditConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Edit(response) {
    //debugger;
    Matvi.alert(response.messageType, window.inventarioEditConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion