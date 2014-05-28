<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_3.aspx.vb" Inherits="AppF2.Change04_3"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx">Suche</asp:HyperLink>&nbsp;<asp:HyperLink
                        ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx">| Fahrzeugauswahl</asp:HyperLink>
                    <a class="active">| Adressauswahl</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <asp:UpdatePanel runat="server" ID="UPData">
                            <ContentTemplate>
                                <div id="data">
                                    <table cellpadding="0" cellspacing="0" style="border-right-width: 1px; border-left-width: 1px;
                                        border-left-style: solid; border-right-style: solid; border-right-color: #DFDFDF;
                                        border-left-color: #DFDFDF">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <table id="Tablex1" runat="server" cellspacing="0" cellpadding="5" width="100%" bgcolor="white"
                                                        border="0">
                                                        <tr>
                                                            <td nowrap="nowrap" width="100%" colspan="6">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="firstLeft active" nowrap="nowrap" width="100%" colspan="6">
                                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_AuswahlAkf" runat="server" >
                                                            <td align="left" class="firstLeft active" style="padding-top: 10px">
                                                                <asp:Label ID="Label3" Text="Adressauswahl" runat="server"></asp:Label>
                                                            </td>
                                                            <td nowrap="nowrap" width="100%" colspan="5" style="padding-top: 10px">
                                                                <asp:RadioButton Text="rb_Haendler" ID="rb_Haendler" runat="server" AutoPostBack="true"
                                                                    GroupName="AuswahlAKF" Checked="True" />                                                                
                                                                <asp:RadioButton Text="rb_Kundenadresse" ID="rb_Kundenadresse" runat="server" AutoPostBack="true"
                                                                    GroupName="AuswahlAKF" />
                                                                <asp:RadioButton Text="rb_freieAdresse" ID="rb_freieAdresse" runat="server" AutoPostBack="true"
                                                                    GroupName="AuswahlAKF" />
                                                                    
                                                            </td>
                                                        </tr>                                                        
                                                        <tr id="tr_Partneradresse" runat="server" >

                                                            <td align="left" width="100%" colspan="5" class="firstLeft active">
                                                                    <div style="border: 1px solid #C0C0C0; padding-top: 15px; padding-bottom: 15px">
                                                                    <table id="Table2" runat="server" cellspacing="0" cellpadding="3" width="100%" bgcolor="white">
                                                                        <tr>
                                                                            <td style="padding-left: 5px">
                                                                                <asp:DropDownList ID="ddlPartneradressen" runat="server" CssClass="DropDownXLarge">
                                                                                </asp:DropDownList>
                                                                                <asp:Label ID="lblSuche" runat="server" Visible="False"></asp:Label>
                                                                                <asp:LinkButton ID="lb_NeuSuche" runat="server" Visible="False">Neu Suche</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                    <table id="tblSuche" runat="server" cellspacing="0" cellpadding="3"
                                                                    width="100%" bgcolor="white" >
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lbl_NameSuche" runat="server" CssClass="TextLarge"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtNameSuche" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                                        </td>
                                                                        <td width="100%">
                                                                            <asp:Label ID="lbl_Info0" CssClass="TextLarge" Text="Alle Eingaben mit Platzhalter-Suche (*) möglich (z.B. Muster*')"
                                                                                runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lbl_HaenderNr" runat="server" CssClass="TextLarge"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtHaendlerNr" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                                            </td>
                                                                            <td width="100%">
                                                                                &nbsp;</td>
                                                                        </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lbl_OrtSuche" runat="server" CssClass="TextLarge"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOrtSuche" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                                        </td>
                                                                        <td width="100%">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lbl_PostleitzahlSuche" runat="server" CssClass="TextLarge"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPostleitzahlSuche" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                                        </td>
                                                                        <td width="100%">
                                                                            <asp:LinkButton ID="lb_Suche" CssClass="TableButton" runat="server" 
                                                                                Visible="False">Suchen</asp:LinkButton>
                                                                            <asp:LinkButton ID="lb_SucheAKF" runat="server" CssClass="TableButton">Suchen</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                    </div>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_Adresse" runat="server" visible="false">
                                                            <td align="left" width="100%" colspan="5" class="firstLeft active">
                                                            <div style="border: 1px solid #C0C0C0; padding-top: 15px; padding-bottom: 15px">
                                                                <table id="tblVersandanschrift" runat="server" cellspacing="0" cellpadding="3" width="100%"
                                                                    bgcolor="white" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_FirmaName" Text="lbl_FirmaName" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtName1" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:Label ID="lbl_AnsprechpartnerZusatz" Text="lbl_AnsprechpartnerZusatz" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td style="width:100%" >
                                                                                <asp:TextBox ID="txtName2" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Strasse" Text="lbl_Strasse" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtStrasse" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:Label ID="lbl_Hausnummer" Text="lbl_Hausnummer" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtHausnummer" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Postleitzahl" Text="lbl_Postleitzahl" runat="server" CssClass="TextLarge"></asp:Label>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtPostleitzahl" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:Label ID="lbl_Ort" Text="lbl_Ort" runat="server" CssClass="TextLarge"></asp:Label>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtOrt" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Land" Text="lbl_Land" runat="server" CssClass="TextLarge"></asp:Label>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:DropDownList ID="ddlLand" runat="server" CssClass="DropDownNormal">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td class="active">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td class="active">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="firstLeft active">
                                                                <u>Zustellungsart:</u>
                                                            </td>
                                                            <td>
                                                                <span>
                                                                    <asp:RadioButton ID="rb_VersandStandard" runat="server" GroupName="Versandart" Checked="True"
                                                                        Text="Standard"></asp:RadioButton></span><br />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="red">(siehe Hinweis)</font>
                                                            </td>
                                                            <td nowrap="nowrap">
                                                                <span>
                                                                    <asp:RadioButton ID="rb_0900" Text="rb_0900" runat="server" GroupName="Versandart"
                                                                        Width="100px"></asp:RadioButton></span><br />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="lbl_0900" runat="server"> lbl_0900</asp:Label>
                                                            </td>
                                                            <td nowrap="nowrap">
                                                                <span>
                                                                    <asp:RadioButton ID="rb_1000" runat="server" GroupName="Versandart" Width="100px"
                                                                        Text="rb_1000"></asp:RadioButton></span><br />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="lbl_1000" runat="server"> lbl_1000</asp:Label>
                                                            </td>
                                                            <td nowrap="nowrap">
                                                                <span>
                                                                    <asp:RadioButton ID="rb_1200" runat="server" GroupName="Versandart" Text="rb_1200"
                                                                        Width="100px"></asp:RadioButton></span><br />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="lbl_1200" runat="server"> lbl_1200</asp:Label>
                                                            </td>
                                                            <td nowrap="nowrap">
                                                                <span>
                                                                    <asp:RadioButton ID="rb_SendungsVerfolgt" runat="server" Text="rb_SendungsVerfolgt"
                                                                        GroupName="Versandart"></asp:RadioButton></span>
                                                                <br />
                                                                <asp:Label ID="lbl_SendungsVerfolgt" runat="server">lbl_SendungsVerfolgt</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="firstLeft active" colspan="6" style="font-weight: normal">
                                                                <strong><u>Hinweis:</u> </strong>
                                                                <br />
                                                                Die Deutsche Post AG garantiert für&nbsp;Standardsendungen keine Zustellzeiten<br />
                                                                und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;
                                                                <br />
                                                                <br />
                                                                &nbsp;&nbsp;&nbsp;-95% aller Sendungen werden dem Empfänger innerhalb von 24 Stunden
                                                                zugestellt,<br />
                                                                &nbsp;&nbsp;&nbsp;-5% aller Sendungen benötigen zwischen 24 und 48 Stunden bis zur Zustellung.<br />
                                                                <br />
                                                                Bitte beachten Sie hierzu auch die Beförderungsbedingungen der Deutschen Post AG.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                                <div id="dataFooter">
                                    <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px">» Weiter</asp:LinkButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
