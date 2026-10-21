$(document).ready(function () {

    let urlBase = "/Finanzas/CorteCaja";

    if (!$.fn.DataTable.isDataTable("#corte-datatable")) {

        $("#corte-datatable").DataTable({

            ...window.matviDataTablesConfig,

            order: [[1, "desc"]],

            serverSide: true,
            processing: true,

            ajax: {
                type: "GET",
                url: `${urlBase}?handler=CortesJson`,
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
                    data: Matvi.toCamel("FechaApertura"),
                    className: "text-start",
                    render: data => Matvi.formatDateTime(data)
                },
                {
                    data: Matvi.toCamel("FechaCierre"),
                    className: "text-start",
                    render: data => data ? Matvi.formatDateTime(data) : ""
                },
                {
                    data: Matvi.toCamel("CtUsua_Nombre"),
                    className: "text-start"
                },
                {
                    data: Matvi.toCamel("FondoInicial"),
                    className: "td-money text-end",
                    render: function (data) {
                        return `$ ${Matvi.formatMiles(data)}`;
                    }
                },
                {
                    data: Matvi.toCamel("Diferencia"),
                    className: "td-money text-end",
                    render: function (data) {
                        return data === null || data === undefined || data === "" ? "" : `$ ${Matvi.formatMiles(parseFloat(data))}`;
                    }
                },
                {
                    data: Matvi.toCamel("Activa"),
                    className: "text-center",
                    orderable: false,
                    searchable: false,
                    render: function (data) {
                        return data
                            ? `<span class="badge bg-success">ABIERTA</span>`
                            : `<span class="badge bg-secondary">CERRADA</span>`;
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

            if ($btn && $target.length && $target.find('#btnNuevo, #btnCerrarTop').length === 0) {
                $target.prepend($btn);
            }
        }, 500);
    }
});