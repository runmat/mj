<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="jve_LogMessage2.aspx.vb" Inherits="Admin.jve_LogMessage2" MasterPageFile="MasterPage/Admin.Master"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;
            </div>
            <div id="innerContent">
                 <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="'Datum von' kann darf nicht größer als 'Zulassungsdatum bis' sein!"
                                                    Type="Date" ControlToValidate="txtDatumVon" ControlToCompare="txtDatumBis"
                                                    Operator="LessThanEqual" Font-Bold="True" CssClass="TextError" 
                                                    ForeColor=""></asp:CompareValidator>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Reportname"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                &nbsp;
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Firma:
                                            </td>
                                            <td nowrap="nowrap"  colspan="2" valign="top" class="firstLeft active">
                                                <asp:DropDownList ID="ddlKunde" runat="server" AutoPostBack="True" 
                                                    Font-Names="Verdana,sans-serif" Width="260px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
												Datum&nbsp;Anzeige (tt.mm.jjjj)*<br />
												von&nbsp;- bis
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox ID="txtDatumVon" runat="server" CssClass="InputTextbox" 
                                                    Width="150px">*</asp:TextBox>
                                                
                                                <ajaxToolkit:CalendarExtender ID="txtDatumVon_CalendarExtender" runat="server" 
                                                    Enabled="True" TargetControlID="txtDatumVon">
                                                </ajaxToolkit:CalendarExtender>
                                                 <asp:CompareValidator ID="cv_txtDatumVon" runat="server" 
                                                    ErrorMessage="Falsches Datumsformat" Type="Date" ControlToValidate="txtDatumVon"
                                                    ControlToCompare="TextBox1" Operator="DataTypeCheck" CssClass="TextError" 
                                                    ForeColor=""></asp:CompareValidator>   
                                                
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" valign="middle" width="100%">
                                                <asp:TextBox ID="txtDatumBis" runat="server" CssClass="InputTextbox" 
                                                    Width="150px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtDatumBis_CalendarExtender" runat="server" 
                                                    Enabled="True" TargetControlID="txtDatumBis">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                                    ControlToCompare="TextBox1" ControlToValidate="txtDatumBis" 
                                                    CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                    Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">Zeit Anzeige (hh:mm)*<br />
												von&nbsp;- bis</td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <asp:textbox id="txtZeitVon" tabIndex="3" runat="server" CssClass="InputTextbox"  Width="150px"></asp:textbox>                                            
                                                <ajaxToolkit:MaskedEditExtender ID="txtZeitVon_MaskedEditExtender" 
                                                    runat="server" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                    CultureDateFormat="99:99" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                                    TargetControlID="txtZeitVon" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="time">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" valign="middle">
                                             <asp:textbox id="txtZeitBis" tabIndex="3" runat="server" CssClass="InputTextbox"  Width="150px"></asp:textbox> 
                                            <ajaxToolkit:MaskedEditExtender ID="txtZeitBis_MaskedEditExtender" 
                                                    runat="server" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                    CultureDateFormat="99:99" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                                    TargetControlID="txtZeitBis" UserTimeFormat="TwentyFourHour" Mask="99:99" MaskType="time">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Zusatzeingabe:
                                            </td>
                                            <td valign="middle" nowrap="nowrap" class="firstLeft active">
                                                <span><asp:CheckBox runat="server" ID="chkZusatzeingabe" /></span>
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Zusatzbemerkung:
                                            </td>
                                            <td valign="top" nowrap="nowrap" class="firstLeft active">
                                                <asp:TextBox Rows="3" TextMode="MultiLine" MaxLength="150"  CssClass="InputTextbox" runat="server" ID="txtZusatzbemerkung"></asp:TextBox></td>
                                            <td class="firstLeft active" nowrap="nowrap" valign="top">
                                                &nbsp;</td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Abruftyp:</td>
                                            <td class="firstLeft active" nowrap="nowrap" valign="top">
                                               <span> <asp:RadioButtonList runat="server" ID="rblTyp">
                                                        <asp:ListItem Text="temporär" Value="temp"></asp:ListItem>
                                                        <asp:ListItem Text="endgültig" Value="endg"></asp:ListItem>
                                                    </asp:RadioButtonList></span>
                                              </td>
                                            <td class="firstLeft active" nowrap="nowrap" valign="top">
                                                &nbsp;</td>
                                        </tr>
                                         <tr class="formquery">
                                            <td colspan="2">
                                                   <asp:TextBox ID="TextBox1" Visible="false" runat="server" ></asp:TextBox>
                                            </td>

                                             <td>
                                                 &nbsp;</td>

                                        </tr>
                                    </tbody>
                                </table>
                          <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                 &nbsp;
                        </div>  
                            </div>
   
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbEintragen" runat="server" CssClass="Tablebutton" Width="78px">» Eintragen</asp:LinkButton>
                    </div>   
                                         
                    <div id="Result" runat="Server" visible="false">
                                <%--<div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False" ForeColor="White">Excel herunterladen</asp:LinkButton>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>--%>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <div class="formqueryHeader" style="background-color: #576B96">
                                                    <span>Temporär</span>
                                                </div>
                                            </td>
                                        </tr>                                    
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvTemporaer" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                <asp:TemplateField HeaderText="Entfernen" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton CausesValidation="false" runat="server" ToolTip="Dieses Fahrzeug aus Übernahmetabelle entfernen"
                                                            ID="imgDelete" Height="14" CommandName="loesch" Width="14" ImageUrl="../Images/del.png"
                                                            CommandArgument='<%# DataBinder.Eval(Container, "DataItem.SapWert") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="mit Zusatzeingabe" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" Enabled="false" ID="chkZusatzeingabe" Checked='<%# Cbool(DataBinder.Eval(Container, "DataItem.MitZusatzText")) %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Web Bezeichnung" DataField="WebBezeichnung" />
                                                <asp:BoundField HeaderText="SAP Wert" DataField="SapWert" />
                                                <asp:BoundField ItemStyle-Width="400px" HeaderText="Zusatzbemerkung" 
                                                            DataField="Zusatzbemerkung" >
                                                    <ItemStyle Width="400px" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#DEE1E0"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                 &nbsp;
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td>
                                                <div class="formqueryHeader" style="background-color: #576B96">
                                                    <span>Endgültig</span>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvEndgueltig" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20" >
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                <asp:TemplateField HeaderText="Entfernen" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton  CausesValidation="false" runat="server" ToolTip="Dieses Fahrzeug aus Übernahmetabelle entfernen"
                                                            ID="imgDelete" Height="14" CommandName="loesch" Width="14" ImageUrl="../Images/del.png"
                                                           CommandArgument='<%# DataBinder.Eval(Container, "DataItem.SapWert") %>'  />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="mit Zusatzeingabe" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" Enabled="false" ID="chkZusatzeingabe" Checked='<%# Cbool(DataBinder.Eval(Container, "DataItem.MitZusatzText")) %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Web Bezeichnung" DataField="WebBezeichnung" />
                                                <asp:BoundField HeaderText="SAP Wert" DataField="SapWert" />
                                                <asp:BoundField ItemStyle-Width="400px" HeaderText="Zusatzbemerkung" 
                                                            DataField="Zusatzbemerkung" >                                                    
                                                    <ItemStyle Width="400px" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    </asp:GridView></td></tr>                                        
                                    </table>
                                </div>
                    </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                                    <div id="dataFooter">
                    &nbsp;</div>
                </div>

            </div>
        </div>
</asp:Content>
