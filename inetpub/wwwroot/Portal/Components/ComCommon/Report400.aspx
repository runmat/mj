<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report400.aspx.vb" Inherits="CKG.Components.ComCommon.Report400" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Anzeige Report)</asp:label></td>
							</TR>
							<tr>
								<td></td>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<TD vAlign="top"></TD>
								<TD vAlign="top">
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td id="tdExcel" runat="server" colSpan="2">
												<asp:ImageButton id="lnkExcelImage" ImageUrl="~/Images/excel.gif" 
                                                    runat="server" Height="20px" Width="20px" ImageAlign="Top" Visible="False"></asp:ImageButton>
												&nbsp;
												<strong>
                                                <asp:LinkButton ID="lnkCreateExcel" runat="server" CssClass="ExcelButton" 
                                                    Visible="False">Excelformat</asp:LinkButton>
                                                </strong>&nbsp;&nbsp;&nbsp;&nbsp;<asp:hyperlink 
                                                    id="lnkBack" runat="server" NavigateUrl="javascript:history.back()" 
                                                    CssClass="ExcelButton">zurück</asp:hyperlink></td>
										</tr>
										<TR>
											<TD align="right" colSpan="2">&nbsp;</TD>
										<TR>
											<TD class="" width="100%" colSpan="1"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label></TD>
											<TD class="LabelExtraLarge" align="right"><asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
										</TR>
										<TR>
											<TD colSpan="2">
                                                <asp:GridView ID="grvAusgabe" runat="server" AllowPaging="True" 
                                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                                    CssClass="tableMain" PageSize="50" Width="100%">
                                                    <PagerSettings Position="Top" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="col_Briefeingang" SortExpression="ZBRIEFEINGANG">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Briefeingang" runat="server" CommandArgument="ZBRIEFEINGANG" 
                                                                    CommandName="Sort">col_Briefeingang</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label7" runat="server" 
                                                                    
                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.ZBRIEFEINGANG", "{0:d}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_LvNr" SortExpression="ZZLVNR">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_LvNr" runat="server" CommandArgument="ZZLVNR" 
                                                                    CommandName="Sort">col_LvNr</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("ZZLVNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Halter" SortExpression="NAME1_ZH">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Halter" runat="server" CommandArgument="NAME1_ZH" 
                                                                    CommandName="Sort">col_Halter</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("NAME1_ZH") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="NotSet" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Haendler" SortExpression="NAME1_HAENDLER">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Haendler" runat="server" 
                                                                    CommandArgument="NAME1_HAENDLER" CommandName="Sort">col_Haendler</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("NAME1_HAENDLER") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="NotSet" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle CssClass="TextExtraLarge" />
                                                    <HeaderStyle CssClass="GridTableHead" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                </asp:GridView>
                                            </TD>
										</TR>
									</TABLE>
									<asp:label id="lblInfo" runat="server" Font-Bold="True" EnableViewState="False"></asp:label></TD>
							</tr>
							<tr>
								<td></td>
								<td><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></td>
							</tr>
							<tr>
								<td></td>
								<td><!--#include File="../../PageElements/Footer.html" -->
									<asp:Label id="lblHidden" runat="server" Visible="False" Width="39px"></asp:Label></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				<TR id="ShowScript" runat="server" visible="False">
					<TD>
						<script language="Javascript">
						<!-- //
						function FreigebenConfirm(Fahrgest,Vertrag,BriefNr,Kennzeichen) {
						var Check = window.confirm("Wollen Sie für dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
						return (Check);
						}
						//-->
						</script>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
