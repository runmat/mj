<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Input_002_03.aspx.cs" Inherits="Leasing.forms.Input_002_03" MasterPageFile="../Master/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                            </h1>
                        </div>
                        <div id="TableQuery">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;
                                border: none">
                                <tr class="formquery">
                                    <td class="firstLeft active" style="width: 100%; height: 19px;">
                                        <asp:Label ID="lblNoData" runat="server" CssClass="TextError" Visible="false"></asp:Label><br />
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="Result">
                            <div class="ExcelDiv">
                                <div align="right" class="rightPadding">
                                    <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                    <span class="ExcelSpan">
                                        <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server" OnClick="lnkCreateExcel1_Click">Excel 
                                        herunterladen</asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                            <div id="pagination" style="border-width: 0px;">
                                <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                            </div>
                            <div id="data">
                                <asp:GridView AutoGenerateColumns="false" BackColor="White" runat="server" ID="GridView1"
                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True" onsorting="GridView1_Sorting"
                                    Style="width: auto;" OnRowCommand="GridView1_RowCommand">
                                    <PagerSettings Visible="False" />
                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                    <RowStyle CssClass="ItemStyle" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="85" />
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="Hyperlink3" runat="server" Target="_blank" ImageUrl="/Services/Images/Lupe_16x16.gif"
                                                    NavigateUrl='<%# "Report_002_02.aspx?equipment=" + DataBinder.Eval(Container.DataItem, "Equipment") + "&amp;kf=" + DataBinder.Eval(Container.DataItem, "Klaerfall") %>'>Details</asp:HyperLink>
                                                &nbsp;
                                                <asp:ImageButton ID="ibtnFormular" CommandName="Formular" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Equipment") %>'
                                                    ImageUrl="/Services/Images/Formular.gif" runat="server" Visible='<%# DataBinder.Eval(Container.DataItem, "Klaerfall")!= "" %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEquipment" Text='<%# DataBinder.Eval(Container, "DataItem.Equipment") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Angelegt" HeaderText="col_Angelegt">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Angelegt" runat="server" CommandName="Sort" CommandArgument="Angelegt">col_Angelegt</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblAngelegt" Text='<%# DataBinder.Eval(Container, "DataItem.Angelegt", "{0:d}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="LVNr" HeaderText="col_LVNr">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_LVNr" runat="server" CommandName="Sort" CommandArgument="LvNr">col_LVNr</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblLVNr" Text='<%# DataBinder.Eval(Container, "DataItem.LVNr") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Beginn" HeaderText="col_Beginn">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Beginn" runat="server" CommandName="Sort" CommandArgument="Beginn">col_Beginn</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBeginn" Text='<%# DataBinder.Eval(Container, "DataItem.Beginn", "{0:d}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="GeplEnde" HeaderText="col_GeplEnde">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_GeplEnde" runat="server" CommandName="Sort" CommandArgument="GeplEnde">col_GeplEnde</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblGeplEnde" Text='<%# DataBinder.Eval(Container, "DataItem.GeplEnde", "{0:d}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Kundennummer" HeaderText="col_Kundennummer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Kundennummer" runat="server" CommandName="Sort" CommandArgument="Kundennummer">col_Kundennummer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblKundennummer" Text='<%# DataBinder.Eval(Container, "DataItem.Kundennummer") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Kundenname" HeaderText="col_Kundenname">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Kundenname" runat="server" CommandName="Sort" CommandArgument="Kundenname">col_Kundenname</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblKundenname" Text='<%# DataBinder.Eval(Container, "DataItem.Kundenname") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Name1" HeaderText="col_Kundenbetreuer">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Kundenbetreuer" runat="server" CommandName="Sort" CommandArgument="Name1">col_Kundenbetreuer</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblKundenbetreuer" Text='<%# DataBinder.Eval(Container, "DataItem.Name1") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Schluesselnr_Klaerfall" HeaderText="col_Schluesselnr_Klaerfall">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Schluesselnr_Klaerfall" runat="server" CommandName="Sort" CommandArgument="Schluesselnr_Klaerfall">col_Schluesselnr_Klaerfall</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSchluesselnr_Klaerfall" Text='<%# DataBinder.Eval(Container, "DataItem.Schluesselnr_Klaerfall") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Klaerfall" HeaderText="col_Klaerfall">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Klaerfall" runat="server" CommandName="Sort" CommandArgument="Klaerfall">col_Klaerfall</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblKlaerfall" Text='<%# DataBinder.Eval(Container, "DataItem.Klaerfall") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Info" HeaderText="col_Info">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="col_Info" runat="server" CommandName="Sort" CommandArgument="Info">col_Info</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblInfo" Text='<%# DataBinder.Eval(Container, "DataItem.Info") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                                <table id="tblBanner" cellspacing="0" cellpadding="3" runat="server">
                                    <tbody>
                                        <tr>
                                            <td>
                                                KF1
                                            </td>
                                            <td>
                                                =
                                            </td>
                                            <td style="width:100%">
                                                Leasingnehmer unterschreibt nicht auf dem Sicherungsschein oder Mahnstufe 4
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                KF2
                                            </td>
                                            <td>
                                                =
                                            </td>
                                            <td>
                                                Versicherung unterschreibt nicht, bzw. kein Stempel auf dem Sicherungsschein oder
                                                Mahnstufe 4
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                KF3
                                            </td>
                                            <td>
                                                =
                                            </td>
                                            <td>
                                                Leasingnehmer hält sich nicht an das Gültigkeitsschema zum Ausfüllen des Sicherungsscheins
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                KF4
                                            </td>
                                            <td>
                                                =
                                            </td>
                                            <td>
                                                Versicherung hält sich nicht an das Gültigkeitsschema zum Ausfüllen des Sicherungsscheins
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                KF5
                                            </td>
                                            <td>
                                                =
                                            </td>
                                            <td>
                                                Leasingnehmer und Versicherungsnehmer sind nicht identisch
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                KF6
                                            </td>
                                            <td>
                                                =
                                            </td>
                                            <td>
                                                §38/§39-Schreiben erhalten (Nichtzahlung der Versicherungsprämie)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                KF7
                                            </td>
                                            <td>
                                                =
                                            </td>
                                            <td>
                                                Kündigung der Versicherung
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>                            
                        </div>
                        <div id="dataFooter" runat="server">
                            &nbsp;
                        </div>
                        <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnFake"
                            PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="cmdCancel">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="mb" runat="server" Width="470px" Height="300px" BackColor="White"
                            Style="display: none; border: solid 2px #ff9138; color: #595959; font-weight: bold">
                            <div style="padding-left: 5px; padding-top: 20px; margin-bottom: 10px;">
                                <asp:Label ID="lblMessagePopUp" runat="server" Font-Bold="True" CssClass="TextError"></asp:Label>
                            </div>
                            <table width="100%">
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px">
                                        LV-Nr:
                                    </td>
                                    <td width="100%">
                                        <strong>
                                            <asp:Label ID="lblLVNr" runat="server"></asp:Label></strong>
                                        <asp:HiddenField ID="hfEquinr" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px" nowrap="nowrap">
                                        LV beendet zum
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDatum" MaxLength="10" runat="server"></asp:TextBox>
                                        <ajaxToolkit:TextBoxWatermarkExtender ID="extWatermarkEmail" runat="server" TargetControlID="txtDatum"
                                            WatermarkText="dd.mm.yyyyy" WatermarkCssClass="Watermarked">
                                        </ajaxToolkit:TextBoxWatermarkExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px" nowrap="nowrap">
                                        SB ist in Ordnung
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxSB" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px" nowrap="nowrap">
                                        Höhe der Entschädigung im<br />
                                        Schadensfall ist in Ordnung
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxEnt" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px; height: 27px;" nowrap="nowrap">
                                        Versichererwechsel
                                    </td>
                                    <td style="height: 27px">
                                        <asp:CheckBox ID="cbxVers" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px" nowrap="nowrap">
                                        Fahrzeugwechsel
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxFahrz" runat="server"></asp:CheckBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 5px; padding-left: 15px">
                                        Sonstiges
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="txtBemerkung" runat="server" Width="256px" MaxLength="256"></asp:TextBox>
                                        <asp:Image ID="Image2" runat="server" ImageUrl="/Portal/Images/info.GIF" ToolTip="Maximal 256 Zeichen">
                                        </asp:Image>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap="nowrap">
                                        &nbsp;
                                    </td>
                                    <td nowrap="nowrap">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2" style="width: 100%; padding-right: 20px;">
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                            TabIndex="30" OnClick="cmdSave_Click">» Absenden</asp:LinkButton><asp:LinkButton
                                                ID="cmdCancel" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                                TabIndex="30">» Abbrechen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
