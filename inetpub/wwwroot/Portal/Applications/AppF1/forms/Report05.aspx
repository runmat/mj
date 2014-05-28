<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report05.aspx.vb" Inherits="AppF1.Report05" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
--%>

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
									<asp:label id="lblPageTitle" runat="server"> (Suche)</asp:label>
								</td>
							</TR>
							<tr>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">
												&nbsp;&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
												<asp:LinkButton id="cmdSearch" OnClientClick="Show_BusyBox1();" runat="server" 
                                                    CssClass="StandardButton" Height="16px" Width="100px">Suchen</asp:LinkButton>&nbsp; </TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
                                                &nbsp;</TD>
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
											<TD class="TaskTitle" vAlign="top">&nbsp;&nbsp;</TD>
										</TR>
									</TABLE>
									
 									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="style1" colspan="2">
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="rdbAlle" runat="server" AutoPostBack="True" Checked="True" 
                                                    Font-Bold="True" Text="Alle" TextAlign="Left" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:RadioButton ID="rdbAnwendung" runat="server" Text="Anwendung" 
                                                    TextAlign="Left" Width="400px" AutoPostBack="True" Font-Bold="True" />
                                                    &nbsp;<asp:RadioButton ID="rdbGruppe" runat="server" Text="Gruppe" TextAlign="Left" Width="300px" AutoPostBack="True" 
                                                    Font-Bold="True" />
                                            </td>
										</tr>
										<tr>
											<td class="style1" colspan="2">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlAnwendung" runat="server" Enabled="False" 
                                                    Height="22px" Width="400px">
                                                </asp:DropDownList>
                                    &nbsp;<asp:DropDownList ID="ddlGruppe" runat="server" Enabled="False" Height="22px" 
                                                    Width="300px">
                                                </asp:DropDownList>
                                            </td>
										</tr>
										
										<tr>
										    <td class="style1" colspan="2">
										        
                                                <asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
                                            </td>
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
                                                    AllowPaging="True" AutoGenerateColumns="False" Width="100%" PageSize="20" 
                                                    BackColor="White" CssClass="tableMain">
                                                    <PagerSettings Position="Top" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Username" HeaderText="Username" 
                                                            SortExpression="Username" />
                                                        <asp:BoundField DataField="Firstname" HeaderText="Vorname" 
                                                            SortExpression="Firstname" />
                                                        <asp:BoundField DataField="LastName" HeaderText="Nachname" 
                                                            SortExpression="LastName" />
                                                        <asp:TemplateField HeaderText="Gruppe" SortExpression="GroupName">
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("GroupName") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" class="style3">
                                                                    <tr>
                                                                        <td width="100%">
                                                                            <asp:Label ID="lblGroup" runat="server" Text='<%# Bind("GroupName") %>'></asp:Label>
                                                                            <asp:Label ID="lblGroupID" runat="server" Text='<%# Bind("GroupID") %>' 
                                                                                Visible="False"></asp:Label>
                                                                        </td>
                                                                        <td style="text-align: right">
                                                                            <asp:Button ID="btnAnwendung" runat="server" 
                                                                                OnClick="Button1_Click" Font-Bold="True" Text="&gt;" CommandName="Edit"
                                                                                CommandArgument='<%# Container.DataItemIndex %>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="LastLogin" DataFormatString="{0:d}" 
                                                            HeaderText="Datum letzte Anmeldung" SortExpression="LastLogin" >
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LastPwdChange" 
                                                            HeaderText="Datum letzte Passwortänderung" SortExpression="LastPwdChange" 
                                                            DataFormatString="{0:d}" >
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Gesperrt" SortExpression="AccountIsLockedOut">
                                                            <EditItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" 
                                                                    Checked='<%# Bind("AccountIsLockedOut") %>' />
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" 
                                                                    Checked='<%# Bind("AccountIsLockedOut") %>' Enabled="false" />
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
								<TD vAlign="top">
								
                                   
                                </TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top">
								           
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
            <ContentTemplate>

            <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display:none" />
            <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" 
                    Visible="False" />

            <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                    TargetControlID="btnFake" PopupControlID="mb" 
                    BackgroundCssClass="modalBackground" DropShadow="true" 
                    CancelControlID="btnOK" X="450" Y="200" >
            </cc1:ModalPopupExtender>

             <asp:Panel ID="mb" runat="server" BackColor="White" Width="300" Style="display:none" >
                <div style="text-align:center;">
                    &nbsp;&nbsp;
                    <asp:Label ID="lblGruppe" Text="Anwendungen der Gruppe " runat="server" 
                        Font-Bold="True"></asp:Label>
                </div>
                <br />
                <div>
                    <asp:Panel ID="pnlAppGrid" runat="server" ScrollBars="Auto" 
                        style="text-align: center">
                        <asp:GridView ID="grvAnwendungen" runat="server" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="AppFriendlyName" 
                                    HeaderText="Zugeordnete Anwendungen">
                                    <HeaderStyle Font-Bold="True" />
                                    <ItemStyle ForeColor="Blue" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
                <br />
                <div style="text-align:center;"><asp:Button ID="btnOK" runat="server" Text="OK" Width="80px" /></div>
                <div>
                    &nbsp;
                </div>
            </asp:Panel>


            </ContentTemplate>
            </asp:UpdatePanel>
        </div>

								
                                    </TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<td>
                                    &nbsp;</td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	
	</body>
</HTML>