<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="_Report02.aspx.vb" Inherits="AppDCL._Report02" %>


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
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<table width="100%" align="center">
				<tr>
					<td height="25"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> :Aufträge bearbeiten</asp:label><asp:hyperlink id="lnkKreditlimit" runat="server" Visible="False" NavigateUrl="Equipment.aspx" CssClass="PageNavigation">Abfragekriterien</asp:hyperlink></td>
							</TR>
							<tr>
								<TD class="StandardTableButtonFrame" vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;<asp:label id="lblDatensatz" runat="server" Visible="False" Font-Bold="True"></asp:label>
												<asp:label id="lblNoData" runat="server" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD class="LabelExtraLarge">
												<TABLE id="Table9" cellSpacing="0" cellPadding="5" width="100%" border="0">
													<TR>
														<TD vAlign="top" align="left" colSpan="2"><TABLE id="Table10" cellSpacing="0" cellPadding="0" width="100%" bgColor="white" border="0" runat="server">
																<TBODY>
																	<TR>
																		<TD vAlign="top" align="left" width="100%" bgColor="#ffffff">
																			<TABLE class="TableBackGround" id="Table4" borderColor="#cccccc" cellSpacing="1" cellPadding="1" width="100%" bgColor="#ffffff" border="0">
																				<TR>
																					<TD class="TableBackGround" id="td01" align="left" bgColor="#ffffff" runat="server"><STRONG>Aufträge</STRONG></TD>
																					<TD class="TableBackGround" align="center" bgColor="#ffffff"><STRONG>Dateien (Web - Verzeichnis)</STRONG></TD>
																				</TR>
																				<TR>
																					<TD id="td03" vAlign="top" bgColor="#ffffff" runat="server" align="left">
                                                                                        <asp:listbox id="lbxAuftrag" runat="server" Height="380px" Width="350px" BackColor="White" CssClass="DropDownStyle" OnSelectedIndexChanged="AuftragSelected" AutoPostBack="True" />
                                                                                    </TD>
																					<TD vAlign="top" width="100%" bgColor="#ffffff">
                                                                                        <div style="height:380px; overflow-y:auto;">
                                                                                            <asp:GridView ID="gridServer" runat="server" Width="100%" BackColor="White" HeaderStyle-CssClass="tableHeader" CssClass="tableBody" BorderColor="Transparent"
                                                                                              AutoGenerateColumns="false" DataKeyNames="filename"
                                                                                              OnRowDeleting="GridServerRowDeleting">
                                                                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="GridTableHead" VerticalAlign="Top" />
                                                                                                <Columns>
                                                                                                    <asp:TemplateField HeaderText="Vorschau">
                                                                                                        <ItemTemplate>
                                                                                                            <div style="margin-left:1px;margin-top:1px;">
                                                                                                        	    <asp:Image ID="Image1" runat="server" Width="75px" Height="75px" BorderColor="Black" ToolTip='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' ImageUrl='<%# DataBinder.Eval(Container, "DataItem.ServerFile") %>' BorderStyle="Solid" BorderWidth="1px" />
																											    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.ServerFile") %>' ToolTip="Bild in Originalgröße anzeigen" ImageUrl="/Portal/Images/lupe.gif" Target="_blank" style="vertical-align:top;" >HyperLink</asp:HyperLink>
                                                                                                            </div>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Verschieben">
                                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox runat="server" ID="selector" OnCheckedChanged="FileSelected" AutoPostBack="true" Checked='<%# DataBinder.Eval(Container, "DataItem.Selected") %>' />
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="l&#246;schen">
																									    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																									    <ItemStyle HorizontalAlign="Center"></ItemStyle>
																									    <ItemTemplate>
																										    <asp:ImageButton id="ibtnSRDelete" runat="server" ToolTip="Zeile löschen" ImageUrl="/Portal/Images/loesch.gif" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FileName") %>' CommandName="Delete"></asp:ImageButton>
																									    </ItemTemplate>
																								    </asp:TemplateField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </TD>
																				</TR>
                                                                                <tr>
                                                                                    <td>
                                                                                        
                                                                                    </td>
                                                                                    <td nowrap align="right" style="position:relative;">
                                                                                        <asp:Panel ID="movePanel" runat="server" style="position:absolute;left:0;">
                                                                                            <asp:Label runat="server" ID="moveLabel" Text="Verschieben..." AssociatedControlID="moveToList"/>
                                                                                            <asp:DropDownList runat="server" ID="moveToList" />
                                                                                            <asp:LinkButton runat="server" ID="moveButton" CssClass="StandardButtonTable" ToolTip="Gewählte Bilder in anderen Auftrag verschieben" OnClick="MoveClick">&#149;&nbsp;Verschieben</asp:LinkButton>
                                                                                        </asp:Panel>
                                                                                        <asp:LinkButton ID="btnFinish" runat="server" CssClass="StandardButtonTable" ToolTip="Weiter zur Bestätigungsseite" OnClick="FinishClick">&#149;&nbsp;Fertig</asp:LinkButton>
                                                                                        <asp:LinkButton ID="btnBack" runat="server" Visible="False" CssClass="StandardButtonTable" ToolTip="Zurück zur Bearbeitung" OnClick="BackClick">&#149;&nbsp;Zurück</asp:linkbutton>
                                                                                        <asp:LinkButton ID="btnConfirm" runat="server" Visible="False" CssClass="StandardButtonTable" ToolTip="Dateien auf dem Archivserver ablegen" OnClick="ConfirmClick">&#149;&nbsp;Auftrag absenden!</asp:linkbutton>
                                                                                    </td>
                                                                                </tr>
																			</TABLE>
																		</TD>
																	</TR>
																</TBODY></TABLE>
														</TD>
													</TR>
												</TABLE>
												<asp:label id="lblError" runat="server" CssClass="TextError"></asp:label><asp:label id="lblOpen" runat="server"></asp:label><asp:label id="lblMsg" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</tr>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>