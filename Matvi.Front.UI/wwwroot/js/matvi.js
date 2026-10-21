const Matvi = {

    // #region BlockUI
    blockUI(message = 'Procesando...') {
        const html = `
            <div>
                <div class="m-auto rounded-circle bg-secondary p-0 d-flex flex-wrap align-items-center" style="width:100px;height:100px;">
                    <div class="spinner-grow m-auto text-white"></div>
                </div>
                <h4 class="mt-3 text-white">${message}</h4>
            </div>`;

        $.blockUI({
            message: html,
            css: { 'zIndex': 9999, 'border': '0', 'background-color': 'transparent', 'color': 'white' },
            overlayCSS: { zIndex: 9998 }
        });
    },

    unblockUI() {
        $.unblockUI();
    },
    // #endregion

    // #region Alert
    alertType: {
        Success: 1,
        Info: 2,
        Warning: 3,
        Danger: 4
    },

    alert(alertType, title, contentMessage, callback) {
        const map = {
            1: { type: 'success', icon: 'mdi mdi-check-circle-outline', btnClass: 'btn btn-success' },
            2: { type: 'info', icon: 'mdi mdi-information-outline', btnClass: 'btn btn-info' },
            3: { type: 'warning', icon: 'mdi mdi-alert-circle-outline', btnClass: 'btn btn-warning' },
            4: { type: 'danger', icon: 'mdi mdi-close-circle-outline', btnClass: 'btn btn-danger' },
        };
        const cfg = map[alertType];

        $.alert({
            theme: 'Modern',
            type: cfg.type,
            icon: cfg.icon,
            title,
            content: contentMessage,
            buttons: {
                close: {
                    text: '<i class="mdi mdi-close me-2"></i>Cerrar',
                    keys: ['enter'],
                    btnClass: cfg.btnClass,
                    action: () => callback?.()
                }
            }
        });
    },

    confirm(title, contentQuestion, callback) {
        $.confirm({
            theme: 'Modern',
            type: 'dark',
            icon: 'mdi mdi-help-circle-outline',
            title,
            content: contentQuestion,
            buttons: {
                confirm: {
                    text: '<i class="mdi mdi-check-circle-outline me-2"></i>Si',
                    keys: ['enter'],
                    btnClass: 'btn mv-btn-primary',
                    action: () => callback?.(true)
                },
                cancel: {
                    text: '<i class="mdi mdi-close-circle-outline me-2"></i>No',
                    keys: ['esc'],
                    btnClass: 'btn mv-btn-secondary',
                    action: () => callback?.(false)
                }
            }
        });
    },
    // #endregion

    // #region Format
    formatPhoneLink(data) {
        const digits = data?.replace(/\D/g, '');
        if (!digits || digits.length !== 10) return data;
        const formatted = `${digits.slice(0, 3)}-${digits.slice(3, 6)}-${digits.slice(6, 10)}`;
        return `<a href="tel:${digits}"><i class="mdi mdi-phone" style="margin-right:4px;"></i>${formatted}</a>`;
    },

    formatDateTime(data) {
        if (!data) return data;
        const date = new Date(data);
        if (isNaN(date.getTime())) return data;
        const pad = n => n.toString().padStart(2, '0');
        return `${pad(date.getDate())}-${pad(date.getMonth() + 1)}-${date.getFullYear()} ${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`;
    },

    formatMoneyMXN(value) {
        if (value === null || value === undefined || value === '') return value;
        const num = Number(value);
        if (isNaN(num)) return value;
        return num.toLocaleString('es-MX', { style: 'currency', currency: 'MXN', minimumFractionDigits: 2, maximumFractionDigits: 2 });
    },

    formatMiles(value) {
        return value.toLocaleString('es-MX', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    },
    // #endregion

    // #region Utils
    toCamel(str) {
        return str.charAt(0).toLowerCase() + str.slice(1);
    },

    rowGet(row, key) {
        return row[this.toCamel(key)] || '';
    },
    // #endregion

    // #region Select2
    initSelect2(idComponent, minInputLength, idDropdownParent, url) {
        $(idComponent).select2({
            dropdownParent: $(idDropdownParent),
            width: '100%',
            theme: 'bootstrap-5',
            placeholder: 'BUSCAR',
            allowClear: true,
            minimumInputLength: minInputLength,
            ajax: {
                url,
                dataType: 'json',
                delay: 250,
                data: params => ({ coincidencia: params.term }),
                processResults: data => ({
                    results: data.map(x => ({ id: x.id, text: x.nombre }))
                }),
                cache: true
            }
        });
    },

    setInitialSelect2Value(selector) {
        const $select = $(selector);
        const selectedValue = $select.data('selected-value');
        const selectedText = $select.data('selected-text');
        if (selectedValue && selectedText) {
            $select.append(new Option(selectedText, selectedValue, true, true)).trigger('change');
        }
    },
    // #endregion

    // #region errors

    setValidationError(fieldId, spanName, message) {
        let spanId = spanName.replace(/\./g, "_");
        $("#" + fieldId).addClass("input-validation-error").attr("aria-invalid", "true");
        $("#" + spanId + "-error").text(message);
        $("[data-valmsg-for='" + spanName + "']")
            .removeClass("field-validation-valid")
            .addClass("field-validation-error");
    },

    clearValidationError(fieldId, spanName) {
         let spanId = spanName.replace(/\./g, "_");
         $("#" + fieldId).removeClass("input-validation-error").removeAttr("aria-invalid");
         $("#" + spanId + "-error").text("");
         $("[data-valmsg-for='" + spanName + "']")
                .removeClass("field-validation-error")
                .addClass("field-validation-valid");
    },

    // endregion

};