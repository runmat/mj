<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04.aspx.vb" Inherits="AppCommonCarRent.Change04" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="150"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="StandardButton" Height="16px"
                                            Width="120px"> Speichern</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdConfirm" runat="server" CssClass="StandardButton" Visible="False"
                                            Height="16px" Width="120px"> Bestätigen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton" Visible="False"
                                            Height="16px" Width="120px"> Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="lb_Auswahl" Visible="True" runat="server" 
                                            CssClass="StandardButton" Height="16px" Width="120px">Funktionsauswahl</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <p>
                                &nbsp;</p>
                            <p>
                                &nbsp;</p>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;&nbsp;</td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td class="TextLarge" valign="top">
                                                    &nbsp;
                                                </td>
                                                <td class="TextLarge" valign="middle" align="right">
                                                    <asp:ImageButton ID="imgbExcel" runat="server" Height="16px" ImageUrl="../../../Images/excel.gif"
                                                        Visible="True" Width="16px" ImageAlign="TextTop" />
                                                    &nbsp;<strong>
                                                        <asp:LinkButton ID="lnkCreateExcel" runat="server" CssClass="ExcelButton">Excelformat</asp:LinkButton>
                                                    </strong>
                                                </td>
                                                <td class="TextLarge" valign="middle" align="right">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" valign="top" >
                                                    <asp:ListBox ID="ListBox1" runat="server" CssClass="ListStyle" AutoPostBack="True"
                                                        Height="420px" Width="200px"></asp:ListBox>
                                                </td>
                                                <td class="TextLarge" valign="top" align="left" Width="100%">
                                                    <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50"
                                                        headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain" bodyHeight="400"
                                                        AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True">
                                                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                        <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Auswahl" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Selected") %>'>
                                                                    </asp:CheckBox>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn SortExpression="Zzkunnr_Zs" HeaderText="Kunde">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name1") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ZZMODELL" SortExpression="ZZMODELL" HeaderText="Fahrzeugtyp">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Verkaufsdatum" SortExpression="Verkaufsdatum" HeaderText="Verkaufsdatum"
                                                                DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Bezahltkennzeichen" ItemStyle-VerticalAlign="Middle"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lbtnBzKennz" runat="server" CommandName="Sort" CommandArgument="Bezahltkennzeichen">Bezahltkennzeichen</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chbBzKennz" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZBEZAHLT")="X" %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Versandsperre" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lbtn_Sperren" runat="server" CommandName="Sort" CommandArgument="Versandsperre">Versandsperre</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chbSperren" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZAKTSPERRE")="X" %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                        <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                            HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                        <br>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="150">
                            
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="150">
                            &nbsp;
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
