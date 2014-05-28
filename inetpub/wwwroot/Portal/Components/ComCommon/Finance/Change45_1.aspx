<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change45_1.aspx.vb" Inherits="CKG.Components.ComCommon.Change45_1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
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
      
      
		
      document.forms["Form1"].xCoordHolder.value = scrollX;
      document.forms["Form1"].yCoordHolder.value = scrollY;
     
   }
   
   function sstchur_SmartScroller_Scroll()
   {
  
	initializeTheDiv();
	//alert("bin nach initialize the div");
	scrollTheDiv();
	
      var x = document.forms["Form1"].xCoordHolder.value;
      var y = document.forms["Form1"].yCoordHolder.value;
      window.scrollTo(x, y);
     
   }

	
	function getTheDiv()
   {
   
	if (div)
	{
		//alert("1es wurde ein div gefunden: " + div.scrollTop);
		document.forms["Form1"].divYCoordHolder.value =div.scrollTop;	
		//	alert("der wert nach füllen des CoordHolders: " + document.forms["Form1"].divYCoordHolder.value);		
	}
	   
    }//end getTheDIV
    
    function scrollTheDiv()
    {
    //alert("scrollTheDiv");
   if (div)
   {
	//alert("DIV IST");
   	div.scrollTop=document.forms["Form1"].divYCoordHolder.value;
    }
    		
    }//end scrollTheDiv()
    
    function initializeTheDiv()
    {
    //alert("initializeTheDiv");
    if (!div)
    {
   // alert("div wird Initialisiert");
    div=document.all.tags("div")[0];
    if(div)
    {
    div.onscroll=getTheDiv;
	div.onkeyPress=getTheDiv;
	div.onclick=getTheDiv;
	}
    }
    }//end initializeTheDiv()
   
  
   
   window.onload = sstchur_SmartScroller_Scroll;
   window.onscroll = sstchur_SmartScroller_GetCoords;
   window.onkeypress = sstchur_SmartScroller_GetCoords;
   window.onclick = sstchur_SmartScroller_GetCoords;
   //window.onload=initializeTheDiv; es kann nur ein onload event geben logisch ne?
   //window.onload=scrollTheDiv;
  
         
// -->
			</script>
			<input type="hidden" id="xCoordHolder" runat="server" NAME="xCoordHolder"> <input type="hidden" id="yCoordHolder" runat="server" NAME="yCoordHolder">
			<input type="hidden" id="divYCoordHolder" runat="server" NAME="divYCoordHolder">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td colSpan="2">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Auswählen)</asp:label></td>
							</TR>
							<TR>
								<TD vAlign="top" colSpan="2">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:linkbutton id="lb_Haendlersuche" runat="server"></asp:linkbutton>&nbsp;
												<asp:linkbutton id="lb_Autorisierung" runat="server" Visible="False">Autorisierungsübersicht</asp:linkbutton></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr id="trVorgangsArt" runat="server">
											<td colSpan="2"></td>
											<TD width="37"></TD>
										</tr>
										<TR>
											<TD class="" width="100%"><strong><asp:label id="lblNoData" runat="server" Visible="False"></asp:label></strong><br>
												<strong>
													<asp:label id="Label1" runat="server"></asp:label></strong></TD>
											<TD class="LabelExtraLarge" align="right"></TD>
										</TR>
										<TR id="trPageSize" runat="server">
											<TD class="LabelExtraLarge" align="left" colSpan="2">
											</TD>
											<TD class="LabelExtraLarge" align="right" width="37" height="6"></TD>
										</TR>
										<TR id="trDataGrid1" runat="server">
											<TD align="middle" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" bodyCSS="tableBody" headerCSS="tableHeader" Width="100%" cssclass="tableMain" bodyHeight="400" PageSize="50">
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<Columns>
														<asp:TemplateColumn HeaderText="H&#228;ndlernummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Haendlernummer" CommandArgument="Haendlernummer" CommandName="Sort" Runat="server">col_Haendlernummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id=Label2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Haendlernummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="AnforderungsUser" HeaderText="col_AnforderungsUser">
															<HeaderTemplate>
																<asp:LinkButton id="col_AnforderungsUser" CommandArgument="AnforderungsUser" CommandName="Sort" Runat="server">col_AnforderungsUser</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AnforderungsUser") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="ZB2Nummer" SortExpression="ZB2Nummer" ReadOnly="True" HeaderText="Nummer ZB2"></asp:BoundColumn>
														<asp:TemplateColumn SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
															<HeaderTemplate>
																<asp:LinkButton id="col_Vertragsnummer" CommandArgument="Vertragsnummer" CommandName="Sort" Runat="server">col_Vertragsnummer</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn SortExpression="Fahrzeugklasse" HeaderText="col_Fahrzeugklasse">
															<HeaderTemplate>
																<asp:LinkButton id="col_Fahrzeugklasse" CommandArgument="Fahrzeugklasse" CommandName="Sort" Runat="server">col_Fahrzeugklasse</asp:LinkButton>
															</HeaderTemplate>
															<ItemTemplate>
																<asp:Label id="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrzeugklasse") %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn DataField="Fahrgestellnummer" SortExpression="Fahrgestellnummer" ReadOnly="True" HeaderText="Fahrgestellnummer"></asp:BoundColumn>
														<asp:BoundColumn DataField="Anforderungsdatum" SortExpression="Anforderungsdatum" ReadOnly="True" HeaderText="Anforderungsdatum" DataFormatString="{0:dd.MM.yyyy}"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kontingentart" SortExpression="Kontingentart" ReadOnly="True" HeaderText="Kontingentart"></asp:BoundColumn>
														<asp:BoundColumn DataField="Abrufgrund" SortExpression="Abrufgrund" ReadOnly="True" HeaderText="Abrufgrund"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Storno Grund">
															<ItemTemplate>
																<asp:Label id="Label3" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.StornoGrund") %>'>
																</asp:Label>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id="txtStornoGrund" Runat="server" Width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.StornoGrund")%>' BorderStyle="Solid" BorderWidth="1" BorderColor="Red" TextMode="MultiLine" MaxLength="120" Rows="4">
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderStyle-Width="80" ItemStyle-Width="80">
															<ItemTemplate>
																<asp:LinkButton CssClass="StandardButtonSmall" ID="lbStorno" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR")%>' Runat="server"  tooltip='<%# DataBinder.Eval(Container, "DataItem.versandadresse")%>' CommandName="Storno">Storno</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderStyle-Width="80" ItemStyle-Width="80">
															<ItemTemplate>
																<asp:LinkButton CssClass="StandardButtonSmall" ID="lbFreigabe" Runat="server"  CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR")%>' tooltip='<%# DataBinder.Eval(Container, "DataItem.versandadresse")%>' CommandName="Freigabe">Freigabe</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderStyle-Width="80" ItemStyle-Width="80">
															<ItemTemplate>
																<asp:LinkButton CssClass="StandardButtonSmall" ID="lbAuthorisierung" Runat="server" CommandName="Autho">Anfrage autorisieren</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderStyle-Width="80" ItemStyle-Width="80">
															<ItemTemplate>
																<asp:LinkButton CssClass="StandardButtonSmall" ID="lbLoeschen" Runat="server" CommandName="Loesch">Anfrage löschen</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="FALSE" DataField="VBELN" HeaderText="VBELN"></asp:BoundColumn>
														<asp:BoundColumn Visible="FALSE" DataField="EQUNR" HeaderText="EQUNR"></asp:BoundColumn>
													</Columns>
												</asp:datagrid></TD>
											<TD align="middle" width="37"></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<td width="148">&nbsp;<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></td>
								<TD vAlign="top" align="left"></TD>
							</TR>
							<TR>
								<td width="148">&nbsp;</td>
								<TD vAlign="top" align="left"><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
