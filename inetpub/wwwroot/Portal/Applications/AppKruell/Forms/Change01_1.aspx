<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_1.aspx.vb" Inherits="AppKruell.Change01_1" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
	
	function sstchur_SmartScroller_GetCoords()
   {
	
      var scrollX, scrollY;
     
      
      
      if (document.all)
      {
		
         if (!document.documentElement.scrollLeft)
            {
            scrollX = document.body.scrollLeft;
           
            }
         else
         {
            scrollX = document.documentElement.scrollLeft;
            
         }     
         if (!document.documentElement.scrollTop)
            {
            scrollY = document.body.scrollTop;
           
            }
         else
         {
            scrollY = document.documentElement.scrollTop;
             
         }
      }   
      else
      {
         scrollX = window.pageXOffset;
         scrollY = window.pageYOffset;
      }
      
      
	//	alert("i have the coordinates: " +scrollX  + " "  +scrollY);
      document.forms["Form1"].xCoordHolder.value = scrollX;
      document.forms["Form1"].yCoordHolder.value = scrollY;
     
   }
   
   function sstchur_SmartScroller_Scroll()
   {
  
	initializeTheDiv();
//	alert("bin nach initialize the div");
	scrollTheDiv();
	
      var x = document.forms["Form1"].xCoordHolder.value;
      var y = document.forms["Form1"].yCoordHolder.value;
      window.scrollTo(x, y);
     
   }

	
	function getTheDiv()
   {
   
	if (div)
	{
	//	alert("1es wurde ein div gefunden: " + div.scrollTop);
		document.forms["Form1"].divYCoordHolder.value =div.scrollTop;	
		//alert("der wert nach füllen des CoordHolders: " + document.forms["Form1"].divYCoordHolder.value);		
	}
	   
    }//end getTheDIV
    
    function scrollTheDiv()
    {
 //   alert("scrollTheDiv");
   if (div)
   {
	//alert("DIV IST: scroll div to" +document.forms["Form1"].divYCoordHolder.value );
   	div.scrollTop=document.forms["Form1"].divYCoordHolder.value;
    }
    		
    }//end scrollTheDiv()
    
    function initializeTheDiv()
    {
    //alert("initializeTheDiv");
    if (!div)
    {
    //alert("div wird Initialisiert");
    
    //------------------------
    //bei IE 7
    //div=document.all.tags("div")[1];
    
     //bei IE 6
    div=document.all.tags("div")[0];
    //------------------------
    div.onscroll=getTheDiv;
	div.onkeyPress=getTheDiv;
	div.onclick=getTheDiv;
	

    }
    }//end initializeTheDiv()
   
    
   window.onload = sstchur_SmartScroller_Scroll;
   window.onscroll = sstchur_SmartScroller_GetCoords;
   window.onkeypress = sstchur_SmartScroller_GetCoords;
   window.onclick = sstchur_SmartScroller_GetCoords;
          
           // --></script>
 <input type="hidden" id="xCoordHolder" runat="server" name="xCoordHolder">
    <input type="hidden" id="yCoordHolder" runat="server" name="yCoordHolder">
    <input type="hidden" id="divYCoordHolder" runat="server" name="divYCoordHolder">
    
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td colspan="3">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="3">
                <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="PageNavigation" colspan="3">
                                <asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server">(Anzeige)</asp:label>
                            </td>
                            <tr>
                                <td class="TaskTitle" valign="top" colspan="3">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" width="117">
                <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                    border="0">
                    <tr>
                        <td width="157">
                            <p>
                                <asp:linkbutton id="lb_Senden" runat="server" cssclass="StandardButton"> &#149;&nbsp;Daten senden</asp:linkbutton><br>
                                <asp:linkbutton id="lb_neuerAuftrag" runat="server" cssclass="StandardButton"> &#149;&nbsp;neuerAuftrag</asp:linkbutton><br>
                                <asp:linkbutton id="lb_Auswahl" runat="server" cssclass="StandardButton"> &#149;&nbsp;zur Auswahl</asp:linkbutton></p>
                        </td>
                    </tr>
                    <tr>
                        <td valign="center" width="157">
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
            <td valign="top">
                <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <p>
                                <asp:label id="lblError" runat="server" cssclass="TextError" enableviewstate="False">
                                </asp:label></p>
                            </asp:label></p>
                            <asp:datagrid id="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False" AllowPaging="False" AllowSorting="True" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%" cssclass="tableMain" bodyHeight="600" >
                                 <alternatingitemstyle cssclass="GridTableAlternate"></alternatingitemstyle>
                                <headerstyle wrap="False" forecolor="White" cssclass="GridTableHead"></headerstyle>
                                <columns>
											<asp:BoundColumn Visible="False" DataField="ROWID" HeaderText="ROWID"></asp:BoundColumn>
											<asp:BoundColumn DataField="NAME_LG" SortExpression="NAME_LG" HeaderText="Leasinggeber"></asp:BoundColumn>
											<asp:BoundColumn DataField="NAME_LN" SortExpression="NAME_LN" HeaderText="Leasingnehmer"></asp:BoundColumn>
											<asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
											<asp:BoundColumn DataField="DAT_ERF_AUFTR" SortExpression="DAT_ERF_AUFTR" HeaderText="Datum der Auftragsanlage"></asp:BoundColumn>
											<asp:BoundColumn DataField="Order_NR" SortExpression="Order_NR" HeaderText="Order Nummer"></asp:BoundColumn>
											<asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Auftrag Status"></asp:BoundColumn>
											<asp:BoundColumn DataField="WEB_USER_FREIG" HeaderText="Freigabe User"></asp:BoundColumn>
											<asp:BoundColumn DataField="DAT_FREIG" HeaderText="Freigabedatum"></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="abgearbeitet">
												<ItemTemplate>
													<P align="center">
														<asp:Linkbutton id="lb_Abgearbeitet" runat="server" Text="<img src=../../../Customize/Kruell/Abgearbeitet_mini.gif border=0>" CommandName="abgearbeitet" visible='<%# DataBinder.Eval(Container, "DataItem.WEB_USER_FREIG")<>"" AND DataBinder.Eval(Container, "DataItem.ABGEARB")=""  %>' Enabled='<%# DataBinder.Eval(Container, "DataItem.WEB_USER_FREIG")<>"" AND DataBinder.Eval(Container, "DataItem.ABGEARB")="" %>'>
														</asp:Linkbutton></P>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="L&#246;schen">
												<ItemTemplate>
													<P align="center">
														<asp:Linkbutton id="lb_delete" runat="server" Text="<img src=../../../Customize/Kruell/loesch.gif border=0>" CommandName="Delete" visible='<%# DataBinder.Eval(Container, "DataItem.Status")<>"gelöscht" %>' Enabled='<%# DataBinder.Eval(Container, "DataItem.Status")<>"gelöscht" %>'>
														</asp:Linkbutton></P>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Bearbeiten">
												<ItemTemplate>
													<P align="center">
														<asp:Linkbutton id=lb_select runat="server" Height="20" Width="20" CommandName="Select" Enabled='<%# DataBinder.Eval(Container, "DataItem.Status")<>"gelöscht" %>' Text="<img src=../../../Customize/Kruell/table.gif border=0>" visible='<%# DataBinder.Eval(Container, "DataItem.Status")<>"gelöscht" %>'>
															<img src="../../../Customize/Kruell/table.gif" border="0"></asp:Linkbutton></P>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Freigabe">
												<ItemTemplate>
													<P align="center">
														<asp:Linkbutton id=lb_Freigabe runat="server" Text="<img src=../../../Customize/Kruell/Confirm_mini.gif border=0>"  Enabled='<%# DataBinder.Eval(Container, "DataItem.WEB_USER_FREIG")=""%>' Visible='<%# DataBinder.Eval(Container, "DataItem.WEB_USER_FREIG")=""  AND DataBinder.Eval(Container, "DataItem.Status")<>"gelöscht" OR DataBinder.Eval(Container, "DataItem.WEB_USER_FREIG")=" " AND DataBinder.Eval(Container, "DataItem.Status")<>"gelöscht"  %>' CommandName="Freigabe" Width="10px" Height="10px">
															<img src="../../../Customize/Kruell/Confirm_mini.gif" border="0"></asp:Linkbutton></P>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Drucken">
												<ItemTemplate>
													<P align="center">
														<asp:LinkButton id="lb_drucken2" runat="server" Text="<img src=../../../Customize/Kruell/printer.jpeg border=0>" Enabled="True" visible='<%# DataBinder.Eval(Container, "DataItem.Status")<>"gelöscht" AND DataBinder.Eval(Container, "DataItem.Status")<>"Auftrag bearbeitet" AND DataBinder.Eval(Container, "DataItem.WEB_USER_FREIG")<>"" %>'    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Order_Nr")%>' CommandName="Drucken" Width="10px" Height="10px">
														</asp:LinkButton></P>
												</ItemTemplate>
											</asp:TemplateColumn>											
											
										</columns>
                            </asp:datagrid>
                            <p>
                                <asp:label id="lbl_SAPResultat" runat="server"></asp:label></p>
                            <asp:datagrid id="Datagrid2" runat="server" allowsorting="False" cssclass="tableMain"
                                bodycss="tableBody" headercss="tableHeader" autogeneratecolumns="False">
                                <alternatingitemstyle cssclass="GridTableAlternate"></alternatingitemstyle>
                                <headerstyle wrap="False" forecolor="White" cssclass="GridTableHead"></headerstyle>
                                <columns>
											<asp:BoundColumn Visible="False" DataField="ROWID" HeaderText="ROWID"></asp:BoundColumn>
											<asp:BoundColumn DataField="NAME_LG" SortExpression="NAME_LG" HeaderText="Leasinggeber"></asp:BoundColumn>
											<asp:BoundColumn DataField="NAME_LN" SortExpression="NAME_LN" HeaderText="Leasingnehmer"></asp:BoundColumn>
											<asp:BoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
											<asp:BoundColumn DataField="DAT_ERF_AUFTR" SortExpression="DAT_ERF_AUFTR" HeaderText="Datum der Auftragsanlage"></asp:BoundColumn>
											<asp:BoundColumn DataField="Order_NR" SortExpression="Order_NR" HeaderText="Order Nummer"></asp:BoundColumn>
											<asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Auftrag Status"></asp:BoundColumn>
											<asp:BoundColumn DataField="WEB_USER_FREIG" HeaderText="Freigabe User"></asp:BoundColumn>
											<asp:BoundColumn DataField="DAT_FREIG" HeaderText="Freigabedatum"></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Drucken">
												<ItemTemplate>
													<P align="center">
														<asp:LinkButton id=lb_drucken runat="server" Text="<img src=../../../Customize/Kruell/printer.jpeg border=0>" Enabled="True" Visible='<%# not DataBinder.Eval(Container, "DataItem.PrintString") is  System.DBNull.Value %>' CommandName="Drucken" Width="10px" Height="10px">
														</asp:LinkButton></P>
												</ItemTemplate>
											</asp:TemplateColumn>
										</columns>
                            </asp:datagrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </TD></TR></TBODY></TABLE></form>
</body>
</html>
