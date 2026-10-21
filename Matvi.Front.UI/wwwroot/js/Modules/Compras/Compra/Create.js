//#region MODAL

var modalElement = document.getElementById(window.compraCreateConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('shown.bs.modal', (event) => {
    new MVStepper('.mv-steps-vertical', {
        formSelector: '#' + window.compraCreateConfig.idForm,
        buttonSelectors: {
            previous: '#btnAnterior',
            next: '#btnSiguiente',
            submit: '#btnGuardar'
        },
        onStepChange: (stepIndex) => {
            var currentStep = stepIndex + 1;
            if (currentStep == 2) {
                actualizarEstadoPaso2();
                inicializarTablaProductos();
            }
            if (currentStep == 3) {
                recalcularTotal();
                cargarSaldoVencido();
            }
        }
    });
});

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

//#region SELECT2

$(document).ready(function () {
    Matvi.initSelect2(
        '#Compra_CtProv_IdFld',
        1,
        '#' + window.compraCreateConfig.idModal,
        '/Compras/Api/Proveedor/select'
    );
});

//#endregion

//#region CHANGE CtProv_IdFld

$("#Compra_CtProv_IdFld").on("select2:select select2:clear", function () {
    var idProveedor = $(this).val();

    limpiarProductos();

    if (!idProveedor) {
        actualizarEstadoPaso2();
        return;
    }

    var $slcProducto = $("#slcProducto");
    if ($slcProducto.hasClass("select2-hidden-accessible")) {
        $slcProducto.select2("destroy");
    }

    Matvi.initSelect2(
        '#slcProducto',
        1,
        '#' + window.compraCreateConfig.idModal,
        '/Productos/Api/Producto/select?idProveedor=' + idProveedor + '&paraCompra=true&paraVenta=false'
    );

    vincularEventoProducto();
    actualizarEstadoPaso2();
});

//#endregion

//#region CHANGE slcProducto

function vincularEventoProducto() {
    $("#slcProducto").off("select2:select select2:clear").on("select2:select select2:clear", function () {
        var idProducto = $(this).val();
        var $slcPresentacion = $("#slcPresentacionProducto");

        $slcPresentacion.empty().append('<option value="">CARGANDO...</option>');

        if (idProducto) {
            $.ajax({
                url: '/Productos/Api/PresentacionProducto/select',
                type: 'GET',
                data: { 'idProducto': idProducto, 'paraCompra': true, 'paraVenta': false },
                success: function (data) {
                    $slcPresentacion.empty().append('<option value="">SELECCIONAR</option>');
                    $.each(data, function (i, item) {
                        $slcPresentacion.append($('<option>', { value: item.id, text: item.nombre }));
                    });
                },
                error: function () {
                    $slcPresentacion.empty().append('<option value="">ERROR AL CARGAR</option>');
                }
            });
        } else {
            $slcPresentacion.empty().append('<option value="">SELECCIONAR</option>');
        }
    });
}

//#endregion

//#region CLICK btnAgregar

$("#btnAgregar").on("click", function () {
    let idPresentacionProducto = $("#slcPresentacionProducto").val();
    let cantidad = $("#txtCantidad").val();
    let costo = $("#txtCosto").val();
    let valido = true;

    if (!idPresentacionProducto) {
        setValidationError("slcPresentacionProducto", "idPresentacionProducto", "Seleccione una presentación");
        valido = false;
    } else {
        clearValidationError("slcPresentacionProducto", "idPresentacionProducto");
    }

    if (!cantidad || cantidad <= 0) {
        setValidationError("txtCantidad", "cantidad", "Ingrese una cantidad válida");
        valido = false;
    } else {
        clearValidationError("txtCantidad", "cantidad");
    }

    if (!costo || parseFloat(costo) < 0) {
        setValidationError("txtCosto", "costo", "Ingrese un costo válido");
        valido = false;
    } else {
        clearValidationError("txtCosto", "costo");
    }

    if (!valido) return;

    $.ajax({
        url: '/Compras/Compra/Create?handler=AgregarProductoCompra',
        type: 'POST',
        data: { idPresentacionProducto, cantidad, costo },
        headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
        beforeSend: function () {
            if ($.fn.DataTable.isDataTable('#tblProductos')) $('#tblProductos').DataTable().destroy();
            $("#btnAgregar").prop("disabled", true);
        },
        success: function (response) {
            if (typeof response === 'object' && response && response.success === false) {
                Matvi.alert(Matvi.alertType.Info, 'Información', response.message);
                return;
            }

            $("#ContainerProductos").html(response);

            let total = parseFloat($("#hdnTotalCompra").val()) || 0;
            $("#txtTotal").val(total.toLocaleString('es-MX', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));

            $("#slcProducto").val(null).trigger("change").trigger("select2:clear");
            $("#txtCantidad").val("");
            $("#txtCosto").val("");
            $("#slcProducto").select2("open");
        },
        complete: function () {
            $("#btnAgregar").prop("disabled", false);
            inicializarTablaProductos();
        }
    });
});

//#endregion

//#region GUARDAR

$("#btnGuardar").on("click", function () {
    if ($("#" + window.compraCreateConfig.idForm).valid()) {
        Matvi.confirm(window.compraCreateConfig.title, "¿Confirma que desea registrar la compra?", function (resp) {
            if (resp == true) {
                $("#" + window.compraCreateConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Create(response) {
    Matvi.alert(response.messageType, window.compraCreateConfig.title, response.message, function () {
        window.location.reload();
    });
}

//#endregion

//#region FUNCTIONS

function recalcularTotal() {
    let saldoVencido = parseFloat($("#txtSaldoVencido").val().replace(/,/g, '')) || 0;
    let totalPedido = parseFloat($("#hdnTotalCompra").val()) || 0;
    let total = saldoVencido + totalPedido;

    $("#txtTotalPedido").val(Matvi.formatMiles(totalPedido));
    $("#txtTotal").val(Matvi.formatMiles(total));
    $("#txtSaldoPendiente").val(Matvi.formatMiles(total));
}

function actualizarEstadoPaso2() {
    var hayProveedor = !!($("#Compra_CtProv_IdFld").val());

    $("#alertSinProveedor").toggleClass("d-none", hayProveedor);
    $("#txtHintProducto").toggleClass("d-none", !hayProveedor);

    $("#slcProducto").prop("disabled", !hayProveedor);
    $("#slcPresentacionProducto").prop("disabled", !hayProveedor);
    $("#txtCantidad").prop("disabled", !hayProveedor);
    $("#txtCosto").prop("disabled", !hayProveedor);
    $("#btnAgregar").prop("disabled", !hayProveedor);
}

function limpiarProductos() {
    var $slcProducto = $("#slcProducto");
    if ($slcProducto.hasClass("select2-hidden-accessible")) {
        $slcProducto.val(null).trigger("change");
    }
    $("#slcPresentacionProducto").empty().append('<option value="">SELECCIONAR</option>');
    $("#txtCantidad").val("");
    $("#txtCosto").val("");
    clearValidationError("slcPresentacionProducto", "idPresentacionProducto");
    clearValidationError("txtCantidad", "cantidad");
    clearValidationError("txtCosto", "costo");
}

function inicializarTablaProductos() {
    if (!$('#tblProductos').length) return;
    if ($.fn.DataTable.isDataTable('#tblProductos')) return;

    ajustarAnchoContenedorProductos();

    $('#tblProductos').DataTable({
        ...window.matviDataTablesConfig,
        responsive: true,
        paging: false,
        searching: false,
        info: false,
        ordering: false,
        language: { emptyTable: "Agrega productos a la compra" },
        columnDefs: [
            { targets: 2, className: "text-center" },
            { targets: 3, className: "td-money" },
            { targets: 4, className: "td-money" },
            { targets: 5, orderable: false }
        ]
    });
}

function cargarSaldoVencido() {
    var idProveedor = $("#Compra_CtProv_IdFld").val();

    if (!idProveedor) {
        $("#txtSaldoVencido").val("0.00");
        recalcularTotal();
        return;
    }

    $.ajax({
        url: '/Compras/Compra/Create?handler=SaldoVencido',
        type: 'GET',
        data: { idProveedor: idProveedor },
        success: function (saldo) {
            $("#txtSaldoVencido").val(Matvi.formatMiles(parseFloat(saldo) || 0));
            recalcularTotal();
        },
        error: function (xhr) {
            console.error("Error saldo vencido:", xhr.status, xhr.responseText);
            $("#txtSaldoVencido").val("0.00");
            recalcularTotal();
        }
    });
}

function setValidationError(fieldId, spanName, message) {
    let spanId = spanName.replace(/\./g, "_");
    $("#" + fieldId).addClass("input-validation-error").attr("aria-invalid", "true");
    $("#" + spanId + "-error").text(message);
    $("[data-valmsg-for='" + spanName + "']").removeClass("field-validation-valid").addClass("field-validation-error");
}

function clearValidationError(fieldId, spanName) {
    let spanId = spanName.replace(/\./g, "_");
    $("#" + fieldId).removeClass("input-validation-error").removeAttr("aria-invalid");
    $("#" + spanId + "-error").text("");
    $("[data-valmsg-for='" + spanName + "']").removeClass("field-validation-error").addClass("field-validation-valid");
}

function ajustarAnchoContenedorProductos() {
    let anchoModalContent = $(".modal-content").width();
    $("#ContainerProductos").css("max-width", (anchoModalContent - 110) + "px");
}

$(window).on('resize', function () {
    ajustarAnchoContenedorProductos();
    if ($.fn.DataTable.isDataTable('#tblProductos')) {
        $('#tblProductos').DataTable().responsive.recalc();
        $('#tblProductos').DataTable().columns.adjust();
    }
});

//#endregion

//#region INPUT PagoFld

$(document).on("input", "#Compra_PagoFld", function () {
    let total = parseFloat($("#txtTotal").val().replace(/,/g, '')) || 0;
    let pago = parseFloat($(this).val()) || 0;
    let saldoPendiente = pago >= total ? 0 : total - pago;

    let $pendienteInput = $("#txtSaldoPendiente");
    $pendienteInput.removeClass("border border-info");
    if (saldoPendiente > 0) $pendienteInput.addClass("border border-info");
    $("#txtSaldoPendiente").val(Matvi.formatMiles(saldoPendiente));
});

//#endregion