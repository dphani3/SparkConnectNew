<%--/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Transact.aspx
  Description: This file is the entry point for the external applications like MPOS, VPOS, Thin Client etc...
  Date Created : 12-Apr-2010
  Revision History: 
  */--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Transact.aspx.cs" Inherits="Transact" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TFPS Transact</title>
</head>
<body>
    <form id="form1" runat="server">
    <p>
        TFPS</p>
    <div>
        Input XML request here:<br />
        <br />
        <asp:TextBox ID="txtInput" runat="server" Height="270px" Width="1056px" TextMode="MultiLine"></asp:TextBox>
        <br />
        <br />
        <br />
        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
    </div>
    </form>
</body>
</html>
