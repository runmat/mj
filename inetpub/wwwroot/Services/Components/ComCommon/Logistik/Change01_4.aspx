<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_4.aspx.vb" Inherits="CKG.Components.ComCommon.Change01_4"
MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change01.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;<asp:HyperLink
                    ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change01_2.aspx">| Fahrzeugauswahl</asp:HyperLink>
                <asp:HyperLink ID="lnkAdressAuswahl" runat="server" NavigateUrl="Change01_3.aspx"> | Adressauswahl</asp:HyperLink>
                <a class="active">|
                    <asp:Label ID="lblAddress" runat="server" Visible="False"></asp:Label><asp:Label
                        ID="lblMaterialNummer" runat="server" Visible="False"></asp:Label></a>
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
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%" style="width: 100%">
                                        <tr id="tr_Message" runat="server" class="formquery">
                                            <td colspan="2" class="firstLeft active">
                                                <p>
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Black" Font-Bold="True"></asp:Label>
                                                    <asp:Label ID="lblerror" runat="server" CssClass="TextError"></asp:Label></p>
                                            </td>
                                        </tr>
                                        
                                        <tr id="tr_VersandAdresse" class="formquery" runat="server">
                                            <td class="firstLeft active">
                                                
                                                <asp:label id="lblVersandAdresse" runat="server" >Abholort:</asp:label>
                                            
                                            </td>
                                                     <td>
                                                <asp:label id="lblAbholortShow" runat="server"></asp:label>
                                                
                                                </td>
                                        </tr>
                                        
                                        <tr class="formquery" id="tr_Versandart" runat="server">
                                            <td class="firstLeft active" vAlign="top">
                                                <asp:label id="lblAnlieferort" runat="server">Anlieferort:</asp:label>
                                            </td>
                                            <td>
                                                <asp:label id="lblAnlieferortShow" runat="server"></asp:label>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="tr_Versandgrund" runat="server">
                                            <td class="firstLeft active" vAlign="top">
                                                <asp:label id="lblAuftragsart" runat="server">Auftragsart:</asp:label>
                                            </td>
                                            <td>
                                                <asp:label id="lblAuftragsartShow" runat="server"></asp:label>
                                            </td>
                                        </tr>
                                        
                                        <tr class="formquery" id="tr_Fahrzeuge" runat="server">
                                            <td class="firstLeft active">
                                                <asp:label id="lblFahrzeuge" runat="server" Visible="False">lbl_Fahrzeuge</asp:label>
                                            </td>
                                            <td class="active">
                                                <div id="data">                                            
                                                <asp:GridView ID="gvResult" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="True" AllowPaging="True" CssClass="GridView" PageSize="20" >
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White"  />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        
																	<asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
																		<HeaderTemplate>
																			<asp:LinkButton id="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
																		</HeaderTemplate>
																		<ItemTemplate>
																			<asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
																			</asp:Label>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:TemplateField SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
																		<HeaderTemplate>
																			<asp:LinkButton id="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
																		</HeaderTemplate>
																		<ItemTemplate>
																			<asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
																			</asp:Label>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:TemplateField SortExpression="ZZHANDELSNAME" HeaderText="col_Fahrzeugtyp">
																		<HeaderTemplate>
																			<asp:LinkButton id="col_Fahrzeugtyp" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Fahrzeugtyp</asp:LinkButton>
																		</HeaderTemplate>
																		<ItemTemplate>
																			<asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHANDELSNAME") %>'>
																			</asp:Label>
																		</ItemTemplate>
																	</asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="REPLA_DATE" HeaderText="col_Mietende">
                                                                        <HeaderTemplate>
                                                                            <asp:LinkButton ID="col_Mietende" runat="server" CommandName="Sort" CommandArgument="REPLA_DATE">col_Mietende</asp:LinkButton></HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.REPLA_DATE", "{0:d}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>																	
																	<asp:TemplateField  HeaderText="col_Auftragsart">
																		<HeaderTemplate>
																			<asp:LinkButton id="col_Auftragsart" runat="server" CommandName="Sort" >col_Auftragsart</asp:LinkButton>
																		</HeaderTemplate>
																		<ItemTemplate>
																			<asp:Label ID="Label5" runat="server" >
																			</asp:Label>
																		</ItemTemplate>
																	</asp:TemplateField>
																	<asp:TemplateField SortExpression="STATUS" HeaderText="col_Status">
																		<HeaderTemplate>
																			<asp:LinkButton id="col_Status" runat="server" CommandName="Sort" CommandArgument="STATUS">col_Status</asp:LinkButton>
																		</HeaderTemplate>
																		<ItemTemplate>
																			<asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.STATUS") %>'>
																			</asp:Label>
																		</ItemTemplate>
																	</asp:TemplateField>
																															

                                                    </Columns>
                                                </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="tr1" runat="server">
                                            <td class="firstLeft active" style="vertical-align:top">
                                                <asp:label id="lblHinweis" runat="server" >Hinweis:</asp:label>
                                            </td>
                                            <td>
                                                Bitte beachten Sie die Rücknahmestandards des jeweiligen <br />
                                                 Herstellers. Informationen zu den Standards erhalten Sie hier:&nbsp; 
                                                <asp:DropDownList ID="ddlStandards" runat="server" AutoPostBack="True" 
                                                    CssClass="DropDowns" Width="300px" >
                                                </asp:DropDownList>
                                                
                                                    <asp:LinkButton ID="lnkStandards"  CssClass="MainmenuLink" Visible="false" runat="server">Anzeigen</asp:LinkButton>
                                                </td>
                                        </tr>
                                        <tr class="formquery" id="tr2" runat="server">
                                            <td class="firstLeft active" vAlign="top">
                                                &nbsp;</td>
                                            <td>
                                                <asp:Label ID="lblEntfernung" runat="server" Text="lblEntfernung"></asp:Label>
                                                &nbsp;<asp:HyperLink ID="lnkPreisStaffel" runat="server">lnkPreisStaffel</asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trAGB" runat="server">
                                        <td class="firstLeft active" vAlign="top">
                                                </td>
                                            <td>
                                                <a href="#" style="text-decoration:underline" class="MainmenuLink" onclick="window.open('Dokumente/DAD_AGB.pdf')">AGB</a>
                                                &nbsp;
                                                <span><asp:CheckBox ID="chkAgb" runat="server" 
                                                    Text="Ich habe die AGB gelesen und akzeptiere diese" AutoPostBack="True" /></span>
                                            </td>                                        
                                        </tr>
                                        <tr class="formquery" id="trButton" runat="server">
                                            <td colspan="2" align="right" class="rightPadding" style="width: 100%">
                                                <div id="dataQueryFooter">
                                                    <asp:LinkButton ID="cmdSend" runat="server" CssClass="Tablebutton" Width="78px"
                                                        Height="16px" EnableTheming="False" EnableViewState="False" 
                                                        Enabled="False">» Absenden</asp:LinkButton>
                                                    &nbsp;
                                                </div>
                                            </td>
                                        </tr>
                                </table>
                            </div>
                         </ContentTemplate>
                    </asp:UpdatePanel>
                           
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
