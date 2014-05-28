<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report05.aspx.vb" Inherits="AppCommonLeasing.Report05" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
    <table id="Table4" width="100%" align="center">
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
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" valign="top" colspan="2">
                                &nbsp;
                            </td>
            </td>
        </tr>
        <tr>
            <td valign="top" width="120">
                <table id="Table2" cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                    <tr id="trCreate" runat="server">
                        <td valign="center">
                            <asp:LinkButton ID="lb_Weiter" OnClientClick="Show_BusyBox1();" Text="Weiter" runat="server"
                                CssClass="StandardButton"></asp:LinkButton>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="center">
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                <span lang="de">
                <br />
                <asp:Label ID="lbl_Info" runat="server" EnableViewState="False" 
                    Font-Bold="True"></asp:Label>
                </span>
            </td>
        </tr>
        <tr>
            <td width="100">
            </td>
            <td valign="top">
                <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                    <tr>
                        <td colspan="2" align="left">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table3" runat="server" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr id="tr_Datumab" runat="server">
                                                <td class="TextLarge" valign="top" width="150">
                                                    <asp:Label ID="lbl_Datumab" runat="server" EnableViewState="False"></asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtAbDatum" runat="server" Width="130px"></asp:TextBox>
                                                    <asp:ImageButton ID="imgbDatumAb" runat="server" 
                                                        ImageUrl="../../../Images/calendar.gif" />
                                                    <asp:Calendar ID="calAbDatum" runat="server" Width="160px" Visible="False" BorderColor="Black"
                                                        BorderStyle="Solid" CellPadding="0">
                                                        <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                                        <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                                        <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                                        <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                                        <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                                        <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                                        <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                                    </asp:Calendar>
                                                   
                                                </td>
                                            </tr>
                                            <tr id="tr_Datumbis" runat="server">
                                                <td class="StandardTableAlternate" valign="top" width="150">
                                                    <asp:Label ID="lbl_Datumbis" runat="server" EnableViewState="False"></asp:Label>
                                                </td>
                                                <td class="StandardTableAlternate" valign="center">
                                                    <asp:TextBox ID="txtBisDatum" runat="server" Width="130px"></asp:TextBox>
                                                    <asp:ImageButton ID="imgbDatumBis" ImageUrl="../../../Images/calendar.gif" runat="server" /><asp:Calendar ID="calBisDatum" runat="server" Width="160px"
                                                            Visible="False" BorderColor="Black" BorderStyle="Solid" CellPadding="0">
                                                            <TodayDayStyle Font-Bold="True"></TodayDayStyle>
                                                            <NextPrevStyle ForeColor="White"></NextPrevStyle>
                                                            <DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
                                                            <SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
                                                            <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
                                                            <WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
                                                            <OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
                                                        </asp:Calendar>
                                                </td>
                                            </tr>
                                            <tr id="tr_Leasingvertragsnummer">
                                                <td class="TextLarge"  valign="top" width="150">
                                                    <asp:Label ID="lbl_Leasingvertragsnummer" runat="server" ></asp:Label>
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox CssClass="TextBoxLarge" ID="txtLeasingVertragsnummer" runat="server"></asp:TextBox>
                                                </td>
                                                
                                            </tr>
                                            
                                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" width="100">
                &nbsp;
            </td>
            <td align="left" colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="top">
                &nbsp;
            </td>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
