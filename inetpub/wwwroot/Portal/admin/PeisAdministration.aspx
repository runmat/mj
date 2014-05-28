<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PeisAdministration.aspx.vb"
    Inherits="CKG.Admin.PeisAdministration" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
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
                        <td class="PageNavigation" colspan="3">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" width="100%">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120px">
                            &nbsp;
                        </td>
                        <td width="300px">
                            &nbsp;
                        </td>
                        <td width="100%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left" nowrap="nowrap">
                            <b>&nbsp;Peis-Configuration</b> <span lang="de">(Web.config)</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td width="300px">
                            Peis-Status:
                        </td>
                        <td width="100%">
                            <asp:Label runat="server" ID="lblPeisStatus" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td width="300px">
                            Peis-Targetmail:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPeisTargetMail" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left" nowrap="nowrap">
                            <b>&nbsp;Peis-progammatische Filter</b> <span lang="de">(Web.config)</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            SessionTimeOut:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSessionTimeOutFilter" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td nowrap="nowrap">
                            Peis interne Fehler:&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPeisInterneFehlerFilter" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left" nowrap="nowrap">
                            <b>&nbsp;Peis-Filter eintragen</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td nowrap="nowrap" colspan="2">
                            <table>
                                <tr>
                                    <td>
                                        Fehler Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFehlerName" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td>
                                        Fehlerbeschreibung:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFehlerBeschreibung" TextMode="MultiLine" Rows="5" runat="server"
                                            MaxLength="500" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        KeyWord
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtKeyWordInsert" runat="server" Width="200px" MaxLength="100"></asp:TextBox>
                                        &nbsp;<asp:ImageButton ImageAlign="Middle" ID="imgbInsertUpdateDG" runat="server"
                                            Height="20px" ImageUrl="../Images/Insert.gif" Visible="True" Width="20px" />
                                    </td>
                                    <td>
                                        Fehler Beispiel:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFehlerBeispiel" TextMode="MultiLine" Rows="5" runat="server"
                                            MaxLength="500" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        KeyWords
                                    </td>
                                    <td>
                                        <asp:DataGrid ShowHeader="false" runat="server" ID="dgKeyWords" Width="230px" AutoGenerateColumns="false">
                                            <Columns>
                                               
                                                <asp:BoundColumn DataField="KeyWord"></asp:BoundColumn>
                                                <asp:TemplateColumn  ItemStyle-Width="28px"  ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate >
                                                        <asp:LinkButton ID="lbDelete" runat="server" 
                                                            CommandName="Delete" Height="10px">
																		<img src="../Images/loesch.gif" border="0" alt="insert" ></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                               
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right" valign="bottom">
                                        <asp:LinkButton ID="lbInsertFilter" runat="server" CssClass="StandardButton">hinzufügen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table id="TableKleinerAbstandVorGrid" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr class="TextLarge">
                                                <td nowrap="nowrap" valign="bottom" width="15%">
                                                    <b>Peis-Filter</b>
                                                </td>
                                                <td align="left" valign="bottom">
                                                    <asp:Label ID="lblInfo" Font-Bold="True" runat="server"></asp:Label>
                                                </td>
                                                <td align="right" nowrap="nowrap">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panel" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2" Width="100%"
                                            runat="server" Height="100%">
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <p>
                                                            <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp;&nbsp;</p>
                                                        <p>
                                                            &nbsp;</p>
                                                    </td>
                                                    <td align="right">
                                                        <asp:ImageButton ID="imgbExcel" runat="server" Height="20px" ImageUrl="../Images/excel.gif"
                                                            Visible="True" Width="20px" />
                                                        &nbsp;Ergebnisse/Seite:&nbsp;<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="DG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                AllowPaging="True" AllowSorting="True">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn DataField="FilterID" Visible="false"></asp:BoundColumn>
                                                    <asp:TemplateColumn  HeaderText="Filter enabled" SortExpression="FilterEnabled" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="100px" >
                                                                                                            
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkFilterEnabled" OnCheckedChanged="chkFilterEnabled_CheckedChanged"
                                                                CausesValidation="true" AutoPostBack="true" Checked='<%# DataBinder.Eval(Container, "DataItem.FilterEnabled") = "X" %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="FehlerName" HeaderText="Fehler Name" SortExpression="FehlerName">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="FehlerBeschreibung" HeaderText="Beschreibung" SortExpression="FehlerBeschreibung">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="FehlerBeispiel" HeaderText="Beispiel" SortExpression="FehlerBeispiel">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="KeyWords" HeaderText="KeyWords" SortExpression="KeyWords">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="40px" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbDelete" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FilterID") %>'
                                                                CommandName="Delete" Height="10px">
																		<img src="../Images/loesch.gif" border="0"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" valign="top" align="left">
                <!--#include File="../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
