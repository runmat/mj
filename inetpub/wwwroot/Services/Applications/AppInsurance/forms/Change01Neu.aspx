<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01Neu.aspx.vb" Inherits="AppInsurance.Change01Neu"
    MasterPageFile="../MasterPage/App.Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <contenttemplate>
                        <div id="divMessage" runat="server" visible="false" style="top: 400px; margin-left: 235px;
                            z-index: 3; width: 400px; position: absolute;">
                            <div style="background-image: url(/services/images/MsgTitle.png); width: 400px;">
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
                                    Style="margin-right: 10px; text-align: center">» OK</asp:LinkButton>
                            </div>
                            <div style="background-image: url(/services/images/MsgBottom.png); background-repeat: no-repeat;
                                height: 6px">
                            </div>
                        </div>
                        <div id="navigationSubmenu">
                            <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                                Text="zurück"></asp:LinkButton>
                        </div>
                        <div id="innerContent">
                            <div id="innerContentRight" style="width: 100%;">
                                <div id="innerContentRightHeading">
                                    <h1>
                                        <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                                </div>
                                <div id="pagination">
                                    <table cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <%--                                    <tr>
                                        <td style="width:100%">
                                            Sie benötigen grüne Versicherungskennzeichen aus dem laufenden Verkehrsjahr 2010/2011? Dann klicken Sie bitte 
                                           <b><a style="text-decoration:underline" href="javascript:openinfo('Bestellung.htm');">hier</a></b> .
                                        </td>
                                    </tr>  --%>
                                            <tr>
                                                <td style="width: 100%">
                                                    <asp:Label ID="lblInfo" runat="server">Bitte tragen Sie Ihre <b>Agenturnummer</b> ein!</asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div id="data">
                                    <div id="divControls" runat="server">
                                        <table cellpadding="" cellspacing="0" style="border-right-width: 1px; border-left-width: 1px;
                                            border-left-style: solid; border-right-style: solid; border-right-color: #DFDFDF;
                                            border-left-color: #DFDFDF">
                                            <tfoot>
                                                <tr>
                                                    <td style="height: 20px" colspan="4">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </tfoot>
                                            <tbody>
                                                <tr>
                                                    <td colspan="4" class="firstLeft active">
                                                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="firstLeft active" colspan="4" style="color: #F44B12">
                                                        <asp:Label ID="lblInfoVer" CssClass="TextError" runat="server">Nach Eingabe der Agenturnummer bestätigen Sie mit "Enter" oder klicken Sie auf den Button "» Weiter".</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_Vermittlernummer" runat="server">Agenturnummer:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="pnlDefault" runat="server" DefaultButton="btndefault">
                                                            <asp:TextBox ID="txtVermittlernummer" runat="server" CssClass="InputTextbox" MaxLength="9"
                                                                Width="100px"></asp:TextBox><ajaxToolkit:MaskedEditExtender ID="txtVermittlernummer_MaskedEditExtender"
                                                                    runat="server" AutoComplete="False" ClearMaskOnLostFocus="False" CultureAMPMPlaceholder=""
                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                    Enabled="True" Mask="CCCC-CCCC-C" Filtered="1234567890" TargetControlID="txtVermittlernummer">
                                                                </ajaxToolkit:MaskedEditExtender>
                                                        </asp:Panel>
                                                    </td>
                                                    <td class="firstLeft active" style="width: 200px">
                                                        <asp:Label ID="lbl_LastOrder" runat="server">Letzte Bestellung am: </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLastOrder" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="">
                                                    <td class="firstLeft active" style="padding-top: 10px;">
                                                        <asp:Label ID="lbl_Anrede" runat="server">Anrede:</asp:Label>
                                                    </td>
                                                    <td style="padding-top: 10px;">
                                                        <span>
                                                            <asp:RadioButtonList ID="rblAnrede" runat="server" BorderColor="White" CellPadding="0"
                                                                Style="padding-top: 0px" CellSpacing="0" Font-Bold="False" RepeatDirection="Horizontal">
                                                                <asp:ListItem Text="Firma" Value="Firma"></asp:ListItem>
                                                                <asp:ListItem Text="Herr" Value="Herr"></asp:ListItem>
                                                                <asp:ListItem Text="Frau" Value="Frau"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </span>
                                                    </td>
                                                    <td class="firstLeft active" style="width: 200px">
                                                        Gesamtanzahl bestellte VKZ Vorjahr:
                                                    </td>
                                                    <td style="width: 5%">
                                                        <asp:Label ID="lblAnz_Ges" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_Vorname" runat="server">Firma (o.Name1):</asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:TextBox ID="txtVorname" runat="server" CssClass="InputTextbox" MaxLength="40"
                                                            Width="240px"></asp:TextBox>
                                                        <asp:Image ID="Image1" runat="server" AlternateText="Info" Height="20px" ImageAlign="AbsMiddle"
                                                            ImageUrl="../../../Images/Info01_06.jpg" ToolTip="(z.B. Agentur etc.&nbsp;oder Erika Mustermann)"
                                                            Width="20px" />
                                                    </td>
                                                    <td class="firstLeft active" style="display:none;width: 200px">
                                                        Davon ausgegeben und dokumentiert:
                                                    </td>
                                                    <td style="width: 5%;display:none">
                                                        <asp:Label ID="lblAnz_Off" runat="server" Height="18px"></asp:Label>
                                                        <asp:Image ID="Image5" runat="server" AlternateText="Info" Height="20px" ImageAlign="AbsMiddle"
                                                            ImageUrl="../../../Images/Info01_06.jpg" ToolTip="Aufgeführt sind alle VKZ die bis zum heutigen Tag im System dokumentiert sind."
                                                            Width="20px" />
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_Name" runat="server">Name 2:</asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="InputTextbox" MaxLength="40" Width="240px"></asp:TextBox>
                                                        <asp:Image ID="Image2" runat="server" AlternateText="Info" Height="20px" ImageUrl="../../../Images/Info01_06.jpg"
                                                            ToolTip="(z.B. Erika Mustermann)" Width="20px" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td class="firstLeft active" style="width: 200px">
                                                        <asp:Label ID="lbl_Versicherungsjahr" runat="server">Verkehrsjahr:</asp:Label>
                                                    </td>
                                                    <td style="width: 100%">
                                                        <asp:RadioButtonList ID="rblVersicherungsjahr" runat="server" RepeatDirection="Horizontal"
                                                            RepeatLayout="Flow">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_Strasse" runat="server">Strasse / Nr.:</asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:TextBox ID="txtStrasse" runat="server" CssClass="InputTextbox" MaxLength="60"
                                                            Width="176px"></asp:TextBox>
                                                        <asp:TextBox ID="txtHausnummer" runat="server" CssClass="InputTextbox" MaxLength="10"
                                                            Width="57px"></asp:TextBox>
                                                        <asp:Image ID="Image4" runat="server" AlternateText="Info" Height="20px" ImageAlign="AbsMiddle"
                                                            ImageUrl="../../../Images/Info01_06.jpg" ToolTip="Pflichtfeld! Sollten Sie keine Hausnummer haben, bitte ein ''-'' eintragen."
                                                            Width="20px" />
                                                    </td>
                                                    <td class="firstLeft active" style="width: 200px">
                                                        <asp:Label ID="lbl_AnzahlKennzeichen" runat="server">Anzahl Kennzeichen:</asp:Label>
                                                    </td>
                                                    <td style="width: 100%">
                                                        <asp:DropDownList ID="ddlAnzahlKennzeichen" runat="server" Width="70px" Font-Names="Verdana,sans-serif">
    
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_Postleitzahl" runat="server">Postleitzahl / Ort:</asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:TextBox ID="txtPostleitzahl" runat="server" CssClass="InputTextbox" MaxLength="5"
                                                            Width="57px"></asp:TextBox>
                                                        <asp:TextBox ID="txtOrt" runat="server" CssClass="InputTextbox" MaxLength="40" Width="176px"></asp:TextBox>
                                                    </td>
                                                    <td class="firstLeft active" style="padding-top: 10px; width: 200px" rowspan="2">
                                                        <asp:Label ID="lbl_AnzahlAAP" runat="server" Text="">
                                                            Anzahl A4-aap-Vordrucke<br />
                                                            <u>OHNE</u> Durchschreibesatz:<br />
                                                            (Für A5-Blockpolicen mit<br />
                                                            Durchschreibesatz beachten<br />
                                                            Sie die Hilfe "i")
                                                        </asp:Label>
                                                    </td>
                                                    <td class="rightPadding" dir="ltr" rowspan="2" style="vertical-align:bottom">
                                                        <asp:TextBox ID="txtAnzalAAP" runat="server" MaxLength="3" Width="50px" CssClass="InputTextbox"></asp:TextBox>
                                                        <a class="tip" href="#">
                                                            <img alt="" border="0" height="20px" src="/Services/images/Info01_06.jpg" width="20px" style="vertical-align:middle" />
                                                            <span style="left:-110px">
                                                                <table id="InfoTab" cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <b>AAP-Anwender:</b><br />
                                                                            Geben Sie hier bitte die Anzahl der A4 Vordrucke an, wenn Sie selbst und direkt
                                                                            über das AAP policieren. Über die Differenz der A4 Vordrucke zur eingegebenen Kennzeichenmenge
                                                                            erhalten Sie A5 Blockpolicen mit Durchschreibesatz. Bei Eingabe "0" erhalten Sie
                                                                            aussschließlich A5 Blockpolicen mit Durchschreibesatz.
                                                                            <br />
                                                                            <b>Nicht-AAP-Anwender oder AAP-Anwender die nicht selbst und direkt policieren:
                                                                            </b>
                                                                            <br />
                                                                            Geben Sie hier bitte immer die Anzahl "0" an, dann erhalten Sie ausschließlich A5-Blockpolicen
                                                                            mit Durchschreibesatz.
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </span> 
                                                        </a>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active" style="height: 34px">
                                                        <asp:Label ID="lbl_Tel" runat="server">Telefon:</asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap" style="height: 34px">
                                                        <asp:TextBox ID="txt_Tel" runat="server" CssClass="InputTextbox" Width="240px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_EmailAdresse" runat="server">E-Mail:</asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:TextBox ID="txtEmailAdresse" runat="server" CssClass="InputTextbox" MaxLength="241"
                                                            onKeyup="javascript:return findeUmlaute(this,this.value)" Width="240px"></asp:TextBox>
                                                        <asp:Image ID="Image3" runat="server" AlternateText="Info" Height="20px" ImageUrl="../../../Images/Info01_06.jpg"
                                                            ToolTip="Angabe Voraussetzung für Bestellung" Width="20px" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td colspan="2">
                                                        <ajaxToolkit:TextBoxWatermarkExtender ID="extWatermarkEmail" runat="server" TargetControlID="txtEmailAdresse"
                                                            WatermarkText="Umlaute(ä,ö,ü) sind nicht erlaubt." WatermarkCssClass="TextError">
                                                        </ajaxToolkit:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="Label2" runat="server">E-Mail bestätigen:</asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:TextBox ID="txtMailConfirm" runat="server" CssClass="InputTextbox" MaxLength="241"
                                                            onKeyup="javascript:return findeUmlaute(this,this.value)" Width="240px" oncopy="return false"
                                                            onpaste="return false" oncut="return false"></asp:TextBox>
                                                        <asp:Image ID="Image6" runat="server" AlternateText="Info" Height="20px" ImageUrl="../../../Images/Info01_06.jpg"
                                                            ToolTip="Angabe Voraussetzung für Bestellung" Width="20px" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td colspan="2">
                                                        <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                                                            TargetControlID="txtMailConfirm" WatermarkText="Umlaute(ä,ö,ü) sind nicht erlaubt."
                                                            WatermarkCssClass="TextError">
                                                        </ajaxToolkit:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        abw. Versandadresse:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkAdresse" runat="server" AutoPostBack="True" BorderColor="White"
                                                            BorderStyle="None" />
                                                    </td>
                                                    <td class="firstLeft active" style="width: 200px">
                                                        <asp:Label ID="Label1" runat="server">Versandart:</asp:Label>
                                                    </td>
                                                    <td class="rightPadding">
                                                        Standard (Versandkosten trägt
                                                        <a class="tip" href="#">
                                                            <img alt="" border="0" height="20px" src="/Services/images/Info01_06.jpg" width="20px" style="vertical-align:middle" />
                                                            <span style="left:-210px">
                                                                <table id="Table1" cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <strong><u>Hinweis:</u></strong><br />
                                                                            Die Deutsche Post AG garantiert für<br />
                                                                            <b><i>Standardsendungen</i></b> keine Zustellzeiten<br />
                                                                            und gibt die Zustellwahrscheinlichkeit wie folgt an:<br />
                                                                            <br />
                                                                            &nbsp;&nbsp;&nbsp;- 95% aller Sendungen werden dem Empfänger<br />
                                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;innerhalb von 24 Stunden zugestellt,<br />
                                                                            &nbsp;&nbsp;&nbsp;- 3% aller Sendungen benötigen zwischen<br />
                                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;24 und 48 Stunden bis zur Zustellung.
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </span> 
                                                        </a>
                                                        Versicherungsgesellschaft)
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_KeineEmailVorhanden" runat="server" Visible="False">Keine E-Mail vorhanden:</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:CheckBox ID="chkKeineEmailVorhanden" runat="server" BorderColor="White" BorderStyle="None"
                                                            Visible="False" />
                                                    </td>
                                                </tr>
                                                <tr class="">
                                                    <td class="firstLeft active" style="padding-top: 5px;">
                                                        <asp:Label ID="lbl_WEAnrede" runat="server" Visible="False">Anrede:</asp:Label>
                                                    </td>
                                                    <td align="left" style="padding-top: 5px;">
                                                        <span>
                                                            <asp:RadioButtonList ID="rblWE_Anrede" runat="server" BorderColor="White" CellPadding="0"
                                                                CellSpacing="0" Font-Bold="False" RepeatDirection="Horizontal" Visible="false">
                                                                <asp:ListItem Text="Firma" Value="Firma"></asp:ListItem>
                                                                <asp:ListItem Text="Herr" Value="Herr"></asp:ListItem>
                                                                <asp:ListItem Text="Frau" Value="Frau"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </span>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td rowspan="6">
                                                        <strong><u>Hinweis: </u></strong>
                                                        <span style="font-weight: normal; font-style: inherit;">
                                                            <br />
                                                            Versicherungskennzeichen und deren Vertrags-<br />
                                                            unterlagen sind teuer! Bei Bestellvorgängen<br />
                                                            über uns möchten wir Sie bitten, die Bestell-<br />
                                                            menge möglichst objektiv zu kalkulieren.<br />
                                                            Die Württembergische Versicherung AG behält<br />
                                                            sich vor, Bestellungen zu reduzieren oder zu<br />
                                                            stornieren, sollte auf den Agenturen noch ein<br />
                                                            großer Bestand an noch nicht verkauften<br />
                                                            Versicherungskennzeichen vorhanden sein. 
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active" style="padding-top: 10px">
                                                        <asp:Label ID="lbl_WEVorname" runat="server" Visible="False">Firma (o.Name1):</asp:Label>
                                                    </td>
                                                    <td style="padding-top: 10px" colspan="2">
                                                        <asp:TextBox ID="txtWE_Vorname" runat="server" CssClass="InputTextbox" MaxLength="40"
                                                            Visible="false" Width="247px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_WEName" runat="server" Visible="False">Name 2:</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtWE_Name" runat="server" CssClass="InputTextbox" MaxLength="40"
                                                            Visible="false" Width="247px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_WEStrasse" runat="server" Visible="False">Strasse / Nr.:</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtWE_Strasse" runat="server" CssClass="InputTextbox" MaxLength="60"
                                                            Visible="false" Width="178px"></asp:TextBox>
                                                        <asp:TextBox ID="txtWE_Hausnummer" runat="server" CssClass="InputTextbox" MaxLength="10"
                                                            Visible="false" Width="60px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_WEPostleitzahl" runat="server">Postleitzahl / Ort:</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtWE_Postleitzahl" runat="server" CssClass="InputTextbox" MaxLength="5"
                                                            Visible="false" Width="60px"></asp:TextBox>
                                                        <asp:TextBox ID="txtWE_Ort" runat="server" CssClass="InputTextbox" MaxLength="40"
                                                            Visible="false" Width="178px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_WETel" runat="server" Visible="False">Telefon:</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtWE_Tel" runat="server" CssClass="InputTextbox" Visible="false"
                                                            Width="247px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td colspan="4">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td colspan="4">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="form">
                                                    <td colspan="3">
                                                        &nbsp;
                                                    </td>
                                                    <td align="right" class="rightPadding">
                                                        <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                                            Text="Button" />
                                                        <asp:LinkButton ID="cmdContinue" runat="server" CssClass="Tablebutton" Height="16px"
                                                            Text="&amp;nbsp;&amp;#187; Weiter" Width="78px"></asp:LinkButton>
                                                        <asp:LinkButton ID="cmdConfirm" runat="server" CssClass="Tablebutton" Height="16px"
                                                            Text="&amp;nbsp;&amp;#187; Absenden" Visible="False" Width="78px"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="3">
                                                        <div>
                                                            <asp:Button ID="btnFake" runat="server" Style="display: none" Text="Fake" />
                                                            <asp:Button ID="Button1" runat="server" OnClick="Button1Click" Text="BUTTON" Visible="False" />
                                                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" BackgroundCssClass="modalBackground"
                                                                CancelControlID="btnCancel" DropShadow="true" PopupControlID="mb" TargetControlID="btnFake"
                                                                X="450" Y="200">
                                                            </ajaxToolkit:ModalPopupExtender>
                                                            <asp:Panel ID="mb" runat="server" BackColor="White" Height="160px" Width="450px"
                                                                Style="display: none">
                                                                <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px;">
                                                                    <asp:Label ID="lblAdressMessage" runat="server" Font-Bold="True" Text="Es wurden Adressalternativen gefunden."></asp:Label>
                                                                </div>
                                                                <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; padding-bottom: 10px">
                                                                    <asp:DropDownList ID="ddlAlternativAdressen" runat="server" Width="420px">
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Button ID="btnOK" runat="server" CssClass="TablebuttonLarge" Font-Bold="True"
                                                                                Text="Übernehmen" Width="90px" />
                                                                        </td>
                                                                        <td align="left" style="width: 100px; padding-left: 5px; padding-right: 10px">
                                                                            <asp:Button ID="btnCancel" runat="server" CssClass="TablebuttonLarge" Font-Bold="true"
                                                                                Text="Ablehnen" Width="90px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div id="divInfoText" runat="server" style="padding: 120px 0px 150px 150px; width: 700px"
                                        visible="false">
                                        <span style="color: #000099; font-size: large"><b>Sie erhalten in Kürze eine e-Mail
                                            von noreply.sgw@kroschke.de.</b></span><br />
                                        <b><span style="font-size: large; color: #000099">Hierbei handelt es sich um eine Sicherheitsabfrage.</span></b>
                                        <br />
                                        <br />
                                        <b><span style="font-size: large; color: #FF0000">Bitte bestätigen Sie darin Ihre Bestellung,
                                            damit die Ausführung erfolgen kann.</span></b>
                                    </div>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                        </div>
                    </contenttemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        function findeUmlaute(Ctrl, Value) {
            if (Value.toLowerCase().indexOf("ö") != -1) {
                alert("Bitte geben Sie keine Umlaute ein!");
                Ctrl.value = Ctrl.value.replace(/ö/g, "");
                Ctrl.value = Ctrl.value.replace(/ä/g, "");
                Ctrl.value = Ctrl.value.replace(/ü/g, "");
                return false;
            }
            else if (Value.toLowerCase().indexOf("ä") != -1) {
                alert("Bitte geben Sie keine Umlaute ein!");
                Ctrl.value = Ctrl.value.replace(/ö/g, "");
                Ctrl.value = Ctrl.value.replace(/ä/g, "");
                Ctrl.value = Ctrl.value.replace(/ü/g, "");
                return false;
            }
            else if (Value.toLowerCase().indexOf("ü") != -1) {
                alert("Bitte geben Sie keine Umlaute ein!");
                Ctrl.value = Ctrl.value.replace(/ö/g, "");
                Ctrl.value = Ctrl.value.replace(/ä/g, "");
                Ctrl.value = Ctrl.value.replace(/ü/g, "");
                return false;
            }

        }

        function openinfo(url) {
            fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=800,height=350");
            fenster.focus();
        }
        function hideInfo() {
            ctl00_ContentPlaceHolder1_trinfo.style.display = 'none';
        }
    </script>
</asp:Content>
