<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change06.aspx.vb" Inherits="AppAvis.Change06" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
		<script language="JavaScript"  type="text/javascript" src="../Javascript/Slideup_Down.js"></script>
	    <style type="text/css">
            .style4
            {
                width: 84%;
            }
            </style>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <input id="SelOpen2" type="hidden" size="1" runat="server" />
    <div>
    			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="10%" class="TaskTitle">
                                        &nbsp;</TD>
								<td vAlign="top" class="TaskTitle">
									&nbsp;</td>
							</tr>
							<tr>
								<TD vAlign="top" width="10%">
                                        &nbsp;</TD>
								<td vAlign="top" class="style4">
									&nbsp;</td>
							</tr>
							<tr>
								<TD vAlign="top" width="10%" >
                                        <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="StandardButton"> •&nbsp;Zurück</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                        <asp:LinkButton ID="Down" runat="server" CssClass="StandardButton"> •&nbsp;Neu</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                            </TD>
								<td vAlign="top" class="style4">
                                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <div id="Suche2" style=" display:none; overflow: hidden; width: 350px; height: 200px;
                                                    background: #FFFFCC; border: groove;">
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td nowrap="nowrap" width="105px">
                                                                &nbsp;
                                                            </td>
                                                            <td nowrap="nowrap" width="8px">
                                                                &nbsp;</td>
                                                            <td align="right">
                                                                <asp:ImageButton ID="UP" runat="server" ImageUrl="../../../Images/loesch.gif" Height="13px"
                                                                    Width="13px" ToolTip="Datum entfernen!" ImageAlign="AbsMiddle" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap="nowrap" width="105px">
                                                                ID: </td>
                                                            <td nowrap="nowrap" width="10" style="color: #FF0000">
                                                                * </td>
                                                            <td>
                                                                <asp:TextBox ID="txtID" Enabled="false" runat="server" MaxLength="20" Width="120px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap="nowrap" width="105px" >
                                                                Name:
                                                                
                                                            </td> 
                                                             
                                                            <td nowrap="nowrap" style="color: #FF0000" width="8px" >
                                                                *
                                                                
                                                            </td> 
                                                             
                                                            <td>
                                                                <asp:TextBox ID="txtName" Enabled="false" runat="server" Height="22px" MaxLength="90" 
                                                                    Width="225px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap="nowrap" width="105px">
                                                                Ort:
                                                                </td>
                                                            <td nowrap="nowrap" style="color: #FF0000" width="8px">
                                                                *
                                                                </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOrt" Enabled="false" runat="server" MaxLength="28" Width="225px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap="nowrap" width="105px">
                                                                1. Mailadresse:&nbsp;</td>
                                                            <td nowrap="nowrap" style="color: #FF0000" width="8px">
                                                                *
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMail1" runat="server" Height="22px" Width="225px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap="nowrap" width="100">
                                                                2. Mailadresse:
                                                            </td>
                                                            <td nowrap="nowrap" width="8px">
                                                                &nbsp;</td>
                                                            <td>
                                                                <asp:TextBox ID="txtMail2" runat="server" Height="22px" Width="225px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td nowrap="nowrap" width="100">
                                                                3. Mailadresse:
                                                            </td>
                                                            <td nowrap="nowrap" width="8px">
                                                                &nbsp;</td>
                                                            <td>
                                                                <asp:TextBox ID="txtMail3" runat="server" Height="22px" Width="225px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="color: #FF0000">
                                                                * Pflichtfelder
                                                            </td>
                                                            <td style="color: #FF0000" width="8px">
                                                                &nbsp;</td>
                                                            <td align="right">
                                                            <asp:LinkButton ID="cmdDel" runat="server" CssClass="ButtonUp"> •&nbsp;Löschen&nbsp;»</asp:LinkButton>&nbsp;
                                                                <asp:LinkButton ID="cmdSave" runat="server" CssClass="ButtonUp"> •&nbsp;Speichern&nbsp;»</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblError"
                                                                runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelExtraLarge">
                                                            <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                        <td id="ExcelCell" runat="server" visible="false" align="right">
                                                            <strong>&nbsp;<img alt="" src="../../../images/excel.gif" style="width: 16px; height: 16px" />&nbsp;
                                                                <asp:LinkButton CssClass="ExcelButton" ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>&nbsp;
                                                                Anzahl Vorgänge / Seite </strong>
                                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True"
                                                    bodyHeight="400" CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader"
                                                    PageSize="50" BackColor="White" AutoGenerateColumns="False" DataKeyNames="ID">
                                                    <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                    <PagerStyle CssClass="TextExtraLarge" Wrap="False"></PagerStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderText="ID" />
                                                        <asp:BoundField DataField="Name" HeaderText="Name" />
                                                        <asp:BoundField DataField="Ort" HeaderText="Ort" />
                                                        <asp:BoundField DataField="Mail1" HeaderText="1. Mailadresse" />
                                                        <asp:BoundField DataField="Mail2" HeaderText="2. Mailadresse" />
                                                        <asp:BoundField DataField="Mail3" HeaderText="3. Mailadresse" />
                                                        <asp:TemplateField HeaderText="Bearbeiten">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lbtnEdit" runat="server" ImageUrl="../../../images/lupe2.gif"
                                                                    ToolTip="Bearbeiten" CommandName="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
								</td>
							</tr>
							<tr>
								<TD vAlign="top">
                                        &nbsp;</TD>

							</tr>
							<tr>
								<td></td>
								<td class="style4">&nbsp;</td>
							</tr>
							<tr>
								<td></td>
								<td class="style4"><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</td>
				</tr>
			</table>
    </div>
        
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
    </form>
</body>
</html>
