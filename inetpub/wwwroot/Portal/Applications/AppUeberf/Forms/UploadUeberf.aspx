<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadUeberf.aspx.vb" Inherits="AppUeberf.UploadUeberf" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
		</HEAD>
	

<script type="text/javascript">
    function checkedAll(e) {

        var aa = document.forms[0];

        var checked = e.checked
        var enabled = false
        for (var i = 0; i < aa.elements.length; i++) {

            var ValId = aa.elements[i].id;
            if (checked == true) {

                if (ValId.indexOf('rb_ok') != -1) {
                    if (aa.elements[i].disabled == enabled) {
                        aa.elements[i].checked = checked;
                    }
                }
            } else {
                if (ValId.indexOf('rb') != -1) {
                    if (aa.elements[i].disabled == enabled) {
                        aa.elements[i].checked = true;
                    }

                }
            }
        }

    }

</script>


<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    
    <uc1:BusyIndicator runat="server" />

    <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="PageNavigation">
                                <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"
                                    Visible="False"> (Fahrzeugsuche)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="right">
                                <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="TaskTitle" valign="top" align="right">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblSelection" cellspacing="0" cellpadding="0" width="100%" border="0"
                                    runat="server">
                                    <tr>
                                        <td valign="top" align="left">
                                            <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0">
                                                <tr>
                                                    <td class="" nowrap align="left">
                                                        <input id="cbxDatei" style="display: none" type="checkbox" name="cbxDatei" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="" valign="top" align="left" width="100%">
                                                        <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                            <tr id="trDateiauswahl">
                                                                <td valign="top">
                                                                    <asp:Label ID="lbl_DateiUpload" runat="server">Datei:</asp:Label>
                                                                </td>
                                                                <td valign="top">
                                                                    <input class="InfoBoxFlat" id="upFile" type="file" size="40" name="File1" runat="server" /><br />
                                                                    <asp:LinkButton ID="cmdUpload0" runat="server" Text="• Hochladen»" OnClientClick="Show_BusyBox1();"
                                                                        CssClass="StandardButtonTable" Height="26px" Width="100px" />
                                                                    &nbsp;
                                                                    <asp:Button ID="cmdNEW" runat="server" CssClass="StandardButtonTable" Height="26px"
                                                                        Text="• manuell  Erfassen" Width="120px" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_Protokoll" runat="server" Height="64px" ReadOnly="True" TextMode="MultiLine"
                                                                        Width="519px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trFilter">
                                                                <td valign="top">
                                                                    <asp:Label ID="lbl_Filter" runat="server">Filter:</asp:Label>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" Height="16px"
                                                                        RepeatDirection="Horizontal" RepeatLayout="Flow" Width="184px" BackColor="White">
                                                                        <asp:ListItem>Alle&nbsp;&nbsp;</asp:ListItem>
                                                                        <asp:ListItem>Eigene</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100%">
                                                        <asp:ImageButton ID="cmdBack0" runat="server" Width="73px" Height="40px" ImageUrl="/Portal/Images/BackToMap.jpg"
                                                            ToolTip="Zurück zur Übersicht" Visible="False"></asp:ImageButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td runat="server" align="right" id="tdExcel" visible="false" class="" width="100%">
                                                        &nbsp;<img alt="" src="../../../images/excel.gif" style="height: 17px" />&nbsp;<asp:LinkButton
                                                            ID="lnkCreateExcel" runat="server" CssClass="ExcelButton">Excelformat</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="Table4" cellspacing="0" cellpadding="5" width="100%" border="0">
                                                <tr runat="server" id="DateGridRow">
                                                    <td>
                                                        <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="25"
                                                            headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain" bodyHeight="300"
                                                            AllowSorting="true" AllowPaging="true" AutoGenerateColumns="False">
                                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                            <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderText="">
                                                                    <ItemTemplate>
                                                                        <asp:Image ImageAlign="AbsMiddle" ToolTip="Pflichtfelder gefüllt" ID="OK" runat="server"
                                                                            Height="16px" ImageUrl="/Portal/Images/AllesOK2.jpg" Width="16px" Visible='<%# DataBinder.Eval(Container, "DataItem.Problem") = "O" %>' />
                                                                        <asp:Image ID="Problem" ImageAlign="AbsMiddle" ToolTip="min. 1 Pflichtfeld nicht gefüllt"
                                                                            runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Problem") = "X" %>'
                                                                            Height="16px" ImageUrl="/Portal/Images/Problem.jpg" Width="16px" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn Visible="False" DataField="Auf_ID"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Datum" SortExpression="Datum" HeaderText="Angel. am">
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="UeberfDatum" SortExpression="UeberfDatum" HeaderText="Überf. am">
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Referenz" SortExpression="Referenz" HeaderText="Referenz">
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Fahrzeugtyp" SortExpression="Fahrzeugtyp" HeaderText="Fahrzeugtyp">
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Startort" SortExpression="Startort" HeaderText="Startort">
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Zielort" SortExpression="Zielort" HeaderText="Zielort">
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="WEZielort" SortExpression="WEZielort" HeaderText="Warenempf. Zielort">
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Rueckort" SortExpression="Rueckort" HeaderText="Rückort">
                                                                </asp:BoundColumn>
                                                                <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label runat="server" Text="OK"></asp:Label>
                                                                        <asp:CheckBox ID="ckb_SelectAll" runat="server" ToolTip="Auf dieser Seite alle OK setzen."
                                                                            onclick="checkedAll(this)" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:RadioButton ID="rb_ok" runat="server" GroupName="Auswahl" Enabled='<%# DataBinder.Eval(Container, "DataItem.Problem") = "O" %>'
                                                                            Checked='<%# DataBinder.Eval(Container, "DataItem.Ok") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="50" HeaderText="Löschen">
                                                                    <ItemTemplate>
                                                                        <asp:RadioButton ID="rb_del" runat="server" GroupName="Auswahl" Checked='<%# DataBinder.Eval(Container, "DataItem.Del") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="50" HeaderText="keine Auswahl">
                                                                    <ItemTemplate>
                                                                        <asp:RadioButton ID="rb_Deselect" runat="server" GroupName="Auswahl" Checked='<%# DataBinder.Eval(Container, "DataItem.NoSel") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Vbeln" SortExpression="Vbeln" HeaderText="Auftragsnummer"
                                                                    Visible="false"></asp:BoundColumn>
                                                                <asp:TemplateColumn ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderText="Bearbeiten">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="cmdEdit" runat="server" CommandName="Edit" Target="_blank" ImageUrl="../../../Images/lupe2.gif"
                                                                            Width="16px" ToolTip="Bearbeiten" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.URL") %>'
                                                                            Height="16px" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                                HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                                        </asp:DataGrid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td valign="top">
                &nbsp;
                <asp:LinkButton CssClass="StandardButtonTable" ID="cmdSend0" runat="server"
                    OnClientClick="Show_BusyBox1();" Text="Absenden" Width="100px" Height="26px" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                &nbsp;
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </form>
</body>
</HTML>
