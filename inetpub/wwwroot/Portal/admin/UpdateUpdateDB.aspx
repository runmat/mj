<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UpdateUpdateDB.aspx.vb"
    Inherits="CKG.Admin.UpdateUpdateDB" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:styles id="ucStyles" runat="server">
    </uc1:styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    
    <uc1:BusyIndicator runat="server" />

    <form id="Form1" method="post" runat="server">

    <script language="javascript" id="ScrollPosition">
<!--
		
	function sstchur_SmartScroller_GetCoords()
   {
		
      var scrollX, scrollY;
      
      if (document.all)
      {
         if (!document.documentElement.scrollLeft)
            scrollX = document.body.scrollLeft;
         else
            scrollX = document.documentElement.scrollLeft;
               
         if (!document.documentElement.scrollTop)
            scrollY = document.body.scrollTop;
         else
            scrollY = document.documentElement.scrollTop;
      }   
      else
      {
         scrollX = window.pageXOffset;
         scrollY = window.pageYOffset;
      }
      
      
		
      document.forms["Form1"].xCoordHolder.value = scrollX;
      document.forms["Form1"].yCoordHolder.value = scrollY;
     
   }
   
   function sstchur_SmartScroller_Scroll()
   {
  
			
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
                <uc1:header id="ucHeader" runat="server">
                </uc1:header>
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
                        <td valign="top" width="120px">
                        </td>
                        <td width="100%">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:LinkButton ID="lb_zurueck" Visible="True" runat="server">lb_zurueck</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center" nowrap="nowrap" style="font-size: larger">
                            <span lang="de">Es werden alle Schnittstellenrelevanten <font color="red">Bapiänderungen
                                seit dem</font>
                                <br />
                                &nbsp;letzten Updatelauf ( </span>
                            <asp:Label ID="lblLastUpdateDate" ForeColor="Red" Font-Bold="True" runat="server"></asp:Label>
                            <span lang="de">&nbsp;) </span>von
                            <br />
                            WebUser
                            <asp:Label ID="lblLastUpdateWebUser" Font-Bold="True" runat="server"></asp:Label>
                            <br />
                            <span lang="de">ermittelt</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center" nowrap="nowrap">
                            &nbsp;<span lang="de"><asp:ImageButton OnClientClick="Show_BusyBox1();" ID="imgbUpdate"
                                runat="server" Height="30px" ImageUrl="../Images/refresh.gif" Visible="True"
                                Width="30px" />
                            </span>
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
                                                    <span lang="de">Updated-Bapis</span>
                                                </td>
                                                <td align="left" valign="bottom">
                                                    <asp:Label ID="lblUpdatedBapisInfo" Font-Bold="True" runat="server"></asp:Label>
                                                </td>
                                                <td align="right" nowrap="nowrap">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panelUpdatedBapis" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2"
                                            Width="100%" runat="server" Height="100%">
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        <p>
                                                            <asp:Label ID="lblUpdatedBapisNoData" runat="server" Visible="False"></asp:Label>&nbsp;<asp:Label
                                                                ID="lblUpdatedBapisError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>&nbsp;</p>
                                                        <p>
                                                            &nbsp;</p>
                                                    </td>
                                                    <td align="right">
                                                        <asp:ImageButton ID="imgbUpdatedBapisExcel" runat="server" Height="20px" ImageUrl="../Images/excel.gif"
                                                            Visible="False" Width="20px" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="UpdatedBapisDG" runat="server" BackColor="White" Width="100%" AutoGenerateColumns="False"
                                                AllowPaging="False" AllowSorting="True">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn SortExpression="BapiName" ItemStyle-Font-Bold="true" HeaderText="Bapi Name"
                                                        DataField="BapiName"></asp:BoundColumn>
                                                    <asp:BoundColumn SortExpression="BapiLastChangeSap" HeaderText="Letzte SAP-Änderung"
                                                        DataFormatString="{0:dd.MM.yyyy}"  DataField="BapiLastChangeSap">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn SortExpression="LastSapDeveloper" HeaderText="Letzte SAP-Änderer"
                                                        DataField="LastSapDeveloper"></asp:BoundColumn>
                                                
                                                    <asp:BoundColumn HeaderText="Resultat" DataField="RESULT"></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
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
