<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01.aspx.vb" Inherits="CKG.Components.ComCommon.Report01" 
 MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%; border-bottom-color: #dfdfdf; border-bottom-width: 1px;
                    border-bottom-style: solid; color: #595959; border-left: 1px solid #DFDFDF; border-right: 1px solid #DFDFDF;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Fahrzeughistorie"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div>
                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                            </div>
                            
                            <div style="background-color: #dfdfdf;height:20px">
                                <div style="float:left;padding-left:15px;padding-top:3px">
                                    <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                                </div>
                                <div style="float:right;padding-right:10px;padding-top:3px">
                                    <asp:ImageButton ID="ibtNewSearch" runat="server" 
                                        ImageUrl="../../../Images/queryArrow.gif" Visible="False" />
                                </div>
                                
                            </div>
                            
                            
                            <asp:Panel ID="Panel2" DefaultButton="lbCreate" runat="server">
                            <div id="divSelection" runat="server">
                                <div id="SelectFields" style="width: 350px; margin-top: 8px; margin-bottom: 8px;
                                    padding-left: 15px">
                                    <asp:Label ID="lbl_AmtlKennzeichen" runat="server" Width="130px"> lbl_AmtlKennzeichen</asp:Label>
                                    <asp:TextBox ID="txtAmtlKennzeichen" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                    <div style="padding-top: 10px">
                                        <asp:Label ID="lbl_Fahrgestellnummer" runat="server" Width="130px">lbl_Fahrgestellnummer</asp:Label>
                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                    </div>
                                    <div style="padding-top: 10px">
                                        <asp:Label ID="lbl_Briefnummer" runat="server" Width="130px">lbl_Briefnummer</asp:Label>
                                        <asp:TextBox ID="txtBriefnummer" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                    </div>
                                    <div style="padding-top: 10px">
                                        <asp:Label ID="lbl_Leasingvertragsnr" runat="server" Width="130px">lbl_Leasingvertragsnr</asp:Label>
                                        <asp:TextBox ID="txtOrdernummer" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                                <div style="float: right; padding: 10px 10px 10px">
                                    <asp:LinkButton ID="lbCreate" runat="server" Text="» Suchen" CssClass="Tablebutton"
                                        Visible="true" Width="78px" Height="16px"></asp:LinkButton>
                                </div>
                            </div>
                            </asp:Panel>
                            <div id="divSpace" runat="server" style="height:20px">&nbsp;</div>
                                &nbsp;
                            <div id="Result" runat="Server" visible="false">
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal">
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" Width="1050px"
                                            ID="gvSelectOne" CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True"
                                            AllowSorting="True">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle Wrap="false" CssClass="ItemStyle" />
                                            <EditRowStyle Wrap="False"></EditRowStyle>
                                            <Columns>
                                                <asp:BoundField Visible="false" HeaderText="EQUNR" DataField="EQUNR" ReadOnly="true" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                            Width="70px" CssClass="Tablebutton" Style="padding-top: 4px; background-image: url(../../../Images/button.jpg);
                                                            vertical-align: middle; text-align: center; font-size: 10px; font-weight: bold;
                                                            color: #333333; width: 78px; height: 16px" CommandName="weiter" Text="Weiter&amp;nbsp;&amp;#187; "
                                                            ID="lbWeiter"></asp:LinkButton>
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
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen" ItemStyle-Wrap="False">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                            CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                            ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="False" />
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
                                    </asp:Panel>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
