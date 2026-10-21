//#region MODAL

var modalElement = document.getElementById(window.usuarioConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('shown.bs.modal', () => {
    new MVStepper('.mv-steps-vertical', {
        formSelector: '#' + window.usuarioConfig.idForm,
        buttonSelectors: {
            previous: '#btnAnterior',
            next: '#btnSiguiente',
            submit: '#btnGuardar'
        },
        onStepChange: (stepIndex) => {
            var currentStep = stepIndex + 1;
            if (currentStep == 2) {
                if ($("#" + window.usuarioConfig.idForm + " #Usuario_RolIdFld").val() == 1) {
                    $("#" + window.usuarioConfig.idForm + " #collapseSucursales .form-check-input[type=\"checkbox\"]").prop('checked', true);
                } else {
                    $("#" + window.usuarioConfig.idForm + " #collapseSucursales .form-check-input[type=\"checkbox\"]").prop('checked', false);
                }
            }
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

$("#" + window.usuarioConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.usuarioConfig.idForm).valid()) {
        Matvi.confirm(window.usuarioConfig.title, "¿Confirma que desea guardar la información del usuario?", function (resp) {
            if (resp == true) {
                $("#" + window.usuarioConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Create(response) {
    Matvi.alert(response.messageType, window.usuarioConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion