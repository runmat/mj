<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report08.aspx.vb" Inherits="AppF2.Report08"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <div id="paginationQuery" style="width: 100%; display: block;">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td nowrap="nowrap" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:LinkButton ID="btnConfirm" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                    Width="130px">» Abfrage starten</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="DivPlaceholder" runat="server" style="height: 550px;">
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lbCreateExcel" runat="server" Text="Excel herunterladen" ForeColor="White"></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="gridview">
                                                    <PagerSettings Visible="false" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Endgültiger Versand">
                                                             <ItemTemplate>
                                                                <asp:ImageButton ID="ibtFreigabe" runat="server" 
                                                                    CommandArgument="<%# Container.DataItemIndex %>" CommandName="Freigabe" 
                                                                    Height="20px" ImageUrl="../../../Images/Confirm_mini.gif" 
                                                                    ToolTip="Endgültiger Versand" Width="20px" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:TemplateField>
                                                                                  
                                                        <asp:TemplateField HeaderText="Equipment" SortExpression="Equnr" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEqunr" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                
                                                        <asp:BoundField DataField="Vertragsnummer" SortExpression="Vertragsnummer" HeaderText="Vertragsnummer"
                                                            ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen"
                                                            ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Versanddatum" SortExpression="Versanddatum" HeaderText="Versanddatum"
                                                            ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField" DataFormatString="{0:dd.MM.yyyy}">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Versandadresse" SortExpression="Versandadresse"
                                                            HeaderText="Versandadresse" ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Distrikt" SortExpression="Distrikt"
                                                            HeaderText="Distrikt" ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Versandgrund" SortExpression="Versandgrund"
                                                            HeaderText="Versandgrund" ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Finanzierungsart" SortExpression="Finanzierungsart"
                                                            HeaderText="Finanzierungsart" ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Webuser" SortExpression="Webuser"
                                                            HeaderText="Webuser" ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="Tablehead" ForeColor="White" />
                                                    <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                          <div>
                                <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                                <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                    PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnOK"
                                    X="450" Y="200">
                                </ajaxToolkit:ModalPopupExtender>
           
                                <asp:Panel ID="mb" runat="server" Width="300px" Height="130px" 
                                    BackColor="White" style="display:none">
                                    <div style="padding-left:10px;padding-top:15px;margin-bottom:10px;">
                                        <asp:Label ID="lblAbfrage" runat="server" Font-Bold="True"></asp:Label>
                                    </div>
                                    <div style="padding-left:50px;padding-top:15px;margin-bottom:10px;">
                                        
                                            <table>
                                                <tr>
                                                    <td style="padding-left:10px">
                                                    <asp:Button ID="btnOK" runat="server" Text="JA" CssClass="TablebuttonLarge" 
                                                Font-Bold="True" Width="80px" />                                                       
                                                       </td>
                                                    <td>

                                                        <asp:Button ID="btnCancel" runat="server" CssClass="TablebuttonLarge" 
                                                            Font-Bold="true" Text="Nein" Width="80px" />
                                                    </td>
                                                </tr>
                                            </table>
                                    </div>
                                </asp:Panel>
                            </div>                            
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbCreateExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>
        </div>
    </div>
</asp:Content>
