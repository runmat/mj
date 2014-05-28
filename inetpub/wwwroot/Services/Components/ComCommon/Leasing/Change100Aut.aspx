<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change100Aut.aspx.vb" Inherits="CKG.Components.ComCommon.Change100Aut" MasterPageFile="../../../MasterPage/Services.Master" EnableEventValidation="false" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <%--<asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>--%>
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
                                           <asp:Label ID="lblFahrgestellnummer" runat="server" >Fahrgestellnummer:</asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblFahrgestellnummerShow" runat="server" ></asp:Label></td>   
                                        <td class="firstLeft active" nowrap="nowrap">
                                            <asp:Label ID="lblVersArt" runat="server" >Versandart:</asp:Label>
                                        </td>
                                        <td nowrap="nowrap"  style="width:100%">
                                              <asp:Label ID="lblVersArtShow" runat="server" ></asp:Label></td>

                                    </tr>

                                    <tr class="formquery">

                                       <td class="firstLeft active" nowrap="nowrap">
                                           <asp:Label ID="lbl_Leasingnummer" runat="server" >lbl_Leasingnummer</asp:Label></td>
                                       <td >
                                            <asp:Label ID="lblLeasingnummerShow" runat="server" ></asp:Label></td>     
                                        <td class="firstLeft active" nowrap="nowrap">
                                            <asp:Label ID="lbl_Options" runat="server">lbl_Options</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOptionsShow" runat="server"></asp:Label>
                                        </td>         
                                                                                                                    
                                    </tr>
                                   <tr class="formquery">

                                        <td class="firstLeft active" nowrap="nowrap" style="vertical-align:top">
                                            <asp:Label ID="lbl_NummerZBII" runat="server" >lbl_NummerZBII</asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblNummerZBIIShow" runat="server" ></asp:Label></td>    
                                        <td class="firstLeft active" nowrap="nowrap" style="vertical-align:top">
                                            <asp:Label ID="lbl_Sachbearbeiter" runat="server">lbl_Sachbearbeiter</asp:Label>
                                        </td>
                                                                                  <td>
                                                                                      <asp:Label ID="lblSachbearbeiterShow" runat="server"></asp:Label>
                                        </td>                                         
                                    </tr>   
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap">
                                           <asp:Label ID="lbl_Referenz1" runat="server" >lbl_Referenz1</asp:Label></td>
                                        <td >
                                            <asp:Label ID="lblReferenz1Show" runat="server" ></asp:Label></td>                                    
                                        <td class="firstLeft active" nowrap="nowrap" style="padding-top:2px">
                                            <asp:Label ID="lblAdr" runat="server">Versandadresse:</asp:Label>
                                        </td>
                                          <td rowspan="2" valign="top">
                                            <asp:Label ID="lblAdrShow" runat="server"></asp:Label>
                                        </td>                                                  
                                    </tr>    
                                   <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap"  rowspan="1">
                                           <asp:Label ID="lbl_Referenz2" runat="server" >lbl_Referenz2</asp:Label></td>
                                        <td  rowspan="1">
                                            <asp:Label ID="lblReferenz2Show" runat="server"></asp:Label></td> 
                                        <td class="firstLeft active"  rowspan="1">
                                            &nbsp;</td>
                                    </tr>                                                                                      
                                     <tr class="formquery">
                                         <td class="firstLeft active" nowrap="nowrap" colspan="4" style="height: 22px">
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
