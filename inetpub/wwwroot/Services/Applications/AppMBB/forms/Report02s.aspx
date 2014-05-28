<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report02s.aspx.cs" Inherits="AppMBB.forms.Report02s"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
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
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap" colspan="2">
                                    Klicken Sie auf "Erstellen" um den Bericht zu starten.
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                    </div>
                    
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                                    OnClick="lbCreate_Click">» Erstellen</asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="/Services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel" runat="server" OnClick="lnkCreateExcel_Click">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="gvAusgabe"
                                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                                    OnSorting="gvAusgabe_Sorting" OnRowCommand="gvAusgabe_RowCommand">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="col_EingangCarportliste" SortExpression="CARPP">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_EingangCarportliste" runat="server" CommandName="Sort" CommandArgument="CARPP">col_EingangCarportliste</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("CARPP") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Kennzeichen" SortExpression="ZZKENN">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="ZZKENN">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("ZZKENN") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_AnzSchilder" SortExpression="SCHILD">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_AnzSchilder" runat="server" CommandName="Sort" CommandArgument="SCHILD">col_AnzSchilder</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSchild" runat="server" Text='<%# Bind("SCHILD") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Form1" SortExpression="SCHILD">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Form1" runat="server" CommandName="Sort" CommandArgument="SCHILD">col_Form1</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="LinkButton1" runat="server" Visible="false" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ZZKENN") %>'
                                                                    CausesValidation="False" ToolTip="Formular öffnen" ImageUrl="/services/images/document.gif" CommandName="Schilder"></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_EingangPhysisch" SortExpression="KZINDAT">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_EingangPhysisch" runat="server" CommandName="Sort" CommandArgument="KZINDAT">col_EingangPhysisch</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEingangPhysisch" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KZINDAT","{0:dd.MM.yyyy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Schein" SortExpression="SCHEIN">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Schein" runat="server" CommandName="Sort" CommandArgument="SCHEIN">col_Schein</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSchein" runat="server" Text='<%# Bind("SCHEIN") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Form2" SortExpression="SCHEIN">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Form2" runat="server" CommandName="Sort" CommandArgument="SCHEIN">col_Form2</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="LinkButton2" runat="server" Visible="false" CommandName="Schein"
                                                                    CausesValidation="False" ToolTip="Formular öffnen" ImageUrl="/services/images/document.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ZZKENN") %>'></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Brief" SortExpression="BRIEF">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Brief" runat="server" CommandName="Sort" CommandArgument="BRIEF">col_Brief</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("BRIEF") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        
                    <div id="dataFooter">
                    </div>
                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>
