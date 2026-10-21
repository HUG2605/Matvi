// matvi.events.js

// #region Ajax Setup
$(function () {
    $.ajaxSetup({ timeout: 60000 });

    $(document).ajaxSend((event, jqXHR, settings) => {
        if (settings.skipBlockUI !== true) Matvi.blockUI();
    });

    $(document).ajaxComplete((event, jqXHR, settings) => {
        if (settings.skipBlockUI !== true) Matvi.unblockUI();
    });

    $(document).ajaxError((event, jqXHR) => {
        if (jqXHR.status === 401) {
            Matvi.alert(Matvi.alertType.Info, 'Sesión finalizada', 'Por tiempo de inactividad...', () => window.location.reload());
            return;
        }
        try {
            const { errorCode, errorCodeDescription } = jqXHR.responseJSON;
            if (errorCode) {
                Matvi.alert(Matvi.alertType.Danger, 'Error', `Error: ${errorCode}<br/>${errorCodeDescription}`);
            } else {
                Matvi.alert(Matvi.alertType.Info, 'Información', errorCodeDescription);
            }
        } catch {
            if (jqXHR.status === 0) {
                Matvi.alert(Matvi.alertType.Info, 'Conexión de internet.', 'Al parecer hubo un problema de conexión, verifique que la acción se haya ejecutado o vuelva a intentarlo.', () => {
                    window.location.href = $('#aHome').attr('href');
                });
            } else {
                Matvi.alert(Matvi.alertType.Danger, jqXHR.status, jqXHR.responseText);
            }
        }
    });
});
// #endregion

// #region Previene doble request por doble click
$(function () {
    let isSubmitting = false;

    $('form:has(.mv-btn-primary)').on('submit', function (e) {
        const $btn = $(this).find('.mv-btn-primary');

        if (isSubmitting || $btn.prop('disabled')) {
            e.preventDefault();
            return false;
        }

        isSubmitting = true;
        $btn.prop('disabled', true);
    });

    $(document).ajaxComplete(function () {
        isSubmitting = false;
        $('.mv-btn-primary').prop('disabled', false);
    });
});
// #endregion

// #region ToUpper automático en inputs
$(document).on('blur change', 'input[type=text]:not(.NotUpper)', function () {
    $(this).val($(this).val().trim().toUpperCase());
});

$(document).on('blur change', 'input[type=email], input[type=url]', function () {
    $(this).val($(this).val().trim().toUpperCase());
});

$(document).on('blur change', 'textarea:not(.NotUpper)', function () {
    $(this).val($(this).val().trim().toUpperCase());
});
// #endregion

// #region Select2 — focus automático al abrir
$(document).on('select2:open', function () {
    const input = document.querySelector('.select2-container--open .select2-search__field');
    if (input) input.focus();
});
// #endregion

// #region DataTables — configuración global
window.matviDataTablesConfig = {
    keys: true,
    stateSave: true,
    pagingType: 'full_numbers',
    language: {
        url: '/js/datatables/lang/es-MX.json',
    },
    drawCallback() {
        $('.dataTables_paginate > .pagination').addClass('pagination-rounded');
        $('.dataTables_filter input[type="search"]').addClass('search-icon');
    },
    initComplete() {
        $('.dataTables_length, .dataTables_filter').addClass('mt-1 mb-1');
        $('.dataTables_length select').removeClass('form-select-sm');
        $('.dataTables_filter input').removeClass('form-control-sm');
        $('.dataTables_length label').addClass('form-label');
    },
    createdRow(row) {
        $(row).addClass('align-middle');
    }
};
// #endregion