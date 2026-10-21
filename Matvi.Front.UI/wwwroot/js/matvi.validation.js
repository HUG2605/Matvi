

// #region numericrange

$.validator.addMethod("numericrange", function (value, element, params) {
    if (!value) return true;
    const digitsOnly = value.replace(/\D/g, "");
    return digitsOnly.length >= params.min && digitsOnly.length <= params.max && /^\d+$/.test(value);
});

$.validator.unobtrusive.adapters.add("numericrange", ["min", "max"], function (options) {
    const displayName = options.element.getAttribute("data-val-numericrange-display") || options.element.name;
    const message = options.message.replace("{0}", displayName);

    options.rules["numericrange"] = {
        min: parseInt(options.params.min),
        max: parseInt(options.params.max)
    };
    options.messages["numericrange"] = message;
});

// #endregion



