<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_3.aspx.vb" Inherits="CKG.Components.ComCommon.Logistik.Change01_3"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkSuche" runat="server" NavigateUrl="Change01.aspx">Suche</asp:HyperLink>
                    <asp:HyperLink ID="lnkFahrzeug" runat="server" NavigateUrl="Change01_2.aspx"> | Fahrzeugauswahl</asp:HyperLink>
                    <a class="active">| Adressauswahl</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <div id="pagination">
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="TableQuery">
                                    <div id="divControls" runat="server">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="4" class="firstLeft active">
                                                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    Abholanschrift:
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    Lieferanschrift:
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Kennzeichen" runat="server">Kennzeichen:</asp:Label>
                                                </td>
                                                <td class="active" nowrap="nowrap">
                                                    <asp:TextBox ID="txtKennz" runat="server" MaxLength="40" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblFahrgestellnr" runat="server">Fahrgestellnummer:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtFahrgestellnr" runat="server" MaxLength="40" CssClass="TextBoxNormal"
                                                        TabIndex="1" />
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblAuswahl" runat="server">Auswahl:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:DropDownList ID="ddlAuswahl" runat="server" AutoPostBack="True" TabIndex="11">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Name" runat="server">Name:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 30%">
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="TextBoxNormal" MaxLength="40"
                                                        Enabled="False" TabIndex="2"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_NameLief" runat="server">Name:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:TextBox ID="txtNamelief" Enabled="False" runat="server" CssClass="TextBoxNormal"
                                                        MaxLength="40" TabIndex="12"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Postleitzahl" runat="server">Postleitzahl:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtPostleitzahl" runat="server" MaxLength="5" CssClass="TextBoxNormal"
                                                        Enabled="False" TabIndex="3"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_PlzLief" runat="server">Postleitzahl:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtPostleitzahllief" runat="server" Enabled="False" CssClass="TextBoxNormal"
                                                        MaxLength="5" TabIndex="13"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Strasse" runat="server">Strasse:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 45%">
                                                    <asp:TextBox ID="txtStrasse" runat="server" Width="145px" MaxLength="60" CssClass="TextBoxNormal"
                                                        Enabled="False" TabIndex="4"></asp:TextBox>
                                                    <asp:TextBox ID="txtNummer" runat="server" CssClass="TextBoxNormal" Enabled="False"
                                                        MaxLength="5" Width="48px" TabIndex="5"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_StrasseLief" runat="server">Strasse:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtStrasseLief" runat="server" Width="145px" CssClass="TextBoxNormal"
                                                        MaxLength="60" Enabled="False" TabIndex="14"></asp:TextBox>
                                                    <asp:TextBox ID="txtLiefNummer" runat="server" CssClass="TextBoxNormal" Enabled="False"
                                                        MaxLength="5" Width="48px" TabIndex="15"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Ort" runat="server">Ort:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtOrt" runat="server" CssClass="TextBoxNormal" Enabled="False"
                                                        TabIndex="6"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblOrtLief" runat="server">Ort:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtOrtlief" Enabled="False" runat="server" CssClass="TextBoxNormal"
                                                        TabIndex="16"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Ansprechopartner" runat="server">Ansprechpartner:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtAnsprechpartner" runat="server" CssClass="TextBoxNormal" MaxLength="241"
                                                        Enabled="False" TabIndex="7"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Ansprechopartner0" runat="server">Ansprechpartner:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtLiefAnsprechpartner" Enabled="False" runat="server" CssClass="TextBoxNormal"
                                                        MaxLength="241" TabIndex="17"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Tel" runat="server">Telefon:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtTel" runat="server" CssClass="TextBoxNormal" Enabled="False"
                                                        TabIndex="8"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_TelLief" runat="server">Telefon:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtTelLief" Enabled="False" runat="server" CssClass="TextBoxNormal"
                                                        TabIndex="18"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_FixTermin" runat="server">Fixtermin:</asp:Label>
                                                </td>
                                                <td class="active" align="left" style="vertical-align: top;">
                                                    <asp:TextBox ID="txtFixTermin" runat="server" CssClass="TextBoxNormal" MaxLength="40"
                                                        TabIndex="9"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CEtxtFixTermin" runat="server" 
                                                        Enabled="True" TargetControlID="txtFixTermin">
                                                    </cc1:CalendarExtender>
                                                    <asp:CompareValidator ID="CVtxtFixTermin" runat="server" 
                                                        ControlToCompare="TextBox1" ControlToValidate="txtFixTermin" 
                                                        CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                        Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                        
                                                </td>
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trWEAnrede" class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Manuell" runat="server" Text="Manuelle Adresse:"></asp:Label>
                                                </td>
                                                <td>
                                                    <span>
                                                        <asp:CheckBox ID="chkManuell" runat="server" AutoPostBack="True" TabIndex="10" />
                                                    </span>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_ManuellLief" runat="server" Text="Manuelle Adresse:"></asp:Label>
                                                </td>
                                                <td class="active">
                                                    <span>
                                                        <asp:CheckBox ID="chkManuellLief" runat="server" AutoPostBack="True" TabIndex="19" />
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr id="trWETel" runat="server" class="formquery">
                                                <td colspan="4" class="rightPadding" align="right">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="4" class="rightPadding" style="width: 100%" align="right">
                                                    <div id="dataQueryFooter">
                                                        <asp:LinkButton ID="cmdContinue" runat="server" CssClass="Tablebutton" Height="16px"
                                                            Text="&amp;nbsp;&amp;#187; Weiter" Width="78px"></asp:LinkButton><asp:LinkButton
                                                                ID="cmdConfirm" runat="server" CssClass="Tablebutton" Height="16px" Text="&nbsp;&#187; Fortfahren"
                                                                Width="78px" Visible="False"></asp:LinkButton></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div>
                                    <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                                    <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                                    <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel"
                                        X="350" Y="185">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="mb" runat="server" Width="450px" Height="360px" BackColor="White" style="display:none">
                                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px;">
                                            <asp:Label ID="lblAdressMessage" runat="server" Text="Es wurden Adressalternativen für die Abholadresse gefunden."
                                                Font-Bold="True"></asp:Label></div>
                                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 5px; padding-bottom: 5px">
                                            <asp:DropDownList ID="ddlAlternativAdressen" runat="server" Width="420px">
                                            </asp:DropDownList>
                                        </div>
                                        <div style="padding-left: 10px; padding-top: 10px; margin-bottom: 10px; padding-bottom: 10px">
                                            <asp:CheckBox ID="chkAltenativ" Text="Übernehmen" runat="server" />
                                        </div>
                                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px;">
                                            <asp:Label ID="lblAdressMessage2" runat="server" Text="Es wurden Adressalternativen für die Lieferadresse gefunden."
                                                Font-Bold="True"></asp:Label></div>
                                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 5px; padding-bottom: 5px">
                                            <asp:DropDownList ID="ddlAlternativAdressen2" runat="server" Width="420px">
                                            </asp:DropDownList>
                                        </div>
                                        <div style="padding-left: 10px; padding-top: 10px; margin-bottom: 10px; padding-bottom: 10px">
                                            <asp:CheckBox ID="chkLiefAltenativ" Text="Übernehmen" runat="server" />
                                        </div>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnOK" runat="server" Text="Weiter" CssClass="TablebuttonLarge" Font-Bold="True"
                                                        Width="90px" />
                                                </td>
                                                <td align="left" style="width: 100px; padding-left: 5px; padding-right: 10px">

                                                    <asp:Button ID="btnCancel2" runat="server" Text="Abbrechen" CssClass="TablebuttonLarge"
                                                        Font-Bold="True" Width="90px" />

                                                </td>
                                            </tr>
                                        </table>
                                                <asp:Button ID="btnCancel" Style="visibility: hidden" runat="server" Text="Abbrechen"
                                                        CssClass="Tablebutton" Font-Bold="True" Width="90px" />
                                    </asp:Panel>
                                    <span>
                                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                           <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox> &nbsp;
                                        </div>
                                    </span>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            &nbsp;</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
