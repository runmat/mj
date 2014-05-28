<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report01.aspx.cs" Inherits="AppRemarketing.forms.Report01"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red"></asp:Label>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    Selektionsauswahl:
                                </td>
                                <td class="active" style="width: 88%">
                                    <span>
                                        <asp:RadioButton ID="rb_Vermieter" Text="Selektion Autovermieter" GroupName="Auswahl"
                                            runat="server" Checked="True" AutoPostBack="True" OnCheckedChanged="rb_Vermieter_CheckedChanged" />
                                        &nbsp;
                                        <asp:RadioButton ID="rbFin" Text="Upload Fahrgestellnummern" GroupName="Auswahl"
                                            runat="server" AutoPostBack="True" OnCheckedChanged="rbFin_CheckedChanged" />
                                        &nbsp;
                                        <asp:RadioButton ID="rbKennzeichen" Text="Upload Kennzeichen" GroupName="Auswahl"
                                            runat="server" AutoPostBack="True" OnCheckedChanged="rbKennzeichen_CheckedChanged" /></span>
                                </td>
                            </tr>
                            <tr id="trSelVermieter" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Vermieter" runat="server">lbl_Vermieter</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:DropDownList ID="ddlVermieter" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trDatumVon" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_DatumVon" runat="server">lbl_DatumVon</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtDatumvon" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_Datumvon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtDatumvon">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr id="trDatumBis" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_DatumBis" runat="server">lbl_DatumBis</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtDatumBis" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Wählen Sie 'Datum bis' größer als 'Datum von'!"
                                        ControlToCompare="txtDatumvon" ControlToValidate="txtDatumBis" Operator="GreaterThan"
                                        Type="Date"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr id="trSearchFin" runat="server" visible="false" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label></td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtFahrgestellnummer" CssClass="TextBoxNormal" runat="server" 
                                        MaxLength="17"></asp:TextBox></td>
                            </tr>
                            <tr id="trUploadFin" runat="server" visible="false" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_UploadFin" runat="server">lbl_UploadFin</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <input id="upFileFin" type="file" size="49" name="upFileFin" runat="server" />&nbsp;
                                    <a href="javascript:openinfo('InfoFin.htm');"><img src="/Services/Images/info.gif" border="0" height="16px" width="16px" 
                                     alt="Struktur Uploaddatei" title="Struktur Uploaddatei Fahrgestllnummern" /></a> &nbsp; * max. 900 Datensätze
                                </td>
                            </tr>
                            <tr id="trSearchKennz" runat="server" visible="false" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtKennzeichen" CssClass="TextBoxNormal" runat="server" 
                                        MaxLength="15"></asp:TextBox></td>
                            </tr>
                            <tr id="trUploadKennz" runat="server" visible="false" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_UploadKennz" runat="server">lbl_UploadKennz</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <input id="upFileKennz" type="file" size="49" name="upFileKennz" runat="server" />&nbsp;
                                     <a href="javascript:openinfo('InfoKennz.htm');"><img src="/Services/Images/info.gif" border="0" height="16px" width="16px" 
                                     alt="Struktur Uploaddatei" title="Struktur Uploaddatei Kennzeichen" /></a> &nbsp; * max. 900 Datensätze
                                </td>
                            </tr>
                            <tr id="trBestandsart" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Art" runat="server">lbl_Art</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:DropDownList ID="ddlBestandsart" runat="server">
                                        <asp:ListItem Selected="True" Text="Alle unvollständigen Daten" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Carporteingang fehlt" Value="C"></asp:ListItem>
                                        <asp:ListItem Text="ZBII-Eingang fehlt" Value="Z"></asp:ListItem>
                                        <asp:ListItem Text="Rückkaufrechnung fehlt" Value="R"></asp:ListItem>
                                        <asp:ListItem Text="Gutachtenfreigabe fehlt" Value="G"></asp:ListItem>
                                        <asp:ListItem Text="Stilllegungsdatum fehlt" Value="S"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2" align="right" style="width: 100%">
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                Height="16px" CausesValidation="False" Font-Underline="False" OnClick="cmdSearch_Click">» Weiter</asp:LinkButton>
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function openinfo(url) {
            fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
            fenster.focus();
        }
 
    </script>
</asp:Content>
