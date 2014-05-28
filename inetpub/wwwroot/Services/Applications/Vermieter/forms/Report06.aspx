<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report06.aspx.cs" Inherits="Vermieter.forms.Report06"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHeadCore" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active" style="width: 25px;">
                                                <asp:ImageButton ID="NewSearch" runat="server" Width="17px" OnClick="NewSearch_Click" />
                                            </td>
                                            <td align="left" class="active" style="vertical-align: middle;">
                                                Abfrageoptionen
                                            </td>
                                            <td class="active" style="width: 25px;" align="right">
                                                <asp:ImageButton ID="NewSearch2" runat="server" Width="17px" OnClick="NewSearch_Click" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="divError" runat="server" enableviewstate="false" style="padding: 10px 0px 10px 15px;
                                margin-top: 10px;">
                                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                <asp:Label ID="lblNoData" runat="server" ForeColor="Blue" EnableViewState="False"></asp:Label>
                            </div>
                            <asp:Panel ID="Panel1" runat="server">
                                <div id="TableQuery">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txt_Fahrgestellnummer" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Vertragsnummer" runat="server">lbl_Vertragsnummer</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txt_Vertragsnummer" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="display: none">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Kennzeichen_alt" runat="server">lbl_Kennzeichen_alt</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txt_Kennzeichen_alt" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery" style="display: none">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Kennzeichen_neu" runat="server">lbl_Kennzeichen_neu</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txt_Kennzeichen_neu" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_DatumVon" runat="server">lbl_DatumVon</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txt_Datum_von" runat="server"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txt_Datum_von1" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txt_Datum_von">
                                                </ajaxToolkit:CalendarExtender>
                                                <ajaxToolkit:MaskedEditExtender ID="txt_Datum_von_MaskedEdit" runat="server" TargetControlID="txt_Datum_von"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_DatumBis" runat="server">lbl_DatumBis</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txt_Datum_bis" runat="server"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txt_Datum_bis1" runat="server" Format="dd.MM.yyyy"
                                                    PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txt_Datum_bis">
                                                </ajaxToolkit:CalendarExtender>
                                                <ajaxToolkit:MaskedEditExtender ID="txt_Datum_bis_MaskedEdit" runat="server" TargetControlID="txt_Datum_bis"
                                                    Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                        </tr>
                                
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="label7" runat="server"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <span>
                                                    <asp:Label runat="server" ID="lblModus" ForeColor="Blue"></asp:Label>
                                                </span>
                                            </td>
                                        </tr>
                                        
                                        <tr runat="server" id="tr_Auftragsgrund">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Auftragsgrund" runat="server">lbl_Auftragsgrund</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:RadioButtonList runat="server" ID="rbl_Auftragsgrund">
                                                    <asp:ListItem Value="EFS" Text="EFS  Ersatz Fahrzeugschein" />
                                                    <asp:ListItem Value="TEI" Text="TEI  Technische Eintragung" />
                                                    <asp:ListItem Value="NAS" Text="NAS  Nachstempelung" />
                                                    <asp:ListItem Value="019" Text="019  Umkennzeichnungen" />
                                                    <asp:ListItem Value="016" Text="016  Sonstige mit Textfeld" Selected="True" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        <%--<tr runat="server" ID="tr_offene" class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="label_offene" runat="server"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <span>
                                                    <asp:RadioButton GroupName="GFilter" ID="rb_offene" runat="server" Text="Nur offene Umkennzeichnungen" />
                                                </span>
                                            </td>
                                        </tr>
                                        <tr runat="server" ID="tr_klaer" class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label ID="label_klaer" runat="server"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <span>
                                                    <asp:RadioButton GroupName="GFilter" ID="rb_klaer" runat="server" Text="Nur Klärfälle selektieren" />
                                                </span>
                                            </td>
                                        </tr>--%>
                                
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="active" style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                &nbsp;
                                            </td>
                                            <td class="active" style="width: 100%">
                                                &nbsp;
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
                                            Height="16px" CausesValidation="False" Font-Underline="False" OnClick="cmdSearch_Click">» Suchen</asp:LinkButton>
                                        <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                            Text="Button" />
                                    </div>
                                </div>
                            </asp:Panel>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="/services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel" runat="server" OnClick="lnkCreateExcel_Click">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server">
                                    </uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                        CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                        Width="970px" OnSorting="GridView1_Sorting" OnRowDataBound="GridView1_RowDataBound">
                                        <PagerSettings Visible="False" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <Columns>
                                            <asp:TemplateField SortExpression="AUGRU" HeaderText="col_AUGRU">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_AUGRU" runat="server" CommandName="Sort" CommandArgument="AUGRU">col_AUGRU</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblAuftragsgrund" Text='<%# DataBinder.Eval(Container, "DataItem.AUGRU") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Chassis_NUM" HeaderText="col_Chassis_NUM">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Chassis_NUM" runat="server" CommandName="Sort" CommandArgument="Chassis_NUM">col_Chassis_NUM</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Chassis_NUM") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Liznr" HeaderText="col_Liznr">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Liznr" runat="server" CommandName="Sort" CommandArgument="Liznr">col_Liznr</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Liznr") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="License_Num_alt" HeaderText="col_License_Num_alt">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_License_Num_alt" runat="server" CommandName="Sort" CommandArgument="License_Num_alt">col_License_Num_alt</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblKennzeichen_alt" Text='<%# DataBinder.Eval(Container, "DataItem.License_Num_alt") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="License_Num_neu" HeaderText="col_License_Num_neu">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_License_Num_neu" runat="server" CommandName="Sort" CommandArgument="License_Num_neu">col_License_Num_neu</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblKennzeichen_neu" Text='<%# DataBinder.Eval(Container, "DataItem.License_Num_neu") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="vdatu" HeaderText="col_vdatu">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_vdatu" runat="server" CommandName="Sort" CommandArgument="vdatu">col_vdatu</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblAuftragsdatum" Text='<%# DataBinder.Eval(Container, "DataItem.vdatu") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="PSTLZ_STO" HeaderText="col_PSTLZ_STO">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_PSTLZ_STO" runat="server" CommandName="Sort" CommandArgument="PSTLZ_STO">col_PSTLZ_STO</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblMietstuetzpunkt2" Text='<%# DataBinder.Eval(Container, "DataItem.Name1_STO") %>'>
                                                    </asp:Label><br />
                                                    <asp:Label runat="server" ID="lblMietstuetzpunkt3" Text='<%# DataBinder.Eval(Container, "DataItem.Name2_STO") %>'>
                                                    </asp:Label><br />
                                                    <asp:Label runat="server" ID="Label6" Text='<%# DataBinder.Eval(Container, "DataItem.ANSPP_STO") %>'>
                                                    </asp:Label><br />
                                                    <asp:Label runat="server" ID="lblMietstuetzpunkt4" Text='<%# DataBinder.Eval(Container, "DataItem.Stras_STO") %>'>
                                                    </asp:Label><br />
                                                    <asp:Label runat="server" ID="lblMietstuetzpunkt6" Text='<%# DataBinder.Eval(Container, "DataItem.PSTLZ_STO") %>'>
                                                    </asp:Label>&nbsp;<asp:Label runat="server" ID="lblMietstuetzpunkt7" Text='<%# DataBinder.Eval(Container, "DataItem.Ort01_STO") %>'>
                                                    </asp:Label><br />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Status" HeaderText="col_Status">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="Status">col_Status</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblStatus" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Klaerfall" HeaderText="col_Klaerfall">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Klaerfall" runat="server" CommandName="Sort" CommandArgument="Klaerfall">col_Klaerfall</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblKlaerfalltext" Text='<%# DataBinder.Eval(Container, "DataItem.Klaerfall") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="PSTLZ_zh" HeaderText="col_PSTLZ_zh">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_PSTLZ_zh" runat="server" CommandName="Sort" CommandArgument="PSTLZ_zh">col_PSTLZ_zh</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblHalteradresse" Text='<%# DataBinder.Eval(Container, "DataItem.Name1_zh") %>'>
                                                    </asp:Label><br />
                                                    <asp:Label runat="server" ID="lblHalteradresse2" Text='<%# DataBinder.Eval(Container, "DataItem.Name2_zh") %>'>
                                                    </asp:Label><br />
                                                    <asp:Label runat="server" ID="lblHalteradresse3" Text='<%# DataBinder.Eval(Container, "DataItem.Stras_zh") %>'>
                                                    </asp:Label>&nbsp;<asp:Label runat="server" ID="lblHalteradresse4" Text='<%# DataBinder.Eval(Container, "DataItem.House_Num1_zh") %>'>
                                                    </asp:Label><br />
                                                    <asp:Label runat="server" ID="lblHalteradresse5" Text='<%# DataBinder.Eval(Container, "DataItem.PSTLZ_zh") %>'>
                                                    </asp:Label>&nbsp;<asp:Label runat="server" ID="lblHalteradresse6" Text='<%# DataBinder.Eval(Container, "DataItem.Ort01_zh") %>'>
                                                    </asp:Label><br />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="PSTLZ_ze" HeaderText="col_PSTLZ_ze">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_PSTLZ_ze" runat="server" CommandName="Sort"
                                                        CommandArgument="PSTLZ_ze">col_PSTLZ_ze</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblScheinSchilderEmpfaenger" Text='<%# DataBinder.Eval(Container, "DataItem.Name1_ze") %>'>
                                                    </asp:Label><br />
                                                    <asp:Label runat="server" ID="Label1" Text='<%# DataBinder.Eval(Container, "DataItem.Name2_ze") %>'>
                                                    </asp:Label><br />
                                                    <asp:Label runat="server" ID="Label2" Text='<%# DataBinder.Eval(Container, "DataItem.Stras_ze") %>'>
                                                    </asp:Label>&nbsp;<asp:Label runat="server" ID="Label3" Text='<%# DataBinder.Eval(Container, "DataItem.House_Num1_ze") %>'>
                                                    </asp:Label><br />
                                                    <asp:Label runat="server" ID="Label4" Text='<%# DataBinder.Eval(Container, "DataItem.PSTLZ_ze") %>'>
                                                    </asp:Label>&nbsp;<asp:Label runat="server" ID="Label5" Text='<%# DataBinder.Eval(Container, "DataItem.Ort01_ze") %>'>
                                                    </asp:Label><br />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Kommentar" HeaderText="col_Kommentar">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_Kommentar" runat="server" CommandName="Sort" CommandArgument="Kommentar">col_Kommentar</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblBemerkung" Text='<%# DataBinder.Eval(Container, "DataItem.Kommentar") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="erledigt" HeaderText="col_erledigt">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_erledigt" runat="server" CommandName="Sort" CommandArgument="erledigt">col_erledigt</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblerledigt" Text='<%# DataBinder.Eval(Container, "DataItem.erledigt") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div id="Div1">
                                </div>
                            </div>
                            <div id="dataFooter">
                                &nbsp;
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
