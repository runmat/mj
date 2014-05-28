<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StatusMonitor.aspx.vb"
    Inherits="Admin.StatusMonitor" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Statusmonitor"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <table id="TblSearch" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                            <tr>
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300"<a href="ShowBapis.aspx">ShowBapis.aspx</a> textcolor="red" Visible="true"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trSearch" runat="server">
                                <td align="left">
                                    <table bgcolor="white" border="0">
                                        <tr class="formquery">
                                            <td class="firstLeft active" width="70px">
                                                Kunde/Abteilung hinzufügen:
                                            </td>
                                            <td class="active" width="130px">
                                                <asp:TextBox ID="txtKundenname" runat="server" Width="130px"></asp:TextBox>
                                            </td>
                                            <td class="active" width="100%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbhinzufuegen" runat="server" CssClass="Tablebutton" Width="78px">Hinzufügen</asp:LinkButton>
                    </div>
                    <div id="Result">
                        <div id="Resultshow" runat="server">
                            <div class="ExcelDiv">
                                &nbsp;
                            </div>
                            <div id="data">
                                <table cellspacing="0" cellpadding="0" bgcolor="white" border="1px" style="border-style: solid;">
                                    <tr>
                                        <td>
                                            <asp:GridView AutoGenerateColumns="False" AllowSorting="true" runat="server" Width="100%"
                                                ID="gv" CssClass="GridView" BorderWidth="0" BorderStyle="None">
                                                <AlternatingRowStyle VerticalAlign="Middle" HorizontalAlign="Center" Font-Bold="true"
                                                    Font-Size="small" BackColor="WhiteSmoke" />
                                                <RowStyle Height="60px" Width="120px" BorderWidth="0" BorderStyle="none" VerticalAlign="Middle"
                                                    HorizontalAlign="Center" Font-Bold="true" Font-Size="small" />
                                                <HeaderStyle Font-Size="small" Font-Bold="true" HorizontalAlign="Center" VerticalAlign="Middle"
                                                    BackColor="#dfdfdf" ForeColor="#2b4c91" />
                                                <Columns>
                                                    <asp:BoundField Visible="false" HeaderText="Datensatz ID" DataField="id" ReadOnly="true" />
                                                    <asp:TemplateField SortExpression="Abteilung">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="Abteilung" CommandName="sort"
                                                                Text="Kunde/Abteilung"></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abteilung") %>'
                                                                ID="lblAnzeigeKundenName" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label runat="server" Text="Berechtigungsreferenz" ID="lblAnzeigeReferenz"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtBerechtigungReferenz" Text='<%# DataBinder.Eval(Container, "DataItem.Benutzerreferenz") %>'></asp:TextBox>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>'
                                                                CommandName="saveReferenz" Text="speichern" ID="lbSaveReferenz" CssClass="Tablebutton"
                                                                Width="78px" Height="16px"> </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label runat="server" Text="Pfad zum Logo" ID="lblAnzeigeTextLogo"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtLogoPfad" Text='<%# DataBinder.Eval(Container, "DataItem.Logo") %>'></asp:TextBox>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>'
                                                                CommandName="save" Text="speichern" ID="lbSave" CssClass="Tablebutton" Width="78px"
                                                                Height="16px"> </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label runat="server" Text="Seite" ID="lblKunde"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DropDownList runat="server" ID="ddlPlatzierung" AutoPostBack="true" AppendDataBoundItems="false">
                                                                <asp:ListItem Text="Rechts" Value="R"></asp:ListItem>
                                                                <asp:ListItem Text="Links" Value="L"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>
