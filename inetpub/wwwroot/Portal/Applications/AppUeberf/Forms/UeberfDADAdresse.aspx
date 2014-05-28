<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UeberfDADAdresse.aspx.vb"
    Inherits="AppUeberf.UeberfDADAdresse" %>

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
        .ErrorColumn
        {
            height: 34px;
        }
        .style32
        {
            width: 510px;
        }
        .TitelGreen
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
                        <td valign="top">
                            <table id="Table1" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td class="ErrorColumn">
                                        &nbsp;<asp:Label ID="lblError" runat="server" EnableViewState="False"
                                            CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 437px">
                                        <b>
                                            <asp:Panel ID="Panel1" runat="server" BackColor="#EEEEEF" style="min-height: 150px; padding:5px; vertical-align: middle;">
                                                <br />
                                                <span class="TitelGreen">Adresssuche:</span>
                                                <table width="100%" style="padding-right: 3px; text-align: left;">
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
                                                            <asp:TextBox ID="txtName" runat="server" Width="196px" MaxLength="35"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPLZ" runat="server" Width="74px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOrt" runat="server" Width="100%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%" >
                                                    <tr >
                                                        <td >
                                                            <asp:Button ID="btnSuchen" runat="server" Text="Suchen" Width="97px" style="margin: 15px 0px;" />
                                                        </td>
                                                        <td style="white-space: nowrap;">
                                                            Platzhaltersuche durch Verwendung des Zeichens * möglich.
                                                        </td>
                                                    </tr>
                                                </table>
                                                <span class="TitelGreen">Auswahl:</span><br />
                                                <br/>
                                                <span style="padding: 3px 3px;">
                                                     <asp:DropDownList ID="drpAdresse" runat="server" Width="500px">
                                                        <asp:ListItem>Auswahl</asp:ListItem>
                                                     </asp:DropDownList>
                                                </span>
                                                <br/>
                                                <table style="text-align: right;" width="100%">
                                                    <tr>
                                                        <td class="style32" style="text-align: right">
                                                            <asp:LinkButton ID="lbCancel" TabIndex="12" runat="server" CssClass="StandardButtonTable"
                                                                Width="100px">Abbrechen</asp:LinkButton>
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:LinkButton ID="lbGet" TabIndex="12" runat="server" CssClass="StandardButtonTable"
                                                                Width="80px">
                                                                    Übernehmen
                                                            </asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 437px" width="437">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
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
