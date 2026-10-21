//#region DELETE

$('button[name="btnDelete"]').on("click", function () {
    Matvi.confirm(window.sucursalDeleteConfig.title, "¿Confirma que desea eliminar la información de la sucursal?", function (resp) {
        if (resp == true) {
            $.ajax({
                url: `${window.sucursalDeleteConfig.urlBase}/Details?handler=Delete`,
                data: { 'id': window.sucursalDeleteConfig.safeId },
                type: "GET"
            }).done(function (response) {
                Matvi.alert(response.messageType, window.sucursalDeleteConfig.title, response.message, function () {
                    window.location.href = window.sucursalDeleteConfig.urlAnterior;
                });
            });
        }
    });
});

//#endregion