<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02s.aspx.vb" Inherits="CKG.Components.ComCommon.Logistik.Change02s"
    MasterPageFile="/services/MasterPage/Services.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/services/PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="submitFiles" runat="server" Text="Übersicht" Style="display: none;" />
    <script type="text/javascript">

        function validationFailed(sender, eventArgs) {
            alert("Es werden nur *.PDF Dateien unterstützt.");
        }

        function checkAbDatum(sender, args) {
            var jetzt = new Date()
            var heute = new Date(jetzt.getFullYear(), jetzt.getMonth(), jetzt.getDate())
            if (sender._selectedDate < heute) {
                alert("Sie können kein Datum wählen, das in der Vergangenheit liegt!");
                sender._selectedDate = heute;
                sender._textbox.set_Value(sender._selectedDate.format("dd.MM.yyyy"));
            }
        }

        //        function onClientUpdFailed(sender, eventArgs) {
        //            alert("Upload fehlgeschlagen");
        //        }

    </script>
    <style type="text/css">
        .gridLines
        {
            border-bottom: 3px solid #CCDCFF;
            height: 28px;
            color: White;
            padding-left: 0px;
        }
        .style1
        {
            color: #595959;
        }
        .Watermark
        {
            color: Gray;
        }
        .AbstandLinks
        {
            padding-left: 8px;
        }
        .AbstandRechts
        {
            padding-right: 8px;
        }
        .Pointer
        {
            cursor: pointer;
        }
    </style>
    <div id="site">
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>--%>
                <div id="content">
                    <div class="divPopupBack" runat="server" visible="false" id="divBackDisabled">
                    </div>
                    <div class="divPopupDetail" runat="server" visible="false" id="divOptions" style="width: 400px">
                        <table class="PopupDetailTable">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label15" runat="server" Text="Dienstleistungen" Font-Bold="True"></asp:Label></h3>
                                    </xxxxelmt> </xxxxelmt> </xxxxelmt> </xxxxelmt>
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
                                        runat="server" AutoPostBack="true" Visible="False">
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
                                                    <asp:Label ID="lblDL" runat="server" Text='<%# Bind("ASNUM") %>' Visible="false"></asp:Label>
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
                    <div id="divMessage" runat="server" visible="false" style="top: 300px; margin-left: 235px;
                        z-index: 3; width: 400px; position: absolute">
                        <div style="background-image: url(/services/images/MsgTitle.png); width: 400px">
                            <table width="100%">
                                <tr>
                                    <td style="width: 85%; padding-left: 10px">
                                        <asp:Label ID="lblMsgHeader" runat="server" ForeColor="White" Font-Bold="true" Text="Achtung"></asp:Label>
                                    </td>
                                    <td align="right" style="padding-right: 5px">
                                        <asp:ImageButton ID="ibtMsgCancel" runat="server" ImageAlign="Top" ImageUrl="/services/images/MsgClose.png"
                                            Width="47px" Height="15" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="border-left: solid 2px #576B96; border-right: solid 2px #576B96; width: 396px;
                            background-color: #DCDCDC">
                            <table width="100%">
                                <tr>
                                    <td valign="top" style="padding-left: 10px; padding-top: 10px; width: 15%">
                                        <img src="/services/images/MsgAtt.png" alt="Attention" />
                                    </td>
                                    <td style="padding-right: 10px; padding-top: 10px">
                                        <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="border-left: solid 2px #576B96; border-right: solid 2px #576B96; text-align: right;
                            background-color: #DCDCDC; padding-bottom: 10px; padding-top: 10px">
                            <asp:LinkButton ID="cmdOKWarnung" runat="server" Width="78px" Height="16px" CssClass="TablebuttonMiddle"
                                Visible="false" Style="margin-right: 10px">» OK</asp:LinkButton>
                            <asp:LinkButton ID="cmdJaWarnung" runat="server" Width="78px" Height="16px" CssClass="TablebuttonMiddle">» Ja</asp:LinkButton>
                            <asp:LinkButton ID="cmdNeinWarnung" runat="server" Width="78px" Height="16px" Style="margin-left: 5px;
                                margin-right: 10px" CssClass="TablebuttonMiddle">» Nein</asp:LinkButton>
                        </div>
                        <div style="background-image: url(/services/images/MsgBottom.png); background-repeat: no-repeat;
                            height: 6px">
                        </div>
                    </div>
                    <div visible="false" id="divKilometer" runat="server" style="z-index: 3; width: 500px;
                        margin-left: 235px; margin-top: 400px; position: absolute; background-color: #DCDCDC">
                        <div style="background-image: url(/services/images/title1.png); height: 25px; margin-bottom: 10px">
                            <div style="padding-left: 25px; padding-top: 5px; font-family: Sans-Serif; font-weight: bold;
                                font-size: 15px; color: White;">
                                Entfernungskilometer
                            </div>
                        </div>
                        <div>
                            <table width="100%" cellspacing="0">
                                <tr>
                                    <td align="center" style="padding-top: 20px">
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="460px">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="/services/images/lkw.png" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="STREET" HeaderText="Straße">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                                    <ItemStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="POSTL_CODE" HeaderText="PLZ">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                                    <ItemStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CITY" HeaderText="Ort">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                                    <ItemStyle HorizontalAlign="Left" CssClass="AbstandLinks" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="KM" HeaderText="Entfernung">
                                                    <HeaderStyle HorizontalAlign="Right" CssClass="AbstandRechts" />
                                                    <ItemStyle HorizontalAlign="Right" CssClass="AbstandRechts" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding-right: 15px">
                                        <asp:LinkButton ID="lbtKmClose" runat="server" CssClass="TablebuttonMiddle" Width="78px"
                                            Height="16px">» Schließen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div id="navigationSubmenu">
                        <%--<asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                            Text="zurück"></asp:LinkButton>--%>
                    </div>
                    <div id="innerContent">
                        <div id="innerContentRight" style="width: 100%">
                            <div>
                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label></div>
                            <div class="DivVersandTabContainer">
                                <asp:LinkButton ID="LinkButton1" CssClass="VersandButtonStamm" runat="server"></asp:LinkButton><asp:LinkButton
                                    ID="LinkButton2" CssClass="LogistikButtonTourDisabled" Enabled="false" runat="server"></asp:LinkButton><asp:LinkButton
                                        ID="LinkButton3" CssClass="LogistikButtonDL_Disabled" Enabled="false" runat="server"></asp:LinkButton><asp:LinkButton
                                            ID="LinkButton4" CssClass="VersandButtonOverviewEnabled" Enabled="false" runat="server"></asp:LinkButton><div
                                                class="DivPanelSteps">
                                                <table width="100%" cellspacing="0" cellpadding="0">
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
                                                </table>
                                            </div>
                            </div>
                            <div id="VersandTabPanel1" runat="server" class="VersandTabPanel">
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="padding-bottom: 0px; width: 100%" class="PanelHead">
                                            <asp:Label ID="lbl_Fahrzeugdaten" runat="server">Fahrzeugstammdaten eingeben</asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-top: 0px;">
                                            <asp:Label ID="Label11" runat="server" Text="Bitte geben Sie hier alle notwendigen Fahrzeugdaten ein."></asp:Label><br />
                                            <asp:Label ID="lblErrorStamm" runat="server" ForeColor="Red" Visible="false">Bitte füllen Sie die rot umrahmten Felder aus.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-left: 7px; width: 50%">
                                            <div id="divHinfahrt" runat="server" class="PanelHeadSuche" style="cursor: pointer"
                                                onclick="javascript:cpeAllDataCollapsed()">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label4" runat="server">Fahrzeug 1</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="/services/Images/versand/plusgreen.png"
                                                                Style="padding-right: 18px;" OnClientClick="javascript:cpeAllDataCollapsed()" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="pnlAllgDaten" runat="server" Width="100%">
                                                <div style="background-image: url(/services/Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                                    height: 8px; width: 16px; margin-left: 15px">
                                                </div>
                                                <div style="width: 450px">
                                                </div>
                                                <table cellspacing="0" cellpadding="0" width="100%">
                                                    <tr>
                                                        <td class="First" style="padding-left: 7px">
                                                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                                        </td>
                                                        <td width="40%">
                                                            <asp:TextBox ID="txtFahrgestellnummer" Width="300px" runat="server" MaxLength="20"></asp:TextBox>
                                                        </td>
                                                        <td class="First" style="width: 130px">
                                                            <asp:Label ID="lbl_Zugelassen" runat="server">Fahrzeug zugelassen<br />und betriebsbereit?</asp:Label>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:RadioButtonList ID="rblZugelassen" runat="server" RepeatDirection="Horizontal"
                                                                    RepeatLayout="Flow">
                                                                    <asp:ListItem Value="J">Ja</asp:ListItem>
                                                                    <asp:ListItem Value="N" style="padding-left: 36px">Nein</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 7px">
                                                            <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtKennzeichen1" Width="300px" runat="server" MaxLength="20"></asp:TextBox><cc1:TextBoxWatermarkExtender
                                                                ID="txtKennzeichen1_TextBoxWatermarkExtender" runat="server" Enabled="True" TargetControlID="txtKennzeichen1"
                                                                WatermarkCssClass="Watermark" WatermarkText="z.B. HH-PT664">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td class="First" style="width: 120px">
                                                            <asp:Label ID="Label7" runat="server">Zulassung an DAD beauftragt?</asp:Label>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:RadioButtonList ID="rblBeauftragt" runat="server" RepeatDirection="Horizontal"
                                                                    RepeatLayout="Flow">
                                                                    <asp:ListItem Value="J">Ja</asp:ListItem>
                                                                    <asp:ListItem Value="N" style="padding-left: 36px">Nein</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 7px; height: 32px;">
                                                            <asp:Label ID="lbl_Typ" runat="server">lbl_Typ</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTyp" runat="server" Width="300px" MaxLength="25"></asp:TextBox><cc1:TextBoxWatermarkExtender
                                                                ID="txtTyp_TextBoxWatermarkExtender" runat="server" Enabled="True" TargetControlID="txtTyp"
                                                                WatermarkCssClass="Watermark" WatermarkText="z.B. Audi A4">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_Bereifung" runat="server" Style="padding-right: 33px" Font-Bold="True">Bereifung: </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblBereifung" runat="server" RepeatDirection="Horizontal"
                                                                RepeatLayout="Flow">
                                                                <asp:ListItem Value="S">Sommer</asp:ListItem>
                                                                <asp:ListItem Value="W" style="padding-left: 2px">Winter</asp:ListItem>
                                                                <asp:ListItem Value="G" style="padding-left: 15px">GJ</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 7px">
                                                            <asp:Label ID="lbl_Referenznummer" runat="server">lbl_Referenznummer</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReferenznummer" runat="server" Width="300px" MaxLength="20"></asp:TextBox>&nbsp;<asp:ImageButton
                                                                ID="ImageButton10" Style="padding-right: 0px; padding-bottom: 4px" ToolTip="Hier können Sie Ihre interne Nummer für diesen Auftrag / Vorgang eingeben."
                                                                runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_Fahrzeugklasse" runat="server" Font-Bold="True" Style="padding-right: 17px">Fahrzeugklasse: </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblFahrzeugklasse" runat="server" RepeatDirection="Horizontal"
                                                                RepeatLayout="Flow">
                                                                <asp:ListItem Value="PKW">&lt; 3,5</asp:ListItem>
                                                                <asp:ListItem Value="PK1" style="padding-left: 20px">3,5 - 7,5</asp:ListItem>
                                                                <asp:ListItem Value="LKW" style="padding-left: 2px">&gt; 7,5</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 7px; height: 30px;">
                                                            <asp:Label ID="lbl_Fahrzeugwert" runat="server">lbl_Fahrzeugwert </asp:Label>
                                                        </td>
                                                        <td>
                                                            <div id="divFahrzeugwert" runat="server" style="width: 303px">
                                                                <asp:DropDownList ID="drpFahrzeugwert" runat="server" Width="303px">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="First" style="height: 30px">
                                                            <asp:ImageButton ID="ibtnSuche1" runat="server" ImageUrl="/services/images/buttonHaken.png"
                                                                ToolTip="Daten ergänzen" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" style="padding-top: 0px; padding-right: 20px" align="right">
                                                            <asp:ImageButton ID="ibtFahrzeug1" runat="server" ImageUrl="/services/images/cont.png"
                                                                ToolTip="Weiter" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <div id="divFahrzeug2" runat="server" class="StandardHeadDetail" style="cursor: pointer;
                                                background-color: #576B96" onclick="javascript:cpeCollapsedRueck()">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label9" Style="padding-top: 3px;" runat="server" ForeColor="White"
                                                                Font-Size="12px" Font-Bold="True">Fahrzeug 2</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" Style="padding-right: 5px; padding-top: 3px"
                                                                ImageUrl="/services/Images/versand/plusgreen.png" OnClientClick="javascript:cpeCollapsedRueck()" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel BackColor="#DCDCDC" ID="pnlRueckholung" runat="server" Width="100%">
                                                <div class="StandardHeadDetailFlag" style="background-color: #576B96">
                                                </div>
                                                <table cellspacing="0" cellpadding="0" width="100%">
                                                    <tr>
                                                        <td class="First" style="padding-left: 7px">
                                                            <asp:Label ID="lbl_RuFahrgestellnummer" runat="server">lbl_RuFahrgestellnummer</asp:Label>
                                                        </td>
                                                        <td width="40%">
                                                            <asp:TextBox ID="txtRuFahrgestellnummer" Width="300px" runat="server" MaxLength="20"></asp:TextBox>
                                                        </td>
                                                        <td class="First" style="width: 130px;">
                                                            <asp:Label ID="Label22" runat="server">Fahrzeug zugelassen<br />und betriebsbereit?</asp:Label>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:RadioButtonList ID="rblRuZugelassen" runat="server" RepeatDirection="Horizontal"
                                                                    RepeatLayout="Flow">
                                                                    <asp:ListItem Value="J">Ja</asp:ListItem>
                                                                    <asp:ListItem Value="N" style="padding-left: 36px">Nein</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 7px">
                                                            <asp:Label ID="lbl_RuKennzeichen" runat="server">lbl_RuKennzeichen</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRuKennzeichen1" Width="300px" runat="server" MaxLength="20"></asp:TextBox>
                                                        </td>
                                                        <td class="First" style="width: 120px">
                                                            <asp:Label ID="Label25" runat="server">Zulassung an DAD beauftragt?</asp:Label>
                                                        </td>
                                                        <td>
                                                            <span>
                                                                <asp:RadioButtonList ID="rblRuBeauftragt" runat="server" RepeatDirection="Horizontal"
                                                                    RepeatLayout="Flow">
                                                                    <asp:ListItem Value="J">Ja</asp:ListItem>
                                                                    <asp:ListItem Value="N" style="padding-left: 36px">Nein</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 7px; height: 32px;">
                                                            <asp:Label ID="lbl_RuTyp" runat="server">lbl_RuTyp</asp:Label>
                                                        </td>
                                                        <td style="height: 32px">
                                                            <asp:TextBox ID="txtRuTyp" runat="server" Width="300px" MaxLength="25"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_RuBereifung" runat="server" CssClass="First" Font-Bold="True">Bereifung: </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblRuBereifung" runat="server" RepeatDirection="Horizontal"
                                                                RepeatLayout="Flow">
                                                                <asp:ListItem Value="S">Sommer</asp:ListItem>
                                                                <asp:ListItem Value="W" style="padding-left: 2px">Winter</asp:ListItem>
                                                                <asp:ListItem Value="G" style="padding-left: 15px">GJ</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 7px">
                                                            <asp:Label ID="lbl_RuReferenznummer" runat="server">lbl_RuReferenznummer</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRuReferenznummer" runat="server" Width="300px" MaxLength="20"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_RuFahrzeugklasse" runat="server" Font-Bold="True" Style="padding-right: 17px">Fahrzeugklasse: </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblRuFahrzeugklasse" runat="server" RepeatDirection="Horizontal"
                                                                RepeatLayout="Flow">
                                                                <asp:ListItem Value="PKW">&lt; 3,5</asp:ListItem>
                                                                <asp:ListItem Value="PK1" style="padding-left: 20px">3,5 - 7,5</asp:ListItem>
                                                                <asp:ListItem Value="LKW" style="padding-left: 2px">&gt; 7,5</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 7px">
                                                            <asp:Label ID="lbl_RuFahrzeugwert" runat="server">lbl_RuFahrzeugwert </asp:Label>
                                                        </td>
                                                        <td>
                                                            <div id="divRuFahrzeugwert" runat="server" style="width: 300px">
                                                                <asp:DropDownList ID="drpRuFahrzeugwert" runat="server" Width="300px">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="First">
                                                            <asp:ImageButton ID="ibtnSuche2" runat="server" ImageUrl="/services/images/buttonhaken.png"
                                                                ToolTip="Daten ergänzen" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="right" style="padding-top: 0px; padding-right: 20px">
                                                            <asp:ImageButton ID="ibtFahrzeug2" runat="server" ImageUrl="/services/images/cont.png"
                                                                ToolTip="Weiter" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <div id="divRechnungszahler" runat="server" class="StandardHeadDetail" style="cursor: pointer;
                                                background-color: #576B96" onclick="javascript:cpeCollapsedRz()">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label20" Style="padding-top: 3px;" runat="server" ForeColor="White"
                                                                Font-Size="12px" Font-Bold="True">Rechnungszahler</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ImageButton2" runat="server" Style="padding-right: 5px; padding-top: 3px"
                                                                ImageUrl="/services/Images/versand/plusgreen.png" OnClientClick="javascript:cpeCollapsedRz()" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel BackColor="#DCDCDC" ID="pnlRechnungszahler" runat="server" Width="100%">
                                                <div class="StandardHeadDetailFlag" style="background-color: #576B96">
                                                </div>
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px" width="150">
                                                            <asp:Label ID="lbl_RzAuswahl" runat="server">lbl_RzAuswahl</asp:Label>
                                                        </td>
                                                        <td>
                                                            <div id="divRG" runat="server" style="width: 306px">
                                                                <asp:DropDownList ID="ddlPartnerRG" runat="server" Width="306px" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_RzFirma" runat="server">lbl_RzFirma</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRzFirma" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_RzStrasse" runat="server">lbl_RzStrasse</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRzStrasse" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_RzPlzOrt" runat="server">lbl_RzPlzOrt</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRzPLZ" runat="server" Width="60px" Enabled="False" MaxLength="5"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                            <asp:TextBox ID="txtRzOrt" Width="222px" runat="server" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_RzAnsprechpartner" runat="server">lbl_RzAnsprechpartner</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRzAnsprechpartner" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_RzTelefon" runat="server">lbl_RzTelefon</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRzTelefon" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="right" style="padding-right: 20px">
                                                            <asp:ImageButton ID="ibtRechnungszahler" runat="server" ImageUrl="/services/images/cont.png"
                                                                ToolTip="Weiter" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <div id="divAbwRechnungsadresse" runat="server" class="StandardHeadDetail" style="cursor: pointer;
                                                background-color: #576B96" onclick="javascript:cpeCollapsedRe()">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label21" Style="padding-top: 3px;" runat="server" ForeColor="White"
                                                                Font-Size="12px" Font-Bold="True">Abweichende Rechnungsadresse</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ImageButton3" runat="server" Style="padding-right: 5px; padding-top: 3px"
                                                                ImageUrl="/services/Images/versand/plusgreen.png" OnClientClick="javascript:cpeCollapsedRe()" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel BackColor="#DCDCDC" ID="pnlAbwRechnungsadresse" runat="server" Width="100%">
                                                <div class="StandardHeadDetailFlag" style="background-color: #576B96">
                                                </div>
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px" width="150">
                                                            <asp:Label ID="lbl_ArAuswahl" runat="server">lbl_ArAuswahl</asp:Label>
                                                        </td>
                                                        <td>
                                                            <div id="divRE" runat="server" style="width: 306px">
                                                                <asp:DropDownList ID="ddlPartnerRE" runat="server" Width="306px" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_ArFirma" runat="server">lbl_ArFirma</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtArFirma" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_ArStrasse" runat="server">lbl_ArStrasse</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtArStrasse" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_ArPlzOrt" runat="server">lbl_ArPlzOrt</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtArPLZ" runat="server" Width="60px" Enabled="False" MaxLength="5"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                            <asp:TextBox ID="txtArOrt" Width="222px" runat="server" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_ArAnsprechpartner" runat="server">lbl_ArAnsprechpartner</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtArAnsprechpartner" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            <asp:Label ID="lbl_ArTelefon" runat="server">lbl_ArTelefon</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtArTelefon" runat="server" Width="300px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="right" style="padding-right: 20px">
                                                            <asp:ImageButton ID="ibtAbwRechnungsadresse" runat="server" ImageUrl="/services/images/cont.png"
                                                                ToolTip="Weiter" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" style="padding-top: 0px;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" style="margin-top: 10px; margin-bottom: 31px; padding-top: 0px; padding-right: 5px;
                                            text-align: right;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="VersandTabPanel2" class="VersandTabPanel" runat="server" visible="false">
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="padding-bottom: 0px; width: 100%" class="PanelHead">
                                            <asp:Label ID="Label10" runat="server">Festlegung der Fahrten</asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-top: 0px;">
                                            <asp:Label ID="Label38" runat="server" Text="Bitte geben Sie hier die Adressen für die Abholung und Anlieferung ein."></asp:Label><br />
                                            <asp:Label ID="lblErrorFahrten" runat="server" ForeColor="Red" Visible="False" EnableViewState="False">Bitte füllen Sie die rot umrahmten Felder aus.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-left: 7px; width: 50%">
                                            <div class="PanelHeadSuche" id="divAbholadresse" runat="server" onclick="__doPostBack('divAbholadresse','')"
                                                style="cursor: pointer">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label13" runat="server">Abholadresse</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ibtAbholadresseHeaderClose" runat="server" Style="padding-right: 18px;
                                                                padding-bottom: 3px" ImageUrl="/services/Images/versand/minusred.png" Visible="true" />
                                                            <asp:ImageButton ID="ibtAbholadresseHeader" runat="server" ImageUrl="/services/Images/versand/plusgreen.png"
                                                                Style="padding-right: 18px; padding-bottom: 3px" Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="pnlAbholadresse" Style="padding-left: 0px" runat="server" Width="100%"
                                                DefaultButton="ibtAbholsuche">
                                                <div style="background-image: url(/services/Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                                    height: 8px; width: 16px; margin-left: 15px">
                                                </div>
                                                <table cellspacing="0" cellpadding="0" style="padding-left: 0px; margin-left: 0px">
                                                    <tr>
                                                        <td valign="top">
                                                            <table width="100%" cellspacing="0" cellpadding="0" style="padding-left: 0px">
                                                                <tr id="trAbholadresse" runat="server" visible="false">
                                                                    <td class="First" style="padding-left: 0px">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlAbholadresse" runat="server" Width="306" AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="lblErrAbholSuche" runat="server" ForeColor="Red" Visible="False">Es wurden keine Adressen gefunden.</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_AbFirma" runat="server">lbl_AbFirma</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAbFirma" runat="server" Width="270px" MaxLength="35"></asp:TextBox><asp:ImageButton
                                                                            ID="ibtAbholsuche" runat="server" ImageUrl="/services/images/Versand/search.png"
                                                                            ToolTip="Suchen" Width="20px" Height="20px" Style="padding-left: 5px" />
                                                                        <asp:ImageButton ID="ibtAbholReset" runat="server" ImageUrl="/services/images/Logistik/loesch.gif"
                                                                            ToolTip="Zurücksetzen" Style="padding-left: 5px" Visible="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_AbStrasse" runat="server">lbl_AbStrasse</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAbStrasse" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_AbPlzOrt" runat="server">lbl_AbPlzOrt</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAbPLZ" runat="server" Width="60px" MaxLength="7"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                                        <asp:TextBox ID="txtAbOrt" Width="222px" runat="server" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_AbLand" runat="server">lbl_AbLand</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlAbLand" runat="server" Width="306px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_AbAnsprechpartner" runat="server">lbl_AbAnsprechpartner</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAbAnsprechpartner" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_AbTelefon" runat="server">lbl_AbTelefon</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAbTelefon" runat="server" Width="300px" MaxLength="16"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_AbDatum" runat="server">lbl_AbDatum</asp:Label>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:TextBox ID="txtAbDatum" runat="server" Width="300px"></asp:TextBox><cc1:TextBoxWatermarkExtender
                                                                            ID="ExAbDatum" runat="server" TargetControlID="txtAbDatum" WatermarkText="kein Fixtermin"
                                                                            WatermarkCssClass="Watermark">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <span>
                                                                            <asp:ImageButton ID="ImageButton23" Style="padding-bottom: 4px" ToolTip="Falls Ihr gewünschtes Überführungsdatum mehr als 3 Tage in der Zukunft liegt, können Sie es hier angeben (kein Pflichtfeld)."
                                                                                runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                                        </span>
                                                                        <cc1:CalendarExtender ID="txtAbDatum_CalendarExtender" runat="server" TargetControlID="txtAbDatum" 
                                                                            OnClientDateSelectionChanged="checkAbDatum">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_AbUhrzeit" runat="server">lbl_AbUhrzeit</asp:Label>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:DropDownList ID="ddlAbUhrzeit" runat="server" Width="306px">
                                                                        </asp:DropDownList>
                                                                        <span>
                                                                            <asp:ImageButton ID="ImageButton14" Style="padding-bottom: 4px" ToolTip="Hier können Sie zusätzlich zum Datum eine gewünschte Uhrzeit für die Überführung angeben (kein Pflichtfeld)."
                                                                                runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" style="width: 100%;">
                                                            <table cellspacing="0" cellpadding="0" width="100%" style="padding-top: 0px; height: 100%">
                                                                <tr>
                                                                    <td colspan="2" style="height: 100px; padding-left: 2px; padding-top: 0px">
                                                                        <asp:Panel ID="Panel10" runat="server" Width="92%" Height="100px" Style="padding-top: 0px;
                                                                            background-image: url(/services/Images/Logistik/BackDetails.png); background-repeat: no-repeat">
                                                                            <div style="padding-top: 5px">
                                                                                <span class="style1" style="padding-left: 5px"><b>Fahrzeugdaten</b></span><br />
                                                                                <br />
                                                                                <asp:Label ID="Label18" runat="server" Height="15px" Style="padding-left: 5px">Fahrzeugtyp: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblAbDetailTyp" runat="server" Height="15px"></asp:Label><br />
                                                                                <asp:Label ID="Label26" runat="server" Height="15px" Style="padding-left: 5px">Kennzeichen: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblAbDetailKennzeichen" runat="server" Height="15px"></asp:Label><br />
                                                                                <asp:Label ID="Label28" runat="server" Height="12px" Style="padding-left: 5px">Fahrgestellnummer: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblAbDetailFin" runat="server" Height="12px"></asp:Label></div>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:CheckBox runat="server" ID="cbxShowZusatzfahrten" Text="Zusatzfahrten einblenden" AutoPostBack="True"/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="right" style="padding-right: 20px; height: 110px" valign="bottom">
                                                                        <asp:ImageButton ID="ibtAbholadresseNext" runat="server" ImageUrl="/services/images/cont.png"
                                                                            ToolTip="Weiter" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr id="trZusatz" runat="server" Visible="false">
                                        <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <span>
                                                <asp:GridView ID="grvZusatz" runat="server" GridLines="Horizontal" ShowHeader="False"
                                                    Width="100%" AutoGenerateColumns="False" BackColor="#2A4889" Style="padding-bottom: 5px;"
                                                    Visible="False">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-Font-Names="Verdana, Sans-Serif"
                                                            ItemStyle-Font-Bold="true" ItemStyle-Font-Size="10">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ID" runat="server" Text='<%# Bind("FAHRT") %>' Visible="false"></asp:Label><asp:Label
                                                                    ID="lblGrOrt" runat="server" Text='<%# Bind("Info") %>' ForeColor="White"></asp:Label></ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-ForeColor="White">
                                                            <ItemTemplate>
                                                                (<asp:Label ID="lblGrvTransporttyp" runat="server" Text='<%# Bind("Transporttyp") %>'
                                                                    ForeColor="White"></asp:Label>)</ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-Width="70px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtArrowDown" runat="server" ImageUrl="/services/Images/Logistik/arrowdown.png"
                                                                    ImageAlign="Right" CommandName="arrowdown" CommandArgument="<%# Container.DataItemIndex %>" />
                                                                <asp:ImageButton ID="ibtArrowUp" runat="server" ImageUrl="/services/Images/Logistik/arrowup.png"
                                                                    ImageAlign="Right" CommandName="arrowup" CommandArgument="<%# Container.DataItemIndex %>" />
                                                                <asp:ImageButton ID="ibtDelete" runat="server" ImageUrl="/services/Images/Logistik/delete.png"
                                                                    ImageAlign="Right" CommandName="Del" CommandArgument="<%# Container.DataItemIndex %>" />
                                                                <asp:ImageButton ID="ibtEdit" runat="server" ImageUrl="/services/Images/Logistik/edit.png"
                                                                    ImageAlign="Right" CommandName="edit" CommandArgument="<%# Container.DataItemIndex %>" />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </span>
                                            <div id="divZusatz" runat="server" class="StandardHeadDetail" style="cursor: pointer;
                                                background-color: #576B96" onclick="__doPostBack('divZusatz','')" >
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="lblZusatz" Style="padding-top: 3px;" runat="server" ForeColor="White"
                                                                Font-Size="12px" Font-Bold="True">Zusatzfahrt hinzufügen</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ibtCloseZusatzfahrt" runat="server" Style="padding-right: 5px;
                                                                padding-top: 3px" ImageUrl="/services/Images/versand/minusred.png" Visible="false" />
                                                            <asp:ImageButton ID="ibtOpenZusatzfahrt" runat="server" Style="padding-right: 5px;
                                                                padding-top: 3px" ImageUrl="/services/Images/versand/plusgreen.png" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="pnlZusatzfahrten" BackColor="#DCDCDC" runat="server" Width="100%"
                                                Visible="false">
                                                <div class="StandardHeadDetailFlag">
                                                </div>
                                                <table cellspacing="0" cellpadding="0" style="padding-left: 0px; margin-left: 0px">
                                                    <tr>
                                                        <td>
                                                            <table cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZuFirma" runat="server">lbl_ZuFirma</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZuFirma" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZuStrasse" runat="server">lbl_ZuStrasse</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZuStrasse" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZuPlzOrt" runat="server">lbl_ZuPlzOrt</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZuPLZ" runat="server" Width="60px" MaxLength="7"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                                        <asp:TextBox ID="txtZuOrt" Width="222px" runat="server" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZuLand" runat="server">lbl_ZuLand</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlZuLand" runat="server" Width="306px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZuAnsprechpartner" runat="server">lbl_ZuAnsprechpartner</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZuAnsprechpartner" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZuTelefon" runat="server">lbl_ZuTelefon</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZuTelefon" runat="server" Width="300px" MaxLength="16"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZuDatum" runat="server">lbl_ZuDatum</asp:Label>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:TextBox ID="txtZuDatum" runat="server" Width="300px"></asp:TextBox><cc1:TextBoxWatermarkExtender
                                                                            ID="ExZuDatum" runat="server" TargetControlID="txtZuDatum" WatermarkText="kein Fixtermin"
                                                                            WatermarkCssClass="Watermark">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <asp:ImageButton ID="ImageButton19" Style="padding-right: 0px; padding-bottom: 4px"
                                                                            ToolTip="Falls Ihr gewünschtes Überführungsdatum mehr als 3 Tage in der Zukunft liegt, können Sie es hier angeben (kein Pflichtfeld)."
                                                                            runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                                        <cc1:CalendarExtender ID="txtZuDatum_CalendarExtender" runat="server" TargetControlID="txtZuDatum" 
                                                                            OnClientDateSelectionChanged="checkAbDatum">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZuUhrzeit" runat="server">lbl_ZuUhrzeit</asp:Label>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:DropDownList ID="ddlZuUhrzeit" runat="server" Width="306px">
                                                                        </asp:DropDownList>
                                                                        <asp:ImageButton ID="ImageButton15" Style="padding-right: 0px; padding-bottom: 4px"
                                                                            ToolTip="Hier können Sie zusätzlich zum Datum eine gewünschte Uhrzeit für die Überführung angeben (kein Pflichtfeld)."
                                                                            runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" style="width: 100%">
                                                            <table cellspacing="0" cellpadding="0" width="100%" style="height: 100%">
                                                                <tr>
                                                                    <td style="padding-left: 2px">
                                                                        Transporttyp:
                                                                    </td>
                                                                    <td style="width: 73%">
                                                                        <div id="divZuTransporttyp" runat="server" style="width: 100%">
                                                                            <asp:DropDownList ID="ddlZuTransporttyp" runat="server" Width="100%">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                    <td style="width: 9%">
                                                                        <asp:ImageButton ID="ImageButton24" runat="server" ImageUrl="/Services/Images/fragezeichen.gif"
                                                                            Style="padding-right: 0px; padding-left: 2px; padding-bottom: 4px" ToolTip="Über den Transporttyp wird die Art der Fahrt definiert, und es werden automatisch unterschiedliche Dienstleistungen ermittelt." />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" style="padding-left: 2px" valign="top">
                                                                        <asp:Panel ID="Panel9" runat="server" Width="92%" Height="80px" Style="background-image: url(/services/Images/Logistik/BackDetails.png);
                                                                            background-repeat: no-repeat">
                                                                            <div style="padding-top: 5px">
                                                                                <span class="style1" style="padding-left: 5px"><b>Fahrzeugdaten</b></span><br />
                                                                                <br />
                                                                                <asp:Label ID="Label1" runat="server" Height="15px" Style="padding-left: 5px">Fahrzeugtyp: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblDetailTyp" runat="server" Height="15px">Audi A3</asp:Label><br />
                                                                                <asp:Label ID="Label2" runat="server" Height="15px" Style="padding-left: 5px">Kennzeichen: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblDetailKennzeichen" runat="server" Height="15px">K-SE3333</asp:Label><br />
                                                                                <asp:Label ID="Label3" runat="server" Height="12px" Style="padding-left: 5px">Fahrgestellnummer: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblDetailVin" runat="server" Height="12px">WAUZBRN3333333333</asp:Label></div>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" align="right" style="padding-right: 20px; height: 120px" valign="bottom">
                                                                        <asp:ImageButton ID="ibtZusatzfahrtNext" runat="server" ImageUrl="/services/images/cont.png"
                                                                            ToolTip="Weiter" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="4" style="padding-left: 7px; width: 50%">--%>
                                        <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <div class="StandardHeadDetail" id="divZieladresse" runat="server" onclick="__doPostBack('divZieladresse','')"
                                                style="cursor: pointer; background-color: #576B96">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Size="12px" ForeColor="White">Zieladresse Fzg. 1</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ibtZieladresseHeaderClose" runat="server" ImageUrl="/services/Images/versand/minusred.png"
                                                                Style="padding-right: 5px; padding-top: 3px" Visible="false" />
                                                            <asp:ImageButton ID="ibtZieladresseHeader" runat="server" Style="padding-right: 5px;
                                                                padding-top: 3px" ImageUrl="/services/Images/versand/plusgreen.png" Visible="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="pnlZieladresse" Style="padding-left: 0px" runat="server" Width="100%"
                                                DefaultButton="ibtZielSuche" Visible="false">
                                                <div class="StandardHeadDetailFlag2">
                                                </div>
                                                <table cellspacing="0" cellpadding="0" style="padding-left: 0px">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" cellspacing="0" cellpadding="0">
                                                                <tr id="trZielAdresse" runat="server" visible="false">
                                                                    <td class="First" style="padding-left: 0px">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlZieladresse" runat="server" Width="306" AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="lblErrZielSuche" runat="server" ForeColor="Red" Visible="False">Es wurden keine Adressen gefunden.</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZielFirma" runat="server">lbl_ZielFirma</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZielFirma" runat="server" Width="270px" MaxLength="35"></asp:TextBox><asp:ImageButton
                                                                            ID="ibtZielSuche" runat="server" ImageUrl="/services/images/Versand/search.png"
                                                                            ToolTip="Suchen" Width="20px" Height="20px" Style="padding-left: 5px" />
                                                                        <asp:ImageButton ID="ibtZielReset" runat="server" ImageUrl="/services/images/Logistik/loesch.gif"
                                                                            ToolTip="Zurücksetzen" Style="padding-left: 5px" Visible="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZielStrasse" runat="server">lbl_ZielStrasse</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZielStrasse" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZielPlzOrt" runat="server">lbl_ZielPlzOrt</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZielPLZ" runat="server" Width="60px" MaxLength="7"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                                        <asp:TextBox ID="txtZielOrt" Width="222px" runat="server" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZielLand" runat="server">lbl_ZielLand</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlZielland" runat="server" AutoPostBack="True" Width="306px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZielAnsprechpartner" runat="server">lbl_ZielAnsprechpartner</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZielAnsprechpartner" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZielTelefon" runat="server">lbl_ZielTelefon</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtZielTelefon" runat="server" Width="300px" MaxLength="16"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZielDatum" runat="server">lbl_ZielDatum</asp:Label>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:TextBox ID="txtZielDatum" runat="server" Width="300px"></asp:TextBox><cc1:TextBoxWatermarkExtender
                                                                            ID="ExZielDatum" runat="server" TargetControlID="txtZielDatum" WatermarkText="kein Fixtermin"
                                                                            WatermarkCssClass="Watermark">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <asp:ImageButton ID="ImageButton20" Style="padding-right: 0px; padding-bottom: 4px"
                                                                            ToolTip="Falls Ihr gewünschtes Überführungsdatum mehr als 3 Tage in der Zukunft liegt, können Sie es hier angeben (kein Pflichtfeld)."
                                                                            runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                                        <cc1:CalendarExtender ID="txtZielDatum_CalendarExtender" runat="server" TargetControlID="txtZielDatum" 
                                                                            OnClientDateSelectionChanged="checkAbDatum">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_ZielUhrzeit" runat="server">lbl_ZielUhrzeit</asp:Label>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:DropDownList ID="ddlZielUhrzeit" runat="server" Width="306px">
                                                                        </asp:DropDownList>
                                                                        <asp:ImageButton ID="ImageButton16" Style="padding-right: 0px; padding-bottom: 4px"
                                                                            ToolTip="Hier können Sie zusätzlich zum Datum eine gewünschte Uhrzeit für die Überführung angeben (kein Pflichtfeld)."
                                                                            runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" style="width: 100%">
                                                            <table cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td style="padding-left: 2px;">
                                                                        Transporttyp:
                                                                    </td>
                                                                    <td style="width: 73%">
                                                                        <div id="divZielTransporttyp" runat="server" style="width: 100%">
                                                                            <asp:DropDownList ID="ddlZielTransporttyp" runat="server" Style="width: 100%">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                    <td style="width: 9%">
                                                                        <asp:ImageButton ID="ImageButton25" runat="server" ImageUrl="/Services/Images/fragezeichen.gif"
                                                                            ToolTip="Über den Transporttyp wird die Art der Fahrt definiert, und es werden automatisch unterschiedliche Dienstleistungen ermittelt." />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" style="height: 120px; padding-left: 2px">
                                                                        <asp:Panel ID="Panel11" runat="server" Width="92%" Height="80px" Style="background-image: url(/services/Images/Logistik/BackDetails.png);
                                                                            background-repeat: no-repeat">
                                                                            <div style="padding-top: 5px">
                                                                                <span class="style1" style="padding-left: 5px"><b>Fahrzeugdaten</b></span><br />
                                                                                <br />
                                                                                <asp:Label ID="Label30" runat="server" Height="15px" Style="padding-left: 5px">Fahrzeugtyp: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblZielDetailTyp" runat="server" Height="15px">Audi A3</asp:Label><br />
                                                                                <asp:Label ID="Label33" runat="server" Height="15px" Style="padding-left: 5px">Kennzeichen: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblZielDetailKennzeichen" runat="server" Height="15px">K-SE3333</asp:Label><br />
                                                                                <asp:Label ID="Label35" runat="server" Height="12px" Style="padding-left: 5px">Fahrgestellnummer: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblZielDetailFin" runat="server" Height="12px">WAUZBRN3333333333</asp:Label></div>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr_Kilometer" runat="server">
                                                                    <td colspan="3" style="padding-left: 4px; padding-top: 40px">
                                                                        <asp:ImageButton ID="lb_Kilometer" runat="server" ImageUrl="/services/images/BAB.png"
                                                                            ToolTip="Berechnung der Entfernungskilometer." />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" align="right" style="padding-right: 20px" valign="bottom">
                                                                        <asp:ImageButton ID="ibtZieladresseNext" runat="server" ImageUrl="/services/images/cont.png"
                                                                            ToolTip="Weiter" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr id="trRueck" runat="server" visible="false">
                                        <td colspan="4" style="padding-left: 7px; width: 50%">
                                            <div class="PanelHeadSuche">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label8" runat="server">Rückholung</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trRZusatz" runat="server" Visible="false">
                                        <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <span>
                                                <asp:GridView ID="grvRZusatz" runat="server" GridLines="Horizontal" ShowHeader="False"
                                                    Width="100%" AutoGenerateColumns="False" BackColor="#2A4889" Style="padding-bottom: 5px;"
                                                    Visible="False">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-Font-Names="Verdana, Sans-Serif"
                                                            ItemStyle-Font-Bold="true" ItemStyle-Font-Size="10">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ID" runat="server" Text='<%# Bind("FAHRT") %>' Visible="false"></asp:Label><asp:Label
                                                                    ID="lblGrOrt" runat="server" Text='<%# Bind("Info") %>' ForeColor="White"></asp:Label></ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-ForeColor="White">
                                                            <ItemTemplate>
                                                                (<asp:Label ID="lblGrvTransporttyp" runat="server" Text='<%# Bind("Transporttyp") %>'
                                                                    ForeColor="White"></asp:Label>)</ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-Width="70px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtArrowDown" runat="server" ImageUrl="/services/Images/Logistik/arrowdown.png"
                                                                    ImageAlign="Right" CommandName="arrowdown" CommandArgument="<%# Container.DataItemIndex %>" />
                                                                <asp:ImageButton ID="ibtArrowUp" runat="server" ImageUrl="/services/Images/Logistik/arrowup.png"
                                                                    ImageAlign="Right" CommandName="arrowup" CommandArgument="<%# Container.DataItemIndex %>" />
                                                                <asp:ImageButton ID="ibtDelete" runat="server" ImageUrl="/services/Images/Logistik/delete.png"
                                                                    ImageAlign="Right" CommandName="Del" CommandArgument="<%# Container.DataItemIndex %>" />
                                                                <asp:ImageButton ID="ibtEdit" runat="server" ImageUrl="/services/Images/Logistik/edit.png"
                                                                    ImageAlign="Right" CommandName="edit" CommandArgument="<%# Container.DataItemIndex %>" />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </span>
                                            <div id="divRZusatz" runat="server" visible="false" class="StandardHeadDetail" style="cursor: pointer;
                                                background-color: #576B96" onclick="__doPostBack('divRZusatz','')">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="lblRueckZusatz" Style="padding-top: 3px;" runat="server" ForeColor="White"
                                                                Font-Size="12px" Font-Bold="True">Zusatzfahrt hinzufügen</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ibtRZClose" runat="server" Style="padding-right: 5px; padding-top: 3px"
                                                                ImageUrl="/services/Images/versand/minusred.png" Visible="false" />
                                                            <asp:ImageButton ID="ibtRZOpen" runat="server" Style="padding-right: 5px; padding-top: 3px"
                                                                ImageUrl="/services/Images/versand/plusgreen.png" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="pnlRZusatzfahrten" BackColor="#DCDCDC" runat="server" Width="100%"
                                                Visible="false">
                                                <div class="StandardHeadDetailFlag" style="background-color: #576B96">
                                                </div>
                                                <table cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <table cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RueckZuFirma" runat="server">lbl_RueckZuFirma</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRueckZuFirma" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RueckZuStrasse" runat="server">lbl_RueckZuStrasse</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRueckZuStrasse" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RueckZuPlzOrt" runat="server">lbl_RueckZuPlzOrt</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRueckZuPLZ" runat="server" Width="60px" MaxLength="7"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                                        <asp:TextBox ID="txtRueckZuOrt" Width="222px" runat="server" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RueckZuLand" runat="server">lbl_RueckZuLand</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlRueckZuLand" runat="server" AutoPostBack="True" Width="306px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RueckZuAnsprechpartner" runat="server">lbl_RueckZuAnsprechpartner</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRueckZuAnsprechpartner" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RueckZuTelefon" runat="server">lbl_RueckZuTelefon</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRueckZuTelefon" runat="server" Width="300px" MaxLength="16"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RueckZuDatum" runat="server">lbl_RueckZuDatum</asp:Label>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:TextBox ID="txtRueckZuDatum" runat="server" Width="300px"></asp:TextBox><cc1:TextBoxWatermarkExtender
                                                                            ID="ExRueckZuDatum" runat="server" TargetControlID="txtRueckZuDatum" WatermarkText="kein Fixtermin"
                                                                            WatermarkCssClass="Watermark">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <asp:ImageButton ID="ImageButton21" Style="padding-right: 0px; padding-bottom: 4px;"
                                                                            ToolTip="Falls Ihr gewünschtes Überführungsdatum mehr als 3 Tage in der Zukunft liegt, können Sie es hier angeben (kein Pflichtfeld)."
                                                                            runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                                        <cc1:CalendarExtender ID="txtRueckZuDatum_CalendarExtender" runat="server" TargetControlID="txtRueckZuDatum" 
                                                                            OnClientDateSelectionChanged="checkAbDatum">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RueckZuUhrzeit" runat="server">lbl_RueckZuUhrzeit</asp:Label>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:DropDownList ID="ddlRueckZuUhrzeit" runat="server" Width="306px">
                                                                        </asp:DropDownList>
                                                                        <asp:ImageButton ID="ImageButton17" Style="padding-right: 0px; padding-bottom: 4px;"
                                                                            ToolTip="Hier können Sie zusätzlich zum Datum eine gewünschte Uhrzeit für die Überführung angeben (kein Pflichtfeld)."
                                                                            runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" style="width: 100%">
                                                            <table cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td style="padding-left: 2px">
                                                                        Transporttyp:
                                                                    </td>
                                                                    <td style="width: 73%">
                                                                        <div id="divRZuTransporttyp" runat="server" style="width: 100%">
                                                                            <asp:DropDownList ID="ddlRZuTransporttyp" runat="server" Width="100%">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                    <td style="width: 9%">
                                                                        <asp:ImageButton ID="ImageButton26" runat="server" ImageUrl="/Services/Images/fragezeichen.gif"
                                                                            Style="padding-right: 0px; padding-left: 2px; padding-bottom: 4px" ToolTip="Über den Transporttyp wird die Art der Fahrt definiert, und es werden automatisch unterschiedliche Dienstleistungen ermittelt." />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" style="height: 120px; padding-left: 2px">
                                                                        <asp:Panel ID="Panel12" runat="server" Width="92%" Height="80px" Style="background-image: url(/services/Images/Logistik/BackDetails.png);
                                                                            background-repeat: no-repeat">
                                                                            <div style="padding-top: 5px">
                                                                                <span class="style1" style="padding-left: 5px"><b>Fahrzeugdaten</b></span><br />
                                                                                <br />
                                                                                <asp:Label ID="Label37" runat="server" Height="15px" Style="padding-left: 5px">Fahrzeugtyp: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblRuDetailTyp" runat="server" Height="15px"></asp:Label><br />
                                                                                <asp:Label ID="Label39" runat="server" Height="15px" Style="padding-left: 5px">Kennzeichen: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblRuDetailKennzeichen" runat="server" Height="15px"></asp:Label><br />
                                                                                <asp:Label ID="Label41" runat="server" Height="12px" Style="padding-left: 5px">Fahrgestellnummer: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblRuDetailFin" runat="server" Height="12px"></asp:Label></div>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" align="right" style="padding-right: 20px; height: 100px" valign="bottom">
                                                                        <asp:ImageButton ID="ibtZusatzRueckNext" runat="server" ImageUrl="/services/images/cont.png"
                                                                            ToolTip="Weiter" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr id="trRueckZiel" runat="server" visible="false">
                                        <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <div class="StandardHeadDetail" id="divZieladresseRueck" runat="server" onclick="__doPostBack('divZieladresseRueck','')"
                                                style="cursor: pointer; background-color: #576B96">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label31" runat="server" Font-Bold="True" Font-Size="12px" ForeColor="White">Zieladresse Fzg. 2</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ibtHeaderZielRueckClose" runat="server" ImageUrl="/services/Images/versand/minusred.png"
                                                                Style="padding-right: 5px; padding-top: 3px" Visible="false" />
                                                            <asp:ImageButton ID="ibtHeaderZielRueck" runat="server" Style="padding-right: 5px;
                                                                padding-top: 3px" ImageUrl="/services/Images/versand/plusgreen.png" Visible="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="pnlRueckZieladresse" Style="padding-left: 0px" runat="server" Width="100%"
                                                DefaultButton="ibtZielSucheRu" Visible="false">
                                                <div class="StandardHeadDetailFlag2">
                                                </div>
                                                <table cellspacing="0" cellpadding="0" style="padding-left: 0px">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" cellspacing="0" cellpadding="0">
                                                                <tr id="trZielAdresseRu" runat="server" visible="false">
                                                                    <td class="First" style="padding-left: 0px">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlZieladresseRu" runat="server" Width="306" AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="lblErrZielSucheRu" runat="server" ForeColor="Red" Visible="False">Es wurden keine Adressen gefunden.</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RuZielFirma" runat="server">lbl_RuZielFirma</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRuZielFirma" runat="server" Width="270px" MaxLength="35"></asp:TextBox><asp:ImageButton
                                                                            ID="ibtZielSucheRu" runat="server" ImageUrl="/services/images/Versand/search.png"
                                                                            ToolTip="Suchen" Width="20px" Height="20px" Style="padding-left: 5px" />
                                                                        <asp:ImageButton ID="ibtZielResetRu" runat="server" ImageUrl="/services/images/Logistik/loesch.gif"
                                                                            ToolTip="Zurücksetzen" Style="padding-left: 5px" Visible="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RuZielStrasse" runat="server">lbl_RuZielStrasse</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRuZielStrasse" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RuZielPlzOrt" runat="server">lbl_RuZielPlzOrt</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRuZielPlz" runat="server" Width="60px" MaxLength="7"></asp:TextBox><span>&nbsp;&nbsp;</span>
                                                                        <asp:TextBox ID="txtRuZielOrt" Width="222px" runat="server" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RuZielLand" runat="server">lbl_RuZielLand</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlRuZielLand" runat="server" AutoPostBack="True" Width="306px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RuZielAnsprechpartner" runat="server">lbl_RuZielAnsprechpartner</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRuZielAnsprechpartner" runat="server" Width="300px" MaxLength="35"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RuZielTelefon" runat="server">lbl_RuZielTelefon</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRuZielTelefon" runat="server" Width="300px" MaxLength="16"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RuZielDatum" runat="server">lbl_RuZielDatum</asp:Label>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:TextBox ID="txtRuZielDatum" runat="server" Width="300px"></asp:TextBox><cc1:TextBoxWatermarkExtender
                                                                            ID="ExRuZielDatum" runat="server" TargetControlID="txtRuZielDatum" WatermarkText="kein Fixtermin"
                                                                            WatermarkCssClass="Watermark">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <asp:ImageButton ID="ImageButton22" Style="padding-right: 0px; padding-bottom: 4px"
                                                                            ToolTip="Falls Ihr gewünschtes Überführungsdatum mehr als 3 Tage in der Zukunft liegt, können Sie es hier angeben (kein Pflichtfeld)."
                                                                            runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtRuZielDatum" 
                                                                            OnClientDateSelectionChanged="checkAbDatum">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" style="padding-left: 0px">
                                                                        <asp:Label ID="lbl_RuZielUhrzeit" runat="server">lbl_RuZielUhrzeit</asp:Label>
                                                                    </td>
                                                                    <td nowrap="nowrap">
                                                                        <asp:DropDownList ID="ddlRuZielUhrzeit" runat="server" Width="306px">
                                                                        </asp:DropDownList>
                                                                        <asp:ImageButton ID="ImageButton18" Style="padding-right: 0px; padding-bottom: 4px"
                                                                            ToolTip="Hier können Sie zusätzlich zum Datum eine gewünschte Uhrzeit für die Überführung angeben (kein Pflichtfeld)."
                                                                            runat="server" ImageUrl="/Services/Images/fragezeichen.gif" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" style="width: 100%">
                                                            <table cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td style="padding-left: 2px">
                                                                        Transporttyp:
                                                                    </td>
                                                                    <td style="width: 73%">
                                                                        <div id="divRuTransporttyp" runat="server" style="width: 100%">
                                                                            <asp:DropDownList ID="ddlRuTransporttyp" runat="server" Width="100%">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                    <td style="width: 9%">
                                                                        <asp:ImageButton ID="ImageButton27" runat="server" ImageUrl="/Services/Images/fragezeichen.gif"
                                                                            Style="padding-right: 0px; padding-left: 2px; padding-bottom: 4px" ToolTip="Über den Transporttyp wird die Art der Fahrt definiert, und es werden automatisch unterschiedliche Dienstleistungen ermittelt." />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" style="height: 120px; padding-left: 2px">
                                                                        <asp:Panel ID="Panel16" runat="server" Width="92%" Height="80px" Style="background-image: url(/services/Images/Logistik/BackDetails.png);
                                                                            background-repeat: no-repeat">
                                                                            <div style="padding-top: 5px">
                                                                                <span class="style1" style="padding-left: 5px"><b>Fahrzeugdaten</b></span><br />
                                                                                <br />
                                                                                <asp:Label ID="Label47" runat="server" Height="15px" Style="padding-left: 5px">Fahrzeugtyp: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblRuDeTyp" runat="server" Height="15px"></asp:Label><br />
                                                                                <asp:Label ID="Label49" runat="server" Height="15px" Style="padding-left: 5px">Kennzeichen: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblRuDeKennzeichen" runat="server" Height="15px"></asp:Label><br />
                                                                                <asp:Label ID="Label51" runat="server" Height="12px" Style="padding-left: 5px">Fahrgestellnummer: </asp:Label>&nbsp;
                                                                                <asp:Label ID="lblRuDeFin" runat="server" Height="12px"></asp:Label></div>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" style="padding-left: 2px;" valign="bottom">
                                                                        <asp:ImageButton ID="ibtCopyAbholadresse" runat="server" ImageUrl="/Services/Images/copy.gif"
                                                                            ToolTip="Abholadresse übernehmen." />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" style="padding-right: 20px; padding-top: 5px; height: 65px" valign="bottom"
                                                                        align="right">
                                                                        <asp:ImageButton ID="ibtRueckZielNext" runat="server" ImageUrl="/services/images/cont.png"
                                                                            ToolTip="Weiter" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" style="padding-top: 0px;">
                                            &nbsp;
                                            <asp:Label ID="lblErrorFahrtBottom" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td colspan="6" style="margin-top: 10px; margin-bottom: 31px; padding-top: 0px; padding-right: 5px;
                                            text-align: right;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="VersandTabPanel3" class="VersandTabPanel" runat="server" visible="false">
                                <table cellspacing="0" cellpadding="0" width="100%">
                                    <tr>
                                        <td style="padding-bottom: 0px;" class="PanelHead">
                                            <asp:Label ID="Label5" runat="server">Auswahl zusätzlicher Dienstleistungen</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 0px;">
                                            <asp:Label ID="Label6" runat="server" Text="Hier können Sie zusätzlich durchzuführende Aufgaben je Einzelfahrt beauftragen."></asp:Label><br />
                                            <asp:Label ID="lblErrorDienst" runat="server" ForeColor="Red" Visible="False">Bitte überprüfen Sie Ihre Bemerkungen: Es sind max. 200 Zeichen erlaubt.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trZusatzDL" runat="server" visible="false">
                                        <td style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <span>
                                                <asp:GridView ID="grvZusatzDL" runat="server" GridLines="Horizontal" ShowHeader="False"
                                                    Width="100%" AutoGenerateColumns="False" BackColor="#2A4889" Style="padding-bottom: 5px;"
                                                    Visible="False">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-Font-Bold="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ID" runat="server" Text='<%# Bind("FAHRT") %>' Visible="false"></asp:Label><asp:Image
                                                                    ID="imgTooltipZusatz" runat="server" ImageUrl="/Services/images/info.gif" ToolTip='<%# Bind("InfoTooltip") %>' />
                                                                <asp:Label ID="lblGrOrt" runat="server" Text='<%# Bind("Info") %>' ForeColor="White"></asp:Label></ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-ForeColor="White">
                                                            <ItemTemplate>
                                                                (<asp:Label ID="lblGrvTransporttyp" runat="server" Text='<%# Bind("Transporttyp") %>'
                                                                    ForeColor="White"></asp:Label>)</ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtDlZuClose" runat="server" Style="padding-right: 3px; padding-top: 3px"
                                                                    ImageUrl="/services/Images/versand/minusred.png" Visible="false" CommandName="Close"
                                                                    CommandArgument="<%# Container.DataItemIndex %>" />
                                                                <asp:ImageButton ID="ibtDlZuOpen" runat="server" Style="padding-right: 3px; padding-top: 3px"
                                                                    ImageUrl="/services/Images/versand/plusgreen.png" CommandName="Open" CommandArgument="<%# Container.DataItemIndex %>" />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr id="trZusatz2DL" runat="server" visible="false">
                                        <td style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <asp:Panel ID="pnlZusatzDL" BackColor="#DCDCDC" runat="server" Width="100%">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td class="ListService" style="height: 15px; width: 30%;" valign="top">
                                                            <div style="margin-left: 15px; height: 230px; overflow: auto;">
                                                                <asp:CheckBoxList ID="chkZusatzGruende" runat="server">
                                                                </asp:CheckBoxList>
                                                            </div>
                                                        </td>
                                                        <td style="width: 10%;" valign="top">
                                                            <asp:Label ID="Label16" runat="server" Style="padding-top: 10px">Bemerkung:</asp:Label>
                                                        </td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtBemerkungZusatz" runat="server" Height="70px" TextMode="MultiLine"
                                                                Width="98%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px;" nowrap="nowrap">
                                                            <asp:Label ID="Label17" runat="server" Height="15px">Dienstleistungen und Pakete</asp:Label>&nbsp;&nbsp;
                                                            <asp:ImageButton ID="ibtnShowOptionsZusatz" runat="server" ImageUrl="/services/Images/versand/plusgreen.png"
                                                                Style="padding-right: 5px; padding-top: 3px;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="right" style="padding-right: 20px; padding-top: 5px;">
                                                            <asp:ImageButton ID="ibtDlZusatz" runat="server" ImageUrl="/services/images/cont.png"
                                                                ToolTip="Weiter" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 0px; padding-left: 7px; padding-right: 5px; padding-bottom: 0px">
                                            <div class="StandardHeadDetail" id="divDlZieladresse" runat="server" onclick="__doPostBack('divDlZieladresse','')"
                                                style="cursor: pointer; background-color: #576B96">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="12px" ForeColor="White">Zieladresse</asp:Label>&nbsp;
                                                            <asp:Label ID="lblAbTransporttyp" runat="server" Font-Size="X-Small" ForeColor="White"></asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ImageButton12" Style="padding-right: 5px; padding-top: 3px"
                                                                ToolTip="" runat="server" ImageUrl="/Services/Images/info.gif" />
                                                            <asp:ImageButton ID="ibtHeaderDlZieladresseClose" runat="server" ImageUrl="/services/Images/versand/minusred.png"
                                                                Style="padding-right: 5px; padding-top: 3px" Visible="false" />
                                                            <asp:ImageButton ID="ibtHeaderDlZieladresse" runat="server" Style="padding-right: 5px;
                                                                padding-top: 3px" ImageUrl="/services/Images/versand/plusgreen.png" Visible="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="pnlDlZieladresseFirma" Style="padding-left: 0px; padding-bottom: 0px"
                                                runat="server" Width="100%" Visible="false" BackColor="#DCDCDC">
                                                <div class="StandardHeadDetailFlag">
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr id="trDLZieladresse" runat="server" visible="false">
                                        <td style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <asp:Panel ID="pnlDLZieladresse" BackColor="#DCDCDC" runat="server" Width="100%">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="width: 30%">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td class="ListService" style="height: 15px; width: 30%;" valign="top">
                                                                        <div style="margin-left: 15px; height: 230px; overflow: auto;">
                                                                            <asp:CheckBoxList ID="chkGruende" runat="server">
                                                                            </asp:CheckBoxList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" nowrap="nowrap" style="height: 60px; padding-left: 0px" valign="bottom"
                                                                        align="left">
                                                                        <asp:Label ID="lbl_Dienstleistungenopt" runat="server" Height="15px" Style="padding-left: 0px">Dienstleistungen und Pakete</asp:Label>&nbsp;&nbsp;
                                                                        <asp:ImageButton ID="ibtnShowOptions" runat="server" ImageUrl="/services/Images/versand/plusgreen.png"
                                                                            Style="padding-right: 5px; padding-top: 3px;" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 10%;" valign="top">
                                                            <asp:Label ID="lblAbBemerkung" runat="server" Style="padding-top: 10px">Bemerkung:</asp:Label>
                                                        </td>
                                                        <td valign="top">
                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAbBemerkung" runat="server" Height="70px" TextMode="MultiLine"
                                                                            Width="98%"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="ibtnProtokollUpload1" runat="server" ImageUrl="/Services/Images/pdf-logo.png"
                                                                            Style="height: 25px; width: 22px; padding-right: 5px; padding-top: 3px;" />
                                                                        <asp:Label ID="lblProtokollUpload1" Font-Bold="true" runat="server" Height="20px">Upload Dokumente</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trProtokollUpload0" runat="server" visible="false">
                                                                    <td style="padding-right: 5px;">
                                                                        <div class="StandardHeadDetail" style="width: 515px; background-image: url(/Services/Images/Versand/Uploadoverflow.png);">
                                                                            <table>
                                                                                <tr>
                                                                                    <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                                                        <asp:Label ID="Label44" Style="padding-left: 5px; vertical-align: middle" runat="server"
                                                                                            ForeColor="White" Font-Size="12px" Font-Bold="True">Dateiupload</asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div class="StandardHeadDetailFlag">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trProtokollUpload1" runat="server" visible="false">
                                                                    <td>
                                                                        <asp:GridView ID="grvProtokollUpload1" runat="server" AutoGenerateColumns="False"
                                                                            GridLines="Both" Width="98%" BackColor="#ffffff" ShowHeader="true">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderText="Nr." />
                                                                                <asp:TemplateField Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblFahrt" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.Fahrt")  %>'></asp:Label></ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="ZZPROTOKOLLART" HeaderText="Dokument" />
                                                                                <asp:TemplateField HeaderText="Datei">
                                                                                    <ItemTemplate>
                                                                                        <telerik:RadAsyncUpload runat="server" ID="radUpload1" AllowedFileExtensions="pdf"
                                                                                            AllowedMimeTypes="pdf" MaxFileInputsCount="1" MultipleFileSelection="Disabled"
                                                                                            OnClientFileUploadFailed="onUploadFailed" OnClientFileUploaded="onFileUploaded"
                                                                                            OnFileUploaded="UploadedComplete" OnClientValidationFailed="validationFailed"
                                                                                            ToolTip='<%# CType(Container, GridViewRow).RowIndex  %>' Visible='<%# DataBinder.Eval(Container, "DataItem.Filename")= "" %>'
                                                                                            DisablePlugins="true" Width="350px" InputSize="30" Localization-Cancel="Abbrechen"
                                                                                            Localization-Remove="Löschen" Localization-Select="Auswählen">
                                                                                            <Localization Select="Hochladen" />
                                                                                        </telerik:RadAsyncUpload>
                                                                                        <asp:Label Width="350px" ID="lblUplFile" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Filename")<> "" %>'
                                                                                            Text='<%# System.IO.Path.GetFilename(DataBinder.Eval(Container, "DataItem.Filename"))  %>' />
                                                                                        <%--<cc1:AsyncFileUpload runat="server" ID="AsyncFileUpload1" Width="320px" UploaderStyle="Traditional"
                                                                                            Visible='<% # DataBinder.Eval(Container, "DataItem.Filename")= "" %>' CompleteBackColor="#00A300"
                                                                                            OnClientUploadComplete="showUploadStartMessage" ThrobberID="Throbber" OnUploadedComplete="AsyncFileUpload1_UploadedComplete"
                                                                                            ToolTip='<%# CType(Container, GridViewRow).RowIndex  %>' />
                                                                                        <asp:Label ID="Throbber" runat="server" Style="display: none"> <img  id="imgWait"  alt="warten.." src="/Services/Images/indicator.gif"/></asp:Label>
                                                                                        <asp:Label ID="Label43" runat="server" Visible='<% # DataBinder.Eval(Container, "DataItem.Filename")<> "" %>'
                                                                                            Text='<% # DataBinder.Eval(Container, "DataItem.Filename")  %>'></asp:Label>--%>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="370px" />
                                                                                    <ItemStyle Width="360px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Löschen">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ibtnDelUploadFile1" CommandArgument='<% # DataBinder.Eval(Container, "DataItem.ID")  %>'
                                                                                            CommandName="Loeschen" runat="server" ImageUrl="/Services/Images/del.png" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                        <asp:Label ID="lblUploadMessage1" Font-Bold="true" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right" style="padding-right: 20px; padding-top: 5px;">
                                                            <asp:ImageButton ID="ibtDlZielNext" runat="server" ImageUrl="/services/images/cont.png"
                                                                ToolTip="Weiter" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblRueckDL" runat="server" cellspacing="0" cellpadding="0" width="100%"
                                    visible="false">
                                    <tr>
                                        <td style="padding-left: 7px; padding-top: 0px; width: 50%">
                                            <div class="PanelHeadSuche">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label19" runat="server">Rückholung</asp:Label>&nbsp;
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trRueckZusatzDL" runat="server" visible="false">
                                        <td style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <span>
                                                <asp:GridView ID="grvRueckZusatzDL" runat="server" GridLines="Horizontal" ShowHeader="False"
                                                    Width="100%" AutoGenerateColumns="False" BackColor="#2A4889" Style="padding-bottom: 5px;">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-Font-Bold="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ID" runat="server" Text='<%# Bind("FAHRT") %>' Visible="false"></asp:Label><asp:Image
                                                                    ID="imgRTooltipZusatz" runat="server" ImageUrl="/Services/images/info.gif" ToolTip='<%# Bind("InfoTooltip") %>' />
                                                                <asp:Label ID="lblGrOrt" runat="server" Text='<%# Bind("Info") %>' ForeColor="White"></asp:Label></ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-ForeColor="White">
                                                            <ItemTemplate>
                                                                (<asp:Label ID="lblGrvTransporttyp" runat="server" Text='<%# Bind("Transporttyp") %>'
                                                                    ForeColor="White"></asp:Label>)</ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="gridLines" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtDlRuClose" runat="server" Style="padding-right: 3px; padding-top: 3px"
                                                                    ImageUrl="/services/Images/versand/minusred.png" Visible="false" CommandName="Close"
                                                                    CommandArgument="<%# Container.DataItemIndex %>" />
                                                                <asp:ImageButton ID="ibtDlRuOpen" runat="server" Style="padding-right: 3px; padding-top: 3px"
                                                                    ImageUrl="/services/Images/versand/plusgreen.png" CommandName="Open" CommandArgument="<%# Container.DataItemIndex %>" />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="gridLines" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr id="trRuZusatzDL" runat="server" visible="false">
                                        <td style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <asp:Panel ID="pnlRuZusatzDL" BackColor="#DCDCDC" runat="server" Width="100%">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td class="ListService" style="height: 15px; width: 30%;" valign="top">
                                                            <div style="margin-left: 15px; height: 230px; overflow: auto;">
                                                                <asp:CheckBoxList ID="chkRueckZusatzGruende" runat="server">
                                                                </asp:CheckBoxList>
                                                            </div>
                                                        </td>
                                                        <td style="width: 10%;" valign="top">
                                                            <asp:Label ID="Label23" runat="server" Style="padding-top: 10px">Bemerkung:</asp:Label>
                                                        </td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtRueckBemZusatz" runat="server" Height="70px" TextMode="MultiLine"
                                                                Width="98%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px;" nowrap="nowrap">
                                                            <asp:Label ID="Label24" runat="server" Height="15px">Dienstleistungen und Pakete</asp:Label>&nbsp;&nbsp;
                                                            <asp:ImageButton ID="ibtShowOptionsRueckZusatz" runat="server" ImageUrl="/services/Images/versand/plusgreen.png"
                                                                Style="padding-right: 5px; padding-top: 3px" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="First" style="padding-left: 21px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="right" style="padding-right: 20px; padding-top: 5px;">
                                                            <asp:ImageButton ID="ibtDLRuZusatz" runat="server" ImageUrl="/services/images/cont.png"
                                                                ToolTip="Weiter" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 7px; padding-right: 5px; padding-bottom: 0px; padding-top: 0px">
                                            <div class="StandardHeadDetail" id="divDlRuZieladresse" runat="server" onclick="__doPostBack('divDlRuZieladresse','')"
                                                style="cursor: pointer; background-color: #576B96">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="padding-left: 8px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label34" runat="server" Font-Bold="True" Font-Size="12px" ForeColor="White">Zieladresse</asp:Label>&nbsp;
                                                            <asp:Label ID="lblRuZielTransporttyp" runat="server" Font-Size="X-Small" ForeColor="White"></asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            <asp:ImageButton ID="ImageButton9" Style="padding-right: 5px; padding-top: 3px" ToolTip=""
                                                                runat="server" ImageUrl="/Services/Images/info.gif" />
                                                            <asp:ImageButton ID="ibtHeaderDlRuZieladresseClose" runat="server" ImageUrl="/services/Images/versand/minusred.png"
                                                                Style="padding-right: 5px; padding-top: 3px" Visible="false" />
                                                            <asp:ImageButton ID="ibtHeaderDlRuZieladresse" runat="server" Style="padding-right: 5px;
                                                                padding-top: 3px" ImageUrl="/services/Images/versand/plusgreen.png" Visible="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="pnlDlRuZieladresseFirma" Style="padding-left: 0px; padding-bottom: 0px"
                                                runat="server" Width="100%" Visible="false" BackColor="#DCDCDC">
                                                <div class="StandardHeadDetailFlag" style="background-color: #576B96">
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                            <asp:Panel ID="pnlDlRuZieladresse" BackColor="#DCDCDC" runat="server" Width="100%"
                                                Visible="false">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 30%;" valign="top">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td class="ListService" style="height: 15px; width: 30%;" valign="top">
                                                                        <div style="margin-left: 15px; height: 230px; overflow: auto;">
                                                                            <asp:CheckBoxList ID="chkGruendeRueck" runat="server">
                                                                            </asp:CheckBoxList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="First" nowrap="nowrap" style="height: 60px; padding-left: 0px" valign="bottom"
                                                                        align="left">
                                                                        <asp:Label ID="Label36" runat="server" Height="15px">Dienstleistungen und Pakete</asp:Label>&nbsp;&nbsp;
                                                                        <asp:ImageButton ID="ibtRueckDL" runat="server" ImageUrl="/services/Images/versand/plusgreen.png"
                                                                            Style="padding-right: 5px; padding-top: 3px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 10%;" valign="top">
                                                            <asp:Label ID="Label32" runat="server" Style="padding-top: 10px">Bemerkung:</asp:Label>
                                                        </td>
                                                        <td valign="top">
                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRueckBemerkung" runat="server" Height="70px" TextMode="MultiLine"
                                                                            Width="98%"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="ibtnProtokollUpload2" runat="server" ImageUrl="/Services/Images/pdf-logo.png"
                                                                            Style="height: 25px; width: 22px; padding-right: 5px; padding-top: 3px;" />
                                                                        <asp:Label ID="lblProtokollUpload2" Font-Bold="true" runat="server" Height="20px">Upload Dokumente</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trProtokollUpload3" runat="server" visible="false">
                                                                    <td style="padding-right: 5px;">
                                                                        <div class="StandardHeadDetail" style="width: 515px; background-image: url(/Services/Images/Versand/Uploadoverflow.png);">
                                                                            <table>
                                                                                <tr>
                                                                                    <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                                                        <asp:Label ID="Label45" Style="padding-left: 5px; vertical-align: middle" runat="server"
                                                                                            ForeColor="White" Font-Size="12px" Font-Bold="True">Dateiupload</asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div class="StandardHeadDetailFlag">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trProtokollUpload2" runat="server" visible="false">
                                                                    <td>
                                                                        <%--<telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecoratedControls="Buttons" />--%>
                                                                        <asp:GridView ID="grvProtokollUpload2" runat="server" AutoGenerateColumns="False"
                                                                            GridLines="Both" Width="98%" BackColor="#ffffff" ShowHeader="true">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderText="Nr." />
                                                                                <asp:TemplateField Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblFahrt" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.Fahrt")  %>'></asp:Label></ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="ZZPROTOKOLLART" HeaderText="Dokument" />
                                                                                <asp:TemplateField HeaderText="Datei">
                                                                                    <ItemTemplate>
                                                                                        <telerik:RadAsyncUpload runat="server" ID="radUpload2" AllowedFileExtensions="pdf"
                                                                                            AllowedMimeTypes="pdf" MaxFileInputsCount="1" MultipleFileSelection="Disabled"
                                                                                            OnClientFileUploadFailed="onUploadFailed" OnClientFileUploaded="onFileUploaded"
                                                                                            OnFileUploaded="UploadedComplete2" OnClientValidationFailed="validationFailed"
                                                                                            ToolTip='<%# CType(Container, GridViewRow).RowIndex  %>' Visible='<%# DataBinder.Eval(Container, "DataItem.Filename")= "" %>'
                                                                                            DisablePlugins="true" Width="350px" InputSize="30" Localization-Cancel="Abbrechen"
                                                                                            Localization-Remove="Löschen" Localization-Select="Auswählen">
                                                                                            <Localization Select="Hochladen" />
                                                                                        </telerik:RadAsyncUpload>
                                                                                        <asp:Label Width="350px" ID="lblUplFile" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Filename")<> "" %>'
                                                                                            Text='<%# System.IO.Path.GetFilename(DataBinder.Eval(Container, "DataItem.Filename"))  %>' />
                                                                                        <%--                                                                                        <cc1:AsyncFileUpload runat="server" Visible='<% # DataBinder.Eval(Container, "DataItem.Filename")= "" %>'
                                                                                            ID="AsyncFileUpload1" Width="320px" UploaderStyle="Traditional" CompleteBackColor="#00A300"
                                                                                            OnClientUploadComplete="showUploadStartMessage" ThrobberID="Throbber" OnUploadedComplete="AsyncFileUpload1_UploadedComplete2"
                                                                                            ToolTip='<%# CType(Container, GridViewRow).RowIndex %>' />
                                                                                        <asp:Label ID="Throbber" runat="server" Style="display: none"> <img  id="imgWait"  alt="warten.." src="/Services/Images/indicator.gif"/></asp:Label><asp:Label
                                                                                         ID="Label43" runat="server" Visible='<% # DataBinder.Eval(Container, "DataItem.Filename")<> "" %>'
                                                                                         Text='<% # DataBinder.Eval(Container, "DataItem.Filename")  %>'></asp:Label>--%>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="360px" />
                                                                                    <ItemStyle Width="350px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Löschen">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="ibtnDelUploadFile1" CommandArgument='<% # DataBinder.Eval(Container, "DataItem.ID")  %>'
                                                                                            CommandName="Loeschen" runat="server" ImageUrl="/Services/Images/del.png" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                        <asp:Label ID="lblUploadMessage2" CssClass="TextError" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right" style="padding-right: 20px; padding-top: 5px; padding-bottom: 5px">
                                                            <asp:ImageButton ID="ibtRuZielNext" runat="server" ImageUrl="/services/images/cont.png"
                                                                ToolTip="Weiter" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <table cellspacing="0" cellpadding="0" width="100%">
                                    <tr>
                                        <td style="margin-top: 10px; margin-bottom: 31px; padding-top: 0px; padding-right: 5px;
                                            text-align: right;">
                                            <div class="dataQueryFooter">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="VersandTabPanel4" class="VersandTabPanel" runat="server" visible="false">
                                <div id="dataFooter" style="position: absolute; z-index: 3; margin-top: 560px; padding-left: 650px;
                                    white-space: nowrap; width: 250px">
                                    <asp:LinkButton ID="lbtNewOrderSameAdress" runat="server" CssClass="TablebuttonLarge"
                                        Style="margin-right: 5px" Width="120px" Height="16px" Visible="False" ToolTip="Neuer Auftrag mit gleichen Adressen.">« Auftrag kopieren</asp:LinkButton>
                                    <asp:LinkButton
                                        ID="lbtnBackToStart" runat="server" CssClass="TablebuttonLarge" Width="110px"
                                        Height="16px" Visible="False">« Neuer Auftrag</asp:LinkButton>
                                    <asp:LinkButton ID="lbtnSend"
                                        runat="server" CssClass="Tablebutton SendeButton" Style="float: right" Width="78px" Height="16px">» Senden</asp:LinkButton>&nbsp;
                                </div>
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="padding-bottom: 0px;" class="PanelHead">
                                            <asp:Label ID="Label27" runat="server">Zusammenfassung</asp:Label>
                                        </td>
                                        <td style="width: 100%" align="right">
                                            <asp:ImageButton ID="ibtnCreatePDF" Style="height: 25px; padding-top: 10px; width: 22px"
                                                ToolTip="PDF herunterladen" runat="server" ImageUrl="/services/Images/pdf-logo.png"
                                                Visible="False" />
                                            &nbsp;<asp:Label ID="lblPDFPrint" runat="server" Text="Auftrag als PDF" Height="20px"
                                                Visible="False" Style="padding-right: 10px; padding-top: 10px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-top: 0px;">
                                            <asp:Label ID="Label40" runat="server">Bitte überprüfen Sie hier noch einmal die Auftragsdaten zur Überführung.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-left: 7px; width: 50%">
                                            <div class="PanelHeadSuche">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                            <asp:Label ID="lblUeKennzeichen" runat="server"></asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="Panel19" Style="padding-left: 15px" runat="server" Width="100%">
                                                <div style="background-image: url(/services/Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                                    height: 8px; width: 16px">
                                                </div>
                                                <table cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 0px">
                                                            <asp:GridView ID="grvUeHin" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                                ShowHeader="False" Width="100%">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Adresse" ItemStyle-Width="250px" />
                                                                    <asp:BoundField DataField="PLZOrt" ItemStyle-Width="300px" />
                                                                    <asp:BoundField DataField="Termin" ItemStyle-Width="200px" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr id="trUeRueck" runat="server" visible="false">
                                        <td colspan="4" style="padding-left: 7px; width: 50%">
                                            <div class="PanelHeadSuche">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                            <asp:Label ID="lblUeRueckKennzeichen" runat="server"></asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="Panel17" Style="padding-left: 15px" runat="server" Width="100%">
                                                <div style="background-image: url(/services/Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                                    height: 8px; width: 16px">
                                                </div>
                                                <table cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 0px">
                                                            <asp:GridView ID="grvUeRueck" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                                ShowHeader="False" Width="100%">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Adresse" ItemStyle-Width="250px" />
                                                                    <asp:BoundField DataField="PLZOrt" ItemStyle-Width="300px" />
                                                                    <asp:BoundField DataField="Termin" ItemStyle-Width="200px" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-left: 7px; width: 50%">
                                            <div class="PanelHeadSuche">
                                                <table width="100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                            <asp:Label ID="Label29" runat="server">Rechnungsdaten</asp:Label>
                                                        </td>
                                                        <td align="right" valign="top" style="padding-bottom: 0px;">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="Panel20" Style="padding-left: 15px" runat="server" Width="100%">
                                                <div style="background-image: url(/services/Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                                    height: 8px; width: 16px">
                                                </div>
                                                <table cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding-left: 0px">
                                                            <asp:GridView ID="grvRechnungsdaten" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                                ShowHeader="False" Width="100%">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Adresse" ItemStyle-Width="250px" />
                                                                    <asp:BoundField DataField="PLZOrt" ItemStyle-Width="500px" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <div id="ResultOverView" visible="false" style="padding-right: 5px; padding-left: 7px"
                                    runat="Server">
                                </div>
                            </div>
                            <cc1:CollapsiblePanelExtender ID="cpeAllData" runat="Server" TargetControlID="pnlAllgDaten"
                                ExpandControlID="divHinfahrt" CollapseControlID="divHinfahrt" Collapsed="false"
                                ImageControlID="NewSearch" ExpandedImage="/services/Images/versand/minusred.png"
                                CollapsedImage="/services/Images/versand/plusgreen.png" SuppressPostBack="true" />
                            <cc1:CollapsiblePanelExtender ID="cpeRueckholung" runat="Server" TargetControlID="pnlRueckholung"
                                ExpandControlID="divFahrzeug2" CollapseControlID="divFahrzeug2" Collapsed="true"
                                ImageControlID="ImageButton1" ExpandedImage="/services/Images/versand/minusred.png"
                                CollapsedImage="/services/Images/versand/plusgreen.png" SuppressPostBack="true" />
                            <cc1:CollapsiblePanelExtender ID="cpeRechnungszahler" runat="Server" TargetControlID="pnlRechnungszahler"
                                ExpandControlID="divRechnungszahler" CollapseControlID="divRechnungszahler" Collapsed="true"
                                ImageControlID="ImageButton2" ExpandedImage="/services/Images/versand/minusred.png"
                                CollapsedImage="/services/Images/versand/plusgreen.png" SuppressPostBack="true" />
                            <cc1:CollapsiblePanelExtender ID="cpeAbwRechnungsadresse" runat="Server" TargetControlID="pnlAbwRechnungsadresse"
                                ExpandControlID="divAbwRechnungsadresse" CollapseControlID="divAbwRechnungsadresse"
                                Collapsed="true" ImageControlID="ImageButton3" ExpandedImage="/services/Images/versand/minusred.png"
                                CollapsedImage="/services/Images/versand/plusgreen.png" SuppressPostBack="true" />
                            <%-- <cc1:CollapsiblePanelExtender ID="cpeZulstelle" runat="Server" TargetControlID="PLZulstelle"
                                ExpandControlID="ImageButton5" CollapseControlID="ImageButton5" Collapsed="true"
                                ImageControlID="ImageButton5" ExpandedImage="/services/Images/versand/minusred.png"
                                CollapsedImage="/services/Images/versand/plusgreen.png" SuppressPostBack="true" />--%>
                            <%--<cc1:CollapsiblePanelExtender ID="cpeAdressmanuell" runat="Server" TargetControlID="PLAdressmanuell"
                                ExpandControlID="ImageButton8" CollapseControlID="ImageButton8" Collapsed="true"
                                ImageControlID="ImageButton8" ExpandedImage="/services/Images/versand/minusred.png"
                                CollapsedImage="/services/Images/versand/plusgreen.png" SuppressPostBack="true" />--%>
                            <script type="text/javascript">
                                function cpeAllDataCollapsed() {

                                    var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAllData');
                                    var Panel2 = $find('ctl00_ContentPlaceHolder1_cpeRueckholung');
                                    var Panel3 = $find('ctl00_ContentPlaceHolder1_cpeRechnungszahler');
                                    var Panel4 = $find('ctl00_ContentPlaceHolder1_cpeAbwRechnungsadresse');
                                    if (Panel1.get_Collapsed() != false) {
                                        Panel2._doClose();
                                        Panel3._doClose();
                                        Panel4._doClose();
                                    }
                                }
                                function cpeCollapsedRueck() {

                                    var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAllData');
                                    var Panel2 = $find('ctl00_ContentPlaceHolder1_cpeRueckholung');
                                    var Panel3 = $find('ctl00_ContentPlaceHolder1_cpeRechnungszahler');
                                    var Panel4 = $find('ctl00_ContentPlaceHolder1_cpeAbwRechnungsadresse');
                                    if (Panel2.get_Collapsed() != false) {
                                        Panel1._doClose();
                                        Panel3._doClose();
                                        Panel4._doClose();
                                    }
                                }

                                function cpeCollapsedRz() {

                                    var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAllData');
                                    var Panel2 = $find('ctl00_ContentPlaceHolder1_cpeRueckholung');
                                    var Panel3 = $find('ctl00_ContentPlaceHolder1_cpeRechnungszahler');
                                    var Panel4 = $find('ctl00_ContentPlaceHolder1_cpeAbwRechnungsadresse');
                                    if (Panel3.get_Collapsed() != false) {
                                        Panel1._doClose();
                                        Panel2._doClose();
                                        Panel4._doClose();
                                    }
                                }
                                function cpeCollapsedRe() {

                                    var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAllData');
                                    var Panel2 = $find('ctl00_ContentPlaceHolder1_cpeRueckholung');
                                    var Panel3 = $find('ctl00_ContentPlaceHolder1_cpeRechnungszahler');
                                    var Panel4 = $find('ctl00_ContentPlaceHolder1_cpeAbwRechnungsadresse');
                                    if (Panel4.get_Collapsed() != false) {
                                        Panel1._doClose();
                                        Panel2._doClose();
                                        Panel3._doClose();
                                    }
                                }
                                function showUploadStartMessage(sender, args) {

                                    var filename = args.get_fileName();
                                    var filext = filename.substring(filename.lastIndexOf(".") + 1);
                                    if (filext == "pdf") {
                                        return true;
                                    } else {

                                        sender._inputFile.style.backgroundColor = "red"
                                        alert('Fehler beim Hochladen der Datei(nur PDF-Format erlaubt!)')
                                        return false;
                                    }
                                }</script>
                        </div>
                    </div>
                </div>
             <%--</ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ibtnCreatePDF" />
            </Triggers>
       </asp:UpdatePanel>--%>
    </div>
</asp:Content>
