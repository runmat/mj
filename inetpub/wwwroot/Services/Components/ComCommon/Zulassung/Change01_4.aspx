<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_4.aspx.vb" Inherits="CKG.Components.ComCommon.Zulassung.Change01_4"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change01.aspx">Fahrzeugsuche</asp:HyperLink>
                    <asp:HyperLink ID="lnkFahrzeugauswahl" runat="server" NavigateUrl="Change01_2.aspx">| Fahrzeugauswahl</asp:HyperLink>
                    <asp:HyperLink ID="lnkAdressen" runat="server" NavigateUrl="Change01_3.aspx">| Adressen/Zulassungsdaten</asp:HyperLink>
                    <a class="active">| Wunschkennzeichen</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <div id="TableQuery">
                            <table id="tab1" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="trWunschInfo" runat="Server">
                                        <td class="firstLeft active">
                                            Bei reservierten Wunschkennzeichen: <span style="color: #C20000">Ausdrucke der Reservierungsbestätigung
                                                dringend dem DAD zukommen lassen!</span>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="Result" runat="Server">
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding">
                                            &nbsp;</div>
                                    </div>
                                    <div id="pagination">
                                        <uc2:GridNavigation ID="GridNavigation1" runat="server">
                                        </uc2:GridNavigation>
                                    </div>
                                    <div id="data">
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EditRowStyle></EditRowStyle>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtSave" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                            CommandName="Save" Height="16px" ImageUrl="../../../Images/Save.gif" ToolTip="Speichern"
                                                            Width="16px" />
                                                        <asp:ImageButton ID="ibtEdit" runat="server" Height="16px" ImageUrl="../../../Images/Edit_01.gif"
                                                            Width="16px" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>'
                                                            ToolTip="Ändern" />
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Equipment" SortExpression="Equnr" Visible="False">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("EQUNR") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEqunr" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Wunschkennz1" HeaderText="col_Wunschkennz1">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Wunschkennz1" runat="server" CommandName="Sort" CommandArgument="Wunschkennz1">col_Wunschkennz1</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWunschkennz1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulkreis") %>'>
                                                        </asp:Label>-
                                                        <asp:TextBox ID="txtWunschkennz1" Width="78px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz1") %>'
                                                            Enabled='<%# (TypeOf DataBinder.Eval(Container, "DataItem.Wunschkennz1") Is System.DBNull) %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Wunschkennz2" HeaderText="col_Wunschkennz2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Wunschkennz2" runat="server" CommandName="Sort" CommandArgument="Wunschkennz2">col_Wunschkennz2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWunschkennz2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulkreis") %>'>
                                                        </asp:Label>-
                                                        <asp:TextBox ID="txtWunschkennz2" Width="78px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz2") %>'
                                                            Enabled='<%# (TypeOf DataBinder.Eval(Container, "DataItem.Wunschkennz2") Is System.DBNull) %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Wunschkennz3" HeaderText="col_Wunschkennz3">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Wunschkennz3" runat="server" CommandName="Sort" CommandArgument="Wunschkennz3">col_Wunschkennz3</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWunschkennz3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulkreis") %>'>
                                                        </asp:Label>-
                                                        <asp:TextBox ID="txtWunschkennz3" Width="78px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz3") %>'
                                                            Enabled='<%# (TypeOf DataBinder.Eval(Container, "DataItem.Wunschkennz3") Is System.DBNull) %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ResNr" HeaderText="col_ResNr">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ResNr" runat="server" CommandName="Sort" CommandArgument="ResNr">col_ResNr</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtResNr" Width="78px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResNr") %>'
                                                            Enabled='<%#  (TypeOf DataBinder.Eval(Container, "DataItem.ResNr") Is System.DBNull) %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ResName" HeaderText="col_ResName">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ResName" runat="server" CommandName="Sort" CommandArgument="ResName">col_ResName</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtResName" Width="78px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResName") %>'
                                                            Enabled='<%# (TypeOf DataBinder.Eval(Container, "DataItem.ResName") Is System.DBNull)  %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdContinue" runat="server" CssClass="Tablebutton" Width="78px"> » Weiter</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
