<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransactionStatus.aspx.cs"
    Inherits="TransactionStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction Status</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 0 auto; width: 70%; height: 100%;">
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
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblResponseMessage" runat="server" Text="Response Message:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblResponseMessageValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAmount" runat="server" Text="Transaction Amount:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAmountValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblTransactionID" runat="server" Text="Transaction ID:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblTransactionIDValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblTransactionTime" runat="server" Text="Transaction Time:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblTransactionTimeValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trToken" runat="server" visible="false">
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Token
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblToken" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Remember Me
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblRememberMe" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Card Number
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCardNumber" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Card Name</td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblcardName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Card Type
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCardType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    App Key</td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAppKey" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:LinkButton ID="lnkbtnHome" runat="server" Text="Pay" OnClick="lnkbtnPay_Click" />
                    &nbsp;&nbsp; &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkbtnTokenTransaction" runat="server" Text="Token" OnClick="lnkbtnToken_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
