<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PeisAdministration.aspx.vb"
    Inherits="Admin.PeisAdministration" MasterPageFile="MasterPage/Admin.Master" %>

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
                            <asp:Label ID="lblHead" runat="server" Text="SAPMonitoring"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="TblSearch" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                                    <tr>
                                        <td class="firstLeft active" colspan="2" style="width: 100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trSearch" runat="server">
                                        <td align="left">
                                            <table bgcolor="white" border="0">
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="70">
                                                        ab Datum:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtAbDatum" runat="server" Width="130px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtAbDatum_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtAbDatum">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td width="100%">
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" width="70">
                                                        bis Datum:
                                                    </td>
                                                    <td class="active">
                                                        <asp:TextBox ID="txtBisDatum" runat="server" Width="130px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtBisDatum_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtBisDatum">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td width="100%">
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
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» 
                                 Erstellen</asp:LinkButton>
                            </div>
                            <div id="Result">
                                <div id="Resultshow" runat="server" visible="false">
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding" id = "ExcelDiv" runat="server">
                                            &nbsp;<img src="Images/iconXLS.gif" alt="Excel herunterladen" />
                                            <span class="ExcelSpan">
                                                <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel 
                                                                herunterladen</asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                    <div id="pagination">
                                        <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                    </div>
                                    <div id="data">
                                        <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                            <tr id="trdata1" runat="server">
                                                <td>
                                                    <asp:DataGrid ID="DG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                AllowPaging="True" AllowSorting="True">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn DataField="FilterID" Visible="false"></asp:BoundColumn>
                                                    <asp:TemplateColumn  HeaderText="Filter enabled" SortExpression="FilterEnabled" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="100px" >
                                                                                                            
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkFilterEnabled" OnCheckedChanged="chkFilterEnabled_CheckedChanged"
                                                                CausesValidation="true" AutoPostBack="true" Checked='<%# DataBinder.Eval(Container, "DataItem.FilterEnabled") = "X" %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="FehlerName" HeaderText="Fehler Name" SortExpression="FehlerName">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="FehlerBeschreibung" HeaderText="Beschreibung" SortExpression="FehlerBeschreibung">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="FehlerBeispiel" HeaderText="Beispiel" SortExpression="FehlerBeispiel">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="KeyWords" HeaderText="KeyWords" SortExpression="KeyWords">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="40px" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbDelete" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FilterID") %>'
                                                                CommandName="Delete" Height="10px">
																		<img src="Images/del.png" border="0"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
