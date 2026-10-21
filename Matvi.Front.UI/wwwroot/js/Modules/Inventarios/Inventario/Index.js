$(document).ready(function () {

    let urlBase = "/Inventarios/Inventario";

    if (!$.fn.DataTable.isDataTable("#inventario-datatable")) {

        $("#inventario-datatable").DataTable({

            ...window.matviDataTablesConfig,

            serverSide: true,
            processing: true,

            ajax: {
                type: "GET",
                url: `${urlBase}?handler=InventariosJson`,
                data: function (d) {
                    const order = d.order?.[0];
                    const columnIndex = order?.column;
                    const sortDir = order?.dir;
                    const sortBy = columnIndex != null ? d.columns[columnIndex].data : null;

                    return {
                        draw: d.draw,
                        start: d.start,
                        length: d.length > 0 ? d.length : 10,
                        search: d.search?.value || "",
                        sortBy: sortBy,
                        sortDesc: sortDir === "desc"
                    };
                },
                skipBlockUI: true
            },

            columns: [
                {
                    data: Matvi.toCamel("CtProd_NombreCompleto"),
                    render: function (data, type, row) {
                        return `<span class="d-block">${data}</span>
                        <small class="text-muted">${Matvi.rowGet(row, "CtSecc_Nombre")}</small> :
                        <small class="text-muted">${Matvi.rowGet(row, "CtGrup_Nombre")}</small>`;
                    }
                },
                {
                    data: Matvi.toCamel("StockMinimoUBI"),
                    className: "text-center text-nowrap",
                    render: function (data, type, row) {
                        return `${data} ${Matvi.rowGet(row, "CtUnidBaseInve_Abreviatura")}`;
                    }
                },
                {
                    data: Matvi.toCamel("ExistenciasUBI"),
                    className: "text-center text-nowrap",
                    render: function (data, type, row) {
                        const existencias = parseFloat(data);
                        const minimo = parseFloat(Matvi.rowGet(row, "StockMinimoUBI"));
                        const maximo = parseFloat(Matvi.rowGet(row, "StockMaximoUBI"));

                        let clase = "";
                        if (existencias < minimo)
                            clase = "text-danger";
                        else if (existencias > maximo)
                            clase = "text-warning";
                        else
                            clase = "text-success";

                        return `<span class="${clase}">${data} ${Matvi.rowGet(row, "CtUnidBaseInve_Abreviatura")}</span>`;
                    }
                },
                {
                    data: Matvi.toCamel("StockMaximoUBI"),
                    className: "text-center text-nowrap",
                    render: function (data, type, row) {
                        return `${data} ${Matvi.rowGet(row, "CtUnidBaseInve_Abreviatura")}`;
                    }
                },
                {
                    data: Matvi.toCamel("Regularizado"),
                    className: "text-center text-nowrap",
                    render: function (data) {
                        const regularizado = String(data).toLowerCase() === "true" || data === 1;
                        return regularizado
                            ? `<span class="badge bg-success">Sí</span>`
                            : `<span class="badge bg-danger">No</span>`;
                    }
                },
                {
                    data: Matvi.toCamel("CtProd_SafeId"),
                    className: "text-center text-nowrap",
                    orderable: false,
                    searchable: false,
                    render: function (data) {
                        return `<a href="${urlBase}/Details?productoId=${data}" title="Ver detalles">
                                    <i class="mdi mdi-file-document-outline"></i>
                                </a>`;
                    }
                }
            ]
        });
    }
});