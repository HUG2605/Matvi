//#region DELETE

$('button[name="btnDelete"]').on("click", function () {
    Matvi.confirm(window.proveedorDeleteConfig.title, "¿Confirma que desea eliminar la información del proveedor?", function (resp) {
        if (resp == true) {
            $.ajax({
                url: `${window.proveedorDeleteConfig.urlBase}/Details?handler=Delete`,
                data: { 'id': window.proveedorDeleteConfig.safeId },
                type: "GET"
            }).done(function (response) {
                Matvi.alert(response.messageType, window.proveedorDeleteConfig.title, response.message, function () {
                    window.location.href = window.proveedorDeleteConfig.urlAnterior;
                });
            });
        }
    });
});

//#endregion