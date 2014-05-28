<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelectionStatistikGrid.aspx.vb" Inherits="CKG.Services.Vorlage5" MasterPageFile="../MasterPage/Services.Master" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div id="site">
        <div id="content">
            <div id="navigationSubmenu">&nbsp;
                </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">

                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    
                  
                    <div id="paginationQuery" style="width: 100%; display: block;">
                        
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="active">
                                            Neue Abfrage starten
                                        </td>
                                        <td align="right">
                                            <div id="queryImage">
                                                <input type="button" id="NewSearch" onclick="ShowHide('tab1');" alt="Neue Abfrage"
                                                    style="border: none; background-image: url(../../../Images/queryArrow.gif); width: 16px;
                                                    height: 16px; cursor: hand;" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div> 
                        <div id="TableQuery" style="margin-bottom: 10px">
                        <table id="tab1" cellpadding="0" cellspacing="0" >
                        <tfoot ><tr><td colspan="4">&nbsp;</td></tr></tfoot>
						<tbody>
                        
                            <tr>
                                <td >
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" width="100%">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td  nowrap="nowrap" class="firstLeft active" >
                                    Versicherungsjahr:
                                </td>
                                <td class="firstLeft active">
                                    
                                    <asp:DropDownList  ID="drpVJahr" runat="server" Width="122px" 
                                        Font-Names="Verdana,sans-serif" CssClass="DropDownNormal"
                                        >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td  nowrap="nowrap" class="firstLeft active" >
                                   VD-Bezirk:
                                </td>
                                <td nowrap="nowrap" class="firstLeft active"  >
                                    <asp:TextBox ID="txtOrgNr" runat="server" Width="120px" CssClass="InputTextbox"></asp:TextBox>
                                    <asp:Label ID="lblPlatzhaltersuche" runat="server">*(mit Platzhaltersuche)</asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td >
                                    &nbsp;
                                </td>
                                <td  align="right" class="rightPadding" >
                                    <asp:LinkButton ID="btnConfirm" runat="server" CssClass="Tablebutton" 
                                        Height="16px" Width="78px">» Weiter</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            </tbody>
                        </table>
                      <div id="Stats" runat="Server" visible="false"> 
                     <div id="statistics" >
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        Stand:
                                        <asp:Label ID="lblStand" runat="server" Width="116px"></asp:Label>
                                    </td>
                                    <td>
                                        Verkaufte Kennzeichen:
                                        <asp:Label ID="lblVerkaufteKennzeichen" runat="server" Width="65px"></asp:Label>
                                    </td>
                                    <td>
                                        Rückläufer:
                                        <asp:Label ID="lblRuecklaeufer" runat="server" Width="65px"></asp:Label>
                                    </td>
                                    </tr><tr>
                                        <td>
                                            Anzahl der Vermittler:
                                            <asp:Label ID="lblVermittlerAnzahl" runat="server" Width="57px"></asp:Label>
                                        </td>
                                        <td>
                                            Unverkaufte Kennzeichen:
                                            <asp:Label ID="lblUnverkaufteKennzeichen" runat="server" Width="65px"></asp:Label>
                                        </td>
                                        <td>
                                            Bestand DAD:
                                            <asp:Label ID="lblBestandDAD" runat="server" Width="116px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Anzahl der Kennzeichen:
                                            <asp:Label ID="lblKennzeichenGesamtbestand" runat="server" Width="65px"></asp:Label>
                                        </td>
                                        <td>
                                            Verlust Kennzeichen:
                                            <asp:Label ID="lblVerlustKennzeichen" runat="server" Width="149px"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                               
                            </table>
                        </div>    
                        </div>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        
                        <div class="ExcelDiv">
                            <div  align="right">
                                <img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                               <span class="ExcelSpan" > <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False" ForeColor="White">Excel herunterladen</asp:LinkButton>
                               </span>                               
                            </div>
                        </div>
                            <asp:UpdatePanel ID="UpdatePanelNavi" runat="server">
                                <ContentTemplate>
                        <div id="pagination">
                            <uc2:gridnavigation id="GridNavigation1" runat="server"></uc2:gridnavigation>
                        </div>
                           <div id="data">
                            <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" 
                                            GridLines="None"  PageSize="20" CssClass="gridview">
                                            <PagerSettings  Visible="false" />
                                            <Columns>
                                                <asp:BoundField DataField="KUN_EXT_VM" SortExpression="KUN_EXT_VM" HeaderText="VD-Bezirk"
                                                  >
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NAME1_VM" SortExpression="NAME1_VM" HeaderText="Name 1"
                                                    ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NAME2_VM" SortExpression="NAME2_VM" HeaderText="Name 2"
                                                    ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ANZ_GES" SortExpression="ANZ_GES" HeaderText="Kennzeichen Gesamt"
                                                    ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ANZ_VERK" SortExpression="ANZ_VERK" HeaderText="Kennzeichen Verkauft"
                                                    ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ANZ_UNVERK" SortExpression="ANZ_UNVERK" HeaderText="Kennzeichen Unverkauft"
                                                    ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ANZ_VERL" SortExpression="ANZ_VERL" HeaderText="Kennzeichen Verlust"
                                                    ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ANZ_RUECK" HeaderText="Kennzeichen R&#252;ckl&#228;ufer"
                                                    SortExpression="ANZ_RUECK" ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                    <HeaderStyle CssClass="BoundField"></HeaderStyle>
                                                    <ItemStyle CssClass="BoundField"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Adresse anzeigen" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle"
                                                    ItemStyle-CssClass="BoundField" HeaderStyle-CssClass="BoundField">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lbtnAdressenAnzeigen" CommandName="AdressenAnzeigen" runat="server"
                                                            ForeColor="#333333" Text="Adressen anzeigen" CssClass="TablebuttonLarge" EnableTheming="True"
                                                            Height="16px" Width="125px"></asp:LinkButton></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAdresseAnzeigen" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                            
                                            <HeaderStyle CssClass="Tablehead" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">&nbsp;</div>
                    </div>

                </div>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></div></div></div></asp:Content>