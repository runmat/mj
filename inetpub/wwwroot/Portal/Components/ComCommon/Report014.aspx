<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report014.aspx.vb" Inherits="CKG.Components.ComCommon.Report014" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        #File1
        {
            width: 436px;
        }
    </style>
</head>

<script type="text/javascript">
    function checkedAll(e) {

        var aa = document.forms[0];

        var checked = e.checked
        var enabled = false
        for (var i = 0; i < aa.elements.length; i++) {

            var ValId = aa.elements[i].id;
            if (checked == true) {

                if (ValId.indexOf('rb_sel') != -1) {
                    if (aa.elements[i].disabled == enabled) {
                        aa.elements[i].checked = checked;
                    }
                }
            } else {
                if (ValId.indexOf('rb_sel') != -1) {
                    if (aa.elements[i].disabled == enabled) {
                        aa.elements[i].checked = false;
                    }

                }
            }
        }

    }
    function cancelClick() {

    }

</script>

<body>
    <form id="Form1" method="post" runat="server" enctype="multipart/form-data">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtnUpload" />
             <asp:PostBackTrigger ControlID="Datagrid1" />
        </Triggers>
        <ContentTemplate>
            <table id="Table4" width="100%" align="center">
                <tbody>
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
                                        <td class="PageNavigation" colspan="2">
                                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PageNavigation" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" width="120">
                                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                                border="0">
                                                <tr>
                                                    <td class="TaskTitle" width="150">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" width="150">
                                                        <asp:LinkButton ID="cmdCreate" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" width="150">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" width="150">
                                                        <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" ></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" width="150">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top">
                                            <asp:Panel ID="PNL" runat="server" Style="display: none; width: 200px; background-color: White;
                                                border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                Are you sure you want to click the Button?
                                                <br />
                                                <br />
                                                <div style="text-align: right;">
                                                    <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" />
                                                </div>
                                            </asp:Panel>
                                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td valign="top" align="left">
                                                            <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                                                <tr>
                                                                    <td class="TaskTitle" colspan="2">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trGruppe">
                                                                    <td colspan="2">
                                                                        <table id="tblGruppe" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    Gruppe:
                                                                                </td>
                                                                                <td width="90%">
                                                                                    <asp:DropDownList ID="ddlGruppe" runat="server" AutoPostBack="True">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trResult">
                                                                    <td colspan="2">
                                                                        <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" headerCSS="tableHeader"
                                                                            bodyCSS="tableBody" CssClass="tableMain" AllowSorting="True" AutoGenerateColumns="False"
                                                                            AllowPaging="True" PageSize="20">
                                                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                                            <HeaderStyle ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                                            <Columns>
                                                                                <asp:TemplateColumn SortExpression="Filename" HeaderText="col_Dokument">
                                                                                    <HeaderStyle Width="40%"></HeaderStyle>
                                                                                    <ItemStyle Width="40%"></ItemStyle>
                                                                                    <HeaderTemplate>
                                                                                        <asp:LinkButton ID="Linkbutton1" runat="server" CommandArgument="Filename" CommandName="Sort">Dokument</asp:LinkButton>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="lbtPDF" runat="server" CommandName="open" Height="18px" Visible="False"
                                                                                            ImageUrl="/Portal/images/pdf.gif" ToolTip="PDF"></asp:ImageButton>
                                                                                        <asp:ImageButton ID="lbtExcel" runat="server" CommandName="open" Height="18px" Visible="False"
                                                                                            ImageUrl="../../Images/excel.gif" ToolTip="Excel"></asp:ImageButton>
                                                                                        <asp:ImageButton ID="lbtWord" runat="server" CommandName="open" Height="18px" ImageUrl="../../Images/Word_Logo.jpg"
                                                                                            Visible="False" ToolTip="Word" />
                                                                                        <asp:ImageButton ID="lbtJepg" runat="server" CommandName="open" Height="18px" ImageUrl="../../Images/jpg-file.png"
                                                                                            ToolTip="JPG" Visible="False" />
                                                                                        <asp:ImageButton ID="lbtGif" runat="server" CommandName="open" Height="18px" ImageUrl="../../Images/Gif_Logo.gif"
                                                                                            ToolTip="GIF" Visible="False" />
                                                                                        <asp:LinkButton ID="Linkbutton2" runat="server" Width="229px" Height="11px" CommandName="open"
                                                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' ForeColor="Blue">
                                                                                        </asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn SortExpression="Filedate" HeaderText="col_Zeit">
                                                                                    <HeaderStyle Width="15%"></HeaderStyle>
                                                                                    <ItemStyle Width="15%"></ItemStyle>
                                                                                    <HeaderTemplate>
                                                                                        <asp:LinkButton ID="col_Zeit" runat="server" CommandArgument="Filedate" CommandName="Sort">Letzte Änderung</asp:LinkButton>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblFileDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Filedate") %>'>
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn Visible="False" DataField="Serverpfad" SortExpression="Serverpfad">
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn Visible="False" DataField="Pattern" SortExpression="Pattern"></asp:BoundColumn>
                                                                                <asp:TemplateColumn SortExpression="Filedate" HeaderText="">
                                                                                    <HeaderStyle Width="30%"></HeaderStyle>
                                                                                    <HeaderTemplate>
                                                                                        <asp:LinkButton ID="col_Status" runat="server" CommandArgument="Filedate" CommandName="Sort">Status</asp:LinkButton>
                                                                                    </HeaderTemplate>
                                                                                    <ItemStyle Width="30%"></ItemStyle>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblStatus" runat="server" Text='*NEU*' Visible="false" ForeColor="#009900"
                                                                                            Font-Bold="True">
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left">
                                                                                    <HeaderStyle Width="15%"></HeaderStyle>
                                                                                    <ItemStyle Width="15%" ></ItemStyle>
                                                                                    <HeaderTemplate>
                                                                                        <asp:CheckBox ID="ckb_SelectAll" runat="server" ToolTip="Auf dieser Seite alle Dokumente löschen setzen."
                                                                                            onclick="checkedAll(this)" />
                                                                                       <asp:Label ID="Label1" runat="server" Text="Löschen" ForeColor="Black"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="rb_sel" runat="server" GroupName="Auswahl" />
                                                                                    </ItemTemplate>
                                                                               </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <PagerStyle Font-Size="10pt" HorizontalAlign="Left" Wrap="False" Mode="NumericPages">
                                                                            </PagerStyle>
                                                                        </asp:DataGrid>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trUpload">
                                                                    <td valign="top" rowspan="4">
                                                                        &nbsp;
                                                                        <asp:FileUpload ID="upFile" runat="server" />
                                                                        <asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click"> •&nbsp;hochladen</asp:LinkButton>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:LinkButton ID="cmdDel" runat="server"  CssClass="StandardButton" Visible="False"> •&nbsp;Löschen</asp:LinkButton>
                                                                        <ajaxtoolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Wollen Sie die markierten Dateien wirklich löschen?"
                                                                            OnClientCancel="cancelClick" TargetControlID="cmdDel" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr >
                                        <td valign="top" width="120px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            &nbsp;
                                        </td>
                                        <td valign="top">
                                            &nbsp;
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            &nbsp;
                                        </td>
                                        <td valign="top">
                                            &nbsp;
                                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
