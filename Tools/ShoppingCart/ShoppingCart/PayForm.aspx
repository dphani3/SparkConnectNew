<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayForm.aspx.cs" Inherits="PayForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shopping Cart</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 0 auto; width: 70%; height: 100%;">
        <table id="tblConsumerInfo" runat="server" style="border: solid 2px #bbbbbb; margin: 0 auto;
            width: 80%;">
            <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAttndantID" runat="server" Text="Attendant ID:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtAttendantID" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtPassword" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblConsumerName" runat="server" Text="Consumer Name:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtConsumerName" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblConsumerAddress" runat="server" Text="Consumer Address:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtConsumerAddress" runat="server" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblEmailAddress" runat="server" Text="Email Address:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtEmailAddress" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAmount" runat="server" Text="Amount:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtAmount" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Currency:
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtCurrency" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblTransactionType" runat="server" Text="Transaction Type:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <%--<asp:TextBox ID="txtTransactionType" runat="server" Width="80%"></asp:TextBox>--%>
                    <asp:DropDownList ID="ddlTransType" runat="server" Width="135px" Height="20px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged">
                        <asp:ListItem Value="1">Sale</asp:ListItem>
                        <asp:ListItem Value="2">Credit</asp:ListItem>
                        <asp:ListItem Value="3">Prior Auth Capture</asp:ListItem>
                        <asp:ListItem Value="4">Authorize</asp:ListItem>
                        <asp:ListItem Value="14">Refund</asp:ListItem>
                        <asp:ListItem Value="5">Void</asp:ListItem>
                        <asp:ListItem Value="16">Save Card</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblTransactionID" runat="server" Text="TransactionID:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtTransactionID" runat="server" Width="80%" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice Number:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtInvoiceNumber" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblOrderNumber" runat="server" Text="Order Number:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtOrderNumber" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblOrderNumber1" runat="server" Text="AppKey:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtAppKey" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblOrderNumber2" runat="server" Text="Notes:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtNotes" runat="server" Width="80%" 
                        ontextchanged="txtNotes_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblOrderNumber0" runat="server" Text="CustomerNo:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtCustomerNo" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblTestMode" runat="server" Text="Is Test Mode:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:CheckBox ID="chkIsTestMode" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblTokenize" runat="server" Text="Is Tokenize:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:CheckBox ID="chkTokenize" runat="server" Enabled="true" />
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Is Guest User
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:CheckBox ID="chkIsGuestUser" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Button ID="btnPay" runat="server" Text="Pay" OnClick="btnPay_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
