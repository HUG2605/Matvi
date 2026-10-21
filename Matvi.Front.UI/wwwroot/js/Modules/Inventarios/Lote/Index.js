$(document).ready(function () {

    let urlBase = "/Inventarios/Lote";

    if (!$.fn.DataTable.isDataTable("#lote-datatable")) {

        $("#lote-datatable").DataTable({

            ...window.matviDataTablesConfig,

            serverSide: true,
            processing: true,

            ajax: {
                type: "GET",
                url: `${urlBase}?handler=LotesJson`,
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
                { data: Matvi.toCamel("CtTipoLote_Nombre"), className: "text-nowrap" },
                { data: Matvi.toCamel("CtProd_NombreCompleto") },
                { data: Matvi.toCamel("CtPresProd_Nombre"), className: "text-nowrap" },
                {
                    data: Matvi.toCamel("CtPresProd_FactorUBI"),
                    className: "text-end text-nowrap",
                    render: function (data, type, row) {
                        return `${data} ${Matvi.rowGet(row, "CtUnidBaseInve_Abreviatura")}`;
                    }
                },
                {
                    data: Matvi.toCamel("CantidadInicialUBI"),
                    className: "text-end text-nowrap",
                    render: function (data, type, row) {
                        return `${data} ${Matvi.rowGet(row, "CtUnidBaseInve_Abreviatura")}`;
                    }
                },
                {
                    data: Matvi.toCamel("CantidadDisponibleUBI"),
                    className: "text-end text-nowrap",
                    render: function (data, type, row) {
                        return `${data} ${Matvi.rowGet(row, "CtUnidBaseInve_Abreviatura")}`;
                    }
                },
                {
                    data: Matvi.toCamel("SafeId"),
                    className: "text-center text-nowrap",
                    orderable: false,
                    searchable: false,
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