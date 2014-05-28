<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report05.aspx.vb" Inherits="AppInsurance.Report05"     MasterPageFile="../MasterPage/App.Master" %>
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
                                            <td class="firstLeft active" nowrap="nowrap">
                                                Datum von:
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox CssClass="InputTextbox" runat="server" Width="150px" ToolTip="Datum von"
                                                    ID="txtDateVon" MaxLength="10" autocomplete="off" />
                                                <uc1:CalendarExtender ID="defaultCalendarExtenderVon" runat="server" TargetControlID="txtDateVon" />
                                                <asp:CompareValidator ID="cv_txtDatumVon" runat="server" ErrorMessage="Falsches Datumsformat"
                                                    Type="Date" ControlToValidate="txtDateVon" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                                    CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Datum bis:
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox CssClass="InputTextbox" runat="server" Width="150px" ToolTip="Datum von"
                                                    ID="txtDateBis" MaxLength="10" autocomplete="off" />
                                                <uc1:CalendarExtender ID="ce_DateBis" runat="server" TargetControlID="txtDateBis" />
                                                <asp:CompareValidator ID="cv_txtDatumBis" runat="server" ErrorMessage="Falsches Datumsformat"
                                                    Type="Date" ControlToValidate="txtDateBis" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                                    CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                                <br />
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
                                                
                                            </td>
                                        </tr>

                                        <tr class="formquery">
                                            <td colspan="2">
                                                <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox></td>
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
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSendnr" runat="server" Text='<%# Bind("ZZTRACK") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                    
                                                        <asp:TemplateField>
                                                            <HeaderStyle Width="15px" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lbSendung" runat="server"
                                                                  ImageUrl="../../../Images/DHL-logo.gif" ToolTip="Sendung verfolgen" Height="16px" Width="16px"
                                                                    />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AGENTUR" HeaderStyle-Width="85px" HeaderText="Agenturnr." SortExpression="AGENTUR" />
                                                        <asp:BoundField DataField="NAME1" HeaderStyle-Width="85px" HeaderText="Name/Firma" SortExpression="NAME1" />
                                                        <asp:BoundField DataField="STREET" HeaderStyle-Width="85px" HeaderText="Straße" SortExpression="STREET" />
                                                        <asp:BoundField DataField="HOUSE_NUM1" HeaderStyle-Width="45px" HeaderText="Hausnr." SortExpression="HOUSE_NUM1" />
                                                        <asp:BoundField DataField="POST_CODE1" HeaderStyle-Width="45px" HeaderText="PLZ" SortExpression="POST_CODE1" />
                                                        <asp:BoundField DataField="CITY1" HeaderStyle-Width="85px" HeaderText="Ort" SortExpression="CITY1" />
                                                        <asp:BoundField DataField="ERDAT" HeaderStyle-Width="75px" HeaderText="beauftragt am" DataFormatString="{0:dd.MM.yyyy}" SortExpression="ERDAT" />
                                                        <asp:BoundField DataField="PACKDAT" HeaderStyle-Width="75px" HeaderText="versendet am" DataFormatString="{0:dd.MM.yyyy}" SortExpression="PACKDAT" />
                                                        <asp:BoundField DataField="PACKZEIT" HeaderStyle-Width="75px" HeaderText="versendet um"  SortExpression="PACKZEIT" />

         
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
                                <script type="text/javascript" language="javascript">
                                        function openinfo(url) {
                                        fenster = window.open(url, "Sendungverfolgung", "menubar=0,scrollbars=2,toolbars=0,location=0,directories=0,status=0,width=800,height=600");
                                        fenster.focus();
                                    }
                                </script>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
         </div>
    </div>
</asp:Content>
