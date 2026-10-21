//#region DELETE

$('button[name="btnDelete"]').on("click", function () {
    Matvi.confirm(window.gastoDeleteConfig.title, "¿Confirma que desea eliminar la información del gasto?", function (resp) {
        if (resp == true) {
            $.ajax({
                url: `${window.gastoDeleteConfig.urlBase}/Details?handler=Delete`,
                data: { 'id': window.gastoDeleteConfig.safeId },
                type: "GET"
            }).done(function (response) {
                Matvi.alert(response.messageType, window.gastoDeleteConfig.title, response.message, function () {
                    window.location.href = window.gastoDeleteConfig.urlAnterior;
                });
            });
        }
    });
});

//#endregion