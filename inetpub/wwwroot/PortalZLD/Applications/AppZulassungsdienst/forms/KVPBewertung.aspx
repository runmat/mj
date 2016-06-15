<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KVPBewertung.aspx.cs"
    Inherits="AppZulassungsdienst.forms.KVPBewertung" MasterPageFile="../MasterPage/App.Master" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <style type="text/css">
        .DistanceText
        {
            padding: 3px 0px 0px 0px;
        }
        .DistanceButton
        {
            margin: 3px 0px 3px 0px;
        }
        .NoDistance td
        {
            margin: 0px;
            padding: 0px;
            border: none 0px white;
        }
    </style>
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
                            <asp:Label ID="lblHead" runat="server" Text="KVP-Bewertung"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                    </div>
                    <div id="TableQuery" style="margin-bottom: 0px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2" width="100%" style="padding-top: 0">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError paddingTop"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="Bewertungsliste" runat="server" style="text-align: center; border-bottom: solid 1px #DFDFDF;
                        border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">
                        <div>&nbsp;</div>
                        <h2>KVP Kroschke-Vorschlags-Prozess</h2>    
                        <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblNoData" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="rgGrid1" runat="server" AllowSorting="False" AllowPaging="False"
                                        AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default" 
                                        OnNeedDataSource="rgGrid1_NeedDataSource" OnItemCommand="rgGrid1_ItemCommand">
                                        <ClientSettings AllowKeyboardNavigation="true" >
                                            <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                        </ClientSettings>
                                        <ItemStyle CssClass="ItemStyle" />
                                        <AlternatingItemStyle CssClass="ItemStyle" />
                                        <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="False">
                                            <SortExpressions>
                                                <telerik:GridSortExpression FieldName="RESTTAGE" SortOrder="Ascending" />
                                            </SortExpressions>
                                            <HeaderStyle ForeColor="#595959" />
                                            <Columns>
                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle Width="40px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" Width="32" Height="32" ImageUrl="/PortalZLD/Images/Star02_06.jpg"
                                                            CommandName="bewerten" ToolTip="Bewerten" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="KVPID" SortExpression="KVPID" HeaderText="KVP-Nr." >
                                                    <HeaderStyle Width="90px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="KTEXT" SortExpression="KTEXT" HeaderText="Kurzbeschreibung" >
                                                    <HeaderStyle Width="400px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="BW_FRIST" SortExpression="BW_FRIST" HeaderText="Bewertungsfrist" DataFormatString="{0:dd.MM.yyyy}" >
                                                    <HeaderStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="RESTTAGE" SortExpression="RESTTAGE" Visible="false">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </table>
                        <div>
                            &nbsp;
                        </div>
                    </div>
                    <div id="Bewertung" runat="server" style="text-align: center; border-bottom: solid 1px #DFDFDF;
                        border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;" Visible="false">
                        <div>&nbsp;</div>
                        <h2>KVP Kroschke-Vorschlags-Prozess</h2>    
                        <table width="80%" align="center">
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left; vertical-align:top">
                                    Kurzbeschreibung:
                                </td>
                                <td colspan="4" style="text-align:left">
                                    <asp:TextBox runat="server" ID="txtBeschreibung" Width="572px" MaxLength="70" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left; vertical-align:top">
                                    Wie ist die
                                    <br/>
                                    derzeitige Situation?
                                </td>
                                <td colspan="4" style="text-align:left">
                                    <asp:TextBox runat="server" ID="txtSituation" Width="570px" TextMode="MultiLine" Rows="4" MaxLength="400" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left; vertical-align:top">
                                    Was sollte wie
                                    <br/>
                                    verändert werden?
                                </td>
                                <td colspan="4" style="text-align:left">
                                    <asp:TextBox runat="server" ID="txtVeraenderung" Width="570px" TextMode="MultiLine" Rows="4" MaxLength="400" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left; vertical-align:top">
                                    Wem entsteht
                                    <br/>
                                    welcher Vorteil?
                                </td>
                                <td colspan="4" style="text-align:left">
                                    <asp:TextBox runat="server" ID="txtVorteil" Width="570px" TextMode="MultiLine" Rows="4" MaxLength="400" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left; vertical-align:top">
                                    Bewertungsfrist:
                                </td>
                                <td style="text-align:left">
                                    <asp:TextBox runat="server" ID="txtBewertungsfrist" Width="100px" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td colspan="3">
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="5">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td>
                                </td>
                                <td>
                                    <img src="/PortalZLD/Images/DaumenHoch.png"/>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <img src="/PortalZLD/Images/DaumenRunter.png"/>
                                </td>
                                <td style="width:120px">
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnLike" runat="server" Text="Gefällt mir" OnClick="btnLike_Click" CssClass="TablebuttonLarge" Style="margin: 5px 10px 5px 10px;" Width="128px" Height="30px" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnDontLike" runat="server" Text="Gefällt mir nicht" OnClick="btnDontLike_Click" CssClass="TablebuttonLarge" Style="margin: 5px 10px 5px 10px;" Width="128px" Height="30px" />
                                </td>
                                <td style="width:120px">
                                </td>
                            </tr>
                        </table>
                        <div>
                            &nbsp;
                        </div>
                    </div>
                    <div>
                        <asp:Button ID="btnDummy" runat="server" Width="0" Height="0" BackColor="Transparent"
                            BorderStyle="none" />
                        <ajaxToolkit:ModalPopupExtender ID="mpeConfirmBewertung" runat="server" TargetControlID="btnDummy"
                            PopupControlID="pConfirmBewertung" BackgroundCssClass="divProgress" CancelControlID="btnPanelConfirmBewertungCancel">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pConfirmBewertung" runat="server" Style="overflow: auto; display: none;">
                            <table id="Table1" cellspacing="0" runat="server" bgcolor="white" cellpadding="0" style="overflow: auto;
                                min-height: 100px; min-width: 100px; border: solid 1px #646464; padding: 10px 10px 15px 0px;">
                                <tr>
                                    <td style="padding-top: 0px; padding-bottom: 0px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft" style="padding-top: 0px; padding-bottom: 0px;
                                        padding-right: 15px;">
                                        <asp:Label runat="server" ID="lblBewertung"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 0px; padding-bottom: 0px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft" style="padding-top: 0; padding-bottom: 0;
                                        padding-left: 15px;">
                                        Bitte bestätigen Sie Ihre Entscheidung mit 'OK'
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 0px; padding-bottom: 0px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft" align="center">
                                        <asp:LinkButton ID="btnPanelConfirmBewertungCancel" runat="server" Text="Abbruch" Height="16px"
                                            Width="78px" CssClass="Tablebutton" CausesValidation="false" OnClick="btnPanelConfirmDeleteCancel_Click" />
                                        <asp:LinkButton ID="btnPanelConfirmBewertungOK" runat="server" Text="OK" Height="16px" Width="78px"
                                            CssClass="Tablebutton" CausesValidation="false" OnClick="btnPanelConfirmDeleteOK_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div id="dataFooter" class="dataQueryFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>