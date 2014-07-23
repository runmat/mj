<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogonMessage.aspx.vb" Inherits="CKG.Portal.LogonMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Logon Message</title>
    <style type="text/css">
        .tabHeader
        {
            vertical-align: middle;
            margin-left: 5px;
            margin-right: 5px;
            white-space: nowrap;
            height: 40px;
        }
        .tabSeparator
        {
            vertical-align: middle;
            margin-left: 5px;
            margin-right: 5px;
            height: 10px;
        }
        .tabBody div
        {
            padding-left: 15px;
            padding-right: 5px;
            vertical-align: top;
            height: 260px;
            width: 580px;
            overflow: auto;
        }
        .tabControls
        {
            vertical-align: middle;
            height: 45px;
            text-align: center;
        }
        .imgMsg
        {
            width: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel Style="background-color: White" ID="pnBody" runat="server" Width="100%"
        Height="100%">
        <table style="background-color: White; width: 100%;">
            <tr class="tabHeader">
                <td class="imgMsg">
                    <asp:Image ID="imgMessage" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="Large" Text="Überschrift" />
                </td>
            </tr>
            <tr class="tabSeparator">
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr class="tabBody">
                <td colspan="2">
                    <div>
                        <asp:Label Font-Size="12px" ID="lblMessage" runat="server" />
                    </div>
                </td>
            </tr>
            <tr class="tabSeparator">
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr class="tabControls">
                <td colspan="2">
                    <asp:Button OnClientClick="window.close();" ID="btOk" runat="server" Text="Ok" Width="101px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>