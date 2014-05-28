<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report03New.aspx.vb" Inherits="AppInsurance.Report03New"
    MasterPageFile="../MasterPage/App.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück" CausesValidation="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label></h1>
                    </div>
                    <asp:UpdatePanel ID="UPResult" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="paginationQuery" style="width: 100%; display: block;">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="active">
                                                Abfrage starten
                                            </td>
                                            <td class="active" valign="top" align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="divQuery" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tr class="formquery">
                                            <td nowrap="nowrap" colspan="2" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td nowrap="nowrap" class="firstLeft active">
                                                Verkehrsjahr<span lang="de">:</span>&nbsp;
                                            </td>
                                            <td nowrap="nowrap" class="active" width="100%">
                                                <asp:TextBox ID="txtVJahr" runat="server" Width="120px"  CssClass="InputTextbox"></asp:TextBox>
                                                 <uc1:MaskedEditExtender ID="txtVJahrMaskedEditExtender" runat="server" AutoComplete="False"
                                                    ClearMaskOnLostFocus="true" Enabled="True" Mask="9999" TargetControlID="txtVJahr"
                                                    MessageValidatorTip="False">
                                                </uc1:MaskedEditExtender>
                                                <asp:RequiredFieldValidator ID="reqFieldValVersJahr" runat="server" ControlToValidate="txtVJahr"
                                                    ErrorMessage="Eingabe des Verkehrsjahres erforderlich."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td nowrap="nowrap" class="firstLeft active">
                                                Agenturnr.:
                                            </td>
                                            <td nowrap="nowrap" class="active">
                                                <asp:TextBox ID="txtOrgNr" runat="server" Width="129px" CssClass="InputTextbox"></asp:TextBox>
                                                <uc1:MaskedEditExtender ID="txtOrgNr_MaskedEditExtender" runat="server"
                                                    AutoComplete="False" ClearMaskOnLostFocus="False" 
                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                    CultureDateFormat="" CultureDatePlaceholder="" 
                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" Mask="CCCC-CCCC-C" 
                                                    TargetControlID="txtOrgNr" MessageValidatorTip="False"
                                                    Filtered="1234567890*">
                                                </uc1:MaskedEditExtender>                                                
                                                <asp:Label ID="lblPlatzhaltersuche" runat="server" Visible="True">*(mit Platzhaltersuche)</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td nowrap="nowrap" class="firstLeft active">
                                                Kennzeichen:
                                            </td>
                                            <td nowrap="nowrap" class="active">
                                                <asp:TextBox ID="txtKennzeichen" runat="server" Width="129px" CssClass="InputTextbox"></asp:TextBox>
                                                <asp:Label ID="lblPlatzhaltersuche2" runat="server" Visible="True">*(mit Platzhaltersuche)</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="2">
                                                &nbsp;</td>
                                        </tr>
                                        <tr class="formquery">
                                            <td  style="background-color: #dfdfdf;" colspan="2">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                             <div id="dataQueryFooter" >
                            <asp:LinkButton ID="btnConfirm" runat="server" CssClass="Tablebutton" Height="16px"
                                                                                Width="78px">» Weiter</asp:LinkButton>
                             </div> 
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnConfirm" />
                            <asp:PostBackTrigger ControlID="lnkCreateExcel" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="Result" runat="Server" visible="false">
                                <div id="DivExcel" class="ExcelDiv" >
                                    <div align="right" class="rightPadding">
                                        
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" /><span class="ExcelSpan">
                                           <asp:LinkButton ID="lnkCreateExcel" runat="server" ForeColor="White">Excel herunterladen</asp:LinkButton></span>
                                    </div>
                                </div>  
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EmptyDataRowStyle BackColor="#DFDFDF" />
                                                    <Columns>
                                                        <asp:BoundField DataField="VD-Bezirk" SortExpression="VD-Bezirk" HeaderText="Agenturnr.">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Name1"  ItemStyle-Width="175px" HeaderStyle-Width="175px"  SortExpression="Name1" HeaderText="Name 1" >
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Name2" SortExpression="Name2" HeaderText="Name 2">
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Kennz. anzeigen" ItemStyle-Width="135px" HeaderStyle-Width="135px"  >
                                                            <HeaderTemplate>
                                                             <span style="float:left">
                                                                <asp:CheckBox ID="chkKennzAnzeigenAll" runat="server" ToolTip="Alle markieren" 
                                                                    oncheckedchanged="chkKennzAnzeigenAll_CheckedChanged" 
                                                                    AutoPostBack="True" ></asp:CheckBox></span>
                                                                <span style="float:right; padding-top:3px;">
                                                                <asp:LinkButton ID="btnKennzAnzeigen" CommandName="KennzAnzeigen" runat="server"
                                                                    Text="» Kennz. anzeigen" EnableTheming="True"
                                                                    Width="110px"></asp:LinkButton></span>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkKennzAnzeigen" runat="server"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Adresse anzeigen" ItemStyle-Width="125px" HeaderStyle-Width="125px" >
                                                            <HeaderTemplate>
                                                            <span style="float:left">
                                                                <asp:CheckBox ID="chkAlleAdrAnzeigen" oncheckedchanged="chkAlleAdrAnzeigen_CheckedChanged" runat="server" ToolTip="Alle markieren" AutoPostBack="True"></asp:CheckBox>
                                                            </span>
                                                            <span style="float:right; padding-top:3px;">
                                                                <asp:LinkButton ID="btnAdressenAnzeigen" CommandName="AdressenAnzeigen" runat="server"
                                                                    Text="» Adr. anzeigen" EnableTheming="True" Width="100px"
                                                                    ></asp:LinkButton></span>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkAdresseAnzeigen" runat="server"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
    
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div><table>

                                    </table>                                    
                                </div>
                                <div id="dataFooter">
                                    &nbsp;</div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
         </div>
    </div>
</asp:Content>
