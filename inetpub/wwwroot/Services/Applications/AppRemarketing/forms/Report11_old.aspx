<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report11.aspx.cs" Inherits="AppRemarketing.forms.Report11"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/services/PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Titlebar
        {
            background-image: url(/Services/Images/overflow.png);
            line-height: 22px;
            color: #ffffff;
            font-weight: bold;
            float: left;
            height: 22px;
            width: 100%;
            background-color: #576b96;
            text-align: center;
            white-space: nowrap;
        }
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <%-- <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" OnClick="lb_zurueck_Click1"></asp:LinkButton>--%>
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
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="lbCreate">
                        <div id="TableQuery" style="margin-bottom: 10px">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="2">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                            <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False" EnableViewState="False"></asp:Label>
                                        </td>
                                        <td style="width: 100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
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
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Inventarnummer:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtInventarnummer" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                                                Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Rechnungsnummer:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtRechnungsnummer" runat="server" CssClass="TextBoxNormal" MaxLength="8"
                                                Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trVermieter" runat="server" visible="false" class="formquery">
                                        <td class="firstLeft active">
                                            Autovermieter:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:DropDownList ID="ddlVermieter" runat="server" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trHC" runat="server" visible="true" class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap" style="height: 22px" width="150px">
                                            Hereinnahmecenter
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:DropDownList ID="ddlHC" runat="server" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Status:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Eingangsdatum von:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtDatumVon" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </cc1:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Eingangsdatum bis:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtDatumBis" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                            </cc1:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Vertragsjahr:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtVertragsjahr" runat="server" CssClass="TextBoxNormal" MaxLength="4"
                                                Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFreibetrag" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                            Freibetrag:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:CheckBox ID="chkFreibetrag" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="2">
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
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <span style="float: left; color: White; font-weight: bold; padding-left: 15px">
                                    <asp:Label ID="lblGutachten" runat="server" Visible="false">Gutachten</asp:Label></span>
                                <img src="/Services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel1" runat="server" OnClick="lnkCreateExcel1_Click">Excel herunterladen</asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data" style="float: none;">
                            <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                <tr id="trPdfError" runat="server" visible="false" class="formquery">
                                    <td>
                                        <asp:Label ID="lblErrorPdf" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
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
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="colAuswahl" runat="server" CommandName="Sort" CommandArgument="Auswahl">Auswahl</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAuswahl" Checked='<%# (DataBinder.Eval(Container, "DataItem.Auswahl")).ToString()!= "0"%>'
                                                            runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblRekla" runat="server" Text=" "></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtnRekla" runat="server" SortExpression="ibtnRekla" CommandName="Rekla"
                                                            ImageUrl="/services/images/del.png" ToolTip="Reklamieren" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FAHRGNR") %>'
                                                            Visible='<%# Convert.ToInt32(DataBinder.Eval(Container, "DataItem.STATU"))== 0 || Convert.ToInt32(DataBinder.Eval(Container, "DataItem.STATU"))== 3 %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15px"></HeaderStyle>
                                                    <ItemStyle Width="15px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgEmpty" runat="server" ImageUrl="/Services/images/blank.gif" Height="16"
                                                            Width="16" Visible='<%# (DataBinder.Eval(Container, "DataItem.DDTEXT")).ToString()!="Widersprochen" && (DataBinder.Eval(Container, "DataItem.DDTEXT")).ToString()!="Blockiert" %>' />
                                                        <asp:ImageButton ID="imgRekla" runat="server" ImageUrl="/services/images/comment.png"
                                                            CommandName="ShowReklamation" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FAHRGNR") %>'
                                                            Visible='<%# (DataBinder.Eval(Container, "DataItem.DDTEXT")).ToString()=="Widersprochen" %>'
                                                            ToolTip="Widerspruch" />
                                                        <asp:ImageButton ID="ibtnblocktext" runat="server" ImageUrl="/services/images/comment.png"
                                                            CommandName="BlockText" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FAHRGNR") %>'
                                                            Visible='<%# (DataBinder.Eval(Container, "DataItem.DDTEXT")).ToString()=="Blockiert" %>'
                                                            ToolTip="Blockadegrund" />
                                                        <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="/services/images/info.gif"
                                                            CommandName="Show" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FAHRGNR") %>'
                                                            ToolTip="Gutachten anzeigen." />
                                                        <asp:ImageButton ID="ibtnpdf" runat="server" ImageUrl="/services/images/pdf-logo.png"
                                                            Height="20" Width="20" CommandName="PDF" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FAHRGNR") %>'
                                                            ToolTip="Belastungsanzeige als PDF" />
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="false" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkTuev" runat="server" Target="_blank" ImageUrl="/services/images/TUEV.png"
                                                            Height="16px" Width="16px" ToolTip="Zum TÜV-Gutachten" Visible='<%# (DataBinder.Eval(Container, "DataItem.GUTA")).ToString()=="TUEV" %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="FAHRGNR" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="FAHRGNR">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkHistorie" Target="_blank" ToolTip="Zur Fahrzeughistorie" runat="server"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.FAHRGNR") %>'>
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="KENNZ" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="KENNZ">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblKennzeichen2" Text='<%# DataBinder.Eval(Container, "DataItem.KENNZ") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="INVENTAR" HeaderText="col_Inventarnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Inventarnummer" runat="server" CommandName="Sort" CommandArgument="INVENTAR">col_Inventarnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblInventarnummer" Text='<%# DataBinder.Eval(Container, "DataItem.INVENTAR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AVNAME" HeaderText="col_Autovermieter">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Autovermieter" runat="server" CommandName="Sort" CommandArgument="AVNAME">col_Autovermieter</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblAutovermieter" Text='<%# DataBinder.Eval(Container, "DataItem.AVNAME") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="HCNAME" HeaderText="col_Hereinnahmecenter">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Hereinnahmecenter" runat="server" CommandName="Sort" CommandArgument="HCNAME">col_Hereinnahmecenter</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblHereinnahmecenter" Text='<%# DataBinder.Eval(Container, "DataItem.HCNAME") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="HCEINGDAT" HeaderText="col_HcEingangsdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_HcEingangsdatum" runat="server" CommandName="Sort" CommandArgument="HCEINGDAT">col_HcEingangsdatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblHcEingangsdatum" Text='<%# DataBinder.Eval(Container, "DataItem.HCEINGDAT", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="GUTAUFTRAGDAT" HeaderText="col_Beauftragungsdatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Beauftragungsdatum" runat="server" CommandName="Sort" CommandArgument="GUTAUFTRAGDAT">col_Beauftragungsdatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBeauftragungsdatum" Text='<%# DataBinder.Eval(Container, "DataItem.GUTAUFTRAGDAT", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="GUTADAT" HeaderText="col_Gutachtendatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Gutachtendatum" runat="server" CommandName="Sort" CommandArgument="GUTADAT">col_Gutachtendatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblGutachtendatum" Text='<%# DataBinder.Eval(Container, "DataItem.GUTADAT", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="SOLLFREI" HeaderText="col_Freigabedatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Freigabedatum" runat="server" CommandName="Sort" CommandArgument="SOLLFREI">col_Freigabedatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFreigabedatum" Text='<%# DataBinder.Eval(Container, "DataItem.SOLLFREI", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="DDTEXT" HeaderText="col_Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="DDTEXT">col_Status</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStatusText" Text='<%# DataBinder.Eval(Container, "DataItem.DDTEXT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="SUMME" HeaderText="col_Summe">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Summe" runat="server" CommandName="Sort" CommandArgument="SUMME">col_Summe</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSumme" Text='<%# DataBinder.Eval(Container, "DataItem.SUMME", "{0:c}") %>'
                                                            Style="text-align: right">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="AZGUT" HeaderText="col_AnzGutachten">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_AnzGutachten" runat="server" CommandName="Sort" CommandArgument="AZGUT">col_AnzGutachten</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblAnzGutachten" Text='<%# DataBinder.Eval(Container, "DataItem.AZGUT") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="RENNR" HeaderText="col_Rechnungsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Rechnungsnummer" runat="server" CommandName="Sort" CommandArgument="RENNR">col_Rechnungsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRechNR" Text='<%# DataBinder.Eval(Container, "DataItem.RENNR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStatus" Text='<%# DataBinder.Eval(Container, "DataItem.STATU") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView2"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True">
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField SortExpression="FAHRGNR" HeaderText="Fahrgestellnummer">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkHistorie" Target="_blank" ToolTip="Zur Fahrzeughistorie" runat="server"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.FAHRGNR") %>'>
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="GUTA" HeaderText="Gutachter" SortExpression="GUTA" HeaderStyle-ForeColor="White">
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="GUTAID" HeaderText="Gutachten-ID" SortExpression="GUTAID"
                                                    HeaderStyle-ForeColor="White">
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="GUTADAT" HeaderText="Gutachtendatum" SortExpression="GUTADAT"
                                                    HeaderStyle-ForeColor="White" DataFormatString="{0:d}">
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SCHADBETR_AV" HeaderText="Unwetterschäden" SortExpression="SCHADBETR_AV"
                                                    HeaderStyle-ForeColor="White" DataFormatString="{0:c}">
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AUFBETR_AV" HeaderText="Unsachgemäß rep. Unfallschäden"
                                                    SortExpression="AUFBETR_AV" HeaderStyle-ForeColor="White" DataFormatString="{0:c}">
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FEHLTBETR_AV" HeaderText="Fehlteile" SortExpression="FEHLTBETR_AV"
                                                    HeaderStyle-ForeColor="White" DataFormatString="{0:c}">
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MAEWERT_AV" HeaderText="Tech. Mängel" SortExpression="MAEWERT_AV"
                                                    HeaderStyle-ForeColor="White" DataFormatString="{0:c}">
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="OPTWRTAV" HeaderText="Opt. Mängel" SortExpression="OPTWRTAV"
                                                    HeaderStyle-ForeColor="White" DataFormatString="{0:c}">
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MINWERT_AV" HeaderText="Merk. Minderwert" SortExpression="MINWERT_AV"
                                                    HeaderStyle-ForeColor="White" DataFormatString="{0:c}">
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="WRTMBETR_AV" HeaderText="Summe" SortExpression="WRTMBETR_AV"
                                                    HeaderStyle-ForeColor="White" DataFormatString="{0:c}">
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div style="float: right; width: 100%; text-align: right; padding-top: 15px">
                        <asp:LinkButton ID="lbtnBack" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="lbtnBack_Click" Visible="false">» Zurück </asp:LinkButton>
                    </div>
                    <div id="dataFooter">
                        <asp:HiddenField ID="hField" runat="server" Value="0" />
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        <asp:LinkButton ID="cmdBlock" runat="server" Visible="False" CssClass="TablebuttonMiddle"
                            Height="16px" Width="100px" OnClick="cmdBlock_Click">» Blockieren</asp:LinkButton>
                        <asp:LinkButton ID="cmdNoBlock" runat="server" CssClass="TablebuttonMiddle" Height="16px"
                            Width="100px" Visible="False" OnClick="cmdNoBlock_Click">» Aufheben</asp:LinkButton>
                        <asp:LinkButton ID="cmdEdit" Visible="False" runat="server" CssClass="TablebuttonMiddle"
                            Height="16px" Width="100px" OnClick="cmdEdit_Click">» In Bearbeitung</asp:LinkButton>
                    </div>
                </div>
                <div>
                    <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                    <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                    <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancel"
                        X="450" Y="200">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="mb" runat="server" Width="500px" Height="290px" BackColor="White"
                        Style="display: none">
                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; text-align: center">
                            <asp:Label ID="lblAdressMessage" runat="server" Text="Bitte geben Sie hier eine Beschreibung ein:"
                                Font-Bold="True"></asp:Label>
                        </div>
                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; padding-bottom: 10px;
                            margin-left: 10px">
                            <asp:TextBox ID="txtBeschreibung" runat="server" Height="70px" TextMode="MultiLine"
                                Width="450px"></asp:TextBox>
                        </div>
                        <table cellpadding="0" cellspacing="0" style="margin-left: 10px;">
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Sachbearbeiter:
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtSachbearbeiter" runat="server" CssClass="TextBoxNormal" MaxLength="25"
                                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    Telefon:
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtTelefon" runat="server" CssClass="TextBoxNormal" MaxLength="15"
                                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    E-Mail:
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtMail" runat="server" CssClass="TextBoxNormal" MaxLength="50"
                                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <div style="text-align: center; padding-bottom: 10px">
                            <asp:Label ID="lblSaveInfo" runat="server" Visible="false" Style="margin-bottom: 15px"></asp:Label>
                        </div>
                        <table width="100%" style="text-align: center">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnOK" runat="server" Text="Übernehmen" CssClass="TablebuttonLarge"
                                        Font-Bold="True" Width="90px" OnClick="btnOK_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Ablehnen" CssClass="TablebuttonLarge"
                                        Font-Bold="true" Width="90px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div>
                    <asp:Button ID="InfoPopupPosition" runat="server" Text="Fake" Style="display: none" />
                    <asp:Button ID="InfoPopupOpener" runat="server" Text="BUTTON" OnClick="InfoPopupOpener_Click"
                        Visible="False" />
                    <cc1:ModalPopupExtender ID="InfoPopup" runat="server" TargetControlID="InfoPopupPosition"
                        PopupControlID="InfoPopupContent" BackgroundCssClass="modalBackground" DropShadow="true"
                        CancelControlID="InfoPopupCancel" X="450" Y="200">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="InfoPopupContent" runat="server" Width="500px" Height="180px" BackColor="#F4F7FC"
                        Style="display: none">
                        <div class="Titlebar">
                            <asp:Literal ID="InfoPopupHeader" runat="server" Text="Reklamationstext" />
                        </div>
                        <div style="padding-left: 10px; margin-bottom: 10px; padding-bottom: 10px; height: 100px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="color: #4C4C4C; font-weight: bold; padding-left: 10px; padding-right: 10px;
                                            padding-top: 10px">
                                            <asp:Label ID="InfoPopupText" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <table width="100%" style="text-align: center">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="InfoPopupCancel" runat="server" Text="OK" CssClass="TablebuttonLarge"
                                        Font-Bold="true" Width="90px" Height="25px" Style="vertical-align: middle" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div>
                    <asp:Button ID="BlockPopupPosition" runat="server" Text="Fake" Style="display: none;" />
                    <asp:Button ID="BlockPopupOpener" runat="server" Text="BUTTON" OnClick="OpenBlockPopup_Click"
                        Visible="False" />
                    <cc1:ModalPopupExtender ID="BlockPopup" runat="server" TargetControlID="BlockPopupPosition"
                        PopupControlID="BlockPopupPanel" BackgroundCssClass="modalBackground" DropShadow="true"
                        CancelControlID="BlockPopupCancel" X="450" Y="200">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="BlockPopupPanel" runat="server" Width="500px" Height="180px" BackColor="#F4F7FC"
                        Style="display: none">
                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; text-align: center">
                            <asp:Label ID="BlockHeader" runat="server" Font-Bold="True"></asp:Label>
                        </div>
                        <div style="padding-left: 10px; padding-top: 15px; margin-bottom: 10px; padding-bottom: 10px;
                            margin-left: 10px">
                            <asp:TextBox ID="BlockText" runat="server" Height="70px" TextMode="MultiLine" Width="450px"></asp:TextBox>
                        </div>
                        <table width="100%" style="text-align: center">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="BlockAccept" runat="server" Text="Übernehmen" CssClass="TablebuttonLarge"
                                        Font-Bold="True" Width="90px" Height="25px" Style="vertical-align: middle" OnClick="BlockAcceptClick" />
                                    <asp:Button ID="BlockPopupCancel" runat="server" Text="Abbrechen" CssClass="TablebuttonLarge"
                                        Font-Bold="true" Width="90px" Height="25px" Style="vertical-align: middle" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
