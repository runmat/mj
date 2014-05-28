﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report10_1.aspx.vb" Inherits="AppCommonLeasing.Report10_1" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
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
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" align="center">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top" style="padding-left: 15px">
                                        <asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 15px">
                                        <p>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" EnableViewState="false"></asp:Label><asp:Label
                                                ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label><asp:Label 
                                                ID="lblMessage" Font-Bold="True" runat="server" EnableViewState="False" 
                                                ForeColor="Blue" Font-Size="Larger"></asp:Label>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <table id="TableKleinerAbstandVorGrid" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td style="padding-left: 15px">
                                        <asp:Label ID="lblInfo" Font-Bold="true" runat="server"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:ImageButton ID="imgbExcel" runat="server" Height="20px" ImageUrl="../../../Images/excel.gif"
                                            Visible="True" Width="20px" />
                                        &nbsp;Ergebnisse/Seite:&nbsp;<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 15px">
                                        <asp:DataGrid ID="DataGrid1" runat="server" BackColor="White" PageSize="50" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                 <asp:TemplateColumn HeaderText="col_CocKeinePflicht" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_CocKeinePflicht" runat="server" CommandName="Sort" CommandArgument="Bezahlt">col_CocKeinePflicht</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEQUNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' Visible="false">
                                                        </asp:Label><asp:CheckBox ID="chkBezahlt" runat="server" Checked='<%# not typeof (DataBinder.Eval(Container, "DataItem.Bezahlt")) is System.DBNull %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="col_Objektnummer">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Objektnummer" runat="server" CommandName="Sort" CommandArgument="Objektnummer">col_Objektnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblObjektnummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Objektnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn> 
                                                <asp:TemplateColumn HeaderText="col_Kennzeichen">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblKennzeichen" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>                                                                                             
                                                <asp:TemplateColumn HeaderText="col_Fahrgestellnummer">
                                                    
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkFahrgestellnummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                            ToolTip="Anzeige Fahrzeughistorie" Target="_blank">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Eingangam">
                                                   
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Eingangam" runat="server" CommandName="Sort" CommandArgument="Eingangam">col_Eingangam</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEingangam" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangam","{0:dd.MM.yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="col_Erstzulassung">
                                                   
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Erstzulassung" runat="server" CommandName="Sort" CommandArgument="Erstzulassung">col_Erstzulassung</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblErstzulassung" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Erstzulassung","{0:dd.MM.yyyy}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>                                                
                                                <asp:TemplateColumn HeaderText="col_Hersteller">
                                                   
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="Hersteller">col_Hersteller</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHersteller" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Hersteller") %>'>
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
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" valign="top" align="right" style="padding-top:5px;padding-bottom:5px">
               <asp:LinkButton ID="lb_Save" CssClass="StandardButton" runat="server">Speichern</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="3" valign="top" align="left">
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
