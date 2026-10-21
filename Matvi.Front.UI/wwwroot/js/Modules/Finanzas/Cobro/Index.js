$(document).ready(function () {

    let urlBase = "/Finanzas/Cobro";

    if (!$.fn.DataTable.isDataTable("#cobro-datatable")) {

        $("#cobro-datatable").DataTable({

            ...window.matviDataTablesConfig,

            serverSide: true,
            processing: true,

            ajax: {
                type: "GET",
                url: `${urlBase}?handler=CobrosJson`,
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
                    data: Matvi.toCamel("Folio"),
                    className: "text-start"
                },
                {
                    data: Matvi.toCamel("Fecha"),
                    render: data => Matvi.formatDateTime(data)
                },
                {
                    data: Matvi.toCamel("CtClie_Nombre"),
                    className: "text-start"
                },
                {
                    data: Matvi.toCamel("CtMediPago_Nombre"),
                    className: "text-start"
                },
                {
                    data: Matvi.toCamel("CtUsuaReci_Nombre"),
                    className: "text-start"
                },
                {
                    data: Matvi.toCamel("MontoTotal"),
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