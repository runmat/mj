<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03_1.aspx.vb" Inherits="AppKroschke.Change03_1" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <title></title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body>
    
    <uc1:BusyIndicator ID="BusyIndicator1" runat="server" />

    <form id="Form1" method="post" runat="server">

    <script language="javascript" type="text/javascript" id="ScrollPosition">
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
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2" height="19">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="2" height="19">
                            &nbsp;<asp:LinkButton ID="lb_zurueck" runat="server" Visible="True">zurück</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                            <table border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblInfo"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid ID="dgAuftraege" CellPadding="4" runat="server" BackColor="White" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="True">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="Fahrer_Status" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="VBELN" Visible="false"></asp:BoundColumn>
                                                <asp:TemplateColumn Visible="true" HeaderText="Auftrag PDF" 
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                           <asp:ImageButton  Width="20px" Height="20px" CommandArgument='<%# Bind("VBELN") %>'
                                                            CommandName="getPDF" runat="server" ToolTip="Auftrag als PDF" ID="imgbPrintPDF" ImageUrl="../../../Images/pdf.gif" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                  <asp:BoundColumn DataField="VDATU"  SortExpression="VDATU" DataFormatString="{0:d}" HeaderText="Wunschlieferdatum">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="VBELN" SortExpression="VBELN" HeaderText="Auftragsnummer">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ZB_POST_CODE1" SortExpression="ZB_POST_CODE1" HeaderText="Von PLZ">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ZB_CITY1" SortExpression="ZB_CITY1" HeaderText="Von Ort">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="WE_POST_CODE1" SortExpression="WE_POST_CODE1" HeaderText="Nach PLZ">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="WE_CITY1" SortExpression="WE_CITY1" HeaderText="Nach Ort">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ZR_POST_CODE1" SortExpression="ZR_POST_CODE1" HeaderText="Rücktour PLZ">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ZR_CITY1" SortExpression="ZR_CITY1" HeaderText="Rücktour Ort">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn ItemStyle-Wrap="false" Visible="true" 
                                                    ItemStyle-HorizontalAlign="Center"   >
                                                    <ItemTemplate>
                                                                                                      
                                                        <asp:LinkButton CssClass="StandardButton" OnClientClick="Show_BusyBox1();" Text="Annehmen" Visible='<%# DataBinder.Eval(Container, "DataItem.Fahrer_Status")="" %>' CommandName="annehmen" runat="server" CommandArgument='<%# Bind("VBELN") %>'
                                                            ID="lbAnnehmen"></asp:LinkButton>
                                                        &nbsp; &nbsp;
                                                        <asp:LinkButton CssClass="StandardButton" OnClientClick="Show_BusyBox1();" Text="Ablehnen" Visible='<%# DataBinder.Eval(Container, "DataItem.Fahrer_Status")="" %>' CommandName="ablehnen" runat="server" CommandArgument='<%# Bind("VBELN") %>'
                                                            ID="lbAblehnen"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                 <asp:BoundColumn DataField="KUNNR_AG" Visible="false"></asp:BoundColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="N&#228;chste Seite" Font-Size="12pt" Font-Bold="True" PrevPageText="Vorherige Seite"
                                                HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                       
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        
                        <td valign="top">
                            <table id="legende"  runat="server" cellspacing="4" cellpadding="4">
                                <tr>
                                    <td style="width: 30; background-color:  #9ACD32 ">
                                    </td>
                                    <td>
                                       Auftrag den Sie angenommen haben.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30; background-color: #FFA300">
                                    </td>
                                    <td>
                                        Auftrag den Sie abgelehnt haben.
                                    </td>
                                </tr>
                               
                            </table>
                        </td>
                    </tr>
                            </table>
                        </td>
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
            </td>
        </tr>
    </table>
            <script type="text/javascript">
                function openinfo(url) {
                    fenster = window.open(url, "", "width=670,height=700,left=20,top=20,resizable=YES, scrollbars=YES,menubar=NO");
                    fenster.focus();
                }
             </script>    
    </form>
</body>
</html>
