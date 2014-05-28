<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report02.aspx.vb" Inherits="AppLeasePlan.Report02" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
        <form id="Form1" method="post" runat="server">
        <table id="Table4" width="100%" align="center">
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                                <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="120">
                                <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                    border="0">
                                    <tr>
                                        <td class="TaskTitle" width="150">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="center" width="150">
                                            <asp:LinkButton ID="lb_Create" runat="server" CssClass="StandardButton"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="TaskTitle" valign="top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td valign="top" align="left">
                                            <table id="tblSelektion" cellspacing="0" runat="server" cellpadding="5" width="100%"
                                                border="0" bgcolor="white">
                                                <tr id="tr_AmtlKennzeichen" runat="server">
                                                    <td class="TextLarge" valign="center" width="150">
                                                        <p>
                                                            <asp:Label ID="lbl_AmtlKennzeichen" runat="server"> lbl_AmtlKennzeichen</asp:Label></p>
                                                    </td>
                                                    <td class="TextLarge" valign="center">
                                                        <asp:TextBox ID="txtAmtlKennzeichen" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="tr_Fahrgestellnummer" runat="server">
                                                    <td class="TextLarge" valign="center" width="150">
                                                        <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                                    </td>
                                                    <td class="TextLarge" valign="center">
                                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="tr_Briefnummer" runat="server">
                                                    <td class="TextLarge" valign="center" width="150">
                                                        <asp:Label ID="lbl_Briefnummer" runat="server">lbl_Briefnummer</asp:Label>
                                                    </td>
                                                    <td class="TextLarge" valign="center">
                                                        <asp:TextBox ID="txtBriefnummer" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="tr_Leasingvertragsnr" runat="server">
                                                    <td class="TextLarge" valign="center" width="150">
                                                        <asp:Label ID="lbl_Leasingvertragsnr" runat="server">lbl_Leasingvertragsnr</asp:Label>
                                                    </td>
                                                    <td class="TextLarge" valign="center">
                                                        <asp:TextBox ID="txtOrdernummer" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:GridView AutoGenerateColumns="False" AllowSorting="false" BackColor="White"
                                                runat="server" Width="100%" ID="gvSelectOne">
                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead" />
                                                <Columns>
                                                    <asp:BoundField Visible="false" HeaderText="EQUNR" DataField="EQUNR" ReadOnly="true" />
                                                    <asp:TemplateField>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                                CssClass="StandardButton" CommandName="weiter" Text="Weiter" ID="lbWeiter"> </asp:LinkButton>
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
                                                    <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandArgument="Vertragsnummer"
                                                                CommandName="sort">col_Vertragsnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'
                                                                ID="lblVertragsnummer" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZBIINummer" HeaderText="col_ZBIINummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZBIINummer" runat="server" CommandArgument="ZBIINummer" CommandName="sort">col_ZBIINummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZBIINummer") %>'
                                                                ID="lblZBIINummer" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Referenznummer" HeaderText="col_Referenznummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Referenznummer" runat="server" CommandArgument="Referenznummer"
                                                                CommandName="sort">col_Referenznummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenznummer") %>'
                                                                ID="lblReferenznummer" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Anlagedatum" HeaderText="col_Anlagedatum">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Anlagedatum" runat="server" CommandArgument="Anlagedatum"
                                                                CommandName="sort">col_Anlagedatum</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anlagedatum","{0:d}") %>'
                                                                ID="lblAnlagedatum" Visible="true"> </asp:Label>
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
                                                    <asp:TemplateField SortExpression="Status" HeaderText="col_Status">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Status" runat="server" CommandArgument="Status" CommandName="sort">col_Status</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'
                                                                ID="lblStatus" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Partnernummer" HeaderText="col_Partnernummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Partnernummer" runat="server" CommandArgument="Partnernummer"
                                                                CommandName="sort">col_Partnernummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Partnernummer") %>'
                                                                ID="lblPartnernummer" Visible="true"> </asp:Label>
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
                            <td valign="top">
                                &nbsp;
                            </td>
                            <td valign="top">
                                <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
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
</HTML>
