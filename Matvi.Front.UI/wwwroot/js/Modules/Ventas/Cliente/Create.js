
//#region MODAL

var modalElement = document.getElementById(window.clienteCreateConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('shown.bs.modal', () => {
    new MVStepper('.mv-steps-vertical', {
        formSelector: "#"+ window.clienteCreateConfig.idForm,
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

//#region CREATE

$("#" + window.clienteCreateConfig.idForm +" .mv-btn-primary").on("click", function () {
    if ($("#" + window.clienteCreateConfig.idForm).valid()) {
        Matvi.confirm(window.clienteCreateConfig.title, "¿Confirma que desea guardar la información del cliente?", function (resp) {
            if (resp == true) {
                $("#" + window.clienteCreateConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region ÉXITO Y RECARGA

function Success_Create(response) {
    Matvi.alert(response.messageType, window.clienteCreateConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion