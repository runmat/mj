<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_1.aspx.vb" Inherits="KBS.Change02_1"
    MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="JavaScript" type="text/javascript" src="../Java/JScript.js"></script>

    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Wareneingangsprüfung"></asp:Label>&nbsp;
                                <asp:Label ID="lblPageTitle" Text="" runat="server"></asp:Label>
                            </h1>
                        </div>
                        <div id="TableQuery">
                            <div id="statistics" style="margin-top: 0px;">
                                <table id="tblAnzeigeVersandDaten" cellpadding="0" cellspacing="0">
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
                                            <asp:TextBox ID="txtLieferscheinnummer" CssClass="TextBoxNormal" runat="server" MaxLength="16"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="TrBelegdatum" runat="server">
                                        <td>
                                            Wareneingangsdatum:
                                        </td>
                                        <td width="100%">
                                            <div id="NeutralCalendar">
                                                <asp:TextBox ID="txtBelegdatum" CssClass="TextBoxShort" runat="server"></asp:TextBox>
                                            </div>
                                            <cc1:CalendarExtender ID="txtBelegdatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtBelegdatum">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="meeBelegdatum" runat="server" TargetControlID="txtBelegdatum"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="mevBelegdatum" runat="server" ControlToValidate="txtBelegdatum"
                                                ControlExtender="meeBelegdatum" Display="none" IsValidEmpty="true" Enabled="true"
                                                EmptyValueMessage="Bitte geben Sie ein gültiges Belegdatum ein" InvalidValueMessage="Bitte geben Sie ein gültiges Belegdatum ein">
                                                                                                                   
                                            </cc1:MaskedEditValidator>
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
                        <asp:UpdatePanel runat="server" ID="upWareneingang">
                            <ContentTemplate>
                                <div id="pagination">
                                </div>
                                <div id="data">
                                    <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
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
                                                    GridLines="None">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEAN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bestellposition") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="60">
                                                            <HeaderTemplate>
                                                                <asp:Image ID="imgbAllVollstaendig" ImageUrl="~/Images/Pfeil_Ablegen_02.jpg" Style="cursor: pointer"
                                                                    runat="server" />
                                                                <asp:Image ID="imgbAlleUnvollstaendig" ImageUrl="~/Images/Delete01.jpg" Style="cursor: pointer"
                                                                    runat="server" />
                                                                <asp:LinkButton ID="colVollstaendig" runat="server" CommandName="Sort" CommandArgument="PositionVollstaendig">Vollständig</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkVollstaendig" ToolTip="diese Position ist vollstaendig/unvollstaendig"
                                                                    CausesValidation="true" Checked='<%# DataBinder.Eval(Container, "DataItem.PositionVollstaendig")="X" %>'
                                                                    runat="server" />
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
                                                            <asp:Label ID="lblKennzForm" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KennzForm") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="45px" />
                                                        <ItemStyle Width="45px" />
                                                    </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Bestellmenge" HeaderStyle-Width="80" SortExpression="BestellteMenge"
                                                            DataField="BestellteMenge">
                                                            <HeaderStyle Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="colPositionLieferMenge" runat="server" CommandName="Sort" CommandArgument="PositionLieferMenge">gelieferte Menge</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input id="txtPositionLieferMenge2" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.PositionLieferMenge") %>'
                                                                    type="hidden" />
                                                                <asp:TextBox ID="txtPositionLieferMenge" onKeyPress="return numbersonly(event, false)"
                                                                    MaxLength="4" onFocus="Javascript:this.select();" Width="50px" Text='<%# DataBinder.Eval(Container, "DataItem.PositionLieferMenge") %>'
                                                                    runat="server"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Mengeneinheit" DataField="Mengeneinheit" />
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="colPositionAbgeschlossen" runat="server" CommandName="Sort" CommandArgument="PositionAbgeschlossen">Lieferung abgeschlossen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:RadioButton GroupName="Abgeschlossen" ID="rbPositionAbgeschlossenJA" runat="server"
                                                                    Text="Ja" Checked='<%# DataBinder.Eval(Container, "DataItem.PositionAbgeschlossen")="J" %>' />
                                                                &nbsp;
                                                                <asp:RadioButton ID="rbPositionAbgeschlossenNEIN" GroupName="Abgeschlossen" runat="server"
                                                                    Text="Nein" Checked='<%# DataBinder.Eval(Container, "DataItem.PositionAbgeschlossen")="N" %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Panel ID="WareneingangsCheck" runat="server" Style="overflow: auto; height: 425px;
                            width: 680px; display: none">
                            <table cellspacing="0" id="tblWareneingangsCheck" runat="server" bgcolor="white"
                                cellpadding="0" style="overflow: auto; height: 425px; width: 653px; border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft active">
                                        Bitte überprüfen Sie Ihren Wareneingang
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView CssClass="GridView" ID="GridView2" runat="server" Width="100%" AutoGenerateColumns="False"
                                            AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None" BackColor="White"
                                            HorizontalAlign="Center">
                                            <PagerSettings Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEAN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bestellposition") %>'>
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
                                                        <asp:Label ID="lblLieferMenge" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PositionLieferMenge") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Mengeneinheit" DataField="Mengeneinheit" />
                                                <asp:TemplateField HeaderText="Lieferung abgeschlossen">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLieferungAbgeschlossenJA" runat="server" Text="Nein" Visible='<%# DataBinder.Eval(Container, "DataItem.PositionAbgeschlossen")="N" %>' />
                                                        <asp:Label ID="lblLieferungAbgeschlossenNEIN" runat="server" Text="Ja" Visible='<%# DataBinder.Eval(Container, "DataItem.PositionAbgeschlossen")="J" OR DataBinder.Eval(Container, "DataItem.PositionVollstaendig")="X" %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="lbWareneingangKorrektur" Text="Korrektur" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>&nbsp; &nbsp;
                                        <asp:LinkButton ID="lbWareneingangOk" Text="Absenden" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="MPEDummy" Width="0" Height="0" runat="server" />
                        <asp:Button ID="MPEDummy2" Width="0" Height="0" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="mpeWareneingangsCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="WareneingangsCheck" TargetControlID="MPEDummy2"
                            CancelControlID="lbWareneingangKorrektur">
                        </cc1:ModalPopupExtender>
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
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <cc1:ModalPopupExtender runat="server" ID="MPEWareneingangsbuchungResultat" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="WareneingangsbuchungResultat" TargetControlID="MPEDummy">
                        </cc1:ModalPopupExtender>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
