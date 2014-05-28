<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03_3.aspx.vb" Inherits="AppServicesCarRent.Change03_3"
    MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change01.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                        ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change01_2.aspx">| Fahrzeugauswahl</asp:HyperLink>
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
                                                            <td nowrap="nowrap" width="100%">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="firstLeft active" nowrap="nowrap" width="100%">
                                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_ZulstSichtbarkeit" runat="server">
                                                            <td align="left" width="100%" class="firstLeft active">
                                                                <div style="border: 1px solid #C0C0C0; padding-top: 15px; padding-bottom: 15px">
                                                                    <table id="Dummy01" runat="server" cellspacing="0" cellpadding="3" width="100%" bgcolor="white">
                                                                        <tr>
                                                                            <td align="left" style="width: 20%" class="firstLeft active">
                                                                                <asp:RadioButton ID="rb_Zulst" runat="server" AutoPostBack="True" Text="rb_Zulst"
                                                                                    GroupName="grpAdresse"></asp:RadioButton>
                                                                            </td>
                                                                            <td align="left" style="width: 80%" class="active">
                                                                                <asp:DropDownList ID="ddlZulDienst" Width="75%" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr id="trSAPAdresse" runat="server">
                                                            <td align="left" width="100%" class="firstLeft active">
                                                                <div style="border: 1px solid #C0C0C0; padding-top: 15px; padding-bottom: 15px">
                                                                    <table id="Dummy" runat="server" cellspacing="0" cellpadding="3" width="100%" bgcolor="white"
                                                                        border="0">
                                                                        <tr>
                                                                            <td valign="top" style="width: 20%" class="firstLeft active">
                                                                                <asp:RadioButton ID="rb_SAPAdresse" runat="server" AutoPostBack="True" Text="rb_SAPAdresse"
                                                                                    GroupName="grpAdresse"></asp:RadioButton>
                                                                            </td>
                                                                            <td style="width: 80%" class="active">
                                                                                <asp:DropDownList ID="ddlSAPAdresse" runat="server" Width="75%" Visible="False">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_Adresse" runat="server">
                                                            <td align="left" width="100%" class="firstLeft active">
                                                                <div style="border: 1px solid #C0C0C0; padding-top: 15px; padding-bottom: 15px">
                                                                    <table id="tblVersandanschrift" runat="server" cellspacing="0" cellpadding="3" width="100%"
                                                                        bgcolor="white" border="0">
                                                                        <tbody>
                                                                            <tr id="trVersandAdrEnd0" runat="server">
                                                                                <td colspan="4" class="firstLeft active">
                                                                                    <asp:RadioButton ID="rb_VersandAdresse" runat="server" AutoPostBack="True" Text="rb_VersandAdresse"
                                                                                        GroupName="grpAdresse"></asp:RadioButton>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trVersandAdrEnd1" runat="server">
                                                                                <td class="firstLeft active">
                                                                                    <asp:Label ID="lbl_Firma" runat="server" Text="lbl_Firma"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtName1" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                                                </td>
                                                                                <td class="active">
                                                                                    <asp:Label ID="lbl_AnsprechpartnerZusatz" runat="server" Text="lbl_AnsprechpartnerZusatz"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 100%">
                                                                                    <asp:TextBox ID="txtName2" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trVersandAdrEnd2" runat="server">
                                                                                <td class="firstLeft active">
                                                                                    <asp:Label ID="lbl_Strasse" Text="lbl_Strasse" runat="server"></asp:Label>
                                                                                </td>
                                                                                <td class="active">
                                                                                    <asp:TextBox ID="txtStr" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                                                </td>
                                                                                <td class="active">
                                                                                    <asp:Label ID="lbl_Hausnummer" Text="lbl_Hausnummer" runat="server"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtNr" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trVersandAdrEnd3" runat="server">
                                                                                <td class="firstLeft active">
                                                                                    <asp:Label ID="lbl_Postleitzahl" Text="lbl_Postleitzahl" runat="server" CssClass="TextLarge"></asp:Label>
                                                                                </td>
                                                                                <td class="active">
                                                                                    <asp:TextBox ID="txtPlz" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                                                </td>
                                                                                <td class="active">
                                                                                    <asp:Label ID="lbl_Ort" Text="lbl_Ort" runat="server" CssClass="TextLarge"></asp:Label>
                                                                                </td>
                                                                                <td class="active">
                                                                                    <asp:TextBox ID="txtOrt" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="tr_Land" runat="server">
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
                                                        <tr id="tr_Grund" runat="server">
                                                            <td class="firstLeft active" valign="top">
                                                                <div style="border: 1px solid #C0C0C0; padding-top: 15px; padding-bottom: 15px">
                                                                    <table id="Table1" runat="server" cellspacing="0" cellpadding="3" width="100%" bgcolor="white"
                                                                        border="0">
                                                                        <tr id="trVersandAdrEnd4" runat="server">
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Grund" runat="server">lbl_Grund</asp:Label>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:DropDownList ID="ddlGrund" TabIndex="10" runat="server" AutoPostBack="True"
                                                                                    Width="300px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Auf" runat="server">lbl_Auf</asp:Label>
                                                                            </td>
                                                                            <td class="active">
                                                                                <asp:TextBox ID="txtAuf" TabIndex="11" runat="server" Width="200px" MaxLength="40"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trVersandAdrEnd9" runat="server" >
                                                                            <td class="firstLeft active">
                                                                                <asp:Label ID="lbl_Bemerkung" runat="server">lbl_Bemerkung</asp:Label>
                                                                            </td>
                                                                            <td colspan="3" style="width:100%" class="active">
                                                                                <asp:TextBox ID="txtBemerkung" TabIndex="12" runat="server" Width="300px" MaxLength="60"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_Versandart1" runat="server">
                                                            <td class="firstLeft active" valign="top">
                                                                <div style="border: 1px solid #C0C0C0; padding-top: 15px; padding-bottom: 15px">
                                                                    <table id="Table2" runat="server" cellspacing="0" cellpadding="3" width="100%" bgcolor="white"
                                                                        border="0">
                                                                        <tr id="tr1" runat="server">
                                                                            <td class="firstLeft active" valign="top">
                                                                                <asp:Label ID="lbl_Versandart" runat="server">lbl_Versandart</asp:Label>
                                                                            </td>
                                                                            <td class="firstLeft active" valign="top">
                                                                                <asp:Label ID="lbl_Express" runat="server">lbl_Express</asp:Label>
                                                                            </td>
                                                                            <td style="width: 100%">
                                                                                <span>
                                                                                    <asp:RadioButton ID="rb_0900" runat="server" GroupName="Versandart" Text="rb_0900">
                                                                                    </asp:RadioButton>
                                                                                </span>
                                                                                <br />
                                                                                <span>
                                                                                    <asp:RadioButton ID="rb_1000" runat="server" GroupName="Versandart" Text="rb_1000">
                                                                                    </asp:RadioButton>
                                                                                </span>
                                                                                <br />
                                                                                <span>
                                                                                    <asp:RadioButton ID="rb_1200" runat="server" GroupName="Versandart" Text="rb_1200">
                                                                                    </asp:RadioButton>
                                                                                </span>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
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
