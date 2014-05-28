 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report05.aspx.cs" Inherits="Leasing.forms.Report05" MasterPageFile="../Master/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                                        Width="17px" onclick="NewSearch_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>         
                    <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            
                            <tr id="trDatumVon" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_DatumVon" runat="server">Datum von:</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtDatumvon" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_Datumvon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtDatumvon">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                    <asp:RequiredFieldValidator ControlToValidate="txtDatumVon" ID="rfvDatumVon" runat="server" ErrorMessage="Bitte wählen Sie ein Datum"></asp:RequiredFieldValidator>
                                    
                                </td>
                            </tr>
                            <tr id="trDatumBis" runat="server" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_DatumBis" runat="server">Datum bis</asp:Label>
                                </td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtDatumBis" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                     <asp:RequiredFieldValidator    ControlToValidate="txtDatumBis" ID="rfvDatumBis" runat="server" ErrorMessage="Bitte wählen Sie ein Datum"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Wählen Sie 'Datum bis' größer als 'Datum von'!"
                                        ControlToCompare="txtDatumvon" ControlToValidate="txtDatumBis" Operator="GreaterThan"
                                        Type="Date"></asp:CompareValidator>
                                       
                                        
                                </td>
                            </tr>
                                                                                 
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    Aktion:
                                </td>
                                <td class="active">
                                    <span>
                                    <asp:radiobuttonlist id="rbAktion" runat="server" RepeatDirection="Horizontal" 
                                        RepeatLayout="Flow">
									<asp:ListItem Value="0" Selected="True">alle</asp:ListItem>
									<asp:ListItem Value="2">endgültiger Versand</asp:ListItem>
									<asp:ListItem Value="1">temporärer Versand</asp:ListItem>
									</asp:radiobuttonlist></span>
                                </td>
                                 
                            </tr>

                                                    
                            
                            <tr class="formquery">
                                <td colspan="2" align="right" style="width: 100%">
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                Height="16px" CausesValidation="False" Font-Underline="False" OnClick="cmdSearch_Click">» 
                            Weiter</asp:LinkButton>
                            &nbsp;
                        </div>
                        
                    </div>
                    
                      <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel1" runat="server" OnClick="lnkCreateExcel1_Click">Excel herunterladen</asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                            AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20" 
                                            onsorting="GridView1_Sorting" >
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EmptyDataRowStyle BackColor="#DFDFDF" />
                                <Columns>
                                    <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer"
                                        Visible="false">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFahrgestellnummer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Leasingvertrag" HeaderText="col_Leasingvertragsnr">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Leasingvertragsnr" runat="server" CommandName="Sort" CommandArgument="Leasingvertragsnr">col_Leasingvertragsnr</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeasingvertragsnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingvertrag") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Briefnummer" HeaderText="col_Briefnummer">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Briefnummer" runat="server" CommandName="Sort" CommandArgument="Briefnummer">col_Briefnummer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblBrief" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Briefnummer") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblKennzeichen" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="COCvorhanden" HeaderText="col_COC">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_COC" runat="server" CommandName="Sort" CommandArgument="COCvorhanden">col_COC</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCOC" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.COCvorhanden") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Versandart" HeaderText="col_Versandart">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Versandart" runat="server" CommandName="Sort" CommandArgument="Versandart">col_Versandart</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVersandart" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandart") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Versanddatum" HeaderText="col_Versanddatum">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Versanddatum" runat="server" CommandName="Sort" CommandArgument="Versanddatum">col_Versanddatum</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVersanddatum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versanddatum", "{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Adresse" HeaderText="col_Adresse">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Adresse" runat="server" CommandName="Sort" CommandArgument="Adresse">col_Adresse</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdresse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Adresse") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                        </asp:GridView>
                           </div>
                    
                        <div id="Div1">
                        </div>
                    </div>
                    
                </div>
            </div>
        </div>
    </div>

   
    
</asp:Content>