$(document).ready(function () {

    let urlBase = "/Ventas/Pedido";

    if (!$.fn.DataTable.isDataTable("#pedido-datatable")) {

        const verSensibles = $("#pedido-datatable").data("verSensibles") === true;

        const columns = [
            { data: Matvi.toCamel("Folio") },
            {
                data: Matvi.toCamel("Fecha"),
                render: data => Matvi.formatDateTime(data)
            },
            { data: Matvi.toCamel("CtClie_Nombre") },
            {
                data: Matvi.toCamel("CtClie_Telefono"),
                render: data => Matvi.formatPhoneLink(data)
            },
            { data: Matvi.toCamel("CtCanaCapt_Nombre") },
            { data: Matvi.toCamel
                
                ("CtEstaPedi_Nombre") },
            {
                data: Matvi.toCamel("EntregaDomicilio"),
                className: "text-center",
                render: function (data) {
                    const aDomicilio = String(data).toLowerCase() === "true" || data === 1;
                    return aDomicilio
                        ? `<span class="badge bg-danger">Si</span>`
                        : `<span class="badge bg-success">No</span>`;
                }
            }
        ];

        if (verSensibles) {
            columns.push({
                data: Matvi.toCamel("Total"),
                className: "text-end",
                render: data => Matvi.formatMoneyMXN(data)
            });
        }

        columns.push({
            data: Matvi.toCamel("SafeId"),
            orderable: false,
            searchable: false,
            className: "text-center",
            render: function (data, type, row) {
                return `<a href="${urlBase}/Details?id=${data}" title="Ver detalles">
                            <i class="mdi mdi-file-document-outline"></i>
                        </a>`;
            }
        });

        $("#pedido-datatable").DataTable({

            ...window.matviDataTablesConfig,

            order: [[1, "desc"]],

            serverSide: true,
            processing: true,

            ajax: {
                type: "GET",
                url: `${urlBase}?handler=PedidosJson`,
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

            columns: columns
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