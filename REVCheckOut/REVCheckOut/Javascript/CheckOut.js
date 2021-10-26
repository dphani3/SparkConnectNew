
function pageLoad() {
    $get('txtCardNumber').focus();
}
function OnTextLoad(control, data) {
    control.value = data;
    control.style.color = "#BBBBBB";
}
function OnTextFocus(control, data) {
    if (control.value == data) {
        control.value = "";
        control.style.color = "#000000";
        control.select();
    }
}
function OnTextBlur(control, data) {
    if (control.value == "") {
        control.value = data;
        control.style.color = "#BBBBBB";
    }
    else {
        control.style.color = "#000000";
    }
}
function AllowNumbersOnly(e) {
    e = (e) ? e : (window.event) ? event : null;
    if (e) {
        var keyEvent = (e.charCode) ? e.charCode :
                        ((e.keyCode) ? e.keyCode :
                        ((e.which) ? e.which : 0));

        if ((keyEvent > 47 && keyEvent < 58)) {
            if (e.keyCode) {
                e.returnValue = true;
            }
            else {
                return true;
            }
        }
        else {
            if (e.keyCode) {
                e.returnValue = false;
            }
            else {
                return false;
            }
        }
    }
}
function MakeUpperCase(element) {
    element.value = element.value.toUpperCase();
}
function CheckAll() {
//    if ($get('txtExpiryMonth').value == "MM") {
//        $get('txtExpiryMonth').value = "";
//    }
//    if ($get('txtExpiryYear').value == "YYYY") {
//        $get('txtExpiryYear').value = "";
//    }

    HideValidationSummary('valSumCheckOut');
}

function HideValidationSummary(validationSummaryId) {

    var divValidationSummary = $get(validationSummaryId);

    if (divValidationSummary) {
        divValidationSummary.style.display = 'none';

        var validationElements = getElementsByClassName("validationCheck");
        if (validationElements.length > 0) {
            for (var i = 0; i < validationElements.length; i++) {
                validationElements[i].style.display = 'none';
            }
        }
    }
}

var getElementsByClassName = function(className, tag, elm) {
    if (document.getElementsByClassName) {
        getElementsByClassName = function(className, tag, elm) {
            elm = elm || document;
            var elements = elm.getElementsByClassName(className),
				nodeName = (tag) ? new RegExp("\\b" + tag + "\\b", "i") : null,
				returnElements = [],
				current;
            for (var i = 0, il = elements.length; i < il; i += 1) {
                current = elements[i];
                if (!nodeName || nodeName.test(current.nodeName)) {
                    returnElements.push(current);
                }
            }
            return returnElements;
        };
    }
    else if (document.evaluate) {
        getElementsByClassName = function(className, tag, elm) {
            tag = tag || "*";
            elm = elm || document;
            var classes = className.split(" "),
				classesToCheck = "",
				xhtmlNamespace = "http://www.w3.org/1999/xhtml",
				namespaceResolver = (document.documentElement.namespaceURI === xhtmlNamespace) ? xhtmlNamespace : null,
				returnElements = [],
				elements,
				node;
            for (var j = 0, jl = classes.length; j < jl; j += 1) {
                classesToCheck += "[contains(concat(' ', @class, ' '), ' " + classes[j] + " ')]";
            }
            try {
                elements = document.evaluate(".//" + tag + classesToCheck, elm, namespaceResolver, 0, null);
            }
            catch (e) {
                elements = document.evaluate(".//" + tag + classesToCheck, elm, null, 0, null);
            }
            while ((node = elements.iterateNext())) {
                returnElements.push(node);
            }
            return returnElements;
        };
    }
    else {
        getElementsByClassName = function(className, tag, elm) {
            tag = tag || "*";
            elm = elm || document;
            var classes = className.split(" "),
				classesToCheck = [],
				elements = (tag === "*" && elm.all) ? elm.all : elm.getElementsByTagName(tag),
				current,
				returnElements = [],
				match;
            for (var k = 0, kl = classes.length; k < kl; k += 1) {
                classesToCheck.push(new RegExp("(^|\\s)" + classes[k] + "(\\s|$)"));
            }
            for (var l = 0, ll = elements.length; l < ll; l += 1) {
                current = elements[l];
                match = false;
                for (var m = 0, ml = classesToCheck.length; m < ml; m += 1) {
                    match = classesToCheck[m].test(current.className);
                    if (!match) {
                        break;
                    }
                }
                if (match) {
                    returnElements.push(current);
                }
            }
            return returnElements;
        };
    }
    return getElementsByClassName(className, tag, elm);
};

/* --- This function used to validate credit card expiry date validation in the primary transaction modalpopup --- */
//function validCardExpiry(dValue, lblError) {

//    var lblErrormessage = document.getElementById(lblError);
//    lblErrormessage.style.display = 'none';

//    var result = false;

//    dValue = dValue.split('/');
//    var pattern = /^\d{2}$/;
//   
//    
//    if (dValue[0] < 1 || dValue[0] > 12)
//        result = true;

//    if (!pattern.test(dValue[0]) || !pattern.test(dValue[1]))
//        result = true;

////    if (dValue[2])
////        result = true;

//    if (result) {
//        alert('a');
//        lblErrormessage.style.display = '';
//        if (document.all) {
//            lblErrormessage.innerText = "Please enter a valid date in MM/YY format.";
//        }
//        else {
//            lblErrormessage.innerText = "Please enter a valid date in MM/YY format.";
//        }
//    }
//}


