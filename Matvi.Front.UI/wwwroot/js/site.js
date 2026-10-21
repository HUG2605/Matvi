
// #region Validación de formulario

// #endregion

// #region Ajax Setup
$(function () {

    $.ajaxSetup({
        //- Si una petición Ajax no recibe respuesta del servidor en 60 segundos, jQuery la aborta automáticamente.
        //- Esto provoca una desconexión TCP que ASP.NET Core detecta.
        //- Como consecuencia, se activa el CancellationToken en el servidor, cancelando la operación en curso.
        timeout: 60000 //Time in milliseconds
    });

    $(document).ajaxSend(function (event, jqXHR, settings) {
        if (settings.skipBlockUI !== true) {
            MV_BlockUI();
        }
    });

    $(document).ajaxStart(function () {

    });

    $(document).ajaxComplete(function (event, jqXHR, settings) {
        if (settings.skipBlockUI !== true) {
            MV_UnblockUI();
        }
    });

    $(document).ajaxError(function (event, jqXHR, settings, error) {
        /*$('input[type="submit"], button[type="submit"], .btn-true').prop('disabled', false);*/

        //alert("Hola");
        debugger;

        if (jqXHR.status != 401) {
            try {

                if (jqXHR.responseJSON.errorCode) {
                    var message = "Error: " + jqXHR.responseJSON.errorCode + "<br/>" + jqXHR.responseJSON.errorCodeDescription;
                    MV_Alert(MV_alertTypeEnum.Danger, "Error", message);
                }
                else {
                    var message = jqXHR.responseJSON.errorCodeDescription;
                    MV_Alert(MV_alertTypeEnum.Info, "Información", message);
                }
            }
            catch (exc) {
                if (jqXHR.status == 0) {
                    MV_Alert(MV_alertTypeEnum.Info, "Conexión de internet.", "Al parecer hubo un problema de conexión de internet, porfavor verifique que la acción que intentó realizar ya este ejecutada, de lo contrario vuelva a intentarlo.", function () {
                        var href = $("#aHome").attr('href');
                        window.location.href = href;
                    });
                }
                else {
                    MV_Alert(MV_alertTypeEnum.Danger, jqXHR.status, jqXHR.responseText);
                }

            }
        }
        else {
            MV_Alert(MV_alertTypeEnum.Info, "Sesión finalizada", "Por tiempo de inactividad la sesión del usuario ha finalizado, si desea continuar trabajando es necesario volver a ingresar al sistema.", function () {
                window.location.reload();
            });
        }
    });
    
});

// #endregion

// #region Previene los dobles request por doble click

$(function () {
    // Flag para controlar el estado de envío
    let isSubmitting = false;

    // Intercepta todos los formularios que contengan un botón .mv-btn-primary
    $('form:has(.mv-btn-primary)').on('submit', function (e) {
        const $btn = $(this).find('.mv-btn-primary');

        if (isSubmitting || $btn.prop('disabled')) {
            e.preventDefault(); // Previene doble envío
            return false;
        }

        isSubmitting = true;
        $btn.prop('disabled', true); // Desactiva el botón visualmente
    });

    // Restablece el estado al finalizar cualquier Ajax
    $(document).ajaxComplete(function () {
        isSubmitting = false;
        $('.mv-btn-primary').prop('disabled', false);
    });
});

// #endregion

// #region ToLower

$(document).on("blur change", "input[type=text]:not(.NotUpper)", function () {
    $(this).val($(this).val().trim().toUpperCase());
});

$(document).on("blur change", "input[type=email], input[type=url]", function () {
    $(this).val($(this).val().trim().toUpperCase());
});

$(document).on("blur change", "textarea:not(.NotUpper)", function () {
    $(this).val($(this).val().trim().toUpperCase());
});

// #endregion

// #region Datatabkes

window.matviDataTablesConfig = {
    keys: true,
    stateSave: true,
    pagingType: "full_numbers",
    // Scroll vertical DataTable
    //scrollY: "350px",
    //scrollCollapse: true,
    //paging: false,

    language: {
        url: "/js/datatables/lang/es-MX.json",
    },
    drawCallback: function () {
        $(".dataTables_paginate > .pagination").addClass("pagination-rounded");
        $('.dataTables_filter input[type="search"]').addClass('search-icon');
    },
    initComplete: function () {
        $('.dataTables_length, .dataTables_filter').addClass('mt-1 mb-1');
        $(".dataTables_length select").removeClass("form-select-sm");
        $('.dataTables_filter input').removeClass('form-control-sm');
        $(".dataTables_length label").addClass("form-label");
    },
    createdRow: function (row, data, dataIndex) {
        $(row).addClass('align-middle');
    }
};

// #endregion

// #region MV_BlockUI

function MV_BlockUI(message) {
    if (message == null || message == '') {
        message = 'Procesando...';
    }

    var html = '' +
        '<div>' +
        '<div class="m-auto rounded-circle bg-secondary p-0 d-flex flex-wrap align-items-center" style="width:100px;height:100px;"> ' +
        '<div class="spinner-grow m-auto text-white"></div>' +
        '</div> ' +
        '<h4 class="mt-3 text-white">' + message + '</h4> ' +
        '</div>';

    $.blockUI({
        message: html,
        css: {
            'zIndex': 9999, // ✅ Más alto que el modal de Bootstrap (por defecto 1050)
            'border': '0',
            'background-color': 'transparent',
            'color': 'white'
        },
        overlayCSS: {
            zIndex: 9998 // ✅ Fondo del bloqueo
        }
    });
}

function MV_UnblockUI() {
    $.unblockUI();
}

// #endregion

// #region MV_Alert

var MV_alertTypeEnum = {
    Success: 1,
    Info: 2,
    Warning: 3,
    Danger: 4
};

function MV_Alert(alertType, title, contentMessage, callback) {

    var _alertType = "";
    var _alertIcon = "";
    var _alertBtnClass = "";

    switch (alertType) {
        case MV_alertTypeEnum.Success:
            _alertType = "success";
            _alertIcon = "mdi mdi-check-circle-outline";
            _alertBtnClass = "btn btn-success";
            break;
        case MV_alertTypeEnum.Info:
            _alertType = "info";
            _alertIcon = "mdi mdi-information-outline";
            _alertBtnClass = "btn btn-info";
            break;
        case MV_alertTypeEnum.Warning:
            _alertType = "warning";
            _alertIcon = "mdi mdi-alert-circle-outline";
            _alertBtnClass = "btn btn-warning";
            break;
        case MV_alertTypeEnum.Danger:
            _alertType = "danger";
            _alertIcon = "mdi mdi-close-circle-outline";
            _alertBtnClass = "btn btn-danger";
            break;
    }

    $.alert({
        theme: 'Modern',
        type: _alertType,
        icon: _alertIcon,
        title: title,
        content: contentMessage,
        buttons: {
            close: {
                text: '<i class="mdi mdi-close me-2"></i>Cerrar',
                keys: ['enter'],
                btnClass: _alertBtnClass,
                action: function () {
                    if (callback) {
                        callback();
                    }
                }
            }
        }
    });

}

function MV_Confirm(title, contentQuestion, callback) {

    $.confirm({
        theme: 'Modern',
        type: 'dark',
        icon: 'mdi mdi-help-circle-outline',
        title: title,
        content: contentQuestion,
        buttons: {
            confirm: {
                text: '<i class="mdi mdi-check-circle-outline me-2"></i>Si',
                keys: ['enter'],
                btnClass: 'btn mv-btn-primary',
                action: function () {
                    if (callback) {
                        callback(true);
                    }
                }
            },
            cancel: {
                text: '<i class="mdi mdi-close-circle-outline me-2"></i>No',
                keys: ['esc'],
                btnClass: 'btn mv-btn-secondary',
                action: function () {
                    if (callback) {
                        callback(false);
                    }
                }
            }
        }
    });

}
// #endregion

// #region MV_Formats
function MV_formatPhoneLink(data) {
    const digits = data?.replace(/\D/g, "");
    if (!digits || digits.length !== 10) return data;

    const formatted = `${digits.slice(0, 3)}-${digits.slice(3, 6)}-${digits.slice(6, 10)}`;

    return `
        <a href="tel:${digits}" style="">
            <i class="mdi mdi-phone" style="margin-right: 4px;"></i>${formatted}
        </a>`;
}

function MV_formatDateTime(data) {
    if (!data) return data;

    const date = new Date(data);
    if (isNaN(date.getTime())) return data;

    const pad = n => n.toString().padStart(2, "0");

    const formatted =
        `${pad(date.getDate())}/${pad(date.getMonth() + 1)}/${date.getFullYear()} ` +
        `${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`;

    return formatted;
}

function MV_formatMoneyMX(value) {
    if (value === null || value === undefined || value === "") return value;

    const num = Number(value);
    if (isNaN(num)) return value;

    return num.toLocaleString("es-MX", {
        style: "currency",
        currency: "MXN",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

function MV_formatMiles(valor) {
    return valor.toLocaleString('es-MX', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

// #endregion

// #region MV_To
function MV_toCamel(str) {
    return str.charAt(0).toLowerCase() + str.slice(1);
}

// #endregion

// #region MV_To

function MV_rowGet(row, key) {
    return row[MV_toCamel(key)] || '';
}

// #endregion

// #region select2

$(document).on('select2:open', function () {
    let input = document.querySelector('.select2-container--open .select2-search__field');
    if (input) {
        input.focus();
    }
});

function MV_InitializeSelect2(idSelect2Component, minInputLength, idDropdownParent, url) {
    $(idSelect2Component).select2({
        dropdownParent: $(idDropdownParent),
        width: '100%',
        theme: 'bootstrap-5',
        placeholder: 'BUSCAR',
        allowClear: true,
        minimumInputLength: minInputLength,
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: params => ({
                coincidencia: params.term
            }),
            processResults: data => ({
                results: data.map(x => ({
                    id: x.id,          // BaseDto.Id → id
                    text: x.nombre     // BaseDto.Nombre → text
                }))
            }),
            cache: true
        }
    });
}

function mv_SetInitialSelect2Value(selector) {
    const $select = $(selector);
    const selectedValue = $select.data('selected-value');
    const selectedText = $select.data('selected-text');

    if (selectedValue && selectedText) {
        const option = new Option(selectedText, selectedValue, true, true);
        $select.append(option).trigger('change');
    }
}

// #endregion




