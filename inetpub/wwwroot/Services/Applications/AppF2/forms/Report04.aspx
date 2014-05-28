<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report04.aspx.vb" Inherits="AppF2.Report04"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
                    <asp:UpdatePanel ID="UP1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                            <asp:PostBackTrigger ControlID="lnkCreatePDF1" />
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
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                                        Width="17px" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px; z-index: 0;">
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
                                                    Versandart:
                                                </td>
                                                <td class="firstLeft active">
                                                    <span>
                                                        <asp:RadioButton ID="rb_Alle" runat="server" Text="rb_Alle" AutoPostBack="true" GroupName="Versandart"
                                                            Checked="True" /></span> <span>
                                                                <asp:RadioButton ID="rb_Temp" runat="server" Text="rb_Temp" AutoPostBack="true" GroupName="Versandart" /></span>
                                                    <span>
                                                        <asp:RadioButton ID="rb_End" runat="server" Text="rb_End" AutoPostBack="true" GroupName="Versandart" /></span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Versanddatum von:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumVon" runat="server" CssClass="InputTextbox" Width="150px"></asp:TextBox>
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
                                                    Versanddatum bis:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumBis" autocomplete="off" runat="server" CssClass="InputTextbox"
                                                        Width="150px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDatumBis_CalendarExtender" runat="server" TargetControlID="txtDatumBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cv_txtDatumBis" runat="server" ControlToCompare="TextBox1"
                                                        ControlToValidate="txtDatumBis" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                        ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Händlernr.:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txt_Haendlernr" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Empfänger:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txt_Empf" runat="server" CssClass="InputTextbox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Abrufgrund:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:DropDownList ID="ddlAbrufgrund" runat="server" Width="310px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr id="trWiedereingang" visible="false" class="formquery">
                                                <td class="firstLeft active">
                                                    Wiedereingang:</td>
                                                <td class="firstLeft active">
                                                    <span>
                                                    <asp:CheckBox ID="cbxWiedereingang" runat="server" />
                                                    </span>
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
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconPDF.gif" alt="PDF herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreatePDF1" ForeColor="White" runat="server">PDF herunterladen</asp:LinkButton>
                                        </span>
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
                                                <asp:GridView ID="gvVersendungen" Width="1700px" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EmptyDataRowStyle BackColor="#DFDFDF" />
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="LIZNR" HeaderText="col_Vertragsnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandArgument="Vertragsnummer"
                                                                    CommandName="sort">col_Vertragsnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'
                                                                    ID="lblVertragsnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen" ItemStyle-Wrap="false">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="Kennzeichen"
                                                                    CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'
                                                                    ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                                    CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'
                                                                    ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Versandart" HeaderText="col_Versandart">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versandart" runat="server" CommandArgument="Versandart" CommandName="sort">col_Versandart</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandart") %>'
                                                                    ID="lblVersandart" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Versandadresse" HeaderText="col_Versandadresse">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versandadresse" runat="server" CommandArgument="Versandadresse"
                                                                    CommandName="sort">col_Versandadresse</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'
                                                                    ID="lblVersandadresse" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="WEB USER" HeaderText="col_WebUser">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_WebUser" runat="server" CommandArgument="WebUser" CommandName="sort">col_WebUser</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.WEB USER") %>'
                                                                    ID="lblWebUser" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Haendlername" HeaderText="col_Haendlername">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Haendlername" runat="server" CommandArgument="Haendlername"
                                                                    CommandName="sort">col_Haendlername</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Haendlername") %>'
                                                                    ID="lblHaendlername" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Haendlernr" HeaderText="col_Haendlernr">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Haendlernr" runat="server" CommandArgument="Haendlernr" CommandName="sort">col_Haendlernr</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Haendlernr") %>'
                                                                    ID="lblHaendlernr" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Finanzierungsart" HeaderText="col_Finanzierungsart">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Finanzierungsart" runat="server" CommandArgument="Finanzierungsart"
                                                                    CommandName="sort">col_Finanzierungsart</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Finanzierungsart") %>'
                                                                    ID="lblFinanzierungsart" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Versandgrund" HeaderText="col_Versandgrund">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versandgrund" runat="server" CommandArgument="Versandgrund"
                                                                    CommandName="sort">col_Versandgrund</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versandgrund") %>'
                                                                    ID="lblVersandgrund" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Zustellungsart" HeaderText="col_Zustellungsart">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Zustellungsart" runat="server" CommandArgument="Zustellungsart"
                                                                    CommandName="sort">col_Zustellungsart</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zustellungsart") %>'
                                                                    ID="lblZustellungsart" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Versanddatum" HeaderText="col_Versanddatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Versanddatum" runat="server" CommandArgument="Versanddatum"
                                                                    CommandName="sort">col_Versanddatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Versanddatum","{0:d}") %>'
                                                                    ID="lblVersanddatum" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField SortExpression="Wiedereingang" HeaderText="col_Wiedereingang">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Wiedereingang" runat="server" CommandArgument="Wiedereingang"
                                                                    CommandName="sort">col_Wiedereingang</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Wiedereingang","{0:d}") %>'
                                                                    ID="lblWiedereingang" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField SortExpression="DatumNachtraeglichEndgueltig" HeaderText="col_DatumNachtraeglichEndgueltig">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_DatumNachtraeglichEndgueltig" runat="server" CommandArgument="DatumNachtraeglichEndgueltig"
                                                                    CommandName="sort">col_DatumNachtraeglichEndgueltig</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DatumNachtraeglichEndgueltig","{0:d}") %>'
                                                                    ID="lblDatumNachtraeglichEndgueltig" Visible="true"> </asp:Label>
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
