<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report12.aspx.vb" Inherits="AppCommonLeasing.Report12" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style1
        {
            width: 8%;
            height: 32px;
        }
        .style2
        {
            width: 100%;
            height: 32px;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager EnableScriptGlobalization="true" runat="server" />
    <script language="javascript" id="ScrollPosition">
<!--

        function sstchur_SmartScroller_GetCoords() {

            var scrollX, scrollY;

            if (document.all) {
                if (!document.documentElement.scrollLeft)
                    scrollX = document.body.scrollLeft;
                else
                    scrollX = document.documentElement.scrollLeft;

                if (!document.documentElement.scrollTop)
                    scrollY = document.body.scrollTop;
                else
                    scrollY = document.documentElement.scrollTop;
            }
            else {
                scrollX = window.pageXOffset;
                scrollY = window.pageYOffset;
            }



            document.forms["Form1"].xCoordHolder.value = scrollX;
            document.forms["Form1"].yCoordHolder.value = scrollY;

        }

        function sstchur_SmartScroller_Scroll() {


            var x = document.forms["Form1"].xCoordHolder.value;
            var y = document.forms["Form1"].yCoordHolder.value;
            window.scrollTo(x, y);

        }

        window.onload = sstchur_SmartScroller_Scroll;
        window.onscroll = sstchur_SmartScroller_GetCoords;
        window.onkeypress = sstchur_SmartScroller_GetCoords;
        window.onclick = sstchur_SmartScroller_GetCoords;
  
// -->
    </script>
    <input type="hidden" id="xCoordHolder" runat="server" />
    <input type="hidden" id="yCoordHolder" runat="server" />
    <table width="100%" align="center">
        <tbody>
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="PageNavigation" colspan="2">
                                    <asp:Label ID="lblHead" runat="server" />
                                    <asp:Label ID="lblPageTitle" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 140px">
                                    <table id="Table2" cellspacing="0" cellpadding="0" style="width: 100%" border="0">
                                        <tr>
                                            <td class="TaskTitle">
                                                <asp:LinkButton ID="lb_zurueck" OnClick="NavigateBackClick" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <asp:LinkButton CssClass="StandardButton" Height="18px" ID="cmdSearch" OnClick="SucheClick"
                                                    runat="server" Width="120px" Text="•&nbsp;Suchen" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <asp:LinkButton Visible="False" CssClass="StandardButton" Height="18px" ID="cmdNewSearch"
                                                    OnClick="OpenSucheClick" runat="server" Width="120px" Text="•&nbsp;Neue&nbsp;Suche" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <asp:LinkButton CssClass="StandardButton" Height="18px" ID="cmdEnd" OnClick="EndVersendet"
                                                    Visible="False" Enabled="False" runat="server" Width="120px" Text="•&nbsp;Endgültig" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td class="TaskTitle">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left">
                                                    <table id="Table1" runat="server" cellspacing="0" cellpadding="5" width="100%" bgcolor="white"
                                                        border="0">
                                                        <tr>
                                                            <td class="TextLarge style1" nowrap="nowrap">
                                                                <asp:Label ID="lbl_Fahrgestellnummer" runat="server" />
                                                            </td>
                                                            <td class="style2">
                                                                <asp:TextBox ID="txtFahrgestellnummer" CssClass="TextBoxNormal" runat="server" MaxLength="30" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge style1" nowrap="nowrap">
                                                                <asp:Label ID="lbl_Kennzeichen" runat="server" />
                                                            </td>
                                                            <td class="style2">
                                                                <asp:TextBox ID="txtKennzeichen" CssClass="TextBoxNormal" runat="server" MaxLength="15" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge style1" nowrap="nowrap">
                                                                <asp:Label ID="lbl_Vertragsnummer" runat="server" />
                                                            </td>
                                                            <td class="style2">
                                                                <asp:TextBox ID="txtVertragsnummer" CssClass="TextBoxNormal" runat="server" MaxLength="20" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge style1" nowrap="nowrap">
                                                                <asp:Label ID="lbl_Objektnummer" runat="server" />
                                                            </td>
                                                            <td class="style2">
                                                                <asp:TextBox ID="txtObjektnummer" CssClass="TextBoxNormal" runat="server" MaxLength="25" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge style1" nowrap="nowrap">
                                                                <asp:Label ID="lbl_DatumVon" runat="server" />
                                                            </td>
                                                            <td class="style2">
                                                                <asp:TextBox ID="txtDatumVon" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                                    Animated="false" Enabled="True" TargetControlID="txtDatumVon" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge style1" nowrap="nowrap">
                                                                <asp:Label ID="lbl_DatumBis" runat="server" />
                                                            </td>
                                                            <td class="style2">
                                                                <asp:TextBox ID="txtDatumBis" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                                    Animated="false" Enabled="True" TargetControlID="txtDatumBis" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblResult" runat="server" visible="false" cellspacing="0" cellpadding="0"
                                                        width="100%" border="0">
                                                        <tr>
                                                            <td style="padding-left: 15px">
                                                                <asp:Label ID="lblInfo" Font-Bold="true" runat="server" />
                                                            </td>
                                                            <td align="right">
                                                                Ergebnisse/Seite:&nbsp;
                                                                <asp:DropDownList ID="pageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="PageSizeChanged">
                                                                    <asp:ListItem Text="10" Value="10" />
                                                                    <asp:ListItem Text="20" Value="20" />
                                                                    <asp:ListItem Text="50" Value="50" Selected="True" />
                                                                    <asp:ListItem Text="100" Value="100" />
                                                                    <asp:ListItem Text="200" Value="200" />
                                                                    <asp:ListItem Text="500" Value="500" />
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="padding-left: 15px">
                                                                <asp:DataGrid ID="tempBriefGrid" runat="server" BackColor="White" PageSize="50" Width="100%"
                                                                    AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" OnSortCommand="SortChanged"
                                                                    OnPageIndexChanged="PageIndexChanged">
                                                                    <AlternatingItemStyle CssClass="GridTableAlternate" />
                                                                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead" />
                                                                    <Columns>
                                                                        <asp:TemplateColumn SortExpression="Status">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="Status" />
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox runat="server" Checked='<%# CStr(Eval("Status")) = "X" %>' Visible='<%# CStr(Eval("Status")) = "-" OrElse CStr(Eval("Status")) = "X" %>'
                                                                                    AutoPostBack="True" OnCheckedChanged="ToggleBrief" />
                                                                                <asp:Image runat="server" Visible='<%# Not string.IsNullOrEmpty(CStr(Eval("Status"))) AndAlso  CStr(Eval("Status")) <> "-" AndAlso CStr(Eval("Status")) <> "X" %>'
                                                                                    Width="12px" Height="12px" ToolTip='<%# Eval("Status") %>' ImageUrl="/Portal/Images/Problem.jpg" />
                                                                                <asp:Image runat="server" Visible='<%# string.IsNullOrEmpty(CStr(Eval("Status"))) %>'
                                                                                    Width="12px" Height="12px" ToolTip="Endgültiger Versand beauftragt" ImageUrl="/Portal/Images/AllesOk2.jpg" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn Visible="False" HeaderText="col_Equipmentnummer" SortExpression="EQUNR">
                                                                            <HeaderStyle Width="0" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="EQUNR" runat="server" Text='<%# Eval("EQUNR") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn HeaderText="col_Fahrgestellnummer" SortExpression="CHASSIS_NUM">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="CHASSIS_NUM" runat="server" Text='<%# Eval("CHASSIS_NUM") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn HeaderText="col_Kennzeichen" SortExpression="LICENSE_NUM">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" Text='<%# Eval("LICENSE_NUM") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn HeaderText="col_Vertragsnummer" SortExpression="LIZNR">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Vertragsnummer</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" Text='<%# Eval("LIZNR") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn HeaderText="col_Objektnummer" SortExpression="ZZREFERENZ1">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Objektnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Objektnummer</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" Text='<%# Eval("ZZREFERENZ1") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn HeaderText="col_Versanddatum" SortExpression="ZZTMPDT">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="col_Versanddatum" runat="server" CommandName="Sort" CommandArgument="ZZTMPDT">col_Versanddatum</asp:LinkButton>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" Text='<%# Eval("ZZTMPDT","{0:dd.MM.yyyy}") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                    </Columns>
                                                                    <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                                        HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages" />
                                                                </asp:DataGrid>
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
                                <td valign="top">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    &nbsp;<asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <!--#include File="../../../PageElements/Footer.html" -->
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
