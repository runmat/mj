<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AHVersandListe.aspx.cs" Inherits="AppZulassungsdienst.forms.AHVersandListe" MasterPageFile="../MasterPage/Big.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <script src="/PortalZLD/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="JavaScript" type="text/javascript">
        $(document).ready(function () {
            $("#dialog").dialog({
                bgiframe: true,
                autoOpen: false,
                height: 150,
                modal: true,
                draggable: false,
                resizable: false,
                buttons: {
                    Abbrechen: function () {
                        $(this).dialog('close');
                    },
                    Absenden: function () {
                        $(this).dialog('close');
                        __doPostBack('ctl00$ContentPlaceHolder1$cmdSend', '');

                    }
                }
            });
            $('#ctl00_ContentPlaceHolder1_cmdSend').click(function () {
                $('#dialog').dialog('open');
            });
        });</script>
	<div id="site">
		<div id="content">
			<div id="navigationSubmenu">
				<asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
					Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
			</div>
			<div id="innerContent">
				<div id="innerContentRight" style="width: 100%">
					<div id="innerContentRightHeading">
						<h1>
							<asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
						</h1>
					</div>
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
						<ContentTemplate>
                        							<div id="TableQuery" style="margin-bottom: 10px">
								<table id="tab1"  runat="server" cellpadding="0" cellspacing="0">
									<tbody>
										<tr class="formquery">
											<td class="firstLeft active"  colspan="6" style="vertical-align:top">
												<asp:Label ID="lblError"  runat="server" CssClass="TextError"></asp:Label>
												<asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
											</td>
										</tr>
                                                                              
									</tbody>
								</table>

							</div>
							<div id="Result" runat="Server">
								<div id="data">
									<table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
										<tr>
											<td>
                                                <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" DataKeyNames="ZULBELN"
                                                    AllowSorting="true" AllowPaging="false" CssClass="GridView" PageSize="1000" 
                                                    onsorting="GridView1_Sorting" onrowcommand="GridView1_RowCommand">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <RowStyle CssClass="ItemStyle"/>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <PagerSettings Visible="False" />
													<Columns>
														<asp:TemplateField  SortExpression="ZULBELN"  HeaderText="ID">
															<ItemTemplate>
															    <asp:Label ID="lblSapId" runat="server" Text='<%# Eval("ZULBELN") %>'/>
															</ItemTemplate>
															<HeaderStyle Width="50px" />
															<ItemStyle Width="50px" />
														</asp:TemplateField> 
														<asp:TemplateField SortExpression="toDelete"  HeaderText="Löschen">
															<ItemTemplate>
															    <asp:Label ID="lblLoeschKZ" runat="server" Text='<%# Eval("toDelete").ToString() %>'/>
														    </ItemTemplate>
															<HeaderStyle Width="30px" />
															<ItemStyle Width="30px" />    
														</asp:TemplateField>
														<asp:TemplateField SortExpression="KUNNR" HeaderText="Kundennr.">
															<ItemTemplate>
															    <asp:Label ID="lblKundennr" runat="server" Text='<%# Eval("KUNNR") %>'/>
															</ItemTemplate>
														</asp:TemplateField>
														<asp:TemplateField SortExpression="NAME1" HeaderText="Kundenname">
															<ItemTemplate>
															    <asp:Label ID="lblKundenname" runat="server" Text='<%# Eval("NAME1") %>'/>
															</ItemTemplate>
														</asp:TemplateField> 
														<asp:TemplateField SortExpression="MAKTX" HeaderText="Dienstleistung">
														    <ItemTemplate>
														        <asp:Label ID="lblMatbez" runat="server" Text='<%# Eval("MAKTX") %>'/>
														    </ItemTemplate>
													    </asp:TemplateField>
														<asp:TemplateField SortExpression="ZZZLDAT" HeaderText="Zulassungsdatum">
															<ItemTemplate>
															    <asp:Label ID="lblDate" runat="server" Visible='<%# Eval("ZULPOSNR").ToString() == "10" %>' Text='<%# Eval("ZZZLDAT", "{0:d}") %>'/>
															</ItemTemplate>
														</asp:TemplateField>
														<asp:TemplateField SortExpression="ZZREFNR1" HeaderText="Referenz1">
															<ItemTemplate>
															    <asp:Label ID="lblReferenz1"  runat="server" Visible='<%# Eval("ZULPOSNR").ToString() == "10" %>' Text='<%# Eval("ZZREFNR1") %>'/>
															</ItemTemplate>
														</asp:TemplateField>  
														<asp:TemplateField SortExpression="Kennzeichen" HeaderText="Kennzeichen">
															<ItemTemplate>
															    <asp:Label ID="lblKennz"  runat="server" Visible='<%# Eval("ZULPOSNR").ToString() == "10" %>' Text='<%# Eval("ZZKENN") %>'/>
															</ItemTemplate>
														</asp:TemplateField> 
														<asp:TemplateField >
															 <ItemTemplate>
																 <asp:ImageButton ID="ibtnedt" Visible='<%# Eval("toDelete").ToString() != "L" %>' ImageUrl="/PortalZLD/images/Edit.gif" CommandArgument='<%# Eval("ZULBELN") %>'  runat="server" CommandName="Edt" ToolTip="Bearbeiten" Width="16px" Height="16px" />
																 <asp:ImageButton ID="ibtnDel" ImageUrl="/PortalZLD/images/del.png" CommandArgument='<%#  ((GridViewRow)Container).RowIndex %>'  runat="server" CommandName="Del" ToolTip="Löschen" />
															</ItemTemplate>
														</asp:TemplateField>
													</Columns>
												</asp:GridView>
											</td>
										</tr>
									</table>
								</div>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
					<div id="dataFooter">
						<asp:LinkButton ID="cmdSend" runat="server" OnClientClick="javascript: return false;" CssClass="Tablebutton" Width="78px" onclick="cmdSend_Click"
						   >» Absenden</asp:LinkButton>
				</div>
                     <div id="dialog" title="Absenden">
                        Sollen die markierten Aufträge jetzt gelöscht werden?
                    </div>
			</div>
		</div>
	</div>
	</div>
</asp:Content>
