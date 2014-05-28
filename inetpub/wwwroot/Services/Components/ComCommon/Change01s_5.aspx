<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01s_5.aspx.vb" Inherits="CKG.Components.ComCommon.Change01s_5"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change01s.aspx">Suche</asp:HyperLink>&nbsp;<asp:HyperLink
                        ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change01s_2.aspx">| Fahrzeugauswahl</asp:HyperLink>
                    <asp:HyperLink ID="lnkAdressAuswahl" runat="server" NavigateUrl="Change01s_3.aspx"
                        Visible="False"> | Adressauswahl</asp:HyperLink>
                    <asp:HyperLink ID="lnkAbrufgrund" runat="server" NavigateUrl="Change01s_4.aspx"> | Abrufgründe</asp:HyperLink>
                    <a class="active">| Senden</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="TableQuery">
                                    <table id="tblMessage" runat="server" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                        CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                        AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                        <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                        <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                        <PagerSettings Visible="False" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <EmptyDataRowStyle BackColor="#DFDFDF" />
                                        <Columns>
                                            <asp:TemplateField SortExpression="MANDT" HeaderText="Auswahl">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.MANDT")="99" %>'
                                                        Enabled='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull %>'>
                                                    </asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField Visible="False" DataField="id" SortExpression="id" HeaderText="ID">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Erstellt" SortExpression="Erstellt" HeaderText="Erstellt">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="m_strSucheFahrgestellNr" SortExpression="m_strSucheFahrgestellNr"
                                                HeaderText="Fahrg.Nr."></asp:BoundField>
                                            <asp:BoundField DataField="m_liznr" SortExpression="m_liznr" HeaderText="LV-Nr.">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="m_versandadrtext" SortExpression="m_versandadrtext" HeaderText="Versandadr.">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="VersandartShow" SortExpression="VersandartShow" HeaderText="Versand">
                                            </asp:BoundField>
                                            <asp:TemplateField SortExpression="m_abckz" HeaderText="Versandart">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.m_abckz")="1" OR DataBinder.Eval(Container, "DataItem.m_abckz")="3" %>'
                                                        ForeColor="LimeGreen">temporär</asp:Label>
                                                    <asp:Literal ID="Literal1" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.m_abckz")="3" %>'
                                                        Text="<br>-><br>">
                                                    </asp:Literal>
                                                    <asp:Label ID="Label2" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.m_abckz")="2" OR DataBinder.Eval(Container, "DataItem.m_abckz")="3" %>'
                                                        ForeColor="Red">endgültig</asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnFreigeben" runat="server" CssClass="StandardButtonTable" Width="100px"
                                                        Enabled='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull %>'
                                                        CommandName="Freigeben" CommandArgument='<%# Container.DataItemIndex %>'>Freigeben</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton3" runat="server" CssClass="StandardButtonTable" Visible='<%# typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull or 1=1 %>'
                                                        Width="100px" Enabled='<%# (typeof (DataBinder.Eval(Container, "DataItem.Status")) is System.DBNull) OrElse (NOT DataBinder.Eval(Container, "DataItem.Status") = "Vorgang OK") %>'
                                                        CommandName="delete" CommandArgument='<%# Container.DataItemIndex %>'>Storno</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField Visible="False" DataField="m_strHaendlernummer" SortExpression="m_strHaendlernummer"
                                                HeaderText="m_strHaendlernummer"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strHalterNummer" SortExpression="m_strHalterNummer"
                                                HeaderText="m_strHalterNummer"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strStandortNummer" SortExpression="m_strStandortNummer"
                                                HeaderText="m_strStandortNummer"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strZielFirma" SortExpression="m_strZielFirma"
                                                HeaderText="m_strZielFirma"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strZielFirma2" SortExpression="m_strZielFirma2"
                                                HeaderText="m_strZielFirma2"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strZielStrasse" SortExpression="m_strZielStrasse"
                                                HeaderText="m_strZielStrasse"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strZielHNr" SortExpression="m_strZielHNr"
                                                HeaderText="m_strZielHNr"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strZielPLZ" SortExpression="m_strZielPLZ"
                                                HeaderText="m_strZielPLZ"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strZielOrt" SortExpression="m_strZielOrt"
                                                HeaderText="m_strZielOrt"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strZielLand" SortExpression="m_strZielLand"
                                                HeaderText="m_strZielLand"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strAuf" SortExpression="m_strAuf" HeaderText="m_strAuf">
                                            </asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strBetreff" SortExpression="m_strBetreff"
                                                HeaderText="m_strBetreff"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_blnFMSZulassung" SortExpression="m_blnFMSZulassung"
                                                HeaderText="m_blnFMSZulassung"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strSucheFahrgestellNr" SortExpression="m_strSucheFahrgestellNr"
                                                HeaderText="m_strSucheFahrgestellNr"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strSucheKennzeichen" SortExpression="m_strSucheKennzeichen"
                                                HeaderText="m_strSucheKennzeichen"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_strSucheLeasingvertragsNr" SortExpression="m_strSucheLeasingvertragsNr"
                                                HeaderText="m_strSucheLeasingvertragsNr"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_kbanr" SortExpression="m_kbanr" HeaderText="m_kbanr">
                                            </asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_zulkz" SortExpression="m_zulkz" HeaderText="m_zulkz">
                                            </asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_Fahrzeuge" SortExpression="m_Fahrzeuge"
                                                HeaderText="m_Fahrzeuge"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_versandadr_ZE" SortExpression="m_versandadr_ZE"
                                                HeaderText="m_versandadr_ZE"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_versandadr_ZS" SortExpression="m_versandadr_ZS"
                                                HeaderText="m_versandadr_ZS"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_versandadrtext" SortExpression="m_versandadrtext"
                                                HeaderText="m_versandadrtext"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_versicherung" SortExpression="m_versicherung"
                                                HeaderText="m_versicherung"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_material" SortExpression="m_material"
                                                HeaderText="m_material"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_schein" SortExpression="m_schein" HeaderText="m_schein">
                                            </asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_abckz" SortExpression="m_abckz" HeaderText="m_abckz">
                                            </asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_equ" SortExpression="m_equ" HeaderText="m_equ">
                                            </asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_kennz" SortExpression="m_kennz" HeaderText="m_kennz">
                                            </asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_tidnr" SortExpression="m_tidnr" HeaderText="m_tidnr">
                                            </asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_liznr" SortExpression="m_liznr" HeaderText="m_liznr">
                                            </asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_versgrund" SortExpression="m_versgrund"
                                                HeaderText="m_versgrund"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="m_versgrundText" SortExpression="m_versgrundText"
                                                HeaderText="m_versgrundText"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div id="dataFooter">
                                    <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px">» Absenden</asp:LinkButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
