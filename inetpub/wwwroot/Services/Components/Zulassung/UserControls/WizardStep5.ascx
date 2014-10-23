<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep5.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep5" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="~/PageElements/GridNavigation.ascx" %>

<table cellspacing="0" cellpadding="0">
    <tr>
        <td style="padding-bottom: 0px; width: 100%" class="PanelHead">
            <asp:Label ID="lbl_Fahrzeugdaten" runat="server">Übersicht</asp:Label>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="padding-top: 0px;">
            <asp:Label ID="Label11" runat="server" Text="Bitte überprüfen Sie Ihre Angaben."></asp:Label><br />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div style="min-height: 332px;">

                <table class="review" cellpadding="0" cellspacing="0" >
         <tr >
            <td>Regulierer:</td>
            <td><asp:Label runat="server" ID="lblReg"></asp:Label></td>
        </tr>
        <tr >
            <td>Rechnungsempfänger:</td>
            <td><asp:Label runat="server" ID="lblEmpf"></asp:Label></td>
        </tr>
        <tr>
            <td>Erstellt von:</td>
            <td><asp:Label runat="server" ID="lblErst"></asp:Label></td>
        </tr>
    </table> 

                <table cellpadding="0" class="review" width="875">
                    <tr class="StandardHeadDetail" style="cursor: default;">
                        <th align="left"><div style="padding-top: 0;">Halter</div></th>
                        <th align="left"><div style="padding-top: 0;">Versand Schein & Schilder</div></th>
                    </tr>
                    <tr>
                        <td valign="top" align="left">
                            <table cellpadding="0" cellspacing="0" class="review">
                                <tr>
                                    <td class="header">Name:</td>
                                    <td><asp:Label runat="server" ID="HalterName"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Name2:</td>
                                    <td><asp:Label runat="server" ID="HalterName2"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Straße, Nr.:</td>
                                    <td><asp:Label runat="server" ID="HalterStrasse"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">PLZ, Ort:</td>
                                    <td><asp:Label runat="server" ID="HalterPlz"></asp:Label>, <asp:Label runat="server" ID="HalterOrt"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Land:</td>
                                    <td><asp:Label runat="server" ID="HalterLand"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left">
                            <table cellpadding="0" cellspacing="0" class="review">
                                <tr>
                                    <td class="header">Name:</td>
                                    <td><asp:Label runat="server" ID="VersandName"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Name2:</td>
                                    <td><asp:Label runat="server" ID="VersandName2"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Straße, Nr.:</td>
                                    <td><asp:Label runat="server" ID="VersandStrasse"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">PLZ, Ort:</td>
                                    <td><asp:Label runat="server" ID="VersandPlz"></asp:Label>, <asp:Label runat="server" ID="VersandOrt"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Land:</td>
                                    <td><asp:Label runat="server" ID="VersandLand"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="StandardHeadDetail" style="cursor: default;">
                        <th align="left"><div style="padding-top: 0;">Versicherungsdaten</div></th>
                        <th align="left"><div style="padding-top: 0;">Versicherungsnehmer</div></th>
                    </tr>
                    <tr>
                        <td valign="top" align="left">
                            <table cellpadding="0" cellspacing="0" class="review">
                                <tr>
                                    <td class="header">Versicherungsgesellschaft:</td>
                                    <td><asp:Label runat="server" ID="VersicherungName"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">eVB Nummer:</td>
                                    <td><asp:Label runat="server" ID="VersicherungEvbNr"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Gültig von:</td>
                                    <td><asp:Label runat="server" ID="VersicherungVon"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Gültig bis:</td>
                                    <td><asp:Label runat="server" ID="VersicherungBis"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left">
                            <table cellpadding="0" cellspacing="0" class="review">
                                <tr>
                                    <td class="header">Name:</td>
                                    <td><asp:Label runat="server" ID="VersicherterName"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Name2:</td>
                                    <td><asp:Label runat="server" ID="VersicherterName2"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Straße, Nr.:</td>
                                    <td><asp:Label runat="server" ID="VersicherterStrasse"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">PLZ, Ort:</td>
                                    <td><asp:Label runat="server" ID="VersicherterPlz"></asp:Label>, <asp:Label runat="server" ID="VersicherterOrt"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Land:</td>
                                    <td><asp:Label runat="server" ID="VersicherterLand"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="StandardHeadDetail" style="cursor: default;">
                        <th colspan="2" align="left"><div style="padding-top: 0;">Zulassungsdaten</div></th>
                    </tr>
                    <tr>
                        <td colspan="2">
                      
                           <table cellpadding="0" cellspacing="0" class="review">
                                <tr>
                                    <td class="header">Zulassungsart:</td>
                                    <td><asp:Label runat="server" ID="Zulassungsart"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Zulassungsdatum:</td>
                                    <td><asp:Label runat="server" ID="Zulassungsdatum"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="header">Zulassungskreis:</td>
                                    <td><asp:Label runat="server" ID="Zulassungskreis"></asp:Label></td>
                                </tr>
                            </table>
                            
             <%--              <div id="Result" runat="Server">
                                <div id="data" style="float: none;">--%>
                                <div>
                                    <asp:GridView AutoGenerateColumns="False" Width = "100%" BackColor="White" runat="server" ID="GridView1"
                                        CssClass="GridView" GridLines="None" PageSize="10" AllowPaging="True" AllowSorting="True">
                                        <PagerSettings Visible="False" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <EditRowStyle CssClass="EditItemStyle" />
                                        <EditRowStyle></EditRowStyle>
                                        <Columns>
                                            <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                <HeaderStyle Width="125px" />
                                                <ItemStyle Width="125px" />
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField SortExpression="LIZNR" HeaderText="col_LIZNR">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_LIZNR" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_LIZNR</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLIZNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField SortExpression="WKZ1" HeaderText="col_Wunschkennz1">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Wunschkennz1" runat="server" CommandName="Sort" CommandArgument="Wunschkennz1">col_Wunschkennz1</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz1") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Wunschkennz2" HeaderText="col_Wunschkennz2">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Wunschkennz2" runat="server" CommandName="Sort" CommandArgument="Wunschkennz2">col_Wunschkennz2</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz2") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Wunschkennz3" HeaderText="col_Wunschkennz3">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Wunschkennz3" runat="server" CommandName="Sort" CommandArgument="Wunschkennz3">col_Wunschkennz3</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wunschkennz3") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>          
                                            <asp:TemplateField SortExpression="ResNr" HeaderText="col_ResNr">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ResNr" runat="server" CommandName="Sort" CommandArgument="ResNr">col_ResNr</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResNr") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>   
                                            <asp:TemplateField SortExpression="ResName" HeaderText="col_ResName">
                                                <HeaderStyle Width="145px" />
                                                <ItemStyle Width="145px" />
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ResName" runat="server" CommandName="Sort" CommandArgument="ResName">col_ResName</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                                                                                                                       
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="Label8" runat="server" ForeColor="Red">Es wurde kein Fahrzeug ausgewählt.</asp:Label>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                               </div>
                               <%-- <div id="pagination">--%>
                              <div>
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                               </div>
                            <%--</div>--%>
                           <br/>
                        </td>
                    </tr>
                    <tr class="StandardHeadDetail" style="cursor: default;">
                        <th align="left"><div style="padding-top: 0;">Dienstleistungen</div></th>
                        <th align="left"><div style="padding-top: 0;">Dokumente</div></th>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <asp:Repeater runat="server" ID="Services">
                                    <ItemTemplate><tr><td><%# DataBinder.Eval(Container, "DataItem.DIENSTL_TEXT")%></td></tr></ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                        <td>
                            <table>
                                <asp:Repeater runat="server" ID="Documents">
                                    <ItemTemplate><tr><td><asp:Image runat="server" Style="vertical-align: middle;" ImageUrl="~/Images/Zulassung/pdf_icon.png" />&nbsp;<a target="_blank" href='<%# ResolveDownloadUrl(DataBinder.Eval(Container, "DataItem.Filename") as string) %>'><%# DataBinder.Eval(Container, "DataItem.Filename")%></a></td></tr></ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr id="DokumenteKopf" runat="server" visible="false" class="StandardHeadDetail" style="cursor: default;">
                        <th colspan="2" align="left"><div style="padding-top: 0;">Vorhandene Dokumente</div></th>
                    </tr>
                    <tr id="DokumenteDetail" runat="server" visible="false">
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td><asp:CheckBox ID="cbxVollmancht" runat="server" Enabled="false" Text="Vollmacht" Font-Bold="true" /></td>
                                    <td><asp:CheckBox ID="cbxRegister" runat="server" Enabled="false" Text="HRG" Font-Bold="true" /></td>
                                    <td><asp:CheckBox ID="cbxPerso" runat="server" Enabled="false" Text="Personalausweis" Font-Bold="true" /></td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="cbxGewerbe" runat="server" Enabled="false" Text="Gewerbeanmeld." Font-Bold="true" /></td>
                                    <td><asp:CheckBox ID="cbxEinzug" runat="server" Enabled="false" Text="Einzugserm." Font-Bold="true" /></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="3"><asp:CheckBox ID="cbxVollst" runat="server" Enabled="false" Text="Vollst." Font-Bold="true" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="rightAlignedNav separator" style="width: 895px;">
                <asp:LinkButton runat="server" ID="buttonNext" Text="Absenden" onclick="buttonSubmit_Click" CssClass="blueButton" />
                <asp:LinkButton ID="LinkButton1" runat="server" Text="&lt; Zur&uuml;ck" onclick="LinkButton1_Click" CssClass="blueButton" />
            </div>
        </td>
    </tr>
</table>