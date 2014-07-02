<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01.aspx.vb" Inherits="AppChrysler.Report01"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
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
                            <asp:Panel ID="Panel1" runat="server">
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
                                                        Type="Date" ControlToValidate="txtDatumVon" ControlToCompare="txtDatumBis" Operator="LessThanEqual"
                                                        Font-Bold="True" CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Kennzeichen:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" Width="150px"
                                                        MaxLength="12"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" Width="150px"
                                                        MaxLength="17"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Dokumenteneingangsdatum von:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumVon" runat="server" CssClass="InputTextbox" Width="150px"
                                                        MaxLength="10"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDatumVon_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtDatumVon">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtDatumVon" runat="server" ControlToCompare="TextBox1"
                                                        ControlToValidate="txtDatumVon" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                        ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Dokumenteneingangsdatum bis:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumBis" runat="server" CssClass="InputTextbox" Width="150px"
                                                        MaxLength="10"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDatumBis_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtDatumBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtDatumBis" runat="server" ControlToCompare="TextBox1"
                                                        ControlToValidate="txtDatumBis" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                        ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Dokumentenausgangsdatum von:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumVonAus" runat="server" CssClass="InputTextbox" Width="150px"
                                                        MaxLength="10"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDatumVonAus_CalendarExtender" runat="server"
                                                        Enabled="True" TargetControlID="txtDatumVonAus">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtDatumVonAus" runat="server" ControlToCompare="TextBox1"
                                                        ControlToValidate="txtDatumVonAus" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                        ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Dokumentenausgangsdatum bis:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumBisAus" runat="server" CssClass="InputTextbox" Width="150px"
                                                        MaxLength="10"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDatumBisAus_CalendarExtender" runat="server"
                                                        Enabled="True" TargetControlID="txtDatumBisAus">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtDatumBisAus" runat="server" ControlToCompare="TextBox1"
                                                        ControlToValidate="txtDatumBisAus" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                        ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Schlüsselerfassungsdatum von</td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtSchluesseleingangVon" runat="server" 
                                                        CssClass="InputTextbox" MaxLength="10" Width="150px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtSchluesseleingangVon_CalendarExtender" 
                                                        runat="server" Enabled="True" TargetControlID="txtSchluesseleingangVon">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtSchluesseleingangVon0" runat="server" 
                                                        ControlToCompare="TextBox1" ControlToValidate="txtSchluesseleingangVon" 
                                                        CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                        Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Schlüsselerfassungsdatum bis</td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtSchluesseleingangBis" runat="server" 
                                                        CssClass="InputTextbox" MaxLength="10" Width="150px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtSchluesseleingangBis_CalendarExtender" 
                                                        runat="server" Enabled="True" TargetControlID="txtSchluesseleingangBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtSchluesseleingangBis" runat="server" 
                                                        ControlToCompare="TextBox1" ControlToValidate="txtSchluesseleingangBis" 
                                                        CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                        Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Schlüsselversanddatum von</td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtSchluesselversandVon" runat="server" 
                                                        CssClass="InputTextbox" MaxLength="10" Width="150px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtSchluesselversandVon_CalendarExtender" 
                                                        runat="server" Enabled="True" TargetControlID="txtSchluesselversandVon">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtSchluesselversandVon" runat="server" 
                                                        ControlToCompare="TextBox1" ControlToValidate="txtSchluesselversandVon" 
                                                        CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                        Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Schlüsselversanddatum bis</td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtSchluesselversandBis" runat="server" 
                                                        CssClass="InputTextbox" MaxLength="10" Width="150px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtSchluesselversandBis_CalendarExtender" 
                                                        runat="server" Enabled="True" TargetControlID="txtSchluesselversandBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtSchluesselversandBis" runat="server" 
                                                        ControlToCompare="TextBox1" ControlToValidate="txtSchluesselversandBis" 
                                                        CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                        Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;</td>
                                                <td class="firstLeft active">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" />
                                                    <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>
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
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvBestand" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="True" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                                    CommandName="sort">Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                                    ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                                    CommandName="sort">Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                                    ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Eingangsdatum" HeaderText="Dokumenteneingansdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandArgument="Eingangsdatum"
                                                                    CommandName="sort">Dokumenten- <br /> eingangsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Eingangsdatum","{0:d}") %>'
                                                                    ID="lblEingangsdatum"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Ausgangsdatum" HeaderText="Dokumentenausgansdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Ausgangsdatum" runat="server" CommandArgument="Ausgangsdatum"
                                                                    CommandName="sort">Dokumenten-<br />ausgangsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ausgangsdatum","{0:d}") %>'
                                                                    ID="lblAusgangsdatum"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Schluesselerfassung" HeaderText="Schlüsselerfassungsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="Schluesselerfassung" runat="server" CommandArgument="Schluesselerfassung"
                                                                    CommandName="sort">Schlüsselerfassungs-<br />datum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Schluesselerfassung","{0:d}") %>'
                                                                    ID="lblSchluesselerfassung"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Schluesselversand" HeaderText="Schlüsselversanddatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="Schluesselversand" runat="server" CommandArgument="Schluesselversand"
                                                                    CommandName="sort">Schlüsselversand-<br />datum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Schluesselversand","{0:d}") %>'
                                                                    ID="lblSchluesselerfassung"></asp:Label>
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
