<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change40Edit.aspx.vb"
    Inherits="CKG.Components.ComCommon.Change40Edit" %>

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
    <table cellspacing="0" cellpadding="0" width="100%" align="center">
        <tr>
            <td colspan="3">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2" height="19">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>)
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
                                        <asp:LinkButton ID="cmdSave" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Sichern</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdConfirm" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Bestätigen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdReset" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Verwerfen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdAuthorize" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Autorisieren</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdDelete" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Löschen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:HyperLink ID="cmdBack2" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;<asp:HyperLink ID="lnkKreditlimit" runat="server" NavigateUrl="Change40.aspx"
                                            CssClass="TaskTitle">Händlersuche</asp:HyperLink>&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td class="TextLarge" valign="top">
                                        <asp:Label ID="lbl_HaendlerNummer" runat="server">Händlernummer:</asp:Label>
                                    </td>
                                    <td class="TextLarge" valign="top" width="100%" colspan="2">
                                        <asp:Label ID="lblHaendlerNummer" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="StandardTableAlternate" valign="top">
                                        Name:&nbsp;&nbsp;
                                    </td>
                                    <td class="StandardTableAlternate" valign="top" colspan="2">
                                        <asp:Label ID="lblHaendlerName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" valign="top">
                                        Adresse:
                                    </td>
                                    <td class="TextLarge" valign="top" colspan="2">
                                        <asp:Label ID="lblAdresse" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="StandardTableAlternate" valign="top">
                                       <asp:Label ID="Label1" runat="server" Text="Insolvenz"></asp:Label>
                                    </td>
                                    <td class="StandardTableAlternate" valign="top" colspan="2">
                                       <asp:CheckBox ID="chkInsolvent" runat="server" AutoPostBack="True" />
                                    </td>
                                </tr>                                
                            </table>
                            <table id="Table7" cellspacing="0" cellpadding="0" width="100%" border="0">
                               <tr>
                                    <td>
                                        &nbsp;</td>
                                 
                                </tr>                                
                                <tr>
                                    <td >
                                        <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="3">
                                            <AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
                                            <ItemStyle CssClass="TextLarge"></ItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn Visible="False" DataField="Kreditkontrollbereich" HeaderText="Kreditkontrollbereich">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Altes Kontingent">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblKontingent_Alt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Alt") %>'>
                                                        </asp:Label>
                                                        <asp:Label ID="lblRichtwert_Alt" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Alt") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Ausschoepfung" HeaderText="Inanspruchnahme">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Freies Kontingent">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFrei" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Frei") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Gesperrt - Alt">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Gesperrt_Alt" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Alt") %>'
                                                            Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Neues Kontingent">
                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image2" runat="server" Width="12px" ImageUrl="/Portal/Images/empty.gif"
                                                            Height="12px"></asp:Image>
                                                        <asp:TextBox ID="txtKontingent_Neu" runat="server" CssClass="InputRight" Width="50px"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent_Neu") %>'>
                                                        </asp:TextBox>
                                                        <asp:TextBox ID="txtRichtwert_Neu" runat="server" CssClass="InputRight" Width="50px"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Richtwert_Neu") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Gesperrt - Neu ">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgGesperrt_Neu" runat="server" Width="12px" ImageUrl="/Portal/Images/empty.gif"
                                                            Height="12px"></asp:Image>
                                                        <asp:CheckBox ID="chkGesperrt_Neu" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt_Neu") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="False" HeaderText="ZeigeKontingentart">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkZeigeKontingentart" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZeigeKontingentart") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table8" cellspacing="0" cellpadding="5" width="100%" border="0">
                                <tr id="ConfirmMessage" runat="server">
                                    <td class="LabelExtraLarge">
                                        <asp:Label ID="lblInformation" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
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
