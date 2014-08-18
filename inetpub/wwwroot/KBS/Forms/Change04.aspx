<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04.aspx.vb" Inherits="KBS.Change04"
    MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2011.3.1305.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function Count(text, long) {
            var maxlength = new Number(long); // Change number to your max length.
            if (document.getElementById('<%=txtFreitext.ClientID%>').value.length > maxlength) {
                text.value = text.value.substring(0, maxlength);
            }
        }
    </script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label">Bestellung Zentrallager</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active">
                                        Bitte geben Sie hier den gewünschten Artikel + Menge ein und drücken Sie hinzufügen.
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr class="formquery">
                                            <td colspan="4" class="firstLeft active">
                                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblKst" runat="server">Kostenstelle</asp:Label>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtKST" runat="server" Enabled="false" AutoPostBack="true" Width="100px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" colspan="2">
                                                <div style="margin-left: 10px">
                                                    <asp:Label ID="lblKSTText" runat="server" TabIndex="0" Visible="false"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Artikelbezeichnung:
                                            </td>
                                            <td class="firstLeft active">
                                                Menge
                                            </td>
                                            <td colspan="2" class="firstLeft active">
                                                Verpackungseinheit
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:DropDownList ID="ddlArtikel" Style="width: auto" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtMenge" Width="50px" runat="server" MaxLength="4"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender Enabled="true" ID="fteMenge" runat="server" TargetControlID="txtMenge"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td class="firstLeft active" style="width: 25%">
                                                <asp:Label ID="lblVerpEinheit" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="width: 100%">
                                                <asp:LinkButton ID="lbtnInsert" Height="16px" Width="78px" CssClass="Tablebutton"
                                                    runat="server">hinzufügen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="4" class="firstLeft active">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery" >
                                            <td colspan="4" class="firstLeft active">
                                                Freitextfeld für Anfragen:
                                            </td>
                                        </tr>
                                        <tr class="formquery"  runat="server" id="trtxtFreitext" >
                                            <td colspan="5" class="firstLeft active">
                                                <asp:TextBox ID="txtFreitext" runat="server" Width="480px" Height="125px" MaxLength="250"
                                                    onKeyUp="javascript:Count(this,250);" onChange="javascript:Count(this,250);"
                                                    Rows="10" TextMode="MultiLine"></asp:TextBox>
                                                <asp:LinkButton ID="lbtFreitextSend" Height="16px" Width="155px" CssClass="TablebuttonXLarge"
                                                    runat="server">Nur Freitext senden</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="4" class="firstLeft active">
                                                <asp:Label ID="lblMessage" CssClass="TextError" runat="server"></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="data">
                        <asp:UpdatePanel runat="server" ID="upGrid">
                            <ContentTemplate>
                                <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView CssClass="GridView" ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                                                AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None">
                                                <PagerSettings Visible="false" />
                                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                <RowStyle CssClass="ItemStyle" />
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMatnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ARTLIF") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Artikelbezeichnung" DataField="ARTBEZ" />
                                                    <asp:BoundField HeaderText="Menge" DataField="Menge" />
                                                    <asp:BoundField HeaderText="Verpackungseinheit" DataField="VMEINS" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lbDelete" runat="server" Width="20" Height="20" ImageUrl="~/Images/RecycleBin.png"
                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ARTLIF") %>' CommandName="entfernen" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                            <asp:LinkButton ID="lbLetzteBestellungen" Text="letzte Bestellungen" Height="16px" Width="128px" runat="server"
                                CssClass="TablebuttonLarge" style="margin-left: 10px"></asp:LinkButton>
                        </div>
                        <div id="divLetzteBestellungen" runat="server" Visible="False" style="margin-bottom: 10px">
                            <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td class="firstLeft active">
                                        Bestellungen in den letzten 4 Wochen:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNoDataLetzteBestellungen" runat="server" Visible="False" Font-Bold="True">Keine Daten gefunden</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView CssClass="GridView" ID="gvLetzteBestellungen" runat="server" Width="100%" AutoGenerateColumns="False"
                                            AllowPaging="False" AllowSorting="False" ShowFooter="False" GridLines="None">
                                            <PagerSettings Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Datum" DataField="Bestelldatum" DataFormatString="{0:dd.MM.yyyy}" />
                                                <asp:BoundField HeaderText="Bestellung Nr." DataField="BSTNR" />
                                                <asp:BoundField HeaderText="Artikel-Nr." DataField="ARTLIF" />
                                                <asp:BoundField HeaderText="Bezeichnung" DataField="ARTBEZ" />
                                                <asp:TemplateField HeaderText="Menge">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# IIf(Eval("MENGE") Is Nothing, "", Eval("MENGE").ToString().TrimStart("0"c)) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Freitext" DataField="FREITEXT" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:Button ID="MPEDummy" style="display: none" runat="server" />
                        <asp:Button ID="MPEDummy1" style="display: none" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="mpeBestellungsCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellungsCheck" TargetControlID="MPEDummy" BehaviorID="BestellCheck">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="BestellungsCheck" runat="server" Style="overflow: auto; height: 425px;
                            width: 600px; display: none">
                            <table cellspacing="0" id="tblBestellungscheck" runat="server" bgcolor="white" cellpadding="0"
                                style="overflow: auto; height: 425px; width: 583px; border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft active">
                                        Bitte überprüfen Sie Ihre Bestellung. Bitte korrigieren Sie gegebenenfalls!<br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridView2" runat="server" AllowPaging="False" AllowSorting="True"
                                            AutoGenerateColumns="False" BackColor="White" CssClass="GridView" GridLines="None"
                                            HorizontalAlign="Center" ShowFooter="False" Width="75%">
                                            <PagerSettings Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMatnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ARTLIF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ARTBEZ" HeaderText="Artikelbezeichnung" />
                                                <asp:BoundField DataField="Menge" HeaderText="Menge" />
                                                <asp:BoundField HeaderText="Verpackungseinheit" DataField="VMEINS" />
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
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
                                        <asp:LinkButton ID="lbBestellungKorrektur" Text="Korrektur" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                        &nbsp; &nbsp;
                                        <asp:LinkButton ID="lbBestellungOk" Text="Weiter" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <cc1:ModalPopupExtender runat="server" ID="MPEBestellResultat" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellResultat" TargetControlID="MPEDummy1">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="BestellResultat" HorizontalAlign="Center" runat="server" Style="display: none">
                            <table cellspacing="0" id="Table1" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                style="width: 50%; border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        Bestellstatus:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblBestellMeldung" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lbBestellFinalize" Text="ok" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
