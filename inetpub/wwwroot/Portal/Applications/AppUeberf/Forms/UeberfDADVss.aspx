<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UeberfDADVss.aspx.vb" Inherits="AppUeberf.UeberfDADVss" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	    <style type="text/css">
            #Table12
            {}
            .style1
            {
                width: 93px;
            }
            .TextError
            {
                margin-left: 0px;
            }
            #Table2
            {
                width: 522px;
            }
        </style>
	    </HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center" cellSpacing="0" cellPadding="0">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td vAlign="top" align="left" width="100%" colSpan="3">
						<table id="Table10" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(
									<asp:label id="lblPageTitle" runat="server">Versand Schein und Schilder</asp:label>)</td>
							</tr>
							<tr>
								<td style="WIDTH: 144px" vAlign="top" width="174">
									<table id="Table12" cellSpacing="0" cellPadding="0" border="0">
										<tr>
											<td width="150">
												<asp:linkbutton id="lbtWeiter" tabIndex="12" runat="server" 
                                                    CssClass="StandardButtonTable" Width="100px"> •&nbsp;Weiter</asp:linkbutton>
												</td>
										</tr>
										<tr>
											<td vAlign="middle" width="150"><asp:linkbutton id="lbtBack" tabIndex="12" 
                                                    runat="server" CssClass="StandardButtonTable" Width="100px"> •&nbsp;Zurück</asp:linkbutton></td>
										</tr>
										<tr>
											<td vAlign="middle" width="150"></td>
										</tr>
									</table>
								</td>
								<td style="WIDTH: 917px" vAlign="top">
									<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td style="WIDTH: 437px" width="437">
												<asp:label id="lblError" runat="server" Width="821px" EnableViewState="False" CssClass="TextError"></asp:label></td>
										</tr>
										
										<tr>
											<td style="WIDTH: 437px" width="437">
												<asp:Literal ID="ltAnzeige" runat="server"></asp:Literal>
                                            </td>
										</tr>
										
										<tr>
											<td style="WIDTH: 437px" width="437">
												&nbsp;&nbsp;</td>
										</tr>
										
										<tr>
											<td style="WIDTH: 437px" width="437">
												<asp:Panel ID="pnl1" runat="server" BorderColor="Black" BorderStyle="Solid" 
                                                    BorderWidth="1px" Width="526px">
									<table class="BorderLeftBottom" id="Table2" cellSpacing="0" cellPadding="5" bgColor="white" 
                                                    border="0" frame="border">
										<tr>
											<td class="style1" vAlign="center" noWrap>Vertragsnummer</td>
											<td class="" vAlign="center" width="295" noWrap>
                                                <asp:Label ID="lblVertragsnummer" runat="server"></asp:Label>
                                            </td>
										</tr>
										<tr>
                                            <td class="style1" noWrap vAlign="center">
                                            </td>
                                            <td class="" noWrap vAlign="center" width="295">
                                                * = Pflichtfeld</td>
                                        </tr>
										<tr>
											<td class="style1" vAlign="center" noWrap>&nbsp;</td>
											<td class="" vAlign="center" width="295" noWrap>&nbsp;</td>
										</tr>
										<tr>
											<td class="style1" vAlign="center" noWrap>Name1*</td>
											<td class="" vAlign="center" noWrap width="295">
												<asp:TextBox ID="txtVssName1" runat="server" Width="200px" MaxLength="35"></asp:TextBox>
                                                                <asp:ImageButton ID="ibtSearchLN" runat="server" 
                                                                    ImageUrl="/Portal/images/lupe2.gif" ToolTip="Adresssuche" />
                                            </td>
										</tr>
										<tr>
											<td class="style1" vAlign="center" noWrap>Name 2</td>
											<td vAlign="center" noWrap width="295">
												<asp:TextBox ID="txtVssName2" runat="server" Width="200px" MaxLength="35"></asp:TextBox>
                                            </td>
										</tr>
										<tr>
											<td class="style1" vAlign="center" noWrap>Strasse*</td>
											<td vAlign="center" noWrap width="295">
												<asp:TextBox ID="txtVssStrasse" runat="server" Width="200px" MaxLength="35"></asp:TextBox>
                                            </td>
										</tr>
										<tr>
											<td class="style1" vAlign="center" noWrap>PLZ*, Ort*</td>
											<td class="" vAlign="center" width="295">
												<P align="right" style="text-align: left">
													<asp:TextBox ID="txtVssPLZ" runat="server" Width="66px"></asp:TextBox>
&nbsp;<asp:TextBox ID="txtVssOrt" runat="server" Width="212px"></asp:TextBox>
                                                </P>
											</td>
										</tr>
										<tr>
											<td class="style1" vAlign="center" noWrap>
                                                &nbsp;</td>
											<td class="" vAlign="center" width="295">
												&nbsp;</td>
										</tr>
									</table>                                                    
                                                </asp:Panel>
                                            </td>
										</tr>
										
										<tr>
											<td style="WIDTH: 437px" width="437">

								            </td>
										</tr>
										
									</table>
								</table>
			<table id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td vAlign="top" align="middle" width="100%">
									&nbsp;</td>
							</tr>
						</table>								
									
									
                                </td>
									
							</tr>
						</table>
			</form>
	</body>
</HTML>