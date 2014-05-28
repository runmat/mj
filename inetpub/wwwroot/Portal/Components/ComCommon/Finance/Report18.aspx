<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page  Language="vb" AutoEventWireup="false" Codebehind="Report18.aspx.vb" Inherits="CKG.Components.ComCommon.Report18" %>

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
    
        <uc1:BusyIndicator ID="BusyIndicator1" runat="server" />
    
		<form id="Form1" method="post" runat="server">
		<asp:ScriptManager ID="scriptmanager1" runat="server" EnableScriptGlobalization="true" ></asp:ScriptManager>
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
								<TD vAlign="top" width="59">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center">
                                                <asp:LinkButton ID="lb_Weiter" OnClientClick="Show_BusyBox1();" Text="Weiter" runat="server"
                                                    CssClass="StandardButton"></asp:LinkButton></TD>
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
                                                   
													
													<TR id="tr_EinAus" runat="server">
														<TD class="TextLarge" vAlign="center" width="150"></TD>
														<TD class="TextLarge" vAlign="center"><asp:radiobutton id="rbEin" runat="server" Text="Eingänge" GroupName="grpAction" Checked="True"></asp:radiobutton>&nbsp;
															<asp:radiobutton id="rbAus" runat="server" Text="Ausgänge" GroupName="grpAction"></asp:radiobutton>&nbsp;
															<asp:radiobutton id="rb_alleTemp" runat="server" Text="rb_alleTemp" GroupName="grpAction"></asp:radiobutton></TD>
													</TR>
													<TR id="tr_Versand" runat="server">
														<TD class="TextLarge" vAlign="center" width="150"><asp:label id="lbl_Versand" runat="server" EnableViewState="False"></asp:label></TD>
														<TD class="TextLarge" vAlign="center"><asp:radiobutton id="rb_alle" runat="server" Text="alle" GroupName="grpVersand" Checked="True"></asp:radiobutton><asp:radiobutton id="rb_Temp" runat="server" Text="temporär" GroupName="grpVersand"></asp:radiobutton><asp:radiobutton id="rb_end" runat="server" Text="endgültig" GroupName="grpVersand"></asp:radiobutton></TD>
													</TR>
                                                    <tr id="tr_Referenznummer" runat="server">
                                                        <td class="TextLarge" valign="center" width="150">
                                                            <asp:Label ID="lbl_Referenznummer" runat="server" EnableViewState="False">lbl_Referenznummer</asp:Label>
                                                        </td>
                                                        <td class="TextLarge" valign="center">
                                                           <asp:TextBox ID="txtReferenznummer" Width="130px" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trInfo" runat="server">
                                                        <td colspan="2" class="TextLarge" valign="center" >
                                                            <asp:Label ID="lbl_Info" runat="server" EnableViewState="False"></asp:Label>
                                                        </td>
                                                        
                                                    </tr>
                                                    
												</TABLE>
												<BR>
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
							<SCRIPT language="JavaScript">										
							<!--	
								function Set_VersandTrue()
								{		
									o = document.getElementById("tr_Versand").style;	

									if (document.Form1.rbAus.checked==true){		
											o.display = "";

										} else {																						
											o.display = "none";	
									}	
									Set_Datums('True');				
								}	
									function Set_VersandFalse()
								{					
									o = document.getElementById("tr_Versand").style;	

									if (document.Form1.rbEin.checked==true){		
											o.display = "none";
										} else {																						
											o.display = "";	
											}	
											Set_Datums('True');					
								}		
									function Set_Datums(sichtbarkeit)
								{		
										
										var o=document.getElementById("tr_Datumab").style;
										var o2=document.getElementById("tr_Datumbis").style;
										
										if (sichtbarkeit=='True')
										{
											o.display="";
											o2.display="";
																					}
										else
										{
											var o3 = document.getElementById("tr_Versand").style;
											o.display="none";
											o2.display="none";
											o3.display = "none";
											
										}										
								}							
								
																																										
							-->
							</SCRIPT>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
