<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report07.aspx.vb" Inherits="AppServicesCarRent.Report07"
    MasterPageFile="../MasterPage/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/services/PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .LeftBold
        {
            font-weight: bold;
            color: #333333;
            text-align: left;
            padding-right: 10px;
        }
        .SelColor
        {
            color: Black;
        }
    </style>
    <div id="site">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="content">
                    <div id="divBackDisabled" class="divPopupBack" runat="server" visible="false" style="height: 100%">
                    </div>
                    <div id="divMessage" class="divPopupDetail" runat="server" visible="false" style="width: 850px;
                        margin-top: 150px; margin-left: 45px; height: auto; text-align: left">
                        <div style="padding-left: 20px; background-color: #dfdfdf; height: 22px; font-weight: bold;
                            color: #333333; text-align: center">
                            <div style="padding-top: 4px;">
                                Bearbeitung</div>
                        </div>
                        <div style="padding: 20px 10px 20px 20px">
                            <table style="width: 95%">
                                <tr>
                                    <td class="LeftBold" style="width: 150px">
                                        <asp:Label ID="lblFin" runat="server" Text="Fahrgestellnummer:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblFinShow" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 25px">
                                        &nbsp;
                                    </td>
                                    <td class="LeftBold" style="width: 150px">
                                        <asp:Label ID="lblKennzeichen" runat="server" Text="Kennzeichen:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblKennzeichenShow" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblTeileingang" runat="server" Text="Teileingang:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblTeileingangShow" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 25px">
                                        &nbsp;
                                    </td>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblMaterialbezeichnung" runat="server" Text="Materialbezeichnung:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblMaterialbezeichnungShow" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblMahnstufe" runat="server" Text="Mahnstufe:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblMahnstufeShow" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 25px">
                                        &nbsp;
                                    </td>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblMahnarten" runat="server" Text="Mahnart:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblMahnartShow" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblMahndatum1" runat="server" Text="Mahndatum Mahnstufe 1:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblMahndatum1Show" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 25px">
                                        &nbsp;
                                    </td>
                                    <td class="LeftBold" rowspan="3" valign="top">
                                        <asp:Label ID="lblAdresse" runat="server" Text="Adresse:"></asp:Label>
                                    </td>
                                    <td rowspan="3" valign="top" class="SelColor">
                                        <asp:Label ID="lblAdresseShow" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblMahndatum2" runat="server" Text="Mahndatum Mahnstufe 2:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblMahndatum2Show" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 25px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblMahndatum3" runat="server" Text="Mahndatum Mahnstufe 3:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblMahndatum3Show" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 25px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblMahrnsperreGesAm" runat="server" Text="Mahnsperre gesetzt am:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblMahrnsperreGesAmShow" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 25px">
                                        &nbsp;
                                    </td>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblMahrnsperreGesDurch" runat="server" Text="Mahnsperre gesetzt durch:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblMahrnsperreGesDurchShow" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblMahnsperreEntfAm" runat="server" Text="Mahnsperre entfernt am:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblMahnsperreEntfAmShow" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 25px">
                                        &nbsp;
                                    </td>
                                    <td class="LeftBold">
                                        <asp:Label ID="lblMahnsperreEntfDurch" runat="server" Text="Mahnsperre entfernt durch:"></asp:Label>
                                    </td>
                                    <td class="SelColor">
                                        <asp:Label ID="lblMahnsperreEntfDurchShow" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <hr style="width: 100%; margin-top: 10px" />
                            <table style="margin-top: 10px" dir="ltr">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErr" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="False" Font-Bold="True"
                                            ForeColor="Blue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LeftBold">
                                        Mahnsperre:
                                    </td>
                                    <td style="text-align: left; padding-left: 5px">
                                        <asp:DropDownList ID="ddlMahnsperre" runat="server">
                                            <asp:ListItem Selected="True" Value="0">Auswahl</asp:ListItem>
                                            <asp:ListItem Value="1">Setzen</asp:ListItem>
                                            <asp:ListItem Value="2">Entfernen</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LeftBold">
                                        Mahndatum ab:
                                    </td>
                                    <td style="text-align: left; padding-left: 5px">
                                        <asp:TextBox ID="txtMahn" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtMahn">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtMahn"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LeftBold" valign="top">
                                        Bemerkung:
                                    </td>
                                    <td style="text-align: left; padding-left: 5px">
                                        <asp:TextBox ID="txtBemerkung" runat="server" CssClass="TextBoxNormal" Height="80px"
                                            TextMode="MultiLine" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="text-align: center; padding-bottom: 10px">
                            <asp:Button ID="btnOK" runat="server" CssClass="TablebuttonLarge" Font-Bold="true"
                                Text="Speichern" />&nbsp;
                            <asp:Button ID="btnCancel" runat="server" CssClass="TablebuttonLarge" Font-Bold="true"
                                Text="Abbrechen" />
                        </div>
                    </div>
                    <div id="navigationSubmenu">
                        <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                            Text="zurück" />
                    </div>
                    <div id="innerContent">
                        <div id="innerContentRight" style="width: 100%">
                            <div id="innerContentRightHeading">
                                <h1>
                                    <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                                </h1>
                            </div>
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
                            <asp:Panel ID="Panel1" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblNoData" runat="server" Font-Bold="True" EnableViewState="False"
                                                        ForeColor="Blue"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="width: 200px">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                        Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Kennzeichen:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                                                        Width="200px"></asp:TextBox>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Zulassungsdatum von:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumVon" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy"
                                                        PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Zulassungsdatum bis:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumBis" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy"
                                                        PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp.label visible="false" id="lbl_Teileing" runat="server">Teileingang schon erfolgt:</asp.label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <span>
                                                        <asp:CheckBox ID="chxTeileingang" runat="server" Visible="False" />
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Mahnstufe:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:CheckBoxList ID="cblMahnstufe" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                        <asp:ListItem>1</asp:ListItem>
                                                        <asp:ListItem>2</asp:ListItem>
                                                        <asp:ListItem>3</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Mahnsperre schon gesetzt:
                                                </td>
                                                <td class="firstLeft active">
                                                    <span>
                                                        <asp:CheckBox Style="padding-left: 12" ID="cbxMahnsperre" runat="server" />
                                                    </span>
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
                                <div id="dataQueryFooter">
                                    <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                                        OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                                </div>
                            </asp:Panel>
                            <div id="Result" runat="Server" visible="false">
                                <div style="padding-left: 15px; padding-bottom: 10px">
                                    <asp:Label ID="lblErrorResult" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    <asp:Label ID="lblNoDataResult" runat="server" EnableViewState="False" Font-Bold="True"
                                        ForeColor="Blue"></asp:Label>
                                </div>
                                <div style="padding-bottom: 10px; font-weight: bold; padding-left: 15px">
                                    Für die ausgewählten Datensätze eine Mahnsperre &nbsp;
                                    <asp:LinkButton ID="lbSetMahnsperre" runat="server" CssClass="Tablebutton" Width="78px">» setzen</asp:LinkButton>
                                    /
                                    <asp:LinkButton ID="lbDelMahnsperre" runat="server" CssClass="Tablebutton" Width="78px">» entfernen</asp:LinkButton>
                                </div>
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" runat="server" OnClick="lnkCreateExcel1_Click">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server">
                                    </uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                                    EnableModelValidation="True">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblID" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblEqunr" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMatnr" Text='<%# DataBinder.Eval(Container, "DataItem.MATNR") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Auswahl" runat="server">Auswahl</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkAuswahl" Checked='<%# DataBinder.Eval(Container, "DataItem.Selected")="X" %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Edit" runat="server">Bearbeiten</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnEdit" runat="server" SortExpression="ibtnEdit" CommandName="EditNew"
                                                                    ImageUrl="/services/images/EditTableHS.png" ToolTip="Bearbeiten" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.ID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="25px" />
                                                            <ItemStyle Width="25px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblFin" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Teileingang" HeaderText="col_Teileingang">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Teileingang" runat="server" CommandName="Sort" CommandArgument="Teileingang">col_Teileingang</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblTeileingang" Text='<%# DataBinder.Eval(Container, "DataItem.Teileingang") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="MAKTX" HeaderText="col_Materialbezeichnung">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Materialbezeichnung" runat="server" CommandName="Sort" CommandArgument="MAKTX">col_Materialbezeichnung</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMaterialbezeichnung" Text='<%# DataBinder.Eval(Container, "DataItem.MAKTX") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZMAHNS" HeaderText="col_Mahnstufe">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Mahnstufe" runat="server" CommandName="Sort" CommandArgument="ZZMAHNS">col_Mahnstufe</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMahnstufe" Text='<%# DataBinder.Eval(Container, "DataItem.ZZMAHNS") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZMADAT_1" HeaderText="col_Mahndatum1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Mahndatum1" runat="server" CommandName="Sort" CommandArgument="ZZMADAT_1">col_Mahndatum1</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMahndatum1" Text='<%# DataBinder.Eval(Container, "DataItem.ZZMADAT_1", "{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZMADAT_2" HeaderText="col_Mahndatum2">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Mahndatum2" runat="server" CommandName="Sort" CommandArgument="ZZMADAT_2">col_Mahndatum2</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMahndatum2" Text='<%# DataBinder.Eval(Container, "DataItem.ZZMADAT_2", "{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZMADAT_3" HeaderText="col_Mahndatum3">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Mahndatum3" runat="server" CommandName="Sort" CommandArgument="ZZMADAT_3">col_Mahndatum3</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMahndatum3" Text='<%# DataBinder.Eval(Container, "DataItem.ZZMADAT_3", "{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="MahnartText" HeaderText="col_Mahnart">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Mahnart" runat="server" CommandName="Sort" CommandArgument="MahnartText">col_Mahnart</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMahnart" Text='<%# DataBinder.Eval(Container, "DataItem.MahnartText") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Adresse" HeaderText="col_Adresse">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Adresse" runat="server" CommandName="Sort" CommandArgument="Adresse">col_Adresse</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblAdresse" Text='<%# DataBinder.Eval(Container, "DataItem.Adresse") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="MAHNSP_GES_AM" HeaderText="col_MahnsperreGesAm">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_MahnsperreGesAm" runat="server" CommandName="Sort" CommandArgument="MAHNSP_GES_AM">col_MahnsperreGesAm</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMahnsperreGesAm" Text='<%# DataBinder.Eval(Container, "DataItem.MAHNSP_GES_AM", "{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="MAHNSP_GES_US" HeaderText="col_MahnsperreGesDurch">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_MahnsperreGesDurch" runat="server" CommandName="Sort" CommandArgument="MAHNSP_GES_US">col_MahnsperreGesDurch</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMahnsperreGesDurch" Text='<%# DataBinder.Eval(Container, "DataItem.MAHNSP_GES_US") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="MAHNSP_ENTF_AM" HeaderText="col_MahnsperreEntfAm">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_MahnsperreEntfAm" runat="server" CommandName="Sort" CommandArgument="MAHNSP_ENTF_AM">col_MahnsperreEntfAm</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMahnsperreEntfAm" Text='<%# DataBinder.Eval(Container, "DataItem.MAHNSP_ENTF_AM", "{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="MAHNSP_ENTF_US" HeaderText="col_MahnsperreEntfDurch">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_MahnsperreEntfDurch" runat="server" CommandName="Sort" CommandArgument="MAHNSP_ENTF_US">col_MahnsperreEntfDurch</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMahnsperreEntfDurch" Text='<%# DataBinder.Eval(Container, "DataItem.MAHNSP_ENTF_US") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="MAHNDATUM_AB" HeaderText="col_MahndatumAb">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_MahndatumAb" runat="server" CommandName="Sort" CommandArgument="MAHNDATUM_AB">col_MahndatumAb</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMahndatumAb" Text='<%# DataBinder.Eval(Container, "DataItem.MAHNDATUM_AB", "{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="BEM" HeaderText="col_Bemerkung">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Bemerkung" runat="server" CommandName="Sort" CommandArgument="BEM">col_Bemerkung</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblBemerkung" Text='<%# DataBinder.Eval(Container, "DataItem.BEM") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField SortExpression="BEZUG_DAT" HeaderText="col_AuftragsDat">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_AuftragsDat" runat="server" CommandName="Sort" CommandArgument="BEZUG_DAT">col_AuftragsDat</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblAuftragsDat" Text='<%# DataBinder.Eval(Container, "DataItem.BEZUG_DAT", "{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="CODE_STORT" HeaderText="col_Mietstuetzpunkt">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Mietstuetzpunkt" runat="server" CommandName="Sort" CommandArgument="CODE_STORT">col_Mietstuetzpunkt</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMietstuetzpunkt" Text='<%# DataBinder.Eval(Container, "DataItem.CODE_STORT") %>'>
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
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
