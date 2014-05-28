<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DHLRetourenLabel.aspx.vb" Inherits="CKG.Components.ComCommon.DHLRetourenLabel"   MasterPageFile= "../../../MasterPage/Services.Master" EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" />
            </div>

            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>

                        <div id="paginationQuery" >
                    <table cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr>
                                <td class="active">
                                    <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                                </td>
                                <td align="right">
                                    <div id="queryImage">
                                        <asp:ImageButton ID="ibtNewSearch" runat="server" visible="false"
                                            ImageUrl="../../../Images/queryArrow.gif" onclick="ibtNewSearch_Click" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:Panel ID="divTrenn" runat="server" visible="false" ><div id="PlaceHolderDiv" ></div>   </asp:Panel>
                </div>
            

                <div id="TableQuery">

   
                <asp:Panel ID="divSelection" runat="server" Visible="true">
              

               <table width="100%" id="tabError" runat="server" cellpadding="0" cellspacing="0" style="border-bottom-width:0">
                           
                              <tr class="formquery" >
                                    <td class="firstLeft active">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false" Visible="false" />
                        <asp:Label ID="lblNoData" runat="server" Font-Bold="true" EnableViewState="false" Visible="false" />
                   
                              </td>
         
                              </tr>
                              </table> 

                <table width="100%" id="Table1" runat="server" cellpadding="0" cellspacing="0" >
                            <tbody>
                                   

                                <tr class="formquery" >
                                    <td class="firstLeft active">
     
                        <table width="100%" id="tab1" runat="server" cellpadding="0" cellspacing="0" >
                            <tbody>
                                   
                                <tr class="formquery">
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:Label ID="lbl_FirstName" runat="server">lbl_FirstName</asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:TextBox ID="txt_FirstName" runat="server" CssClass="TextBoxNormal" 
                                            Width="250px"></asp:TextBox>
                                    </td>
                
                 
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:Label ID="lbl_SecondName" runat="server">lbl_SecondName</asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:TextBox ID="txt_SecondName" runat="server" CssClass="TextBoxNormal" 
                                            MaxLength="17" Width="250px"></asp:TextBox>
                                    </td>
         
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:Label ID="lbl_Street" runat="server">lbl_Street</asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:TextBox ID="txt_Street" runat="server" Width="213"></asp:TextBox>
                                        <asp:TextBox ID="txt_StreetNumber" runat="server" Width="30"></asp:TextBox>
                                                                   

                                    </td>
    
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:Label ID="lbl_PostalCode" runat="server">lbl_PostalCode</asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:TextBox ID="txt_PostalCode" runat="server" Width="40px"></asp:TextBox>
                                        <asp:TextBox ID="txt_City" runat="server" Width="203px"></asp:TextBox>

                                    </td>
                                </tr>

                                 <tr class="formquery">
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:Label ID="lbl_Phone" runat="server">lbl_Phone</asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:TextBox ID="txt_Phone" runat="server" Width="250"></asp:TextBox>
                                    </td>
                                </tr>

               <%--            EMAil Wird nicht verwendet daher unsichtbar  --%>     
                              <tr class="formquery">
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:Label ID="lbl_Mail" visible="false" runat="server">lbl_Mail</asp:Label>
                                    </td>
                                    <td class="firstLeft active" style="height: 22px">
                                        <asp:TextBox ID="txt_Mail" visible="false" runat="server" Width="250"></asp:TextBox>
                                    </td>
                                </tr>
                                    
                            </tbody>
                        </table>
                        </td>
                        <td align="right" style="width:100%;vertical-align:top;padding-right:5px;" >
                            &nbsp</td> 

                                    <td style="width:100%;vertical-align:top;padding-right:5px;">
                                        <asp:Panel ID="Panel1" runat="server" style="text-align:left" Visible="true" width="350px">
                                            <div class="new_layout">
                                                <div id="infopanel" class="infopanel">
                                                    <label style="margin-left:0; margin-right:0; width:275px">
                                                    Hilfe!</label>
                                                    <div>
                                                        Bitte geben Sie Ihre Adressdaten ein und klicken<br/>
                                                        Sie zum Erstellen des Retouren Labels auf 'Weiter'!
                                             
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </td>

                        </tr>
                            <tr class="formquery" >
                            <td>&nbsp</td>  
                            <td>&nbsp;</td> 
                            <td>&nbsp;</td> 
                         </tr>
                        </tbody> 
                        </table>

                             <asp:Literal runat="server" ID="ShowFile" Visible="false" />

                         <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>            
                    </asp:Panel>
                </div>

                 <div id="dataQueryFooter">
                        <asp:LinkButton ID="lb_Search" runat="server" CssClass="Tablebutton" Width="78px"
                            Height="16px" CausesValidation="False" Font-Underline="False" 
                                OnClick="SearchClick">» Weiter</asp:LinkButton>
                 </div>

                 <div id="dataFooter">&nbsp;</div>
                   
                </div>
            </div>
        </div>
    </div>
    </asp:Content>