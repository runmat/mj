<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change53_1.aspx.vb" Inherits="CKG.Components.ComCommon.Change53_1" %>

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
                                        <asp:LinkButton ID="lb_back" runat="server"></asp:LinkButton>&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr id="trVorgangsArt" runat="server">
                                    <td colspan="2">
                                    </td>
                                    <td width="37">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" width="100%">
                                        <strong>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label></strong><br />
                                        <strong>
                                            <asp:Label ID="Label1" runat="server"></asp:Label></strong>
                                    </td>
                                    <td class="LabelExtraLarge" align="right">
                                    </td>
                                </tr>
                                <tr id="trPageSize" runat="server">
                                    <td align="left" colspan="2">
                                        <asp:LinkButton ID="lbLoeschen" runat="server" Text="löschen" CssClass="standardButton" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr runat="server">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
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
                                                <asp:BoundColumn DataField="Index" ReadOnly="True" Visible="false"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="col_Vertragsnummer" SortExpression="Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" CommandArgument="Vertragsnummer" CommandName="Sort"
                                                            runat="server">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Haendlernummer" HeaderText="col_Haendlernummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Haendlernummer" CommandArgument="Haendlernummer" CommandName="Sort"
                                                            runat="server">col_Haendlernummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Haendlernummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer"
                                                    ReadOnly="True" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="ZB2Nummer" HeaderText="col_ZB2Nummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ZB2Nummer" CommandArgument="ZB2Nummer" CommandName="Sort"
                                                            runat="server">col_ZB2Nummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblZB2Nummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZB2Nummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Versandart" HeaderText="col_Versandart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandart" CommandArgument="Versandart" CommandName="Sort"
                                                            runat="server">col_Versandart</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label51" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandart") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" CommandArgument="Kennzeichen" CommandName="Sort"
                                                            runat="server">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Fehlertext" SortExpression="Fehlertext" ReadOnly="True"
                                                    HeaderText="Fehlertext"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="auswählen" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chbLoeschen" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
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
