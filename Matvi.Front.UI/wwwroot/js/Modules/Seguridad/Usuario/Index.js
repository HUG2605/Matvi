$(document).ready(function () {

    let urlBase = "/Seguridad/Usuario";

    if (!$.fn.DataTable.isDataTable("#usuario-datatable")) {

        $("#usuario-datatable").DataTable({

            ...window.matviDataTablesConfig,

            serverSide: true,
            processing: true,

            ajax: {
                type: "GET",
                url: `${urlBase}?handler=UsuariosJson`,
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
                { data: Matvi.toCamel("NombreCompleto") },
                {
                    data: Matvi.toCamel("Telefono"),
                    render: (data) => Matvi.formatPhoneLink(data)
                },
                { data: Matvi.toCamel("CtRol_Nombre") },
                { data: Matvi.toCamel("Usuario") },
                {
                    data: Matvi.toCamel("Activo"),
                    className: "text-center",
                    render: function (data) {
                        const activo = String(data).toLowerCase() === "true" || data === 1;
                        return activo
                            ? `<span class="badge bg-success">Activo</span>`
                            : `<span class="badge bg-danger">Inactivo</span>`;
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