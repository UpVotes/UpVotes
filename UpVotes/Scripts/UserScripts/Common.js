$.fn.IsValidNumber = function (evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

$.ClearDropdown = function (DdlObject) {
    var OptionsDdlObject = DdlObject.options;
    for (var j = 1; j < OptionsDdlObject.length; j++) {
        OptionsDdlObject.remove(j);
        j--;
    }
}

$.EncryptString = function (str) {
    //str = str!="&#"
    if (str.indexOf("&#") != -1) {
        str = str.replace("&#", "%lthash%")
    }

    var index = str.indexOf("+"); //Added for QA Correction as on 10-Oct-2014 for Sales_PL_Unit_0452 by Ashok Kumar M

    while (index != -1) {

        str = str.replace("+", "%Plus%");

        index = str.indexOf("+");

    }
    return escape(encodeURI(str));
};

$.DecryptString = function (str) {
    //str = str!="&#"
    if (str.indexOf("%lthash%") != -1) {
        str = str.replace("%lthash%", "&#")
    }
    //return escape(encodeURI(str));
    return decodeURI(unescape(str));
};

$.ClearDropdown = function (DdlObject) {
    if (DdlObject[0].options !== undefined) {
        var OptionsDdlObject = DdlObject[0].options;
        for (var j = 1; j < OptionsDdlObject.length; j++) {
            OptionsDdlObject.remove(j);
            j--;
        }
    }
}

$.fn.LoadOptions = function (data, name, value) {
    if (data != undefined && data != null) {
        this.sel = $(this);
        for (i = 0; i < data.length; i++) {
            this.sel.append($("<option style='color:black;' title='" + $(data[i]).attr(name) + "' value='" + $(data[i]).attr(value) + "' >" + $(data[i]).attr(name) + "</option>"));
        }
    }
}