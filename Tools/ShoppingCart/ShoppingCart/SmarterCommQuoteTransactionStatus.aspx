<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SmarterCommQuoteTransactionStatus.aspx.cs"
    Inherits="SmarterCommQuoteTransactionStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Transaction Status</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 0 auto; width: 70%; height: 100%;">
        &nbsp;&nbsp;&nbsp;
        <table id="tblTrxStatus" runat="server" style="border: solid 2px #bbbbbb; margin: 0 auto;
            width: 80%;">
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblResponseCode" runat="server" Text="Response Code:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblResponseCodeValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblResponseMessage" runat="server" Text="Response Message:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblResponseMessageValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblTransactionAmount" runat="server" Text="Transaction Amount:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblTransactionAmountValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAuthCode" runat="server" Text="Auth Code:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAuthCodeValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblOrderNumber" runat="server" Text="Order Number:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblOrderNumberValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Remember Me
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblRememberMeValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCardNumber" runat="server" Text="Card Number:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCardNumberValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCardType" runat="server" Text="Card Type:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCardTypeValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblPNRef" runat="server" Text="PN Ref:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblPNRefValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblSavedCardCustomer" runat="server" Text="Saved Card Customer:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblSavedCardCustomerValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblSavedCardLine" runat="server" Text="Saved Card Line:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblSavedCardLineValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:LinkButton ID="lnkbtnHome" runat="server" Text="Pay" OnClick="lnkbtnPay_Click" />
                    &nbsp;&nbsp; &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkbtnSavedCardTransaction" runat="server" Text="Saved Card"
                        OnClick="lnkbtnSavedCard_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
