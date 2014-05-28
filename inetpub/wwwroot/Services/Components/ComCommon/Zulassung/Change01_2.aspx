<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_2.aspx.vb" Inherits="CKG.Components.ComCommon.Zulassung.Change01_2"  MasterPageFile="../../../MasterPage/Services.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change01.aspx">Fahrzeugsuche</asp:HyperLink>
                    <a class="active">| Fahrzeugauswahl</a>
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
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="ShowScript" runat="server" class="formquery">
                                        <td class="active" width="100%">
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
                                        <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
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
                                                <asp:TemplateField HeaderText="Equipment" SortExpression="Equnr" Visible="False">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("EQUNR") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEqunr" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="MANDT" HeaderText="col_Auswahl">
                                                    <HeaderTemplate>
                                                        <asp:ImageButton ID="ibtAuswahl" runat="server" Height="12px" 
                                                            ImageUrl="/services/images/haken_gruen.gif" onclick="ibtAuswahl_Click" 
                                                            Width="12px" ToolTip="Alle Dokumente auswählen" style="padding-left:4px" />
                                                        <asp:ImageButton ID="ibtnAbwahl" runat="server" Height="12px" 
                                                            ImageUrl="/services/images/del.png" onclick="ibtnAbwahl_Click" 
                                                            Width="12px" ToolTip="Alle Dokumente abwählen" style="padding-left:4px" />                                                            
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk0000" Checked='<%# DataBinder.Eval(Container, "DataItem.Auswahl") = "99" %>' runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Bem")="" %>' ></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderStyle Width="125px" />
                                                    <ItemStyle Width="125px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="TIDNR" HeaderText="col_NummerZB2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">NummerZB2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZHERST_TEXT" HeaderText="col_Hersteller">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="ZZHERST_TEXT">Hersteller</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHERST_TEXT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZKLARTEXT_TYP" HeaderText="col_Typ">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="ZZKLARTEXT_TYP">Typ</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKLARTEXT_TYP") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>          
                                                <asp:TemplateField SortExpression="ZZHANDELSNAME" HeaderText="col_Handelsname">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Handelsname" runat="server" CommandName="Sort" CommandArgument="ZZHANDELSNAME">Handelsname</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHANDELSNAME") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>   
                                                <asp:TemplateField SortExpression="LIZNR" HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                                                                                                           
                                                <asp:TemplateField SortExpression="STATUS" HeaderText="col_Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="STATUS">Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bem") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                   <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                                    <ajaxToolkit:modalpopupextender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel"
                                        >
                                    </ajaxToolkit:modalpopupextender>
                                    <asp:Panel ID="mb" runat="server" Width="240" Height="100px" BackColor="White" style="display:none;
                                    border: solid 2px #bc2b2b">
                                        <div style="padding-left: 20px; padding-top: 20px; margin-bottom: 10px;">
                                            <asp:Label ID="lblMessagePopUp" runat="server" 
                                                Font-Bold="True" CssClass="TextError"></asp:Label></div>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 100px; padding-left: 20px;">
                                                    <asp:Button ID="btnOK" runat="server" Text="Weiter" CssClass="TablebuttonLarge" Font-Bold="True"
                                                        Width="90px" />
                                                </td>
                                                <td style="width: 100px; padding-right: 20px">

                                                    <asp:Button ID="btnCancel2" runat="server" Text="Abbrechen" CssClass="TablebuttonLarge"
                                                        Font-Bold="True" Width="90px" />

                                                </td>
                                            </tr>
                                        </table>
                                                <asp:Button ID="btnCancel" Style="visibility: hidden" runat="server" Text="Abbrechen"
                                                        CssClass="Tablebutton" Font-Bold="True" Width="90px" />
                                    </asp:Panel>                                  
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdContinue" runat="server" CssClass="Tablebutton" 
                                Width="78px"> » Weiter</asp:LinkButton>
                        </div>
                      
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
