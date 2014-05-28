<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report03.aspx.cs" Inherits="Leasing.forms.Report03" EnableEventValidation="false"  MasterPageFile="../Master/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
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
                            <div id="TableQuery" style="margin-bottom: 10px">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;border: none">
                                <tr class="formquery">
                                
                                    <td class="firstLeft active" style="width: 100%; height: 19px;">
                                        <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red"></asp:Label>
                                        <asp:Label ID="lblNoData" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            </div>
                            <div id="Result" visible="false" runat="Server">
                                <div class="ExcelDiv">
                                    <div align="right">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lbCreateExcel" runat="server" 
                                            Text="Excel herunterladen" ForeColor="White" onclick="lbCreateExcel_Click"></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>                            
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                        <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                            AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20" 
                                            onsorting="GridView1_Sorting" onrowcommand="GridView1_RowCommand1" >
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EmptyDataRowStyle BackColor="#DFDFDF" />
                                            <Columns>
                                                <asp:TemplateField  HeaderText="Memo">
                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <p align="center">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'
                                                                CommandName="Memo" ToolTip='<%# DataBinder.Eval(Container, "DataItem.MemoString") %>' ImageUrl="../../../Images/Edit_01.gif" Height="16px" Width="16px" /></p>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                            
                                                <asp:TemplateField SortExpression="Memo" HeaderText="col_Memo">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Memo" runat="server" CommandName="Sort" CommandArgument="Memo">col_Memo</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMemo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Memo") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVertragsnummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                   
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                   
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblKennzeichen" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Briefnummer"  Visible="false"  HeaderText="col_Briefnummer">
                                                   
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Briefnummer" runat="server" CommandName="Sort" CommandArgument="Briefnummer">col_Briefnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBriefnummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Briefnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>    
                                                <asp:TemplateField SortExpression="Versand" HeaderText="col_Versand">
                                                   
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versand" runat="server" CommandName="Sort" CommandArgument="Versand">col_Versand</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVersand" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versand","{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>  
                                                <asp:TemplateField SortExpression="Versandgrund" HeaderText="col_Versandgrund">
                                                   
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandgrund" runat="server" CommandName="Sort" CommandArgument="Versandgrund">col_Versandgrund</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVersandgrund" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandgrund") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>   
                                                <asp:TemplateField SortExpression="Name1" HeaderText="col_Name1">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Name1" runat="server" CommandName="Sort" CommandArgument="Name1">col_Name1</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name1") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                <asp:TemplateField SortExpression="Strasse" HeaderText="col_Strasse">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Strasse" runat="server" CommandName="Sort" CommandArgument="Strasse">col_Strasse</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStrasse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Strasse") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>  
                                                <asp:TemplateField SortExpression="Ort" HeaderText="col_Ort">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ort" runat="server" CommandName="Sort" CommandArgument="Ort">col_Ort</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ort") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>         
                                                <asp:TemplateField SortExpression="Mahnstufe" Visible="false" HeaderText="col_Mahnstufe">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Mahnstufe" runat="server" CommandName="Sort" CommandArgument="Mahnstufe">col_Mahnstufe</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMahnstufe" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Mahnstufe") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                                                                                                                                                                                                                                                                                                        


                                            </Columns>
                                        </asp:GridView>
                                </div>
                            </div>
                         
                            <div>
                                <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                                <asp:Button ID="Button1" runat="server" Style="display: none" Text="BUTTON" OnClick="Button1_Click" />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                    PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancel">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="mb" runat="server" Width="350px" Height="150px" BackColor="White"
                                    Style="display: none ;border:  solid 2px #000000" >
                                   <%--display: none ;--%>
                                    <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px;">
                                        <asp:Label ID="lblAdressMessage" runat="server" Text="Memo erfassen" Font-Bold="True"></asp:Label>
                                    </div>
                                    <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 5px; padding-bottom: 5px">
                                        <table style="width: 100%">
                                            <tr>
                                                <td nowrap="nowrap" valign="top">
                                                    Memo erfassen für Vertragsnummer: &nbsp;<asp:Label ID="lblVertag" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%">
                                                    <asp:TextBox ID="txtMemo1" runat="server" BorderColor="#990000" BorderStyle="Solid"
                                                        BorderWidth="1" Font-Names="Verdana" Font-Size="11px" MaxLength="120" Width="95%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
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

                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
