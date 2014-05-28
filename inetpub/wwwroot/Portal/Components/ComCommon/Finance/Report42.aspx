<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report42.aspx.vb" Inherits="CKG.Components.ComCommon.Report42" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge">
                                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False">Excelformat</asp:LinkButton>&nbsp;&nbsp;
                                                </td>
                                                <td align="right">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelExtraLarge">
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label><asp:Label ID="lblError"
                                                        runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView AutoGenerateColumns="False" AllowSorting="false" BackColor="White"
                                            runat="server" Width="100%" ID="GridView1" AllowPaging="True" 
                                            PageSize="20">
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="EQUNR" SortExpression="EQUNR" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEQUNR" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Personennummer" HeaderText="col_Personennummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Personennummer" runat="server" CommandArgument="Personennummer"
                                                            CommandName="sort">col_Personennummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Personennummer") %>'
                                                            ID="lblPersonennummer" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                            CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                            ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                            CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                            ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Versanddatum" HeaderText="col_Versanddatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versanddatum" runat="server" CommandArgument="Versanddatum"
                                                            CommandName="sort">col_Versanddatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versanddatum","{0:d}") %>'
                                                            ID="lblVersanddatum" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Versandadresse" HeaderText="col_Versandadresse">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandadresse" runat="server" CommandArgument="Status" CommandName="sort">col_Versandadresse</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'
                                                            ID="lblVersandadresse" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Memo" HeaderStyle-Width="300px" HeaderText="col_Memo">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Memo" runat="server" CommandArgument="Memo"
                                                            CommandName="sort">col_Memo</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Memo") %>'
                                                            ID="lblMemo" ToolTip='<%# DataBinder.Eval(Container, "DataItem.User") %>' Visible="true"> </asp:Label>
                                                        <asp:TextBox ID="txtMemo" width="300px" MaxLength="72" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Memo") %>'></asp:TextBox>    
                                                                                                                             
                                                    </ItemTemplate>

<HeaderStyle Width="300px"></HeaderStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtnEdit" runat="server" ToolTip="Memo Neu/Bearbeiten" CommandArgument='<%# Container.DataItemIndex %>'
                                                            CommandName="Show" ImageUrl="../../../Images/EditTableHS.png" Height="16px" Width="16px"
                                                            />  
                                                        <asp:ImageButton ID="ibtnSve" Visible="false" runat="server" ToolTip="Memo Speichern" CommandArgument='<%# Container.DataItemIndex %>'
                                                            CommandName="Save" ImageUrl="../../../Images/saveHS.png" Height="16px" Width="16px"
                                                            />
                                                        <asp:ImageButton ID="ibtnDel" runat="server" ToolTip="Memo Löschen" CommandArgument='<%# Container.DataItemIndex %>'
                                                            CommandName="Del" ImageUrl="../../../Images/icon_nein_s.gif" Height="16px" Width="16px"
                                                            />
                                                        <asp:ImageButton ID="ibtnCancel" Visible="false" runat="server" ToolTip="Abbrechen" CommandArgument='<%# Container.DataItemIndex %>'
                                                            CommandName="Cancel" ImageUrl="../../../Images/selectgrey.gif" Height="16px" Width="16px"
                                                            />                                                                                                                                                                                  
                                                    </ItemTemplate>

<HeaderStyle Width="50px"></HeaderStyle>
                                                </asp:TemplateField>                                                
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
