<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_2.aspx.vb" Inherits="AppEC.Change02_2" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
    <input id="txtOrtsKzOld" type="hidden" name="txtOrtsKzOld" runat="server" />
    <input id="txtFree2" type="hidden" name="txtFree2" runat="server" />
    <table width="100%" align="center">
        <tbody>
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="PageNavigation" colspan="2" height="19">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(Anzeige)
                            </td>
                        </tr>
                        <tr>
                            <td class="" valign="top">
                                <p>
                                    &nbsp;</p>
                            </td>
                            <td valign="top" align="left">
                                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="TaskTitle" align="right">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="LabelExtraLarge">
                                            <table id="Table9" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                <tr>
                                                    <td valign="top" align="left" colspan="2">
                                                        <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="left" colspan="2">
                                                        <asp:Label ID="lblTableTitle" runat="server" Font-Bold="True"> Anzahl Datensätze:&nbsp;</asp:Label><asp:Label
                                                            ID="lblAnzahl" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="dataGrid" runat="server" CssClass="tableMain" CellPadding="1" AllowPaging="True"
                                                BackColor="White" Width="99%" AutoGenerateColumns="False" AllowSorting="True"
                                                PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" bodyHeight="300">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle CssClass="GridTableHead" VerticalAlign="Top"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn DataField="UnitNR" SortExpression="UnitNR" HeaderText="Unit Nummer">
                                                    </asp:BoundColumn>
                                                    <%--<asp:BoundColumn Visible="False" DataField="UnitNrPruefziffer" SortExpression="UnitNrPruefziffer" HeaderText="UNPZ"></asp:BoundColumn>--%>
                                                    <asp:TemplateColumn HeaderText="gesperrt">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbxDelete" runat="server" ></asp:CheckBox><%--Checked='<%# DataBinder.Eval(Container, "DataItem.Loesch")="X" %>'--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Sperr Vermerk">
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" Width="120" MaxLength="60" ToolTip='<%# DataBinder.Eval(Container, "DataItem.Sperrdatum") + ", " + DataBinder.Eval(Container, "DataItem.Sperrbenutzer")%> '
                                                                Text='<%# DataBinder.Eval(Container, "DataItem.Sperrbemerkung") %>' ID="txtSperrVermerk">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn Visible="False" DataField="RowId" SortExpression="RowId" HeaderText="Lfd.Nr.">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ModelID" SortExpression="ModelID" HeaderText="ModelID">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Modell" SortExpression="Modell" HeaderText="Modell">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="BatchID" SortExpression="BatchID" HeaderText="BatchID">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Verwendungszweck" SortExpression="Verwendungszweck" HeaderText="Verwendungszweck">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="AuftragsNrVonBis" SortExpression="AuftragsNrVonBis" HeaderText="AuftragsNr">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Anzahl" SortExpression="Anzahl" HeaderText="Anzahl">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Einsteuerung" SortExpression="Einsteuerung" HeaderText="Einsteuerung">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Leasing" SortExpression="Leasing" HeaderText="Leasing">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Bemerkung">
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" Width="120" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>'
                                                                ID="txtBemerkung">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="Sachbearbeiter" SortExpression="Sachbearbeiter" HeaderText="Erfasser">
                                                        <ItemStyle Width="100px"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="EQUNR" SortExpression="EQUNR" HeaderText="EQUNR">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Loesch" SortExpression="Loesch" HeaderText="LV">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Laufzeitbindung" SortExpression="Laufzeitbindung"
                                                        HeaderText="Laufzeitbindung"></asp:BoundColumn>
                                                </Columns>
                                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                    <tr id="trSumme" runat="server">
                                        <td class="LabelExtraLarge">
                                            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td nowrap align="right">
                                                        <p align="left">
                                                            <asp:LinkButton ID="Linkbutton1" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton></p>
                                                    </td>
                                                    <td nowrap align="right" width="100%">
                                                        &nbsp;
                                                        <asp:LinkButton ID="btnSaveSAP" runat="server" CssClass="StandardButton"> &#149;&nbsp;Liste abschicken!</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <p align="left">
                                    &nbsp;</p>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
