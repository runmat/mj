<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report_03.aspx.vb" Inherits="AppARVAL.Report_03" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server" Visible="False"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" nowrap colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(Klärfallformular)
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2">
                            <p align="right">
                                <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" CssClass="TaskTitle" NavigateUrl="javascript:window.close()">Fenster schließen</asp:HyperLink></p>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120" height="19">
                        </td>
                        <td valign="top" height="19">
                            <table id="Table2" class="TableFrame" cellspacing="1" cellpadding="3" width="430"
                                border="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        LV-Nr:
                                    </td>
                                    <td width="100%">
                                        <strong>
                                            <asp:Label ID="lblLVNr" runat="server"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        LV beendet zum
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDatum" runat="server" Width="100px"></asp:TextBox>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="/Portal/Images/info.GIF" ToolTip="Format: TT.MM.JJJJ">
                                        </asp:Image>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        SB ist in Ordnung
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxSB" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        Höhe der Entschädigung im<br>
                                        Schadensfall ist in Ordnung
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxEnt" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        Versichererwechsel
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxVers" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        Fahrzeugwechsel
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxFahrz" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        Sonstiges
                                    </td>
                                    <td nowrap>
                                        <asp:TextBox ID="txtBemerkung" runat="server" Width="256px" MaxLength="256"></asp:TextBox>
                                        <asp:Image ID="Image2" runat="server" ImageUrl="/Portal/Images/info.GIF" ToolTip="Maximal 256 Zeichen">
                                        </asp:Image>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        &nbsp;
                                    </td>
                                    <td nowrap>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                    </td>
                                    <td nowrap>
                                        <p align="right">
                                            <asp:LinkButton ID="btnAbsenden" runat="server" CssClass="StandardButtonTable">&#149;&nbsp;Formular absenden</asp:LinkButton></p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ShowScript" runat="server" visible="False">
            <td>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
