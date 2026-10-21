//#region MODAL

var modalElement = document.getElementById(window.gastoCreateConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region GUARDAR

$("#btnGuardar").on("click", function () {
    if ($("#" + window.gastoCreateConfig.idForm).valid()) {
        Matvi.confirm(window.gastoCreateConfig.title, "¿Confirma que desea registrar el gasto?", function (resp) {
            if (resp == true) {
                $("#" + window.gastoCreateConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO

function Success_Create(response) {
    Matvi.alert(response.messageType, window.gastoCreateConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion