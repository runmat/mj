<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_Suchhilfe.aspx.vb"
    Inherits="AppAlphabet.Change01_Suchhilfe" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="HEAD1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        #Table12
        {
            width: 124px;
        }
        .style1
        {
            width: 185px;
        }
        .style3
        {
            width: 185px;
            font-weight: bold;
        }
        #Table1
        {
            margin-left: 0px;
        }
        .style4
        {
            width: 123px;
        }
        .style30
        {
            height: 34px;
        }
        .style31
        {
            width: 139px;
        }
        .style32
        {
            width: 510px;
        }
        .style33
        {
            color: #009900;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" width="100%" colspan="3">
                <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                            <asp:Label ID="lblPageTitle" runat="server">Adresssuche</asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="style4">
                            <table id="Table12" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td class="style1">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="style1">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="style3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 917px" valign="top">
                            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="style30">
                                        &nbsp;<asp:Label ID="lblError" runat="server" Width="821px" EnableViewState="False"
                                            CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 620px;">
                                        <asp:Panel ID="Panel1" runat="server" BackColor="#EEEEEF" Height="179px" Style="width: 620px;">
                                            <b>
                                                <br />
                                                <span class="style33">Adresssuche:</span> </b>
                                            <div style="width: 600px;">
                                                <table class="style31">
                                                    <tr>
                                                        <td>
                                                            Name:
                                                        </td>
                                                        <td>
                                                            PLZ:
                                                        </td>
                                                        <td>
                                                            Ort
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtName" runat="server" Width="260px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPLZ" runat="server" Width="74px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOrt" runat="server" Width="260px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="width: 600px;">
                                                <table class="style31">
                                                    <tr>
                                                        <td width="100px">
                                                            <asp:Button ID="btnSuchen" runat="server" Text="Suchen" Width="97px" />
                                                        </td>
                                                        <td>
                                                            Platzhaltersuche durch Verwendung des Zeichens * möglich.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style33" width="100px">
                                                            <b>Auswahl:</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlAdresse" runat="server" Height="16px" Width="500px" AutoPostBack="True">
                                                                <asp:ListItem>Auswahl</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="width: 600px; text-align: right;">
                                                <asp:LinkButton ID="lbCancel" TabIndex="12" runat="server" CssClass="StandardButtonTable"
                                                    Width="100px">Abbrechen</asp:LinkButton>
                                                <asp:LinkButton ID="lbGet" TabIndex="12" runat="server" CssClass="StandardButtonTable"
                                                    Width="80px" Enabled="False">Übernehmen</asp:LinkButton>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                </table>
                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td valign="top" align="middle" width="100%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
        </tr>
    </table>
    </form>
</body>
</html>
