<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Bapi2Report.aspx.vb" Inherits="Admin.Bapi2Report"
    MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="~/PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="GroupedSelect" Namespace="Admin.CKG_Adapter" Assembly="Admin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;
            </div>
            <div id="innerContent">
                <asp:Label ID="lblError" runat="server" CssClass="TextError" Style="max-width: 900px"></asp:Label>
                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Reportname"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Kunde:
                                            </td>
                                            <td nowrap="nowrap" valign="top" class="firstLeft active">
                                                <asp:DropDownList ID="ddlFilterCustomer" runat="server" Font-Names="Verdana,sans-serif">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCustomer" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                BAPI:
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox ID="txtBapiName" runat="server" CssClass="InputTextbox">*</asp:TextBox>
                                                <asp:Label ID="lblBapiName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Freundlicher Name:
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox ID="txtFriendName" runat="server" CssClass="InputTextbox">*</asp:TextBox>
                                                <asp:Label ID="lblFriendName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Technischer Name:
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox ID="txtTechName" runat="server" CssClass="InputTextbox">*</asp:TextBox>
                                                <asp:Label ID="lblTechName" runat="server" Visible="False" Width="160px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                im Zeitraum von:
                                            </td>
                                            <td valign="top" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox CssClass="InputTextbox" runat="server" Width="83px" ToolTip="Datum von"
                                                    ID="txtDateVon" MaxLength="10" />
                                                <ajaxToolkit:CalendarExtender ID="defaultCalendarExtenderVon" runat="server" TargetControlID="txtDateVon" />
                                                &nbsp;bis:&nbsp;
                                                <asp:TextBox CssClass="InputTextbox" runat="server" Width="83px" ToolTip="Datum bis"
                                                    ID="txtDateBis" MaxLength="10" />
                                                <ajaxToolkit:CalendarExtender ID="defaultCalendarExtenderBis" runat="server" TargetControlID="txtDateBis" />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="btnSuche" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False" ForeColor="White">Excel herunterladen</asp:LinkButton>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanelNavi" runat="server">
                            <ContentTemplate>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server">
                                    </uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvResult" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="xxGridViewxx" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Anwendung">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="Hidden_ApplicationID" runat="server" Value='<%# Eval("ApplicationID") %>' />
                                                                <asp:TextBox ID="Hidden_New_ApplicationID" runat="server" Value='<%# Eval("ApplicationID") %>'
                                                                    Style="display: none" />
                                                                <GroupedSelect:GroupedDropDownListAdapter runat="server" ID="Grid_DDL_ApplicationID">
                                                                </GroupedSelect:GroupedDropDownListAdapter>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Bapi">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="Hidden_BapiID" runat="server" Value='<%# Eval("BapiID") %>' />
                                                                <asp:TextBox ID="Hidden_New_BapiID" runat="server" Value='<%# Eval("BapiID") %>'
                                                                    Style="display: none" />
                                                                <GroupedSelect:GroupedDropDownListAdapter ID="DDL_BapiID" runat="server">
                                                                </GroupedSelect:GroupedDropDownListAdapter>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="LastUsed" SortExpression="LastUsed" HeaderText="Letzte Nutzung" />
                                                        <asp:TemplateField HeaderText="col_Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Delete" runat="server">Delete</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Image src="../Images/Papierkorb_01.gif" alt="Löschen" Width="16px" Height="16px"
                                                                    border="0" runat="server" ID="IMG_delete" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                                </asp:GridView>
                                                <div id="Div1" style="text-align: right">
                                                    <br />
                                                    <asp:LinkButton ID="gvResult_Speichern" runat="server" CssClass="Tablebutton" Width="78px">Speichern</asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div id="dataFooter">
                    &nbsp;</div>
                <div>
                    <h3>
                        Neu</h3>
                    <h4>
                        Anwendung</h4>
                    <asp:TextBox runat="server" ID="FilterNew" Visible="false"></asp:TextBox>
                    <h4 id="FilterHeader">
                        Suche</h4>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="FilterUL" Visible="true"></asp:TextBox><br />
                            </td>
                            <td>
                                <asp:Label ID="FilterULError" runat="server" CssClass="TextError" Style="max-width: 900px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <GroupedSelect:GroupedDropDownListAdapter runat="server" ID="DDL_ApplicationID" Width="600">
                    </GroupedSelect:GroupedDropDownListAdapter>
                    <ul runat="server" id="UL_ApplicationID" style="display: none">
                    </ul>
                    <h4>
                        Bapi</h4>
                    <GroupedSelect:GroupedDropDownListAdapter ID="DDL_BapiID" runat="server" Width="600">
                    </GroupedSelect:GroupedDropDownListAdapter>
                    <asp:ImageButton ImageAlign="Middle" ID="imgbInsert" runat="server" Height="20px"
                        ImageUrl="../Images/Save.gif" AlternateText="Hinzufügen..." Visible="True" Width="20px" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
