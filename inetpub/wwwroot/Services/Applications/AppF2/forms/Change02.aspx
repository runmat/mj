<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02.aspx.vb" Inherits="AppF2.Change02"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

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
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="Table1" cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr class="formquery">
                                            <td nowrap="nowrap" class="firstLeft active" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblNoData" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft">
                                                <span>
                                                    <asp:RadioButton ID="rb_ZBI" runat="server" Text="rb_ZBI" Checked="True" GroupName="Abweichungen" /></span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft">
                                                <span>
                                                    <asp:RadioButton ID="rb_COC" runat="server" Text="rb_COC" GroupName="Abweichungen" /></span>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft">
                                                <span>
                                                    <asp:RadioButton ID="rb_Abrufgrund" runat="server" Text="rb_Abrufgrund" GroupName="Abweichungen" /></span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="btnConfirm" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                    Width="130px">» Abfrage starten</asp:LinkButton>
                            </div>
                            <div id="Result" visible="false" runat="Server">
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto">
                                        <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                            AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EmptyDataRowStyle BackColor="#DFDFDF" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEQUNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Anforderer" HeaderText="col_Anforderer">
                                                    <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anforderer" runat="server" CommandName="Sort" CommandArgument="Anforderer">col_Anforderer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAnforderer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anforderer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Haendlernummer" HeaderText="col_Haendlernummer">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Haendlernummer" runat="server" CommandName="Sort" CommandArgument="Haendlernummer">col_Haendlernummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnHaendlernummer" ToolTip="Filtern" CommandArgument="Haendlernummer"
                                                            CommandName="Filter" Text='<%# DataBinder.Eval(Container, "DataItem.Haendlernummer") %>'
                                                            runat="server">LinkButton</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="LIZNR" HeaderText="col_LIZNR">
                                                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_LIZNR" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_LIZNR</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                    <HeaderStyle Width="70px"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVertragsnummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderStyle Width="120px"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField ControlStyle-Width="25px" DataField="Distrikt" SortExpression="Distrikt"
                                                    ReadOnly="True" HeaderText="Distrikt"></asp:BoundField>
                                                <asp:TemplateField SortExpression="Nummer ZB2" HeaderText="col_Briefnummer">
                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Briefnummer" runat="server" CommandName="Sort" CommandArgument="Nummer ZB2">col_Briefnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Nummer ZBII") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Memo" HeaderText="Memo">
                                                    <HeaderStyle Width="150px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMemo" Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Memo") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField ControlStyle-Width="80px" DataField="Datum Ausgang" SortExpression="Datum Ausgang"
                                                    ReadOnly="True" HeaderText="Datum Ausgang" DataFormatString="{0:dd.MM.yyyy}">
                                                </asp:BoundField>
                                                <asp:BoundField ControlStyle-Width="80px" DataField="Datum Eingang" SortExpression="Datum Eingang"
                                                    ReadOnly="True" HeaderText="Datum Eingang" DataFormatString="{0:dd.MM.yyyy}">
                                                </asp:BoundField>
                                                <asp:TemplateField  HeaderText="Abweichung">
                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <p align="center">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                                CommandName="Edit" ImageUrl="../../../Images/Plus.gif" Height="16px" Width="16px" /></p>
                                                    </ItemTemplate>
                                                   
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Erledigt">
                                                    <HeaderStyle Width="30px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <p align="center">
                                                            <asp:ImageButton ID="lbErledigt" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                                CommandName="Erledigt" ImageUrl="../../../Images/Confirm_mini.gif" Height="16px"
                                                                Width="16px" />
                                                        </p>
                                                    </ItemTemplate>
                                                   
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>
                         
                            <div>
                                <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                                <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                    PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancel">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="mb" runat="server" BackColor="White" Width="1150"
                                    Style="display: none; border:  solid 2px #000000" >
                                   <%-- Style="display: none"--%>
                                    <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px;">
                                        <asp:Label ID="lblAdressMessage" runat="server" Text="Detailinformationen" Font-Bold="True"></asp:Label>
                                    </div>
                                    <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 5px; padding-bottom: 5px;padding-right:10px">
                                        <table style="width: 100%">
                                            <tr>
                                                <td nowrap="nowrap" valign="top">
                                                    Memo erfassen:
                                                </td>
                                                <td style="width:100%">
                                                    <asp:TextBox Visible="True" ID="txtMemo1" TextMode="MultiLine" runat="server" Width="99%"
                                                    Rows="5" BorderColor="#990000" BorderStyle="Solid" BorderWidth="1" Text=""
                                                    Font-Names="Verdana" Font-Size="11px" MaxLength="120"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" >
                                                    <asp:Label ID="lblEQUNR" runat="server" Visible="false" Text=""></asp:Label>
                                                </td>
                 
                                            </tr>                                            
                                            <tr>
                                                <td colspan="2" >
                                            <asp:GridView ID="GridView2" Width="100%" runat="server" AutoGenerateColumns="True" 
                                            CellPadding="0" CellSpacing="0" GridLines="Vertical" AlternatingRowStyle-BackColor="#DEE1E0"
                                            AllowSorting="false" AllowPaging="false" CssClass="GridView" PageSize="20">
                                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EmptyDataRowStyle BackColor="#DFDFDF" />
                                            <Columns>
                                            </Columns>
                                            
                                            </asp:GridView>
                                                                                            </td>
                 
                                            </tr>
                                                    <tr>
                                                <td colspan="2" >
                                                    &nbsp;
                                                </td>
                 
                                            </tr>   
                                        </table>
                                    </div>
                                    <table width="100%">
                                        <tr>
                                                <td align="right" style="width:100%;padding-right: 15px;padding-bottom:10px">
                                                    <asp:Button ID="btnOK" runat="server" Text="Speichern" CssClass="TablebuttonLarge"
                                                        Font-Bold="True" Width="90px" />
                                                </td>                                        
                                            <td align="right" class="rightPadding" style="padding-right: 15px;padding-bottom:10px">
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
