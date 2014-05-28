<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02.aspx.vb" Inherits="CKG.Components.ComCommon.Report02"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td nowrap="nowrap" class="firstLeft active">
                                                </td>
                                                <td class="firstLeft active" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                    <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="'Datum von' kann darf nicht größer als 'Datum bis' sein!"
                                                        Type="Date" ControlToValidate="txtAuftragdatum" ControlToCompare="txtAuftragdatumBis"
                                                        Operator="LessThanEqual" Font-Bold="True" CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trKUNNR" runat="server">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_KundenNr" runat="server">Kunde</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:DropDownList ID="cmb_KundenNr" runat="server" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblAuftrag" runat="server">Auftragsnr.</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtAuftrag" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblReferenz" runat="server">Referenz</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtReferenz" runat="server" CssClass="TextBoxNormal" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKennzeichen" runat="server">Kennzeichen</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" MaxLength="10" CssClass="TextBoxNormal"></asp:TextBox>
                                                  </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblAuftragdatum" runat="server">Auftragsdatum von</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtAuftragdatum" runat="server" CssClass="TextBoxNormal"></asp:TextBox><ajaxToolkit:CalendarExtender
                                                        ID="txtDatumVon_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtAuftragdatum">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <span>
                                                    <asp:CompareValidator ID="cv_txtDatumVon" runat="server" 
                                                        ControlToCompare="TextBox1" ControlToValidate="txtAuftragdatum" 
                                                        CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                        Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                    </span>
                                                </td>
                                            </tr>      
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                   <asp:Label ID="Label3" runat="server">Auftragsdatum bis</asp:Label></td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtAuftragdatumBis" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDatumBis_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtAuftragdatumBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtDatumBis" runat="server" 
                                                        ControlToCompare="TextBox1" ControlToValidate="txtAuftragdatumBis" 
                                                        CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                        Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                    </td>
                                            </tr>           
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label5" runat="server">Überf.- datum von</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtUeberfuehrungdatumVon" runat="server" 
                                                        CssClass="TextBoxNormal"></asp:TextBox><ajaxToolkit:CalendarExtender ID="CalendarExtender1"
                                                            runat="server" Enabled="True" TargetControlID="txtUeberfuehrungdatumVon">
                                                        </ajaxToolkit:CalendarExtender>

                                                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                                        ControlToCompare="TextBox1" ControlToValidate="txtUeberfuehrungdatumVon" 
                                                        CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                        Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>

                                                </td>
                                            </tr>                                                                                                                      
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    
                                                    <asp:Label ID="Label2" runat="server">Überf.- datum bis</asp:Label>
                                                    </td>
                                                <td class="active">

                                                    <asp:TextBox ID="txtUeberfuehrungdatumBis" runat="server" 
                                                        CssClass="TextBoxNormal"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
                                                        Enabled="True" TargetControlID="txtUeberfuehrungdatumBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="CompareValidator2" runat="server" 
                                                        ControlToCompare="TextBox1" ControlToValidate="txtUeberfuehrungdatumBis" 
                                                        CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                        Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>

                                                </td>
                                            </tr>
                                         
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Leasinggesellschaft" runat="server">Leasinggesellschaft</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txt_Leasinggesellschaft" runat="server" 
                                                        CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Leasingkunde" runat="server">Leasingkunde</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txt_Leasingkunde" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="active">
                                                    <span>
                                                        <asp:RadioButtonList ID="rbAuftragart" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="A" Selected="True">alle Auftr&#228;ge</asp:ListItem>
                                                            <asp:ListItem Value="O">offene Auftr&#228;ge</asp:ListItem>
                                                            <asp:ListItem Value="D">durchgef&#252;hrte Auftr&#228;ge</asp:ListItem>
                                                            <asp:ListItem Value="N">Nur Kl&#228;rf&#228;lle*</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox><asp:ImageButton
                                                        ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>
                            </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen </asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton></span></div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvUeberf" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White"  />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false" HeaderText="Lfd.Nr.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLfdnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Counter") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Details">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" 
                                                                     ToolTip="Detailansicht (in neuem Fenster öffnen)" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.URL") %>' ImageUrl="../../../images/Lupe_16x16.gif" ></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField   HeaderStyle-Width="80px"  DataField="Aufnr" SortExpression="Aufnr" HeaderText="Auftragsnr.">
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="130px"  DataField="ERDAT" DataFormatString="{0:dd.MM.yyyy}" SortExpression="ERDAT" HeaderText="Auftragsdatum" HeaderStyle-Wrap="true">
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="50px"  DataField="Fahrtnr" SortExpression="Fahrtnr" HeaderText="Fahrt">
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderStyle-Width="100px" DataField="Zzkenn" SortExpression="Zzkenn" HeaderText="Kennzeichen">
                                                        </asp:BoundField>
                                                        <asp:BoundField  HeaderStyle-Width="50px" DataField="Zzbezei" SortExpression="Zzbezei" HeaderText="Typ"></asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="140px"  DataField="VDATU" SortExpression="VDATU" DataFormatString="{0:dd.MM.yyyy}" HeaderText="Überführungsdat.">
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="80px"  DataField="wadat_ist" DataFormatString="{0:dd.MM.yyyy}" SortExpression="wadat_ist" HeaderText="Abgabe">
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="50px"  DataField="Fahrtvon" SortExpression="Fahrtvon" HeaderText="Von">
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="50px"  DataField="Fahrtnach" SortExpression="Fahrtnach" HeaderText="Nach">
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderStyle-Width="50px"  DataField="Gef_Km" SortExpression="Gef_Km" HeaderText="Km"></asp:BoundField>
                                                        <asp:TemplateField HeaderStyle-Width="80px" SortExpression="KFTEXT" HeaderText="Kl&#228;rfall">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.KFTEXT")<>String.Empty %>'>X</asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
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
