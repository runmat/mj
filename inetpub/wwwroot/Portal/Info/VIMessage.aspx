<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VIMessage.aspx.vb" Inherits="CKG.Portal.VIPMessage" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
    <div style="border-right: #cccccc 0px solid; border-top: #cccccc 0px solid; overflow: auto;
        border-left: #cccccc 0px solid; border-bottom: #cccccc 0px solid; height: 200px; font-family: Arial, Helvetica, Sans-Serif; font-size: 12px;">
        <asp:Repeater ID="Repeater1"  runat="server">
            <HeaderTemplate >
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td width="20">
                        <img alt="" src="/Portal/Images/arrow.gif" border="0">
                    </td>
                    <td>
                        <b>
                            <%# DataBinder.Eval(Container.DataItem, "titleText") %></b>
                    </td>
                </tr>
                <tr>
                    <td width="20">
                        &nbsp;
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "messageText") %>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </TABLE>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
