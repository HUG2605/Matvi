$(document).ready(function () {

    let urlBase = "/Compras/Compra";

    if (!$.fn.DataTable.isDataTable("#compra-datatable")) {

        $("#compra-datatable").DataTable({

            ...window.matviDataTablesConfig,

            serverSide: true,
            processing: true,

            ajax: {
                type: "GET",
                url: `${urlBase}?handler=ComprasJson`,
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
                { data: Matvi.toCamel("Folio"), className: "text-start" },
                {
                    data: Matvi.toCamel("Fecha"),
                    render: data => Matvi.formatDateTime(data)
                },
                { data: Matvi.toCamel("CtProv_Nombre") },
                {
                    data: null,
                    className: "text-center",
                    render: function (data, type, row) {
                        let total = Matvi.rowGet(row, "Total") || 0;
                        let totalPagado = Matvi.rowGet(row, "TotalPagado") || 0;
                        let pagado = total > 0 && total === totalPagado;
                        return pagado
                            ? `<span class="badge bg-success">Pagado</span>`
                            : `<span class="badge bg-danger">Pendiente</span>`;
                    }
                },
                {
                    data: Matvi.toCamel("Total"),
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

        setTimeout(() => {
            const $btn = $('#contenedorBotonNuevo').children();
            const $target = $('.dataTables_length');

            if ($btn && $target.length && $target.find('#btnNuevo').length === 0) {
                $target.prepend($btn);
            }
        }, 500);
    }
});