<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01s_02.aspx.vb"
    Inherits="CKG.Components.ComCommon.Report01s_02" MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
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
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table cellpadding="0"  cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="5" style="height: 19px">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr class="formquery">
                                            <td colspan="5" class="firstLeft active">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Fahrgestellnummer:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtFahrgestellnummer" Enabled="false" runat="server" CssClass="InputTextbox" Width="150px"></asp:TextBox>
                                            </td>
                                            <td style="padding-left: 20px" class="firlstLeft active" nowrap="nowrap">
                                                Fahrername(Halter):
                                            </td>
                                            <td style="padding-left: 20px">
                                                <asp:TextBox ID="txtHalter" runat="server" Enabled="false" CssClass="InputTextbox"
                                                    Width="150px" MaxLength="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Leistungsart:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:DropDownList CssClass="DropDownNormal" ID="ddlLeistungsart" Width="250" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="padding-left: 20px" class="firlstLeft active" nowrap="nowrap">
                                                Postleitzahl:
                                            </td>
                                            <td style="padding-left: 20px">
                                                <asp:TextBox ID="txtPostleitzahl" runat="server" Enabled="false" CssClass="InputTextbox"
                                                    Width="150px" MaxLength="10"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px">
                                                Arbeitsende:
                                            </td>
                                            <td class="firstLeft active" style="height: 22px">
                                                <asp:TextBox  ID="txtArbeitsende" runat="server"  Enabled="false" CssClass="InputTextbox" Width="100px"></asp:TextBox>
                                            </td>
                                            <td style="padding-left: 20px; height: 22px;" class="firlstLeft active" 
                                                nowrap="nowrap">
                                                Strasse:
                                            </td>
                                            <td style="padding-left: 20px; height: 22px;">
                                                <asp:TextBox ID="txtStrasse" runat="server" Enabled="false" CssClass="InputTextbox"
                                                    Width="150px" MaxLength="10"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Prognose Ende:</td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtProgEnd" runat="server" CssClass="InputTextbox"
                                                  Width="100px" ></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CE_txtProgEnd" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtProgEnd">
                                                </ajaxToolkit:CalendarExtender> 
                                                <ajaxToolkit:MaskedEditExtender ID="MEE_txtProgEnd" runat="server" TargetControlID="txtProgEnd"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </ajaxToolkit:MaskedEditExtender> 
                                                  </td>
                                            <td class="firlstLeft active" nowrap="nowrap" style="padding-left: 20px">
                                                 Ort:</td>
                                            <td style="padding-left: 20px">
                                                                                                <asp:TextBox ID="txtOrt" runat="server" Enabled="false" CssClass="InputTextbox" Width="150px"
                                                    MaxLength="10"></asp:TextBox></td>

                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Endrückmeldung:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:CheckBox ID="chkEndrückmeldung" BorderStyle="None" runat="server" AutoPostBack="True" 
                                                 />
                                            </td>
                                            <td style="padding-left: 20px" class="firlstLeft active" nowrap="nowrap">
                                               
                                                Telefon:
                                               
                                            </td>
                                            <td style="padding-left: 20px">

                                                <asp:TextBox ID="txtTelefon" runat="server" CssClass="InputTextbox" 
                                                    Enabled="false" MaxLength="10" Width="150px"></asp:TextBox>

                                            </td>

                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Rückmeldetext:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtRueckmeldetext" CssClass="TextBoxLarge" runat="server" MaxLength="40"></asp:TextBox>
                                            </td>
                                            <td style="padding-left: 20px" class="firlstLeft active" nowrap="nowrap">
                                                E-Mail:
                                            </td>
                                            <td style="padding-left: 20px">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxLarge" 
                                                    Enabled="false" MaxLength="10" Width="250px"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="2">
                                                <asp:HiddenField ID="hiddenTeilRück" runat="server" /> 
                                            </td>
                                            <td style="padding-left: 20px" class="firlstLeft active" nowrap="nowrap">
                                                &nbsp;</td>
                                            <td style="padding-left: 20px">
                                                &nbsp;</td>

                                        </tr>
                                    </tbody>
                                </table>
 
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbInsert" runat="server" CssClass="Tablebutton" Width="78px">» Eintragen </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
