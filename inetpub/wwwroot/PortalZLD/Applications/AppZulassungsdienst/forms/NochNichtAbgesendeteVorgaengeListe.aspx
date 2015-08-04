<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NochNichtAbgesendeteVorgaengeListe.aspx.cs"
    Inherits="AppZulassungsdienst.forms.NochNichtAbgesendeteVorgaengeListe" MasterPageFile="../MasterPage/Big.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
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
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" visible="false" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active"  style="vertical-align:top">
                                                <asp:Label ID="lblError"  runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                        </tr>

                                    </tbody>
                                </table>
                            </div>
                            <div id="Result" runat="Server" visible="false">

                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr class="formquery">
                                            <td class="firstLeft active"  style="vertical-align:top">
                                                <asp:Label ID="lblAnzahl" Font-Bold="True" Font-Size="9pt" runat="server" ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvZuldienst" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" DataKeyNames="SapId,PositionsNr"
                                                    AllowSorting="true" AllowPaging="false" CssClass="GridView" PageSize="1000" 
                                                    onsorting="gvZuldienst_Sorting" onrowcommand="gvZuldienst_RowCommand">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <PagerSettings Visible="False" />
                                                    <Columns> 
                                                        <asp:TemplateField HeaderText="Status" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("FehlerText") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="IsSelected" HeaderText="Auswahl">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_IsSelected" runat="server" CommandName="Sort" CommandArgument="SapId">Auswahl</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIsSelected" runat="server" Text='<%# ((bool)Eval("IsSelected") ? "X" : "") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="SapId" HeaderText="ID">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_ID" runat="server" CommandName="Sort" CommandArgument="SapId">ID</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsapID" runat="server" Text='<%# Eval("SapId") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="KundenNr" HeaderText="Kundennr.">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kundennr" runat="server" CommandName="Sort" CommandArgument="KundenNr">Kundennr.</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKundennr" runat="server" Text='<%# Eval("KundenNr") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="KundenName" HeaderText="Name">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kundenname" runat="server" CommandName="Sort" CommandArgument="KundenName">Name</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKundenname" runat="server" Text='<%# Eval("KundenName") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="MaterialName" HeaderText="Dienstleistung">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Dienstleistung" runat="server" CommandName="Sort" CommandArgument="MaterialName">Dienstleistung</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDienstleistung" runat="server" Text='<%# Eval("MaterialName") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Zulassungsdatum" HeaderText="Zulassungsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Zulassungsdatum"  runat="server" CommandName="Sort" CommandArgument="Zulassungsdatum">Zulassungsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblZulassungsdatum" runat="server" Text='<%# Eval("Zulassungsdatum", "{0:d}") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Referenz1" HeaderText="Referenz1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="Referenz1">Referenz1</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReferenz1"  runat="server" Text='<%# Eval("Referenz1") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>  
                                                        <asp:TemplateField SortExpression="Referenz2" HeaderText="Referenz2">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="Referenz2">Referenz2</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReferenz2" runat="server" Text='<%# Eval("Referenz2") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKennzeichen" runat="server" Text='<%# Eval("Kennzeichen") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnOK" ImageUrl="/PortalZLD/images/haken_gruen.gif" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                                                    runat="server" CommandName="OK" ToolTip="Vorgang übernehmen" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="60px" />
                                                            <ItemStyle CssClass="TablePadding" Width="60px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        <asp:LinkButton ID="cmdOK" runat="server" CssClass="TablebuttonLarge" Width="128px" onclick="cmdOK_Click">» alle übernehmen</asp:LinkButton>
                        <asp:LinkButton ID="cmdSend" runat="server" CssClass="Tablebutton" Width="78px" onclick="cmdSend_Click" >» Absenden</asp:LinkButton>   
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
