<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Druck1.aspx.vb" Inherits="AppFFD.Druck1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Zu autorisierende Vorgänge - Druckansicht</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <style type="text/css">
        .style1 {
            height: 20px;
        }
    </style>
</head>
<body bgcolor="white" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table1" cellspacing="1" cellpadding="1" width="550" border="0">
        <tr>
            <td nowrap colspan="2">
                <font face="Arial"></font>
                <p>
                    <font face="Arial"><strong><font size="4"><u>Freizugebende&nbsp;Vorgänge</u></font></strong></font></p>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <font face="Arial"></font><font face="Arial"><font size="2">Benutzer:</font>&nbsp;
                    <asp:TextBox ID="txtUser" runat="server" BorderColor="Transparent" ReadOnly="True"
                        BorderWidth="0px" BackColor="Transparent"></asp:TextBox></font>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="2">
                <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td width="22">
                            <font face="Arial" size="2"><strong>Händlernr:</strong></font>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtNr" runat="server" BorderColor="Transparent" ReadOnly="True"
                                BorderWidth="0px" BackColor="Transparent" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="22">
                            <font face="Arial" size="2"><strong>Name:</strong></font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" BorderColor="Transparent" ReadOnly="True"
                                BorderWidth="0px" BackColor="Transparent" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="22">
                            <font face="Arial" size="2"><strong>Adresse:</strong></font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAdresse" runat="server" BorderColor="Transparent" ReadOnly="True"
                                BorderWidth="0px" BackColor="Transparent" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="2">
                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="1">
                    <tr>
                        <td>
                            <font face="Arial" size="1"><strong>Kontingentart</strong></font>
                        </td>
                        <td>
                            <font face="Arial" size="1"><strong>Kontingent</strong></font>
                        </td>
                        <td>
                            <font face="Arial" size="1"><strong>Inanspruchnahme</strong></font>
                        </td>
                        <td nowrap>
                            <font face="Arial" size="1"><strong>Freies Kontingent</strong></font>
                        </td>
                        <td nowrap>
                            <font face="Arial" size="1">ZE Eingang</font>
                        </td>
                        <td nowrap>
                            <font face="Arial" size="1">Neues freies<br>
                                Kontingent</font>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            <font face="Arial" size="2">
                                <asp:TextBox ID="txtA001" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                    BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:TextBox></font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtK001" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtI001" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtF001" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="Textbox1" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                Width="50px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="Textbox5" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                Width="50px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="20">
                            <font face="Arial" size="2">
                                <asp:TextBox ID="txtA002" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                    BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:TextBox></font>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="txtK002" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="txtI002" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="txtF002" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="Textbox2" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                Width="50px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="Textbox6" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                Width="50px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trRet" runat="server">
                        <td class="style1">
                            <font face="Arial" size="2">
                                <asp:TextBox ID="txtA003" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                    BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:TextBox></font>
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtK003" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtI003" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtF003" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="Textbox12" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                Width="50px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="Textbox13" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                Width="50px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trDP" runat="server">
                        <td height="20">
                            <font face="Arial" size="2">
                                <asp:TextBox ID="txtA004" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                    BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:TextBox></font>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="txtK004" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="txtI004" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="txtF004" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small" ></asp:TextBox>
                       </td>
                        <td height="20">
                            <asp:TextBox ID="Textbox3" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                Width="50px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="Textbox7" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                Width="50px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trHEZ" runat="server">
                        <td height="20" nowrap>
                            <font face="Arial" size="2">
                                <asp:TextBox ID="txtA005" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                    BackColor="Transparent" Width="100%" Font-Size="XX-Small"></asp:TextBox></font>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="txtK005" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="txtI005" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="txtF005" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                BackColor="Transparent" Width="30px" Font-Size="XX-Small" ></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="Textbox4" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                Width="50px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                        <td height="20">
                            <asp:TextBox ID="Textbox8" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                Width="50px" Font-Size="XX-Small"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <p>
                    <font face="Arial"><u><strong>Vorgänge</strong></u></font></p>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <p>
                    <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" headerCSS="tableHeader"
                        bodyCSS="tableBody" CssClass="tableMain" bodyHeight="250" AutoGenerateColumns="False">
                        <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                        <HeaderStyle Font-Size="XX-Small" Font-Names="Arial" Font-Bold="True" Wrap="False"
                            CssClass="GridTableHead"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="Auftragsnummer" SortExpression="Auftragsnummer" HeaderText="Auftrags-Nr.">
                                <ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Vertragsnummer" SortExpression="Vertragsnummer" HeaderText="Vertrags-Nr.">
                                <ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Angefordert am" SortExpression="Angefordert am" HeaderText="Angefordert am:"
                                DataFormatString="{0:dd.MM.yyyy}">
                                <ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer"
                                HeaderText="Fahrgestell-Nr."></asp:BoundColumn>
                            <asp:BoundColumn DataField="Briefnummer" SortExpression="Briefnummer" HeaderText="Briefnummer">
                                <ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Kontingentart" SortExpression="Kontingentart" HeaderText="Kontingentart">
                                <ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Finanzierungsart" SortExpression="Finanzierungsart" HeaderText="FA">
                                <ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="InAutorisierung" SortExpression="InAutorisierung"
                                HeaderText="InAutorisierung"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Initiator" SortExpression="Initiator"
                                HeaderText="Initiator"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Freigabe">
                                <ItemStyle Font-Size="XX-Small" Font-Names="Arial"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblAut" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.InAutorisierung")=TRUE %>'
                                        Text='>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                            PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Wrap="False"></PagerStyle>
                    </asp:DataGrid></p>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <p>
                    <font face="Arial" size="2">
                        <br>
                        <br>
                        <br>
                        <strong>Tagesdatum, Unterschrift</strong>:</font>________________________________________</p>
            </td>
        </tr>
    </table>
    <p>
        &nbsp;</p>
    <p>
    </p>
    </form>
</body>
</html>
