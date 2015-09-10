<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BestaetigungZulassungen.aspx.vb"
    Inherits="KBS.BestaetigungZulassungen" MasterPageFile="~/KBS.Master" %>
<%@ Import Namespace="GeneralTools.Models" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="../Java/JScript.js"></script>
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
                                        <Scrolling ScrollHeight="410px" AllowScroll="True" UseStaticHeaders="True" />
                                    </ClientSettings>
                                    <ItemStyle CssClass="ItemStyle" />
                                    <AlternatingItemStyle CssClass="ItemStyle" />
                                    <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="False">
                                        <HeaderStyle ForeColor="#595959" />
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="ID" SortExpression="ID" HeaderText="ID" UniqueName="ID" >
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="POSNR" SortExpression="POSNR" Visible="False" UniqueName="POSNR" />
                                            <telerik:GridTemplateColumn HeaderText="Status">
                                                <HeaderStyle Width="45px" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# Eval("STATUS") %>' Font-Bold='<%# Eval("STATUS").ToString() = "E" OrElse Eval("STATUS").ToString() = "L" %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="KUNNR" SortExpression="KUNNR" HeaderText="Kundennr." >
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="NAME1" SortExpression="NAME1" HeaderText="Kundenname" >
                                                <HeaderStyle Width="120px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="MATNR" SortExpression="MATNR" Visible="False" />
                                            <telerik:GridBoundColumn DataField="MAKTX" SortExpression="MAKTX" HeaderText="Dienstleistung" >
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="GEB_POS" SortExpression="GEB_POS" Visible="False" UniqueName="GEB_POS" />
                                            <telerik:GridTemplateColumn HeaderText="Gebühr">
                                                <HeaderStyle Width="65px" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGebuehr" runat="server" Width="50px" Text='<%# Eval("GEBUEHR", "{0:F}") %>' onKeyPress="return numbersonly(event, true)" 
                                                        Visible='<%# Eval("GEB_POS").ToString.ToInt(0) > 0 %>' style="text-align: right"/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Zul.datum">
                                                <HeaderStyle Width="70px" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtZulassungsdatum" runat="server" Width="55px" Text='<%# Eval("ZZZLDAT", "{0:ddMMyy}") %>' onKeyPress="return numbersonly(event, false)" MaxLength="6" 
                                                        Visible='<%# Eval("POSNR").ToString.ToInt(0) = 10 %>'/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Referenz">
                                                <HeaderStyle Width="120px" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# Eval("REFERENZ") %>' Visible='<%# Eval("POSNR").ToString.ToInt(0) = 10 %>'/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Kennzeichen">
                                                <HeaderStyle Width="115px" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" Width="100px" Text='<%# Eval("ZZKENN") %>' onkeyup="FilterKennz(this,event)" MaxLength="20" 
                                                        Visible='<%# Eval("POSNR").ToString.ToInt(0) = 10 %>'/>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle Width="55px" />
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ImageUrl="~/Images/delete01.gif" CommandName="Del" ToolTip="Löschen" Visible='<%# Eval("POSNR").ToString.ToInt(0) = 10 %>'/>
                                                    <asp:ImageButton runat="server" ImageUrl="~/Images/haken_gruen.gif" CommandName="Ok" ToolTip="Erledigt" 
                                                        Visible='<%# Eval("STATUS").ToString() <> "L" AndAlso Eval("POSNR").ToString.ToInt(0) = 10 %>' style="margin-left: 2px"/>
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
