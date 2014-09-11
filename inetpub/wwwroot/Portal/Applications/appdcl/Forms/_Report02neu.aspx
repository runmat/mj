<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="_Report02neu.aspx.vb" Inherits="AppDCL._Report02neu" %>

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
    <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
    <table width="100%" align="center">
        <tr>
            <td height="25">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2" height="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> :Aufträge bearbeiten</asp:Label><asp:HyperLink
                                ID="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Equipment.aspx"
                                CssClass="PageNavigation">Abfragekriterien</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="StandardTableButtonFrame" valign="top">
                        </td>
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
                                                                            <td class="TableBackGround" id="td01" align="left" bgcolor="#ffffff" runat="server">
                                                                                <strong>Aufträge</strong>
                                                                            </td>
                                                                            <td class="TableBackGround" align="center" bgcolor="#ffffff">
                                                                                <strong>Dateien (Web - Verzeichnis)</strong>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td id="td03" valign="top" bgcolor="#ffffff" runat="server" align="left">
                                                                                <asp:ListBox ID="lbxAuftrag" runat="server" Height="380px" CssClass="DropDownStyle" AutoPostBack="true">
                                                                                </asp:ListBox>
                                                                            </td>
                                                                            <td valign="top" width="100%" bgcolor="#ffffff">
                                                                                <asp:DataGrid ID="gridServer" runat="server" Width="100%" BackColor="White" AutoGenerateColumns="False"
                                                                                    AllowSorting="True" headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain"
                                                                                    bodyHeight="350" BorderColor="Transparent">
                                                                                    <SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
                                                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                                                    <HeaderStyle HorizontalAlign="Left" CssClass="GridTableHead" VerticalAlign="Top">
                                                                                    </HeaderStyle>
                                                                                    <Columns>
                                                                                        <asp:BoundColumn Visible="False" DataField="Serverpfad" SortExpression="Serverpfad"
                                                                                            HeaderText="Serverpfad"></asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="Zeit" SortExpression="Zeit" HeaderText="Erstellt am">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:TemplateColumn Visible="False" SortExpression="Filename" HeaderText="Dateiname">
                                                                                            <ItemTemplate>
                                                                                                <asp:HyperLink ID="lnkFile" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") &amp; DataBinder.Eval(Container, "DataItem.Filename") %>'
                                                                                                    BorderColor="Transparent" Target="_blank" Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>'>
                                                                                                </asp:HyperLink>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:TemplateColumn HeaderText="Vorschau (75x75)">
                                                                                            <ItemTemplate>
                                                                                                <table id="Table14" cellspacing="1" cellpadding="1" border="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:HyperLink ID="Hyperlink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") &amp; DataBinder.Eval(Container, "DataItem.Filename") %>'
                                                                                                                ToolTip="Bild in Originalgröße anzeigen" Target="_blank" >
                                                                                                                <asp:Image ID="Image2" runat="server" Width="75px" Height="75px" BorderColor="Black"
                                                                                                                    ToolTip='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' ImageUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") &amp; DataBinder.Eval(Container, "DataItem.Filename") %>'
                                                                                                                    BorderStyle="Solid" BorderWidth="1px"></asp:Image>
                                                                                                            </asp:HyperLink>
                                                                                                        </td>
                                                                                                        <%--<td valign="top">
                                                                                                            <asp:HyperLink ID="Hyperlink2" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") &amp; DataBinder.Eval(Container, "DataItem.Filename") %>'
                                                                                                                ToolTip="Bild in Originalgröße anzeigen" ImageUrl="/Portal/Images/lupe.gif" Target="_blank">HyperLink</asp:HyperLink>
                                                                                                        </td>--%>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="Filename" SortExpression="Filename" HeaderText="Dateiname">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:TemplateColumn HeaderText="Archivieren">
                                                                                            <ItemTemplate>
                                                                                                <table id="Table11" cellspacing="1" cellpadding="1" border="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <table id="Table16" cellspacing="1" cellpadding="1" border="0">
                                                                                                                <tr>
                                                                                                                    <td class="">
                                                                                                                        <p>
                                                                                                                            <asp:CheckBox ID="cbxArchiv" runat="server" ToolTip="Dokument zum Archivieren markieren"
                                                                                                                                Enabled="False"></asp:CheckBox></p>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:TemplateColumn HeaderText="Verschieben">
                                                                                            <ItemTemplate>
                                                                                                <table id="tblMove" cellspacing="1" cellpadding="1" border="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="Label4" runat="server" ToolTip="Quellauftrag (aktuell)" Enabled="False">&nbsp;Von:</asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="Label5" runat="server" Enabled="False" Text='<%# DataBinder.Eval(Container, "DataItem.Auftrag") &amp; "." &amp; DataBinder.Eval(Container, "DataItem.Tour") %>'>
                                                                                                            </asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <table id="Table18" cellspacing="1" cellpadding="1" border="0">
                                                                                                                <tr>
                                                                                                                    <td class="">
                                                                                                                        <asp:CheckBox ID="cbxMove" runat="server" ToolTip="Dokument zum Verschieben markieren">
                                                                                                                        </asp:CheckBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="Label6" runat="server" ToolTip="Ziel-Auftrag" Enabled="False">&nbsp;Nach:</asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="ddlAuftrag" runat="server" Width="100px" DataTextField="value"
                                                                                                                DataValueField="id">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:BoundColumn DataField="Status" HeaderText="Status">
                                                                                            <ItemStyle Font-Size="XX-Small"></ItemStyle>
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="FilenameNew" SortExpression="FilenameNew"
                                                                                            HeaderText="DateinameNeu"></asp:BoundColumn>
                                                                                        <asp:TemplateColumn HeaderText="l&#246;schen">
                                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="ibtnSRDelete" runat="server" ToolTip="Zeile löschen" ImageUrl="/Portal/Images/loesch.gif"
                                                                                                    CausesValidation="false" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Kunnr") &amp; "\" &amp; DataBinder.Eval(Container, "DataItem.Auftrag") &amp; "\" %>'></asp:ImageButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="Save" SortExpression="Save" HeaderText="Speichern">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="Auftrag" SortExpression="Auftrag" HeaderText="Auftrag">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="Tour" SortExpression="Tour" HeaderText="Tour">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="FilenameOld" HeaderText="DateinameAlt">
                                                                                        </asp:BoundColumn>
                                                                                    </Columns>
                                                                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                                                                </asp:DataGrid>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td id="td04" valign="top" nowrap align="left" bgcolor="#ffffff" runat="server">
                                                                                <%--<asp:LinkButton ID="btnShowPics" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Hochgeladene Bilder anzeigen</asp:LinkButton>&nbsp;--%>
                                                                            <%--<asp:LinkButton ID="btnSessionAbort" runat="server" CssClass="StandardButtonTable" visible="false">&#149;&nbsp;Abort Session</asp:LinkButton>--%>
                                                                            </td>
                                                                            <td valign="top" nowrap align="right" width="100%" bgcolor="#ffffff">
                                                                                <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td nowrap>
                                                                                            <asp:CheckBox ID="cbxFinished" runat="server" Text="Auftrag abgeschlossen" AutoPostBack="True"
                                                                                                CssClass="TableBackground" Font-Underline="True" Visible="False"></asp:CheckBox>
                                                                                        </td>
                                                                                        <td nowrap align="right" width="100%" colspan="1" rowspan="1">
                                                                                            <asp:LinkButton ID="btnBack" runat="server" Visible="False" CssClass="StandardButtonTable"
                                                                                                ToolTip="Zurück zur Bearbeitung">&#149;&nbsp;Zurück</asp:LinkButton><asp:LinkButton
                                                                                                    ID="btnConfirm" runat="server" Visible="False" CssClass="StandardButtonTable"
                                                                                                    ToolTip="Dateien auf dem Archivserver ablegen">&#149;&nbsp;Auftrag absenden!</asp:LinkButton>
                                                                                            <asp:LinkButton ID="btnFinish" runat="server" CssClass="StandardButtonTable" ToolTip="Weiter zur Bestätigungsseite"> &#149;&nbsp;Fertig</asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label><asp:Label
                                            ID="lblOpen" runat="server"></asp:Label>
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
