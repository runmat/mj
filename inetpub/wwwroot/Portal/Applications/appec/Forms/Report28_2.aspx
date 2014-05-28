<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report28_2.aspx.vb" Inherits="AppEC.Report28_2" EnableEventValidation="false"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .show
        {
            display: inline;
            visibility: visible;
        }
        .hide
        {
            display: none;
            visibility: hidden;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
		    <asp:ScriptManager ID="ScriptManager1" runat="server"/>
            <script language="javascript" type="text/javascript">
            
                function showWaitSymbol(sender) {

                    sender.setAttribute("class", "hide");

                    var div = document.getElementById("<%=ExcelWait.UniqueID %>");
                    div.setAttribute("class","show");

                    __doPostBack('btnUpdateExcel', 'btnUpdateExcel');
                }
        </script>
			<table width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<table id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="PageNavigation" colSpan="2">
									<asp:label id="lblHead" runat="server"></asp:label>
								</td>
							</tr>
                            <tr>
								<td vAlign="top" colspan="2">
									<table id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td align="left" colSpan="2">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<td class="TaskTitle">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <div style="white-space: nowrap;">
                                                                            <asp:HyperLink ID="lnkCSV" runat="server" CssClass="TaskTitle" style="margin-right:4px; margin-left:4px;" Visible="False" 
                                                                                Target="_blank" ToolTip="CSV Download"  BorderStyle="None">
                                                                                <img src="..\..\..\Images\csv.gif" width="20" height="20" alt="CSV image"/>
                                                                            </asp:HyperLink>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button runat="server" ID="btnUpdateExcel" Text="Excel generieren" OnClick="btnUpdateExcel_Click"
                                                                                        Enabled="True" OnClientClick="javascript:showWaitSymbol(this);"/>
                                                                    </td>
                                                                    <td>
                                                                        <div id="ExcelWait" name="ExcelWait" runat="server" class="hide">
                                                                            <img src="..\..\..\Images\indicator.gif" alt="WaitSymbol"  />
                                                                            Exceldatei wird erstellt...
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel runat="server" ID="UpExcel" UpdateMode="conditional">
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="btnUpdateExcel" />
                                                                            </Triggers>
                                                                            <ContentTemplate>
                                                                                <div style="white-space: nowrap; float: left;">
                                                                                    <asp:HyperLink ID="lnkExcel2" runat="server" CssClass="TaskTitle" Visible="False"
                                                                                        Target="_blank" BorderStyle="None">
                                                                                      <img src="..\..\..\Images\Excel.gif" width="20" height="20" alt="CSV image"/>  
                                                                                    </asp:HyperLink>&nbsp;
                                                                                    <asp:Label ID="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:Label>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
														</td>
                                                    </tr>
                                                </table>
											</td>
										</tr>
									</table>
									<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label>
								</td>
                            </tr>
							<tr>
								<td class="TextExtraLarge" colSpan="2">
								    <asp:label id="lblNoData" runat="server" Visible="False">
								        
								    </asp:label>
								</td>
							</tr>
							<tr>
								<td colspan="2" align="right">
									<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True" Visible="False"></asp:dropdownlist>
                                </td>
							</tr>
							<tr>
							    <td valign="top">
							        <asp:LinkButton ID="cmdBack"  Height="16px" runat="server" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:LinkButton>
							    </td>
								<td>
								    <asp:datagrid id="DataGrid1" runat="server" BackColor="White" AutoGenerateColumns="False" PageSize="50" headerCSS="tableHeader" 
                                        bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True" AllowPaging="True" Width="100%">
										<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="Carport" SortExpression="Carport" HeaderText="Carport">
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Carportname" SortExpression="Carportname" HeaderText="Carport-Name">
											</asp:BoundColumn>
											<asp:TemplateColumn SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer">
										        <ItemTemplate>
										            <asp:HyperLink ID="lnkHistorie" runat="server" Target="_blank" ToolTip="Zur Fahrzeughistorie" 
                                                        Text='<%# Eval("Fahrgestellnummer") %>'>
										            </asp:HyperLink>
										        </ItemTemplate>
										    </asp:TemplateColumn>
											<asp:BoundColumn DataField="Kennzeichen" SortExpression="Kennzeichen" HeaderText="Kennzeichen">
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Zulassungsdatum" SortExpression="Zulassungsdatum" HeaderText="Zulassungsdatum" DataFormatString="{0:dd.MM.yyyy}">
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Unitnummer" SortExpression="Unitnummer" HeaderText="Unit Nr.">
											</asp:BoundColumn>
                                            <asp:BoundColumn DataField="ModellID" SortExpression="ModellID" HeaderText="Modell-ID">
											</asp:BoundColumn>
                                            <asp:BoundColumn DataField="Modell" SortExpression="Modell" HeaderText="Modell">
											</asp:BoundColumn>
                                            <asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status">
											</asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
									</asp:datagrid>
								</td>
				            </tr>
				            <tr>
					            <td colspan="2">
					                &nbsp;
                                </td>
				            </tr>
				            <tr>
					            <td vAlign="top" width="50">
					                &nbsp;
                                </td>
					            <td align="right">
					                <!--#include File="../../../PageElements/Footer.html" -->
                                </td>
				            </tr>
			            </table>
			        </td>
                </tr>
            </table>
	    </form>
	</body>
</html>