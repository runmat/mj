<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_2.aspx.vb" Inherits="AppF1.Change01_2" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../controls/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="2" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:Label>
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
                                <tr id="trcmdSave" runat="server">
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trcmdSave2" runat="server">
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="cmdSave2" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:HyperLink ID="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change01.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;
                                        <asp:HyperLink ID="lnkFahrzeugAuswahl" runat="server" CssClass="TaskTitle" NavigateUrl="Change01_2.aspx">Fahrzeugauswahl</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="3">
                                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblInfo" Visible="false" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                                    &nbsp;<asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <strong>Anzahl Vorgänge / Seite</strong>
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <strong>Betreff für Empfänger:</strong>&nbsp;
                                        <asp:TextBox ID="txtKopf" runat="server" MaxLength="23" Width="250px"></asp:TextBox><asp:LinkButton
                                            ID="LinkButton1" runat="server" CssClass="StandardButtonTable" Visible="False">&#149;&nbsp;Kopftext erf.</asp:LinkButton>
                                        <asp:DropDownList ID="ddlAbrufgrund" runat="server" Style="padding-left: 15px" Visible="false"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False"
                                            AllowPaging="True" AllowSorting="True" bodyHeight="400" CssClass="tableMain"
                                            bodyCSS="tableBody" headerCSS="tableHeader" PageSize="20" 
                                            BackColor="White">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZFAHRG") %>'>
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="LIZNR" HeaderText="col_Kontonummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kontonummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Kontonummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'
                                                            ID="Label1">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="TIDNR" HeaderText="col_NummerZB2">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'
                                                            ID="Label3">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="LICENSE_NUM"
                                                            CommandName="Sort">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZREFERENZ1" HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'
                                                            ID="Label2">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="False" DataField="Positionstext" SortExpression="Positionstext"
                                                    HeaderText="Referenz"></asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="ZZBEZAHLT" HeaderText="col_Bezahlt">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Bezahlt" runat="server" CommandArgument="ZZBEZAHLT" CommandName="Sort">col_Bezahlt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkBezahlt" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZBEZAHLT") %>'
                                                            Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZBEZAHLT") %>'>
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZCOCKZ" HeaderText="col_COC">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_COC" runat="server" CommandArgument="ZZCOCKZ" CommandName="Sort">col_COC</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Checkbox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="Textbox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'>
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="ZZBOOLAUFZEIT" HeaderText="col_Laufzeit">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Laufzeit" runat="server" CommandName="Sort" CommandArgument="ZZBOOLAUFZEIT">col_Laufzeit</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkHaltefrist" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZBOOLAUFZEIT") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Nicht&lt;br&gt;anfordern">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="chkNichtAnfordern" runat="server" Checked="True" GroupName="Kontingentart">
                                                        </asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Temp" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="chk0001" runat="server" GroupName="Kontingentart"></asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="anfordern">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="chk0002" runat="server" GroupName="Kontingentart"></asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="DP&lt;br&gt;endg." Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="chk0004" runat="server" GroupName="Kontingentart"></asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Retail" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="chk0003" runat="server" GroupName="Kontingentart"></asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="KF/KL" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="chk0006" runat="server" GroupName="Kontingentart"></asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="false" SortExpression="TEXT300" HeaderText="Anfrage-Nr.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAnfragenr" runat="server" MaxLength="13" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.TEXT300") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Referenz">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPosition" runat="server" MaxLength="15" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.POSITIONSTEXT") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="False" HeaderText="Versandart">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTemp" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="1" %>'>Temporär</asp:Label>
                                                        <asp:Label ID="lblEndg" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="2" %>'>Endgültig</asp:Label>
                                                        <asp:Label ID="lblRet" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="3" %>'>Retail</asp:Label>
                                                        <asp:Label ID="lblDP" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="4" %>'>DP</asp:Label>
                                                        <asp:Label ID="lblKFKL" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT")="6" %>'>KF/KL</asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Abrufgrund">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="cmbAbrufgrund" runat="server" AutoPostBack="True" DataTextField="WebBezeichnung"
                                                            DataValueField="SapWert" DataSource='<%# cmbAbrufgrund_ItemDataBound1( DataBinder.Eval(Container, "DataItem.MANDT"))%>'>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="false" HeaderText="Abrufgrund-Info/Text">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblZusatzinfo" EnableViewState="True" Visible="True" runat="server"></asp:Label><br>
                                                        <asp:TextBox ID="txtZusatztext" runat="server" EnableViewState="True" Visible="False"
                                                            MaxLength="50" Width="250px" BorderWidth="1" BorderStyle="Solid" BorderColor="red"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True"
                                                PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False"
                                                Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            &nbsp;
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                    <tr id="ShowScript" runat="server">
                        <td width="120">
                            &nbsp;
                        </td>
                        <td>

                            <script language="JavaScript">
										<!--                                //
                                // window.document.Form1.elements[window.document.Form1.length-3].focus();
										//-->
                            </script>

                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
