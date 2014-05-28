<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report10.aspx.vb" Inherits="AppF1.Report10" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>


<%@ Register src="../controls/SucheHaendler.ascx" tagname="SucheHaendler" tagprefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	    <style type="text/css">
            .style1
            {
                width: 97%;
            }
            .style2
            {
                width: 259px;
            }
        </style>
	</HEAD>
	<body topmargin="0" leftmargin="0" MS_POSITIONING="FlowLayout">
    
        <uc1:BusyIndicator runat="server" />

		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</tr>
				<tr>
					<td>
					  
                    
                    
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2">
									<asp:Label id="lblHead" runat="server"></asp:Label>&nbsp;
									</td>
							</TR>
							<tr>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">
												&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
												<asp:LinkButton id="cmdSearch" OnClientClick="Show_BusyBox1();" runat="server" 
                                                    CssClass="StandardButton" Visible="False">Weiter</asp:LinkButton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
                            <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" Visible="false" CssClass="StandardButton">Neue Suche</asp:LinkButton>
                                            </TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
												<asp:LinkButton id="cmdBack" runat="server" 
                                                    CssClass="StandardButton">Zurück</asp:LinkButton></TD>
										</TR>
									</TABLE>
								    <asp:Calendar ID="calVon" runat="server" BorderColor="Black" 
                                        BorderStyle="Solid" CellPadding="0" Visible="False" Width="120px">
                                        <TodayDayStyle Font-Bold="True" />
                                        <NextPrevStyle ForeColor="White" />
                                        <DayHeaderStyle BackColor="Silver" Font-Bold="True" />
                                        <SelectedDayStyle BackColor="#FF8080" />
                                        <TitleStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                        <WeekendDayStyle ForeColor="Silver" />
                                        <OtherMonthDayStyle ForeColor="Silver" />
                                    </asp:Calendar>
                                    <asp:Calendar ID="calBis" runat="server" BorderColor="Black" 
                                        BorderStyle="Solid" CellPadding="0" Visible="False" Width="120px">
                                        <TodayDayStyle Font-Bold="True" />
                                        <NextPrevStyle ForeColor="White" />
                                        <DayHeaderStyle BackColor="Silver" Font-Bold="True" />
                                        <SelectedDayStyle BackColor="#FF8080" />
                                        <TitleStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                        <WeekendDayStyle ForeColor="Silver" />
                                        <OtherMonthDayStyle ForeColor="Silver" />
                                    </asp:Calendar>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										
										<tr>
										    <td class="style2" colspan="2">
										        
                                        <asp:Label runat="server" ID="lbl_AnzeigeHaendlerSuche" Font-Underline="true" Font-Bold="true"
                                            ForeColor="Black" Font-Size="Larger" Text="lbl_AnzeigeHaendlerSuche"></asp:Label>
										        
										    </td>
										</tr>
										<tr>
										    <td class="style1" colspan="2">
										        <uc2:SucheHaendler ID="SucheHaendler1" runat="server" />
										    </td>
										</tr>
                                        <tr id="trDatumVon" runat="server">
														<TD class="StandardTableAlternate" vAlign="center"  style="width:150px" nowrap="nowrap">Versanddatum von:
														</TD>
														<TD class="StandardTableAlternate" vAlign="center" width="100%">
															<asp:TextBox id="txtErfassungsdatumVon" runat="server"></asp:TextBox>
															<asp:Label id="lblInputReq" runat="server" CssClass="TextError">*</asp:Label>&nbsp;
															<asp:LinkButton id="btnVon" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton>
														</TD>
													</tr>
													<tr id="trDatumBis" runat="server">
														<TD class="TextLarge" vAlign="center" style="width:150px"  nowrap="nowrap">
															Versanddatum bis :</TD>
														<TD class="TextLarge" vAlign="center">
															<asp:TextBox id="txtErfassungsdatumBis" runat="server"></asp:TextBox>
															<asp:Label id="Label1" runat="server" CssClass="TextError">*</asp:Label>&nbsp;
															<asp:LinkButton id="btnBis" runat="server" CssClass="StandardButtonTable" Width="100px"> &#149;&nbsp;Kalender...</asp:LinkButton></TD>
													</tr>
										<tr>
                                            <td colspan="2" style="width:100%">
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td class="style2" colspan="1" valign="bottom">
															<asp:label id="lblNoData" runat="server" Font-Bold="True" Visible="False"></asp:label>
											</td>
											<td ID="tdExcel" runat="server" vAlign="bottom" align="right" 
                                                style="text-align: right" colspan="1" height="50" nowrap="nowrap">
												<STRONG> 
												            <img   alt="" src="../../../images/excel.gif" style="width: 15px; height: 17px" /><asp:LinkButton CssClass="ExcelButton" id="lnkCreateExcel"  runat="server" Visible="False">Excelformat</asp:LinkButton></STRONG>
															<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True" Visible="False"></asp:dropdownlist>
											</td>
                                                    </tr>
                                                </table>
                                            </td>
											
										</tr>
										<tr>
											<td vAlign="top" align="left" colspan="2">
												<asp:GridView ID="grvAusgabe" runat="server" AllowSorting="True" 
                                                    AllowPaging="True" AutoGenerateColumns="False" Width="100%" PageSize="50" 
                                                    BackColor="White" CssClass="tableMain">
                                                    <PagerSettings Position="Top" />
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="col_Datum" SortExpression="ZZLSDAT">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Datum" runat="server" CommandArgument="ZZLSDAT" 
                                                                    CommandName="Sort">col_Datum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZLSDAT", "{0:d}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Referenz" SortExpression="ZZLFDNR">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz" runat="server" CommandArgument="ZZLFDNR" 
                                                                    CommandName="Sort">col_Referenz</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label44" runat="server" Text='<%# Bind("ZZLFDNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_PoolNr" SortExpression="POOLNR">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_PoolNr" runat="server" CommandArgument="POOLNR" 
                                                                    CommandName="Sort">col_PoolNr</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label99" runat="server" Text='<%# Bind("POOLNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Haendler"
                                                            SortExpression="HAENDLER">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Haendler" runat="server" CommandArgument="HAENDLER" 
                                                                    CommandName="Sort">col_Haendler</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("HAENDLER") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_VHNummer" SortExpression="MAKTX">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_VHNummer" runat="server" CommandArgument="MAKTX" 
                                                                    CommandName="Sort">col_VHNummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("MAKTX") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="col_FIN" SortExpression="CHASSIS_NUM">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_FIN" runat="server" CommandArgument="CHASSIS_NUM" 
                                                                    CommandName="Sort">col_FIN</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label33" runat="server" Text='<%# Bind("CHASSIS_NUM") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="col_Versandart" SortExpression="VERS_WEG_TX">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versandart" runat="server" CommandArgument="VERS_WEG_TX" 
                                                                    CommandName="Sort">col_Versandart</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("VERS_WEG_TX") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                                                                                                                                                                                          
                                                                                                                                                                                                                                
                                                    </Columns>
                                                    <PagerStyle CssClass="TextExtraLarge" />
                                                    <HeaderStyle CssClass="GridTableHead" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                </asp:GridView>
												<br>
											</td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	
	</body>
</HTML>