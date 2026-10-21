//#region MODAL

var modalElement = document.getElementById(pedidoConfig.idModal);
var modal = bootstrap.Modal.getInstance(modalElement)
    ?? new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('shown.bs.modal', (event) => {

    new MVStepper('.mv-steps-vertical', { 
        formSelector: '#' + pedidoConfig.idForm,
        buttonSelectors: {
            previous: '#btnAnterior',
            next: '#btnSiguiente',
            submit: '#btnGuardar'
        },
        onStepChange: (stepIndex) => {
            var currentStep = stepIndex + 1;
            if (currentStep == 2) {
                initProductsTable();
            }
        }
    });

    // Regla: pago no puede exceder el total
    $.validator.addMethod("maxPago", function (value, element) {
        let total = parseFloat($("#txtTotal").val().replace(/,/g, '')) || 0;
        let pago = parseFloat(value) || 0;
        return pago <= total;
    }, "El monto no puede ser mayor al total del pedido");

    $("[name='Pedido.PagoFld']").rules("add", { maxPago: true });

    // Inicializar badges en estado neutro
    updateProductBadges(false, false);
});

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    if (modal) {
        modal.dispose();
        modal = null;
    }
});

//#endregion


//#region SELECT2

$(document).ready(function () {

    Matvi.initSelect2(
        '#Pedido_CtClie_IdFld',
        1,
        '#' + pedidoConfig.idModal,
        '/Ventas/Api/Cliente/select'
    );

    Matvi.initSelect2(
        '#slcProducto',
        1,
        '#' + pedidoConfig.idModal,
        '/Productos/Api/Producto/select'
    );
});

//#endregion


//#region PRODUCT BADGES

function updateProductBadges(regularizado, bajoPedido) {

    const $spnReg = $('#spnRegulado');
    const $spnBP = $('#spnBajoPedido');

    $spnReg
        .removeClass('bg-success-subtle text-success bg-secondary-subtle text-secondary')
        .addClass(regularizado
            ? 'bg-success-subtle text-success'
            : 'bg-secondary-subtle text-secondary');

    $spnBP
        .removeClass('bg-info-subtle text-info bg-secondary-subtle text-secondary')
        .addClass(bajoPedido
            ? 'bg-info-subtle text-info'
            : 'bg-secondary-subtle text-secondary');
}

//#endregion


//#region CHANGE slcProducto

$("#slcProducto").on("select2:select select2:clear", function () {

    var idProducto = $(this).val();
    var $slcPresentacion = $("#slcPresentacionProducto");

    $slcPresentacion.empty().append('<option value="">CARGANDO...</option>');

    if (idProducto) {

        $.ajax({
            url: '/Ventas/Pedido/Create?handler=ProductoSucursal',
            type: 'GET',
            data: { 'idProducto': idProducto },
            success: function (data) {
                updateProductBadges(data.regularizado, data.ctProd_BajoPedido);
            },
            error: function () {
                updateProductBadges(false, false);
            }
        });

        $.ajax({
            url: '/Productos/Api/PresentacionProducto/select',
            type: 'GET',
            data: {
                'idProducto': idProducto,
                'paraCompra': false,
                'paraVenta': true
            },
            success: function (data) {
                $slcPresentacion.empty().append('<option value="">SELECCIONAR</option>');
                $.each(data, function (i, item) {
                    $slcPresentacion.append($('<option>', {
                        value: item.id,
                        text: item.nombre
                    }));
                });
            },
            error: function () {
                $slcPresentacion.empty().append('<option value="">ERROR AL CARGAR</option>');
            }
        });

        $("#txtExistencias")
            .removeClass("bg-success-subtle text-success bg-danger-subtle text-danger")
            .val("");

    } else {
        $slcPresentacion.empty().append('<option value="">SELECCIONAR</option>');
        updateProductBadges(false, false);
    }
});

//#endregion


//#region CHANGE slcPresentacionProducto

$("#slcPresentacionProducto").on("change", function () {

    var idPresentacionProducto = $(this).val();

    if (idPresentacionProducto) {

        $.ajax({
            url: '/Ventas/Pedido/Create?handler=PresentacionProducto',
            type: 'GET',
            data: { 'idPresentacionProducto': idPresentacionProducto },
            success: function (data) {
                $("#txtExistencias").val(data.existenciasPresentacion);

                if (data.ctProd_BajoPedido === false) {
                    if (data.rlProdSucu_Regularizado === false) {
                        setStockColor(true);
                    } else {
                        setStockColor(data.existenciasPresentacion > 0);
                    }
                } else {
                    setStockColor(true);
                }
            },
            error: function () {
                $("#txtExistencias").val("0");
            }
        });

    } else {
        $("#txtExistencias").val("0");
    }
});

function setStockColor(procede) {
    if (procede) {
        $("#txtExistencias")
            .addClass("bg-success-subtle text-success")
            .removeClass("bg-danger-subtle text-danger");
    } else {
        $("#txtExistencias")
            .removeClass("bg-success-subtle text-success")
            .addClass("bg-danger-subtle text-danger");
    }
}

//#endregion


//#region CHANGE CtClie_IdFld

let ultimoClienteCreditoId = null;

function cargarEstadoCreditoCliente(idCliente) {
    if (!idCliente || idCliente === "1") {
        ultimoClienteCreditoId = null;
        $("#grpCreditoCliente").addClass("d-none");
        return;
    }

    if (ultimoClienteCreditoId === idCliente) {
        return;
    }
    ultimoClienteCreditoId = idCliente;

    $.ajax({
        type: "GET",
        url: `/Ventas/Pedido/Create?handler=EstadoCreditoCliente&idCliente=${idCliente}`,
        skipBlockUI: true,
        success: function (data) {
            if (!data) {
                $("#grpCreditoCliente").addClass("d-none");
                return;
            }

            $("#lblLimiteCredito").text(`$ ${Matvi.formatMiles(data.limiteCreditoSemanal)}`);
            $("#lblDeudaActual").text(`$ ${Matvi.formatMiles(data.deudaActual)}`);
            $("#lblCreditoDisponible").text(`$ ${Matvi.formatMiles(data.creditoDisponible)}`);

            const $badge = $("#badgeEstadoCredito");
            const $rowMotivo = $("#rowMotivoBloqueo");
            const $lblMotivo = $("#lblMotivoBloqueo");

            if (data.aptoParaCredito) {
                $badge
                    .removeClass("bg-danger bg-warning text-dark")
                    .addClass("bg-success")
                    .html('<i class="mdi mdi-check-circle-outline me-1"></i> Apto para crédito');
                $rowMotivo.addClass("d-none");
            } else {
                $badge
                    .removeClass("bg-success")
                    .addClass("bg-danger")
                    .html('<i class="mdi mdi-close-circle-outline me-1"></i> No apto para crédito');
                $lblMotivo.text(data.motivoEstado);
                $rowMotivo.removeClass("d-none");
            }

            $("#grpCreditoCliente").removeClass("d-none");
        },
        error: function () {
            ultimoClienteCreditoId = null;
            $("#grpCreditoCliente").addClass("d-none");
        }
    });
}

$("#Pedido_CtClie_IdFld").on("select2:select select2:clear change", function () {

    var idCliente = $(this).val();

    if (idCliente === "1") {
        $("#chkMostrador").prop("checked", true);
        $("#colNombreMostrador, #colTelefonoMostrador").removeClass("d-none");
        // Cliente mostrador -> canal presencial y BLOQUEADO.
        aplicarCanalCapturaMostrador(true);
        // Mostrador: pago = total, campo bloqueado.
        aplicarPagoMostrador(true);
        // Mostrador no puede ser apartado.
        $("#colApartado, #colFechaEntregaEstimada").addClass("d-none");
        $("#Pedido_EsApartadoFld").prop("checked", false);
        aplicarApartado(false);
        // Ocultar info de crédito para mostrador
        cargarEstadoCreditoCliente(null);
    } else {
        $("#chkMostrador").prop("checked", false);
        $("#colNombreMostrador, #colTelefonoMostrador").addClass("d-none");
        aplicarCanalCapturaMostrador(false);
        // Cliente normal: liberar campo de pago.
        aplicarPagoMostrador(false);
        // Cliente registrado puede elegir apartado.
        if (idCliente) {
            $("#colApartado").removeClass("d-none");
            cargarEstadoCreditoCliente(idCliente);
        } else {
            $("#colApartado, #colFechaEntregaEstimada").addClass("d-none");
            $("#Pedido_EsApartadoFld").prop("checked", false);
            aplicarApartado(false);
            cargarEstadoCreditoCliente(null);
        }
    }
});

//#endregion


//#region CHANGE chkMostrador

$("#chkMostrador").on("change", function () {

    const $cliente = $("#Pedido_CtClie_IdFld");

    if (this.checked) {
        if ($cliente.find('option[value="1"]').length === 0) {
            $cliente.append(new Option("MOSTRADOR", "1", false, false));
        }
        $cliente.val("1").trigger("change").trigger("select2:select");
        $("#colNombreMostrador, #colTelefonoMostrador").removeClass("d-none");
        aplicarCanalCapturaMostrador(true);
        // Mostrador: pago = total, campo bloqueado.
        aplicarPagoMostrador(true);
    } else {
        $cliente.val(null).trigger("change").trigger("select2:clear");
        $("#colNombreMostrador, #colTelefonoMostrador").addClass("d-none");
        aplicarCanalCapturaMostrador(false);
        // Cliente normal: liberar campo de pago.
        aplicarPagoMostrador(false);
    }
});

// Aplica el estado "Canal de Captura = presencial" cuando el cliente es
// mostrador. Como un `<select disabled>` no envía su valor al servidor,
// usamos un `<input type="hidden">` con el mismo `name` que actúa como
// espejo: si el select está disabled, ASP.NET recibe solo el valor del hidden.
function aplicarCanalCapturaMostrador(mostrador) {
    const $canalCaptura = $("#Pedido_CtCanaCapt_IdFld");
    const $mirror = $("#Pedido_CtCanaCapt_IdFld_Mirror");

    if (mostrador) {
        // Forzar valor 1 (presencial), bloquear el select visualmente y
        // sincronizar el hidden para que el valor viaje en el POST.
        $canalCaptura.val("1").prop("disabled", true).addClass("select-locked");
        $mirror.val("1");
    } else {
        $canalCaptura.prop("disabled", false).removeClass("select-locked");
        $mirror.val($canalCaptura.val() || "");
    }
}

// Mantener el hidden sincronizado con el select cuando el usuario cambia el
// valor (caso normal, no mostrador).
$("#Pedido_CtCanaCapt_IdFld").on("change", function () {
    if (!$(this).prop("disabled")) {
        $("#Pedido_CtCanaCapt_IdFld_Mirror").val($(this).val() || "");
    }
});

function aplicarPagoMostrador(mostrador) {
    const $pago = $("#Pedido_PagoFld");
    const total = parseFloat($("#txtTotal").val().replace(/,/g, '')) || 0;

    if (mostrador) {
        $pago.val(total.toFixed(2));
        $pago.prop("disabled", true).addClass("select-locked");
        $("#spnPagoError").text("");
        $pago.removeClass("is-invalid");
        $("#txtCambio").val(Matvi.formatMiles(0));
        $("#txtPendiente").val(Matvi.formatMiles(0));
    } else {
        $pago.prop("disabled", false).removeClass("select-locked");
    }
}

//#endregion


//#region CHANGE EsApartadoFld

$("#Pedido_EsApartadoFld").on("change", function () {
    aplicarApartado(this.checked);
});

function aplicarApartado(apartado) {
    const $pago = $("#Pedido_PagoFld");

    if (apartado) {
        // Apartado: pago editable.
        $pago.prop("disabled", false).removeClass("select-locked");
        $("#spnPagoError").text("");
        $pago.removeClass("is-invalid");
        $("#txtCambio").val(Matvi.formatMiles(0));
        $("#txtPendiente").val(Matvi.formatMiles(0));
        // Mostrar fecha de entrega estimada.
        $("#colFechaEntregaEstimada").removeClass("d-none");
        // Apartado: sin entrega a domicilio ni dirección, Notas a todo el ancho.
        $("#colEntregaDomicilio, #colDireccionEntrega").addClass("d-none");
        $("#Pedido_EntregaDomicilioFld").prop("checked", false);
        $("#Pedido_DireccionEntregaFld").prop("disabled", true).val("");
        $("#colNotas").removeClass("col-md-6").addClass("col-md-12");
        // Marcar entregas como N/A.
        $(".chkEntregadoMostrador").each(function () {
            $(this).prop("checked", false).prop("disabled", true)
                .after('<span class="badge bg-secondary lblNA">N/A</span>')
                .hide();
        });
    } else {
        // Restaurar fecha de entrega estimada.
        $("#colFechaEntregaEstimada").addClass("d-none");
        // Restaurar entrega a domicilio y dirección, Notas comparte ancho.
        $("#colEntregaDomicilio, #colDireccionEntrega").removeClass("d-none");
        $("#colNotas").removeClass("col-md-12").addClass("col-md-6");
        $("#Pedido_DireccionEntregaFld").prop("disabled", !$("#Pedido_EntregaDomicilioFld").is(":checked")).val("");
        // Restaurar entregas.
        $(".chkEntregadoMostrador").prop("disabled", false).show();
        $(".lblNA").remove();
    }
}

//#endregion


//#region CHANGE EntregaDomicilioFld

$("#Pedido_EntregaDomicilioFld").on("change", function () {
    let $textarea = $("#Pedido_DireccionEntregaFld");
    if ($(this).is(":checked")) {
        $textarea.prop("disabled", false).focus();
    } else {
        $textarea.prop("disabled", true).val("");
    }
});

//#endregion


//#region INPUT PagoFld

$(document).on("input", "#Pedido_PagoFld", function () {

    let total = parseFloat($("#txtTotal").val().replace(/,/g, '')) || 0;
    let cobro = parseFloat($(this).val().replace(/,/g, '')) || 0;

    if (cobro > total) {
        $("#spnPagoError").text("El monto no puede ser mayor al total del pedido.");
        $(this).addClass("is-invalid");
    } else {
        $("#spnPagoError").text("");
        $(this).removeClass("is-invalid");
    }

    let cambio = cobro >= total ? cobro - total : 0;
    let pendiente = cobro >= total ? 0 : total - cobro;

    $("#txtCambio").val(Matvi.formatMiles(cambio));
    $("#txtPendiente").val(Matvi.formatMiles(pendiente));

    $("#txtCambio").toggleClass("border border-success", cambio > 0);
    $("#txtPendiente").toggleClass("border border-info", pendiente > 0);
});

//#endregion


//#region CLICK btnAgregar

$("#btnAgregar").on("click", function () {

    let idPresentacionProducto = $("#slcPresentacionProducto").val();
    let cantidad = $("#txtCantidad").val();
    let valido = true;

    if (!idPresentacionProducto) {
        Matvi.setValidationError("slcPresentacionProducto", "idPresentacionProducto", "Seleccione una presentación");
        valido = false;
    } else {
        Matvi.clearValidationError("slcPresentacionProducto", "idPresentacionProducto");
    }

    if (!cantidad || cantidad <= 0) {
        Matvi.setValidationError("txtCantidad", "cantidad", "Ingrese una cantidad válida");
        valido = false;
    } else {
        Matvi.clearValidationError("txtCantidad", "cantidad");
    }

    if (!valido) return;

    $.ajax({
        url: '/Ventas/Pedido/Create?handler=AgregarProductoPedido',
        type: 'POST',
        data: {
            idPresentacionProducto: idPresentacionProducto,
            cantidad: cantidad
        },
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        beforeSend: function () {
            if ($.fn.DataTable.isDataTable('#tblProductos'))
                $('#tblProductos').DataTable().destroy();
            $("#btnAgregar").prop("disabled", true);
        },
        success: function (response) {
            if (response && response.messageType) {
                Matvi.alert(response.messageType, 'Validación de producto', response.message);
                return;
            }

            $("#ContainerProductos").html(response);

            let total = parseFloat($("#hdnTotalPedido").val());
            $("#txtTotal").val(Matvi.formatMiles(total));

            // Si es mostrador o apartado, sincronizar pago con el nuevo total.
            if ($("#chkMostrador").is(":checked")) {
                aplicarPagoMostrador(true);
            } else if ($("#Pedido_EsApartadoFld").is(":checked")) {
                // Re-aplicar N/A sobre los nuevos checkboxes.
                $(".chkEntregadoMostrador").each(function () {
                    $(this).prop("checked", false).prop("disabled", true)
                        .after('<span class="badge bg-secondary lblNA">N/A</span>')
                        .hide();
                });
            }

            $("#slcProducto").val(null).trigger("change").trigger("select2:clear");
            $("#txtCantidad").val("");
            $("#slcProducto").select2("open");
        },
        complete: function () {
            $("#btnAgregar").prop("disabled", false);
            initProductsTable();
        }
    });
});

//#endregion


//#region CLICK btnGuardar

$("#btnGuardar").on("click", function () {
    if ($("#" + pedidoConfig.idForm).valid()) {
        Matvi.confirm(pedidoConfig.confirmTitle, "¿Confirma que desea registrar el pedido?", function (resp) {
            if (resp == true)
                $("#" + pedidoConfig.idForm).submit();
        });
    }
});

//#endregion


//#region CALLBACKS

function successCreate(response) {
    Matvi.alert(response.messageType, pedidoConfig.confirmTitle, response.message, function () {
        if (response.messageType === Matvi.alertType.Success) {
            window.location.reload();
        }
    });
}

//#endregion


//#region FUNCTIONS

function initProductsTable() {
    if (!$('#tblProductos').length) return;
    if ($.fn.DataTable.isDataTable('#tblProductos')) return;

    adjustProductsContainerWidth();

    $('#tblProductos').DataTable({
        ...window.matviDataTablesConfig,
        responsive: true,
        paging: false,
        searching: false,
        info: false,
        ordering: false,
        language: {
            emptyTable: "Agrega productos al pedido"
        },
        columnDefs: [
            { targets: 2, className: "text-center" },
            { targets: 3, className: "td-money" },
            { targets: 4, className: "td-money" },
            { targets: 5, orderable: false },
            { targets: 6, orderable: false }
        ]
    });
}

function adjustProductsContainerWidth() {
    let anchoModalContent = $(".modal-content").width();
    $("#ContainerProductos").css("max-width", (anchoModalContent - 110) + "px");
}

$(window).on('resize', function () {
    adjustProductsContainerWidth();
    if ($.fn.DataTable.isDataTable('#tblProductos')) {
        $('#tblProductos').DataTable().responsive.recalc();
        $('#tblProductos').DataTable().columns.adjust();
    }
});

//#endregion