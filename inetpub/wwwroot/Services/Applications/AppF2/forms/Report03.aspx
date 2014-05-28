<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report03.aspx.vb" Inherits="AppF2.Report03"
    MasterPageFile="../MasterPage/AppMaster.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                            <asp:PostBackTrigger ControlID="lnkCreatePDF1" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                               <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr  class="formquery" >
                                        <td class="firstLeft active" colspan="2"  style="width:100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblDateVon" runat="server" Text="Von:"></asp:Label>
                                        </td>
                                        <td class="active" style="width:100%">
                                            <asp:TextBox ID="txtDateVon" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtDateVon_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtDateVon" >
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblDateBis" runat="server" Text="Bis:"></asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtDateBis" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtDateBis_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtDateBis">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" colspan="2"  style="width:100%">
                                             &nbsp;</td>
                                     </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                             <div id="dataQueryFooter" >
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                            </div>   
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconPDF.gif" alt="PDF herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreatePDF1" ForeColor="White" runat="server">PDF herunterladen</asp:LinkButton>
                                        </span>
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
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
                                                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="GridView">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                                    <Columns>
                                                        <asp:BoundField Visible="false" HeaderText="LIZNR" DataField="LIZNR" ReadOnly="true" />
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
                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                                    CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                                    ID="lblKennzeichen" Visible="true"> </asp:Label>
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
                                                        <asp:TemplateField SortExpression="Erfassungsdatum" HeaderText="col_Erfassungsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Erfassungsdatum" runat="server" CommandArgument="Erfassungsdatum"
                                                                    CommandName="sort">col_Erfassungsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Erfassungsdatum","{0:d}") %>'
                                                                    ID="lblErfassungsdatum" Visible="true"> </asp:Label>
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
                </div>
            </div>
        </div>
    </div>
</asp:Content>
