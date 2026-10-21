//#region DELETE

$('button[name="btnDelete"]').on("click", function () {
    Matvi.confirm(window.seccionDeleteConfig.title, "¿Confirma que desea eliminar la información de la sección?", function (resp) {
        if (resp == true) {
            $.ajax({
                url: `${window.seccionDeleteConfig.urlBase}/Details?handler=Delete`,
                data: { 'id': window.seccionDeleteConfig.safeId },
                type: "GET"
            }).done(function (response) {
                Matvi.alert(response.messageType, window.seccionDeleteConfig.title, response.message, function () {
                    window.location.href = window.seccionDeleteConfig.urlAnterior;
                });
            });
        }
    });
});

//#endregion