<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change100sAut.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change100sAut" MasterPageFile="../../../MasterPage/Services.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                            <div id="TableQuery">
                               <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr  class="formquery" >
                                        <td class="firstLeft active" colspan="4"  style="width:100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                ></asp:Label>
                                                
                                            <asp:Label ID="lblNoData" runat="server" ></asp:Label>                                                
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap">
                                            <asp:Label ID="lbl_Treunehmer" runat="server" >lbl_Treunehmer</asp:Label>
                                        </td>
                                        <td nowrap="nowrap">
                                              <asp:Label ID="lblTreunehmShow" runat="server" ></asp:Label></td>
                                       <td class="firstLeft active" nowrap="nowrap">
                                           <asp:Label ID="lbl_Aktion" runat="server" >lbl_Aktion</asp:Label></td>
                                        <td style="width:100%">
                                            <asp:Label ID="lblAktionShow" runat="server" ></asp:Label></td>   
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap">
                                           <asp:Label ID="lbl_Referenz" runat="server" >lbl_Referenz</asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblRefShow" runat="server" ></asp:Label></td>
                                       <td class="firstLeft active" nowrap="nowrap">
                                           <asp:Label ID="lbl_Sachbearbeiter" runat="server" >lbl_Sachbearbeiter</asp:Label></td>
                                        <td style="width:100%">
                                            <asp:Label ID="lblSachbShow" runat="server" ></asp:Label></td>                                               
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap">
                                           <asp:Label ID="lbl_Datum" runat="server" >lbl_Datum</asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblDatumShow" runat="server" ></asp:Label></td>
                                       <td class="firstLeft active" nowrap="nowrap">
                                           <asp:Label ID="lbl_SperrDatum" runat="server" >lbl_SperrDatum</asp:Label></td>
                                        <td style="width:100%" >
                                            <asp:Label ID="lbl_SperrDatShow" runat="server" >lbl_SperrDatShow</asp:Label></td>                                               
                                    </tr>
                        
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap" style="vertical-align:top">
                                            &nbsp;&nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        <td class="firstLeft active"  style="vertical-align:top">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>                                            
                                    </tr>                                                                        
                                     <tr class="formquery">
                                         <td class="firstLeft active" nowrap="nowrap" colspan="4">
                                            <asp:Label ID="lblBELNR" runat="server" Visible="false" />&nbsp;</td>

                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" colspan="4"  style="background-color: #dfdfdf; height: 22px;">
                                             &nbsp;</td>
                                     </tr>
                                </table>

                                </div>
                               <div id="dataQueryFooter" >
                                 <asp:LinkButton ID="cmdSave" runat="server" CssClass="TablebuttonLarge" Height="16px" Width="130px">» Autorisieren</asp:LinkButton>
                                 <asp:LinkButton ID="cmdDel" runat="server" CssClass="TablebuttonLarge" Height="16px" Width="130px">» Löschen</asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
