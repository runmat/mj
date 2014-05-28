<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report_01.aspx.vb" Inherits="AppCCU.Report_01" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" nowrap colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                            <asp:Label ID="lblPageTitle" runat="server">Anzeige Report</asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2">
                            &nbsp;
                            <asp:HyperLink ID="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:HyperLink>&nbsp;&nbsp;
                            <asp:Label ID="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tbody>
                                                <tr>
                                                    <td class="LabelExtraLarge" width="100%">
                                                        <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                    <td align="right">
                                        <asp:DropDownList ID="ddlPageSize" runat="server" BackColor="White" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="Datagrid1" runat="server" BackColor="White" Width="100%" AllowPaging="True"
                                AllowSorting="True" bodyHeight="400" CssClass="tableMain" bodyCSS="tableBody"
                                headerCSS="tableHeader">
                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateColumn>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <table id="Table10" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td nowrap>
                                                        <asp:HyperLink ID="Hyperlink3" runat="server" Target='<%# "_blank" %>' Width="75px"
                                                            CssClass="StandardButtonTable" NavigateUrl='<%# "Report_01_01.aspx?equipment=" &amp; DataBinder.Eval(Container.DataItem, "Equipment") &amp; "&amp;kf=" &amp; DataBinder.Eval(Container.DataItem, "Klärfall") %>'>Details</asp:HyperLink>&nbsp;
                                                    </td>
                                                    <td nowrap>
                                                        <asp:HyperLink ID="Hyperlink4" runat="server" Visible='<%# DataBinder.Eval(Container.DataItem, "Klärfall")<>"" %>'
                                                            Target='<%# "_blank" %>' Width="75px" CssClass="StandardButtonTable" NavigateUrl='<%# "Report_03.aspx?e=" &amp; DataBinder.Eval(Container.DataItem, "Equipment") %>'>Formular</asp:HyperLink>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="Kl&#228;rfall" SortExpression="Kl&#228;rfall"
                                        HeaderText="Kl&#228;rfall">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="Red"></ItemStyle>
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                        <tr>
                            <td>
                            </td>
                        </tr>
                </table>
                <table id="tblBanner" cellspacing="0" cellpadding="3" runat="server">
                    <tbody>
                        <tr>
                            <td class="Banner">
                                KF1
                            </td>
                            <td class="Banner">
                                =
                            </td>
                            <td class="Banner">
                                Lizenznehmer unterschreibt nicht auf dem Sicherungsschein oder Mahnstufe 4
                            </td>
                        </tr>
                        <tr>
                            <td class="Banner">
                                KF2
                            </td>
                            <td class="Banner">
                                =
                            </td>
                            <td class="Banner">
                                Versicherung unterschreibt nicht, bzw. kein Stempel auf dem Sicherungsschein oder
                                Mahnstufe 4
                            </td>
                        </tr>
                        <tr>
                            <td class="Banner">
                                KF3
                            </td>
                            <td class="Banner">
                                =
                            </td>
                            <td class="Banner">
                                Lizenznehmer hält sich nicht an das Gültigkeitsschema zum Ausfüllen des Sicherungsscheins
                            </td>
                        </tr>
                        <tr>
                            <td class="Banner">
                                KF4
                            </td>
                            <td class="Banner">
                                =
                            </td>
                            <td class="Banner">
                                Versicherung hält sich nicht an das Gültigkeitsschema zum Ausfüllen des Sicherungsscheins
                            </td>
                        </tr>
                        <tr>
                            <td class="Banner">
                                KF5
                            </td>
                            <td class="Banner">
                                =
                            </td>
                            <td class="Banner">
                                Lizenznehmer und Versicherungsnehmer sind nicht identisch
                            </td>
                        </tr>
                        <tr>
                            <td class="Banner">
                                KF6
                            </td>
                            <td class="Banner">
                                =
                            </td>
                            <td class="Banner">
                                §38/§39-Schreiben erhalten (Nichtzahlung der Versicherungsprämie)
                            </td>
                        </tr>
                        <tr>
                            <td class="Banner">
                                KF7
                            </td>
                            <td class="Banner">
                                =
                            </td>
                            <td class="Banner">
                                Kündigung der Versicherung
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
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
    </TD></TR>
    <tr id="ShowScript" runat="server" visible="False">
        <td>
            <script language="Javascript">
						<!--                //
                function FreigebenConfirm(Fahrgest, Vertrag, BriefNr, Kennzeichen) {
                    var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
                    return (Check);
                }
						//-->
            </script>
        </td>
    </tr>
    </form>
</body>
</html>
