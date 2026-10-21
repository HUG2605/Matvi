//#region MODAL

var modalElement = document.getElementById(window.productoEditConfig.idModal);
var modal = new bootstrap.Modal(modalElement);
modal.show();

modalElement.addEventListener('shown.bs.modal', () => {
    new MVStepper('.mv-steps-vertical', {
        formSelector: '#' + window.productoEditConfig.idForm,
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

//#region READY

$(function () {
    $("#Producto_NombreFld").trigger("blur");
    setTimeout(() => { $("#Producto_CtGrup_IdFld").trigger("change"); }, 500);
    setTimeout(() => { $("#Producto_CodigoBarrasInternoFld").trigger("change"); }, 500);
});

//#endregion

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

                var idSeccion = $("#Producto_CtSecc_IdFld").attr("data-seccion");
                if (idSeccion) {
                    $seccionSelect.val(idSeccion).trigger("change");
                    $("#Producto_CtSecc_IdFld").removeAttr("data-seccion").removeData("seccion");
                }
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

                var idGrupo = $("#Producto_CtGrup_IdFld").attr("data-grupo");
                if (idGrupo) {
                    $grupoSelect.val(idGrupo);
                    $("#Producto_CtGrup_IdFld").removeAttr("data-grupo").removeData("grupo");
                }
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

//#region EDIT

$("#" + window.productoEditConfig.idForm + " .mv-btn-primary").on("click", function () {
    if ($("#" + window.productoEditConfig.idForm).valid()) {
        Matvi.confirm(window.productoEditConfig.title, "¿Confirma que desea guardar la información del producto?", function (resp) {
            if (resp == true) {
                $("#" + window.productoEditConfig.idForm).submit();
            }
        });
    }
});

//#endregion

//#region EXITO Y RECARGA

function Success_Edit(response) {
    Matvi.alert(response.messageType, window.productoEditConfig.title, response.message, function () {
        if (response.messageType == Matvi.alertType.Success) {
            window.location.reload();
        }
    });
}

//#endregion