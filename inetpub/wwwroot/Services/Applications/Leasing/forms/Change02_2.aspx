<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change02_2.aspx.cs" Inherits="Leasing.forms.Change02_2"    MasterPageFile="../Master/App.Master" %>

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
                
                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;border: none">
                        <tr class="formquery">
                        
                            <td class="firstLeft active" style="width: 100%; height: 19px;">
                                <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red"></asp:Label>
                                <asp:Label ID="lblNoData" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
                    <div id="Result" runat="Server">
                   
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                            Style="width: 100%;" 
                                            onrowcommand="GridView1_RowCommand" onsorting="GridView1_Sorting">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead" ></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                      <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblID" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                            
                                                <asp:TemplateField SortExpression="MARKE_TXT" HeaderText="col_Marke">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Marke" runat="server" CommandName="Sort" CommandArgument="MARKE_TXT">col_Marke</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMarke" Text='<%# DataBinder.Eval(Container, "DataItem.MARKE_TXT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="MODELL_TXT" HeaderText="col_Modell">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Modell" runat="server" CommandName="Sort" CommandArgument="MODELL_TXT">col_Modell</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblModell" Text='<%# DataBinder.Eval(Container, "DataItem.MODELL_TXT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="LIEFERWOCHE" HeaderText="col_LW">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_LW" runat="server" CommandName="Sort" CommandArgument="LIEFERWOCHE">col_LW</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
																<asp:TextBox id="txtLW" runat="server" Width="59px" Enabled='<%# DataBinder.Eval(Container, "DataItem.LIEFERWOCHE") == null || (DataBinder.Eval(Container, "DataItem.LIEFERWOCHE").ToString()!= "Diff.") %>' MaxLength="5" Text='<%# DataBinder.Eval(Container, "DataItem.LIEFERWOCHE") %>'>
																</asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                                <asp:TemplateField HeaderText="Neue mögliche Lieferwoche"  HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lnkLW" runat="server" ToolTip="erfassen" CommandArgument='<%#  DataBinder.Eval(Container, "DataItem.ID") %>'
                                                            CommandName="OpenTyp" ImageUrl="../../../Images/Edit_01.gif" Height="16px" Width="16px"/>                                                    
                                                            
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bemerkungen" >
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lnkBemerkungen" runat="server" ToolTip="erfassen" CommandArgument='<%#  DataBinder.Eval(Container, "DataItem.ID") %>'
                                                            CommandName="OpenBem" ImageUrl="../../../Images/Edit_01.gif" Height="16px" Width="16px"/>                                                      
                                                            
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="STATUS" HeaderText="col_Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="STATUS">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStatus" Text='<%# DataBinder.Eval(Container, "DataItem.STATUS") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                               
                                            </Columns>
                                        </asp:GridView>
                                  </div>
                        <div id="dataQueryFooter"><asp:LinkButton ID="cmdSend" runat="server" 
                                CssClass="Tablebutton" Width="78px"
                                Height="16px" CausesValidation="False" Font-Underline="False" onclick="cmdSend_Click" 
                               >» Speichern</asp:LinkButton>
                            &nbsp;
                        </div>
                    </div>
                            <div>
                                <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                               
                                <cc1:ModalPopupExtender ID="mpeDetails" runat="server" TargetControlID="btnFake"
                                    PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancel">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="mb" runat="server" Width="500px" Height="250px" BackColor="White"
                                    Style="display: none; border:  solid 2px #595959" >
                                   <%-- Style="display: none"--%>
                                    <div class="GridTableHead" style="padding-left: 10px;">
                                        <asp:Label ID="lblAdressMessage" runat="server" Text="Detailinformationen" Font-Bold="True"></asp:Label>
                                    </div>
                                    <div style="padding: 15px 10px 5px 10px" >
                                        <table style="width: 100%;color:#595959" >
                                        <tr>
                                            <td colspan="2">
                                            <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView2"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                            Style="width: 100%;" >
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead" ></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                               <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblUID" Text='<%# DataBinder.Eval(Container, "DataItem.UID") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                            
                                               <asp:TemplateField SortExpression="MARKE_TXT" HeaderText="col_Marke">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Marke" runat="server" CommandName="Sort" CommandArgument="MARKE_TXT">col_Marke</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMarke" Text='<%# DataBinder.Eval(Container, "DataItem.MARKE_TXT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="MODELL_TXT" HeaderText="col_Modell">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Modell" runat="server" CommandName="Sort" CommandArgument="MODELL_TXT">col_Modell</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblModell" Text='<%# DataBinder.Eval(Container, "DataItem.MODELL_TXT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:TemplateField SortExpression="BODYTYP_TEXT" HeaderText="col_Typ">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="BODYTYP_TEXT">col_Typ</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblTyp" Text='<%# DataBinder.Eval(Container, "DataItem.BODYTYP_TEXT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:TemplateField SortExpression="LIEFERWOCHE" HeaderText="col_LW">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_LW" runat="server" CommandName="Sort" CommandArgument="LIEFERWOCHE">col_LW</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtLWGrid" Width="72px" Text='<%# DataBinder.Eval(Container, "DataItem.LIEFERWOCHE") %>'>
                                                        </asp:TextBox>
                                                        <cc1:TextBoxWatermarkExtender ID="extWatermarkEmail" runat="server" TargetControlID="txtLWGrid" WatermarkText="WW.JJ" WatermarkCssClass="Watermarked"></cc1:TextBoxWatermarkExtender>
                                                     </ItemTemplate>
                                                </asp:TemplateField>                                                                                                 
                                            </Columns>
                                            </asp:GridView>
                                            </td>
                                        
                                        </tr>
                                        
                                           <tr id="trBEM" runat="server">
                                                <td nowrap="nowrap" valign="top" style="padding-top: 10px;font-weight:bold">
                                                    Bemerkung:
                                                </td>
                                                <td style="width:100%" valign="top">
                                                    <asp:Label ID="lblBem" runat="server" ></asp:Label>
                                                </td>  
                                            </tr>                                             
                                            <tr  id="trBEMEdit" runat="server">
                                                <td nowrap="nowrap" valign="top" style="padding-top: 10px;font-weight:bold">
                                                    Bemerkung/LW bearbeiten :
                                                </td>
                                                <td style="width:100%" valign="top">
                                                    <asp:TextBox ID="txtBEM" Width="200px" runat="server"></asp:TextBox><span style="padding-right: 3px;padding-left: 3px;font-weight:bold" >/</span>
                                                    <asp:TextBox ID="txtBEMLW" Width="72px" runat="server"></asp:TextBox>
                                                   <cc1:TextBoxWatermarkExtender ID="extWatermarkBEM" runat="server" TargetControlID="txtBEM" WatermarkText="Bemerkung" WatermarkCssClass="Watermarked"></cc1:TextBoxWatermarkExtender></asp:Label>
                                                    <cc1:TextBoxWatermarkExtender ID="extWatermarkBEMLW" runat="server" TargetControlID="txtBEMLW" WatermarkText="WW.JJ" WatermarkCssClass="Watermarked"></cc1:TextBoxWatermarkExtender></asp:Label>
                                                </td>  
                                            </tr>  
                                            <tr  id="trError" runat="server" visible="false">
                                                <td colspan="2" style="width:100%">
                                                    <asp:Label ID="lblErrorDetail" runat="server"  ></asp:Label>
                                                </td>
                                                           </tr>                                                                                         
                                            <tr  id="trData" runat="server" visible="false">
                                                <td>
                                                    <asp:Label ID="lblID" runat="server"  ></asp:Label>
                                                </td>
                                                <td style="width:100%" valign="top">
                                                    <asp:Label ID="lblAktion" runat="server"  ></asp:Label>
                                                   
                                                </td>  
                                            </tr>                                             
                                        </table>
                                    </div>
                                    <table width="100%">
                                        <tr>
                                                <td align="right" style="width:100%;padding-right: 15px">
                                                    <asp:Button ID="btnOK" runat="server" Text="Speichern" CssClass="TablebuttonLarge"
                                                        Font-Bold="True" Width="90px" onclick="btnOK_Click" />
                                                </td>                                        
                                                <td align="right" class="rightPadding" style="padding-right: 15px">
                                                    <asp:Button ID="btnCancel" runat="server" Text="Schließen" CssClass="TablebuttonLarge"
                                                        Font-Bold="true" Width="90px" />
                                                </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>

