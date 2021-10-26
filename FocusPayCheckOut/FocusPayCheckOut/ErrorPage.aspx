<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

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
    <link rel="stylesheet" href="StyleSheets/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="StyleSheets/style.css" />
    <!-- Favicon -->
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <link rel="icon" href="images/favicon.ico" type="image/x-icon" />
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
        <form id="form1" runat="server" class="checkoutForm">
        <!-- header container having logo-->
        <header>
        <div class="container">
            <div class="logo-wrap col-lg-2">
               <img src="Images/logo.png" alt="Milacron" />
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
            <div class="errmsg">
             We're sorry,the page you are looking for is not available.
             <%--   We're sorry, but an unhandled error occured on the server.
                <br />
                The Server Administrator has been notified and the error logged.
                <br />
                Please close this window.--%>
            </div>
        </div>
        <div class="footer">
          <%--  <%=Resources.FocusPayCheckOut.lblCompanyTerms%>
            <asp:HyperLink ID="hlnkTFPaymentPolicies" Target="_blank" runat="server" CssClass="footer_link"
                NavigateUrl="<%$ AppSettings:PolicyUrl %>" Text="<%$ AppSettings:PolicyName %>"></asp:HyperLink>.
            <%=Resources.FocusPayCheckOut.lblCopyright%>
            <asp:HyperLink ID="hlnkTFPaymentInc" Target="_blank" runat="server" CssClass="footer_link"
                NavigateUrl="<%$ AppSettings:PlatformUrl %>" Text="<%$ AppSettings:PlatformName %>"></asp:HyperLink>.
            <%=Resources.FocusPayCheckOut.lblTerms%>--%>
        </div>
        <div >
           <%-- <%=Resources.FocusPayCheckOut.lblVersion%>
            (<asp:Label ID="lblVersion" runat="server" Text="<%$ AppSettings:Version %>" />)--%>
        </div>
        </form>
    </div>
    <%-- </div>--%>
    <!-- Javascript Library-->
</body>
</html>
