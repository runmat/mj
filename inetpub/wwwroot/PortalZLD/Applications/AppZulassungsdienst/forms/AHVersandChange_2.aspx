<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AHVersandChange_2.aspx.cs" Inherits="AppZulassungsdienst.forms.AHVersandChange_2"MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <script src="/PortalZLD/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery.blockUI.js" type="text/javascript"></script>
        <script src="/PortalZLD/JScript/jquery.ui.datepicker-de.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery.scrollTo-1.4.2.js" type="text/javascript"></script>
    <div id="site">
        <div id="content">
            <div class="divPopupBack" runat="server" visible="false" id="divBackDisabled">
            </div>
            <div class="divPopupDetail" runat="server" visible="false" id="divOptions">
                <table class="PopupDetailTable">
                    <tr>
                        <td align="left">
                            <h5>
                                <asp:Label ID="Label15" runat="server" Text="Stammdaten Lieferant/Zulassungsdienst"
                                    Font-Bold="True"></asp:Label></h5>
                        </td>
                        <td align="right">
                            <h5>
                                <asp:LinkButton ID="lbtnCloseOption" runat="server" OnClick="lbtnCloseOption_Click">X</asp:LinkButton></h5>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblName" Font-Size="12px" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblStreet" Font-Size="12px" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblPLZOrt" Font-Size="12px" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblTel" Font-Size="12px" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <div id="navigationSubmenuInput">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" OnClick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="cmdCreate" />
                            <asp:PostBackTrigger ControlID="lbtnStamm" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                    <table cellpadding="0" runat="server" id="TableBank" cellspacing="0">
                                        <tr>
                                            <td colspan="4" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trError" runat="server">
                                            <td colspan="4" class="firstLeft active">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblZulDateBank" runat="server">Lieferant/ausführ. ZLD:</asp:Label>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtZLDLief" onfocus="javascript:this.select();" Enabled="false"
                                                    runat="server" CssClass="TextBoxNormal" MaxLength="8" Width="70px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:DropDownList ID="ddlLief" runat="server" Style="width: 375px">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="firstLeft active" >
                                                <asp:LinkButton ID="lbtnStamm" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                                    Height="22px" OnClick="lbtnStamm_Click"> Stammdaten Lief. </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="Label1" runat="server">Kunde:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:TextBox ID="txtKunde" onfocus="javascript:this.select();" Enabled="false" runat="server"
                                                    CssClass="TextBoxNormal" MaxLength="8" Width="70px"></asp:TextBox>
                                                <asp:TextBox ID="txtKundeName" Style="margin-left: 15px" Enabled="false" runat="server"
                                                    CssClass="TextBoxNormal" Width="400px"></asp:TextBox>
                                                &nbsp;
                                                    <asp:LinkButton ID="lbtnAdresseHin" runat="server" 
                                                    CssClass="TablebuttonXLarge" Width="155px"
                                                    Height="22px" 
                                                    onclick="lbtnAdresseHin_Click"> abw. Adresse Hinsendung </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblFrachtHin" runat="server">Frachtbrief.Nr. hin:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:TextBox ID="txtFrachtHin" runat="server" CssClass="TextBoxNormal" MaxLength="20"
                                                    Width="170px"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="fteFrachtHin"
                                                    TargetControlID="txtFrachtHin" InvalidChars="|}" FilterMode="InvalidChars" />
                                                <asp:Label ID="Label2" Style="margin-left: 15px" runat="server" Height="16px">Frachtbrief.Nr. zurück:  </asp:Label>
                                                <asp:TextBox ID="txtFrachtBack" Style="margin-left: 36px" runat="server" Width="170px"
                                                    CssClass="TextBoxNormal" MaxLength="20" />
                                                <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="fteFrachtBack"
                                                    TargetControlID="txtFrachtBack" InvalidChars="|}" FilterMode="InvalidChars" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBox ID="chkKennzWunsch" runat="server" Text="Wunschkennzeichen:" Enabled="False" />
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:TextBox ID="txtKennzWunsch" runat="server" CssClass="TextBoxNormal" MaxLength="16"
                                                    Width="170px" Enabled="False"></asp:TextBox>
                                                <asp:CheckBox ID="chkReserviert" Style="margin-left: 15px" runat="server" Text="Kennz.-Reserviert, Nr.:"
                                                    Enabled="False" Height="20px" />
                                                <asp:TextBox ID="txtReserviertNr" Style="margin-left: 15px" Enabled="False" Width="170px"
                                                    runat="server" CssClass="TextBoxNormal" MaxLength="16" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="firstLeft active" style="background-color: #dfdfdf; height: 22px;
                                                padding-left: 15px">
                                                Beiliegende Unterlagen
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblBez" runat="server">Orginal&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Kopie</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkHandRegist" runat="server" RepeatDirection="Horizontal"
                                                    Height="20px" AutoPostBack="True" 
                                                    onselectedindexchanged="chkHandRegist_SelectedIndexChanged" >
                                                    <asp:ListItem Value="O" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label3" runat="server" Height="16px">Handelsregisterauszug der Fa.</asp:Label>
                                                <asp:TextBox ID="txtHandelsregFa" runat="server" CssClass="TextBoxNormal" Style="margin-left: 15px"
                                                    Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="Gewerbe" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="Gewerbe_SelectedIndexChanged">
                                                    <asp:ListItem Value="O" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label4" runat="server" Height="16px">Gewerbeanmeldung der Fa.</asp:Label>
                                                <asp:TextBox ID="txtGewerb" runat="server" CssClass="TextBoxNormal" Style="margin-left: 15px"
                                                    Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkPerso" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="chkPerso_SelectedIndexChanged">
                                                    <asp:ListItem Value="O" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label14" runat="server" Height="16px">Personalausweis von Frau/Herrn</asp:Label>
                                                <asp:TextBox ID="txtPersoName" runat="server" CssClass="TextBoxNormal" Style="margin-left: 15px"
                                                    Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkReisepass" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="chkReisepass_SelectedIndexChanged">
                                                    <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                    <asp:ListItem Text="" Value="K"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:Label ID="Label5" runat="server" Height="16px">Reisepass + Meldebescheinigung + Aufenthaltsgenehmigung von Frau/Herrn</asp:Label><br />
                                                <asp:TextBox ID="txtReisepass" runat="server" CssClass="TextBoxNormal" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkZulVoll" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="chkZulVoll_SelectedIndexChanged">
                                                    <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                    <asp:ListItem Text="" Value="K"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label6" runat="server" Height="16px">Zulassungsvollmacht des neuen Halters</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkEinzug" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="chkEinzug_SelectedIndexChanged">
                                                    <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                    <asp:ListItem Text="" Value="K"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label7" runat="server" Height="16px">Einzugsermächtigung Kfz-Steuer</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkevB" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="chkevB_SelectedIndexChanged">
                                                    <asp:ListItem Value="O" Text="" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label8" runat="server" Height="16px">evB-Nummer</asp:Label>
                                                <asp:TextBox ID="txtEVB" runat="server" CssClass="TextBoxNormal" Style="margin-left: 15px"
                                                    Width="150px" MaxLength="7"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkZulBeschein1" runat="server" 
                                                    RepeatDirection="Horizontal" AutoPostBack="True" 
                                                    onselectedindexchanged="chkZulBeschein1_SelectedIndexChanged">
                                                    <asp:ListItem Value="O" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label9" runat="server" Height="16px">Zulassungsbescheinigung Teil I</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkZulBeschein2" runat="server" 
                                                    RepeatDirection="Horizontal" AutoPostBack="True" 
                                                    onselectedindexchanged="chkZulBeschein2_SelectedIndexChanged">
                                                    <asp:ListItem Value="O" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label10" runat="server" Height="16px">Zulassungsbescheinigung Teil II</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkCoC" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="chkCoC_SelectedIndexChanged">
                                                    <asp:ListItem Value="O" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label11" runat="server" Height="16px">CoC-Bescheinigung</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkHU" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="chkHU_SelectedIndexChanged">
                                                    <asp:ListItem Value="O" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label12" runat="server" Height="16px">HU-Nachweis</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkAU" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="chkAU_SelectedIndexChanged">
                                                    <asp:ListItem Value="O" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:Label ID="Label13" runat="server" Height="16px">AU-Bescheinigung</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkFrei1" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="chkFrei1_SelectedIndexChanged">
                                                    <asp:ListItem Value="O" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:TextBox ID="txtFrei1" runat="server" CssClass="TextBoxNormal" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:CheckBoxList ID="chkFrei2" runat="server" RepeatDirection="Horizontal" 
                                                    AutoPostBack="True" onselectedindexchanged="chkFrei2_SelectedIndexChanged">
                                                    <asp:ListItem Value="O" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="K" Text=""></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:TextBox ID="txtFrei2" runat="server" CssClass="TextBoxNormal" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="firstLeft active" style="background-color: #dfdfdf; height: 22px;
                                                padding-left: 15px">
                                                Rücksendung
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblink" runat="server">inklusive</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3">
                                                <asp:CheckBox ID="chkKennzeichen" runat="server" Text="Kennzeichen" Height="20px" />
                                                <asp:CheckBox ID="chkkopie" Style="margin-left: 15px" runat="server" Text="Kopie des Gebührenbeleges"
                                                    Height="20px" />
                                                <asp:CheckBox ID="chkFrei3" Style="margin-left: 15px" runat="server" Height="20px" />
                                                <asp:TextBox ID="txtFrei3" Style="margin-left: 5px" Width="225px" runat="server"
                                                    CssClass="TextBoxNormal" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lblper" runat="server">per</asp:Label>
                                            </td>
                                            <td class="firstLeft active" colspan="3" style="width: 100%;">
                                                <asp:RadioButton ID="rbPostexpress" Style="margin-left: 0px" runat="server" Text="Postexpress Frachtbrief anbei"
                                                    GroupName="GrpVersandart" Checked="true" />
                                                <asp:RadioButton ID="rbNormPost" Style="margin-left: 15px" Text="Normaler Postweg"
                                                    runat="server" GroupName="GrpVersandart" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" align="right" colspan="4">
                                               <asp:LinkButton ID="lbtnAdresseRueck" runat="server" 
                                                    CssClass="TablebuttonXLarge" Width="155px"
                                                    Height="22px" onclick="lbtnAdresseRueck_Click"> Versandadr. Rücksendung </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>

                            </div>
                            <div id="ButtonFooter" runat="server" class="dataQueryFooter">
                                <asp:LinkButton ID="lbtnErfassen" runat="server" Visible="false" CssClass="TablebuttonXLarge"
                                    Width="155px" Height="22px" Style="padding-right: 5px" OnClick="lbtnErfassen_Click"> zurück zur Liste </asp:LinkButton>
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" Style="padding-right: 5px" OnClick="cmdCreate_Click"> Speichern/Zul. Drucken </asp:LinkButton>
                                     <asp:LinkButton ID="btnRefreshAdressHin" CausesValidation="false"    
                                    style="display:none" runat="server" onclick="btnRefreshAdressHin_Click"></asp:LinkButton>   
                                     <asp:LinkButton ID="btnRefreshAdresseRueck" CausesValidation="false"  
                                    style="display:none" runat="server" 
                                    onclick="btnRefreshAdresseRueck_Click" ></asp:LinkButton>   
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    <asp:Button ID="MPEDummyAdrHin" Width="0" Height="0" runat="server" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender runat="server" ID="MPEAdrHin" BackgroundCssClass="divProgress"
                        Enabled="true" PopupControlID="PanelAdrHin" TargetControlID="MPEDummyAdrHin">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="PanelAdrHin" HorizontalAlign="Center" runat="server" Style="display: none">
                        <div id="divEditAdrHin" title="Abweichende Adresse Hinsendung" style="display: block;">
                            <table cellpadding="0" cellspacing="0" style="color:#595959;width:480px; border: solid 1px #646464" bgcolor="white">
                                <tr>
                                    <td style="padding-top:10px;padding-right:10px;font-weight:bold">
                                        <asp:Label ID="lblNameHin" runat="server" Text="Name"></asp:Label>
                                    </td>
                                    <td  style="padding-top:10px;padding-right:10px;" colspan="2">
                                        <asp:TextBox ID="txtNameHin1" Width="400px" CssClass="TextBoxNormal" runat="server"
                                            MaxLength="40"></asp:TextBox> 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top:10px;padding-right:10px;" nowrap="nowrap">
                                    </td>
                                    <td class="firstLeft active" style="padding-top:10px;padding-right:10px;" colspan="2">
                                        <asp:TextBox ID="txtNameHin2" Width="400px" CssClass="TextBoxNormal" runat="server"
                                            MaxLength="40"></asp:TextBox>  
                                    </td>
                                               
                                </tr>
                                <tr>
                                    <td style="padding-top:10px;padding-right:10px;font-weight:bold">
                                        <asp:Label ID="lblStrasseHin" runat="server">Straße:</asp:Label>
                                    </td>
                                    <td style="padding-top:10px;padding-right:10px;" colspan="2">
                                        <asp:TextBox ID="txtStrasseHin" Width="400px" CssClass="TextBoxNormal" runat="server"
                                            MaxLength="60"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top:10px;padding-right:10px;font-weight:bold">
                                        <asp:Label ID="lblPLZBank" runat="server">PLZ/Ort*:</asp:Label>
                                    </td>
                                    <td style="padding-top:10px;padding-right:10px;">
                                        <asp:TextBox ID="txtPlz" runat="server" CssClass="TextBoxNormal" MaxLength="5" Width="65px"  onKeyPress="return numbersonly(event, false)"></asp:TextBox>

                                    </td>
                                    <td style="padding-top:10px;padding-right:10px;" class="ui-accordion">
                                        <asp:TextBox ID="txtOrt" Width="320px" CssClass="TextBoxNormal" runat="server" 
                                            MaxLength="40"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top:10px;padding-right:10px;font-weight:bold" colspan="3">
                                        <asp:Label ID="lblAdrHinError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding-top:10px;padding-right:10px;" colspan="3">
                                <asp:LinkButton ID="cmdSetBackHin" runat="server" Text="Zurücksetzen" CssClass="Tablebutton"
                                    Width="78px" onclick="cmdSetBackHin_Click" />
                                <asp:LinkButton ID="cmdCloseDialogHin" runat="server" Text="Schließen" CssClass="Tablebutton"
                                    Width="78px" onclick="cmdCloseDialogHin_Click"  />
                                <asp:LinkButton ID="cmdSaveAdrHin" runat="server" Text="Übernehmen" CssClass="Tablebutton"
                                    Width="78px" onclick="cmdSaveAdrHin_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    
                    <asp:Button ID="MPEDummyAdrRueck" Width="0" Height="0" runat="server" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender runat="server" ID="MPEAdrRueck" BackgroundCssClass="divProgress"
                        Enabled="true" PopupControlID="PanelAdrRueck" TargetControlID="MPEDummyAdrRueck">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="PanelAdrRueck" HorizontalAlign="Center" runat="server" Style="display: none">
                        <div id="divEditAdrRueck" title="Vesandadressen Rücksendung" style="display: block; width: 550px;">
                            <table cellpadding="0" cellspacing="0" style="color:#595959; width: 507px;border: solid 1px #646464" bgcolor="white">
                                <tr>  
                                    <td style="padding-top: 10px; padding-right: 10px; font-weight: bold; width: 120px;">
                                    <asp:Label ID="lblDocRueck" runat="server" Text="welche Unterlagen:"></asp:Label>
                                </td>
                                <td colspan="2" style="padding-top: 10px; padding-right: 10px;">
                                    <asp:TextBox ID="txtDocRueck1" runat="server" CssClass="TextBoxNormal" MaxLength="100"
                                        Width="400px"></asp:TextBox>
                                </td>  </tr>                                   
                            <tr>
                                <td style="padding-top:10px;padding-right:10px;font-weight:bold; width: 120px;">
                                    <asp:Label ID="lblNameRueck" runat="server" Text="Name"></asp:Label>
                                </td>
                                <td  style="padding-top:10px;padding-right:10px;" colspan="2">
                                    <asp:TextBox ID="txtNameRueck1" Width="400px" CssClass="TextBoxNormal" runat="server"
                                        MaxLength="40"></asp:TextBox> 
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top:10px;padding-right:10px; width: 120px;" nowrap="nowrap">
                                </td>
                                <td class="firstLeft active" style="padding-top:10px;padding-right:10px;" colspan="2">
                                    <asp:TextBox ID="txtNameRueck2" Width="400px" CssClass="TextBoxNormal" runat="server"
                                        MaxLength="40"></asp:TextBox>  
                                </td>
                                               
                            </tr>
                            <tr>
                                <td style="padding-top:10px;padding-right:10px;font-weight:bold; width: 120px;">
                                    <asp:Label ID="Label17" runat="server">Straße:</asp:Label>
                                </td>
                                <td style="padding-top:10px;padding-right:10px;" colspan="2">
                                    <asp:TextBox ID="txtStrasseRueck" Width="400px" CssClass="TextBoxNormal" runat="server"
                                        MaxLength="60"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top:10px;padding-right:10px;font-weight:bold; width: 120px;">
                                    <asp:Label ID="Label18" runat="server">PLZ/Ort*:</asp:Label>
                                </td>
                                <td style="padding-top:10px;padding-right:10px;">
                                    <asp:TextBox ID="txtPLZRueck" runat="server" CssClass="TextBoxNormal" 
                                        MaxLength="5" Width="65px" onKeyPress="return numbersonly(event, false)"></asp:TextBox>

                                </td>
                                <td style="padding-top:10px;padding-right:10px;width: 100%;">
                                    <asp:TextBox ID="txtOrtRueck" Width="320px" CssClass="TextBoxNormal" runat="server" 
                                        MaxLength="40"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="height: 10px;" >
                                               
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="margin-top: 10px;" class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix" >
                                               
                                </td>
                            </tr>
                                <tr>  
                                    <td style="padding-top: 10px; padding-right: 10px; font-weight: bold; width: 120px;">
                                    <asp:Label ID="Label16" runat="server" Text="welche Unterlagen:"></asp:Label>
                                </td>
                                <td colspan="2" style="padding-top: 10px; padding-right: 10px;">
                                    <asp:TextBox ID="txtDoc2Rueck" runat="server" CssClass="TextBoxNormal" MaxLength="100"
                                        Width="400px"></asp:TextBox>
                                </td>  </tr>                                   
                            <tr>
                                <td style="padding-top:10px;padding-right:10px;font-weight:bold; width: 120px;">
                                    <asp:Label ID="Label19" runat="server" Text="Name"></asp:Label>
                                </td>
                                <td  style="padding-top:10px;padding-right:10px;" colspan="2">
                                    <asp:TextBox ID="txtName1Rueck2" Width="400px" CssClass="TextBoxNormal" runat="server"
                                        MaxLength="40"></asp:TextBox> 
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top:10px;padding-right:10px; width: 120px;" nowrap="nowrap">
                                </td>
                                <td class="firstLeft active" style="padding-top:10px;padding-right:10px;" colspan="2">
                                    <asp:TextBox ID="txtName2Rueck2" Width="400px" CssClass="TextBoxNormal" runat="server"
                                        MaxLength="40"></asp:TextBox>  
                                </td>
                                               
                            </tr>
                            <tr>
                                <td style="padding-top:10px;padding-right:10px;font-weight:bold; width: 120px;">
                                    <asp:Label ID="Label20" runat="server">Straße:</asp:Label>
                                </td>
                                <td style="padding-top:10px;padding-right:10px;" colspan="2">
                                    <asp:TextBox ID="txtStrasse2Rueck" Width="400px" CssClass="TextBoxNormal" runat="server"
                                        MaxLength="60"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top:10px;padding-right:10px;font-weight:bold; width: 120px;">
                                    <asp:Label ID="Label21" runat="server">PLZ/Ort*:</asp:Label>
                                </td>
                                <td style="padding-top:10px;padding-right:10px;">
                                    <asp:TextBox ID="txtPLZ2Rueck" runat="server" CssClass="TextBoxNormal" 
                                        MaxLength="5" Width="65px" onKeyPress="return numbersonly(event, false)"></asp:TextBox>

                                </td>
                                <td style="padding-top:10px;padding-right:10px;width: 100%;">
                                    <asp:TextBox ID="txtOrt2Rueck" Width="320px" CssClass="TextBoxNormal" runat="server" 
                                        MaxLength="40"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top:10px;padding-right:10px;font-weight:bold" colspan="3">
                                    <asp:Label ID="lblAdrRueckError" runat="server" CssClass="TextError"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-top:10px;padding-right:50px;" colspan="3">
                                <asp:LinkButton ID="cmdSetBackRueck" runat="server" Text="Zurücksetzen" CssClass="Tablebutton"
                                Width="78px" onclick="cmdSetBackRueck_Click" />
                            <asp:LinkButton ID="cmdCloseDialogRueck" runat="server" Text="Schließen" CssClass="Tablebutton"
                                Width="78px" onclick="cmdCloseDialogRueck_Click"  />
                            <asp:LinkButton ID="cmdSaveAdrRueck" runat="server" Text="Übernehmen" CssClass="Tablebutton"
                                Width="78px" onclick="cmdSaveAdrRueck_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        </div>
                    </asp:Panel>

                    <asp:Button ID="MPEDummy" Width="0" Height="0" runat="server" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender runat="server" ID="MPE48h" BackgroundCssClass="divProgress"
                        Enabled="true" PopupControlID="Panel48h" TargetControlID="MPEDummy">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="Panel48h" HorizontalAlign="Center" runat="server" Style="display: none">
                        <table cellspacing="0" id="Table1" runat="server" width="55%" bgcolor="white" cellpadding="0"
                            style="width: 55%; border: solid 1px #646464">
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active" colspan="2">
                                    <asp:Label runat="server" ID="lblPanel48hTitle" Text="48h-Versandzulassung" Font-Bold="True" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active" colspan="2" style="color: red; padding-left: 3px; padding-right: 3px">
                                    <asp:Label runat="server" ID="lblPanel48hHint" Text="Achtung! Aufgrund des gewählten Zulassungsdatums, Amtes und Lieferanten wird dieser Vorgang als 48h-Expresszulassung behandelt." Font-Bold="True" />
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active" colspan="2" style="color: red; padding-left: 3px; padding-right: 3px">
                                    <b>Bitte beachten Sie die folgenden Zusatzinformationen und passen Sie ggf. den Versand vor dem Absenden noch einmal an.</b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active">
                                    <b>Lieferuhrzeit:</b>
                                </td>
                                <td class="firstLeft active">
                                    <asp:Label ID="lblLieferuhrzeit" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="firstLeft active">
                                    <b>Abw. Lieferadresse:</b>
                                </td>
                                <td class="firstLeft active">
                                    <asp:Label ID="lblAbwName" runat="server"/>
                                    <br/>
                                    <asp:Label ID="lblAbwStrasse" runat="server"/>
                                    <br/>
                                    <asp:Label ID="lblAbwOrt" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:LinkButton ID="lb48hCancel" Text="Versand anpassen" Height="16px" Width="128px" runat="server"
                                                    CssClass="TablebuttonLarge"/>
                                    <asp:LinkButton ID="lb48hContinue" Text="Weiter" Height="16px" Width="78px" runat="server"
                                                    CssClass="Tablebutton" OnClick="lb48hContinue_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
