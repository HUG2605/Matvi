$(document).ready(function () {
    let urlBase = "/Finanzas/MovimientoEfectivo";

    if (!$.fn.DataTable.isDataTable("#movimiento-efectivo-datatable")) {
        $("#movimiento-efectivo-datatable").DataTable({
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
                    className: "text-start"
                },
                {
                    data: Matvi.toCamel("OrigenFolio"),
                    className: "text-start"
                },
                {
                    data: Matvi.toCamel("OrigenDescripcion")
                },
                {
                    data: Matvi.toCamel("Cajero")
                },
                {
                    data: Matvi.toCamel("Monto"),
                    className: "td-money text-end",
                    render: function (data) {
                        return `$ ${Matvi.formatMiles(data)}`;
                    }
                },
                {
                    data: Matvi.toCamel("IdOpCorteCaja"),
                    className: "text-center",
                    render: function (data, type, row) {
                        const folio = row[Matvi.toCamel("CorteCajaFolio")];
                        const safeId = row[Matvi.toCamel("CorteCajaSafeId")];
                        if (data && folio) {
                            return `<a href="/Finanzas/CorteCaja/Details?id=${safeId}" title="Ver corte de caja">
                                        ${folio}
                                    </a>`;
                        }
                        return `<span class="text-muted">Sin corte</span>`;
                    }
                },
                {
                    data: Matvi.toCamel("SafeId"),
                    orderable: false,
                    searchable: false,
                    className: "text-center",
                    render: function (data, type, row) {
                        const origen = encodeURIComponent(row[Matvi.toCamel("OrigenTipo")] || "");
                        return `<a href="${urlBase}/Details?origen=${origen}&id=${data}" title="Ver detalles">
                                    <i class="mdi mdi-file-document-outline"></i>
                                </a>`;
                    }
                }
            ]
        });
    }
});
