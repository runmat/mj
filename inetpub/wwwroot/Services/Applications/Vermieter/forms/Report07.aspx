<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report07.aspx.cs" Inherits="Vermieter.forms.Report07"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
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
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_DatumVon" runat="server">lbl_DatumVon</asp:Label>
                                        </td>
                                        <td class="active" style="width: 100%">
                                            <asp:TextBox ID="txt_DatumVon" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txt_DatumVon_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txt_DatumVon">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="txt_DatumVon_MaskedEdit" runat="server" TargetControlID="txt_DatumVon"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </cc1:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_DatumBis" runat="server">lbl_DatumBis</asp:Label>
                                        </td>
                                        <td class="active" style="width: 100%">
                                            <asp:TextBox ID="txt_DatumBis" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txt_DatumBis_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txt_DatumBis">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="txt_DatumBis_MaskedEdit" runat="server" TargetControlID="txt_DatumBis"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </cc1:MaskedEditExtender>
                                        </td>
                                    </tr>
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
                                            <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
                                        </td>
                                        <td class="active" style="width: 100%">
                                            <asp:TextBox ID="txt_Kennzeichen" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <%--<tr class="formquery">
                                        <td class="firstLeft active"></td>
                                        <td class="active" style="width: 100%">
                                            <asp:CheckBox ID="cbScheinFehlt" Text="Nur Aufträge mit fehlendem <u>Schein</u>" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active"></td>
                                        <td class="active" style="width: 100%">
                                            <asp:CheckBox ID="cbSchildFehlt" Text="Nur Aufträge mit fehlendem <u>Schild</u>" runat="server" />
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
                                            <asp:TemplateField SortExpression="LIZNR" HeaderText="col_LIZNR">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_LIZNR" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_LIZNR</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ZZKENN" HeaderText="col_ZZKENN">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ZZKENN" runat="server" CommandName="Sort" CommandArgument="ZZKENN">col_ZZKENN</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKENN") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ZFAHRG" HeaderText="col_ZFAHRG">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ZFAHRG" runat="server" CommandName="Sort" CommandArgument="ZFAHRG">col_ZFAHRG</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.ZFAHRG") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ZB2_FEHLT" HeaderText="col_ZB2_FEHLT">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ZB2_FEHLT" runat="server" CommandName="Sort" CommandArgument="ZB2_FEHLT">col_ZB2_FEHLT</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblZB2_FEHLT" runat="server" Text='<%# Bind("ZB2_FEHLT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="KUNNR" HeaderText="col_KUNNR">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_KUNNR" runat="server" CommandName="Sort" CommandArgument="KUNNR">col_KUNNR</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKUNNR" runat="server" Text='<%# Bind("KUNNR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                    
                                            <asp:TemplateField SortExpression="NAMEK" HeaderText="col_NAMEK">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_NAMEK" runat="server" CommandName="Sort" CommandArgument="NAMEK">col_NAMEK</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNAMEK" runat="server" Text='<%# Bind("NAMEK") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField SortExpression="ERDAT" HeaderText="col_ERDAT">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ERDAT" runat="server" CommandName="Sort" CommandArgument="ERDAT">col_ERDAT</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblERDAT" runat="server" Text='<%# Bind("ERDAT", "{0:dd.MM.yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ERZET" HeaderText="col_ERZET">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ERZET" runat="server" CommandName="Sort" CommandArgument="ERZET">col_ERZET</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblERZET" runat="server" Text='<%# Bind("ERZET") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ABMDAT" HeaderText="col_ABMDAT">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ABMDAT" runat="server" CommandName="Sort" CommandArgument="ABMDAT">col_ABMDAT</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblABMDAT" runat="server" Text='<%# Bind("ABMDAT", "{0:dd.MM.yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ABMORT" HeaderText="col_ABMORT">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ABMORT" runat="server" CommandName="Sort" CommandArgument="ABMORT">col_ABMORT</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblABMORT" runat="server" Text='<%# Bind("ABMORT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ABNAME" HeaderText="col_ABNAME">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ABNAME" runat="server" CommandName="Sort" CommandArgument="ABNAME">col_ABNAME</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblABNAME" runat="server" Text='<%# Bind("ABNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="KBANR" HeaderText="col_KBANR">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_KBANR" runat="server" CommandName="Sort" CommandArgument="KBANR">col_KBANR</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKBANR" runat="server" Text='<%# Bind("KBANR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ZTUEV" HeaderText="col_ZTUEV">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ZTUEV" runat="server" CommandName="Sort" CommandArgument="ZTUEV">col_ZTUEV</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblZTUEV" runat="server" Text='<%# Bind("ZTUEV") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ZASU" HeaderText="col_ZASU">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_ZASU" runat="server" CommandName="Sort" CommandArgument="ZASU">col_ZASU</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblZASU" runat="server" Text='<%# Bind("ZASU") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="SCHEIN_FEHLT" HeaderText="col_SCHEIN_FEHLT">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_SCHEIN_FEHLT" runat="server" CommandName="Sort" CommandArgument="SCHEIN_FEHLT">col_SCHEIN_FEHLT</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSCHEIN_FEHLT" runat="server" Text='<%# Bind("SCHEIN_FEHLT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="SCHILD_FEHLT" HeaderText="col_SCHILD_FEHLT">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_SCHILD_FEHLT" runat="server" CommandName="Sort" CommandArgument="SCHILD_FEHLT">col_SCHILD_FEHLT</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSCHILD_FEHLT" runat="server" Text='<%# Bind("SCHILD_FEHLT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="STATUS" HeaderText="col_STATUS">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_STATUS" runat="server" CommandName="Sort" CommandArgument="STATUS">col_STATUS</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSTATUS" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="PICKDAT" HeaderText="col_PICKDAT">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_PICKDAT" runat="server" CommandName="Sort" CommandArgument="PICKDAT">col_PICKDAT</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPICKDAT" runat="server" Text='<%# Bind("PICKDAT", "{0:dd.MM.yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="PICKZET" HeaderText="col_PICKZET">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_PICKZET" runat="server" CommandName="Sort" CommandArgument="PICKZET">col_PICKZET</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPICKZET" runat="server" Text='<%# Bind("PICKZET") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="HANAME1" HeaderText="col_HANAME1">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_HANAME1" runat="server" CommandName="Sort" CommandArgument="HANAME1">col_HANAME1</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHANAME1" runat="server" Text='<%# Bind("HANAME1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                    
                                            <asp:TemplateField SortExpression="HANAME2" HeaderText="col_HANAME2">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_HANAME2" runat="server" CommandName="Sort" CommandArgument="HANAME2">col_HANAME2</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHANAME2" runat="server" Text='<%# Bind("HANAME2") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="HAPSTLZ" HeaderText="col_HAPSTLZ">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_HAPSTLZ" runat="server" CommandName="Sort" CommandArgument="HAPSTLZ">col_HAPSTLZ</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHAPSTLZ" runat="server" Text='<%# Bind("HAPSTLZ") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="HAORT01" HeaderText="col_HAORT01">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_HAORT01" runat="server" CommandName="Sort" CommandArgument="HAORT01">col_HAORT01</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHAORT01" runat="server" Text='<%# Bind("HAORT01") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="HASTRAS" HeaderText="col_HASTRAS">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_HASTRAS" runat="server" CommandName="Sort" CommandArgument="HASTRAS">col_HASTRAS</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHASTRAS" runat="server" Text='<%# Bind("HASTRAS") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="HAUSNR" HeaderText="col_HAUSNR">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="col_HAUSNR" runat="server" CommandName="Sort" CommandArgument="HAUSNR">col_HAUSNR</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHAUSNR" runat="server" Text='<%# Bind("HAUSNR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div id="Div1">
                                </div>
                            </div>
                            <div id="dataFooter" style="display: none">
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="Tablebutton" Width="78px"
                                    Visible="false" OnClick="btnDelete_Click">» Speichern</asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
