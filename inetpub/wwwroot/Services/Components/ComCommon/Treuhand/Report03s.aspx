<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report03s.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Report03s"
     MasterPageFile="../../../MasterPage/Services.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel" />
                       </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                               <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr  class="formquery" >
                                        <td class="firstLeft active" colspan="2"  style="width:100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                ></asp:Label>
                                                
                                            <asp:Label ID="lblNoData" runat="server" ></asp:Label>                                                
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowracmdCreatep="nowrap">
                                            <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label>
                                        </td>
                                        <td class="active" style="width:100%">
                                              <span> <asp:RadioButton ID="rbFreigabe" GroupName="Vorgang" runat="server" 
                                                    Text="Freigegebene Vorgänge" />
                                              </span> 
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap">
                                            &nbsp;</td>
                                        <td class="active">
                                            <span>
                                                <asp:RadioButton ID="rbAbgelehnt" runat="server" GroupName="Vorgang" Text="Abgelehnte Vorgänge" />
                                            </span>
                                        </td>
                                    </tr>
                                             <tr class="formquery">
                                                 <td class="firstLeft active" nowrap="nowrap">
                                                     &nbsp;</td>
                                                 <td class="active">
                                              <span>
                                                <asp:RadioButton ID="rbGesperrt" runat="server" GroupName="Vorgang" Text="Gesperrte Vorgänge" />
                                            </span>;</td>
                                    </tr>
                                             <tr>
                                                <td class="firstLeft active" nowrap="nowrap">
                                                    <asp:Label ID="lblFreigabevon" runat="server"  Text="Datum von:"></asp:Label></td>
                                                <td style="width:90%">
                                                    <asp:TextBox ID="txtFreigabevon" runat="server"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CE_Freigabevon" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtFreigabevon">
                                                </cc1:CalendarExtender> 
                                                <cc1:MaskedEditExtender ID="MEE_Freigabevon" runat="server" TargetControlID="txtFreigabevon"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </cc1:MaskedEditExtender>                                                                                                   
                                                  </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" nowrap="nowrap">
                                                    <asp:Label ID="lblFreigabebis" runat="server"  Text="Datum bis:"></asp:Label></td>
                                                <td style="width:90%">
                                                    <asp:TextBox ID="txtFreigabebis" runat="server"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CE_FreigabeBis" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtFreigabebis">
                                                </cc1:CalendarExtender> 
                                                <cc1:MaskedEditExtender ID="MEE_FreigabeBis" runat="server" TargetControlID="txtFreigabebis"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </cc1:MaskedEditExtender>                                                                                                   
                                                  </td>
                                            </tr> 
                                     <tr class="formquery">
                                         <td class="firstLeft active" nowrap="nowrap" colspan="2">
                                            &nbsp </td>

                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" colspan="2"  style="width:100%">
                                             &nbsp;</td>
                                     </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
   
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellPadding="0" AlternatingRowStyle-BackColor="#DEE1E0" AllowPaging="True" GridLines="None"
                                                    PageSize="20" CssClass="GridView">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                    <EditRowStyle Wrap="False"></EditRowStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="BELNR" SortExpression="BELNR" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBELNR" runat="server" Text='<%# Bind("BELNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kunnr" HeaderText="col_Kunnr" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kunnr") %>'
                                                            ID="lblKunnr" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField SortExpression="Treugeber" HeaderText="col_Treugeber">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Treugeber" runat="server" CommandArgument="Treugeber"
                                                            CommandName="sort">col_Treugeber</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Treugeber") %>'
                                                            ID="lblTreugeber" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                            CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                            ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                            CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                            ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField SortExpression="Versandadresse" HeaderText="col_Versandadresse">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandadresse" runat="server" CommandArgument="Versandadresse" CommandName="sort">col_Versandadresse</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'
                                                            ID="lblVersandadresse" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Ersteller" HeaderText="col_Ersteller">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ersteller" runat="server" CommandArgument="Ersteller" CommandName="sort">col_Ersteller</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ersteller") %>'
                                                            ID="lblErsteller" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Status" HeaderText="col_Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandArgument="Status" CommandName="sort">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'
                                                            ID="lblStatus" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                    
                                                </asp:TemplateField>                                                
                                                <asp:TemplateField SortExpression="Ablehnungsgrund" HeaderText="col_Ablehnungsgrund">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ablehnungsgrund" runat="server" CommandArgument="Ablehnungsgrund" CommandName="sort">col_Ablehnungsgrund</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle  HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ablehnungsgrund") %>'
                                                            ID="lblAblehnungsgrund" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Ablehnender" HeaderText="col_Ablehnender">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ablehnender" runat="server" CommandArgument="Ablehnender" CommandName="sort">col_Ablehnender</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ablehnender") %>'
                                                            ID="lblAblehnender" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                    
                                                </asp:TemplateField>
                                                                                                
                                                <asp:TemplateField HeaderText="Auswahl">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle  HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSperre" runat="server"  
                                                        />                                                                                                                    
                                                    </ItemTemplate>

                                                <HeaderStyle Width="50px"></HeaderStyle>
                                                </asp:TemplateField>                                                
                                            </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                             <div id="dataQueryFooter" >
                                <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                                <asp:LinkButton ID="cmdLoeschen" runat="server" CssClass="Tablebutton" Width="78px">» Löschen</asp:LinkButton>
                            </div>                                
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
