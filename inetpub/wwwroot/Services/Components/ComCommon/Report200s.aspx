<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report200s.aspx.vb" Inherits="CKG.Components.ComCommon.Report200s"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
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
                            <asp:Label ID="lblHead" runat="server" Text=""></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="ibtNewSearch" runat="server" ImageUrl="../../Images/queryArrow.gif" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery">
                        <asp:Panel ID="divSelection" runat="server">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                <tbody>
                                <tr class="formquery">
                                        <td nowrap="nowrap" class="firstLeft active" colspan="2">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="'Datum von' kann darf nicht größer als 'Datum bis' sein!"
                                                Type="Date" ControlToValidate="txtDurchfuehrungVon" ControlToCompare="TextBox1"
                                                Operator="LessThanEqual" Font-Bold="True" CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                                <asp:CompareValidator ID="CompareValidator14" runat="server" ErrorMessage="'Datum von' kann darf nicht größer als 'Datum bis' sein!"
                                                Type="Date" ControlToValidate="txtAbmeldedatumVon" ControlToCompare="TextBox1"
                                                Operator="LessThanEqual" Font-Bold="True" CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap" colspan="2" style="padding-bottom:10px">
                                            Bei Selektionen über einen Zeitraum größer als 30 Tage muss mit längeren Laufzeiten
                                            gerechnet werden. (max. 180 Tage)
                                        </td>
                                    </tr>
                                    
                                    <tr class="formquery" id="tr_DurchfuehrungsdatumVon" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_DurchfuehrungsdatumVon" runat="server">lbl_DurchfuehrungsdatumVon</asp:Label>
                                        </td>
                                        <td class="active" style="width: 100%">
                                            <asp:TextBox ID="txtDurchfuehrungVon" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtDatumVon_CalendarExtender" runat="server" Format="dd.MM.yyyy" 
                                                PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDurchfuehrungVon">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="meetxtDurchfuehrungVon" runat="server" TargetControlID="txtDurchfuehrungVon"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <span>
                                                <asp:CompareValidator ID="cv_txtDurchfuehrungVon" runat="server" ControlToCompare="TextBox1"
                                                    ControlToValidate="txtDurchfuehrungVon" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                    ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_DurchfuehrungsdatumBis" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_DurchfuehrungsdatumBis" runat="server">lbl_DurchfuehrungsdatumBis</asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtDurchfuehrungBis" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtDurchfuehrungBis_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDurchfuehrungBis">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="meetxtDurchfuehrungBis" runat="server" TargetControlID="txtDurchfuehrungBis"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <asp:CompareValidator ID="cv_txtDurchfuehrungBis" runat="server" ControlToCompare="TextBox1"
                                                ControlToValidate="txtDurchfuehrungBis" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_AbmeldedatumVon" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_AbmeldedatumVon" runat="server">lbl_AbmeldedatumVon</asp:Label>
                                        </td>
                                        <td class="active" style="width: 100%">
                                            <asp:TextBox ID="txtAbmeldedatumVon" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd.MM.yyyy" 
                                                PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtAbmeldedatumVon">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="meetxtAbmeldedatumVon" runat="server" TargetControlID="txtAbmeldedatumVon"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <span>
                                                <asp:CompareValidator ID="CompareValidator10" runat="server" ControlToCompare="TextBox1"
                                                    ControlToValidate="txtAbmeldedatumVon" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                    ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_AbmeldedatumBis" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_AbmeldedatumBis" runat="server">lbl_AbmeldedatumBis</asp:Label>
                                        </td>
                                        <td class="active" style="width: 100%">
                                            <asp:TextBox ID="txtAbmeldedatumBis" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender12" runat="server" Format="dd.MM.yyyy" 
                                                PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtAbmeldedatumBis">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="meetxtAbmeldedatumBis" runat="server" TargetControlID="txtAbmeldedatumBis"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <span>
                                                <asp:CompareValidator ID="CompareValidator11" runat="server" ControlToCompare="TextBox1"
                                                    ControlToValidate="txtAbmeldedatumBis" CssClass="TextError" ErrorMessage="Falsches Datumsformat"
                                                    ForeColor="" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_Kennzeichen" runat="server">
                                        <td nowrap="nowrap" class="firstLeft active">
                                            <asp:Label ID="lbl_Kennzeichen" runat="server" Width="130px">lbl_Kennzeichen</asp:Label>
                                        </td>
                                        <td class="active" width="100%">
                                            <asp:TextBox ID="txtKennzeichen" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_Fahrgestellnummer" runat="server">
                                        <td nowrap="nowrap" class="firstLeft active">
                                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server" Width="130px">lbl_Fahrgestellnummer</asp:Label>
                                        </td>
                                        <td class="active" width="100%">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_NummerZB2" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_NummerZB2" runat="server">lbl_NummerZB2</asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtNummerZB2" runat="server" MaxLength="20"></asp:TextBox>
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
                        </asp:Panel>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" Width="78px">» Erstellen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div>
                            <asp:Label ID="lblErrorResult" CssClass="TextError" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton></span></div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True">
                                <PagerSettings Visible="False" />
                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle CssClass="ItemStyle" />
                                <Columns>
                                    <asp:TemplateField SortExpression="VDATU" HeaderText="col_Abmeldedatum">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="VDATU">col_Abmeldedatum</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAbmeldedatum" Text='<%# DataBinder.Eval(Container, "DataItem.VDATU","{0:d}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="VDATU" HeaderText="col_AbmeldedatumADAC">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_AbmeldedatumADAC" runat="server" CommandName="Sort" CommandArgument="VDATU">col_AbmeldedatumADAC</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAbmeldedatum2" Text='<%# DataBinder.Eval(Container, "DataItem.VDATU","{0:yyyyMMdd}") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZKENN" HeaderText="col_Kennzeichen">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="ZZKENN">col_Kennzeichen</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblKennzeichen" Text='<%# Eval("ZZKENN") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZFAHRG" HeaderText="col_Fahrgestellnummer">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="ZZFAHRG">col_Fahrgestellnummer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# Eval("ZZFAHRG") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZBRIEF" HeaderText="col_ZBII">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_ZBII" runat="server" CommandName="Sort" CommandArgument="ZZBRIEF">col_ZBII</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblZBII" Text='<%# Eval("ZZBRIEF") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZREF1" HeaderText="col_Vertragsnummer">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZREF1">col_Vertragsnummer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# Eval("ZZREF1") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZHANDELSNAME" HeaderText="col_Handelsname">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Handelsname" runat="server" CommandName="Sort" CommandArgument="ZZHANDELSNAME">col_Handelsname</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblHandelsname" Text='<%# Eval("ZZHANDELSNAME") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZREFERENZ1" HeaderText="col_Referenz1">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="ZZREFERENZ1">col_Referenz1</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblReferenz1" Text='<%# Eval("ZZREFERENZ1") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZREF2" HeaderText="col_Referenz2">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="ZZREF2">col_Referenz2</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblReferenz2" Text='<%# Eval("ZZREF2") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZHERST_TEXT" HeaderText="col_Hersteller">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="ZZHERST_TEXT">col_Hersteller</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblHersteller" Text='<%# Eval("ZZHERST_TEXT") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZZKLARTEXT_TYP" HeaderText="col_Typ">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="ZZKLARTEXT_TYP">col_Typ</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTyp" Text='<%# Eval("ZZKLARTEXT_TYP") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DADPDI_NAME1" HeaderText="col_Carport">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_Carport" runat="server" CommandName="Sort" CommandArgument="DADPDI_NAME1">col_Carport</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCarport" Text='<%# Eval("DADPDI_NAME1") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ZSTATUS" HeaderText="col_StatusZBII">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_StatusZBII" runat="server" CommandName="Sort" CommandArgument="ZSTATUS">col_StatusZBII</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblStatusZBII" Text='<%# Eval("ZSTATUS") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="dataFooter">
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
