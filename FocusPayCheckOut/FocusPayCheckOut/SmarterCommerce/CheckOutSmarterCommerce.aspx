<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckOutSmarterCommerce.aspx.cs"
    Inherits="CheckOutSmarterCommerce" ViewStateEncryptionMode="Always" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cap" %>
<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <%--<meta charset="utf-8" />--%>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Milacron</title>
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
            </Scripts>
        </asp:ScriptManager>
        <script type="text/javascript">

            window.onload = window.history.forward(0);

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
                $("input#txtExpiryMonthYear").on('change', function () {
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
                var txtExpiryMonthYear = document.getElementById("<%=txtExpiryMonthYear.ClientID %>");
                var txtCvvNumber = document.getElementById("<%=txtCvvNumber.ClientID %>");
                var txtCardName = document.getElementById("<%=txtCardName.ClientID %>");
                var lblError = document.getElementById("<%=lblError.ClientID%>");
                var lblCardNoError = document.getElementById("<%=lblCardNoError.ClientID%>");
                var txtCaptcha = document.getElementById("<%=txtCaptcha.ClientID %>");

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


                if (txtExpiryMonthYear.value == '') {

                    $('#txtExpiryMonthYear').addClass('has-error');
                    $('#lblExpiryMonthYear').show().text('Enter card expiry.');
                    //                    txtExpiryMonthYear.focus();
                    status = false;
                }
                else {
                    var cardexpiry = txtExpiryMonthYear.value.replace(/\s/g, "");

                    var date = new Date();

                    var currentMonth = date.getMonth() + 1;
                    var currentYear = date.getFullYear();

                    if (currentMonth < 10) currentMonth = "0" + currentMonth;
                    var currYear = currentYear.toString().substr(2, 2);

                    var expiryMonth = cardexpiry.substring(0, 2);
                    var expiryYear = cardexpiry.substring(3, 5);

                    var cardlength = expiryMonth + expiryYear;

                    if (cardlength.length != 4) {

                        $('#txtExpiryMonthYear').addClass('has-error');
                        $('#lblExpiryMonthYear').show().text('Invalid Card Expiry.');
                        txtCaptcha.value = '';
                        status = false;
                    }
                    else if (expiryYear < currYear) {

                        $('#txtExpiryMonthYear').addClass('has-error');
                        $('#lblExpiryMonthYear').show().text('Invalid Card Expiry.');
                        txtCaptcha.value = '';
                        status = false;
                    }
                    else if (expiryYear > currYear) {

                        //                        if (expiryMonth < currentMonth) {
                        //                            alert('a');
                        //                            $('#txtExpiryMonthYear').addClass('has-error');
                        //                            $('#lblExpiryMonthYear').show().text('Invalid Card Expiry.');
                        //                            txtCaptcha.value = '';
                        //                            status = false;
                        //                        }
                        if (expiryMonth > 12) {

                            $('#txtExpiryMonthYear').addClass('has-error');
                            $('#lblExpiryMonthYear').show().text('Invalid Card Expiry.');
                            txtCaptc
                            ha.value = '';
                            status = false;
                        }
                    }
                    else if (expiryYear == currYear) {

                        if (expiryMonth < currentMonth) {

                            $('#txtExpiryMonthYear').addClass('has-error');
                            $('#lblExpiryMonthYear').show().text('Invalid Card Expiry.');
                            txtCaptcha.value = '';
                            status = false;
                        }
                        else if (expiryMonth > 12) {

                            $('#txtExpiryMonthYear').addClass('has-error');
                            $('#lblExpiryMonthYear').show().text('Invalid Card Expiry.');
                            txtCaptc
                            ha.value = '';
                            status = false;
                        }
                    }
                    else {
                        $('.cc-exp').payment('formatCardExpiry');
                        $('#txtExpiryMonthYear').removeClass('has-error');
                    }
                }


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
                <img src="../Images/logo.png" alt="Milacron" />
            </div>
            <div class="col-lg-8">
                <h3>
                    Payment Checkout</h3>
            </div>
            <div class="col-lg-2">
                &nbsp;</div>
        </div>
        </header>
        <!-- Checkout form -->
        <div class="container">
            <div class="formWrap">
                <p class="helpText">
                    <strong>Welcome
                        <asp:Label ID="lblWelcomeText" runat="server"> </asp:Label>, Please complete this
                        form to process your payment</strong></p>
                <%--<form novalidate autocomplete="on" method="POST" class="checkoutForm">--%>
                <asp:UpdatePanel ID="CheckOutPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblProgress" runat="server" CssClass="transaction"></asp:Label>
                        <div class="form-group">
                            <h4 class="amount">
                                Order Number:
                                <asp:Label ID="lblOrderNumber" runat="server" CssClass="amountCurrency"></asp:Label>&nbsp;
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
                                <input id="txtExpiryMonthYear" runat="server" type="text" class="input-lg form-control cc-exp"
                                    autocomplete="off" placeholder="MM / YY" maxlength="7" tabindex="3" />
                                <asp:Label ID="lblExpiryMonthYear" runat="server" CssClass="errorMsg"></asp:Label>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="cc-cvc" class="control-label">
                                    CVV Number <sup class="mandatory">*</sup></label>
                                <input type="text" name="prevent_autofill" id="prevent_autofill" value="" style="display: none;" />
                                <input type="password" name="password_fake" id="password_fake" value="" style="display: none;" />
                                <input id="txtCvvNumber" runat="server" type="password" class="input-lg form-control cc-cvc"
                                    placeholder="CVV" maxlength="4" title="enter cvv" tabindex="4" autocomplete="off" />
                                <asp:Label ID="lblCvvNumber" runat="server" CssClass="errorMsg"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="name-on-card" class="control-label">
                                Name on Card <sup class="mandatory">*</sup></label>
                            </label>
                            <input id="txtCardName" runat="server" type="text" class="input-lg form-control"
                                placeholder="Name on Card" maxlength="20" tabindex="5" autocomplete="off" onkeyup="CheckFirstChar(event.keyCode, this);"
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
                                            CustomValidatorErrorMessage="<%$ Resources:FocusPayCheckOut,
                                             captchaMismatch %>" />
                                        <div class="form-group col-md-2 pull-center">
                                            <asp:ImageButton ID="imgRefresh" ImageUrl="~/Images/re-fresh.png" runat="server"
                                                CausesValidation="false" ToolTip="Refresh" Style="position: absolute; top: 15px;"
                                                OnClientClick="clearCaptcha();" />
                                        </div>
                                    </div>
                                    <div class="form-group col-md-5 pull-right">
                                        <input id="txtCaptcha" runat="server" type="text" class="input-lg form-control" maxlength="6"
                                            tabindex="6" autocomplete="off" style="text-transform: uppercase" />
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
                            <asp:Button ID="btnPay" CssClass="btn btn-primary btn-pay" runat="server" Text="<%$ Resources:FocusPayCheckOut, btnPay %>"
                                TabIndex="7" OnClick="btnPay_Click" OnClientClick="return Validate();" />
                            <%--<button type="reset" class="btn btn-primary
                        btn-cancel" id="btnCancel1" runat="server" onclick="btnCancel_Click"> Cancel</button>--%>
                            <asp:Button ID="btnCancel" CssClass="btn btn-primary btn-cancel" runat="server" Text="<%$ Resources:FocusPayCheckOut, btnCancel %>"
                                TabIndex="8" OnClick="btnCancel_Click" />
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
                                <asp:Image ID="imgProgress" src="../Images/ajax-loader-fb-default.gif" runat="server" /><br />
                                <span id="lblProgress" class="transaction">
                                    <%=Resources.FocusPayCheckOut.lblProgress
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
            <%-- <%=Resources.FocusPayCheckOut.lblCompanyTerms%>
    <asp:HyperLink ID="hlnkTFPaymentPolicies" Target="_blank" runat="server" CssClass="footer_link"
    NavigateUrl="<%$ AppSettings:PolicyUrl %>" Text="<%$ AppSettings:PolicyName %>"></asp:HyperLink>.
    <%=Resources.FocusPayCheckOut.lblCopyright%> <asp:HyperLink ID="hlnkTFPaymentInc"
    Target="_blank" runat="server" CssClass="footer_link" NavigateUrl="<%$ AppSettings:PlatformUrl
    %>" Text="<%$ AppSettings:PlatformName %>"></asp:HyperLink>. <%=Resources.FocusPayCheckOut.lblTerms%>--%>
        </div>
        <%-- <div> <%=Resources.FocusPayCheckOut.lblVersion%> (<asp:Label ID="lblVersion"
    runat="server" Text="<%$ AppSettings:Version %>" />)</div>--%>
        </form>
    </div>
    <%-- </div>--%>
    <!-- Javascript Library-->
</body>
</html>
