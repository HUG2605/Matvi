$(document).ready(function () {
    const idCorte = $("#corte-id").val();
    const currentUrl = window.location.pathname;

    let cobrosDt = null;
    let gastosDt = null;
    let pagosDt = null;

    function initCobrosDt() {
        if (!$.fn.DataTable.isDataTable("#corte-cobros-datatable")) {
            cobrosDt = $("#corte-cobros-datatable").DataTable({
                ...window.matviDataTablesConfig,
                serverSide: true,
                processing: true,
                ajax: {
                    type: "GET",
                    url: `${currentUrl}?handler=CobrosJson&idCorte=${idCorte}`,
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
                        data: Matvi.toCamel("Fecha"),
                        render: data => Matvi.formatDateTime(data)
                    },
                    {
                        data: Matvi.toCamel("CtClie_Nombre"),
                        className: "text-start"
                    },
                    {
                        data: Matvi.toCamel("CtMediPago_Nombre"),
                        className: "text-start"
                    },
                    {
                        data: Matvi.toCamel("EsBancario"),
                        className: "text-center",
                        render: function (data) {
                            return data
                                ? '<span class="badge bg-info-subtle text-info">Bancario</span>'
                                : '<span class="badge bg-success-subtle text-success">Efectivo</span>';
                        }
                    },
                    {
                        data: Matvi.toCamel("MontoTotal"),
                        className: "td-money text-end",
                        render: function (data) {
                            return `$ ${Matvi.formatMiles(data)}`;
                        }
                    }
                ]
            });
        }
    }

    function initGastosDt() {
        if (!$.fn.DataTable.isDataTable("#corte-gastos-datatable")) {
            gastosDt = $("#corte-gastos-datatable").DataTable({
                ...window.matviDataTablesConfig,
                serverSide: true,
                processing: true,
                ajax: {
                    type: "GET",
                    url: `${currentUrl}?handler=GastosJson&idCorte=${idCorte}`,
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
                        data: Matvi.toCamel("FechaPago"),
                        render: data => Matvi.formatDateTime(data)
                    },
                    {
                        data: Matvi.toCamel("CtTipoGast_Nombre"),
                        className: "text-start"
                    },
                    {
                        data: Matvi.toCamel("Proveedor"),
                        className: "text-start"
                    },
                    {
                        data: Matvi.toCamel("Concepto"),
                        className: "text-start"
                    },
                    {
                        data: Matvi.toCamel("CtMedioPago_Nombre"),
                        className: "text-start"
                    },
                    {
                        data: Matvi.toCamel("EsBancario"),
                        className: "text-center",
                        render: function (data) {
                            return data
                                ? '<span class="badge bg-info-subtle text-info">Bancario</span>'
                                : '<span class="badge bg-success-subtle text-success">Efectivo</span>';
                        }
                    },
                    {
                        data: Matvi.toCamel("Importe"),
                        className: "td-money text-end",
                        render: function (data) {
                            return `$ ${Matvi.formatMiles(data)}`;
                        }
                    }
                ]
            });
        }
    }

    function initPagosDt() {
        if (!$.fn.DataTable.isDataTable("#corte-pagos-datatable")) {
            pagosDt = $("#corte-pagos-datatable").DataTable({
                ...window.matviDataTablesConfig,
                serverSide: true,
                processing: true,
                ajax: {
                    type: "GET",
                    url: `${currentUrl}?handler=PagosJson&idCorte=${idCorte}`,
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
                        data: Matvi.toCamel("Fecha"),
                        render: data => Matvi.formatDateTime(data)
                    },
                    {
                        data: Matvi.toCamel("CtProv_Nombre"),
                        className: "text-start"
                    },
                    {
                        data: Matvi.toCamel("CtMediPago_Nombre"),
                        className: "text-start"
                    },
                    {
                        data: Matvi.toCamel("EsBancario"),
                        className: "text-center",
                        render: function (data) {
                            return data
                                ? '<span class="badge bg-info-subtle text-info">Bancario</span>'
                                : '<span class="badge bg-success-subtle text-success">Efectivo</span>';
                        }
                    },
                    {
                        data: Matvi.toCamel("MontoTotal"),
                        className: "td-money text-end",
                        render: function (data) {
                            return `$ ${Matvi.formatMiles(data)}`;
                        }
                    }
                ]
            });
        }
    }

    // Initialize when tabs are shown
    $('a[data-bs-toggle="tab"]').on("shown.bs.tab", function (e) {
        const target = $(e.target).attr("href");
        if (target === "#tab-movimientos") {
            initCobrosDt();
        } else if (target === "#subtab-cobros") {
            initCobrosDt();
        } else if (target === "#subtab-gastos") {
            initGastosDt();
        } else if (target === "#subtab-pagos") {
            initPagosDt();
        }
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    });
});
