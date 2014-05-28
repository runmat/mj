<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report10.aspx.cs" Inherits="Leasing.forms.Report10"
    MasterPageFile="../Master/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
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
                    <div id="TableQuery">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="cmdSearch">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2" style="width: 100%">
                                        <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red"
                                            EnableViewState="false"></asp:Label>
                                        <asp:Label ID="lblNoData" runat="server" Visible="False" EnableViewState="false"></asp:Label>&nbsp;
                                    </td>
                                </tr>
                                <tr id="trSearchFin" runat="server" visible="false" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <asp:TextBox ID="txtFahrgestellnummer" CssClass="TextBoxNormal" runat="server" MaxLength="17"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trSearchVertragsnummer" runat="server" visible="false" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Vertragsnummer" runat="server">lbl_Vertragsnummer</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtVertragsnummer" CssClass="TextBoxNormal" runat="server" MaxLength="20"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trSearchEmissionsklasse" runat="server" visible="false" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_Emissionsklasse" runat="server">lbl_Emissionsklasse</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtEmissionsklasse" CssClass="TextBoxNormal" runat="server" MaxLength="40"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trSelection" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Selektionsauswahl:
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <span>
                                            <asp:RadioButton ID="rb_Einzelselektion" Text="Einzelselektion" GroupName="Auswahl"
                                                runat="server" AutoPostBack="True" OnCheckedChanged="rb_Einzelselektion_CheckedChanged" />
                                            &nbsp;
                                            <asp:RadioButton ID="rbUpload" Text="Upload" GroupName="Auswahl" runat="server" Checked="True"
                                                AutoPostBack="True" OnCheckedChanged="rbUpload_CheckedChanged" />
                                            &nbsp; </span>
                                    </td>
                                </tr>
                                <tr id="trUploadFin" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        <asp:Label ID="lbl_UploadFin" runat="server">lbl_UploadFin</asp:Label>
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <input id="upFileFin" type="file" size="49" name="upFileFin" runat="server" />&nbsp;
                                        <a href="javascript:openinfo('InfoUpload.htm');">
                                            <img src="/Services/Images/info.gif" border="0" height="16px" width="16px" alt="Struktur Uploaddatei"
                                                title="Struktur Uploaddatei Fahrgestllnummern" /></a> &nbsp; * max. 900
                                        Datensätze
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
                        </asp:Panel>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                Height="16px" CausesValidation="False" Font-Underline="False" OnClick="cmdSearch_Click">» 
                            Suchen</asp:LinkButton>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="Result" runat="Server" visible="false" style="padding-top: 10px">
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
                                        <table cellspacing="0" style="width: 100%" cellpadding="0" bgcolor="white" border="0">
                                            <tr>
                                                <td>
                                                    <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                                        CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                                        OnSorting="GridView1_Sorting1">
                                                        <PagerSettings Visible="False" />
                                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                        <RowStyle CssClass="ItemStyle" />
                                                        <Columns>
                                                            <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblFin" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="LIZNR" HeaderText="col_Vertragsnummer">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Vertragsnummer</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'>

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
                                                            <asp:TemplateField SortExpression="KD_EMIKL" HeaderText="col_KdEmissionsklasse">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_KdEmissionsklasse" runat="server" CommandName="Sort" CommandArgument="KD_EMIKL">col_KdEmissionsklasse</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblKdEmissionsklasse" Text='<%# DataBinder.Eval(Container, "DataItem.KD_EMIKL") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZZNATIONALE_EMIK" HeaderText="col_Emissionsklasse">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_Emissionsklasse" runat="server" CommandName="Sort" CommandArgument="ZZNATIONALE_EMIK">col_Emissionsklasse</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblEmissionsklasse" Text='<%# DataBinder.Eval(Container, "DataItem.ZZNATIONALE_EMIK") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZZSLD" HeaderText="col_CodeEmissionsklasse">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_CodeEmissionsklasse" runat="server" CommandName="Sort" CommandArgument="ZZSLD">col_CodeEmissionsklasse</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblCodeEmissionsklasse" Text='<%# DataBinder.Eval(Container, "DataItem.ZZSLD") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField SortExpression="ANZ_HALTER" HeaderText="col_AnzahlHalter">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_AnzahlHalter" runat="server" CommandName="Sort" CommandArgument="ANZ_HALTER">col_AnzahlHalter</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblAnzahlHalter" Text='<%# DataBinder.Eval(Container, "DataItem.ANZ_HALTER") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH1_NAME1" HeaderText="col_ZH1NameHalter1">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH1NameHalter1" runat="server" CommandName="Sort" CommandArgument="ZH1_NAME1">col_ZH1NameHalter1</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH1NameHalter1" Text='<%# DataBinder.Eval(Container, "DataItem.ZH1_NAME1") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH1_NAME2" HeaderText="col_ZH1NameHalter2">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH1NameHalter2" runat="server" CommandName="Sort" CommandArgument="ZH1_NAME1">col_ZH1NameHalter2</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH1NameHalter2" Text='<%# DataBinder.Eval(Container, "DataItem.ZH1_NAME2") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH1_STRAS" HeaderText="col_ZH1Strasse">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH1Strasse" runat="server" CommandName="Sort" CommandArgument="ZH1_STRAS">col_ZH1Strasse</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH1Strasse" Text='<%# DataBinder.Eval(Container, "DataItem.ZH1_STRAS") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH1_PSTLZ" HeaderText="col_ZH1PLZ">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH1PLZ" runat="server" CommandName="Sort" CommandArgument="ZH1_PSTLZ">col_ZH1PLZ</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH1PLZ" Text='<%# DataBinder.Eval(Container, "DataItem.ZH1_PSTLZ") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH1_ORT01" HeaderText="col_ZH1Ort">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH1Ort" runat="server" CommandName="Sort" CommandArgument="ZH1_ORT01">col_ZH1Ort</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH1Ort" Text='<%# DataBinder.Eval(Container, "DataItem.ZH1_ORT01") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH2_NAME1" HeaderText="col_ZH2NameHalter1">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH2NameHalter1" runat="server" CommandName="Sort" CommandArgument="ZH2_NAME2">col_ZH2NameHalter1</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH2NameHalter1" Text='<%# DataBinder.Eval(Container, "DataItem.ZH2_NAME1") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH2_NAME2" HeaderText="col_ZH2NameHalter2">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH2NameHalter2" runat="server" CommandName="Sort" CommandArgument="ZH2_NAME2">col_ZH2NameHalter2</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH2NameHalter2" Text='<%# DataBinder.Eval(Container, "DataItem.ZH2_NAME2") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH2_STRAS" HeaderText="col_ZH2Strasse">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH2Strasse" runat="server" CommandName="Sort" CommandArgument="ZH2_STRAS">col_ZH2Strasse</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH2Strasse" Text='<%# DataBinder.Eval(Container, "DataItem.ZH2_STRAS") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH2_PSTLZ" HeaderText="col_ZH2PLZ">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH2PLZ" runat="server" CommandName="Sort" CommandArgument="ZH2_PSTLZ">col_ZH2PLZ</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH2PLZ" Text='<%# DataBinder.Eval(Container, "DataItem.ZH2_PSTLZ") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH2_ORT01" HeaderText="col_ZH2Ort">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH2Ort" runat="server" CommandName="Sort" CommandArgument="ZH2_ORT01">col_ZH2Ort</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH2Ort" Text='<%# DataBinder.Eval(Container, "DataItem.ZH2_ORT01") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH3_NAME1" HeaderText="col_ZH3NameHalter1">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH3NameHalter1" runat="server" CommandName="Sort" CommandArgument="ZH3_NAME3">col_ZH3NameHalter1</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH3NameHalter1" Text='<%# DataBinder.Eval(Container, "DataItem.ZH3_NAME1") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH3_NAME2" HeaderText="col_ZH3NameHalter2">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH3NameHalter2" runat="server" CommandName="Sort" CommandArgument="ZH3_NAME2">col_ZH3NameHalter2</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH3NameHalter2" Text='<%# DataBinder.Eval(Container, "DataItem.ZH3_NAME2") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH3_STRAS" HeaderText="col_ZH3Strasse">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH3Strasse" runat="server" CommandName="Sort" CommandArgument="ZH3_STRAS">col_ZH3Strasse</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH3Strasse" Text='<%# DataBinder.Eval(Container, "DataItem.ZH3_STRAS") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH3_PSTLZ" HeaderText="col_ZH3PLZ">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH3PLZ" runat="server" CommandName="Sort" CommandArgument="ZH3_PSTLZ">col_ZH3PLZ</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH3PLZ" Text='<%# DataBinder.Eval(Container, "DataItem.ZH3_PSTLZ") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH3_ORT01" HeaderText="col_ZH3Ort">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH3Ort" runat="server" CommandName="Sort" CommandArgument="ZH3_ORT01">col_ZH3Ort</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH3Ort" Text='<%# DataBinder.Eval(Container, "DataItem.ZH3_ORT01") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH4_NAME1" HeaderText="col_ZH4NameHalter1">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH4NameHalter1" runat="server" CommandName="Sort" CommandArgument="ZH4_NAME4">col_ZH4NameHalter1</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH4NameHalter1" Text='<%# DataBinder.Eval(Container, "DataItem.ZH4_NAME1") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH4_NAME2" HeaderText="col_ZH4NameHalter2">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH4NameHalter2" runat="server" CommandName="Sort" CommandArgument="ZH4_NAME2">col_ZH4NameHalter2</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH4NameHalter2" Text='<%# DataBinder.Eval(Container, "DataItem.ZH4_NAME2") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH4_STRAS" HeaderText="col_ZH4Strasse">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH4Strasse" runat="server" CommandName="Sort" CommandArgument="ZH4_STRAS">col_ZH4Strasse</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH4Strasse" Text='<%# DataBinder.Eval(Container, "DataItem.ZH4_STRAS") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH4_PSTLZ" HeaderText="col_ZH4PLZ">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH4PLZ" runat="server" CommandName="Sort" CommandArgument="ZH4_PSTLZ">col_ZH4PLZ</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH4PLZ" Text='<%# DataBinder.Eval(Container, "DataItem.ZH4_PSTLZ") %>'>

                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ZH4_ORT01" HeaderText="col_ZH4Ort">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="col_ZH4Ort" runat="server" CommandName="Sort" CommandArgument="ZH4_ORT01">col_ZH4Ort</asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblZH4Ort" Text='<%# DataBinder.Eval(Container, "DataItem.ZH4_ORT01") %>'>

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
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            &nbsp;<asp:HiddenField ID="hField" runat="server" Value="0" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function openinfo(url) {
            fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
            fenster.focus();
        }
 
    </script>
</asp:Content>
