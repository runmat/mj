<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report10.aspx.vb" Inherits="AppAvis.Report10" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .StandardButton
        {}
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
        <uc1:BusyIndicator ID="BusyIndicator1" runat="server" />
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager runat="server" ID="Scriptmanager1" EnableScriptLocalization="true"
        EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton" Height="16px"
                                            Width="100px"> &#149;&nbsp;Erstellen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton
                                            ID="lbBack" runat="server" Height="16px" CssClass="StandardButton" Visible="False">•&nbsp;Zur Übersicht</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton" Height="16px"
                                            Width="100px" CausesValidation="false"> •&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="tblSelection" runat="server" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td width="160">
                                                    Rechnungsnummer von:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtRechnNr" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Rechnungsnummer bis:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtRechnNrBis" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtFahrgestNr" runat="server"></asp:TextBox>
                                                    &nbsp;
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Rechnungsdatum von:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtRechnDatVon" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtRechnDatVon_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtRechnDatVon">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                    <asp:CompareValidator ID="cv_txtRechnDatVon" runat="server" ErrorMessage="Falsches Datumsformat"
                                                        Type="Date" ControlToValidate="txtRechnDatVon" ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Rechnungsdatum bis:
                                                </td>
                                                <td class="StandardTableAlternate" valign="center">
                                                    <asp:TextBox ID="txtRechnDatBis" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtRechnDatBis_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtRechnDatBis">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                    <asp:CompareValidator ID="cv_txtRechnDatBis" runat="server" ErrorMessage="Falsches Datumsformat"
                                                        Type="Date" ControlToValidate="txtRechnDatBis" ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Leistungsdatum von:
                                                </td>
                                                <td class="StandardTableAlternate" valign="center">
                                                    <asp:TextBox ID="txtLeistDatVon" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtLeistDatVon_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtLeistDatVon">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                    <asp:CompareValidator ID="cv_txtLeistDatVon" runat="server" ErrorMessage="Falsches Datumsformat"
                                                        Type="Date" ControlToValidate="txtLeistDatVon" ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Leistungsdatum bis:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtLeistDatBis" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtLeistDatBis_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtLeistDatBis">
                                                    </cc1:CalendarExtender>
                                                    &nbsp;(TT.MM.JJJJ)&nbsp;
                                                    <asp:CompareValidator ID="cv_txtLeistDatBis" runat="server" ErrorMessage="Falsches Datumsformat"
                                                        Type="Date" ControlToValidate="txtLeistDatBis" ControlToCompare="Textbox2" Operator="DataTypeCheck"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Leistungscode von:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtLeistungscodeVon" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Leistungscode bis:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtLeistungscodeBis" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Leistungsart:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtLeistArt" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Abgearbeitet:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:RadioButtonList ID="rblAbgearbeitet" runat="server" 
                                                        RepeatDirection="Horizontal">
                                                        <asp:ListItem Selected="True" Value="N">Nein</asp:ListItem>
                                                        <asp:ListItem Value="J">Ja</asp:ListItem>
                                                        <asp:ListItem Value="A">Alle</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Ohne Grunddatenvorlage:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:CheckBox ID="cbxGrunddatenvorlage" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="160">
                                                    Spediteur:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:DropDownList ID="drpSpediteur" runat="server">
                                                        <asp:ListItem>Alle</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="'Rechnungsdatum von' darf nicht größer als 'Rechnungsdatum bis' sein!"
                                            Type="Date" ControlToValidate="txtRechnDatVon" ControlToCompare="txtRechnDatBis"
                                            Operator="LessThanEqual" Font-Bold="True"></asp:CompareValidator>
                                        <br />
                                        <asp:CompareValidator ID="CompareValidator6" runat="server" ErrorMessage="'Leistungsdatum von' darf nicht größer als 'Leistungsdatum bis' sein!"
                                            Type="Date" ControlToValidate="txtLeistDatVon" ControlToCompare="txtLeistDatBis"
                                            Operator="LessThanEqual" Font-Bold="True"></asp:CompareValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120" >
                        </td>
                        <td style="float: left;width: 100%">
                            <table id="tblKum" runat="server" Visible="False" style="float: left" width="800px">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblSuccess" runat="server" 
                                            Text="Ihre Daten wurden erfolgreich gespeichert." Visible="False" 
                                            EnableViewState="False" ForeColor="#009900" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="lblSaveError" runat="server" Visible="False" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>

                                </tr>
                                <tr style="border-style: solid; border-width: 1px; border-color: gray; padding: 1px 4px">
                                    <td class="">
                                        <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:DataGrid ID="dgKum" runat="server" BackColor="White" AutoGenerateColumns="False"
                                            PageSize="10" headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain"
                                            AllowSorting="True" AllowPaging="True" Visible="False" Style="float: left;width: 100%">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" CssClass="GridTableHead" ></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn SortExpression="RECH_NR" HeaderText="Rechnungsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Rechnungsnummer" runat="server" CommandName="Sort" CommandArgument="RECH_NR">Rechnungsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbRechnungsnummer" ForeColor="Blue" runat="server" CommandArgument="Rechnungsnummer" CommandName="Rechnr"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.RECH_NR") %>'>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="SPEDI" SortExpression="SPEDI" HeaderText="Spediteur">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Verarbeitet">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="Verarbeitet" runat="server" CommandName="Sort" CommandArgument="ABGEARB_FLAG">Verarbeitet</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="cbxVerarbeitet" Checked='<%# DataBinder.Eval(Container, "DataItem.ABGEARB_FLAG")="X" %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton" Height="16px"
                                            Width="100px"> •&nbsp;Speichern</asp:LinkButton>
                                        <asp:LinkButton ID="cmdShowAll" runat="server" CssClass="StandardButton" Height="16px"
                                            Width="100px"> •&nbsp;Alle anzeigen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table id="tblDetail" runat="server" Visible="False" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="lnkCreateExcel" ImageUrl="~/Images/excel.gif" runat="server"
                                            Height="20px" Width="20px" ImageAlign="Top"></asp:ImageButton>
                                        <asp:Label ID="lblExcel" runat="server" Text="Excel-Download"></asp:Label>&nbsp;

                                        </td>
                                    <td align="right">
                                        
                                    </td>
                                </tr>
                                <tr>
                                        <td>
                                            <asp:Label ID="lblNoDataDetail" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td class="LabelExtraLarge" align="right">
                                            <asp:DropDownList ID="ddlPageSizeDetail" runat="server" AutoPostBack="True" style="padding-right: 10px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AllowPaging="True" AllowSorting="True"
                                                bodyHeight="300" CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader"
                                                PageSize="50" BackColor="White" AutoGenerateColumns="False">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                                <Columns>
                                                    <asp:BoundColumn DataField="Spediteur" HeaderText="Spediteur" SortExpression="Spediteur"/>
                                                    <asp:BoundColumn DataField="Rechnungsnummer" HeaderText="Rechnungsnummer" SortExpression="Rechnungsnummer"/>
                                                    <asp:BoundColumn DataField="Rechnungsdatum" HeaderText="Rechnungsdatum" SortExpression="Rechnungsdatum" DataFormatString="{0:dd.MM.yyy}"/>
                                                    <asp:BoundColumn DataField="Leistungsdatum" HeaderText="Leistungsdatum" SortExpression="Leistungsdatum" DataFormatString="{0:dd.MM.yyy}"/>
                                                    <asp:BoundColumn DataField="Fahrgestellnummer" HeaderText="Fahrgestellnummer" SortExpression="Fahrgestellnummer"/>
                                                    <asp:BoundColumn DataField="Leistungsart" HeaderText="Leistungsart" SortExpression="Leistungsart"/>
                                                    <asp:BoundColumn DataField="Betrag" HeaderText="Betrag" SortExpression="Betrag"/>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;<asp:TextBox ID="TextBox2" runat="server" Visible="false" />
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
