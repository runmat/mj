<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_4.aspx.vb" Inherits="AppPorsche.Change04_4"   MasterPageFile="../MasterPage/Porsche.Master" %>
<%@ Register TagPrefix="uc2" TagName="menue" Src="MenuePorsche.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<meta name="author" content="Christoph Kroschke AG" />
</asp:Content>
 <asp:Content ID="ContentMenue" ContentPlaceHolderID="ContentPlaceHolderMenue" runat="server">
     <uc2:menue ID="menue"  runat="server"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
		<div id="site">
		<div id="content">
				<div id="navigationSubmenu">
	                <asp:hyperlink id="lnkFahrzeugsuche" runat="server" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:hyperlink>&nbsp;<asp:hyperlink id="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change04_2.aspx">| Fahrzeugauswahl</asp:hyperlink>
				    <asp:HyperLink ID="lnkAdressAuswahl" runat="server" CssClass="TaskTitle" 
                        NavigateUrl="Change04_3.aspx" Visible="False"> | Adressauswahl</asp:HyperLink>
                        <a class="active" >| Senden</a>
				</div>
		
				<div id="innerContent">

	
					<div id="innerContentRight" style="width:100%;">
						<div id="innerContentRightHeading">
							<h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
														
						</div>
						<ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
                        EnableScriptLocalization="true" ID="ScriptManager1" />

                        <uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate> 
						    <div id="TableQuery">
                                            <table id="tblMessage" runat="server" cellpadding="0" cellspacing="0">

                                                <tr>
                                                    <td colspan="4">
                                                       <asp:Label ID="lblError" runat="server" CssClass="TextError" Visible="False"></asp:Label></td>
                                                         
                                                </tr>
                                                <tr class="formquery">
                                                    <td nowrap="nowrap" class="firstLeft active" colspan="3">
                                                        <u>Hinweis:</u>&nbsp;Überzählige Anforderungen werden gesperrt angelegt.
                                                    </td>
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" colspan="3" nowrap="nowrap">
                                                        &nbsp;</td>
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap" >
                                                        Kontingentart <u>Standard temporär</u>:
                                                    </td>
                                                    <td class="active" nowrap="nowrap">
                                                        <strong>
                                                            <asp:Label ID="lblTemp" runat="server"></asp:Label></strong>
                                                    </td>
                                                    <td class="firstLeft active" nowrap="nowrap" >
                                                        überzählige Anforderung(en)
                                                    </td>
                                                    <td class="active" nowrap="nowrap" width="100%">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap" >
                                                        Kontingentart <u>Standard endgültig</u>:
                                                    </td>
                                                    <td class="active" nowrap="nowrap" >
                                                      
                                                            <asp:Label ID="lblEnd" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="firstLeft active" nowrap="nowrap" >
                                                        überzählige Anforderung(en)&nbsp;
                                                    </td>
                                                    <td class="active" nowrap="nowrap" width="100%">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" colspan="3" nowrap="nowrap">
                                                        <i>
                                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                                        </i>&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td class="firstLeft active" nowrap="nowrap">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                            <table id="Table7" cellspacing="0" cellpadding="5" width="100%" border="0" bgcolor="white">
                                                                                                     
                                                <tr class="formquery">
                                                    <td class="firstLeft active" valign="top">
                                                        Versandart:
                                                    </td>
                                                    <td class="active" style="width: 90%">
                                                        <asp:Label ID="lblVersandart" runat="server"></asp:Label><asp:Label ID="lblMaterialNummer"
                                                            runat="server" Visible="False"></asp:Label><asp:Label ID="lblVersandhinweis" runat="server"
                                                                Visible="False"> - Gilt nicht für gesperrt angelegte Anforderungen!</asp:Label>
                                                    </td>
                                                    <td class="active" nowrap="nowrap">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Adresse:
                                                    </td>
                                                    <td class="active" style="width: 90%">
                                                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="active" nowrap="nowrap">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
    							
						    </div>
						    <div id="pagination">
                                <uc2:GridNavigation id="GridNavigation1" runat="server" ></uc2:GridNavigation>
                            </div>
						    <div id ="data">
                                <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False"
                                    CellPadding="3" GridLines="None" CssClass="GridBorderBottom" AlternatingItemStyle-BackColor="#DEE1E0"
                                    AllowPaging="True" AllowSorting="true"  >
                                                <HeaderStyle CssClass="GridTableHead" ForeColor="White"  />
                                                <AlternatingItemStyle CssClass="GridTableAlternate" />
                                                <PagerStyle  Visible="False" />
                                                <ItemStyle CssClass="ItemStyle" />
                                    <Columns>
                                        <asp:BoundColumn DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer">
                                          </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="LIZNR" SortExpression="LIZNR" HeaderText="Kontonummer">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="TIDNR" SortExpression="TIDNR" HeaderText="Nummer ZB2">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kennzeichen">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1"
                                            HeaderText="Ordernr."></asp:BoundColumn>
                                        <asp:TemplateColumn Visible="False" HeaderText="Bezahlt">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Bezahlt" runat="server" Enabled="False"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="CoC" HeaderStyle-ForeColor="#FFFFFF">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ZZCOCKZ" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'
                                                    Enabled="False"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="VBELN" SortExpression="VBELN" HeaderText="Auftragsnr.">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="COMMENT" SortExpression="COMMENT" HeaderText="Kommentar">
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Standard<br /> tempor&#228;r" HeaderStyle-ForeColor="#FFFFFF">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk0001" runat="server" Enabled="False"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Standard<br /> endg&#252;ltig" HeaderStyle-ForeColor="#FFFFFF">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk0002" runat="server" Enabled="False"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn Visible="False" HeaderText="DP<br /> endg." HeaderStyle-ForeColor="#FFFFFF">
                                            <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                            <ItemStyle CssClass="BoundField"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk0004" runat="server" Enabled="False"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn Visible="False" HeaderText="Kontingentart"></asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Bold="True" PrevPageText="&amp;lt;vorherige"
                                        HorizontalAlign="Left"  Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                          
						    </div>						
						    <div id="dataFooter">
                                            &nbsp;<asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" 
                                            Height="20px" Width="78px">» Absenden</asp:LinkButton>

                                        </div>
                             </ContentTemplate>
                        </asp:UpdatePanel>   
					</div>

				</div>
			</div>
	</div>
</div> 
</asp:Content>