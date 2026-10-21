$(document).ready(function () {

    let urlBase = "/Configuracion/Empresa";

    if (!$.fn.DataTable.isDataTable("#empresa-datatable")) {

        $("#empresa-datatable").DataTable({

            ...window.matviDataTablesConfig,

            ajax: {
                url: `${urlBase}?handler=EmpresasJson`,
                type: "GET",
                dataSrc: function (json) {
                    return json;
                }
            },

            columns: [
                { data: Matvi.toCamel("Id") },
                { data: Matvi.toCamel("Nombre") },
                {
                    data: Matvi.toCamel("Telefono"),
                    render: (data) => Matvi.formatPhoneLink(data)
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
    }
});