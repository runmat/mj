<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change04_2.aspx.vb" Inherits="AppPorsche.Change04_2"  MasterPageFile="../../../MasterPage/Services.Master" %>
<%@ Register TagPrefix="uc2" TagName="menue" Src="MenuePorsche.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
		<div id="site">
		<div id="content">
				<div id="navigationSubmenu">
	                <asp:hyperlink id="lnkKreditlimit" runat="server" NavigateUrl="Change04.aspx">Fahrzeugsuche</asp:hyperlink>
	                <a class="active" >| Fahrzeugauswahl</a>
				</div>
		
				<div id="innerContent">

	
					<div id="innerContentRight" style="width:100%;">
						<div id="innerContentRightHeading">
							<h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
														
						</div>
                        <uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten>
						<div id="TableQuery">
						 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                        <table id="tab1" cellpadding="0" cellspacing="0" >
						<tbody>
                        
                            <tr  class="formquery">
                                <td class="firstLeft active" colspan="2">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" >
                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap" style="width: 182px">
                                    <strong>Nachricht für Briefempfänger</strong>*<strong>:</strong>&nbsp;</td>
                                <td class="firstLeft active" style="width: 76%">
                                    <asp:TextBox ID="txtKopf" runat="server" CssClass="InputTextbox" Width="120px"></asp:TextBox>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="TablebuttonLarge" 
                                        Visible="False">&#8226;&nbsp;Kopftext erf.</asp:LinkButton>
                                </td>
                            </tr>
                            <tr id="ShowScript" runat="server" class="formquery">
                                <td>
                                 
                                    </td>
                                <td   >
                                    <asp:Literal ID="Literal2" runat="server"></asp:Literal></td>
                            </tr>
                            </tbody>
                        </table>
                                     </ContentTemplate>
                                     </asp:UpdatePanel>							
						</div>
						<div id="Result"  runat="Server">
                                               <div id="pagination">
                                                    <uc2:GridNavigation id="GridNavigation1" runat="server" ></uc2:GridNavigation>
                                                </div>
                        <div id="data">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                            CellPadding="0" cellSpacing="0" GridLines="None"
                                            AlternatingRowStyle-BackColor="#DEE1E0" AllowSorting="true" 
                                            AllowPaging="True" CssClass="GridBorderBottom" >
                                            <HeaderStyle CssClass="GridTableHead" ForeColor="White"  />
                                            <AlternatingRowStyle CssClass="GridTableAlternate" ></AlternatingRowStyle>
                                            <PagerSettings  Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            
                                            <EmptyDataRowStyle  BackColor="#DFDFDF"/>
                                            <Columns>
                                                <asp:BoundField DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnummer">
                                                </asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="MANDT" SortExpression="MANDT" HeaderText="MANDT">
                                                    </asp:BoundField>
                                                <asp:BoundField DataField="LIZNR" SortExpression="LIZNR" HeaderText="Kontonummer">

                                                </asp:BoundField>
                                                <asp:BoundField DataField="TIDNR" SortExpression="TIDNR" HeaderText="Nummer ZB2">

                                                </asp:BoundField>
                                                <asp:BoundField DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" HeaderText="Kennzeichen">

                                                </asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1"
                                                    HeaderText="Ordernr.">

                                                </asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="TEXT50" SortExpression="TEXT50" HeaderText="Kopftext">

                                                </asp:BoundField>
                                                <asp:TemplateField Visible="False" SortExpression="ZZBEZAHLT" HeaderText="Bezahlt">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkBezahlt" runat="server" Enabled="False"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZBEZAHLT") %>'> </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZCOCKZ" HeaderText="CoC">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Checkbox1" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="Textbox2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'> </asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nicht<br /> anfordern">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="chkNichtAnfordern" runat="server" GroupName="Kontingentart">
                                                        </asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stnd.<br /> temp.">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="chk0001" runat="server" GroupName="Kontingentart"></asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stnd.<br /> endg.">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="chk0002" runat="server" GroupName="Kontingentart"></asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False" HeaderText="DP<br /> endg.">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="chk0004" runat="server" GroupName="Kontingentart"></asp:RadioButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="TEXT200" HeaderText="Referenz*">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPosition"  runat="server" MaxLength="15" Width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.TEXT200") %>' CssClass="TextBoxNormal"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                                                                       
                            </asp:GridView>
                                        
                        </div>
                       
                    </div>
						
						
						<div id="dataFooter">
                                        &nbsp;<asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" 
                                        Height="20px" Width="78px">» Weiter</asp:LinkButton></div></div></div></div></div></div></asp:Content>