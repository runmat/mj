<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WareneingangDetails.aspx.cs"
    Inherits="AppZulassungsdienst.forms.WareneingangDetails" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" runat="server" Visible="True" OnClick="lb_zurueck_Click" CausesValidation="false">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text=""></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <div id="statistics" style="margin-top: 0px;">
                            <table id="tblAnzeigeVersandDaten" runat="server" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2" width="100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Bestellnummer,Lieferant:
                                    </td>
                                    <td width="100%">
                                        <asp:Label ID="lblBestellnummerLieferant" runat="server"></asp:Label>
                                        <asp:Label ID="lblBelegnummer" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="TrLiefernr" runat="server">
                                    <td>
                                        Lieferscheinnummer:
                                    </td>
                                    <td width="100%">
                                        <asp:TextBox ID="txtLieferscheinnummer" runat="server" MaxLength="16"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="TrBelegdatum" runat="server">
                                    <td>
                                        Wareneingangsdatum:
                                    </td>
                                    <td width="100%">
                                        <div id="NeutralCalendar">
                                            <asp:TextBox ID="txtBelegdatum" runat="server"></asp:TextBox>
                                        </div>
                                        <cc1:CalendarExtender ID="txtBelegdatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtBelegdatum">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="meeBelegdatum" runat="server" TargetControlID="txtBelegdatum"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                        <cc1:MaskedEditValidator ID="mevBelegdatum" runat="server" ControlToValidate="txtBelegdatum"
                                            ControlExtender="meeBelegdatum" Display="none" IsValidEmpty="true" Enabled="true"
                                            EmptyValueMessage="Bitte geben Sie ein gültiges Belegdatum ein" InvalidValueMessage="Bitte geben Sie ein gültiges Belegdatum ein"></cc1:MaskedEditValidator>
                                        <cc1:ValidatorCalloutExtender Enabled="true" ID="vceBelegdatum" Width="350px" runat="server"
                                            HighlightCssClass="validatorCalloutHighlight" TargetControlID="mevBelegdatum">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" width="100%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div id="pagination">
                    </div>
                    <div id="data">
                        <asp:UpdatePanel runat="server" ID="upWareneingang">
                            <ContentTemplate>
                                <table cellspacing="0" runat="server" id="tblGrid1" visible="true" width="100%" cellpadding="0"
                                    bgcolor="white" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblError" runat="server" Visible="true" CssClass="TextError" EnableViewState="False"></asp:Label>
                                            <asp:Label ID="lblNoData" runat="server" Visible="true" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView CssClass="GridView" ID="GridView1" runat="server" PageSize="500" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ShowFooter="False"
                                                GridLines="None" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
                                                <PagerSettings Visible="false" />
                                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                <RowStyle CssClass="ItemStyle" />
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEAN" runat="server" Text='<%# Eval("Bestellposition") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="60">
                                                        <HeaderTemplate>
                                                            <asp:Image ID="imgbAllVollstaendig" Width="16px" Height="16px" ImageUrl="/PortalZLD/Images/Pfeil_Ablegen_02.jpg"
                                                                Style="cursor: pointer" runat="server" />
                                                            <asp:Image ID="imgbAlleUnvollstaendig" ImageUrl="/PortalZLD/Images/Delete01.jpg"
                                                                Style="cursor: pointer" runat="server" />
                                                            <asp:LinkButton ID="colVollstaendig" runat="server" CommandName="Sort" CommandArgument="PositionVollstaendig">Vollständig</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkVollstaendig" ToolTip="diese Position ist vollstaendig/unvollstaendig"
                                                                CausesValidation="true" Checked='<%# (Eval("PositionVollstaendig").ToString() == "X") %>'
                                                                runat="server" OnCheckedChanged="chkVollstaendig_CheckedChanged" AutoPostBack="True" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="60px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="ARTNR Lieferant" SortExpression="MaterialnummerLieferant"
                                                        DataField="MaterialnummerLieferant" />
                                                    <asp:BoundField HeaderText="EAN" DataField="EAN" SortExpression="EAN" />
                                                    <asp:BoundField HeaderText="Artikelbezeichnung" SortExpression="Artikelbezeichnung"
                                                        DataField="Artikelbezeichnung" />
                                                    <asp:TemplateField SortExpression="KennzForm">
                                                        <HeaderTemplate>
																<asp:LinkButton ID="lblKennzform" runat="server" CommandName="Sort" CommandArgument="KennzForm">Kennz.- <br />Größe </asp:LinkButton></HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblKennzForm" runat="server" Text='<%# Eval("KennzForm") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="45px" />
                                                        <ItemStyle Width="45px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Bestellmenge" HeaderStyle-Width="80px" SortExpression="BestellteMenge"
                                                        DataField="BestellteMenge">
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="colPositionLieferMenge" runat="server" CommandName="Sort" CommandArgument="PositionLieferMenge">gelieferte<br /> Menge</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtPositionLieferMenge" onKeyPress="return numbersonly(event, false)"
                                                                MaxLength="4" onFocus="Javascript:this.select();" Width="50px" Text='<%# Eval("PositionLieferMenge") %>'
                                                                runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Freitext">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Eval("Freitext")%>' 
                                                                ToolTip='<%# Eval("LangText")%>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="colPositionAbgeschlossen" runat="server" CommandName="Sort" CommandArgument="PositionAbgeschlossen">Lieferung<br /> abgeschlossen</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:RadioButton GroupName="Abgeschlossen" ID="rbPositionAbgeschlossenJA" runat="server"
                                                                Text="Ja" Checked='<%# (Eval("PositionAbgeschlossen").ToString() == "J") %>' />
                                                            &nbsp;
                                                            <asp:RadioButton ID="rbPositionAbgeschlossenNEIN" GroupName="Abgeschlossen" runat="server"
                                                                Text="Nein" Checked='<%# (Eval("PositionAbgeschlossen").ToString() == "N") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:Button ID="MPEDummy" Width="16px" Height="0" runat="server" Style="display: none" />
                    <asp:Button ID="MPEDummy2" Width="0" Height="0" runat="server" Style="display: none" />
                    <asp:Panel ID="WareneingangsbuchungResultat" HorizontalAlign="Center" runat="server"
                        Style="display: none">
                        <table cellspacing="0" id="Table1" runat="server" width="50%" bgcolor="white" cellpadding="0"
                            style="border: solid 1px #646464">
                            <tr>
                                <td class="firstLeft active">
                                    Buchungsstatus:
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active">
                                    <asp:Label ID="lblWareneingangsbuchungMeldung" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lbWareneingangsbuchungFinalize" Text="ok" Height="16px" Width="78px"
                                        runat="server" CssClass="Tablebutton" OnClick="lbWareneingangsbuchungFinalize_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="WareneingangsCheck" runat="server" Style="overflow: auto; height: 425px;
                        width: 750px; display: block">
                        <table cellspacing="0" runat="server" id="tblGrid2" style="overflow: auto; height: 226px;
                            width: 733px; border: solid 1px #646464" cellpadding="0" bgcolor="white" border="0">
                            <tr>
                                <td align="center" class="firstLeft active" style="height: 12px">
                                    Bitte überprüfen Sie Ihren Wareneingang
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 12px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 168px">
                                    <asp:GridView CssClass="GridView" ID="GridView2" runat="server" Width="100%" AutoGenerateColumns="False"
                                        AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None" BackColor="White"
                                        HorizontalAlign="Center" OnSorting="GridView2_Sorting">
                                        <PagerSettings Visible="false" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEAN" runat="server" Text='<%# Eval("Bestellposition") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="ARTNR Lieferant" SortExpression="MaterialnummerLieferant"
                                                DataField="MaterialnummerLieferant" />
                                            <asp:BoundField HeaderText="EAN" DataField="EAN" SortExpression="EAN" HeaderStyle-Width="25px" />
                                            <asp:BoundField HeaderText="Artikelbezeichnung" SortExpression="Artikelbezeichnung"
                                                DataField="Artikelbezeichnung" />
                                            <asp:BoundField HeaderText="Kennz.- Größe" SortExpression="KennzForm"
                                                DataField="KennzForm" />
                                            <asp:BoundField HeaderText="Bestellmenge" HeaderStyle-Width="80" SortExpression="BestellteMenge"
                                                DataField="BestellteMenge"></asp:BoundField>
                                            <asp:TemplateField HeaderText="gelieferte Menge">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLieferMenge" runat="server" Text='<%# Eval("PositionLieferMenge") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Mengeneinheit" DataField="Mengeneinheit" />
                                            <asp:TemplateField HeaderText="Lieferung abgeschlossen">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLieferungAbgeschlossenJA" runat="server" Text="Nein" Visible='<%# (Eval("PositionAbgeschlossen")=="N") %>' />
                                                    <asp:Label ID="lblLieferungAbgeschlossenNEIN" runat="server" Text="Ja" Visible='<%# ((Eval("PositionAbgeschlossen")=="J") || (Eval("PositionVollstaendig")=="X")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 12px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:LinkButton ID="lbWareneingangKorrektur" Text="Korrektur" Height="16px" Width="78px"
                                        runat="server" CssClass="Tablebutton" OnClick="lbWareneingangKorrektur_Click"></asp:LinkButton>&nbsp;
                                    &nbsp;
                                    <asp:LinkButton ID="lbWareneingangOk" Text="Absenden" Height="16px" Width="78px"
                                        runat="server" CssClass="Tablebutton" OnClick="lbWareneingangOk_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <cc1:ModalPopupExtender runat="server" ID="mpeWareneingangsCheck" BackgroundCssClass="divProgress"
                        Enabled="true" PopupControlID="WareneingangsCheck" TargetControlID="MPEDummy2">
                    </cc1:ModalPopupExtender>
                    <cc1:ModalPopupExtender runat="server" ID="MPEWareneingangsbuchungResultat" BackgroundCssClass="divProgress"
                        Enabled="true" PopupControlID="WareneingangsbuchungResultat" TargetControlID="MPEDummy">
                    </cc1:ModalPopupExtender>
                    <div id="dataFooter">
                        <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                            CssClass="Tablebutton" OnClick="lbAbsenden_Click"></asp:LinkButton></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
