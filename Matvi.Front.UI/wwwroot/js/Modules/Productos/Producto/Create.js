//#region POPOVERS

var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
    return new bootstrap.Popover(popoverTriggerEl);
});

//#endregion

//#region MODAL

var modalElement = document.getElementById(window.productoCreateConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('shown.bs.modal', () => {
    new MVStepper('.mv-steps-vertical', {
        formSelector: '#' + window.productoCreateConfig.idForm,
        buttonSelectors: {
            previous: '#btnAnterior',
            next: '#btnSiguiente',
            submit: '#btnGuardar'
        },
        onStepChange: (stepIndex) => { }
    });
});

modalElement.addEventListener('hidden.bs.modal', event => {
    $("#ContainerModal").html("");
    modal.dispose();
    modal = null;
});

//#endregion

$(function () {
    $("#colDescripcionGrupo").hide();
});

//#region CHANGE Producto_NombreFld

$("#Producto_NombreFld").on("blur", function () {
    var $grupoSelect = $("#Producto_CtGrup_IdFld");
    $grupoSelect.empty().append('<option value="">SELECCIONAR</option>');

    var descripcionGrupo = $(this).val();
    var $seccionSelect = $("#Producto_CtSecc_IdFld");

    $seccionSelect.empty().append('<option value="">CARGANDO...</option>');

    if (descripcionGrupo) {
        $.ajax({
            url: '/Productos/api/seccion/select',
            type: 'GET',
            data: { descripcionGrupo: descripcionGrupo },
            success: function (data) {
                $seccionSelect.empty().append('<option value="">SELECCIONAR</option>');
                $.each(data, function (i, item) {
                    var esCoincidente = item.nombre.startsWith("★");
                    $seccionSelect.append($('<option>', {
                        value: item.id,
                        text: item.nombre,
                        style: esCoincidente ? "color:#EC7016; font-weight:bold;" : ""
                    }));
                });
            },
            error: function () {
                $seccionSelect.empty().append('<option value="">ERROR AL CARGAR</option>');
            }
        });
    } else {
        $seccionSelect.empty().append('<option value="">SELECCIONAR</option>');
        $("#colDescripcionGrupo").hide();
    }
});

//#endregion

//#region CHANGE Producto_CtSecc_IdFld

$("#Producto_CtSecc_IdFld").on("change", function () {
    var idSeccion = $(this).val();
    var $grupoSelect = $("#Producto_CtGrup_IdFld");

    $grupoSelect.empty().append('<option value="">CARGANDO...</option>');

    if (idSeccion) {
        var descripcion = $("#Producto_NombreFld").val();
        $.ajax({
            url: '/Productos/api/grupo/select',
            type: 'GET',
            data: { 'idSeccion': idSeccion, 'descripcion': descripcion },
            success: function (data) {
                $grupoSelect.empty().append('<option value="">SELECCIONAR</option>');
                $.each(data, function (i, item) {
                    var esCoincidente = item.nombre.startsWith("★");
                    $grupoSelect.append($('<option>', {
                        value: item.id,
                        text: item.nombre,
                        style: esCoincidente ? "color:#EC7016;font-weight:bold" : ""
                    }));
                });
            },
            error: function () {
                $grupoSelect.empty().append('<option value="">ERROR AL CARGAR</option>');
            }
        });
    } else {
        $grupoSelect.empty().append('<option value="">SELECCIONAR</option>');
        $("#colDescripcionGrupo").hide();
    }
});

//#endregion

//#region CHANGE Producto_CtGrup_IdFld

$("#Producto_CtGrup_IdFld").on("change", function () {
    var idGrupo = $(this).val();
    if (idGrupo) {
        $.ajax({
            url: '/Productos/api/grupo/descripcion',
            type: 'GET',
            data: { 'idGrupo': idGrupo },
            success: function (data) {
                $("#colDescripcionGrupo").show();
                $("#colDescripcionGrupo output").text(data);
            },
            error: function () {
                $("#colDescripcionGrupo").hide();
            }
        });
    } else {
        $("#colDescripcionGrupo").hide();
    }
});

//#endregion

//#region CHANGE Producto_CodigoBarrasInternoFld

$("#Producto_CodigoBarrasInternoFld").on("change", function () {
    if ($(this).is(":checked")) {
        $("#Producto_CodigoBarrasFld").prop("disabled", true).val("");
    } else {
        $("#Producto_CodigoBarrasFld").prop("disabled", false);
    }
});

//#endregion

//#region CREATE

$("#" + window.productoCreateConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.productoCreateConfig.idForm).valid()) {

        var unidadMedidaSeleccionada = $("#Producto_CtUnidBaseInve_IdFld option:selected").text();
        var msg =
            "La unidad de medida base define cómo se controla el inventario de este producto.<br>" +
            "Una vez guardado, este dato no podrá modificarse.<br><br>" +
            "¿Confirma que desea crear el producto con la unidad de medida base de inventario <b>" + unidadMedidaSeleccionada + "</b>?";

        Matvi.confirm(window.productoCreateConfig.title, msg, function (resp) {
            if (resp === true) {
                $("#" + window.productoCreateConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Create(response) {
    Matvi.alert(response.messageType, window.productoCreateConfig.title, response.message, function () {
        if (response.messageType == Matvi.alertType.Success) {
            window.location.reload();
        }
    });
}

//#endregion