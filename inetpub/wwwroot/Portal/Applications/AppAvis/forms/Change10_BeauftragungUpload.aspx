<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change10_BeauftragungUpload.aspx.vb" Inherits="AppAvis.Change10_BeauftragungUpload" %>

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

<script language="JavaScript" type="text/javascript">
    function openinfo(url) {
        fenster = window.open(url, "SIXT", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=500,height=200");
        fenster.focus();
    }
</script>

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
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
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
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" width="120">
                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                        <tr id="trCreate" runat="server">
                            <td valign="center">
                                <asp:LinkButton ID="cmdCreate" runat="server" 
                                    CssClass="StandardButton">&#149;&nbsp;Absenden</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td valign="center">
                                <asp:LinkButton ID="lbBack" runat="server" 
                                    CssClass="StandardButton">&#149;&nbsp;Zurück</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                </td>   
            </tr>
            <tr>
                <td width="120">
                </td>
                <td valign="top">
                    <table cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                        <tr>
                            <td colspan="1" align="left" class="TaskTitle">
                                  <asp:Label ID="lbl_Info" Text="Transportbeauftragung-Excel-Datei-Auswahl" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-color: #f5f5f5; border-style: solid; border-width: 3;">
                                <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                    <tr>
                                        <td class="TextLarge" nowrap align="right">
                                            Dateiauswahl <a href="javascript:openinfo('Info04.htm');">
                                                <img src="../../../images/fragezeichen.gif" border="0" /></a>:&nbsp;&nbsp;
                                        </td>
                                        <td class="TextLarge">
                                            <input id="upFile" type="file" size="49" name="File1" runat="server" />&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap align="right">
                                            &nbsp;
                                        </td>
                                        <td class="TextLarge">
                                            &nbsp;
                                            <asp:Label ID="lblExcelfile" runat="server" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="ExcelCell" runat="server" visible="false" align="right">
                                <strong>&nbsp;<img alt="" src="../../../images/excel.gif" style="width: 16px; height: 16px" />&nbsp;
                                    <asp:LinkButton CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>
                                    &nbsp;&nbsp; </strong>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                    bodyCSS="tableBody" bodyHeight="400" CssClass="tableMain" DataKeyNames="CHASSIS_NUM" 
                                    headerCSS="tableHeader" PageSize="50" Width="100%">
                                    <HeaderStyle CssClass="GridTableHead" Wrap="False" />
                                    <PagerStyle CssClass="TextExtraLarge" Wrap="False" />
                                    <Columns>
                                        <asp:BoundField DataField="CHASSIS_NUM" HeaderText="Fahrgestellnummer" />
                                        <asp:BoundField DataField="EX_KUNNR" HeaderText="Stationscode" />
                                        <asp:BoundField DataField="BEAUFDAT" HeaderText="Anlieferungsdatum" DataFormatString="{0:dd.MM.yyyy}" />
                                        <asp:BoundField DataField="SPEDITION" HeaderText="Spediteur" />
                                        <asp:TemplateField HeaderText="Status" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMessage" Font-Bold = "true" Text ='<%# Eval("BEMERKUNG") %>' runat="server" />
                                              </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
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
            <tr>
                <td colspan="2">
                    <!--#include File="../../../PageElements/Footer.html" -->
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
