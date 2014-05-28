<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change11.aspx.cs" Inherits="AppRemarketing.forms.Change11"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu" style="margin-top: 10px; margin-bottom: 10px">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="innerContentRight">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="innerContentRight" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <script type="text/javascript">
                    function openinfo(url) {
                        fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
                        fenster.focus();
                    }
                </script>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading" style="float: none">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <div id="paginationQuery" style="float: none">
                        <table cellpadding="0" cellspacing="0">
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
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server">
                        <div id="TableQuery" runat="server" style="margin-bottom: 10px">
                            <table cellpadding="0" cellspacing="0">
                                <tr id="tr_Auswahl" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Auswahl</td>
                                    <td class="active" style="width: 88%">
                                        <asp:RadioButtonList ID="rblMahnung" runat="server" 
                                            RepeatDirection="Horizontal" RepeatLayout="Flow" 
                                            onselectedindexchanged="rblMahnung_SelectedIndexChanged1">
                                            <asp:ListItem Value="1" Selected="True">Mahnfrist setzen</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="tr_SearchFin" runat="server" visible="true" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap" style="width:150px">
                                        Fahrgestellnummer:
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <asp:TextBox ID="txtFahrgestellnummer" CssClass="TextBoxNormal" runat="server" 
                                            MaxLength="17" Width="170px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_UploadFin" runat="server"  class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                       Upload:
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <input id="upFileFin" type="file" size="49" name="upFileFin" runat="server" />&nbsp;
                                        <a href="javascript:openinfo('InfoFin.htm');">
                                            <img src="/Services/Images/info.gif" border="0" height="16px" width="16px" alt="Struktur Uploaddatei"
                                                title="Struktur Uploaddatei Fahrgestellnummern" /></a> &nbsp; * max. 900
                                        Datensätze
                                    </td>
                                </tr>                           
                                <tr id="tr_ShowFin" runat="server" visible="false" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap" style="width:150px">
                                        Fahrgestellnummer:</td>
                                    <td class="active" style="width: 88%">
                                        <asp:Label ID="lblFin" runat="server"></asp:Label>
                                    </td>
                                </tr>                            
                                <tr id="tr_ShowKennzeichen" runat="server" visible="false" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Kennzeichen:</td>
                                    <td class="active" style="width: 88%">
                                        <asp:Label ID="lblKennzeichen" runat="server"></asp:Label>
                                    </td>
                                </tr>                            
                                <tr id="tr_ShowAuslieferung" runat="server" visible="false" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Auslieferungsdatum:</td>
                                    <td class="active" style="width: 88%">
                                        <asp:Label ID="lblAuslieferung" runat="server"></asp:Label>
                                    </td>
                                </tr>                            
                                <tr id="tr_ShowEreignis" runat="server" visible="false" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Ereignis:</td>
                                    <td class="active" style="width: 88%">
                                        <asp:RadioButtonList ID="rblEreignis" runat="server" 
                                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem Value="2" Selected="True">Meldung der Zulassung</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>                            
                                <tr id="tr_ShowSollDatum" runat="server" visible="false" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        SOLL-Datum:</td>
                                    <td class="active" style="width: 88%">
                                        <asp:TextBox ID="txtSolldatum" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_DatumVon" runat="server" 
                                            Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtSolldatum">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_Solldatum" runat="server" TargetControlID="txtSolldatum"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <div id="Div1" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </div>
                    </asp:Panel>
                    <div style="float:right;width:100%;text-align:right">
                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                            Height="16px" CausesValidation="False" Font-Underline="False" OnClientClick="Show_BusyBox1();" OnClick="cmdSearch_Click">» Suchen</asp:LinkButton>
                        <asp:LinkButton ID="cmdSend" runat="server" CssClass="Tablebutton" Width="78px"
                            Height="16px" CausesValidation="False" Font-Underline="False" 
                            OnClick="cmdSend_Click" Visible="False">» Senden</asp:LinkButton>
                        <asp:LinkButton ID="cmdBack" runat="server" CssClass="Tablebutton" Width="78px"
                            Height="16px" CausesValidation="False" Font-Underline="False" 
                            OnClick="cmdBack_Click" Visible="False">» Zurück</asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15"  
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" 
                                            OnNeedDataSource="rgGrid1_NeedDataSource" ShowGroupPanel="True" >
                                            <ClientSettings AllowKeyboardNavigation="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" AllowPaging="true" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="FAHRGNR" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="FAHRGNR" SortExpression="FAHRGNR" >
                                                        <HeaderStyle Width="140px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="BEM" SortExpression="BEM" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dataFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
