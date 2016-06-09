<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preisanlage.aspx.cs" Inherits="AppZulassungsdienst.Preisanlage"   MasterPageFile="../MasterPage/Big.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>

                            <div id="TableQuery" style="margin-bottom: 10px">
                                <asp:Panel ID="Panel1" runat="server">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td colspan="3" 
                                                    style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" ForeColor="#269700" runat="server" Visible="false" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3">
                                                  
                                                  
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="false" CssClass="GridView" PageSize="1000" onrowcommand="GridView1_RowCommand">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>

                                                       <asp:TemplateField SortExpression="KUNNR" HeaderText="col_KUNNR">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_KUNNR" runat="server" CommandName="Sort" CommandArgument="KUNNR">col_KUNNR</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKUNNR" runat="server"  Text='<%# Eval("KUNNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>    
 
                                                        <asp:TemplateField SortExpression="NAME1" HeaderText="col_Kundenname">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kundenname" runat="server" CommandName="Sort" CommandArgument="NAME1">col_Kundenname</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKundenname" runat="server" Text='<%# Eval("NAME1") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField SortExpression="KONDA" HeaderText="col_Preisgruppe">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Preisgruppe" runat="server" CommandName="Sort" CommandArgument="KONDA">col_Preisgruppe</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPreisgruppe" runat="server" Text='<%# Eval("KONDA") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnproLandkr" CommandArgument='<%# Eval("KUNNR") %>' runat="server" CommandName="proLandkr" CssClass="TablebuttonXXLarge" Width="165px"
                                                                Height="16px" >» Preiserf. pro Landkreis </asp:LinkButton>                                                               
                                                            </ItemTemplate>
                                                        </asp:TemplateField>            
                                                       <asp:TemplateField>
                                                            <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnohneLandkr" CommandArgument='<%# Eval("KUNNR") %>' runat="server" CommandName="ohneLandkr" CssClass="TablebuttonXXLarge" Width="165px"
                                                                Height="16px">» Preiserf. ohne Landkreis </asp:LinkButton>                                                                 </ItemTemplate>
                                                        </asp:TemplateField>  
                                                       <asp:TemplateField>
                                                            <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnOK" CommandArgument='<%# Eval("KUNNR") %>' runat="server" CommandName="OK" CssClass="TablebuttonSmall" Width="50px"
                                                               >OK</asp:LinkButton>                                                                 </ItemTemplate>
                                                        </asp:TemplateField>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
                                                    </Columns>
                                                </asp:GridView>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </asp:Panel>
                            </div>
                            <div id="dataQueryFooter" runat="server" class="dataQueryFooter">
                            </div>

                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
