<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report07.aspx.cs" Inherits="AppRemarketing.forms.Report07" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
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
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    Selektionsauswahl:
                                </td>
                                <td class="active" style="width: 88%">
                                    <span>
                                        <asp:RadioButton ID="rb_Einzelselektion" Text="Einzelselektion" GroupName="Auswahl"
                                            runat="server" Checked="True" AutoPostBack="True" OnCheckedChanged="rb_Einzelselektion_CheckedChanged" />
                                        &nbsp;
                                         <asp:RadioButton ID="rbFin" Text="Fahrgestellnummer Upload" GroupName="Auswahl"
                                            runat="server" Checked="False" AutoPostBack="True" OnCheckedChanged="rbFin_CheckedChanged" />
                                        &nbsp; </span>
                                </td>
                                 
                            </tr>
                            <tr id="tr1" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtKennzeichen" CssClass="TextBoxNormal" runat="server" 
                                        MaxLength="17" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr2" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtFahrgestellnummer" CssClass="TextBoxNormal" runat="server" 
                                        MaxLength="17" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr3" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Vermieter" runat="server">lbl_Vermieter</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:DropDownList ID="ddlVermieter" runat="server" Width="200px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="tr4" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_DatumVon" runat="server">lbl_DatumVon</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtDatumvon" CssClass="TextBoxNormal" runat="server" 
                                        Width="200px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_Datumvon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtDatumvon">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                    <asp:RequiredFieldValidator  ControlToValidate="txtDatumVon" ID="rfvDatumVon" runat="server" ErrorMessage="Bitte wählen Sie ein Datum."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="tr5" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_DatumBis" runat="server">lbl_DatumBis</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtDatumBis" CssClass="TextBoxNormal" runat="server" 
                                        Width="200px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                     <asp:RequiredFieldValidator    ControlToValidate="txtDatumBis" ID="rfvDatumBis" runat="server" ErrorMessage="Bitte wählen Sie ein Datum."></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Wählen Sie 'Datum bis' größer als 'Datum von'!"
                                        ControlToCompare="txtDatumvon" ControlToValidate="txtDatumBis" Operator="GreaterThanEqual"
                                        Type="Date"></asp:CompareValidator>
                                       
                                        
                                </td>
                            </tr>
                            
                            <tr class="formquery" id="trUploadFin" runat="server" visible="false">
                                    <td class="firstLeft active" valign="top">
                                        <asp:Label ID="lbl_Upload" runat="server">lbl_Upload</asp:Label>
                                    </td>
                                    <td class="active" style="width: 100%">
                                        <input id="upFileFin" type="file" size="35" name="File1" runat="server" />&nbsp;
                                        <a href="javascript:openinfo('InfoFin.htm');"><img src="/Services/Images/info.gif" border="0" height="16px" width="16px" 
                                     alt="Struktur Uploaddatei" title="Struktur Uploaddatei Fahrgestllnummern" /></a> 
                                    &nbsp; * max. 900 Datensätze
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
                                Height="16px" CausesValidation="False" Font-Underline="False" OnClick="cmdSearch_Click">» 
                            Weiter</asp:LinkButton>
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