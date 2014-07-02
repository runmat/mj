<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02.aspx.vb" Inherits="AppChrysler.Report02"
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
                                                    Zulassungsdatum von:
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
                                                    Zulassungsdatum bis:
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
                                                    Abmeldedatum von:
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
                                                    Abmeldedatum bis:
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
                                                    Autovermieter:</td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtAutovermieter" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="12" Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
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
                                                        <asp:TemplateField SortExpression="Zulassungsdatum" HeaderText="Zulassungsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Zulassungsdatum" runat="server" CommandArgument="Zulassungsdatum"
                                                                    CommandName="sort">Zulassungsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulassungsdatum","{0:d}") %>'
                                                                    ID="lblZulassungsdatum"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Abmeldedatum" HeaderText="Abmeldedatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandArgument="Abmeldedatum"
                                                                    CommandName="sort">Abmeldedatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum","{0:d}") %>'
                                                                    ID="lblAbmeldedatum"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Nichtruecklaeufer" HeaderText="Nichtrückläufer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Nichtruecklaeufer" runat="server" CommandArgument="Nichtruecklaeufer"
                                                                    CommandName="sort">Nichtrückläufer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Nichtruecklaeufer") %>'
                                                                    ID="lblNichtruecklaeufer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Autovermieter" HeaderText="Autovermieter">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Autovermieter" runat="server" CommandArgument="Autovermieter"
                                                                    CommandName="sort">Autovermieter</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Autovermieter") %>'
                                                                    ID="lblAutovermieter" Visible="true"> </asp:Label>
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
