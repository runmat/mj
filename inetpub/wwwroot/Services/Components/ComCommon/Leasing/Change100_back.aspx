<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change100.aspx.vb" Inherits="CKG.Components.ComCommon.Change100"
    MasterPageFile="/services/MasterPage/Services.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/services/PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Watermark
        {
            color: Gray;
        }
    </style>
    <div id="site">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="ibtnUpload" />
                <asp:PostBackTrigger ControlID="ibtnCreatePDF" />
            </Triggers>
            <ContentTemplate>
                <div id="content" style="padding-bottom: 15px">
                    <div class="divPopupBack" runat="server" visible="false" id="divBackDisabled">
                    </div>
                    <div class="divPopupDetail" runat="server" visible="false" id="divOptions" style="width: 400px">
                        <table class="PopupDetailTable">
                            <tr>
                                <td align="left">
                                    <h3>
                                        <asp:Label ID="Label5" runat="server" Text="Versandoptionen" Font-Bold="True"></asp:Label></h3>
                                </td>
                                <td align="right">
                                    <h5>
                                        <asp:LinkButton ID="lbtnCloseOption" runat="server">X</asp:LinkButton></h5>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Label ID="lblErrPopUp" runat="server" CssClass="TextError" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:CheckBoxList CellPadding="3" class="ListGruendeTable" ID="chkListGruende" Width="100%"
                                        runat="server" AutoPostBack="true" Visible="false">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:GridView ID="grvDL" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                        ShowHeader="False" Width="100%" CssClass="ListGruendeTable">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDL" runat="server" Text='<%# Bind("EAN11") %>' Visible="false"></asp:Label>
                                                    <asp:CheckBox ID="cbxDL" runat="server"></asp:CheckBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ASKTX" />
                                            <asp:BoundField DataField="TBTWR" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="AbstandRechts"
                                                ItemStyle-Width="60px" DataFormatString="{0:C}">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtDL" runat="server" ImageUrl="/Services/Images/info.gif" Visible='<%# DataBinder.Eval(Container, "DataItem.Description") <> "" %>'
                                                        ToolTip='<%# DataBinder.Eval(Container, "DataItem.Description") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:LinkButton ID="lbtnSelectGruende" runat="server" CssClass="TablebuttonMiddle"
                                        Width="100px" Height="16px">» Übernehmen</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="navigationSubmenu">
                        <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                            Text="Zurück" Visible="false"></asp:LinkButton>
                    </div>
                    <div id="innerContent">
                        <div id="innerContentRight" style="width: 100%">
                            <div>
                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label><asp:HiddenField
                                    ID="hdnField" runat="server" />
                            </div>
                            <div class="DivVersandTabContainer">
                                <asp:LinkButton ID="lbtnStammdaten" CssClass="VersandButtonStamm" runat="server"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnAdressdaten" CssClass="VersandButtonAdresseEnabled" Enabled="false"
                                    runat="server" Width="120px"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnVersanddaten" CssClass="VersandButtonOptionenEnabled" Enabled="false"
                                    runat="server"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnOverview" CssClass="VersandButtonOverviewEnabled" Enabled="false"
                                    runat="server"></asp:LinkButton>
                                <div class="DivPanelSteps">
                                    <table width="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="right" nowrap="nowrap">
                                                <asp:Label ID="lblSteps" runat="server">Schritt 1 von 4</asp:Label>
                                            </td>
                                            <td class="PanelHeadSteps">
                                                <asp:Panel ID="Panel1" CssClass="StepActive" runat="server">
                                                </asp:Panel>
                                            </td>
                                            <td class="PanelHeadSteps">
                                                <asp:Panel ID="Panel2" CssClass="Steps" runat="server">
                                                </asp:Panel>
                                            </td>
                                            <td class="PanelHeadSteps">
                                                <asp:Panel ID="Panel3" CssClass="Steps" runat="server">
                                                </asp:Panel>
                                            </td>
                                            <td class="PanelHeadSteps" style="padding-right: 5px;">
                                                <asp:Panel ID="Panel4" CssClass="Steps" runat="server">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="VersandTabPanel1" runat="server" class="VersandTabPanel" style="height: 600px;
                                margin-bottom: 10px">
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="padding-bottom: 0px;" class="PanelHead">
                                            <asp:Label ID="lbl_Fahrzeugdaten" runat="server">Schlüsselauswahl</asp:Label>
                                        </td>
                                        <td style="width: 100%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-top: 0px;">
                                            <asp:Label ID="Label6" runat="server">Bitte wählen Sie den für die Versendung vorgesehenen Schlüssel aus.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-left: 7px; width: 50%">
                                            <div id="divSearch" class="PanelHeadSuche" runat="server" style="cursor: pointer"
                                                onclick="javascript:cpeAllDataCollapsed()">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label4" runat="server">Suche</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ibtnFrage" Style="padding-right: 0px;" ToolTip="Bitte geben Sie die Suchkriterien ein und klicken zur Suche auf die Lupe!"
                                                                runat="server" ImageUrl="/services/Images/fragezeichen.gif" />
                                                            <asp:ImageButton ID="NewSearch" CssClass="PanelHeadSucheImg" runat="server" ImageUrl="/services/Images/versand/plusgreen.png"
                                                                OnClientClick="javascript:cpeAllDataCollapsed()" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="pnlAllgDaten" Style="padding-left: 15px" runat="server" DefaultButton="ImageButton2">
                                                <div style="background-image: url(../../../Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                                    height: 8px; width: 16px">
                                                </div>
                                                <div style="height: 200px">
                                                    <table id="tblSearch" runat="server" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Leasingvertragsnummer" runat="server">lbl_Leasingvertragsnummer</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLeasingvertragsnummer" runat="server" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtKennz" Width="300px" runat="server"></asp:TextBox>
                                                                <cc1:TextBoxWatermarkExtender ID="txtKennz_TextBoxWatermarkExtender" runat="server"
                                                                    Enabled="True" TargetControlID="txtKennz" WatermarkCssClass="Watermark" WatermarkText="z.B. HH-TZ589">
                                                                </cc1:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFahrgestellnummer" Width="300px" runat="server" MaxLength="17"></asp:TextBox>
                                                                <cc1:TextBoxWatermarkExtender ID="txtFahrgestellnummer_TextBoxWatermarkExtender"
                                                                    runat="server" Enabled="True" TargetControlID="txtFahrgestellnummer" WatermarkCssClass="Watermark"
                                                                    WatermarkText="z.B. WVW2323KJKJN223J">
                                                                </cc1:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td class="First" style="width: 250px">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_ZBIINummer" runat="server">lbl_ZBIINummer</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtZBIINummer" runat="server" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_Referenz1" runat="server">
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Referenznummer1" runat="server">lbl_Referenznummer1 </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReferenznummer1" runat="server" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_Referenz2" runat="server">
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Referenznummer2" runat="server">lbl_Referenznummer2</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReferenznummer2" runat="server" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblSearchExtend" runat="server" cellspacing="0" cellpadding="0" visible="false"
                                                        style="vertical-align: top">
                                                        <tr id="tr_Brieflieferant" runat="server">
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Brieflieferant" runat="server">lbl_Brieflieferant</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlBrieflieferant" runat="server" Width="310px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Restlaufzeit" runat="server">lbl_Restlaufzeit</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRestlaufzeit" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Abmeldedatum" runat="server">lbl_Abmeldedatum</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAbmeldedatumVon" runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtAbmeldedatumBis" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="txtAbmeldedatumVon_CalendarExtender" runat="server" TargetControlID="txtAbmeldedatumVon">
                                                                </cc1:CalendarExtender>
                                                                <cc1:CalendarExtender ID="txtAbmeldedatumBis_CalendarExtender" runat="server" TargetControlID="txtAbmeldedatumBis">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Abmeldeauftrag" runat="server">lbl_Abmeldeauftrag</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAbmeldeauftragVon" runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtAbmeldeauftragBis" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="txtAbmeldeauftragVon_CalendarExtender" runat="server" TargetControlID="txtAbmeldeauftragVon">
                                                                </cc1:CalendarExtender>
                                                                <cc1:CalendarExtender ID="txtAbmeldeauftragBis_CalendarExtender" runat="server" TargetControlID="txtAbmeldeauftragBis">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Abgemeldet" runat="server">lbl_Abgemeldet </asp:Label>
                                                            </td>
                                                            <td>
                                                                <span>
                                                                    <asp:CheckBox ID="chkAbgemeldet" runat="server" />
                                                                </span>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Zulassungsdatum" runat="server">lbl_Zulassungsdatum</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtZulassungsdatumVon" runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtZulassungsdatumBis" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="txtZulassungsdatumVon_CalendarExtender" runat="server"
                                                                    TargetControlID="txtZulassungsdatumVon">
                                                                </cc1:CalendarExtender>
                                                                <cc1:CalendarExtender ID="txtZulassungsdatumBis_CalendarExtender" runat="server"
                                                                    TargetControlID="txtZulassungsdatumBis">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td style="float: right; padding-right: 5px; width: 100%" align="right">
                                                            <asp:Label ID="lbl_ExtendSearch" runat="server" Width="1">lbl_ExtendSearch</asp:Label>
                                                            <asp:ImageButton ID="ibtBack" runat="server" ImageUrl="/Services/images/cancel.png"
                                                                ToolTip="Abbrechen" Visible="false" />
                                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="/Services/images/Adresspflege/SucheSmall.png"
                                                                ToolTip="Suchen" />
                                                            <asp:ImageButton ID="ibtExtendSearch" runat="server" ImageUrl="/Services/images/ErweiterteSelektion.png"
                                                                ToolTip="Erweiterte Selektion" Style="padding-left: 5px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <div id="divUpload" class="StandardHeadDetail" runat="server" style="cursor: pointer"
                                                onclick="javascript:cpeUploadCollapsed()">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label9" Style="padding-top: 3px;" runat="server" ForeColor="White"
                                                                Font-Size="12px" Font-Bold="True">Dateiupload</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" Style="padding-right: 7px; padding-top: 3px"
                                                                ImageUrl="/services/Images/versand/plusgreen.png" OnClientClick="javascript:cpeUploadCollapsed()" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel BackColor="#DCDCDC" ID="PLUpload" runat="server" Width="100%">
                                                <div class="StandardHeadDetailFlag">
                                                </div>
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_Datei" runat="server">lbl_Datei </asp:Label>
                                                        </td>
                                                        <td style="width: 25%" nowrap="nowrap">
                                                            <input id="upFile1" type="file" size="35" name="File1" runat="server" style="background-color: #FFFFFF" />
                                                            <a href="javascript:openinfo('Info01.htm');">
                                                                <img src="/Services/Images/info.gif" border="0" height="16px" width="16px" title="Struktur Uploaddatei" /></a>
                                                        </td>
                                                        <td style="width: 45%">
                                                            <asp:ImageButton ID="ibtnUpload" ToolTip="Suchen" ImageUrl="/Services/images/Adresspflege/SucheSmall.png"
                                                                runat="server" />
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" style="padding-top: 0px;">
                                            <asp:Label ID="lblErrorDokumente" CssClass="TextError" runat="server" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Result" visible="false" style="padding-right: 5px; padding-left: 7px" runat="Server">
                                    <div id="divDokuAusgabe" class="StandardHeadDetail" runat="server" style="cursor: pointer"
                                        onclick="javascript:cpeDokuAusgabeCollapsed()">
                                        <table width="100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="padding-left: 8px; padding-bottom: 0px;">
                                                    <asp:Label ID="Label20" Style="padding-top: 3px;" runat="server" ForeColor="White"
                                                        Font-Size="12px" Font-Bold="True">Schlüsselauswahl</asp:Label>
                                                </td>
                                                <td align="right" valign="top" style="padding-bottom: 0px;">
                                                    <asp:ImageButton ID="ImageButton3" runat="server" Style="padding-right: 7px; padding-top: 3px"
                                                        ImageUrl="../../../Images/versand/plusgreen.png" OnClientClick="javascript:cpeDokuAusgabeCollapsed()" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:Panel ID="pnlDokuAusgabe" runat="server" Width="100%" style="overflow: hidden;">                                    
                                        <div style="overflow: visible; height: 370px; width: 100%;"> 
                                            <div id="pagination">
                                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                            </div>
                                             <div id="data" runat="server" style="overflow-x: scroll; overflow-y: visible; height: 350px;
                                                width: 895px; scrollbar-face-color: #dfdfdf; scrollbar-shadow-color: #9B9B9B;
                                                scrollbar-highlight-color: #9B9B9B; scrollbar-3dlight-color: #dfdfdf; scrollbar-darkshadow-color: #dfdfdf;
                                                scrollbar-track-color: #9B9B9B; scrollbar-arrow-color: #9B9B9B;"><br>
                                                <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEQUNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Auswahl">
                                                            <HeaderTemplate>
                                                                <asp:ImageButton ID="ibtAuswahl" runat="server" Height="12px" ImageUrl="/services/images/haken_gruen.gif"
                                                                    OnClick="ibtAuswahl_Click" Width="12px" ToolTip="Alle auswählen" Style="padding-left: 4px" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle Width="30px" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkAnfordern" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Selected") = "1" %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Leasingnummer" HeaderText="col_Leasingnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Leasingnummer" runat="server" CommandName="Sort" CommandArgument="Leasingnummer">col_Leasingnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblLeasingnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnummer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="NummerZBII" HeaderText="col_NummerZBII">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_NummerZBII" runat="server" CommandName="Sort" CommandArgument="NummerZBII">col_NummerZBII</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblNummerZBII" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Referenz1" HeaderText="col_Referenz1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="Referenz1">col_Referenz1</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblReferenz1" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz1") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Referenz2" HeaderText="col_Referenz2">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="Referenz2">col_Referenz2</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblReferenz2" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz2") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Status" HeaderText="col_Status">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="Referenz2">col_Status</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblStatus" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Bemerkung" HeaderText="col_Bemerkung">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Bemerkung" runat="server" CommandName="Sort" CommandArgument="Bemerkung">col_Bemerkung</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblBemerkung" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Abmeldedatum" HeaderText="col_Abmeldedatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="Abmeldedatum">col_Abmeldedatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblAbmeldedatum" Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum","{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="CoC" HeaderText="col_CoC">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_CoC" runat="server" CommandName="Sort" CommandArgument="CoC">col_CoC</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblCoC" Text='<%# DataBinder.Eval(Container, "DataItem.CoC") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Brieflieferant" HeaderText="col_Brieflieferant">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Brieflieferant" runat="server" CommandName="Sort" CommandArgument="Brieflieferant">col_Brieflieferant</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblBrieflieferant" Text='<%# DataBinder.Eval(Container, "DataItem.Brieflieferant") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Restlaufzeit" HeaderText="col_Restlaufzeit">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Restlaufzeit" runat="server" CommandName="Sort" CommandArgument="Restlaufzeit">col_Restlaufzeit</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblRestlaufzeit" Text='<%# DataBinder.Eval(Container, "DataItem.Restlaufzeit") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <div class="paginationGrid">
                                                <uc2:GridNavigation ID="GridNavigation2" runat="server"></uc2:GridNavigation>
                                            </div>
                                            <div id="data2" runat="server" class="dataGrid GridRowHeight" style="overflow-x: scroll; overflow-y: visible; height: 350px;">
                                                <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView2"
                                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="Bemerkung" HeaderText="col_Bemerkung">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Bemerkung" runat="server" CommandName="Sort" CommandArgument="Bemerkung">col_Bemerkung</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblBemerkung" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Leasingnummer" HeaderText="col_Leasingnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Leasingnummer" runat="server" CommandName="Sort" CommandArgument="Leasingnummer">col_Leasingnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblLeasingnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnummer") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="NummerZBII" HeaderText="col_NummerZBII">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_NummerZBII" runat="server" CommandName="Sort" CommandArgument="NummerZBII">col_NummerZBII</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblNummerZBII" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Referenz1" HeaderText="col_Referenz1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="Referenz1">col_Referenz1</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblReferenz1" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz1") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Referenz2" HeaderText="col_Referenz2">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="Referenz2">col_Referenz2</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblReferenz2" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz2") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Versanddatum" HeaderText="col_Versanddatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versanddatum" runat="server" CommandName="Sort" CommandArgument="Versanddatum">col_Versanddatum </asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblVersanddatum" Text='<%# DataBinder.Eval(Container, "DataItem.Versanddatum") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Status" HeaderText="col_Status">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="Referenz2">col_Status</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblStatus" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Versandadresse" HeaderText="col_Versandadresse">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versandadresse" runat="server" CommandName="Sort" CommandArgument="Versandadresse">col_Versandadresse</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblVersandadresse" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Abmeldedatum" HeaderText="col_Abmeldedatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="Abmeldedatum">col_Abmeldedatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblAbmeldedatum" Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum","{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="CoC" HeaderText="col_CoC">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_CoC" runat="server" CommandName="Sort" CommandArgument="CoC">col_CoC</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblCoC" Text='<%# DataBinder.Eval(Container, "DataItem.CoC") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>                                            
                                        </div>
                                        <%--<table width="100%">
                                            <tr>
                                                <td align="right" >
                                                    <asp:LinkButton ID="ibtNext" runat="server" CssClass="TablebuttonLarge" Width="130px"
                                                        Height="16px">» Weiter</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table> --%>
                                        <div style="padding: 15px 0px 5px 0px; text-align:right; height:40px;"><asp:LinkButton ID="ibtNext" runat="server" CssClass="TablebuttonLarge" Width="130px"
                                                        Height="16px" >» Weiter</asp:LinkButton></div>                                       
                                    </asp:Panel>
                                </div>
                            </div>
                            <div id="VersandTabPanel2" class="VersandTabPanel" runat="server" visible="false"
                                style="height: 600px; margin-bottom: 10px">
                                <div style="height: 545px">
                                    <table cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="padding-bottom: 0px; width: 200%" class="PanelHead">
                                                <asp:Label ID="Label10" runat="server">Adresse auswählen</asp:Label>
                                            </td>
                                            <td style="width: 100%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="padding-top: 0px;">
                                                <asp:Label ID="Label12" runat="server">Bitte wählen Sie eine Empfängeradresse aus.</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="padding-left: 7px; width: 50%">
                                                <div class="PanelHeadSuche">
                                                    <table width="100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                                <asp:Label ID="Label13" runat="server">Versandart</asp:Label>
                                                            </td>
                                                            <td align="right" valign="top" style="padding-bottom: 0px;">
                                                                <asp:ImageButton ID="ImageButton10" Style="padding-right: 15px;" ToolTip="Bitte wählen Sie hier eine Empfängeradresse aus!"
                                                                    runat="server" ImageUrl="/services/Images/fragezeichen.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <asp:Panel ID="PLTempEndg" Style="padding-left: 15px" runat="server" Width="100%">
                                                    <div style="background-image: url(/services/Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                                        height: 8px; width: 16px">
                                                    </div>
                                                    <table cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_Versandart" CssClass="VersandAdressLabels" runat="server">lbl_Versandart</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rb_temp" Text="temporär" CssClass="VersandAdressLabels" runat="server"
                                                                    GroupName="Versandart" AutoPostBack="True" />
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rb_endg" runat="server" CssClass="VersandAdressLabels" Text="endgültig"
                                                                    GroupName="Versandart" AutoPostBack="True" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr id="trSelAdresse" runat="server" visible="false">
                                                            <td class="First" style="padding-left: 7px" valign="top">
                                                                <asp:Label ID="lbl_SelAdresse" CssClass="VersandAdressLabels" runat="server">lbl_SelAdresse</asp:Label>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:Label ID="lbl_SelAdresseShow" CssClass="VersandAdressLabels" runat="server">lbl_SelAdresseShow</asp:Label>
                                                            </td>
                                                            <td valign="bottom">
                                                                <asp:LinkButton ID="lbtnAdrUnload" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                                                    Height="18px">» Adresse zurücksetzen</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" style="padding-top: 0px;">
                                                                <asp:Label ID="lblErrorAdressen" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr id="trAdressuche" runat="server">
                                            <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                                <div id="DivAdressSucheHead" class="StandardHeadDetail" runat="server" style="cursor: pointer"
                                                    onclick="javascript:cpeAdressSucheCollapsed()">
                                                    <table width="100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="padding-left: 8px; padding-bottom: 0px;">
                                                                <asp:Label ID="lblAdressauswahl" Style="padding-top: 3px;" runat="server" ForeColor="White"
                                                                    Font-Size="12px" Font-Bold="True">Adressauswahl</asp:Label>
                                                            </td>
                                                            <td align="right" valign="top" style="padding-bottom: 0px;">
                                                                <asp:ImageButton ID="ImageButton6" runat="server" Style="padding-right: 5px; padding-top: 3px"
                                                                    ImageUrl="/services/Images/versand/plusgreen.png" OnClientClick="javascript:cpeAdressSucheCollapsed()" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <asp:Panel ID="PLAdressSuche" BackColor="#DCDCDC" runat="server" DefaultButton="ibtnSearchAdresse"
                                                    Width="100%">
                                                    <div id="DivAdressLeftFlag" runat="server" class="StandardHeadDetailFlag">
                                                    </div>
                                                    <div style="width: 450px">
                                                        <asp:Label ID="Label14" Height="32" Style="margin-left: 21px;" runat="server">Bitte geben Sie die Suchkriterien ein und klicken zur Suche auf die Lupe</asp:Label>
                                                    </div>
                                                    <table width="100%" cellspacing="0" cellpadding="0">
                                                        <tr id="trddlAdresse" runat="server" visible="false">
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_Versandan" runat="server">lbl_Versandan</asp:Label>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:DropDownList ID="ddlAdresse" Style="width: auto" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblSucheAdr" runat="server" Text="Label" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_Referenz" runat="server">
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_Referenz" runat="server">lbl_Referenz</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReferenz" runat="server" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td class="First" style="width: 50%">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_Firma" runat="server">lbl_Firma</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFirma" runat="server" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td class="First" style="width: 50%">
                                                                <asp:Label ID="Label21" runat="server" Text="Alle Eingaben mit Platzhalter-Suche (*) möglich (z.B. Muster*')"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_StrasseNr" runat="server">lbl_StrasseNr</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtStrasse" runat="server" Width="220px"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                                <asp:TextBox ID="txtHNr" Width="61px" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_PlzOrt" runat="server">lbl_PlzOrt</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPlz" runat="server" Width="61px"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                                <asp:TextBox ID="txtOrt" Width="220px" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_Land" runat="server">lbl_Land</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLand" runat="server" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                <asp:ImageButton ID="ibtnSearchAdresse" runat="server" ToolTip="Suchen" ImageUrl="/Services/images/Adresspflege/SucheSmall.png" />
                                                                &nbsp;
                                                                <asp:ImageButton ID="ibtnSucheSave" runat="server" ImageUrl="/Services/images/Adresspflege/ConfirmMiddle.png"
                                                                    ToolTip="Übernehmen" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <%--                                    <tr id="trZulStelleSuche" runat="server">
                                        <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <div id="DivZulstelleSucheHead" runat="server" class="StandardHeadDetail">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label15" Style="padding-top: 3px;" runat="server" ForeColor="White"
                                                                Font-Size="12px" Font-Bold="True">Zulassungsstellen</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ImageButton5" runat="server" Style="padding-right: 5px; padding-top: 3px"
                                                                ImageUrl="/services/Images/versand/plusgreen.png" OnClientClick="javascript:cpeZulstelleCollapsed()" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel BackColor="#DCDCDC" ID="PLZulstelle" runat="server" DefaultButton="ibtn_SucheGesch"
                                                Width="100%">
                                                <div id="DivZulstelleHeadFlag" runat="server" class="StandardHeadDetailFlag">
                                                </div>
                                                <div style="width: 450px">
                                                    <asp:Label ID="Label16" Height="32" Style="margin-left: 21px;" runat="server">Bitte geben Sie die Suchkriterien ein und klicken zur Suche auf die Lupe.</asp:Label>
                                                </div>
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr id="trZulStelle" runat="server" visible="false">
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_ZulStelle" runat="server">lbl_ZulStelle </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlZulStelle" Style="width: auto" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblErrZulStelle" Visible="False" runat="server" Text="Label" 
                                                                CssClass="TextError" EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_OrtSucheGe" runat="server">lbl_OrtSucheGe</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOrtSucheGe" runat="server" Width="300px"></asp:TextBox>
                                                        </td>
                                                        <td class="First" style="width: 50%">
                                                            <asp:Label ID="Label17" runat="server" CssClass="TextLarge" Text="Alle Eingaben mit Platzhalter-Suche (*) möglich (z.B. Muster*')"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_KennzKreis" runat="server">lbl_KennzKreis</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtKennzKreis" runat="server" Width="300px"></asp:TextBox>
                                                        </td>
                                                        <td class="First">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_PLZSucheGe" runat="server">lbl_PLZSucheGe</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPLZSucheGe" runat="server" Width="300px"></asp:TextBox>
                                                        </td>
                                                        <td class="First">
                                                            <asp:ImageButton ID="ibtn_SucheGesch" runat="server" ToolTip="Suchen" ImageUrl="/services/images/Versand/search.png" />
                                                            &nbsp;
                                                            <asp:ImageButton ID="ibtnSucheGeschSave" runat="server" ImageUrl="/services/images/haken_gruen24x24.gif"
                                                                ToolTip="Übernehmen" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>--%>
                                        <tr id="trFreieAdresse" runat="server">
                                            <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                                <div id="DivFreeAdrSucheHead" runat="server" class="StandardHeadDetail" style="cursor: pointer"
                                                    onclick="javascript:cpeAdressmanuellCollapsed()">
                                                    <table width="100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="padding-left: 8px; padding-bottom: 0px;">
                                                                <asp:Label ID="lblManuelleAdresseingabe" Style="padding-top: 3px;" runat="server"
                                                                    ForeColor="White" Font-Size="12px" Font-Bold="True">manuelle Adresseingabe</asp:Label>
                                                            </td>
                                                            <td align="right" valign="top" style="padding-bottom: 0px;">
                                                                <asp:ImageButton ID="ImageButton8" runat="server" Style="padding-right: 5px; padding-top: 3px"
                                                                    ImageUrl="/services/Images/versand/plusgreen.png" OnClientClick="javascript:cpeAdressmanuellCollapsed()" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <asp:Panel ID="PLAdressmanuell" BackColor="#DCDCDC" runat="server" Width="100%">
                                                    <div id="DivFreeAdressLeftFlag" runat="server" class="StandardHeadDetailFlag">
                                                    </div>
                                                    <div style="width: 450px">
                                                        <asp:Label ID="Label19" Height="32" Style="margin-left: 21px;" runat="server">Bitte geben Sie hier eine Adresse ein.</asp:Label>
                                                    </div>
                                                    <table width="100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_FirmaManuell" runat="server">lbl_FirmaManuell</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFirmaManuell" runat="server" Width="300px" MaxLength="40"></asp:TextBox>
                                                            </td>
                                                            <td class="First" style="width: 50%">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_Name2" runat="server">lbl_Name2</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtName2" runat="server" MaxLength="40" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_StrasseNrManuell" runat="server">lbl_StrasseNrManuell</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtStrasseManuell" MaxLength="60" runat="server" Width="220px"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                                <asp:TextBox ID="txtNrManuell" MaxLength="10" Width="61px" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_PlzOrtManuell" runat="server">lbl_PlzOrtManuell</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPlzManuell" runat="server" Width="61px" MaxLength="10"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                                <asp:TextBox ID="txtOrtManuell" Width="220px" MaxLength="40" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="First">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 21px">
                                                                <asp:Label ID="lbl_LandManuell" runat="server">lbl_LandManuell</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlLand" Style="width: auto" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="First">
                                                                <asp:ImageButton ID="ibtnSucheManuellSave" runat="server" ImageUrl="/Services/images/Adresspflege/ConfirmMiddle.png"
                                                                    ToolTip="Übernehmen" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" colspan="3" style="padding-left: 21px">
                                                                <asp:Label ID="lblErrorAdrManuell" runat="server" Visible="False" CssClass="TextError"
                                                                    EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="float: right; padding-top: 10px; padding-bottom: 10px; padding-right: 10px">
                                    <asp:LinkButton ID="ibtnNextToOptions" runat="server" CssClass="TablebuttonLarge"
                                        Width="130px" Height="16px">» Weiter</asp:LinkButton>
                                </div>
                            </div>
                            <div id="VersandTabPanel3" class="VersandTabPanel" runat="server" visible="false"
                                style="height: 600px; margin-bottom: 10px">
                                <div style="height: 545px">
                                    <table cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="padding-bottom: 0px; width: 400px;" class="PanelHead">
                                                <asp:Label ID="Label1" runat="server">Versandoption wählen</asp:Label>
                                            </td>
                                            <td style="width: 78%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="padding-top: 0px;">
                                                <asp:Label ID="Label3" runat="server">Bitte wählen Sie aus, wie die Schlüssel versendet werden sollen.</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="padding-left: 7px; width: 50%">
                                                <div class="PanelHeadSuche">
                                                    <table width="100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                                <asp:Label ID="Label22" runat="server">Versandoption</asp:Label>
                                                            </td>
                                                            <td align="right" valign="top" style="padding-bottom: 0px;">
                                                                <asp:ImageButton ID="ImageButton12" Style="padding-right: 15px;" ToolTip="Bitte wählen Sie hier einen Versandgrund aus!"
                                                                    runat="server" ImageUrl="/services/Images/fragezeichen.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <asp:Panel ID="Panel13" Style="padding-left: 15px" runat="server" Width="100%">
                                                    <div style="background-image: url(/services/Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                                        height: 8px; width: 16px">
                                                    </div>
                                                    <table cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td class="ListService" colspan="2" style="height: 15px">
                                                                <div style="margin-left: 7px">
                                                                    <asp:CheckBoxList ID="chkGruende" runat="server">
                                                                    </asp:CheckBoxList>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 10px;" nowrap="nowrap">
                                                                <asp:Label ID="lbl_Versandopt" runat="server">lbl_Versandopt</asp:Label>
                                                            </td>
                                                            <td style="width: 100%">
                                                                <asp:ImageButton ID="ibtnShowOptions" runat="server" ImageUrl="/services/Images/versand/plusgreen.png"
                                                                    Style="padding-right: 5px; padding-top: 3px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px" colspan="2">
                                                                <asp:Label ID="lblErrorVersandOpt" runat="server" Visible="False" CssClass="TextError"
                                                                    Style="padding-left: 0px" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="padding-left: 7px;">
                                                <div class="PanelHeadSuche" runat="server" id="pnlStuecklisteHeader" visible="false">
                                                    <table width="100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                                <asp:Label runat="server">Vorhandene Komponenten zum Fahrzeug</asp:Label>
                                                            </td>
                                                            <td align="right" valign="top" style="padding-bottom: 0px;">
                                                                <asp:ImageButton ID="ImageButton4" Style="padding-right: 15px;" ToolTip="Bitte wählen Sie hier zu versendende Komponenten!"
                                                                    runat="server" ImageUrl="/services/Images/fragezeichen.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <asp:Panel ID="pnlStueckliste" Style="padding-left: 15px;" runat="server" Width="100%"
                                                    Visible="false">
                                                    <div style="background-image: url(/services/Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                                        height: 8px; width: 16px">
                                                    </div>
                                                    <div style="margin: 8px 0;">
                                                        <asp:CheckBox runat="server" ID="stuecklisteSelectAll" OnCheckedChanged="StuecklisteSelectAllChanged"
                                                            Text="Alle auswählen" AutoPostBack="true" />
                                                    </div>
                                                    <div style="overflow-y: auto; height: 260px; margin-right: 30px;">
                                                        <asp:Repeater ID="stuecklisteOuter" runat="server" OnItemDataBound="stuecklisteOuterItemDataBound">
                                                            <ItemTemplate>
                                                                <div style="padding: 0 10px 20px 0; width: 30%; float: left;">
                                                                    <span style="font-weight: bold;">
                                                                        <%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM")%>/<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %></span><asp:Repeater
                                                                            runat="server" DataSource='<%# GetEntries(DataBinder.Eval(Container, "DataItem.EQUNR")) %>'>
                                                                            <ItemTemplate>
                                                                                <div style="padding: 1px;">
                                                                                    <asp:HiddenField runat="server" ID="stuecklisteEQUNR" Value='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' />
                                                                                    <asp:CheckBox runat="server" ID="stuecklisteSelect" Checked='<%# DataBinder.Eval(Container, "DataItem.Selected") = "1" %>'
                                                                                        Text='<%# DataBinder.Eval(Container, "DataItem.MAKTX")%>' OnCheckedChanged="StuecklisteSelectChanged"
                                                                                        AutoPostBack="true" />
                                                                                    <%--<asp:CheckBox runat="server" ID="stuecklisteSelect" Checked='<%# DataBinder.Eval(Container, "DataItem.Selected") = "1" %>' Style="float:left;" OnCheckedChanged="StuecklisteSelectChanged" AutoPostBack="true" />
                                                                            <span style="font-weight:bold;"><%# DataBinder.Eval(Container, "DataItem.MAKTX")%></span>&nbsp;(<%# Integer.Parse(DataBinder.Eval(Container, "DataItem.IDNRK"))%>)</span><br />
                                                                            <span><%# DataBinder.Eval(Container, "DataItem.ETEXT")%></span>--%>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                </div>
                                                                <asp:Literal runat="server" ID="outerLinebreak" Visible="false"><br style='clear:both;height:0px;font-size:0px;' /></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <table cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lblErrorStueckliste" runat="server" Visible="False" CssClass="TextError"
                                                                    Style="padding-left: 0px" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="float: right; padding-top: 10px; padding-bottom: 10px; padding-right: 10px">
                                    <asp:LinkButton ID="ibtnNextToOverView" runat="server" CssClass="TablebuttonLarge"
                                        Width="130px" Height="16px">» Weiter</asp:LinkButton>
                                </div>
                            </div>
                            <div id="VersandTabPanel4" class="VersandTabPanel" runat="server" visible="false"
                                style="height: 600px; margin-bottom: 10px">
                                <div style="height: 545px">
                                    <table cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="padding-bottom: 0px;" class="PanelHead">
                                                <asp:Label ID="Label24" runat="server">Übersicht</asp:Label>
                                            </td>
                                            <td style="width: 100%" align="right">
                                                <asp:ImageButton ID="ibtnCreatePDF" runat="server" ImageUrl="/services/Images/pdf-logo.png"
                                                    Style="height: 25px; padding-top: 10px; width: 22px" ToolTip="PDF herunterladen"
                                                    Visible="False" />
                                                <asp:Label ID="lblPDFPrint" runat="server" Height="20px" Style="padding-right: 10px;
                                                    padding-top: 10px" Text="Auftrag als PDF" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="padding-left: 7px; width: 50%">
                                                <div class="PanelHeadSuche">
                                                    <table width="100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                                <asp:Label ID="Label27" runat="server">Auftrag absenden</asp:Label>
                                                            </td>
                                                            <td align="right" valign="top" style="padding-bottom: 0px;">
                                                                <asp:ImageButton ID="ImageButton9" Style="padding-right: 15px;" ToolTip="Klicken Sie auf absenden."
                                                                    runat="server" ImageUrl="/services/Images/fragezeichen.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <asp:Panel ID="Panel19" Style="padding-left: 15px" runat="server" Width="100%" Height="200px"
                                                    ScrollBars="Auto">
                                                    <div style="background-image: url(/services/Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                                        height: 8px; width: 16px">
                                                    </div>
                                                    <table cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px" colspan="2">
                                                                <asp:Label ID="lblErrorAnfordern" runat="server" CssClass="TextError" Visible="False"
                                                                    EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px">
                                                                <asp:Label ID="lbl_VersArtOverview" runat="server" CssClass="VersandAdressLabels">lbl_VersArtOverview</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblVersArtOverviewShow" runat="server" CssClass="VersandAdressLabels"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px" valign="top">
                                                                <asp:Label ID="lblAdrOverview" runat="server" CssClass="VersandAdressLabels"> </asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="lblAdrOverviewShow" runat="server" CssClass="VersandAdressLabels"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <%--                                                    <tr>
                                                        <td class="First" style="padding-left: 7px">
                                                            <asp:Label ID="lbl_GrundOverview" runat="server" CssClass="VersandAdressLabels"> lbl_GrundOverview</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblGrundOverviewShow" runat="server" CssClass="VersandAdressLabels"></asp:Label>
                                                        </td>
                                                    </tr>--%>
                                                        <tr>
                                                            <td class="First" style="padding-left: 7px" valign="top">
                                                                <asp:Label ID="lbl_OptionsOverView" runat="server" CssClass="VersandAdressLabels">lbl_OptionsOverView </asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="lblOptionsOverViewShow" runat="server" CssClass="VersandAdressLabels"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7" style="margin-top: 10px; margin-bottom: 31px; padding-top: 0px; padding-right: 5px;
                                                text-align: right;">
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="Panel5" runat="server" Width="100%">
                                        <div style="height: 260px; overflow: auto; width: 100%;">
                                            <div id="ResultOverView" visible="false" style="padding-right: 5px; padding-left: 7px"
                                                runat="Server">
                                                <div id="VersandOverviewData">
                                                    <table id="Table1" runat="server" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td style="padding-left: 0px;">
                                                                <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView3"
                                                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="false" AllowSorting="false">
                                                                    <PagerSettings Visible="False" />
                                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                                    <RowStyle CssClass="ItemStyle" />
                                                                    <Columns>
                                                                        <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                                            <HeaderStyle Width="125px" />
                                                                            <ItemStyle Width="125px" />
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                                            <HeaderStyle Width="80px" />
                                                                            <ItemStyle Width="80px" />
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Leasingnummer" HeaderText="col_Leasingnummer">
                                                                            <HeaderStyle Width="100px" />
                                                                            <ItemStyle Width="100px" />
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Leasingnummer" runat="server" CommandName="Sort" CommandArgument="Leasingnummer">col_Leasingnummer</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblLeasingnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnummer") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="NummerZBII" HeaderText="col_NummerZBII">
                                                                            <HeaderStyle Width="80px" />
                                                                            <ItemStyle Width="80px" />
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_NummerZBII" runat="server" CommandName="Sort" CommandArgument="NummerZBII">col_NummerZBII</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblNummerZBII" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Referenz1" HeaderText="col_Referenz1">
                                                                            <HeaderStyle Width="80px" />
                                                                            <ItemStyle Width="80px" />
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="Referenz1">col_Referenz1</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblReferenz1" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz1") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Referenz2" HeaderText="col_Referenz2">
                                                                            <HeaderStyle Width="80px" />
                                                                            <ItemStyle Width="80px" />
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="Referenz2">col_Referenz2</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblReferenz2" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz2") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Versanddatum" HeaderText="col_Versanddatum">
                                                                            <HeaderStyle Width="80px" />
                                                                            <ItemStyle Width="80px" />
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Versanddatum" runat="server" CommandName="Sort" CommandArgument="Versanddatum">col_Versanddatum </asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblVersanddatum" Text='<%# DataBinder.Eval(Container, "DataItem.Versanddatum") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Status" HeaderText="col_Status">
                                                                            <HeaderStyle Width="80px" />
                                                                            <ItemStyle Width="80px" />
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="Referenz2">col_Status</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblStatus" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Versandadresse" HeaderText="col_Versandadresse">
                                                                            <HeaderStyle Width="150px" />
                                                                            <ItemStyle Width="150px" />
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Versandadresse" runat="server" CommandName="Sort" CommandArgument="Versandadresse">col_Versandadresse</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblVersandadresse" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Bemerkung" HeaderText="col_Bemerkung">
                                                                            <HeaderStyle Width="100px" />
                                                                            <ItemStyle Width="100px" />
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Bemerkung" runat="server" CommandName="Sort" CommandArgument="Bemerkung">col_Bemerkung</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblBemerkung" Text='<%# DataBinder.Eval(Container, "DataItem.Bemerkung") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div id="dataFooter">
                                    <asp:LinkButton ID="lbtnSend" runat="server" CssClass="Tablebutton" Width="78px"
                                        Height="16px">» Senden</asp:LinkButton>&nbsp;
                                </div>
                            </div>
                            <cc1:CollapsiblePanelExtender ID="cpeAllData" runat="Server" TargetControlID="pnlAllgDaten"
                                ExpandControlID="divSearch" CollapseControlID="divSearch" Collapsed="false" ImageControlID="NewSearch"
                                ExpandedImage="/services/Images/versand/minusred.png" CollapsedImage="/services/Images/versand/plusgreen.png"
                                SuppressPostBack="true" />
                            <cc1:CollapsiblePanelExtender ID="cpeUpload" runat="Server" TargetControlID="PLUpload"
                                ExpandControlID="divUpload" CollapseControlID="divUpload" Collapsed="true" ImageControlID="ImageButton1"
                                ExpandedImage="/services/Images/versand/minusred.png" CollapsedImage="/services/Images/versand/plusgreen.png"
                                SuppressPostBack="true" />
                            <cc1:CollapsiblePanelExtender ID="cpeDokuAusgabe" runat="Server" TargetControlID="pnlDokuAusgabe"
                                ExpandControlID="divDokuAusgabe" CollapseControlID="divDokuAusgabe" Collapsed="false"
                                ImageControlID="ImageButton3" ExpandedImage="../../../Images/versand/minusred.png"
                                CollapsedImage="../../../Images/versand/plusgreen.png" SuppressPostBack="true" />
                            <cc1:CollapsiblePanelExtender ID="cpeAdressSuche" runat="Server" TargetControlID="PLAdressSuche"
                                ExpandControlID="DivAdressSucheHead" CollapseControlID="DivAdressSucheHead" Collapsed="true"
                                ImageControlID="ImageButton6" ExpandedImage="/services/Images/versand/minusred.png"
                                CollapsedImage="/services/Images/versand/plusgreen.png" SuppressPostBack="true" />
                            <%--                            <cc1:CollapsiblePanelExtender ID="cpeZulstelle" runat="Server" TargetControlID="PLZulstelle"
                                ExpandControlID="ImageButton5" CollapseControlID="ImageButton5" Collapsed="true"
                                ImageControlID="ImageButton5" ExpandedImage="/services/Images/versand/minusred.png"
                                CollapsedImage="/services/Images/versand/plusgreen.png" SuppressPostBack="true" />--%>
                            <cc1:CollapsiblePanelExtender ID="cpeAdressmanuell" runat="Server" TargetControlID="PLAdressmanuell"
                                ExpandControlID="DivFreeAdrSucheHead" CollapseControlID="DivFreeAdrSucheHead"
                                Collapsed="true" ImageControlID="ImageButton8" ExpandedImage="/services/Images/versand/minusred.png"
                                CollapsedImage="/services/Images/versand/plusgreen.png" SuppressPostBack="true" />
                            <script type="text/javascript">

                                function cpeAdressSucheCollapsed() {

                                    var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAdressSuche');
                                    var Panel3 = $find('ctl00_ContentPlaceHolder1_cpeAdressmanuell');
                                    if (Panel1.get_Collapsed() != false) {
                                        if (Panel3 != null) { Panel3._doClose(); }
                                    }
                                }

                                function cpeAdressmanuellCollapsed() {

                                    var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAdressSuche');
                                    var Panel3 = $find('ctl00_ContentPlaceHolder1_cpeAdressmanuell');
                                    if (Panel3.get_Collapsed() != false) {
                                        if (Panel1 != null) { Panel1._doClose(); }

                                    }
                                }

                                function cpeAllDataCollapsed() {

                                    var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAllData');
                                    var Panel2 = $find('ctl00_ContentPlaceHolder1_cpeUpload');
                                    var Panel3 = $find('ctl00_ContentPlaceHolder1_cpeDokuAusgabe');
                                    if (Panel1.get_Collapsed() != false) {
                                        Panel2._doClose();
                                        if (Panel3 != null) { Panel3._doClose(); }
                                    }
                                }

                                function cpeUploadCollapsed() {

                                    var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAllData');
                                    var Panel2 = $find('ctl00_ContentPlaceHolder1_cpeUpload');
                                    var Panel3 = $find('ctl00_ContentPlaceHolder1_cpeDokuAusgabe');
                                    if (Panel2.get_Collapsed() != false) {
                                        Panel1._doClose();
                                        if (Panel3 != null) { Panel3._doClose(); }
                                    }
                                }

                                function cpeDokuAusgabeCollapsed() {

                                    var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAllData');
                                    var Panel2 = $find('ctl00_ContentPlaceHolder1_cpeUpload');
                                    var Panel3 = $find('ctl00_ContentPlaceHolder1_cpeDokuAusgabe');
                                    if (Panel3.get_Collapsed() != false) {
                                        if (Panel1 != null) { Panel1._doClose(); }
                                        if (Panel2 != null) { Panel2._doClose(); }
                                    }
                                }

                                function openinfo(url) {
                                    fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
                                    fenster.focus();
                                }
 
                            </script>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
