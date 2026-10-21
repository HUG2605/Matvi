
//#region MODAL

var modalElement = document.getElementById(window.clienteEditConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('shown.bs.modal', () => {
    new MVStepper('.mv-steps-vertical', {
        formSelector: '#' + window.clienteEditConfig.idForm,
        buttonSelectors: {
            previous: '#btnAnterior',
            next: '#btnSiguiente',
            submit: '#btnGuardar'
        },
        onStepChange: (stepIndex) => {
            var currentStep = stepIndex + 1;
            console.log('Paso actual:', currentStep);
        }
    });
});


modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region EDIT

$("#" + window.clienteEditConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.clienteEditConfig.idForm).valid()) {

        Matvi.confirm(window.clienteEditConfig.title, "¿Confirma que desea guardar la información del cliente?", function (resp) {
            if (resp == true) {
                $("#" + window.clienteEditConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Edit(response) {
    //debugger;
    Matvi.alert(response.messageType, window.clienteEditConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion