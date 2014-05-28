<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change01_2.aspx.cs" Inherits="Leasing.forms.Change01_2"
    MasterPageFile="../Master/App.Master" %>

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
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;
                            border: none">
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
                            <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                            OnSorting="GridView1_Sorting1" Style="width: auto;">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>                                               
                                                <asp:TemplateField SortExpression="Modell" HeaderText="col_Modell">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Modell" runat="server" CommandName="Sort" CommandArgument="Marke">col_Modell</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMarke" Text='<%# DataBinder.Eval(Container, "DataItem.Marke") %>'>
                                                                    </asp:Label>
                                                        </br>
                                                                     <asp:Label runat="server" ID="lblModell" Text='<%# DataBinder.Eval(Container, "DataItem.Modell") %>'>
                                                                    </asp:Label>                                                              
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="middle"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Leasingnummer" HeaderText="col_Leasingnummer"
                                                    HeaderStyle-Width="100px">
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Leasingnummer" runat="server" CommandName="Sort" CommandArgument="Modell">col_Leasingnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblLeasingnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kunde" HeaderText="col_Kunde">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kunde" runat="server" CommandName="Sort" CommandArgument="Kunde">col_Kunde</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblKunde" style="white-space:normal;" Text='<%# DataBinder.Eval(Container, "DataItem.Kunde") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="true"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Fahrer" HeaderText="col_Fahrer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrer" runat="server" CommandName="Sort" CommandArgument="Fahrer">col_Fahrer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFahrer" style="white-space:normal;" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="true"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="LW" HeaderText="col_LW">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_LW" runat="server" CommandName="Sort" CommandArgument="LW">col_LW</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblLW" Text='<%# DataBinder.Eval(Container, "DataItem.LW") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="LWCONFIRM" HeaderText="col_LWnochAktuell">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_LWnochAktuell" runat="server" CommandName="Sort" CommandArgument="LWCONFIRM">col_LWnochAktuell</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkConfirm" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.LWCONFIRM") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="LWneu" HeaderText="col_LWneu" HeaderStyle-Width="100px">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_LWneu" runat="server" CommandName="Sort" CommandArgument="LWneu">col_LWneu</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Table runat="server" Width="80px" style="margin-bottom:0;">
                                                            <asp:TableRow >
                                                                <asp:TableCell style="padding-left: 0px;" VerticalAlign="Middle">
                                                                    <asp:TextBox ID="txtLwNeu" runat="server" Width="25px" MaxLength="2" Text='<%# DataBinder.Eval(Container, "DataItem.LWWoche") %>'><%--BorderWidth='1px'--%>
                                                                    </asp:TextBox>
                                                                    <cc1:MaskedEditExtender ID="meeLwNeu" runat="server" TargetControlID="txtLwNeu" Mask="99"
                                                                        MaskType="Number">
                                                                    </cc1:MaskedEditExtender>
                                                                    <asp:Label ID="lblSpacer" runat="server" Width="9px">-</asp:Label>
                                                                    <asp:TextBox ID="txtLwNeuJahr" runat="server" Width="36px" MaxLength="4" Text='<%# DataBinder.Eval(Container, "DataItem.LWJahr") %>'>
                                                                    </asp:TextBox>
                                                                    <cc1:MaskedEditExtender ID="meeLwNeuJahr" runat="server" TargetControlID="txtLwNeuJahr"
                                                                        Mask="9999" MaskType="Number">
                                                                    </cc1:MaskedEditExtender>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle Wrap="False" VerticalAlign="Middle"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Liefertermin" HeaderText="col_Liefertermin">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Liefertermin" runat="server" CommandName="Sort" CommandArgument="Liefertermin">col_Liefertermin</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLiefertermin" runat="server" Width="72px" MaxLength="10" BorderWidth="1px" BorderColor="#969696"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Liefertermin", "{0:d}") %>'>
                                                        </asp:TextBox>
                                                        <cc1:MaskedEditExtender ID="meeLieftermin" runat="server" TargetControlID="txtLiefertermin"
                                                            Mask="99/99/9999" MaskType="Date">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:CalendarExtender ID="ceLiefertermin" runat="server" TargetControlID="txtLiefertermin" Format="d" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" Width="85px" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="NummerIntern" HeaderText="col_NummerIntern">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerIntern" runat="server" CommandName="Sort" CommandArgument="NummerIntern">col_NummerIntern</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtIntNr" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container, "DataItem.NummerIntern") %>'
                                                            MaxLength="10">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Status" HeaderText="col_Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="Status">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStatus" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSend" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                CausesValidation="False" Font-Underline="False" OnClick="cmdSend_Click">» 
                            Absenden</asp:LinkButton>
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
