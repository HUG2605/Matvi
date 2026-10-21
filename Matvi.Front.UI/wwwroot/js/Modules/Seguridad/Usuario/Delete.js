//#region DELETE

$('button[name="btnDelete"]').on("click", function () {
    Matvi.confirm(window.usuarioDeleteConfig.title, "¿Confirma que desea eliminar la información del usuario?", function (resp) {
        if (resp == true) {
            $.ajax({
                url: `${window.usuarioDeleteConfig.urlBase}/Details?handler=Delete`,
                data: { 'id': window.usuarioDeleteConfig.safeId },
                type: "GET"
            }).done(function (response) {
                Matvi.alert(response.messageType, window.usuarioDeleteConfig.title, response.message, function () {
                    window.location.href = window.usuarioDeleteConfig.urlAnterior;
                });
            });
        }
    });
});

//#endregion