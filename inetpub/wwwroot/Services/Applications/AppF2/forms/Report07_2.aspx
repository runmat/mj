<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report07_2.aspx.vb" Inherits="AppF2.Report07_2"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkKreditlimit" runat="server" NavigateUrl="Report07.aspx">Suche</asp:HyperLink>
                    <a class="active">| Ergebnisse</a>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
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
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                            </Triggers>                        
                            <ContentTemplate>
                                <div id="Result" runat="Server">
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding">
                                            <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                            <span class="ExcelSpan">
                                                <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
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
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEQUNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <p align="center">
                                                            <asp:ImageButton ID="lbSave" runat="server" ToolTip="Speichern" CommandArgument='<%# Container.DataItemIndex %>'
                                                                CommandName="Update" ImageUrl="../../../Images/Save.gif" Height="16px" Width="16px" />
                                                        </p>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <p align="center">
                                                            <asp:ImageButton ID="lbEdit" runat="server" ToolTip="Ändern" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                                CommandName="Edit" ImageUrl="../../../Images/Edit_01.gif" Height="16px" Width="16px" />
                                                        </p>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField ReadOnly="true" DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer"
                                                    HeaderText="Fahrgestellnummer"></asp:BoundField>                                                
                                                <asp:BoundField ReadOnly="true" DataField="Kennzeichen" SortExpression="Kennzeichen"
                                                    HeaderText="Kennzeichen"></asp:BoundField>
                                                <asp:BoundField ReadOnly="true" DataField="Versanddatum" SortExpression="Versanddatum"
                                                    HeaderText="Versanddatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundField>
                                                <asp:BoundField ReadOnly="true" DataField="Versandadresse" SortExpression="Versandadresse"
                                                    HeaderText="Versandadresse" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                </asp:BoundField>
                                                <asp:BoundField ReadOnly="true" DataField="Distrikt" SortExpression="Distrikt" HeaderText="Distrikt">
                                                </asp:BoundField>
                                                <asp:BoundField ReadOnly="true" DataField="geändert von" SortExpression="geändert von"
                                                   HeaderText="geändert von"></asp:BoundField>
                                                <asp:BoundField ReadOnly="true"   DataFormatString="{0:dd.MM.yyyy}" DataField="geändert am" SortExpression="geändert am" HeaderText="geändert am">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Empfaenger" SortExpression="Empfaenger" HeaderText="Empfänger">
                                                </asp:BoundField> 
                                                <asp:TemplateField HeaderText="Memo 1" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="#FFFFFF">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFleet20" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fleet20") %>'>
                                                        </asp:Label>
                                                        <asp:TextBox ID="txtFleet20" Text='<%# DataBinder.Eval(Container, "DataItem.Fleet20") %>'
                                                            Visible="false" MaxLength="27" runat="server" Width="100px" Font-Size="8pt" Font-Names="Verdana,sans-serif;">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Memo 2" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="#FFFFFF">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFleet21" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fleet21") %>'>
                                                        </asp:Label>
                                                        <asp:TextBox ID="txtFleet21" Text='<%# DataBinder.Eval(Container, "DataItem.Fleet21") %>'
                                                            Visible="false" runat="server" MaxLength="27" Width="100px"  Font-Size="8pt" Font-Names="Verdana,sans-serif;">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            &nbsp;</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
