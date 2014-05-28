<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change15.aspx.cs" Inherits="AppRemarketing.forms.Change15"  MasterPageFile="../Master/AppMaster.Master" %>

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
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="lbCreate">
                        <div id="TableQuery" runat="server" style="margin-bottom: 10px">
                            <table cellpadding="0" cellspacing="0">
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Art der Selbstvermarktung:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:RadioButtonList runat="server" ID="rblSelbstvermarktungsart" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="01" Selected="True">Diebstahl</asp:ListItem>
                                            <asp:ListItem Value="02">Verunfallt</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Selbstvermarkterdatum:
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtSelbstvermarkterdatum" runat="server" 
                                            CssClass="TextBoxNormal" Width="100px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_Selbstvermarkterdatum" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtSelbstvermarkterdatum"/>
                                        <cc1:MaskedEditExtender ID="MEE_Selbstvermarkterdatum" runat="server" TargetControlID="txtSelbstvermarkterdatum"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Upload (max. 5MB/Datei):
                                    </td>
                                    <td class="firstLeft active">
                                        <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1" EnableInlineProgress="false"
                                            EnableViewState="false" ViewStateMode="Disabled" AllowedFileExtensions="pdf"
                                            MaxFileSize="10240000" MultipleFileSelection="Automatic"
                                            Localization-Cancel="Abbrechen" Localization-Remove="Löschen" Localization-Select="Hochladen"
                                            Skin="Simple" ToolTip="Dateinamen: <17-stellige FIN>.pdf" />
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClientClick="Show_BusyBox1();" OnClick="lbCreate_Click">» Weiter </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-bottom: 2px">
                                    Upload-Ergebnis:
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" 
                                            OnNeedDataSource="rgGrid1_NeedDataSource">
                                            <ClientSettings AllowKeyboardNavigation="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="FAHRGESTELLNUMMER" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="FAHRGESTELLNUMMER" SortExpression="FAHRGESTELLNUMMER" >
                                                        <HeaderStyle Width="200px"> 
                                                        </HeaderStyle>
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="STATUS" SortExpression="STATUS" >
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
