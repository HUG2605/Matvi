//#region DELETE

$('button[name="btnDelete"]').on("click", function () {
    Matvi.confirm(window.clienteDeleteConfig.title, "¿Confirma que desea eliminar la información del cliente?", function (resp) {
        if (resp == true) {
            $.ajax({
                url: `${window.clienteDeleteConfig.urlBase}/Details?handler=Delete`,
                data: {
                    'id': window.clienteDeleteConfig.safeId
                },
                type: "GET"
            }).done(function (response) {
                Matvi.alert(response.messageType, window.clienteDeleteConfig.title, response.message, function () {
                    window.location.href = window.clienteDeleteConfig.urlAnterior;
                });
            });
        }
    });
});

//#endregion