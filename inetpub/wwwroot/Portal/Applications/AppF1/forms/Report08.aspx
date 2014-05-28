<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report08.aspx.vb" Inherits="AppF1.Report08" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

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
    
    <uc1:BusyIndicator runat="server" />

    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center">
        <tr>
            <td colspan="2">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                
                    
                    
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" runat="server"> (Mahnung)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="100">
                            <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton">Zurück</asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="100">
                            <asp:LinkButton ID="cmdSearch" OnClientClick="Show_BusyBox1();" runat="server" CssClass="StandardButton">Weiter</asp:LinkButton>
                            <asp:LinkButton ID="cmdNewSearch" OnClientClick="Show_BusyBox1();" runat="server"
                                CssClass="StandardButton" Visible="False">Neue Suche</asp:LinkButton>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="100" style="padding-top:4px">
                            <p>
                                <asp:Calendar ID="calVon" runat="server" CellPadding="0" BorderColor="Black" BorderStyle="Solid"
                                    Width="120px" Visible="False">
                                    <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                    <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                    <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                    <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                    <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                    <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                    <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                </asp:Calendar>
                            </p>
                        </td>
                        <td valign="top" style="padding-left:50px">
                            <asp:Label ID="lblMeldungsdatum" runat="server" Text="Meldungsdatum: "></asp:Label>
                            <asp:TextBox ID="txtDatumVon" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:ImageButton ID="ibtCal" runat="server" ImageUrl="../../../images/calendar.jpg" />
                            <asp:CompareValidator ID="cv_txtDatumVon" runat="server" ErrorMessage="Falsches Datumsformat"
                                Type="Date" ControlToValidate="txtDatumVon" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                CssClass="TextError" ForeColor=""></asp:CompareValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblGrid" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td width="100">
                            &nbsp;
                        </td>
                        <td align="left" valign="bottom">
                            <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                        </td>
                        <td id="tdExcel" runat="server" valign="bottom" align="right">
                            <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" /><asp:LinkButton
                                CssClass="ExcelButton" ID="lnkCreateExcel" runat="server" Visible="False">Excelformat</asp:LinkButton></STRONG>
                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" Visible="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="100" valign="top">
                        </td>
                        <td colspan="2">
                            <asp:GridView ID="grvAusgabe" runat="server" AllowSorting="True" AllowPaging="True"
                                AutoGenerateColumns="False" Width="100%" PageSize="50" BackColor="White" CssClass="tableMain">
                                <PagerSettings Position="Top" />
                                <Columns>
                                    <asp:TemplateField HeaderText="col_Haendler_Ex" SortExpression="HAENDLER_EX">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Haendler_Ex" runat="server" CommandArgument="Haendler_Ex"
                                                CommandName="Sort">col_Haendler_Ex</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbHaendler" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                CommandName="weiter" Text='<%# Bind("HAENDLER_EX") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="col_Name" SortExpression="NAME1">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Name" runat="server" CommandArgument="NAME1" CommandName="Sort">col_Name</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("NAME1") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="col_Ort" SortExpression="ORT01">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Ort" runat="server" CommandArgument="ORT01" CommandName="Sort">col_Ort</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("ORT01") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="col_AnzWiedereingang" SortExpression="COUNT_WE">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_AnzWiedereingang" runat="server" CommandArgument="COUNT_WE"
                                                CommandName="Sort">col_AnzWiedereingang</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:D}", Convert.ToINT32(Eval("[COUNT_WE]"))) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="col_AeltWiedereingang" SortExpression="ZZLSDAT_WE_FIRST">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_AeltWiedereingang" runat="server" CommandArgument="ZZLSDAT_WE_FIRST"
                                                CommandName="Sort">col_AeltWiedereingang</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZLSDAT_WE_FIRST", "{0:d}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="col_Zahlungseingang" SortExpression="COUNT_ZE">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Zahlungseingang" runat="server" CommandArgument="COUNT_ZE"
                                                CommandName="Sort">col_Zahlungseingang</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# String.Format("{0:D}", Convert.ToINT32(Eval("[COUNT_ZE]"))) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="col_AeltVorgang" SortExpression="ZZLSDAT_ZE_FIRST">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_AeltVorgang" runat="server" CommandArgument="ZZLSDAT_ZE_FIRST"
                                                CommandName="Sort">col_AeltVorgang</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZLSDAT_ZE_FIRST", "{0:d}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="col_OffBetraege" SortExpression="OFFENER_BETRAG_ZE_SUM">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_OffBetraege" runat="server" CommandArgument="OFFENER_BETRAG_ZE_SUM"
                                                CommandName="Sort">col_OffBetraege</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblHSN" runat="server" Text='<%# String.Format("{0:C}", Convert.ToDouble(Eval("[OFFENER_BETRAG_ZE_SUM]"))) %>' ></asp:Label>
                                            
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="TextExtraLarge" />
                                <HeaderStyle CssClass="GridTableHead" />
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td width="100" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
                <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
            </td>
        </tr>
    </table>
    </form>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
</body>
</html>
