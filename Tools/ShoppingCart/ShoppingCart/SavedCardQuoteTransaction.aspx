<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SavedCardQuoteTransaction.aspx.cs"
    Inherits="SavedCardQuoteTransaction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 0 auto; width: 70%; height: 100%;">
        <table id="tblConsumerInfo" runat="server" style="border: solid 2px #bbbbbb; margin: 0 auto;
            width: 80%;">
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblOrganizationCode" runat="server" Text="Organization Code:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtOrganizationCode" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblUserName" runat="server" Text="User Name:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtUserName" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCarrierCode" runat="server" Text="Carrier Code:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtCarrierCode" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCompany" runat="server" Text="Company:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtCompany" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCustomerPO" runat="server" Text="Customer PO:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtCustomerPO" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblQuoteNumber" runat="server" Text="Quote Number:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtQuoteNumber" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblShippingInstructions1" runat="server" Text="Shipping Instructions1:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtShippingInstructions1" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblShippingInstructions2" runat="server" Text="Shipping Instructions2:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtShippingInstructions2" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Saved Card Customer:
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtSavedCardCustomer" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Saved Card Line:
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtSavedCardLine" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Button ID="btnPay" runat="server" Text="Pay" OnClick="btnPay_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2" id="trResponse" runat="server" visible="false">
                    <span style="font-size: large; color: Blue;" id="spanResponse" runat="server" visible="false">
                        Response: </span>&nbsp;&nbsp;
                    <asp:Label ID="lblResponse" runat="server" Style="color: Green;"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
