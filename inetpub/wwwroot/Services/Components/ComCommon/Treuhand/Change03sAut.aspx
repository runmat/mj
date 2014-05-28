<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03sAut.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change03sAut"      MasterPageFile="../../../MasterPage/Services.Master" %>


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
                                           <asp:Label ID="lbl_Fin" runat="server" >lbl_Fin</asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblFinShow" runat="server" ></asp:Label></td>
                                       <td class="firstLeft active" nowrap="nowrap">
                                           <asp:Label ID="lbl_Ersteller" runat="server" >lbl_Ersteller</asp:Label></td>
                                        <td style="width:100%">
                                            <asp:Label ID="lblErstellerShow" runat="server" ></asp:Label></td>                                               
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap">
                                           <asp:Label ID="lbl_Kennzeichen" runat="server" >lbl_Kennzeichen</asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblKennzShow" runat="server" ></asp:Label></td>
                                       <td class="firstLeft active" nowrap="nowrap">
                                           <asp:Label ID="lbl_Ablehnender" runat="server" >lbl_Ablehnender</asp:Label></td>
                                        <td style="width:100%">
                                            <asp:Label ID="lblAblehnShow" runat="server" ></asp:Label></td>                                               
                                    </tr>
                        
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap" style="vertical-align:top">
                                           <asp:Label ID="lbl_Versandadresse" runat="server" >lbl_Versandadresse</asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblAdresseShow" runat="server" ></asp:Label></td>
                                        <td class="firstLeft active"  style="vertical-align:top">
                                           <asp:Label ID="lbl_AblehnGrund" runat="server" >lbl_AblehnGrund</asp:Label></td>
                                        <td style="vertical-align:top">
                                            <asp:Label ID="lblAblehnGrundShow" runat="server" ></asp:Label></td>                                            
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
                               <div id="dataQueryFooter">
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
