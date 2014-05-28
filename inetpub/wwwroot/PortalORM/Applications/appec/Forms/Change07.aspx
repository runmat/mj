<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change07.aspx.vb" Inherits="AppEC.Change07" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2">
									<asp:Label id="lblHead" runat="server"></asp:Label>
								</TD>
							</TR>
							<TR>
								<TD class="TaskTitle" colSpan="2">Bitte geben Sie die Auswahlkriterien ein.</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="" vAlign="top">
												<TABLE class="BorderLeftBottom" id="Table5" cellSpacing="1" cellPadding="1" width="300" border="0">
													<TR>
														<TD>&nbsp;</TD>
														<TD colSpan="3">
															<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
													</TR>
													<TR>
														<TD>&nbsp;</TD>
														<TD>
															Auswahl:</TD>
														<TD>
															<asp:RadioButtonList ID="rblAuswahl" runat="server" 
                                                                RepeatDirection="Horizontal" >
                                                                <asp:ListItem Selected="True" Value="U">Unfallmeldung</asp:ListItem>
                                                                <asp:ListItem Value="D">Diebstahl</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </TD>
														<TD>&nbsp;</TD>
													</TR>
													<TR>
														<TD></TD>
														<TD>
															<P>Amtl. Kennzeichen:</P>
														</TD>
														<TD>
															<asp:TextBox id="txtAmtlKennzeichen" runat="server"></asp:TextBox><FONT color="red">
                                                            <asp:image id="Image1" runat="server" 
                                                                ToolTip="Platzhaltersuche mit *. Minimum 7 Stellen. Beispiel: HH-AB71*" 
                                                                ImageUrl="/PortalORM/Images/info.gif"></asp:image></FONT></TD>
														<TD>&nbsp;</TD>
													</TR>
													<TR>
														<TD></TD>
														<TD>Fahrgestellnummer:</TD>
														<TD>
															<asp:TextBox id="txtFahrgestellnummer" runat="server" MaxLength="17"></asp:TextBox><FONT color="red">
                                                            <asp:image id="Image2" runat="server" 
                                                                ToolTip="Platzhaltersuche mit *. Minimum 8 Stellen. Beispiel: WAUZZZ8E*" 
                                                                ImageUrl="/PortalORM/Images/info.gif"></asp:image></FONT></TD>
														<TD>&nbsp;</TD>
													</TR>
													<TR>
														<TD></TD>
														<TD>Briefnummer:
														</TD>
														<TD>
															<asp:TextBox id="txtBriefnummer" runat="server"></asp:TextBox><FONT color="red">
                                                            <asp:image id="Image3" runat="server" 
                                                                ToolTip="Platzhaltersuche mit *. Minimum 4 Stellen. Beispiel: DH9014*" 
                                                                ImageUrl="/PortalORM/Images/info.gif"></asp:image></FONT></TD>
														<TD>&nbsp;</TD>
													</TR>
													<TR>
														<TD></TD>
														<TD>Unit-Nr.</TD>
														<TD>
															<asp:TextBox id="txtOrdernummer" runat="server"></asp:TextBox><FONT color="red">
                                                            <asp:image id="Image4" runat="server" 
                                                                ToolTip="Platzhaltersuche mit *. Minimum 6 Stellen. Beispiel: 763693*" 
                                                                ImageUrl="/PortalORM/Images/info.gif"></asp:image></FONT></TD>
														<TD>&nbsp;</TD>
													</TR>
													<TR>
														<TD>&nbsp;</TD>
														<TD colspan="2"></TD>
														<TD></TD>
													</TR>
													<TR>
														<TD></TD>
														<TD></TD>
														<TD></TD>
														<TD>
															<asp:LinkButton id="cmdCreate" runat="server" CssClass="StandardButton"> •&nbsp;Suchen</asp:LinkButton></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top">&nbsp;</TD>
							</TR>
							
							
							<tr>
								<TD vAlign="top">&nbsp;</TD>
								<td>
                                    
                                    <asp:GridView ID="gvSelectOne" runat="server" AllowSorting="false" 
                                        AutoGenerateColumns="False" BackColor="White" Width="100%">
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <HeaderStyle CssClass="GridTableHead" ForeColor="White" Wrap="False" />
                                        <Columns>
                                            <asp:BoundField DataField="EQUNR" HeaderText="EQUNR" ReadOnly="true" 
                                                Visible="false" />
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbWeiter" runat="server" 
                                                        CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>' 
                                                        CommandName="weiter" CssClass="StandardButton" Text="Weiter"> </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Kennzeichen" SortExpression="LICENSE_NUM">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LICENSE_NUM" runat="server" 
                                                        CommandArgument="LICENSE_NUM" CommandName="sort">Kennzeichen</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKennzeichen" runat="server" 
                                                        Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>' Visible="true"> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fahrgestellnummer" 
                                                SortExpression="CHASSIS_NUM">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="CHASSIS_NUM" runat="server" 
                                                        CommandArgument="CHASSIS_NUM" CommandName="sort">Fahrgestellnummer</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFahrgestellnummer" runat="server" 
                                                        Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>' 
                                                        Visible="true"> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Briefnummer" 
                                                SortExpression="TIDNR">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="TIDNR" runat="server" 
                                                        CommandArgument="TIDNR" CommandName="sort">Briefnummer</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBriefnummer" runat="server" 
                                                        Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>' 
                                                        Visible="true"> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit Nr." 
                                                SortExpression="ZZREFERENZ1">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="ZZREFERENZ1" runat="server" 
                                                        CommandArgument="ZZREFERENZ1" CommandName="sort">Unit Nr.</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnitnummer" runat="server" 
                                                        Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>' 
                                                        Visible="true"> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    
                                </TD>
							</TR>
							
							<tr>
							<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" -->
							</tr>
							
							
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>