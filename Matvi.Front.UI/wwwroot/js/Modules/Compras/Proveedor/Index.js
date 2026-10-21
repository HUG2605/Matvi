$(document).ready(function () {

    let urlBase = "/Compras/Proveedor";

    if (!$.fn.DataTable.isDataTable("#proveedor-datatable")) {

        $("#proveedor-datatable").DataTable({

            ...window.matviDataTablesConfig,

            serverSide: true,
            processing: true,

            ajax: {
                type: "GET",
                url: `${urlBase}?handler=ProveedoresJson`,
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
                    data: Matvi.toCamel("Nombre"),
                    render: function (data, type, row) {
                        return `<span class="d-block">${data}</span>
                        <small class="text-muted">${Matvi.rowGet(row, "Direccion")}</small>`;
                    }
                },
                {
                    data: Matvi.toCamel("Telefono"),
                    render: data => Matvi.formatPhoneLink(data)
                },
                {
                    data: Matvi.toCamel("TelefonoContacto"),
                    render: function (data, type, row) {
                        const nombre = Matvi.rowGet(row, "NombreContacto");
                        const tel = Matvi.formatPhoneLink(data);
                        return nombre
                            ? `${tel}<br><small class="text-muted">${nombre}</small>`
                            : tel;
                    }
                },
                {
                    data: Matvi.toCamel("SafeId"),
                    orderable: false,
                    searchable: false,
                    className: "text-center",
                    render: function (data, type, row) {
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