<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report00s.aspx.vb" Inherits="CKG.Components.ComArchive.Report00s"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                        </div>
                        <asp:panel id="DivSearch" DefaultButton="btnEmpty" runat="server">
                        <div id="TableQuery">
                            <table id="tab1" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="2">
                                            <asp:Label ID="lblDatensatz" runat="server" CssClass="TextError"></asp:Label>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Archivtypen:
                                        </td>
                                        <td id="tdSearch" runat="server">
                                            <span>
                                                <asp:RadioButtonList ID="rblTypes" runat="server" RepeatDirection="Horizontal" AutoPostBack="True">
                                                </asp:RadioButtonList>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width:10%">
                                            Verfügbare&nbsp;Archive:
                                        </td>
                                        <td class="active" style="width:90%">
                                            <span>
                                                <asp:CheckBoxList ID="cblArchive" runat="server" RepeatDirection="Horizontal" BackColor="White">
                                                </asp:CheckBoxList>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td valign="top" bgcolor="#ffffff" colspan="2">
                                            <table id="tblSearch" cellspacing="0" cellpadding="0" border="0" runat="server">
            
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td  colspan="2">
                                            <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../images/empty.gif"
                                                    Width="1px" />
                                        </td>
                                    </tr>                                    
                                </tbody>
                            </table>
                        </div>
                        </asp:panel>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="btnSuche" runat="server" CssClass="Tablebutton" Width="78px">» Suchen </asp:LinkButton>
                            <span runat="server" id="Spani"></span>
                        </div>
                        <div id="Result" runat="Server" visible="false">
                            <div class="ExcelDiv">
                                <div align="right" class="rightPadding">
                                    &nbsp;</div>
                            </div>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <asp:GridView AutoGenerateColumns="True" BackColor="White" runat="server" ID="gvArchiv"
                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True">
                                    <PagerSettings Visible="False" />
                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                    <RowStyle CssClass="ItemStyle" />
                                    <EditRowStyle></EditRowStyle>
                                    <Columns>
                                        <asp:TemplateField Visible="False">
                                            <HeaderStyle Width="30px"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" ToolTip="Vorschau anzeigen" ImageUrl="../../Images/camera.gif"
                                                    CommandName="vorschau"></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Imagebutton2" runat="server" Visible='<%# typeof (DataBinder.Eval(Container,"DataItem.Bilder")) is System.DBNull %>'
                                                    CommandName="ansicht" ImageUrl="../../Images/iconPDF.gif" ToolTip="Dokumente vom Server laden">
                                                </asp:ImageButton>
                                                <asp:HyperLink ID="Hyperlink5" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Bilder") %>'
                                                    Visible='<%# not (typeof (DataBinder.Eval(Container,"DataItem.Bilder")) is System.DBNull) %>'
                                                    ImageUrl="../../Images/iconPDF.gif" ToolTip="Dokumente anzeigen (.PDF)" Target="_blank">HyperLink</asp:HyperLink>
                                                <asp:ImageButton ID="btnDetails" runat="server" CommandName="Details" ImageUrl="../../images/Ausrufezeichen.gif"
                                                    ToolTip="Zusatzinformationen" Width="16px" Height="16px"></asp:ImageButton>
                                                <asp:Literal ID="Literal3" runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.DOC_ID") &amp; """><font color=""#FFFFFF"">.</font></a>" %>'>
                                                </asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False" HeaderText="Seiten">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Bilder") %>'
                                                    Target="_blank" Text='<%# DataBinder.Eval(Container, "DataItem.Link") %>'>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <input id="txtShowAll" type="hidden" runat="server" />
                                <asp:literal id="Literal1" runat="server"></asp:literal>
                            </div>
                        </div>
                        <div id="dataFooter">
                        </div>
                        <table id="Table2" cellspacing="0" cellpadding="0" border="0" runat="server">
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
