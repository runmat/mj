<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change05_2.aspx.vb" Inherits="AppF2.Change05_2"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbHaendlersuche" runat="server">Händlerauswahl</asp:LinkButton>
                &nbsp;</div>
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
                            <div id="Result" runat="Server">
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data" style="overflow-x: scroll;">

                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" Width="1040px"
                                            ID="GridView1" CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True"
                                            AllowSorting="True">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle  CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle  CssClass="ItemStyle" />
                                            <EditRowStyle ></EditRowStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtStornoFreigabe" runat="server" Height="20px" ImageUrl="../../../Images/Papierkorb_01.gif"
                                                        OnClick="Button1_Click" Width="20px" CommandName="Edit" 
                                                        CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Stornieren" />
                                                    <asp:ImageButton ID="ibtFreigabe" runat="server" 
                                                        CommandArgument="<%# Container.DataItemIndex %>" CommandName="Freigabe" 
                                                        Height="20px" ImageUrl="../../../Images/Edit_01.gif" 
                                                        ToolTip="Freigeben" Width="20px" />
                                                </ItemTemplate>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="H&#228;ndlernummer">
                                                <HeaderTemplate  >
                                                    <asp:LinkButton ID="col_Haendlernummer" CommandArgument="Haendlernummer" CommandName="Sort"
                                                        runat="server" >col_Haendlernummer</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHaendlernummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Haendlernummer") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Name" HeaderText="col_Name">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Name" CommandArgument="Name" CommandName="Sort"
                                                        runat="server" >col_Name</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="AnforderungsUser" HeaderText="col_AnforderungsUser"
                                                Visible="False">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_AnforderungsUser" CommandArgument="AnforderungsUser" CommandName="Sort"
                                                        runat="server" >col_AnforderungsUser</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AnforderungsUser") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ZB2Nummer" SortExpression="ZB2Nummer" ReadOnly="True"
                                                HeaderText="Nummer ZBII" ></asp:BoundField>
                                            <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Vertragsnummer" CommandArgument="Vertragsnummer" CommandName="Sort"
                                                        runat="server" >col_Vertragsnummer</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fahrgestellnummer" 
                                                SortExpression="Fahrgestellnummer">
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblFahrgestellnummer" runat="server" Text='<%# Eval("Fahrgestellnummer") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFahrgestellnummer" runat="server" Text='<%# Bind("Fahrgestellnummer") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Anforderungsdatum" SortExpression="Anforderungsdatum"
                                                ReadOnly="True" HeaderText="angefordert am" DataFormatString="{0:dd.MM.yyyy}">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Kontingentart" SortExpression="Kontingentart" ReadOnly="True"
                                                HeaderText="Kontingentart"></asp:BoundField>
                                            <asp:BoundField DataField="Abrufgrund" SortExpression="Abrufgrund" ReadOnly="True"
                                                HeaderText="Abrufgrund"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Storno Grund">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.StornoGrund") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtStornoGrund" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.StornoGrund")%>'
                                                        BorderStyle="Solid" BorderWidth="1" BorderColor="Red" TextMode="MultiLine" MaxLength="120"
                                                        Rows="4">
                                                    </asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField  Visible="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="StandardButtonSmall" ID="lbStorno" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR")%>'
                                                        runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.versandadresse")%>'
                                                        CommandName="Storno">Storno</asp:LinkButton>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField  Visible="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="StandardButtonSmall" ID="lbFreigabe" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR")%>'
                                                        ToolTip='<%# DataBinder.Eval(Container, "DataItem.versandadresse")%>' CommandName="Freigabe">Freigabe</asp:LinkButton>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="StandardButtonSmall" ID="lbAuthorisierung" runat="server"
                                                        CommandName="Autho">Anfrage autorisieren</asp:LinkButton>
                                                </ItemTemplate>
  
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="StandardButtonSmall" ID="lbLoeschen" runat="server" CommandName="Loesch">Anfrage löschen</asp:LinkButton>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="VBELN" Visible="False">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("VBELN") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVbeln" runat="server" Text='<%# Bind("VBELN") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EQUNR" Visible="False">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("EQUNR") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEquinr" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            
                            <div>

                            </div>
                            
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                                <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                                <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                                    PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnOK"
                                    X="450" Y="200">
                                </ajaxToolkit:ModalPopupExtender>
           
                                <asp:Panel ID="mb" runat="server" Width="300px" Height="200px" 
                                    BackColor="White">
                                    <div style="padding-left:10px;padding-top:15px;margin-bottom:10px;">
                                        <asp:Label ID="Label1" runat="server" Text="Storno-Text:" Font-Bold="True"></asp:Label>
                                    </div>
                                    <div style="padding-left:10px;padding-top:15px;margin-bottom:10px;">
                                    <asp:TextBox ID="txtStornotext" runat="server" Height="70px" 
                                        TextMode="MultiLine" Width="250px"></asp:TextBox>
                                        </div>
                                            <table>
                                                <tr>
                                                    <td style="padding-left:10px">
                                                    <asp:Button ID="btnCancel" runat="server" Text="Senden" CssClass="TablebuttonLarge"
                                                    Font-Bold="true" Width="80px" />
                                                    
                                                    </td>
                                                    <td>
                                                    <asp:Button ID="btnOK" runat="server" Text="Abbrechen" CssClass="TablebuttonLarge" 
                                                Font-Bold="True" Width="80px" />
                                                    </td>
                                                </tr>
                                            </table>
                                    
                                </asp:Panel>                    
                    <div id="dataFooter">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
