<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Performance2.aspx.vb" Inherits="Admin.Performance2"
    MasterPageFile="MasterPage/Admin.Master" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="cmdcreate" runat="server" 
                    Text="Zurück" CssClass="Tablebutton" Width="78px"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Administration (Allgemeine Leistungsangaben)"></asp:Label>
                        </h1>
                    </div>
                            <div id="TableQuery">
                               <TABLE id="Table1" cellSpacing="0" cellPadding="0" bgColor="white" border="0" width="100%">
										<TR class="formquery">
											<TD class="firstLeft active" width="13%">Kategorie:</TD>
											<TD width="12%"><asp:label id="lblCategoryName" runat="server"></asp:label></TD>
											<TD class="firstLeft active"width="13%">Bezeichnung:</TD>
											<TD width="12%"><asp:label id="lblCounterName" runat="server"></asp:label></TD>
											<TD class="firstLeft active"width="13%">Instanz:</TD>
											<TD width="12%"><asp:label id="lblInstanceName" runat="server"></asp:label></TD>
										    <TD class="firstLeft active">Wert(<asp:label id="lblCounterUnit" runat="server"></asp:label>):</TD>
											<TD><asp:label id="lblValue" runat="server"></asp:label></TD>    
										</TR>
										<TR class="formquery">
											<TD colspan= "8">&nbsp;</TD>
											<TD colspan= "8"><asp:Label ID="lblError" runat="server" Text="" Visible="false"></asp:Label></TD>
										</TR>
									</TABLE>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">                
                                    &nbsp;
                                </div>
                            </div>
                            <div id="Div2" runat="server">
                            &nbsp;
                            </div>
                            <div id="Result" runat="Server" visible="true">
                                <div id="data" style="border: solid 1px #dfdfdf">
                                    <table id="Table3" cellspacing="0" cellpadding="0" bgcolor="white" border="0">                                        
                                        <tr>
                                            <td width="20%">&nbsp;</td>
                                            <td width="20%">
                                                <table id="Table4" cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                                    <tr>
                                                        <TD class="active">Minimum</TD>
                                                    </tr>
                                                    <tr>
                                                        <TD ><asp:label id="lblMin" runat="server"></asp:label></TD>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="40%">
                                                 <table id="Table2" cellspacing="0" cellpadding="0" bgcolor="white" border="0" >
                                                    <tr>    
											            <TD class="active" align="center">Betrachteter Zeitraum:&nbsp;</TD>
											        </tr>
											        <tr>
											            <TD align="center"><asp:dropdownlist id="ddlPageSize" runat="server" Width="60px" 
                                                    AutoPostBack="True"></asp:dropdownlist></TD>
											        </tr>
                                                </table>
                                            </td>
                                            <td width="20%">
                                               	<table id="Table5" cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                                    <tr>    
											            <TD class="active" align="right">Maximum </TD>
											        </tr>
											        <tr>
											            <td align="right"><asp:label id="lblMax" runat="server"></asp:label></td>
											        </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label1" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater1" runat="server">
													<ItemTemplate>
														<asp:Image id="Image1" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label2" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater2" runat="server">
													<ItemTemplate>
														<asp:Image id="Image2" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label3" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater3" runat="server">
													<ItemTemplate>
														<asp:Image id="Image3" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label4" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater4" runat="server">
													<ItemTemplate>
														<asp:Image id="Image4" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label5" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater5" runat="server">
													<ItemTemplate>
														<asp:Image id="Image5" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label6" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater6" runat="server">
													<ItemTemplate>
														<asp:Image id="Image6" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label7" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater7" runat="server">
													<ItemTemplate>
														<asp:Image id="Image7" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label8" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater8" runat="server">
													<ItemTemplate>
														<asp:Image id="Image8" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label9" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater9" runat="server">
													<ItemTemplate>
														<asp:Image id="Image9" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label10" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater10" runat="server">
													<ItemTemplate>
														<asp:Image id="Image10" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label11" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater11" runat="server">
													<ItemTemplate>
														<asp:Image id="Image11" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label12" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater12" runat="server">
													<ItemTemplate>
														<asp:Image id="Image12" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label13" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater13" runat="server">
													<ItemTemplate>
														<asp:Image id="Image13" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label14" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater14" runat="server">
													<ItemTemplate>
														<asp:Image id="Image14" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label15" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater15" runat="server">
													<ItemTemplate>
														<asp:Image id="Image15" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label16" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater16" runat="server">
													<ItemTemplate>
														<asp:Image id="Image16" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label17" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater17" runat="server">
													<ItemTemplate>
														<asp:Image id="Image17" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label18" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater18" runat="server">
													<ItemTemplate>
														<asp:Image id="Image18" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label19" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater19" runat="server">
													<ItemTemplate>
														<asp:Image id="Image19" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label20" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater20" runat="server">
													<ItemTemplate>
														<asp:Image id="Image20" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label21" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater21" runat="server">
													<ItemTemplate>
														<asp:Image id="Image21" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label22" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater22" runat="server">
													<ItemTemplate>
														<asp:Image id="Image22" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label23" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater23" runat="server">
													<ItemTemplate>
														<asp:Image id="Image23" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
										<TR>
											<TD vAlign="top" class="firstLeft"><asp:label id="Label24" runat="server"></asp:label></TD>
											<TD colspan="3"><asp:repeater id="Repeater24" runat="server">
													<ItemTemplate>
														<asp:Image id="Image24" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.IntValue"))) %>' ImageUrl="../Images/blue.gif" Height="1px" EnableViewState="False">
														</asp:Image><br>
													</ItemTemplate>
												</asp:repeater></TD>
										</TR>
									</table>
								    <div id="Div1" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>	
                                </div>
                                
                            </div>                   
                </div>
            </div>
        </div>
    </div>
</asp:Content>
