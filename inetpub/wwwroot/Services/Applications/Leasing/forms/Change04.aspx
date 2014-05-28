<%@ Page Language="C#" AutoEventWireup="true" Culture="auto" UICulture="auto" CodeBehind="Change04.aspx.cs" Inherits="Leasing.forms.Change04"
    MasterPageFile="../Master/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/services/PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .CellStyle
        {
            font-weight: bold;
            color: #4c4c4c;
            padding-left: 15px;
            padding-top: 5px;
        }
                .DetailHeader
        {
            background-color: #dfdfdf;
            font-weight: bold;
            color: #4c4c4c;
            height: 22px;
            width: 100%;
            padding-left: 15px;
        }
    </style>
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                                        ToolTip="Abfrage öffnen" Visible="false" OnClick="NewSearch_Click" />
                                                    <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                                        Visible="false" OnClick="NewSearchUp_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                             <asp:Panel ID="Panel1" runat="server">
                                <div id="TableQuery" runat="server" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%"  border="0" style="border-right-width: 1px;
                                    border-left-width: 1px; border-right-color: #dfdfdf; border-left-color: #dfdfdf;
                                    border-right-style: solid; border-left-style: solid;">
                                        <tbody>
                                            <tr>
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False" EnableViewState="False"></asp:Label>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td class="CellStyle" style="width: 150px">
                                                    Haltername:
                                                </td>
                                                <td class="CellStyle">
                                                    <asp:TextBox ID="txtHaltername" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="CellStyle">
                                                    Art:
                                                </td>
                                                <td class="CellStyle">
                                                    <asp:RadioButtonList ID="rbArt" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                        <asp:ListItem Selected="True" Value="0">alle</asp:ListItem>
                                                        <asp:ListItem Value="1">vollständig</asp:ListItem>
                                                        <asp:ListItem Value="2">unvollständig</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" />&nbsp;
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
                                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                                            OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                                    </div>

                            <div id="Result" runat="Server" visible="false">
                            <div style="height: 30px;line-height: 30px;float: right">&nbsp;</div>
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
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                                    OnSorting="GridView1_Sorting" OnRowCommand="GridView1_RowCommand">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Width="15px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnEdit" runat="server" SortExpression="ibtnEdit" CommandName="Show"
                                                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.KUNNR_ZH") %>' ImageUrl="/services/images/EditTableHS.png"
                                                                    ToolTip="bearbeiten" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="15px"></HeaderStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="NAME1_ZH" HeaderText="col_Halter">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Halter" runat="server" CommandName="Sort" CommandArgument="NAME1_ZH">col_Halter</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl22" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_ZH") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ORT01_ZH" HeaderText="col_Ort">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Ort" runat="server" CommandName="Sort" CommandArgument="ORT01_ZH">col_Ort</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblOrt" Text='<%# DataBinder.Eval(Container, "DataItem.ORT01_ZH") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="KUNNR_ZH" HeaderText="col_Kunnr">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kunnr" runat="server" CommandName="Sort" CommandArgument="KUNNR_ZH">col_Kunnr</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl23" Text='<%# DataBinder.Eval(Container, "DataItem.KUNNR_ZH") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="VOLLST" HeaderText="col_Vollst">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Vollst" runat="server" CommandName="Sort" CommandArgument="VOLLST">col_Vollst</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblVollst" Text='<%# DataBinder.Eval(Container, "DataItem.VOLLST") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="dataFooter">
                                &nbsp;<asp:HiddenField ID="hField" runat="server" Value="0" />
                            </div>

                            <div id="divChange" runat="server" visible="false">

                            
                                   <table width="100%" style="background-color: #dfdfdf">
                                    <tr>
                                     <td style="font-weight: bold; color: #4c4c4c; padding-left: 15px;height: 22px">
                                         Detaildaten&nbsp;
                                    </td>
                                 </tr>
                                   </table>
                                

                                 <table width="100%" style="border-right-width: 1px; border-left-width: 1px; border-right-color: #dfdfdf;
                                    border-left-color: #dfdfdf; border-right-style: solid; border-left-style: solid;">
                                 
                                 <tr>
                                    <td>
                                <table width="100%">
                                    <tr>
                                        <td style="padding-left: 15px;" colspan="6" height="30">
                                            <asp:Label ID="lblErrorDetail" runat="server" CssClass="TextError" 
                                                EnableViewState="False"></asp:Label>
                                            <asp:Label ID="lblSaveInfo" runat="server" EnableViewState="False" 
                                                Font-Bold="True" ForeColor="Blue"></asp:Label>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td class="CellStyle" style="width: 120px">
                                            Kundennr:
                                        </td>
                                        <td style="width: 265px">
                                            <asp:Label ID="lblKunnr" runat="server"></asp:Label>
                                        </td>
                                        <td class="CellStyle" style="width: 150px">
                                            Vollmacht:
                                        </td>
                                        <td style="width: 20px">
                                            <asp:Label ID="lblVollmacht" runat="server"></asp:Label>
                                        </td>
                                        <td class="CellStyle">
                                            Register:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRegister" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            Halter:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHalter" runat="server"></asp:Label>
                                        </td>
                                        <td class="CellStyle">
                                            Gewerbeanmeld.:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGewerbe" runat="server"></asp:Label>
                                        </td>
                                        <td class="CellStyle">
                                            Einzugserm.:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEinzug" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            HOrt:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHOrt" runat="server"></asp:Label>
                                        </td>
                                        <td class="CellStyle">
                                            Vollst.:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblVollst" runat="server"></asp:Label>
                                        </td>
                                        <td class="CellStyle">
                                            Personalausweis:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPerso" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="First" width="100">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lblKUNNR_SAP" runat="server" Visible="False"></asp:Label>
                                        </td>
                                        <td class="CellStyle">
                                            Versich.Bestätigung:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblKarte" runat="server"></asp:Label>
                                        </td>
                                        <td width="150">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="First" width="100">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="150">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="150">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td class="CellStyle" style="width: 400px">
                                            Ausstellungsdatum Vollmacht:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDateVollm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            Beschafftungsdatum&nbsp;Handelsregister/&nbsp;Gewerbeanmeldung:
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_DateGew" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            neue&nbsp;Beschaffung&nbsp;der&nbsp;Vollmacht/Registerauszüge&nbsp;am:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblVollmRegDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            Besonderheiten Kunde:
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblBemerk"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td class="CellStyle" style="width: 200px">
                                            Nummer der Dauer-EVB:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NummerEVB" runat="server" MaxLength="7" Width="100px"></asp:TextBox>
                                            <asp:Label ID="lblEVB" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            Datum gültig ab:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDatumvon" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy"
                                                PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <asp:Label ID="lblvon" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            Datum gültig bis:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDatumbis" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy"
                                                PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <asp:Label ID="lblbis" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CellStyle">
                                            Ist gel&ouml;scht:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="CBgeloescht" runat="server" Checked=false />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="CellStyle">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>

                                </td>
                                </tr>
                                 </table>
                               

                                <div style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                                <div id="Div1" align="right" style="padding-right: 10px; padding-top: 10px;padding-bottom: 15px">
                                    <asp:LinkButton ID="lbSave" runat="server" CssClass="Tablebutton" Width="78px" OnClick="lbSave_Click">» Speichern </asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="lbCancel" runat="server" CssClass="Tablebutton" Width="78px"
                                        OnClick="lbCancel_Click">» Abbrechen </asp:LinkButton>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
