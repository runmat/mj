<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change81.aspx.cs" Inherits="Leasing.forms.Change81"
    MasterPageFile="../Master/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
                <a class="active">| Fahrzeugsuche</a>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div>
                        <p>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Black" Font-Bold="True"></asp:Label>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label></p>
                    </div>
                    <div id="VersandTabPanel1" runat="server" class="VersandTabPanel">
                        <table cellspacing="0" cellpadding="0" style="border: solid 1px #dfdfdf;">
                            <tr>
                                <td style="padding-bottom: 0px;" class="PanelHead">
                                    <asp:Label ID="lbl_Fahrzeugdaten" runat="server">Fahrzeugauswahl</asp:Label>
                                </td>
                                <td style="width: 100%">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="padding-top: 0px;">
                                    <asp:Label ID="Label6" runat="server">Bitte wählen Sie das für die Beauftragung vorgesehene Fahrzeug aus.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="padding-left: 7px; width: 50%">
                                    <div class="PanelHeadSuche">
                                        <table width="100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td valign="top" style="padding-left: 1px; padding-bottom: 0px;">
                                                    <asp:Label ID="Label4" runat="server">Suche</asp:Label>
                                                </td>
                                                <td align="right" valign="top" style="padding-bottom: 0px;">
                                                    <asp:ImageButton ID="ibtnFrage" Style="padding-right: 0px;" ToolTip="Bitte geben Sie die Suchkriterien ein und klicken zur Suche auf die Lupe!"
                                                        runat="server" ImageUrl="../../../Images/fragezeichen.gif" />
                                                    <asp:ImageButton ID="NewSearch" CssClass="PanelHeadSucheImg" runat="server" ImageUrl="../../../Images/versand/plusgreen.png"
                                                        OnClientClick="javascript:cpeAllDataCollapsed()" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:Panel ID="pnlAllgDaten" Style="padding-left: 15px" runat="server" Width="100%"
                                        DefaultButton="ibtnSearch">
                                        <div style="background-image: url(../../../Images/Versand/headSucheopen.png); background-repeat: no-repeat;
                                            height: 8px; width: 16px">
                                        </div>
                                        <table cellspacing="0" cellpadding="0">
                                            <tr id="tr_Kennzeichen" runat="server">
                                                <td class="First" style="padding-left: 7px; height: 22px;">
                                                    <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
                                                </td>
                                                <td style="height: 22px">
                                                    <asp:TextBox ID="txtAmtlKennzeichen" Width="300px" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="First" style="height: 22px">
                                                    <asp:Label ID="Label7" runat="server">Beispiel: HH-AB933, HH-AB1*, HH-AB* </asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr_Fahrgestellnummer" runat="server">
                                                <td class="First" style="padding-left: 7px">
                                                    <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFahrgestellnummer" Width="300px" runat="server" MaxLength="17"></asp:TextBox>
                                                </td>
                                                <td class="First" style="width: 300px">
                                                    <asp:Label ID="lbl_Fahrzeugmodell" runat="server">Beispiel: WVW2323KJKJN223J, JKJN223J</asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr_NummerZB2" runat="server">
                                                <td class="First" style="padding-left: 7px">
                                                    <asp:Label ID="lbl_ZBIINummer" runat="server">lbl_ZBIINummer</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNummerZB2" runat="server" Width="300px"></asp:TextBox>
                                                </td>
                                                <td class="First">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="tr_Leasingvertragsnummer" runat="server">
                                                <td class="First" style="padding-left: 7px">
                                                    <asp:Label ID="lbl_Leasingvertragsnummer" runat="server">lbl_Leasingvertragsnummer</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrdernummer" runat="server" Width="300px"></asp:TextBox>
                                                </td>
                                                <td class="First">
                                                    <asp:ImageButton ID="ibtnSearch" ToolTip="Suchen" ImageUrl="../../../images/Versand/search.png"
                                                        runat="server" OnClick="ibtnSearch_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="padding-top: 0px;">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="padding-top: 0px; padding-left: 7px; padding-right: 5px">
                                    <div class="StandardHeadDetail">
                                        <table width="100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="padding-left: 8px; padding-bottom: 0px;">
                                                    <asp:Label ID="Label9" Style="padding-top: 3px;" runat="server" Font-Size="12px"
                                                        Font-Bold="True" ForeColor="White">Dateiupload</asp:Label>
                                                </td>
                                                <td align="right" valign="top" style="padding-bottom: 0px;">
                                                    <asp:ImageButton ID="ImageButton1" runat="server" Style="padding-right: 7px; padding-top: 3px"
                                                        ImageUrl="../../../Images/versand/plusgreen.png" OnClientClick="javascript:cpeUploadCollapsed()" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:Panel BackColor="#DCDCDC" ID="PLUpload" runat="server" Width="100%">
                                        <div class="StandardHeadDetailFlag">
                                        </div>
                                        <table width="100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="First" style="padding-left: 21px">
                                                    <asp:Label ID="lbl_Datei" runat="server">lbl_Datei </asp:Label>
                                                </td>
                                                <td style="width: 25%" nowrap="nowrap">
                                                    <input id="upFile" type="file" size="35" name="File" runat="server" style="background-color: #FFFFFF" />
                                                    &nbsp; <a href="javascript:openinfo('Info.htm');">
                                                        <img src="/Services/Images/info.gif" border="0" height="16px" width="16px" alt="Struktur Uploaddatei"
                                                            title="Struktur Uploaddatei Fahrgestellnummern" /></a>
                                                </td>
                                                <td style="width: 45%">
                                                    <asp:ImageButton ID="ibtnUpload" ToolTip="Suchen" ImageUrl="../../../images/Versand/search.png"
                                                        runat="server" OnClick="ibtnUpload_Click" />
                                                </td>
                                                <td style="width: 10%" align="right">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7" style="padding-top: 0px;">
                                    <asp:Label ID="lblErrorDokumente" CssClass="TextError" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div id="Result" visible="false" style="padding-right: 5px; padding-left: 7px" runat="Server">
                            <div class="StandardHeadDetail">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="padding-left: 8px; padding-bottom: 0px;">
                                            <asp:Label ID="Label20" Style="padding-top: 3px;" runat="server" Font-Size="12px"
                                                Font-Bold="True">Fahrzeugauswahl</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="pagination">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                    OnSorting="GridView1_Sorting">
                                    <PagerSettings Visible="False" />
                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                    <RowStyle CssClass="ItemStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Equipment" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEqunr" Text='<%# DataBinder.Eval(Container, "DataItem.Equnr") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Anfordern">
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle Width="50px" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkAuswahl" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.MANDT").ToString() != "11" %>'
                                                    Checked='<%# DataBinder.Eval(Container, "DataItem.MANDT").ToString() == "99" %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="Fahrgestellnummer"
                                            HeaderStyle-Width="100px">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">Fahrgestellnummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkHistorie" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Leasingnummer" HeaderText="Leasingnummer" HeaderStyle-Width="100px">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Leasingnummer" runat="server" CommandName="Sort" CommandArgument="Leasingnummer">Leasingnummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblLeasingnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Leasingnummer") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="NummerZB2" HeaderText="NummerZB2">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="NummerZB2">NummerZB2</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblNummerZB2" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZB2") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">Kennzeichen</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Ordernummer" HeaderText="Ordernummer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Ordernummer" runat="server" CommandName="Sort" CommandArgument="Ordernummer">Ordernummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblOrdernummer" Text='<%# DataBinder.Eval(Container, "DataItem.Ordernummer") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Abmeldedatum" HeaderText="Abmeldedatum">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Abmeldedatum" runat="server" CommandName="Sort" CommandArgument="Abmeldedatum">Abmeldedatum</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblNoDate" runat="server" Font-Bold="True" Visible='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum") == System.DBNull.Value %>'
                                                    ForeColor="Red">XX.XX.XXXX</asp:Label>
                                                <asp:Label ID="lblYesDate" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum") != System.DBNull.Value %>'
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Abmeldedatum", "{0:d}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="CoC" HeaderText="CoC">
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle Width="50px" />
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_CoC" runat="server" CommandName="Sort" CommandArgument="CoC">CoC</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCoC" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.CoC").ToString()=="X" %>'
                                                    Enabled="False" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Status" HeaderText="Status">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="Status">Status</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblStatus" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="cmdNext" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                    OnClick="cmdNext_Click">» Weiter</asp:LinkButton>
                                &nbsp;
                            </div>
                        </div>
                    </div>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
                <cc1:CollapsiblePanelExtender ID="cpeAllData" runat="Server" TargetControlID="pnlAllgDaten"
                    ExpandControlID="NewSearch" CollapseControlID="NewSearch" Collapsed="false" ImageControlID="NewSearch"
                    ExpandedImage="../../../Images/versand/minusred.png" CollapsedImage="../../../Images/versand/plusgreen.png"
                    SuppressPostBack="true" />
                <cc1:CollapsiblePanelExtender ID="cpeUpload" runat="Server" TargetControlID="PLUpload"
                    ExpandControlID="ImageButton1" CollapseControlID="ImageButton1" Collapsed="true"
                    ImageControlID="ImageButton1" ExpandedImage="../../../Images/versand/minusred.png"
                    CollapsedImage="../../../Images/versand/plusgreen.png" SuppressPostBack="true" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function openinfo(url) {
            fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
            fenster.focus();
        }
        function cpeAllDataCollapsed() {

            var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAllData');
            var Panel2 = $find('ctl00_ContentPlaceHolder1_cpeUpload');
            if (Panel1.get_Collapsed() != false) {
                Panel2._doClose();
            }
        }

        function cpeUploadCollapsed() {

            var Panel1 = $find('ctl00_ContentPlaceHolder1_cpeAllData');
            var Panel2 = $find('ctl00_ContentPlaceHolder1_cpeUpload');
            if (Panel2.get_Collapsed() != false) {
                Panel1._doClose();
            }
        }
    </script>
</asp:Content>
