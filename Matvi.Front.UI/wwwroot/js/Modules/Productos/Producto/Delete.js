//#region DELETE

$('button[name="btnDelete"]').on("click", function () {
    Matvi.confirm(window.productoDeleteConfig.title, "¿Confirma que desea eliminar la información del producto?", function (resp) {
        if (resp == true) {
            $.ajax({
                url: `${window.productoDeleteConfig.urlBase}/Details?handler=Delete`,
                data: { 'id': window.productoDeleteConfig.safeId },
                type: "GET"
            }).done(function (response) {
                Matvi.alert(response.messageType, window.productoDeleteConfig.title, response.message, function () {
                    window.location.href = window.productoDeleteConfig.urlAnterior;
                });
            });
        }
    });
});

//#endregion

//#region TAB PRESENTACIONES

$('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
    var target = $(e.target).attr("href");

    if (target === "#home-b3") {
        $("#home-b3").load("/Productos/PresentacionProducto?handler=List&productoId=" + window.productoDeleteConfig.safeId);
    }
    if (target === "#home-b4") {
        $("#home-b4").load("/Productos/ProductoSucursal/Details?productoId=" + window.productoDeleteConfig.safeId);
    }
});

//#endregion