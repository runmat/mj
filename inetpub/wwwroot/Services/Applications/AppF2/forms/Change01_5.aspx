<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_5.aspx.vb" Inherits="AppF2.Change01_5"  MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change01.aspx">Suche</asp:HyperLink>&nbsp;<asp:HyperLink
                        ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change01_2.aspx">| Fahrzeugauswahl</asp:HyperLink>
                    <asp:HyperLink ID="lnkAdressAuswahl" runat="server" NavigateUrl="Change01_3.aspx"
                        Visible="False"> | Adressauswahl</asp:HyperLink>
                    <asp:HyperLink ID="lnkAbrufgrund" runat="server" NavigateUrl="Change01_4.aspx"
                        > | Abrufgründe</asp:HyperLink>                        
                    <a class="active">| Senden</a>                        
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="TableQuery">
                                    <table id="tblMessage" runat="server" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="4" class="firstLeft active">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td nowrap="nowrap" class="firstLeft active" colspan="3">
                                                <u>Hinweis:</u>&nbsp;Überzählige Anforderungen werden gesperrt angelegt.
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="3" nowrap="nowrap">
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                Kontingentart <u>Standard temporär</u>:
                                            </td>
                                            <td class="active" nowrap="nowrap">
                                                <strong>
                                                    <asp:Label ID="lblTemp" runat="server"></asp:Label></strong>
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap">
                                                überzählige Anforderung(en)
                                            </td>
                                            <td class="active" nowrap="nowrap" width="100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" nowrap="nowrap">
                                                Kontingentart <u>Standard endgültig</u>:
                                            </td>
                                            <td class="active" nowrap="nowrap">
                                                <asp:Label ID="lblEnd" runat="server"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap">
                                                überzählige Anforderung(en)&nbsp;
                                            </td>
                                            <td class="active" nowrap="nowrap" width="100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="3" nowrap="nowrap">
                                                <i>
                                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                                </i>&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="Table7" cellspacing="0" cellpadding="5" width="100%" border="0" bgcolor="white">
                                        <tr class="formquery">
                                            <td class="firstLeft active" valign="top" style="height: 36px">
                                                Versandart:
                                            </td>
                                            <td class="active" style="width: 90%; height: 36px;">
                                                <asp:Label ID="lblVersandart" runat="server"></asp:Label><asp:Label ID="lblMaterialNummer"
                                                    runat="server" Visible="False"></asp:Label><asp:Label ID="lblVersandhinweis" runat="server"
                                                        Visible="False"> - Gilt nicht für gesperrt angelegte Anforderungen!</asp:Label>
                                            </td>
                                            <td class="active" nowrap="nowrap" style="height: 36px">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Adresse:
                                            </td>
                                            <td class="active" style="width: 90%">
                                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                            </td>
                                            <td class="active" nowrap="nowrap">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                        CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                        AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                        <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                        <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                        <PagerSettings Visible="False" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <EmptyDataRowStyle BackColor="#DFDFDF" />
                                        <Columns>
                                            <asp:TemplateField SortExpression="MANDT" Visible="False" HeaderText="col_MANDT">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_MANDT" runat="server" CommandName="Sort" CommandArgument="MANDT">col_Kontonummer</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MANDT") %>'
                                                        ID="lblMandt">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ZZREFERENZ1" HeaderText="col_Vertragsnummer">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Vertragsnummer</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'
                                                        ID="Label2">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="LIZNR" HeaderText="col_Kontonummer">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Kontonummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Kontonummer</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'
                                                        ID="Label1">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="TIDNR" HeaderText="col_NummerZB2">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'
                                                        ID="Label3">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>'
                                                        ID="lblFahrg">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="LICENSE_NUM"
                                                        CommandName="Sort">col_Kennzeichen</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False" SortExpression="ZZBEZAHLT" HeaderText="Bezahlt">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkBezahlt" runat="server" Enabled="False"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ZZCOCKZ">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_COC" runat="server" CommandArgument="LICENSE_NUM" CommandName="Sort">col_COC</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Checkbox2" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'
                                                        Enabled="False"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField  HeaderStyle-ForeColor="White" SortExpression="MANDT" HeaderText="Versandart">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTemp2" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="1" %>'>Temporär</asp:Label>
                                                    <asp:Label ID="lblEndg" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="2" %>'>Endgültig</asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField  HeaderStyle-ForeColor="White" HeaderText="Abrufgrund-Info/Text">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAUGRU" runat="server" NAME="Label6" ToolTip='<%# DataBinder.Eval(Container, "DataItem.ANFNR") %>'
                                                        Text='<%# DataBinder.Eval(Container, "DataItem.AUGRU_Klartext") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           <asp:TemplateField Visible="False"  HeaderStyle-ForeColor="White" SortExpression="VBELN" HeaderText="col_VBELN">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_VBELN" runat="server" CommandName="Sort" CommandArgument="VBELN">col_VBELN</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VBELN") %>'
                                                        ID="lblVbeln">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField Visible="False"  HeaderStyle-ForeColor="White" SortExpression="COMMENT" HeaderText="col_COMMENT">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_COMMENT" runat="server" CommandName="Sort" CommandArgument="COMMENT">col_COMMENT</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.COMMENT") %>'
                                                        ID="lblComment">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div id="dataFooter">
                                    <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px">» Absenden</asp:LinkButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
