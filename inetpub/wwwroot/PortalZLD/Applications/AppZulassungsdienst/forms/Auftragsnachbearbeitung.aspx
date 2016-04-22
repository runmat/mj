<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Auftragsnachbearbeitung.aspx.cs" Inherits="AppZulassungsdienst.forms.Auftragsnachbearbeitung" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script type="text/javascript" src="../JavaScript/helper.js?22042016"></script>
    <script language="javascript" type="text/javascript">
        function checkZulassungsdatum() {
            var tb = document.getElementById('<%= txtStornoZulassungsdatum.ClientID %>');
            document.getElementById('<%= ihDatumIstWerktag.ClientID %>').value = nurWerktage(tb.value)[0];
            return true;
        }
    </script>

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="margin-top: 25px">
                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                            </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="width: 20%">
                                                    <asp:Label ID="lblSucheAuftragsnummer" runat="server">Auftragsnummer:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 80%">
                                                    <asp:TextBox ID="txtSucheAuftragsnummer" runat="server" MaxLength="10" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="width: 20%">
                                                    <asp:Label ID="lblSucheId" runat="server">ID:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 80%">
                                                    <asp:TextBox ID="txtSucheId" runat="server" MaxLength="10" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>                                          
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton
                                                        ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" onclick="cmdCreate_Click" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                            </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="cmdZurSuche" runat="server" CssClass="TablebuttonLarge" 
                                    Width="128px" Height="16px" onclick="cmdZurSuche_Click" Visible="False">» Zur Suche </asp:LinkButton>
                                <asp:LinkButton ID="cmdOffeneStornos" runat="server" CssClass="TablebuttonLarge" 
                                    Width="128px" Height="16px" onclick="cmdOffeneStornos_Click" style="margin-left: 4px">» Offene Stornos </asp:LinkButton>
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdCreate_Click">» Weiter </asp:LinkButton>
                            </div>
                            <div id="data">
                                <div id="OffeneStornos" runat="server" Visible="False" style="margin-top: 10px">
                                    <telerik:RadGrid ID="rgOffeneStornos" runat="server" AllowSorting="False" AllowPaging="False"
                                        AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default" 
                                        OnNeedDataSource="rgOffeneStornos_NeedDataSource" OnItemCommand="rgOffeneStornos_ItemCommand">
                                        <ItemStyle CssClass="ItemStyle" />
                                        <AlternatingItemStyle CssClass="ItemStyle" />
                                        <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="False">
                                            <SortExpressions>
                                                <telerik:GridSortExpression FieldName="ZULBELN" SortOrder="Ascending" />
                                            </SortExpressions>
                                            <HeaderStyle ForeColor="#595959" />
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="ZULBELN" HeaderText="ID"/>
                                                <telerik:GridBoundColumn DataField="Erfasst" HeaderText="Erfasst" ItemStyle-Wrap="False"/>
                                                <telerik:GridBoundColumn DataField="VE_ERNAM" HeaderText="Erfasser"/>
                                                <telerik:GridBoundColumn DataField="Kunde" HeaderText="Kunde"/>
                                                <telerik:GridBoundColumn DataField="ZZZLDAT" HeaderText="Zulassungsdatum" DataFormatString="{0:dd.MM.yyyy}"/>
                                                <telerik:GridBoundColumn DataField="ZZKENN" HeaderText="Kennzeichen"/>
                                                <telerik:GridBoundColumn DataField="ZULBELN_ALT" HeaderText="alte ID"/>
                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle Width="40px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" Width="32" Height="32" ImageUrl="/PortalZLD/Images/Edit_01.gif"
                                                            CommandName="nachbearbeiten" ToolTip="nachbearbeiten" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                                <div id="VorgangInfo" runat="server" Visible="False" style="margin-top: 10px">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr class="formquery" style="height: 26px">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblID" runat="server">ID:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:Label ID="lblIDDisplay" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="height: 26px">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblAuftragsnummer" runat="server">Auftrag:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:Label ID="lblAuftragsnummerDisplay" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="height: 26px">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblKundennummer" runat="server">Kundennr.:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:Label ID="lblKundennummerDisplay" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="height: 26px">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblKunde" runat="server">Kunde:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:Label ID="lblKundeDisplay" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="height: 26px">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblReferenz1" runat="server">Referenz 1:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:Label ID="lblReferenz1Display" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="height: 26px">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblZulassungsdatum" runat="server">Zulassungsdatum:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:Label ID="lblZulassungsdatumDisplay" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="height: 26px">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblKennzeichen" runat="server">Kennzeichen:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:Label ID="lblKennzeichenDisplay" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="height: 26px">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblStatus" runat="server">Status:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:Label ID="lblStatusDisplay" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trVorgangPositionenDisplay" runat="server" class="formquery">
                                            <td class="firstLeft active" style="width: 20%; vertical-align: top">
                                                <asp:Label ID="lblPositionen" runat="server">Positionen:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <telerik:RadGrid ID="rgPositionenDisplay" runat="server" AllowSorting="False" AllowPaging="False"
                                                    AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default" 
                                                    OnNeedDataSource="rgPositionenDisplay_NeedDataSource">
                                                    <ItemStyle CssClass="ItemStyle" />
                                                    <AlternatingItemStyle CssClass="ItemStyle" />
                                                    <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="False">
                                                        <SortExpressions>
                                                            <telerik:GridSortExpression FieldName="PositionsNr" SortOrder="Ascending" />
                                                        </SortExpressions>
                                                        <HeaderStyle ForeColor="#595959" />
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="PositionsNr" Visible="False"/>
                                                            <telerik:GridBoundColumn DataField="MaterialNr" Visible="False"/>
                                                            <telerik:GridBoundColumn DataField="MaterialName" HeaderText="Dienstleistung" HeaderStyle-Width="40%"/>
                                                            <telerik:GridTemplateColumn HeaderText="Preis" HeaderStyle-Width="12%">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Eval("WebMaterialart").ToString() == "D" ? Eval("Preis", "{0:F}") : "" %>'/>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Gebühr" HeaderStyle-Width="12%">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Eval("WebMaterialart").ToString() == "G" ? Eval("Preis", "{0:F}") : "" %>'/>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="GebuehrAmt" HeaderText="Gebühr Amt" HeaderStyle-Width="12%"/>
                                                            <telerik:GridTemplateColumn HeaderText="Steuer" HeaderStyle-Width="12%">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Eval("WebMaterialart").ToString() == "S" ? Eval("Preis", "{0:F}") : "" %>'/>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Preis Kennz." HeaderStyle-Width="12%">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Eval("WebMaterialart").ToString() == "K" ? Eval("Preis", "{0:F}") : "" %>'/>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="StornoDetails" runat="server" Visible="False" style="margin-top: 10px">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr class="formquery" style="height: 32px">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblStornogrund" runat="server">Grund für Storno:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:DropDownList runat="server" ID="ddlStornogrund" AutoPostBack="True" OnSelectedIndexChanged="ddlStornogrund_SelectedIndexChanged"/>
                                            </td>
                                        </tr>
                                        <tr id="trStornoKundennummer" runat="server" class="formquery" style="height: 32px" Visible="False">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblStornoKundennummer" runat="server">Neuer Kunde:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtStornoKundennummer" runat="server" onKeyPress="return numbersonly(event, false)" 
                                                    CssClass="TextBoxNormal" MaxLength="8" Width="75px"></asp:TextBox>
                                                <asp:DropDownList ID="ddlStornoKunde" runat="server" AutoPostBack="True" 
                                                    OnSelectedIndexChanged="ddlStornoKunde_SelectedIndexChanged" Style="width: 375px"/>
                                            </td>
                                        </tr>
                                        <tr id="trStornoBegruendung" runat="server" class="formquery" style="height: 32px" Visible="False">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblStornoBegruendung" runat="server">Begründung:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtStornoBegruendung" runat="server" CssClass="TextBoxNormal" Width="99%" MaxLength="100"/>
                                            </td>
                                        </tr>
                                        <tr id="trStornoStva" runat="server" class="formquery" style="height: 32px" Visible="False">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblStornoStva" runat="server">Neues Amt:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtStornoAmt" runat="server" CssClass="TextBoxNormal" Width="45px" MaxLength="3" style="text-transform:uppercase;"/>
                                            </td>
                                        </tr>
                                        <tr id="trStornoKennzeichen" runat="server" class="formquery" style="height: 32px" Visible="False">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblStornoKennzeichen" runat="server">Neues Kennzeichen:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtStornoKennz1" MaxLength="3" CssClass="TextBoxNormal" Width="45px" runat="server" style="text-transform:uppercase;"/>
                                                <span style="padding-right: 2px; padding-left: 2px">-</span>
                                                <asp:TextBox ID="txtStornoKennz2" MaxLength="6" CssClass="TextBoxNormal" Width="100px" runat="server" style="text-transform:uppercase;"/>
                                            </td>
                                        </tr>
                                        <tr id="trStornoZulassungsdatum" runat="server" class="formquery" style="height: 32px" Visible="False">
                                            <td class="firstLeft active" style="width: 20%">
                                                <asp:Label ID="lblStornoZulassungsdatum" runat="server">Neues Zulassungsdatum:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtStornoZulassungsdatum" runat="server" CssClass="TextBoxNormal" Width="65px" MaxLength="6"/>
                                                <asp:Label ID="txtStornoZulassungsdatumFormat" Style="padding-left: 2px; font-weight: normal"
                                                    Height="15px" runat="server">(ttmmjj)</asp:Label>
                                                <asp:LinkButton runat="server" Style="padding-left: 10px; font-weight: normal" Height="15px"
                                                    ID="lbtnGestern" Text="Gestern |" Width="60px" />
                                                <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnHeute"
                                                    Width="50px" Text="Heute |" />
                                                <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnMorgen"
                                                    Width="60px" Text="Morgen" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="EditPreise" runat="server" Visible="False" style="margin-top: 10px">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="width: 20%; vertical-align: top">
                                                <asp:Label ID="lblPositionenEdit" runat="server">Positionen:</asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <telerik:RadGrid ID="rgPositionenEdit" runat="server" AllowSorting="False" AllowPaging="False"
                                                    AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default" 
                                                    OnNeedDataSource="rgPositionenEdit_NeedDataSource">
                                                    <ItemStyle CssClass="ItemStyle" />
                                                    <AlternatingItemStyle CssClass="ItemStyle" />
                                                    <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="False">
                                                        <SortExpressions>
                                                            <telerik:GridSortExpression FieldName="PositionsNr" SortOrder="Ascending" />
                                                        </SortExpressions>
                                                        <HeaderStyle ForeColor="#595959" />
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="PositionsNr" Visible="False"/>
                                                            <telerik:GridBoundColumn DataField="MaterialNr" Visible="False"/>
                                                            <telerik:GridBoundColumn DataField="MaterialName" HeaderText="Dienstleistung" HeaderStyle-Width="40%"/>
                                                            <telerik:GridTemplateColumn HeaderText="Preis" HeaderStyle-Width="12%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPreis" runat="server" Enabled='<%# Eval("WebMaterialart").ToString() == "D" %>' CssClass="TextBoxNormal"
														                onKeyPress="return numbersonly(event, true)" Width="45" Font-Size="8pt" Text='<%# Eval("WebMaterialart").ToString() == "D" ? Eval("Preis", "{0:F}") : "" %>'/>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Gebühr" HeaderStyle-Width="12%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGebuehr" runat="server" Enabled='<%# Eval("WebMaterialart").ToString() == "G" %>' CssClass="TextBoxNormal"
														                onKeyPress="return numbersonly(event, true)" Width="45" Font-Size="8pt" Text='<%# Eval("WebMaterialart").ToString() == "G" ? Eval("Preis", "{0:F}") : "" %>'/>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Gebühr Amt" HeaderStyle-Width="12%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGebuehrAmt" runat="server" CssClass="TextBoxNormal"
														                onKeyPress="return numbersonly(event, true)" Width="45" Font-Size="8pt" Text='<%# Eval("GebuehrAmt", "{0:F}") %>' Enabled='<%# Eval("WebMaterialart").ToString() != "D" %>'/>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Steuer" HeaderStyle-Width="12%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtSteuer" runat="server" Enabled='<%# Eval("WebMaterialart").ToString() == "S" %>' CssClass="TextBoxNormal"
														                onKeyPress="return numbersonly(event, true)" Width="45" Font-Size="8pt" Text='<%# Eval("WebMaterialart").ToString() == "S" ? Eval("Preis", "{0:F}") : "" %>'/>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Preis Kennz." HeaderStyle-Width="12%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPreisKennz" runat="server" Enabled='<%# Eval("WebMaterialart").ToString() == "K" %>' CssClass="TextBoxNormal"
														                onKeyPress="return numbersonly(event, true)" Width="45" Font-Size="8pt" Text='<%# Eval("WebMaterialart").ToString() == "K" ? Eval("Preis", "{0:F}") : "" %>'/>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                    </table>
                                </div>
                            </div>
                            <div id="dataFooter">
                                <asp:LinkButton ID="cmdAbbrechen" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdAbbrechen_Click" Visible="False">» Abbrechen </asp:LinkButton>
                                <asp:LinkButton ID="cmdStorno" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdStorno_Click" OnClientClick="checkZulassungsdatum();" Visible="False">» Stornieren </asp:LinkButton>
                                <asp:LinkButton ID="cmdAbsenden" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdAbsenden_Click" Visible="False">» Absenden </asp:LinkButton>
                            </div>
                            <div>
                                <asp:Button ID="btnDummy" runat="server" style="display: none"/>
                                <ajaxToolkit:ModalPopupExtender ID="mpeConfirmPreisminderung" runat="server" TargetControlID="btnDummy"
                                    PopupControlID="pConfirmPreisminderung" BackgroundCssClass="divProgress" CancelControlID="btnPanelConfirmPreisminderungCancel">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="pConfirmPreisminderung" runat="server" Style="overflow: auto; display: none;">
                                    <table id="Table1" cellspacing="0" runat="server" bgcolor="white" cellpadding="0" style="overflow: auto;
                                        min-height: 100px; min-width: 100px; border: solid 1px #646464; padding: 10px 10px 15px 0px;">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="firstLeft" style="padding-left: 15px; padding-right: 15px;">
                                                Achtung! Sie reduzieren den Preis. Bitte mit OK bestätigen
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft" align="center">
                                                <asp:LinkButton ID="btnPanelConfirmPreisminderungCancel" runat="server" Text="Abbruch" Height="16px"
                                                    Width="78px" CssClass="Tablebutton" CausesValidation="false" OnClick="btnPanelConfirmPreisminderungCancel_Click" />
                                                <asp:LinkButton ID="btnPanelConfirmPreisminderungOK" runat="server" Text="OK" Height="16px" Width="78px"
                                                    CssClass="Tablebutton" CausesValidation="false" OnClick="btnPanelConfirmPreisminderungOK_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Button ID="btnDummy2" Width="16px" Height="0" runat="server" Style="display: none" />
                                <ajaxToolkit:ModalPopupExtender runat="server" ID="MPEBarquittungen" BackgroundCssClass="ui-widget-overlay"
                                    Enabled="true" PopupControlID="pnlPrintBar" TargetControlID="btnDummy2">
                                </ajaxToolkit:ModalPopupExtender>

                                <asp:Panel ID="pnlPrintBar" runat="server" Style="overflow: auto; height: 100px;
                                    width: 400px; display: none;" >
                                    <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix" style="width: 95%;" >
                                                              <asp:LinkButton ID="cmdClose" runat="server" 
                                        Width="10px" onclick="cmdClose_Click"  style="float:right" 
						               >X</asp:LinkButton>  
                                    </div>
                                    <asp:GridView ID="GridView2" GridLines="None" Style="border: 1px solid #dfdfdf; width: 96%;
                                        font-size: 9px; color: #595959" runat="server" BackColor="White" AutoGenerateColumns="False" OnRowCommand="GridView2_RowCommand"
                                        CaptionAlign="Left">
                                        <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Quittung">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("Filename") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Aufrufen">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="cmdPrint" CommandName="Print" CommandArgument='<%# Eval("Path") %>'
                                                        runat="server" ImageUrl="/PortalZLD/Images/iconPDF.gif" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="40px" />
                                                <ItemStyle Width="40px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                            <input type="hidden" runat="server" id="ihDatumIstWerktag" value="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
