<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="KVPBewertung.aspx.vb"
    Inherits="KBS.KVPBewertung" MasterPageFile="~/KBS.Master" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="KVP-Bewertung"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" Text="" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="TableQuery" runat="server" style="border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="txtBedienerkarte" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="CardScann">
                        <asp:UpdatePanel runat="server" ID="upGrid">
                            <ContentTemplate>
                                <table id="tblBedienerkarte" runat="server" cellspacing="0" width="100%" cellpadding="0"
                                    bgcolor="white" border="0" style="border-bottom: solid 1px #DFDFDF; border-right: solid 1px #DFDFDF;
                                    border-left: solid 1px #DFDFDF;">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td align="center" class="firstLeft active">
                                            <asp:Label ID="lblBedienError" runat="server" CssClass="TextError">
                                                    Bitte scannen Sie ihre Bedienerkarte.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td id="Usercard" runat="server" align="center" class="firstLeft" style="padding-top: 10px;
                                            padding-bottom: 5px;">
                                            <asp:TextBox ID="txtBedienerkarte" Width="240px" runat="server" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td>
                                            <input id="SendTopSap" type="hidden" runat="server" />&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <script type="text/javascript" language="javascript">

                                    function ControlField(control1) {

                                        if (control1.value.length == 15) {
                                            if (control1.value.substring(control1.value.length - 1) == '}') {
                                                theForm.__EVENTTARGET.value = '__Page';
                                                theForm.__EVENTARGUMENT.value = 'MyCustomArgument';
                                                theForm.submit();
                                                var hiddenInput = document.getElementById("ctl00_ContentPlaceHolder1_SendTopSap");
                                                hiddenInput.value = 1;
                                            } else {
                                                control1.focus();
                                            }
                                        } else {
                                            control1.focus();
                                        }
                                    }     
                                    
                                </script>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                                        AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default">
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
                                                        <asp:ImageButton ID="ImageButton1" runat="server" Width="32" Height="32" ImageUrl="~/Images/Star02_06.jpg"
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
                                                <telerik:GridBoundColumn DataField="BW_FRIST" SortExpression="BW_FRIST" HeaderText="Bewertungsfrist" >
                                                    <HeaderStyle Width="60px" />
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
                        border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">
                        <div>&nbsp;</div>
                        <h2>KVP Kroschke-Vorschlags-Prozess</h2>    
                        <table width="80%" align="center">
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left; vertical-align:top">
                                    Kurzbeschreibung:
                                </td>
                                <td colspan="4" style="text-align:left">
                                    <asp:TextBox runat="server" ID="txtBeschreibung" Width="95%" MaxLength="70" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left; vertical-align:top">
                                    Wie ist die
                                    <br/>
                                    derzeitige Situation?
                                </td>
                                <td colspan="4" style="text-align:left">
                                    <asp:TextBox runat="server" ID="txtSituation" Width="95%" TextMode="MultiLine" Rows="4" MaxLength="400" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left; vertical-align:top">
                                    Was sollte wie
                                    <br/>
                                    verändert werden?
                                </td>
                                <td colspan="4" style="text-align:left">
                                    <asp:TextBox runat="server" ID="txtVeraenderung" Width="95%" TextMode="MultiLine" Rows="4" MaxLength="400" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" style="width:120px; text-align:left; vertical-align:top">
                                    Wem entsteht
                                    <br/>
                                    welcher Vorteil?
                                </td>
                                <td colspan="4" style="text-align:left">
                                    <asp:TextBox runat="server" ID="txtVorteil" Width="95%" TextMode="MultiLine" Rows="4" MaxLength="400" ReadOnly="True"></asp:TextBox>
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
                                    <img src="../images/DaumenHoch.png"/>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <img src="../images/DaumenRunter.png"/>
                                </td>
                                <td style="width:120px">
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnLike" runat="server" Text="Gefällt mir" CssClass="ButtonTouch" Style="margin: 5px 10px 5px 10px;" Width="160px" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnDontLike" runat="server" Text="Gefällt mir nicht" CssClass="ButtonTouch" Style="margin: 5px 10px 5px 10px;" Width="160px" />
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
                        <cc1:ModalPopupExtender ID="mpeConfirmBewertung" runat="server" TargetControlID="btnDummy"
                            PopupControlID="pConfirmBewertung" BackgroundCssClass="divProgress" CancelControlID="btnPanelConfirmBewertungCancel">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pConfirmBewertung" runat="server" Style="overflow: auto; display: none;">
                            <table cellspacing="0" runat="server" bgcolor="white" cellpadding="0" style="overflow: auto;
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
                                    <td align="center" class="firstLeft" style="padding-top: 0px; padding-bottom: 0px;
                                        padding-right: 15px;">
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
                                            Width="78px" CssClass="Tablebutton" CausesValidation="false" />
                                        <asp:LinkButton ID="btnPanelConfirmBewertungOK" runat="server" Text="OK" Height="16px" Width="78px"
                                            CssClass="Tablebutton" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div id="dataFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
