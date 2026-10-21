//#region DELETE

$('button[name="btnDelete"]').on("click", function () {
    Matvi.confirm(window.grupoDeleteConfig.title, "¿Confirma que desea eliminar la información del grupo?", function (resp) {
        if (resp == true) {
            $.ajax({
                url: `${window.grupoDeleteConfig.urlBase}/Details?handler=Delete`,
                data: { 'id': window.grupoDeleteConfig.safeId },
                type: "GET"
            }).done(function (response) {
                Matvi.alert(response.messageType, window.grupoDeleteConfig.title, response.message, function () {
                    window.location.href = window.grupoDeleteConfig.urlAnterior;
                });
            });
        }
    });
});

//#endregion