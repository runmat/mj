<%@ Page Language="vb"  AutoEventWireup="false" CodeBehind="Report02_1.aspx.vb" Inherits="AppF1.Report02_1"   %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>




<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD runat="server">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
    
        <uc1:BusyIndicator runat="server" />
    
		<form id="Form1" method="post" runat="server">
		<asp:ScriptManager ID="scriptmanager1" runat="server" EnableScriptLocalization="true" EnableScriptGlobalization="true" ></asp:ScriptManager>
			  
                            
                            
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" noWrap colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> - Zusammenstellung von Abfragekriterien</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdCreate" runat="server" OnClientClick="Show_BusyBox1();" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top" width="100%">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
													<TR id="tr_Datumab" runat="server">
														<TD class="TextLarge" vAlign="top" width="150">
															<asp:label id="lbl_Datumab" runat="server" EnableViewState="False"></asp:label>
														</TD>
														<TD class="TextLarge" vAlign="center"><asp:textbox id="txtAbDatum" runat="server" Width="130px"></asp:textbox>
														<cc1:CalendarExtender ID="CEAbDatum"  Format="dd.MM.yyyy" runat="server" TargetControlID="txtAbDatum">
                                                            </cc1:CalendarExtender>
														</TD>
													</TR>
													<TR id="tr_Datumbis" runat="server">
														<TD class="StandardTableAlternate" vAlign="top" width="150">
															<asp:label id="lbl_Datumbis" runat="server" EnableViewState="False"></asp:label></TD>
														<TD class="StandardTableAlternate" vAlign="center"><asp:textbox id="txtBisDatum" runat="server" Width="130px"></asp:textbox>
														 <cc1:CalendarExtender ID="CEbisDatum" Format="dd.MM.yyyy" runat="server" TargetControlID="txtBisDatum">
                                                            </cc1:CalendarExtender>
														
														</TD>
															</TR>
													<TR id="tr_Auswahl" runat="server">
														<TD class="TextLarge" vAlign="top" width="150">
															<asp:label id="lbl_Abrufgr0" runat="server"  Text="lbl_Abrufauswahl"></asp:label>
														</TD>
														<TD>
														    <asp:RadioButton ID="rb_Temp" runat="server"  GroupName="Auswahl" 
                                                                Text="rb_Temp" AutoPostBack="True" />
                                                            <asp:RadioButton ID="rb_Endg" runat="server" Checked="True" GroupName="Auswahl" 
                                                                Text="rb_Endg" AutoPostBack="True" />
                                                        </TD>
													</TR>
													
													<TR id="tr_Bezahlt" runat="server">
														<TD class="TextLarge" vAlign="top" width="150">
															<asp:label id="lbl_Bezahlt" runat="server"  Text="lbl_Bezahlt"></asp:label>
														</TD>
														<TD>
														    <asp:RadioButton ID="rb_bezahlt" runat="server"  GroupName="Bezahlt" 
                                                                Text="rb_bezahlt" AutoPostBack="false" />
                                                            <asp:RadioButton ID="rb_Unbezahlt" runat="server"  GroupName="Bezahlt" 
                                                                Text="rb_Unbezahlt" AutoPostBack="false" />
                                                                <asp:RadioButton ID="rb_alle" runat="server" Checked="True" GroupName="Bezahlt" 
                                                                Text="rb_alle" AutoPostBack="false" />
                                                        </TD>
													</TR>
													
													<TR id="tr_Abruf" runat="server">
														<TD class="TextLarge" vAlign="top" width="150">
															<asp:label id="lbl_Abrufgr" runat="server"  Text="lbl_Abrufgr"></asp:label>
														</TD>
														<TD>
														    <asp:DropDownList ID="ddlAbrufgrund" runat="server">
                                                            </asp:DropDownList>
                                                        </TD>
													</TR>
													
												</TABLE>
												
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="59" height="18">&nbsp;</TD>
								<TD vAlign="top" height="18"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="59">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
