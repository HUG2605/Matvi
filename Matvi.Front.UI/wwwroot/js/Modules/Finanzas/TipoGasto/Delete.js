//#region DELETE

$('button[name="btnDelete"]').on("click", function () {
    Matvi.confirm(window.tipoGastoDeleteConfig.title, "¿Confirma que desea eliminar la información del tipo de gasto?", function (resp) {
        if (resp == true) {
            $.ajax({
                url: `${window.tipoGastoDeleteConfig.urlBase}/Details?handler=Delete`,
                data: { 'id': window.tipoGastoDeleteConfig.safeId },
                type: "GET"
            }).done(function (response) {
                Matvi.alert(response.messageType, window.tipoGastoDeleteConfig.title, response.message, function () {
                    window.location.href = window.tipoGastoDeleteConfig.urlAnterior;
                });
            });
        }
    });
});

//#endregion