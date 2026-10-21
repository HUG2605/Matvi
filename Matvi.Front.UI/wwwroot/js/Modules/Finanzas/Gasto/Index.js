$(document).ready(function () {

    let urlBase = "/Finanzas/Gasto";

    if (!$.fn.DataTable.isDataTable("#gasto-datatable")) {

        $("#gasto-datatable").DataTable({

            ...window.matviDataTablesConfig,

            serverSide: true,
            processing: true,

            ajax: {
                type: "GET",
                url: `${urlBase}?handler=GastosJson`,
                data: function (d) {
                    const order = d.order?.[0];
                    const columnIndex = order?.column;
                    const sortDir = order?.dir;
                    const sortBy = columnIndex != null
                        ? d.columns[columnIndex].data
                        : null;

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
                    data: Matvi.toCamel("Folio"),
                    className: "text-center"
                },
                {
                    data: Matvi.toCamel("FechaPago"),
                    className: "text-center",
                    render: function (data) {
                        if (!data) return "";
                        var fecha = new Date(data);
                        var dia = String(fecha.getDate()).padStart(2, '0');
                        var mes = String(fecha.getMonth() + 1).padStart(2, '0');
                        var anio = fecha.getFullYear();
                        return `${dia}/${mes}/${anio}`;
                    }
                },
                {
                    data: Matvi.toCamel("CtTipoGast_Nombre"),
                    className: "text-start"
                },
                {
                    data: Matvi.toCamel("Proveedor")
                },
                {
                    data: Matvi.toCamel("CtMedioPago_Nombre"),
                    className: "text-start"
                },
                {
                    data: Matvi.toCamel("Importe"),
                    className: "td-money text-end",
                    render: function (data) {
                        return `$ ${Matvi.formatMiles(data)}`;
                    }
                },
                {
                    data: Matvi.toCamel("SafeId"),
                    orderable: false,
                    searchable: false,
                    className: "text-center",
                    render: function (data) {
                        return `<a href="${urlBase}/Details?id=${data}" title="Ver detalles">
                                    <i class="mdi mdi-file-document-outline"></i>
                                </a>`;
                    }
                }
            ]
        });
    }
});