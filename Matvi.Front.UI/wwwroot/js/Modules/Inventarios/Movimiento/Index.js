$(document).ready(function () {

    let urlBase = "/Inventarios/Movimiento";

    if (!$.fn.DataTable.isDataTable("#movimiento-datatable")) {

        $("#movimiento-datatable").DataTable({

            ...window.matviDataTablesConfig,

            serverSide: true,
            processing: true,

            ajax: {
                type: "GET",
                url: `${urlBase}?handler=MovimientosJson`,
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
                { data: Matvi.toCamel("Folio"), className: "text-start text-nowrap" },
                {
                    data: Matvi.toCamel("Fecha"),
                    className: "text-nowrap",
                    render: data => Matvi.formatDateTime(data)
                },
                { data: Matvi.toCamel("CtTipoMovimiento_Nombre"), className: "text-start text-nowrap" },
                { data: Matvi.toCamel("CtProd_NombreCompleto"), className: "text-start" },
                { data: Matvi.toCamel("CtPresProd_Nombre"), className: "text-start text-nowrap" },
                { data: Matvi.toCamel("Cantidad"), className: "text-center text-nowrap" },
                {
                    data: Matvi.toCamel("SafeId"),
                    orderable: false,
                    searchable: false,
                    className: "text-center text-nowrap",
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