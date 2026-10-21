$(document).ready(function () {
    let urlBase = "/Finanzas/MovimientoBancario";

    if (!$.fn.DataTable.isDataTable("#movimiento-bancario-datatable")) {
        $("#movimiento-bancario-datatable").DataTable({
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
                {
                    data: Matvi.toCamel("Fecha"),
                    render: data => Matvi.formatDateTime(data)
                },
                {
                    data: Matvi.toCamel("Banco"),
                    render: function (data, type, row) {
                        const cuenta = row[Matvi.toCamel("NumeroCuenta")] || "";
                        return `${data} (${cuenta})`;
                    }
                },
                {
                    data: Matvi.toCamel("TipoDescripcion"),
                    className: "text-start",
                    render: function (data, type, row) {
                        const esIngreso = row[Matvi.toCamel("Tipo")] === 1;
                        return esIngreso
                            ? `<span class="badge bg-success">Ingreso</span>`
                            : `<span class="badge bg-danger">Egreso</span>`;
                    }
                },
                {
                    data: Matvi.toCamel("OrigenTipo"),
                    className: "text-start",
                    render: function (data, type, row) {
                        const folio = row[Matvi.toCamel("OrigenFolio")] || "";
                        return folio ? `${data} (${folio})` : data;
                    }
                },
                {
                    data: Matvi.toCamel("OrigenDescripcion")
                },
                {
                    data: Matvi.toCamel("Referencia"),
                    render: data => data || ""
                },
                {
                    data: Matvi.toCamel("Monto"),
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
