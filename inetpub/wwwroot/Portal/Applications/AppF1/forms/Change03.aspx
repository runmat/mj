<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03.aspx.vb" Inherits="AppF1.Change03" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../controls/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
</head>
<body>
    <form id="form1" method="post" runat="server">
        <asp:ScriptManager ID="Scriptmanager1" runat="server" EnableScriptGlobalization="true"
            EnableScriptLocalization="true" AsyncPostBackTimeout="36000">
        </asp:ScriptManager>
        <input type="hidden" value="empty" name="txtAuftragsnummer" />
        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
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
                                <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Werte ändern)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="120">
                                <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                    border="0">
                                    <tr>
                                        <td class="TaskTitle">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" width="150">
                                            <asp:LinkButton ID="cmdSave" runat="server" Visible="False" CssClass="StandardButton">&#149;&nbsp;Sichern</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="TaskTitle" valign="top">
                                            <asp:HyperLink ID="lnkExcel" runat="server" Visible="False" Target="_blank">Excelformat</asp:HyperLink>&nbsp;&nbsp;
                                            <asp:Label ID="lblDownloadTip" runat="server" Visible="False" Font-Bold="True" Font-Size="8pt">rechte Maustaste => Ziel speichern unter...</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <uc1:kopfdaten id="Kopfdaten1" runat="server">
                                            </uc1:kopfdaten>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td class="LabelExtraLarge" align="left">
                                                        <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label><asp:Label ID="lblError"
                                                            runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <img alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" />
                                                        <asp:LinkButton ID="lnkCreateExcel" CssClass="ExcelButton" runat="server" Visible="False"><strong>Excelformat</strong> </asp:LinkButton>
                                                        &nbsp;<strong>Anzahl Vorgänge / Seite</strong>
                                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" PageSize="50" bodyHeight="400"
                                                CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn SortExpression="Auftragsnummer" HeaderText="col_Auftragsnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Auftragsnummer" runat="server" CommandName="Sort" CommandArgument="Auftragsnummer">col_Auftragsnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Auftragsnummer") %>'
                                                                ID="lblAuftragsnummer">
                                                            </asp:Label>
                                                       
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Vertragsnummer" CommandName="sort" CommandArgument="Vertragsnummer"
                                                                runat="server">col_Vertragsnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'
                                                                ID="lblVertragsnummer">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Angefordert am" HeaderText="col_Angefordertam">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Angefordertam" runat="server" CommandName="Sort" CommandArgument="Angefordert am">col_Angefordertam</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Angefordert am", "{0:d}" ) %>'
                                                                ID="Label3">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Briefnummer" HeaderText="col_Briefnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Briefnummer" CommandName="sort" CommandArgument="Briefnummer"
                                                                runat="server">col_Briefnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Briefnummer")%>'
                                                                ID="Label5">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Zustellart" HeaderText="col_Zustellart">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Zustellart" CommandName="sort" CommandArgument="Zustellart"
                                                                runat="server">col_Zustellart</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zustellart")%>'
                                                                ID="lblZustellart">                                                                
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>                                                    
                                                    <asp:TemplateColumn SortExpression="Kontingentart" HeaderText="col_Kontingentart">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Kontingentart" CommandName="sort" CommandArgument="Kontingentart"
                                                                runat="server">col_Kontingentart</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Visible="True" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingentart")%>'
                                                                ID="Label2">                                                                
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Status" HeaderText="col_Gesperrt">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Gesperrt" runat="server" CommandName="Sort" CommandArgument="Status">col_Gesperrt</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'
                                                                ID="Label1">
																                                   
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:ButtonColumn ItemStyle-HorizontalAlign="Center" Text="Storno" CommandName="Storno">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:ButtonColumn>
                                                    <asp:TemplateColumn SortExpression="Kontingent" HeaderText="col_Kontingent">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Kontingent" CommandName="sort" CommandArgument="Kontingent"
                                                                runat="server">col_Kontingent</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kontingent")%>'
                                                                ID="Label6">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn SortExpression="Equinummer" HeaderText="col_Equinummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Equinummer" CommandName="sort" CommandArgument="Equinummer"
                                                                runat="server">col_Equinummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                                ID="lblEquinummer" Visible="False">
                                                            </asp:Label>    
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>                                                                                                        
                                                </Columns>
                                                <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="ShowScript" runat="server" visible="False">
                            <td colspan="2">

                                <script type="text/javascript">
									<!--                                    //
                                    function StornoConfirm(Auftragsnummer, Vertragsnummer, AngefordertAm, Fahrgestellnummer, Briefnummer, Kontingentart) {
                                        var Check = window.confirm("Wollen Sie diesen Versandauftrag wirklich stornieren?\n\tAngefordert am:\t" + AngefordertAm + "\n\tFahrgestellnr.:\t" + Fahrgestellnummer + "\t\n\tNummer ZBII:\t" + Briefnummer);
                                        if (Check) {
                                            window.document.form1.txtAuftragsnummer.value = Auftragsnummer;
                                        }
                                        return (Check);
                                    }
									//-->
                                </script>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" align="left">
                                <!--#include File="../../../PageElements/Footer.html" -->
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Timer ID="timerHidePopup" runat="server" Interval="2000" Enabled="false"></asp:Timer>
        <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnFake"
            PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="false">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="mb" runat="server" BackColor="White" Width="300" Height="66"
            Style="display: none; border: solid 2px #000000">
            <div style="padding-top: 10px; padding-bottom: 5px; text-align: center">
                <%--Meldungstext kann per Feldübersetzung individuell angepasst werden--%>
                <asp:Label ID="lbl_AuftragStornoErfolgreichMessage" runat="server" Text="Versandauftrag erfolgreich gelöscht" Font-Bold="True"></asp:Label>
            </div>
            <table width="200" align="center">
                <tr>
                    <td>
                        Auftragsnummer:
                    </td>
                    <td>
                        <asp:Label ID="lblStornoDetailsAuftragsnummer" runat="server"></asp:Label>
                    </td>
                </tr>           
            </table>
        </asp:Panel>
    </form>
</body>
</html>

