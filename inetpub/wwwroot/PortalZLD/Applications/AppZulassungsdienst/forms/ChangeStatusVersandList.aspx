<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeStatusVersandList.aspx.cs"
    Inherits="AppZulassungsdienst.forms.ChangeStatusVersandList" MasterPageFile="../MasterPage/Big.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/PortalZLD/Applications/AppZulassungsdienst/Javascript/GridViewHelper.js"
        type="text/javascript"></script>

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" OnClick="lb_zurueck_Click"></asp:LinkButton>
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
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="vertical-align: top">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="Result" runat="Server">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel" runat="server" OnClick="lnkCreateExcel_Click">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>

                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AllowSorting="true" AllowPaging="false" CssClass="GridView" OnSorting="GridView1_Sorting"
                                                    OnRowCommand="GridView1_RowCommand" PageSize="1000" DataKeyNames="ID">
                                                    <HeaderStyle CssClass="GridTableHead" Width="100%" ForeColor="White" />
                                                    <PagerSettings Visible="False" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnDel" ImageUrl="/PortalZLD/images/del.png" CommandArgument='<%#  ((GridViewRow)Container).RowIndex %>'
                                                                    runat="server" CommandName="Del" ToolTip="Löschen" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="25px" />
                                                            <ItemStyle CssClass="TablePadding" Width="25px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:DropDownList Font-Size="8pt" Width="90px" Visible='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR").ToString() == "10" %>' ID="ddlStatus" runat="server">
                                                                    <asp:ListItem Value="N" Text="offen"></asp:ListItem>
                                                                    <asp:ListItem Value="R" Text="in Klärung"  ></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:HiddenField ID="hfStatus" Value='<%# DataBinder.Eval(Container, "DataItem.BLTYP") %>'
                                                                    runat="server"></asp:HiddenField>
                                                                <asp:HiddenField ID="STATUSVERSAND" Value='<%# DataBinder.Eval(Container, "DataItem.STATUS") %>'
                                                                    runat="server"></asp:HiddenField>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="90px" />
                                                            <ItemStyle CssClass="TablePadding" Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ID" HeaderText="col_ID">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_ID" runat="server" CommandName="Sort" CommandArgument="ID">col_ID</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsapID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="40px" />
                                                            <ItemStyle CssClass="TablePadding" Width="40px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="toDelete" HeaderText="col_LoeschKZ">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_LoeschKZ" runat="server" CommandName="Sort" CommandArgument="LoeschKZ">col_LoeschKZ</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLoeschKZ" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.LoeschKZ").ToString()=="X" %>'
                                                                    Text="L"></asp:Label>
                                                                <asp:Label ID="lblLoeschKZ2" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.LoeschKZ").ToString()=="" %>'
                                                                    Text=""></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="30px" />
                                                            <ItemStyle CssClass="TablePadding" Width="30px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="KUNNR" HeaderText="col_Kundennr">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kundennr" runat="server" CommandName="Sort" CommandArgument="KUNNR">col_Kundennr</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKundennr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KUNNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="57px" />
                                                            <ItemStyle CssClass="TablePadding" Width="57px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="KUNNAME" HeaderText="col_Kundenname">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kundenname" runat="server" CommandName="Sort" CommandArgument="KUNNAME">col_Kundenname</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKundenname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kunde") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="125px" />
                                                            <ItemStyle CssClass="TablePadding" Width="125px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false" HeaderText="col_id_pos">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblid_pos" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="MAKTX" HeaderText="col_Matbez">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Matbez" runat="server" CommandName="Sort" CommandArgument="Matbez">col_Matbez</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMatbez" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dienstleistung") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="125px" />
                                                            <ItemStyle CssClass="TablePadding" Width="125px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Preis" HeaderText="col_Preis">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Preis" runat="server" CommandName="Sort" CommandArgument="Preis">col_Preis</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPreis" runat="server" Text='<%# 	DataBinder.Eval(Container, "DataItem.Preis")  %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="55px" />
                                                            <ItemStyle CssClass="TablePadding" Width="55px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="GebPreis" HeaderText="col_GebPreis">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_GebPreis" runat="server" CommandName="Sort" CommandArgument="Preis">col_GebPreis</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGebPreis" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GebPreis") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="55px" />
                                                            <ItemStyle CssClass="TablePadding" Width="55px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Steuer" HeaderText="col_Steuer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Steuer" runat="server" CommandName="Sort" CommandArgument="Steuer">col_Steuer</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSteuer" Visible='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR").ToString() == "10" %>' runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Steuer") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="55px" />
                                                            <ItemStyle CssClass="TablePadding" Width="55px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="PreisKZ" HeaderText="col_PreisKZ">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_PreisKZ" runat="server" CommandName="Sort" CommandArgument="PreisKZ">col_PreisKZ</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPreisKZ" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR").ToString() == "10" %>' Text='<%# DataBinder.Eval(Container, "DataItem.PreisKZ", "{0:F}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="55px" />
                                                            <ItemStyle CssClass="TablePadding" Width="55px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Zulassungsdatum" HeaderText="col_Zulassungsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Zulassungsdatum"  runat="server" CommandName="Sort" CommandArgument="Zulassungsdatum">col_Zulassungsdatum</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblZulassungsdatum" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR").ToString() == "10" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Zulassungsdatum", "{0:d}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="65px" />
                                                            <ItemStyle CssClass="TablePadding" Width="65px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Referenz1" HeaderText="col_Referenz1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="Referenz1">col_Referenz1</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReferenz1" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR").ToString() == "10" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Referenz1") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="60px" />
                                                            <ItemStyle CssClass="TablePadding" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZKENN" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="ZZKENN">col_Kennzeichen</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblKennKZ1" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR").ToString() == "10" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="65px" />
                                                            <ItemStyle CssClass="TablePadding" Width="65px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Reserviert" HeaderText="col_Reserviert">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Reserviert" runat="server" CommandName="Sort" CommandArgument="Reserviert">col_Reserviert</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReserviert" runat="server" Visible='<%# (DataBinder.Eval(Container, "DataItem.Reserviert").ToString() == "X") %>'
                                                                    Text="R"></asp:Label>
                                                                <asp:Label ID="lblWunschKennz" runat="server" Visible='<%# (DataBinder.Eval(Container, "DataItem.Wunschkennz").ToString() == "X")  && (DataBinder.Eval(Container, "DataItem.Reserviert").ToString() == "") %>'
                                                                    Text="W"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="18px" />
                                                            <ItemStyle CssClass="TablePadding" Width="18px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Feinstaub" HeaderText="col_Feinstaub">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Feinstaub" runat="server" CommandName="Sort" CommandArgument="Feinstaub">col_Feinstaub</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFeinstaub" runat="server" Visible='<%#(DataBinder.Eval(Container, "DataItem.Feinstaub").ToString() == "X") %>'
                                                                    Text="F"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="10px" />
                                                            <ItemStyle CssClass="TablePadding" Width="10px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZL_RL_FRBNR_HIN" HeaderText="col_FrachtHin">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_FrachtHin" runat="server" CommandName="Sort" CommandArgument="FrachtHin">col_FrachtHin</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFrachtHin" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR").ToString() == "10" %>' Text='<%# DataBinder.Eval(Container, "DataItem.FrachtHin") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="85px" />
                                                            <ItemStyle CssClass="TablePadding" Width="85px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZL_RL_FRBNR_ZUR" HeaderText="col_FrachtZUR">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_FrachtZUR" runat="server" CommandName="Sort" CommandArgument="FrachtZur">col_FrachtZUR</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFrachtZUR" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR").ToString() == "10" %>' Text='<%# DataBinder.Eval(Container, "DataItem.FrachtZur") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="85px" />
                                                            <ItemStyle CssClass="TablePadding" Width="85px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Lieferant" HeaderText="col_Lieferant">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Lieferant" runat="server" CommandName="Sort" CommandArgument="Lieferant">col_Lieferant</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLieferant" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Lieferant") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="125px" />
                                                            <ItemStyle CssClass="TablePadding" Width="125px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="PreisLief" HeaderText="col_PreisLief">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_PreisLief" runat="server" CommandName="Sort" CommandArgument="PreisLief">col_PreisLief</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPreisLief" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.PreisLief").ToString() != "0" %>'
                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.PreisLief") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="55px" />
                                                            <ItemStyle CssClass="TablePadding" Width="55px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="WBKunde" HeaderText="col_Erfasst">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_WBKunde" runat="server" CommandName="Sort" CommandArgument="WBKunde">col_WBKunde</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWBKunde" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR").ToString() == "10" %>' Text='<%# DataBinder.Eval(Container, "DataItem.WBKunde") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="40px" />
                                                            <ItemStyle CssClass="TablePadding" Width="40px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Erfasst" HeaderText="col_Erfasst">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Erfasst" runat="server" CommandName="Sort" CommandArgument="Erfasst">col_Erfasst</asp:LinkButton></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblErfasst" runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.ZULPOSNR").ToString() == "10" %>' Text='<%# DataBinder.Eval(Container, "DataItem.Erfasst", "{0:d}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="TablePadding" Width="65px" />
                                                            <ItemStyle CssClass="TablePadding" Width="65px" />
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
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" 
                            Width="78px" onclick="cmdCreate_Click"
                            >» Absenden</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
