<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeZLDListe.aspx.cs"
    Inherits="AppZulassungsdienst.forms.ChangeZLDListe" MasterPageFile="../MasterPage/Big.Master" %>

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
                                                    CellPadding="0" CellSpacing="0" GridLines="None"  DataKeyNames="ID"
                                                    AllowSorting="true" AllowPaging="false" CssClass="GridView" PageSize="1000" 
                                                    onsorting="gvZuldienst_Sorting" onrowcommand="gvZuldienst_RowCommand" 
                                                   >
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <PagerSettings Visible="False" />
                                                    <Columns>
                                                    
                                                        <asp:TemplateField Visible="false">
                                                             <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>   
                                                        <asp:TemplateField HeaderText="Status"  Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                                                                           
                                                        <asp:TemplateField HeaderText="Bearb." HeaderStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnEdit" Width="17px" Height="21" ToolTip="Bearbeiten" Visible='<%# DataBinder.Eval(Container, "DataItem.toDelete").ToString() != "X" %>'
                                                                    ImageUrl="/PortalZLD/images/Edit_01.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>' CommandName="Bearbeiten" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Löschen" HeaderStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnDel"  ToolTip="Löschen"
                                                                    ImageUrl="/PortalZLD/images/del.png"  CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' CommandName="Loeschen" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="id_sap" HeaderText="col_ID">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_ID" runat="server" CommandName="Sort" CommandArgument="id_sap">col_ID</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsapID" runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>' Text='<%# DataBinder.Eval(Container, "DataItem.id_sap") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="toDelete" HeaderText="col_LoeschKZ">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_LoeschKZ" runat="server" CommandName="Sort" CommandArgument="toDelete">col_LoeschKZ</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLoeschKZ" runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>' Text='<%# DataBinder.Eval(Container, "DataItem.PosLoesch") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="kundennr" HeaderText="col_Kundennr">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kundennr" runat="server" CommandName="Sort" CommandArgument="kundennr">col_Kundennr</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKundennr" runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>' Text='<%# DataBinder.Eval(Container, "DataItem.kundennr") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="kundenname" HeaderText="col_Kundenname">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kundenname" runat="server" CommandName="Sort" CommandArgument="kundenname">col_Kundenname</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKundenname" runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>' Text='<%# DataBinder.Eval(Container, "DataItem.kundenname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>       
														<asp:TemplateField Visible="false" HeaderText="col_id_pos">
															<ItemTemplate>
																<asp:Label ID="lblid_pos"  runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.id_pos") %>'></asp:Label>
															</ItemTemplate>
														</asp:TemplateField>                                                                                                           
                                                        <asp:TemplateField SortExpression="Matbez" HeaderText="col_Dienstleistung">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Dienstleistung" runat="server" CommandName="Sort" CommandArgument="Matbez">col_Dienstleistung</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDienstleistung" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>' runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Matbez") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Zulassungsdatum" HeaderText="col_Zulassungsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Zulassungsdatum"  runat="server" CommandName="Sort" CommandArgument="Zulassungsdatum">col_Zulassungsdatum</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblZulassungsdatum" runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>' Text='<%# DataBinder.Eval(Container, "DataItem.Zulassungsdatum", "{0:d}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Referenz1" HeaderText="col_Referenz1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="Referenz1">col_Referenz1</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReferenz1"  runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>' Text='<%# DataBinder.Eval(Container, "DataItem.Referenz1") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>  
                                                        <asp:TemplateField SortExpression="Referenz2" HeaderText="col_Referenz2">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="Referenz2">col_Referenz2</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReferenz2"  runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>' Text='<%# DataBinder.Eval(Container, "DataItem.Referenz2") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>     
                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKennzeichen" runat="server" Font-Bold='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>' Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'></asp:Label>
                                                            </ItemTemplate>
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
                      <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" Width="78px" onclick="cmdSend_Click"
                           >» Erfassen </asp:LinkButton>
                        <asp:LinkButton ID="cmdSend" runat="server" CssClass="Tablebutton" Width="78px"
                           onclick="cmdCreate_Click1" >» Absenden</asp:LinkButton>
                            
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
