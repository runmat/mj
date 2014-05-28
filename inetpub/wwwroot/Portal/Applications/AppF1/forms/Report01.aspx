<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01.aspx.vb" Inherits="AppF1.Report01" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../controls/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>


<%@ Register src="../controls/SucheHaendler.ascx" tagname="SucheHaendler" tagprefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

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
									</td>
							</TR>
							<tr>
								<TD vAlign="top" width="100">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">
												&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
												<asp:LinkButton id="cmdSearch" OnClientClick="Show_BusyBox1();" runat="server" 
                                                    CssClass="StandardButton" Visible="False">Weiter</asp:LinkButton></TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
                            <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" Visible="false" CssClass="StandardButton">Neue Suche</asp:LinkButton>
                                            </TD>
										</TR>
										<TR>
											<TD vAlign="middle" width="150">
												<asp:LinkButton id="cmdBack" runat="server" 
                                                    CssClass="StandardButton">Zurück</asp:LinkButton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="style1" colspan="2"><uc1:kopfdaten id="Kopfdaten1" runat="server"></uc1:kopfdaten>
											</td>
										</tr>
										<tr>
										    <td class="style2" colspan="1">
										        
                                        <asp:Label runat="server" ID="lbl_AnzeigeHaendlerSuche" Font-Underline="true" Font-Bold="true"
                                            ForeColor="Black" Font-Size="Larger" Text="lbl_AnzeigeHaendlerSuche"></asp:Label>
										        
										    </td>
										</tr>
										<tr>
										    <td class="style1" colspan="2">
										        <uc2:SucheHaendler ID="SucheHaendler1" runat="server" />
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
                                                    AllowPaging="True" AutoGenerateColumns="False" Width="100%" PageSize="50" 
                                                    BackColor="White" CssClass="tableMain">
                                                    <PagerSettings Position="Top" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="col_Haendler" Visible="False" 
                                                            SortExpression="NAME1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Haendler" runat="server" CommandArgument="HAENDLER" 
                                                                    CommandName="Sort">col_Haendler</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("NAME1") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_FIN" SortExpression="CHASSIS_NUM">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_FIN" runat="server" CommandArgument="CHASSIS_NUM" 
                                                                    CommandName="Sort">col_FIN</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="lnkHistorie" runat="server" Target="_blank" 
                                                                    Text='<%# Bind("CHASSIS_NUM") %>'></asp:HyperLink>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Briefnummer" SortExpression="TIDNR">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Briefnummer" runat="server" CommandArgument="TIDNR" 
                                                                    CommandName="Sort">col_Briefnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("TIDNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_ProduktFzgKlasse" SortExpression="ZZFINART_TEXT">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_ProduktFzgKlasse" runat="server" CommandArgument="ZZFINART_TEXT" 
                                                                    CommandName="Sort">col_ProduktFzgKlasse</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("ZZFINART_TEXT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Kennzeichen" SortExpression="LICENSE_NUM">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="LICENSE_NUM" 
                                                                    CommandName="Sort">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("LICENSE_NUM") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_COC" SortExpression="ZZCOCKZ">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_COC" runat="server" CommandArgument="ZZCOCKZ" 
                                                                    CommandName="Sort">col_COC</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("ZZCOCKZ") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Buchungsdatum" SortExpression="DATAB">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Buchungsdatum" runat="server" CommandArgument="DATAB" 
                                                                    CommandName="Sort">col_Buchungsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DATAB", "{0:d}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_HSN" SortExpression="ZZHERSTELLER_SCH">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_HSN" runat="server" CommandArgument="ZZHERSTELLER_SCH" 
                                                                    CommandName="Sort">col_HSN</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHSN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHERSTELLER_SCH")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_TSN" SortExpression="ZZTYP_SCHL">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_TSN" runat="server" CommandArgument="ZZTYP_SCHL" 
                                                                    CommandName="Sort">col_TSN</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTSN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZTYP_SCHL")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_Hubraum" SortExpression="ZZHUBRAUM">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Hubraum" runat="server" CommandArgument="ZZHUBRAUM" 
                                                                    CommandName="Sort">col_Hubraum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHubraum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHUBRAUM")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="col_KW" SortExpression="ZZNENNLEISTUNG">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_KW" runat="server" CommandArgument="ZZNENNLEISTUNG" 
                                                                    CommandName="Sort">col_KW</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKW" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZNENNLEISTUNG")%>'></asp:Label>
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
									<strong><asp:label id="lblHinweis" runat="server" ></asp:label></strong>
								</td>
							</tr>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	
	</body>
</HTML>