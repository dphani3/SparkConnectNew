<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SmarterCommercePayForm.aspx.cs"
    Inherits="SmarterCommercePayForm" %>

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
           <%-- <tr>
                <td align="left" style="width: 30%; font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAuthToken" runat="server" Text="Auth Token:"></asp:Label>
                </td>
                <td align="left" style="width: 70%; border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtAuthToken" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>--%>
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
                    <asp:Label ID="lblAddressLine1" runat="server" Text="AddressLine1:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtAddressLine1" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAddressLine2" runat="server" Text="AddressLine2:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtAddressLine2" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAddressLine3" runat="server" Text="AddressLine3:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtAddressLine3" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblAddressLine4" runat="server" Text="AddressLine4:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtAddressLine4" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCity" runat="server" Text="City:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtCity" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblCountry" runat="server" Text="Country:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtCountry" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblFax" runat="server" Text="Fax:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtFax" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblName" runat="server" Text="Name:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtName" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblPhone" runat="server" Text="Phone:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtPhone" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblPostalCode" runat="server" Text="Postal Code:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtPostalCode" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblState" runat="server" Text="State:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtState" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    <asp:Label ID="lblOrderCompany" runat="server" Text="Order Company:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtOrderCompany" runat="server" Width="80%"></asp:TextBox>
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
                    <asp:Label ID="lblOrderType" runat="server" Text="Order Type:"></asp:Label>
                </td>
                <td align="left" style="border: solid 1px #bbbbbb;">
                    <asp:TextBox ID="txtOrderType" runat="server" Width="80%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-weight: bold; border: solid 1px #bbbbbb;">
                    Amount:
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
