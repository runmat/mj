<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%--<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../PageElements/Kopfdaten.ascx" %>--%>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_1.aspx.vb" Inherits="AppBPLG.Change04_1" %>

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
        var div;

        function sstchur_SmartScroller_GetCoords() {

            var scrollX, scrollY;



            if (document.all) {

                if (!document.documentElement.scrollLeft) {
                    scrollX = document.body.scrollLeft;

                }
                else {
                    scrollX = document.documentElement.scrollLeft;

                }
                if (!document.documentElement.scrollTop) {
                    scrollY = document.body.scrollTop;

                }
                else {
                    scrollY = document.documentElement.scrollTop;

                }
            }
            else {
                scrollX = window.pageXOffset;
                scrollY = window.pageYOffset;
            }



            document.forms["Form1"].xCoordHolder.value = scrollX;
            document.forms["Form1"].yCoordHolder.value = scrollY;

        }

        function sstchur_SmartScroller_Scroll() {

            initializeTheDiv();
            //alert("bin nach initialize the div");
            scrollTheDiv();

            var x = document.forms["Form1"].xCoordHolder.value;
            var y = document.forms["Form1"].yCoordHolder.value;
            window.scrollTo(x, y);

        }


        function getTheDiv() {

            if (div) {
                //alert("1es wurde ein div gefunden: " + div.scrollTop);
                document.forms["Form1"].divYCoordHolder.value = div.scrollTop;
                //	alert("der wert nach füllen des CoordHolders: " + document.forms["Form1"].divYCoordHolder.value);		
            }

        } //end getTheDIV

        function scrollTheDiv() {
            //alert("scrollTheDiv");
            if (div) {
                //alert("DIV IST");
                div.scrollTop = document.forms["Form1"].divYCoordHolder.value;
            }

        } //end scrollTheDiv()

        function initializeTheDiv() {
            //alert("initializeTheDiv");
            if (!div) {
                // alert("div wird Initialisiert");
                div = document.all.tags("div")[0];
                if (div) {
                    div.onscroll = getTheDiv;
                    div.onkeyPress = getTheDiv;
                    div.onclick = getTheDiv;
                }
            }
        } //end initializeTheDiv()



        window.onload = sstchur_SmartScroller_Scroll;
        window.onscroll = sstchur_SmartScroller_GetCoords;
        window.onkeypress = sstchur_SmartScroller_GetCoords;
        window.onclick = sstchur_SmartScroller_GetCoords;
        //window.onload=initializeTheDiv; es kann nur ein onload event geben logisch ne?
        //window.onload=scrollTheDiv;
  
         
// -->
    </script>
    <input type="hidden" id="xCoordHolder" runat="server" name="xCoordHolder" />
    <input type="hidden" id="yCoordHolder" runat="server" name="yCoordHolder" />
    <input type="hidden" id="divYCoordHolder" runat="server" name="divYCoordHolder" />
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Auswählen)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:LinkButton ID="lb_Haendlersuche" runat="server"></asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="lb_Autorisierung" runat="server" Visible="False">Autorisierungsübersicht</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr runat="server">
                                    <td colspan="2">
                                    </td>
                                    <td width="37">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" width="100%">
                                        <strong>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label></strong><br>
                                        <strong>
                                            <asp:Label ID="Label1" runat="server"></asp:Label></strong>
                                    </td>
                                    <td class="LabelExtraLarge" align="right">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr id="trPageSize" runat="server">
                                    <td align="left" colspan="2">
                                        <asp:LinkButton CssClass="StandardButton" ID="btnMassenfreigabe" runat="server" OnClientClick="return window.confirm(&quot;alle Ausgewählten Vorgänge werden freigegeben&quot;);"
                                            Text="Massenfreigabe" />
                                    </td>
                                    <td class="LabelExtraLarge" align="right" width="37" height="6">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr id="trDataGrid1" runat="server">
                                    <td align="middle" colspan="2">
                                        <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False"
                                            AllowPaging="True" AllowSorting="True" bodyCSS="tableBody" headerCSS="tableHeader"
                                            Width="100%" CssClass="tableMain" bodyHeight="400" PageSize="50">
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Endkundennummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Endkundennummer" CommandArgument="Endkundennummer" CommandName="Sort"
                                                            runat="server">col_Endkundennummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Endkundennummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="AnforderungsUser" HeaderText="col_AnforderungsUser">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_AnforderungsUser" CommandArgument="AnforderungsUser" CommandName="Sort"
                                                            runat="server">col_AnforderungsUser</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AnforderungsUser") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" CommandArgument="Vertragsnummer" CommandName="Sort"
                                                            runat="server">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer"
                                                    ReadOnly="True" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Anforderungsdatum" SortExpression="Anforderungsdatum"
                                                    ReadOnly="True" HeaderText="Anforderungsdatum" DataFormatString="{0:dd.MM.yyyy}">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Abrufart" SortExpression="Abrufart" ReadOnly="True" HeaderText="Abrufart">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Abrufgrund" SortExpression="Abrufgrund" ReadOnly="True"
                                                    HeaderText="Abrufgrund"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Storno Grund">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.StornoGrund") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtStornoGrund" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.StornoGrund")%>'
                                                            BorderStyle="Solid" BorderWidth="1" BorderColor="Red" TextMode="MultiLine" MaxLength="120"
                                                            Rows="4">
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderStyle-Width="80" ItemStyle-Width="80">
                                                    <ItemTemplate>
                                                        <asp:LinkButton CssClass="StandardButtonSmall" ID="lbStorno" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR")%>'
                                                            runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.versandadresse")%>'
                                                            CommandName="Storno">Storno</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderStyle-Width="80" ItemStyle-Width="80">
                                                    <ItemTemplate>
                                                        <asp:LinkButton CssClass="StandardButtonSmall" ID="lbFreigabe" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR")%>'
                                                            ToolTip='<%# DataBinder.Eval(Container, "DataItem.versandadresse")%>' CommandName="Freigabe">Freigabe</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Massenfreigabe" HeaderText="col_Auswahl" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Auswahl" CommandArgument="Massenfreigabe" CommandName="Sort"
                                                            runat="server">col_Auswahl</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkMassenfreigabeAuswahl" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Massenfreigabe") = "X" %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="False" HeaderStyle-Width="80" ItemStyle-Width="80">
                                                    <ItemTemplate>
                                                        <asp:LinkButton CssClass="StandardButtonSmall" ID="lbAuthorisierung" runat="server"
                                                            CommandName="Autho">Anfrage autorisieren</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn Visible="False" HeaderStyle-Width="80" ItemStyle-Width="80">
                                                    <ItemTemplate>
                                                        <asp:LinkButton CssClass="StandardButtonSmall" ID="lbLoeschen" runat="server" CommandName="Loesch">Anfrage löschen</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn Visible="FALSE" DataField="VBELN" HeaderText="VBELN"></asp:BoundColumn>
                                                <asp:BoundColumn Visible="FALSE" DataField="EQUNR" HeaderText="EQUNR"></asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                    <td align="middle" width="37">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="148">
                            &nbsp;<asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                        </td>
                    </tr>
                    <tr>
                        <td width="148">
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
    </form>
</body>
</html>
