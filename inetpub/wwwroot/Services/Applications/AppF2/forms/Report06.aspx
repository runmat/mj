<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report06.aspx.vb" Inherits="AppF2.Report06"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="BusyBoxDotNet" Namespace="BusyBoxDotNet" TagPrefix="busyboxdotnet" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
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
                            <div id="paginationQuery" style="width: 100%; display: block;">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" cellpadding="0" cellspacing="0">
                                    <tfoot>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    <tbody>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td nowrap="nowrap" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:LinkButton ID="btnConfirm" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                    Width="130px">» Abfrage starten</asp:LinkButton>
                                                <asp:LinkButton ID="btnZuordnen" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                    Width="130px" Visible ="false">» Zuordnen</asp:LinkButton>
                                                <asp:LinkButton ID="btnSave" runat="server" CssClass="TablebuttonLarge" Height="16px"
                                                    Width="130px" Visible="false">» &Uuml;bernehmen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="DivPlaceholder" runat="server" style="height: 550px;">
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lbCreateExcel" runat="server" Text="Excel herunterladen" ForeColor="White"></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" Width="905px">
                                                    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="gridview">
                                                        <PagerSettings Visible="false" />
                                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                        <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                        <EditRowStyle Wrap="False"></EditRowStyle>
                                                        <Columns>
                                                            <asp:TemplateField SortExpression="Erfassungsdatum" HeaderText="col_Erfassungsdatum">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Erfassungsdatum" runat="server" CommandArgument="Erfassungsdatum"
                                                                        CommandName="sort" ForeColor="White">col_Erfassungsdatum</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Erfassungsdatum","{0:d}") %>'
                                                                        ID="lblErfassungsdatum" Visible="true"> </asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="BoundField" ForeColor="White" />
                                                                <ItemStyle CssClass="BoundField" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="col_Kennzeichen" SortExpression="col_Kennzeichen">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                                        CommandName="sort" ForeColor="White">col_Kennzeichen</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblKennzeichen" runat="server" Text='<%# Bind("Kennzeichen") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="col_Fahrgestellnummer" SortExpression="col_Fahrgestellnummer">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                                        CommandName="sort" ForeColor="White">col_Fahrgestellnummer</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFahrgestellnummer" runat="server" Text='<%# Bind("Fahrgestellnummer") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="col_Briefnummer" SortExpression="col_Briefnummer">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Briefnummer" runat="server" CommandArgument="Briefnummer"
                                                                        CommandName="sort" ForeColor="White">col_Briefnummer</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBriefnummer" runat="server" Text='<%# Bind("Briefnummer") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="col_Zuordnen" SortExpression="col_Zuordnen">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Zuordnen" runat="server" CommandArgument="Zuordnen" CommandName="sort" ForeColor="White">col_Zuordnen</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkAdresseAnzeigen" runat="server" 
                                                                        Checked='<%# Bind("Zuordnen") %>'></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="col_Haendlernummer" SortExpression="col_Haendlernummer"
                                                                Visible="False">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Haendlernummer" runat="server" CommandArgument="Haendlernummer"
                                                                        CommandName="sort" ForeColor="White">col_Haendlernummer</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtHaendlernummer" runat="server" 
                                                                        Text='<%# Bind("Haendlernummer") %>' MaxLength="10" Width="80px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="col_Lizenznr" SortExpression="col_Lizenznr" Visible="False">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Lizenznr" runat="server" CommandArgument="Lizenznr" CommandName="sort" ForeColor="White">col_Lizenznr</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtLizenznr" runat="server" Text='<%# Bind("Lizenznr") %>' 
                                                                        MaxLength="20" Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="col_Finart" SortExpression="col_Finart" Visible="False">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Finart" runat="server" CommandArgument="Finart" CommandName="sort" ForeColor="White">col_Finart</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlFinart" runat="server" style="width:auto">
                                                                        <asp:ListItem Value="42" Selected="true" Text="Darlehen"></asp:ListItem>
                                                                        <asp:ListItem Value="44" Text="Direct Sales"></asp:ListItem>
                                                                        <asp:ListItem Value="40" Text="Leasing"></asp:ListItem>
                                                                    </asp:DropDownList>   
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="col_Label" SortExpression="col_Label" Visible="False">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Label" runat="server" CommandArgument="Label" CommandName="sort" ForeColor="White">col_Label</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtLabel" runat="server" Text='<%# Bind("Label") %>' 
                                                                        MaxLength="3" Width="40px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="col_Status" SortExpression="col_Status" Visible="False">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Status" runat="server" CommandArgument="Status" CommandName="sort" ForeColor="White">col_Status</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <AlternatingRowStyle BackColor="#DEE1E0" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbCreateExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                </div>
        </div>
    </div>
</asp:Content>
