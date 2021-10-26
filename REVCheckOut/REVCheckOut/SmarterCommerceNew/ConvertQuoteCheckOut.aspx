<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConvertQuoteCheckOut.aspx.cs" Inherits="ConvertQuoteCheckOut"
ViewStateEncryptionMode="Always" EnableEventValidation="false" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cap" %>
<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <%--<meta charset="utf-8" />--%>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>REV</title>
    <!-- CSS -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:400italic,400" rel="stylesheet"
        type='text/css' />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="../StyleSheets/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../StyleSheets/style.css" />
    <!-- Favicon -->
    <link rel="shortcut icon" href="../Images/favicon.ico" type="image/x-icon" />
    <link rel="icon" href="../Images/favicon.ico" type="image/x-icon" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.javascripts/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <!-- Main Wrapper having header,footer and content -->
    <div class="main-wrapper">
        <form id="form1" runat="server" defaultbutton="btnPay" class="checkoutForm">
        <asp:ScriptManager ID="CheckOutManager" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/Javascript/jquery.min.js" />
                <asp:ScriptReference Path="~/Javascript/bootstrap.min.js" />
                <asp:ScriptReference Path="~/Javascript/jquery.payment.js" />
                <asp:ScriptReference Path="~/Javascript/jquery.creditCardValidator.js" />
                <asp:ScriptReference Path="~/Javascript/CheckOut.js" />
                <asp:ScriptReference Path="~/Javascript/jquery_003.js" />
                <asp:ScriptReference Path="~/Javascript/jquery.js" />
            </Scripts>
        </asp:ScriptManager>
        <script type="text/javascript">

            window.onload = window.history.forward(0);


            var date = new Date();
            var currentMonth = date.getMonth();
            var currentYear = date.getFullYear();

            var monthArray = new Array();
            monthArray[0] = "01";
            monthArray[1] = "02";
            monthArray[2] = "03";
            monthArray[3] = "04";
            monthArray[4] = "05";
            monthArray[5] = "06";
            monthArray[6] = "07";
            monthArray[7] = "08";
            monthArray[8] = "09";
            monthArray[9] = "10";
            monthArray[10] = "11";
            monthArray[11] = "12";

            jQuery(function ($) {
                $('[data-numeric]').payment('restrictNumeric');
                $('.cc-number').payment('formatCardNumber');
                $('.cc-exp').payment('formatCardExpiry');
                $('.cc-cvc').payment('formatCardCVC');

                $.fn.toggleInputError = function (erred) {
                    this.parent('.form-group').toggleClass('has-error', erred);
                    return this;
                };

                //            $('form').submit(function (e) {
                //            $("#btnPay").click(function () {


                //                var cardType = $.payment.cardType($('.cc-number').val());
                //                $('.cc-number').toggleInputError(!$.payment.validateCardNumber($('.cc-number').val()));
                //                $('.cc-exp').toggleInputError(!$.payment.validateCardExpiry($('.cc-exp').payment('cardExpiryVal')));
                //                $('.cc-cvc').toggleInputError(!$.payment.validateCardCVC($('.cc-cvc').val(), cardType));
                //                $('.cc-brand').text(cardType);

                //                $('.validation').removeClass('text-danger text-success');
                //                $('.validation').addClass($('.has-error').length ? 'text-danger' : 'text-success');

                //                if ($('.has-error').length != '0') {
                //                    alert('clicked');
                //                    return false;
                //                } else {
                //                    $('#btnPay').trigger('click');
                //                }
                //            });

                for (m = 0; m <= 11; m++) {
                    var optn = document.createElement("OPTION");
                    optn.text = monthArray[m];

                    optn.value = (m + 1);


                    if (m == currentMonth) {
                        optn.selected = true;
                    }

                    document.getElementById('txtExpiryMonth').options.add(optn);
                }
                //year
                for (var i = currentYear; i <= 2050; i++) {
                    var optn = document.createElement("OPTION");
                    optn.text = i;
                    optn.value = i;


                    if (i == currentYear) {
                        optn.selected = true;
                    }

                    document.getElementById('txtExpiryYear').options.add(optn);
                }



                $('input#txtCardNumber').validateCreditCard(function (result) {
                    var currentClass = $('.cardTypeIcon i').attr('class');
                    //console.log(currentClass);
                    $('.cardTypeIcon i').removeClass(currentClass).addClass(result.card_type == null ? 'default' : result.card_type.name);
                });

                $("#CheckOutPanel input").on('change', function () {
                    $(this).parents().removeClass('has-error');
                });

                $("input#txtCardNumber").on('change', function () {
                    $('#lblCardNoError').hide();
                });
                $("#txtExpiryMonth, #txtExpiryYear").on('change', function () {
                    $('#hdnMonth').val($('#txtExpiryMonth').val());
                    $('#hdnYear').val($('#txtExpiryYear').val());
                    $('#lblExpiryMonthYear').hide();
                });
                $("input#txtCvvNumber").on('change', function () {
                    $('#lblCvvNumber').hide();
                });
                $("input#txtCardName").on('change', function () {
                    $('#lblCardName').hide();
                });

            });

            function Validate() {

                var txtCardNumber = document.getElementById("<%=txtCardNumber.ClientID %>");
                var txtExpiryMonth = document.getElementById("<%=txtExpiryMonth.ClientID %>");
                var txtExpiryYear = document.getElementById("<%=txtExpiryYear.ClientID %>");
                var txtCvvNumber = document.getElementById("<%=txtCvvNumber.ClientID %>");
                var txtCardName = document.getElementById("<%=txtCardName.ClientID %>");
                var lblError = document.getElementById("<%=lblError.ClientID%>");
                var lblCardNoError = document.getElementById("<%=lblCardNoError.ClientID%>");
                var txtCaptcha = document.getElementById("<%=txtCaptcha.ClientID %>");
                var expiryMonth = parseInt($('#txtExpiryMonth').val());
                var expiryYear = parseInt($('#txtExpiryYear').val());

                $('#hdnMonth').val(expiryMonth);
                $('#hdnYear').val(expiryYear);
                $('#lblCardNoError, #lblExpiryMonthYear, #lblCvvNumber, #lblCardName, #lblCaptcha').hide();

                var status = true;

                if (txtCardNumber.value == '') {

                    $('.form-group').addClass('has-error');
                    $('#lblCardNoError').show().text('Enter card number.');
                    txtCardNumber.focus();
                    status = false;
                }
                else if (txtCardNumber.value != '') {
                    var cardNumber = txtCardNumber.value.replace(/\s/g, "");

                    $('input#txtCardNumber').validateCreditCard(function (result) {

                        var currentClass = $('.cardTypeIcon i').attr('class');
                        //console.log(currentClass);
                        $('.cardTypeIcon i').removeClass(currentClass).addClass(result.card_type == null ? 'default' : result.card_type.name);
                    });
                }
                else {
                    $('#txtCardNumber').removeClass('has-error');
                }


                if (parseInt($('#txtExpiryMonth').val()) == (currentMonth + 1) && parseInt($('#txtExpiryYear').val()) == currentYear) {
                    //                    $('#txtExpiryMonth').addClass('has-error');
                    //                    $('#txtExpiryYear').addClass('has-error');
                    //                    $('#lblExpiryMonthYear').show().text('Enter card expiry.');
                    //                    $('#txtExpiryMonth,#txtExpiryYear').css('color', '#555');
                    $('.cc-exp').payment('formatCardExpiry');
                    $('#txtExpiryMonth').parents('.form-group').removeClass('has-error');
                    $('#txtExpiryYear').parents('.form-group').removeClass('has-error');
                    $('#txtExpiryMonth,#txtExpiryYear').css('color', 'green');
                    status = true;
                }
                else {


                    if (expiryYear > currentYear) {
                        if (expiryMonth < 1 || expiryMonth > 12) {
                            $('#txtExpiryMonth').addClass('has-error');
                            $('#txtExpiryYear').addClass('has-error');
                            $('#lblExpiryMonthYear').show().text('Invalid Card Expiry.');
                            $('#txtExpiryMonth,#txtExpiryYear').css('color', '#555');
                            txtCaptcha.value = '';
                            status = false;
                        }
                        else {
                            $('.cc-exp').payment('formatCardExpiry');
                            $('#txtExpiryMonth').parents('.form-group').removeClass('has-error');
                            $('#txtExpiryYear').parents('.form-group').removeClass('has-error');
                            $('#txtExpiryMonth,#txtExpiryYear').css('color', 'green');
                        }
                    } else if (expiryYear < currentYear) {
                        $('#txtExpiryMonth').addClass('has-error');
                        $('#txtExpiryYear').addClass('has-error');
                        $('#lblExpiryMonthYear').show().text('Invalid Card Expiry.');
                        $('#txtExpiryMonth,#txtExpiryYear').css('color', '#555');
                        txtCaptcha.value = '';
                        status = false;
                    } else if (expiryYear == currentYear) {
                        if (expiryMonth < currentMonth) {

                            $('#txtExpiryMonth').addClass('has-error');
                            $('#txtExpiryYear').addClass('has-error');
                            $('#lblExpiryMonthYear').show().text('Invalid Card Expiry.');
                            $('#txtExpiryMonth,#txtExpiryYear').css('color', '#555');
                            txtCaptcha.value = '';
                            status = false;
                        }
                        else {

                            $('.cc-exp').payment('formatCardExpiry');
                            $('#txtExpiryMonth').parents('.form-group').removeClass('has-error');
                            $('#txtExpiryYear').parents('.form-group').removeClass('has-error');
                            $('#txtExpiryMonth,#txtExpiryYear').css('color', 'green');
                        }
                    }

                }

                //Date End ---


                if (txtCvvNumber.value == '') {
                    $('#txtCvvNumber').addClass('has-error');
                    $('#lblCvvNumber').show().text('Enter CVV number.');
                    status = false;
                }
                else if (txtCvvNumber.value.length < 3) {
                    $('#txtCvvNumber').addClass('has-error');
                    $('#lblCvvNumber').show().text('Invalid CVV number.');
                    txtCaptcha.value = '';
                    status = false;
                }
                else {
                    $('#txtCvvNumber').removeClass('has-error');
                }

                if (txtCardName.value == '') {

                    $('#txtCardName').addClass('has-error');
                    $('#lblCardName').show().text('Enter Name on Card.');
                    txtCaptcha.value = '';
                    status = false;
                }
                else {
                    var name = /^[A-z0-9.\- ]+$/;
                    var entcardname = txtCardName.value;
                    if (!name.test(entcardname)) {
                        status = false;
                        $('#lblCardName').show().text('Name on Card Cannot have special characters and hypehns(-).');
                    }
                }

                if (txtCaptcha.value == '') {
                    $('#txtCaptcha').addClass('has-error');
                    $('#lblCaptcha').show().text('Please enter the characters as shown in the above Captcha.');
                    status = false;
                }
                else {
                    $('#txtCaptcha').removeClass('has-error');
                }

                if (status == false) {
                    return false;
                }
                else {
                    document.getElementById('hdnFlag').value = '';
                }
                return true;
            }


            function CheckFirstChar(key, txt) {

                if (key == 32 && txt.value.length <= 0) {
                    return false;
                }
                else if (txt.value.length > 0) {
                    if (txt.value.charCodeAt(0) == 32) {
                        txt.value = txt.value.substring(1, txt.value.length);
                        return true;
                    }
                }
                return true;
            }

            //            function pageLoad() {
            //                var manager = Sys.WebForms.PageRequestManager.getInstance();
            //                manager.add_endRequest(endRequest);
            //                manager.add_beginRequest(OnBeginRequest);
            //            }
            //            function OnBeginRequest(sender, args) {
            //                var postBackElement = args.get_postBackElement();
            //                if (postBackElement.id == 'btnPay') {
            //                    $get('CheckOutProgress').style.display = "block";

            //                }

            //            }
            //            function endRequest(sender, args) {
            //            }

            function pageLoad() {
                $('[data-numeric]').payment('restrictNumeric');
                $('.cc-number').payment('formatCardNumber');
                $('.cc-exp').payment('formatCardExpiry');
                $('.cc-cvc').payment('formatCardCVC');

                $('input#txtCardNumber').validateCreditCard(function (result) {
                    var currentClass = $('.cardTypeIcon i').attr('class');
                    $('.cardTypeIcon i').removeClass(currentClass).addClass(result.card_type == null ? 'default' : result.card_type.name);
                });
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_beginRequest(BeginRequestHandler);
                prm.add_endRequest(EndRequestHandler);
            }
            function BeginRequestHandler(sender, args) {

                if (args._postBackElement.id != '<%=btnPay.ClientID %>') {
                    document.getElementById('divHide').style.display = "none";
                }
                else {
                    if (document.getElementById('hdnFlag').value == "true") {
                        document.getElementById('divHide').style.display = "none";
                    }
                    else {
                        document.getElementById('divHide').style.display = "block";
                    }
                }
            }

            function EndRequestHandler(sender, args) {

                if (document.getElementById('hdnFlag').value == "true") {
                    document.getElementById('divHide').style.display = "none";
                }
                else {
                    document.getElementById('divHide').style.display = "block";
                }
            }

            function clearCaptcha() {
                document.getElementById("<%=txtCaptcha.ClientID %>").value = '';
            }
        </script>
        <!-- header container having logo-->
        <header>
        <div class="container">
            <div class="logo-wrap col-lg-2">
                <img src="../Images/logo.png" alt="REV" />
            </div>
            <div class="col-lg-8">
                <h3>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblHeader" runat="server" Text="Payment Checkout"></asp:Label>
                </h3>
            </div>
            <div class="col-lg-2">
                &nbsp;</div>
        </div>
        </header>
        <!-- Checkout form -->
        <div class="container">
            <div class="formWrap">
                <p class="helpText">
                    &nbsp;</p>
                <%--<form novalidate autocomplete="on" method="POST" class="checkoutForm">--%>
                <asp:UpdatePanel ID="CheckOutPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblProgress" runat="server" CssClass="transaction"></asp:Label>
                        <strong>Welcome
                        <asp:Label ID="lblWelcomeText" runat="server"> </asp:Label>
                        , Please complete this form to process your payment</strong><div class="form-group">
                            <h4 class="amount">
                                Quote Number:
                                <asp:Label ID="lblQuoteNumber" runat="server" CssClass="amountCurrency"></asp:Label>&nbsp;
                            </h4>
                        </div>
                        <div class="form-group">
                            <h4 class="amount">
                                Amount: <span style="padding-left: 55px;">
                                    <asp:Label ID="lblCurrencyValue" runat="server" CssClass="amountCurrency"></asp:Label>&nbsp;
                                    <asp:Label ID="lblAmountValue" runat="server" CssClass="amountCurrency"></asp:Label>
                                </span>
                            </h4>
                        </div>
                        <div class="form-group">
                            <label for="cc-number" class="control-label">
                                Card Number <sup class="mandatory">*</sup><!-- <small class="text-muted">[<span class="cc-brand"></span>]</small> --></label>
                            <div class="input-group form-group">
                                <input id="txtCardNumber" runat="server" type="text" class="input-lg form-control cc-number"
                                    autocomplete="off" placeholder="Enter Card Number" maxlength="20" tabindex="1" />
                                <span class="input-group-addon cardTypeIcon"><i class="default"></i></span>
                            </div>
                            <asp:Label ID="lblCardNoError" runat="server" CssClass="errorMsg"></asp:Label>
                            <label for="chkRememberMe" class="control-label remember-me" id="lblCheckBox" runat="server">
                                <input id="chkRememberMe" type="checkbox" runat="server" tabindex="2" />
                                Save Card Number</label>
                        </div>
                        <div class="row">
                            <div class="form-group col-md-6">
                                <label for="cc-exp" class="control-label">
                                    Card Expiry <sup class="mandatory">*</sup></label>
                                <div class="date-hold">
                                    <asp:DropDownList ID="txtExpiryMonth" runat="server" TabIndex="3" class="form-control  cc-exp" >
                                       
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="txtExpiryYear" runat="server" TabIndex="4" 
                                        class="form-control  cc-exp"  >
                                        
                                    </asp:DropDownList>
                                    <asp:Label ID="lblExpiryMonthYear" runat="server" CssClass="errorMsg"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="cc-cvc" class="control-label">
                                    CVV Number <sup class="mandatory">*</sup></label>
                                <input type="text" name="prevent_autofill" id="prevent_autofill" value="" style="display: none;" />
                                <input type="password" name="password_fake" id="password_fake" value="" style="display: none;" />
                                <input id="txtCvvNumber" runat="server" type="password" class="input-lg form-control cc-cvc"
                                    placeholder="CVV" maxlength="4" title="enter cvv" tabindex="5" autocomplete="off" />
                                <asp:Label ID="lblCvvNumber" runat="server" CssClass="errorMsg"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="name-on-card" class="control-label">
                                Name on Card <sup class="mandatory">*</sup></label>
                            </label>
                            <input id="txtCardName" runat="server" type="text" class="input-lg form-control"
                                placeholder="Name on Card" maxlength="20" tabindex="6" autocomplete="off" onkeyup="CheckFirstChar(event.keyCode, this);"
                                onkeydown="return CheckFirstChar(event.keyCode, this);" />
                            <asp:Label ID="lblCardName" runat="server" CssClass="errorMsg"></asp:Label>
                        </div>
                        <div class="row">
                            <div class="form-group col-md-12">
                                <label for="captcha" class="control-label">
                                    Type the characters you see in the Captcha below</label>
                            </div>
                        </div>
                        <div class="row">
                            <asp:UpdatePanel ID="uppnlCaptcha" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="imgRefresh" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="form-group col-md-7">
                                        <cap:CaptchaControl ID="captNoBot" CssClass="capNoBot col-md-10" runat="server" CaptchaBackgroundNoise="None"
                                            CaptchaLength="6" CaptchaWidth="250" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                            CaptchaLineNoise="None" CaptchaChars="ACDEFGHIJKLMNOPQRTUVXYZ2346789" BackColor="#AFB7BC"
                                            CustomValidatorErrorMessage="<%$ Resources:REVCheckOut,
                                             captchaMismatch %>" />
                                        <div class="form-group col-md-2 pull-center">
                                            <asp:ImageButton ID="imgRefresh" ImageUrl="~/Images/re-fresh.png" runat="server"
                                                CausesValidation="false" ToolTip="Refresh" Style="position: absolute; top: 15px;"
                                                OnClientClick="clearCaptcha();" />
                                        </div>
                                    </div>
                                    <div class="form-group col-md-5 pull-right">
                                        <input id="txtCaptcha" runat="server" type="text" class="input-lg form-control" maxlength="6"
                                            tabindex="7" autocomplete="off" style="text-transform: uppercase" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblCaptcha" runat="server" CssClass="errorMsg"></asp:Label>
                        <hr class="line-sep" />
                        <div class="form-group text-center">
                            <%--<button type="submit"
                        class="btn btn-primary btn-pay" id="btnPay" name="" runat="server" onclick="btnPay_Click">
                        Pay</button>--%>
                            <asp:Button ID="btnPay" CssClass="btn btn-primary btn-pay" runat="server" Text="<%$ Resources:REVCheckOut, btnPay %>"
                                TabIndex="8" OnClick="btnPay_Click" OnClientClick="return Validate();" />
                            <%--<button type="reset" class="btn btn-primary
                        btn-cancel" id="btnCancel1" runat="server" onclick="btnCancel_Click"> Cancel</button>--%>
                            <asp:Button ID="btnCancel" CssClass="btn btn-primary btn-cancel" runat="server" Text="<%$ Resources:REVCheckOut, btnCancel %>"
                                TabIndex="9" OnClick="btnCancel_Click" />
                            <h5 class="validation ">
                                <asp:Label ID="lblError" runat="server" CssClass="errorMsg"></asp:Label>
                            </h5>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="divHide" style="display: none;">
                    <asp:UpdateProgress ID="CheckOutProgress" runat="server" AssociatedUpdatePanelID="CheckOutPanel">
                        <ProgressTemplate>
                            <div id="divProgress" runat="server">
                                <asp:Image ID="imgProgress" src="../Images/ajax-loader-fb-default.gif"  runat="server" /><br />
                                <span id="lblProgress" class="transaction">
                                    <%=Resources.REVCheckOut.lblProgress
                                    %></span>
                            </div>
                            <div id="divBackGround">
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </div>
        </div>
        <div class="footer">
            <asp:HiddenField ID="hdnFlag" runat="server" />
             <asp:HiddenField ID="hdnMonth" runat="server" />
            <asp:HiddenField ID="hdnYear" runat="server" />
            <%-- <%=Resources.REVCheckOut.lblCompanyTerms%>
    <asp:HyperLink ID="hlnkTFPaymentPolicies" Target="_blank" runat="server" CssClass="footer_link"
    NavigateUrl="<%$ AppSettings:PolicyUrl %>" Text="<%$ AppSettings:PolicyName %>"></asp:HyperLink>.
    <%=Resources.REVCheckOut.lblCopyright%> <asp:HyperLink ID="hlnkTFPaymentInc"
    Target="_blank" runat="server" CssClass="footer_link" NavigateUrl="<%$ AppSettings:PlatformUrl
    %>" Text="<%$ AppSettings:PlatformName %>"></asp:HyperLink>. <%=Resources.REVCheckOut.lblTerms%>--%>
        </div>
        <%-- <div> <%=Resources.REVCheckOut.lblVersion%> (<asp:Label ID="lblVersion"
    runat="server" Text="<%$ AppSettings:Version %>" />)</div>--%>
        </form>
    </div>
    <%-- </div>--%>
    <!-- Javascript Library-->
</body>
</html>
