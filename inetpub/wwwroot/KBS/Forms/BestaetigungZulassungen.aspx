<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BestaetigungZulassungen.aspx.vb"
    Inherits="KBS.BestaetigungZulassungen" MasterPageFile="~/KBS.Master" %>
<%@ Import Namespace="GeneralTools.Models" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div id="site">
		<div id="content">
			<div id="navigationSubmenu">
				<asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
			</div>
			<div id="innerContent">
				<div id="innerContentRight" style="width: 100%">
					<div id="innerContentRightHeading">
						<h1>
							<asp:Label ID="lblHead" runat="server" Text="Bestätigung Zulassungen"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" Text="" runat="server"></asp:Label>
						</h1>
					</div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
					<div id="TableQuery">
						&nbsp;
					</div>
					<div id="Result" runat="Server">
                    <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                        <tr>
                            <td>
                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadGrid ID="rgGrid1" runat="server" AllowSorting="False" AllowPaging="False"
                                    AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default">
                                    <ClientSettings AllowKeyboardNavigation="true" >
                                        <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                    </ClientSettings>
                                    <ItemStyle CssClass="ItemStyle" />
                                    <AlternatingItemStyle CssClass="ItemStyle" />
                                    <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="False">
                                        <SortExpressions>
                                            <telerik:GridSortExpression FieldName="ID" SortOrder="Ascending" />
                                            <telerik:GridSortExpression FieldName="POSNR" SortOrder="Ascending" />
                                        </SortExpressions>
                                        <HeaderStyle ForeColor="#595959" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="ID" SortExpression="ID" HeaderText="ID" >
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="POSNR" SortExpression="POSNR" Visible="False" />
                                            <telerik:GridTemplateColumn HeaderText="Status">
                                                <HeaderStyle Width="50px" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# Eval("STATUS") %>' Font-Bold='<%# Eval("STATUS").ToString() = "E" OrElse Eval("STATUS").ToString() = "L" %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="KUNNR" SortExpression="KUNNR" HeaderText="Kundennr" >
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="NAME1" SortExpression="NAME1" HeaderText="Kundenname" >
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="MATNR" SortExpression="MATNR" Visible="False" />
                                            <telerik:GridBoundColumn DataField="MAKTX" SortExpression="MAKTX" HeaderText="Dienstleistung" >
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="GEB_POS" SortExpression="GEB_POS" Visible="False" />
                                            <telerik:GridTemplateColumn HeaderText="Gebühr">
                                                <HeaderStyle Width="60px" />
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" Text='<%# Eval("GEBUEHR", "{0:F}") %>' onKeyPress="return numbersonly(event, true)" Visible='<%# Eval("GEB_POS").ToString.ToInt(0) > 0 %>' AutoPostBack="True" OnTextChanged="gebuehrChanged"/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Zulassungsdatum">
                                                <HeaderStyle Width="80px" />
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" Text='<%# Eval("ZZZLDAT", "{0:ddMMyy}") %>' onKeyPress="return numbersonly(event, false)" MaxLength="6" Visible='<%# Eval("POSNR").ToString.ToInt(0) = 10 %>' AutoPostBack="True" OnTextChanged="zulassungsdatumChanged"/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Referenz">
                                                <HeaderStyle Width="90px" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# Eval("REFERENZ") %>' Visible='<%# Eval("POSNR").ToString.ToInt(0) = 10 %>'/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Kennzeichen">
                                                <HeaderStyle Width="80px" />
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" Text='<%# Eval("ZZKENN") %>' onkeyup="FilterKennz(this,event)" MaxLength="20" Visible='<%# Eval("POSNR").ToString.ToInt(0) = 10 %>' AutoPostBack="True" OnTextChanged="kennzeichenChanged"/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle Width="40px" />
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" Width="32" Height="32" ImageUrl="~/Images/delete01.jpg" CommandName="Del" ToolTip="Löschen"/>
                                                    <asp:ImageButton runat="server" Width="32" Height="32" ImageUrl="~/Images/haken_gruen.gif" CommandName="Ok" ToolTip="Erledigt" Visible='<%# Eval("STATUS").ToString() <> "L" %>'/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                    </table>
					</div>
					<div id="dataFooter">
						<asp:LinkButton ID="cmdSave" runat="server" CssClass="TablebuttonXLarge" Width="155px" Height="16px">» Speichern/Absenden</asp:LinkButton>   
				    </div>
			    </div>
		    </div>
	    </div>
	</div>
</asp:Content>
