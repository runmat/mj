<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AutUserUpload.aspx.vb" Inherits="CKG.Admin.AutUserUpload" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../PageElements/Styles.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
	<HEAD>
		<meta content="DAD DEUTSCHER AUTO DIENST GmbH" name="Copyright">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	    <style type="text/css">
            .TextLarge
            {
                text-align: left;
            }
            .style1
            {
                width: 103px;
            }
            .style2
            {
                width: 100%;
            }
            .style3
            {
                width: 64px;
            }
        </style>
     <script type="text/javascript" language="javascript">
         function openinfo(url) {
             fenster = window.open(url, "Dateiaufbau", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=650,height=250");
//             fenster.focus();
         }    
     </script>
    
        
        
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<TBODY>
					<tr>
						<td height="18"><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
					</tr>
					<tr>
						<td>
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<TR>
										<td class="PageNavigation" height="25" colSpan="2">Administration (Upload, 
                                            Bearbeitung, Freigabe)</td>
									</TR>
									<tr>
										<TD vAlign="top" class="style3">
											<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" 
                                                border="0">
												<TR>
													<TD class="TaskTitle"><div>&nbsp;</div></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="120">
                                                        <asp:linkbutton id="lbtnUpload" runat="server" 
                                                            CssClass="StandardButton" Height="20px" Width="78px">&#149;&nbsp;Upload</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="120">
                                                        <asp:linkbutton id="lbtnLaden" runat="server" 
                                                            CssClass="StandardButton" Visible="False" Height="20px" Width="78px">•&nbsp;Bearbeiten</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="120">
                                                        <asp:linkbutton id="lbtnSave" runat="server" 
                                                            CssClass="StandardButton" Visible="False" Height="20px" Width="78px">•&nbsp;Anlegen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="120">
                                                        <asp:linkbutton id="lbtnPruefen" runat="server" 
                                                            CssClass="StandardButton" Visible="False" Height="20px" Width="78px">•&nbsp;Prüfen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="100">
                                                        <asp:linkbutton id="lbtnFreigeben" runat="server" 
                                                            CssClass="StandardButton" Visible="False" Height="20px" Width="78px">•&nbsp;Suchen</asp:linkbutton></TD>
												</TR>
												<TR>
													<TD vAlign="center" width="100">
                                                        <asp:linkbutton id="lbtnBack" runat="server" 
                                                            CssClass="StandardButton" Height="20px" Width="78px">&#149;&nbsp;Zurück</asp:linkbutton></TD>
												</TR>
											</TABLE>
										</TD>
										<td vAlign="top">
											<table cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
												<TBODY>
													<tr>
														<TD class="TaskTitle">&nbsp;&nbsp;</TD>
													</tr>
													<TR>
														<TD align="left">
                                                            <asp:Label ID="Label1" runat="server" Text="Auswahl Firma:  "></asp:Label>
&nbsp;&nbsp; &nbsp;&nbsp; <asp:DropDownList
                                                                ID="ddlFilterCustomer" runat="server" Width="300px" AutoPostBack="True"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                        </TD>
													</TR>
													<TR>
														<TD align="left">
                                                            <asp:Label ID="lblLoginLink" runat="server" Text="Link zur Startseite:"></asp:Label>
&nbsp;<asp:DropDownList
                                                                ID="ddlLoginPage" runat="server" Width="300px"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                        </TD>
													</TR>
													<TR>
														<TD align="left">
                                                            <asp:RadioButtonList ID="rblUpload" runat="server" AutoPostBack="True" 
                                                                RepeatDirection="Horizontal" TextAlign="Left">
                                                                <asp:ListItem Selected="True">Upload</asp:ListItem>
                                                                <asp:ListItem>Freigeben</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        &nbsp;</TD>
													</TR>
													<TR>
														<TD align="left"><asp:label id="lblMessage" runat="server" CssClass="TextLarge" EnableViewState="False"></asp:label><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
													</TR>
													<TR>
														<TD>
												            <TABLE id="TableUpload" cellSpacing="0" cellPadding="5" width="100%" border="0" runat="server">
													            <TR>
														            <TD noWrap align="right">Dateiauswahl 
                                                                        <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="/Portal/Images/fragezeichen.gif" NavigateUrl="~/admin/info01.htm" Target="_blank">Datei-Info</asp:HyperLink>                                                                            
                                                                        :&nbsp;&nbsp;</TD>
														            <TD class="TextLarge"><INPUT id="upFile" type="file" size="49" name="File1" runat="server">&nbsp;</TD>
													            </TR>
													            <TR>
														            <TD class="style1" noWrap align="right">&nbsp;</TD>
														            <TD class="TextLarge">&nbsp;
															            <asp:label id="lblExcelfile" runat="server"></asp:label></TD>
													            </TR>
												            </TABLE>														
													
														</TD>
													</TR>
													<TR>
														<TD align="left">
															<table class="style2">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                                    </td>
                                                                    <td ID="tdExcel" runat="server" style="text-align: right">
                                                                        <strong>
                                                                        <img alt="" src="../images/excel.gif" style="width: 15px; height: 17px" /><asp:LinkButton 
                                                                            ID="lnkCreateExcel" runat="server" CssClass="ExcelButton" Visible="False">Excelformat</asp:LinkButton>
                                                                        </strong>
                                                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" 
                                                                            Visible="False">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </TD>
													</TR>
													<tr>
														<td align="left">
                                                            <asp:GridView ID="grvAusgabe" runat="server" AllowPaging="True" 
                                                                AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                                                CssClass="tableMain" PageSize="50" Width="100%">
                                                                <PagerSettings Position="Top" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Anrede" SortExpression="Title">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtAnrede" runat="server" Text='<%# Bind("Title") %>' 
                                                                                Width="50px" MaxLength="4"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Vorname" SortExpression="Firstname">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtVorname" runat="server" MaxLength="20" 
                                                                                Text='<%# Bind("Firstname") %>' Width="80px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Nachname" SortExpression="LastName">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtNachname" runat="server" MaxLength="50" 
                                                                                Text='<%# Bind("LastName") %>' Width="120px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Benutzername" SortExpression="Username">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtUsername" runat="server" MaxLength="50" 
                                                                                Text='<%# Bind("Username") %>' Width="120px" Wrap="False"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Referenz" SortExpression="Reference">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtReference" runat="server" MaxLength="12" 
                                                                                Text='<%# Bind("Reference") %>' Width="70px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="80px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Filiale" SortExpression="Store">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtFiliale" runat="server" MaxLength="50" 
                                                                                Text='<%# Bind("Store") %>' Width="80px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Test" SortExpression="TestUser">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtTestzugang" runat="server" MaxLength="4" 
                                                                                Text='<%# Bind("TestUser") %>' Width="30px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Gruppe" SortExpression="GroupName">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtGruppe" runat="server" MaxLength="50" 
                                                                                Text='<%# Bind("GroupName") %>' Width="80px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Organisation" SortExpression="Organization">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtOrganization" runat="server" MaxLength="50" 
                                                                                Text='<%# Bind("Organization") %>' Width="80px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="E-Mailadresse" SortExpression="EMailAdress">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtMail" runat="server" MaxLength="50" 
                                                                                Text='<%# Bind("EMailAdress") %>' Width="150px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Wrap="False" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Gültig ab" SortExpression="ValidFrom">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtGueltigkeitsdatum" runat="server" MaxLength="10" 
                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.ValidFrom","{0:d}") %>' Width="80px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Error" SortExpression="Error" 
                                                                        HeaderStyle-Width="50px" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkError" runat="server" Checked='<%# Bind("Error") %>' 
                                                                                Enabled="False" />
                                                                        </ItemTemplate>

<HeaderStyle Width="50px"></HeaderStyle>

                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="ID">
                                                                    <HeaderStyle Width="0px" />
                                                                    <ItemStyle Width="0px" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <PagerStyle CssClass="TextExtraLarge" />
                                                                <HeaderStyle CssClass="GridTableHead" />
                                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                            </asp:GridView>
                                                        </td>
													</tr>
													<tr>
														<td align="left">
															&nbsp;</td>
													</tr>
													<TR>
														<TD align="left" height="25"></TD>
													</TR>
												</TBODY></table>
										</td>
									</tr>
									<tr>
										<td class="style3"></td>
										<td>
                                            &nbsp;</td>
									</tr>
									<tr>
										<td class="style3"></td>
										<td><!--#include File="../PageElements/Footer.html" --></td>
									</tr>
								</TBODY></TABLE>
						</td>
					</tr>
				</TBODY></table>
		</form>
	</body>
</html>
