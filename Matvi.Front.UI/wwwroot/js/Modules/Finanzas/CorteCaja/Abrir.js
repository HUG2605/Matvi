//#region MODAL & STEPPER

var modalElement = document.getElementById(window.cajaAbrirConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

var stepper = new MVStepper('.mv-steps-vertical', {
    formSelector: '#' + window.cajaAbrirConfig.idForm,
    buttonSelectors: {
        previous: '#btnAnterior',
        next: '#btnSiguiente',
        submit: '#btnGuardar'
    }
});

//#endregion

//#region GUARDAR

$("#btnGuardar").on("click", function () {
    if ($("#" + window.cajaAbrirConfig.idForm).valid()) {
        Matvi.confirm(window.cajaAbrirConfig.title, "¿Confirma que desea abrir la caja con el fondo inicial especificado?", function (resp) {
            if (resp == true) {
                $("#" + window.cajaAbrirConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO

function Success_Abrir(response) {
    Matvi.alert(response.messageType, window.cajaAbrirConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion