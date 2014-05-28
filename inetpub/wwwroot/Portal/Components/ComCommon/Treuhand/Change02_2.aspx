<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_2.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change02_2" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
    <table cellspacing="0" cellpadding="2" width="100%" align="center">
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
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Absenden</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="lb_zurueck" Visible="false" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <table id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3" height="41">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="LabelExtraLarge" align="left" width="618" height="9">
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                                    <td nowrap align="right" height="9">
                                                        <p align="right">
                                                            &nbsp;
                                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" Height="14px">
                                                            </asp:DropDownList>
                                                        </p>
                                                    </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" BackColor="White" PageSize="200"
                                            headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain" bodyHeight="400"
                                            AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateField SortExpression="SUBRC" HeaderText="col_Anfordern">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Anfordern" runat="server" CommandName="Sort" CommandArgument="SUBRC">col_Anfordern</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                          <asp:CheckBox ID="chkAnfordern" runat="server" 
                                                            AutoPostBack="True"  Enabled='<%# DataBinder.Eval(Container, "DataItem.MESSAGE")= "" %>'  Checked='<%# DataBinder.Eval(Container, "DataItem.SELECT") = "99" %>' oncheckedchanged="chkAnfordern_CheckedChanged"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="EQUI_KEY" HeaderText="col_Schluessel">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Schluessel" runat="server" CommandName="Sort" CommandArgument="EQUI_KEY">col_Schluessel</asp:LinkButton>
                                                    </HeaderTemplate>
                                                     <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEQUI_KEY" Text='<%# DataBinder.Eval(Container, "DataItem.EQUI_KEY") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField SortExpression="ZZREFERENZ2" HeaderText="col_Referenznummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Referenznummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ2">col_Referenznummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                     <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblZZREFERENZ2" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ2") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ERNAM" HeaderText="col_Sachbearbeiter">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Sachbearbeiter" runat="server" CommandName="Sort" CommandArgument="ERNAM">col_Sachbearbeiter</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblERNAM" Text='<%# DataBinder.Eval(Container, "DataItem.ERNAM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ERDAT" HeaderText="col_Datum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Datum" runat="server" CommandName="Sort" CommandArgument="ERDAT">col_Datum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                     <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblERDAT" Text='<%# DataBinder.Eval(Container, "DataItem.ERDAT", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="SPERRDAT" HeaderText="col_Sperrdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Sperrdatum" runat="server" CommandName="Sort" CommandArgument="SPERRDAT">col_Sperrdatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                     <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSPERRDAT" Text='<%# DataBinder.Eval(Container, "DataItem.SPERRDAT", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="TREUH_VGA" HeaderText="col_Aktion">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Aktion" runat="server" CommandName="Sort" CommandArgument="TREUH_VGA">col_Aktion</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblTREUH_VGA" Text="Sperren" Visible='<%# DataBinder.Eval(Container, "DataItem.TREUH_VGA")= "S" %>'>
                                                        </asp:Label>
                                                        <asp:Label runat="server" ID="lblTREUH_VGA2" Text="Ensperren" Visible='<%# DataBinder.Eval(Container, "DataItem.TREUH_VGA")= "F" %>'>
                                                        </asp:Label>                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="MESSAGE" HeaderText="col_Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="MESSAGE">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMESSAGE" Text='<%# DataBinder.Eval(Container, "DataItem.MESSAGE") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
