<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Performance2.aspx.vb" Inherits="CKG.Admin.Performance2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" onload="window.setInterval('ReloadThis()',15000)" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server">Administration</asp:label>
									<asp:label id="lblPageTitle" runat="server"> (Allgemeine Leistungsangaben)</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table5" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width=""><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									&nbsp;<br>
									<TABLE id="Table1" cellSpacing="0" cellPadding="0" bgColor="white" border="0">
										<TR>
											<TD width="150">Kategorie:</TD>
											<TD align="left" colSpan="2"><asp:label id="lblCategoryName" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD width="150">Bezeichnung:</TD>
											<TD align="left" colSpan="2"><asp:label id="lblCounterName" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD width="150">Instanz:</TD>
											<TD align="left" colSpan="2"><asp:label id="lblInstanceName" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD width="150"></TD>
											<TD align="left" colSpan="2"></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150">Wert (
												<asp:label id="lblCounterUnit" runat="server"></asp:label>):</TD>
											<TD vAlign="top" align="left" width="200">
												<asp:label id="lblValue" runat="server"></asp:label></TD>
											<TD vAlign="top" align="right" width="200">
												<TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0">
													<TR>
														<TD noWrap>Betrachteter Zeitraum:&nbsp;</TD>
														<TD noWrap><asp:dropdownlist id="ddlPageSize" runat="server"></asp:dropdownlist></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD width="150">&nbsp;&nbsp;&nbsp;</TD>
											<TD align="left" width="200">&nbsp;&nbsp;&nbsp;
											</TD>
											<TD align="left" width="200">&nbsp;&nbsp;&nbsp;</TD>
										</TR>
										<TR>
											<TD width="150"></TD>
											<TD align="left" width="200">Minimum</TD>
											<TD align="right" width="200">Maximum</TD>
										</TR>
										<TR>
											<TD width="150">&nbsp;</TD>
											<TD align="left" width="200"><asp:label id="lblMin" runat="server"></asp:label></TD>
											<TD align="right" width="200"><asp:label id="lblMax" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="550" bgColor="white" border="1">
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label1" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater1" runat="server">
													<ItemTemplate>
														<asp:Image id=Image1 runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label2" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater2" runat="server">
													<ItemTemplate>
														<asp:Image id="Image2" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label3" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater3" runat="server">
													<ItemTemplate>
														<asp:Image id="Image3" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label4" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater4" runat="server">
													<ItemTemplate>
														<asp:Image id="Image4" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label5" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater5" runat="server">
													<ItemTemplate>
														<asp:Image id="Image5" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label6" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater6" runat="server">
													<ItemTemplate>
														<asp:Image id="Image6" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label7" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater7" runat="server">
													<ItemTemplate>
														<asp:Image id="Image7" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label8" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater8" runat="server">
													<ItemTemplate>
														<asp:Image id="Image8" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label9" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater9" runat="server">
													<ItemTemplate>
														<asp:Image id="Image9" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label10" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater10" runat="server">
													<ItemTemplate>
														<asp:Image id="Image10" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label11" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater11" runat="server">
													<ItemTemplate>
														<asp:Image id="Image11" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label12" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater12" runat="server">
													<ItemTemplate>
														<asp:Image id="Image12" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label13" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater13" runat="server">
													<ItemTemplate>
														<asp:Image id="Image13" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label14" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater14" runat="server">
													<ItemTemplate>
														<asp:Image id="Image14" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label15" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater15" runat="server">
													<ItemTemplate>
														<asp:Image id="Image15" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label16" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater16" runat="server">
													<ItemTemplate>
														<asp:Image id="Image16" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label17" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater17" runat="server">
													<ItemTemplate>
														<asp:Image id="Image17" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label18" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater18" runat="server">
													<ItemTemplate>
														<asp:Image id="Image18" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label19" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater19" runat="server">
													<ItemTemplate>
														<asp:Image id="Image19" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label20" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater20" runat="server">
													<ItemTemplate>
														<asp:Image id="Image20" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label21" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater21" runat="server">
													<ItemTemplate>
														<asp:Image id="Image21" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label22" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater22" runat="server">
													<ItemTemplate>
														<asp:Image id="Image22" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label23" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater23" runat="server">
													<ItemTemplate>
														<asp:Image id="Image23" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="150"><asp:label id="Label24" runat="server"></asp:label></TD>
											<TD><asp:repeater id="Repeater24" runat="server">
													<ItemTemplate>
														<asp:Image id="Image24" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/green.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><asp:label id="lblError" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD>&nbsp;</TD>
								<TD vAlign="top" align="left"><!--#include File="../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<script language="javascript">
		<!--
			function ReloadThis() {
				var theform = document.Form1;
				theform.submit();
			}
		// -->
		</script>
	</body>
</HTML>
