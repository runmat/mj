<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report29.aspx.vb" Inherits="AppSIXT.Report29" %>
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
	    <style type="text/css">
            .style1
            {
                width: 97%;
            }
            .style2
            {
                width: 259px;
            }
            .style3
            {
                width: 100%;
            }
            .style4
            {
                font-size: xx-small;
            }
        </style>
        
        
	</HEAD>
	<body topmargin="0" leftmargin="0" MS_POSITIONING="FlowLayout">
    
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
												<asp:LinkButton id="cmdBack" runat="server" 
                                                    CssClass="StandardButton" CausesValidation="False">Zurück</asp:LinkButton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
                                                &nbsp;&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
												&nbsp;</TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="top">&nbsp;&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="top">&nbsp;&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="style1" colspan="2">
															    
											            <asp:Panel ID="pnlAuswahl" runat="server" Height="25px" Width="500px" Wrap="False" 
                                                                BackColor="#999999" ForeColor="White"> 
                                                            <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                                                                <div style="float: left;"><b>Auswahlkriterien</b></div>
                                                                <div style="float: left; margin-left: 20px;">
                                                                    <asp:Label ID="lblOpenClose" runat="server" Font-Bold="False">(Schließen...)</asp:Label>
                                                                </div>
                                                                <div style="float: right; vertical-align: middle;">
                                                                    <asp:ImageButton ID="Image1" runat="server" 
                                                                        ImageUrl="/Portal/images/collapse.jpg" AlternateText="(Schließen...)"/>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
											
											    
                                                <asp:Panel ID="pnlSuche" runat="server" Width="450px" BackColor="White" BorderStyle="Solid" 
                                                                BorderWidth="1px">
                
                                                    <table class="style3">
                                                        <tr>
                                                            <td class="style4" colspan="3">
                                                                <b>Geben Sie mindestens ein Kriterium ein.</b></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <b>Kennzeichen:</b></td>
                                                            <td>
                                                                <asp:TextBox ID="txtKennzeichen" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="font-weight: 700">
                                                                Erfassungsdatum von:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtDatumVon" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CompareValidator ID="cvlDatumVon" runat="server" 
                                                                    ControlToValidate="txtDatumVon" 
                                                                    ErrorMessage="Ungültiges Datum. Format(TT.MM.JJJJ)." Operator="DataTypeCheck" 
                                                                    Type="Date"></asp:CompareValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="font-weight: 700">
                                                                Erfassungsdatum bis:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtDatumBis" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CompareValidator ID="cvlDatumBis" runat="server" 
                                                                    ControlToValidate="txtDatumBis" 
                                                                    ErrorMessage="Ungültiges Datum. Format(TT.MM.JJJJ)." Operator="DataTypeCheck" 
                                                                    Type="Date"></asp:CompareValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" style="font-weight: 700">
                                                                <asp:CompareValidator ID="cvlDatum" runat="server" 
                                                                    ControlToCompare="txtDatumVon" ControlToValidate="txtDatumBis" 
                                                                    Display="Dynamic" 
                                                                    ErrorMessage="Erfassungsdatum bis kleiner als Erfassungsdatum von" 
                                                                    Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                                                                <asp:Label ID="lblPanelError" runat="server" EnableViewState="False" 
                                                                    ForeColor="Red" Text="Bitte geben Sie ein Kriterium ein." Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td style="text-align: right">
                                                                <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton" 
                                                                    Visible="True">Weiter</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                   
                                                </asp:Panel>
                                                
           								</td>
										</tr>
										<tr>
										    <td class="style2" colspan="1">
										        
                                                &nbsp;</td>
										</tr>
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
										<tr>
											<td vAlign="top" align="left" colspan="2">
												<asp:GridView ID="grvAusgabe" runat="server" AllowSorting="True" 
                                                    AllowPaging="True" AutoGenerateColumns="False" Width="100%" PageSize="50" 
                                                    BackColor="White" CssClass="tableMain">
                                                    <PagerSettings Position="Top" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="col_Kennzeichen" SortExpression="LICENSE_NUM">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="LICENSE_NUM" 
                                                                    CommandName="Sort">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("LICENSE_NUM") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Erfassungsdatum" SortExpression="ERDAT">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Erfassungsdatum" runat="server" CommandArgument="ERDAT" 
                                                                    CommandName="Sort">col_Erfassungsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label7" runat="server" 
                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.ERDAT", "{0:d}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
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