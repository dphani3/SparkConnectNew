﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConvertQuoteProxy.aspx.cs" Inherits="ConvertQuoteProxy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=Resources.FocusPayCheckOut.ltlTitle %></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ProxyManager" runat="server" EnablePartialRendering="false">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="ProxyPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
