<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report04.aspx.vb" Inherits="CKG.Components.ComArchive.Report04" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../PageElements/Kopfdaten.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td colspan="1">
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" height="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Abfragekriterien)</asp:Label><asp:HyperLink
                                ID="lnkKreditlimit" runat="server" CssClass="PageNavigation" NavigateUrl="Equipment.aspx"
                                Visible="False">Abfragekriterien</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;<asp:Label ID="lblDatensatz" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="lblNoData" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelExtraLarge">
                                        <table id="Table9" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td valign="top" align="left" colspan="2">
                                                    <table id="Table10" cellspacing="0" cellpadding="0" width="100%" bgcolor="white"
                                                        border="0" runat="server">
                                                        <tbody>
                                                            <tr>
                                                                <td valign="top" align="left" width="100%" bgcolor="#ffffff">
                                                                    <table class="TableBackGround" id="Table4" bordercolor="#cccccc" cellspacing="1"
                                                                        cellpadding="1" width="100%" bgcolor="#ffffff" border="0">
                                                                        <tr>
                                                                            <td bgcolor="#ffffff" rowspan="1">
                                                                                <strong>Archivtypen&nbsp;</strong>
                                                                            </td>
                                                                            <td id="tdSearch" bgcolor="#ffffff" runat="server">
                                                                                <asp:RadioButtonList ID="rblTypes" runat="server" RepeatDirection="Horizontal" AutoPostBack="True">
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td bgcolor="#ffffff">
                                                                                <strong>Verfügbare&nbsp;Archive</strong>
                                                                            </td>
                                                                            <td bgcolor="#ffffff">
                                                                                <asp:CheckBoxList ID="cblArchive" runat="server" RepeatDirection="Horizontal" BackColor="White">
                                                                                </asp:CheckBoxList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td bgcolor="#ffffff">
                                                                                <asp:LinkButton ID="btnSuche" runat="server" CssClass="StandardButtonTable" Width="100%">&#149&nbsp;Suchen</asp:LinkButton>
                                                                            </td>
                                                                            <td bgcolor="#ffffff">
                                                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="GridTableHead" align="left">
                                                                                &nbsp;<strong>Suchfelder</strong>
                                                                            </td>
                                                                            <td class="GridTableHead">
                                                                                <strong>Trefferliste</strong>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" bgcolor="#ffffff" colspan="1" rowspan="1">
                                                                                <table id="tblSearch" cellspacing="0" cellpadding="0" border="0" runat="server">
                                                                                    <tr id="tRow" runat="server">
                                                                                        <td id="tCell" runat="server">
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td valign="top" width="100%" bgcolor="#ffffff">
                                                                                <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" PageSize="50" BackColor="White"
                                                                                    headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain" bodyHeight="400"
                                                                                    AllowSorting="True">
                                                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                                                    <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                                                    <Columns>
                                                                                        <asp:TemplateColumn Visible="False">
                                                                                            <HeaderStyle Width="30px"></HeaderStyle>
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="ImageButton1" runat="server" ToolTip="Vorschau anzeigen" ImageUrl="../Images/camera.gif"
                                                                                                    CommandName="vorschau"></asp:ImageButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:TemplateColumn>
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="Imagebutton2" runat="server" Visible='<%# typeof (DataBinder.Eval(Container,"DataItem.Bilder")) is System.DBNull %>'
                                                                                                    CommandName="ansicht" ImageUrl="/Portal/Images/PDF_grey.gif" ToolTip="Dokumente vom Server laden">
                                                                                                </asp:ImageButton>
                                                                                                <asp:HyperLink ID="Hyperlink5" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Bilder") %>'
                                                                                                    Visible='<%# not (typeof (DataBinder.Eval(Container,"DataItem.Bilder")) is System.DBNull) %>'
                                                                                                    ImageUrl="/Portal/images/pdf.gif" ToolTip="Dokumente anzeigen (.PDF)" Target="_blank">HyperLink</asp:HyperLink>
                                                                                                <asp:ImageButton ID="btnDetails" runat="server" CommandName="Details" ImageUrl="/Portal/Images/ausruf.gif"
                                                                                                    ToolTip="Zusatzinformationen"></asp:ImageButton>
                                                                                                <asp:Literal ID="Literal3" runat="server" Text='<%# "<a name=""" &amp; DataBinder.Eval(Container, "DataItem.DOC_ID") &amp; """><font color=""#FFFFFF"">.</font></a>" %>'>
                                                                                                </asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:TemplateColumn Visible="False" HeaderText="Seiten">
                                                                                            <ItemTemplate>
                                                                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Bilder") %>'
                                                                                                    Target="_blank" Text='<%# DataBinder.Eval(Container, "DataItem.Link") %>'>
                                                                                                </asp:HyperLink>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                    </Columns>
                                                                                    <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                                                        HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                                                                </asp:DataGrid>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <input id="txtShowAll" type="hidden" runat="server">
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </table>
                                        <font face="Arial" size="1">
                                            <table id="Table2" cellspacing="0" cellpadding="0" border="0" runat="server">
                                                <tr>
                                                    <td nowrap>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br>
                                        </font>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
