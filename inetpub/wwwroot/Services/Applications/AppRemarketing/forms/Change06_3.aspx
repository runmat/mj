<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change06_3.aspx.cs" Inherits="AppRemarketing.forms.Change06_3"
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
                    <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="innerContentRight">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="innerContentRight" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <script type="text/javascript">
                    function onRequestStart(sender, args) {
                        if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0) {
                            args.set_enableAjax(false);
                        }
                    }
                    function numbersonly(e, decimal) {
                        var key;
                        var keychar;

                        if (window.event) {
                            key = window.event.keyCode;
                        }
                        else if (e) {
                            key = e.which;
                        }
                        else {
                            return true;
                        }
                        keychar = String.fromCharCode(key);

                        if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
                            return true;
                        }
                        else if ((("0123456789").indexOf(keychar) > -1)) {
                            return true;
                        }
                        else if (decimal && (keychar == ",")) {
                            return true;
                        }
                        else
                            return false;
                    }
                    function formatCurr(e) {
                        var strValue = e.value;
                        if (strValue == "") { strValue = "0"; }
                        strValue = strValue.toString().replace(",", ".");
                        if (strValue > 0) {
                            e.value = formatCurrency(strValue.toString().replace(".", ","));
                        }
                    }
                    function formatCurrency(num) {
                        num = num.toString().replace(".", "");
                        num = num.toString().replace(",", ".");
                        if (isNaN(num))
                            num = '0';
                        sign = (num == (num = Math.abs(num)));
                        num = Math.floor(num * 100 + 0.50000000001);
                        cents = num % 100;
                        num = Math.floor(num / 100).toString();
                        if (cents < 10)
                            cents = "0" + cents;
                        for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                            num = num.substring(0, num.length - (4 * i + 3)) + "." +
                            num.substring(num.length - (4 * i + 3));
                        return (((sign) ? "" : "-") + num + ',' + cents);
                    }
                </script>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading" style="float: none">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="cmdSearch">
                        <div id="TableQuery" runat="server" style="margin-bottom: 10px">
                            <table cellpadding="0" cellspacing="0">
                                <tr id="tr_Versandadresse" runat="Server" class="formquery" visible="false">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Versandadresse" runat="server">lbl_Versandadresse</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:DropDownList runat="server" ID="ddlVersandadressen" Width="500px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Zahlungsart" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Zahlungsart" runat="server">lbl_Zahlungsart</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:DropDownList runat="server" ID="ddlZahlungsart" Width="344px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlZahlungsart_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_Bankadresse" visible="false" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Bank" runat="server">lbl_Bank</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:DropDownList runat="server" ID="ddlBankAdressen" Width="500px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="tr_Haendlernummer" class="formquery" runat="server">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_HaendlerNummer" runat="server">lbl_HaendlerNummer</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtNummer" AutoPostBack="true" OnTextChanged="txtNummer_TextChanged"
                                            Width="340px" runat="server" MaxLength="10"></asp:TextBox>
                                        <asp:Label ID="lblSHistoryNR" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="tr_SearchHandler" runat="Server">
                                    <td colspan="2">
                                        <table id="SearchHaendler" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                            <tr id="trNummerDetail" visible="false" class="formquery" runat="server">
                                                <td class="firstLeft active" style="height: 25px">
                                                    <asp:Label ID="lbl_NummerDetail" runat="server">Händlernr.:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%; height: 25px;">
                                                    <asp:TextBox ID="txtNummerDetail" runat="server" MaxLength="10" AutoPostBack="True"
                                                        OnTextChanged="txtNummerDetail_TextChanged"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trName1" visible="false" runat="server" class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label runat="server" ID="lbl_Name1">Name:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtName1" runat="server" MaxLength="35" AutoPostBack="True" OnTextChanged="txtName1_TextChanged"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trPLz" visible="false" runat="server" class="formquery">
                                                <td class="firstLeft active">
                                                    PLZ:
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtPLZ" runat="server" MaxLength="35" AutoPostBack="True" OnTextChanged="txtPLZ_TextChanged"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trOrt" visible="false" runat="server" class="formquery">
                                                <td class="firstLeft active">
                                                    Ort:
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtOrt" runat="server" MaxLength="35" Width="200px" AutoPostBack="True"
                                                        OnTextChanged="txtOrt_TextChanged"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trSelectionButton" visible="false" runat="server" class="formquery">
                                                <td colspan="2" class="firstLeft active" style="height: 57px">
                                                    <asp:Label ID="lbldirektInput" Style="display: none" runat="server" Width="40" ForeColor="green">
				                                <u>Direkteingabe</u>
                                                    </asp:Label><br />
                                                    Anzahl Treffer:
                                                    <asp:Label ID="lblErgebnissAnzahl" runat="server" Width="40"></asp:Label><br />
                                                    <asp:Label ID="lblwait" Style="display: none" runat="server" ForeColor="red" Font-Bold="True">bitte 
                                                        warten</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="right" style="width: 100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trHaendlerAuswahl" runat="server" visible="false">
                                                <td colspan="2" class="firstLeft active">
                                                    <div style="float: left">
                                                        <asp:ListBox ID="lbHaendler" runat="server" Width="500px" Height="126px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="lbHaendler_SelectedIndexChanged"></asp:ListBox>
                                                    </div>
                                                    <div>
                                                        <b>&nbsp;&nbsp;
                                                            <asp:Label ID="lblHalter" runat="server" Font-Size="11pt"></asp:Label></b><br />
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="lblHaendlerName1" runat="server" Font-Size="10pt"></asp:Label><br />
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="lblHaendlerName2" runat="server" Font-Size="10pt"></asp:Label><br />
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="lblHaendlerStrasse" runat="server" Font-Size="10pt"></asp:Label><br />
                                                        <br />
                                                        <b>&nbsp;&nbsp;<asp:Label ID="lblHaendlerPLZ" runat="server" Font-Size="10pt"></asp:Label>
                                                            <br />
                                                            &nbsp;&nbsp;<asp:Label ID="lblHaendlerOrt" runat="server" Font-Size="10pt"></asp:Label>
                                                        </b>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="formquery" id="tr_HaendlerCommands" runat="server">
                                    <td colspan="2" class="rightPadding" align="right">
                                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="TablebuttonMiddle" Width="100px"
                                            Height="16px" OnClick="cmdSearch_Click">» Suchen</asp:LinkButton>
                                        <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" CssClass="TablebuttonMiddle"
                                            Width="100px" Height="16px" OnClick="lbSelektionZurueckSetzen_Click1">» Neue 
                                            Suche</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right" style="width: 100%">
                                        &nbsp;
                                    </td>
                                </tr>                                    
                                <tr id="tr_AdresseHaendler" runat="server" visible="false">
                                    <td align="left" width="100%" colspan="2" class="firstLeft active">
                                        <div style="border: 1px solid #C0C0C0; padding-top: 15px; padding-bottom: 15px">
                                            <table id="tblVersandanschrift" runat="server" cellspacing="0" cellpadding="3" width="100%"
                                                bgcolor="white" border="0">
                                                <tbody>
                                                <tr>
                                                    <td class="firstLeft active" colspan="4" style="width: 100%">
                                                        <asp:Label ID="lbl_AdresseSchluessel" Font-Size="10pt" runat="server" Text="lbl_AdresseSchluessel"></asp:Label>
                                                    </td>
                                                </tr>                                                    
                                                    <tr>
                                                        <td class="firstLeft active">
                                                            <asp:Label ID="lbl_FirmaName" Text="lbl_FirmaName" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNameHaendler" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="active">
                                                            <asp:Label ID="lbl_AnsprechpartnerZusatz" Text="lbl_AnsprechpartnerZusatz" runat="server"></asp:Label>
                                                        </td>
                                                        <td style="width: 100%">
                                                            <asp:TextBox ID="txtName2" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="firstLeft active">
                                                            <asp:Label ID="lbl_Strasse" Text="lbl_Strasse" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="active">
                                                            <asp:TextBox ID="txtStrasse" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="active">
                                                            <asp:Label ID="lbl_Hausnummer" Text="lbl_Hausnummer" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtHausnummer" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="firstLeft active">
                                                            <asp:Label ID="lbl_Postleitzahl" Text="lbl_Postleitzahl" runat="server" CssClass="TextLarge"></asp:Label>
                                                        </td>
                                                        <td class="active">
                                                            <asp:TextBox ID="txtPostleitzahl" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                        </td>
                                                        <td class="active">
                                                            <asp:Label ID="lbl_Ort" Text="lbl_Ort" runat="server" CssClass="TextLarge"></asp:Label>
                                                        </td>
                                                        <td class="active">
                                                            <asp:TextBox ID="txtOrtHaendler" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="firstLeft active">
                                                            <asp:Label ID="lbl_Land" Text="lbl_Land" runat="server" CssClass="TextLarge"></asp:Label>
                                                        </td>
                                                        <td class="active">
                                                            <asp:DropDownList ID="ddlLand" runat="server" CssClass="DropDownNormal">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="active" colspan="2">
                                                            <asp:CheckBox ID="chkAbwAdresseBrief" runat="server" AutoPostBack="True" 
                                                                oncheckedchanged="CheckBox1_CheckedChanged" 
                                                                Text="abweichender Briefempfänger" />
                                                        </td>
                                   
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right" style="width: 100%">
                                        &nbsp;
                                    </td>
                                </tr>                                    
                                <tr id="tr_AdresseBank" runat="server" visible="false">
                                    <td align="left" width="100%" colspan="2" class="firstLeft active">
                                        <div style="border: 1px solid #C0C0C0; padding-top: 15px; padding-bottom: 15px">
                                            <table id="Table1" runat="server" cellspacing="0" cellpadding="3" width="100%" bgcolor="white"
                                                border="0">
                                                <tbody>
                                                <tr>
                                                    <td class="firstLeft active" colspan="4" style="width: 100%">
                                                        <asp:Label ID="lbl_AdresseBank" Font-Size="10pt" runat="server" Text="lbl_AdresseBank"></asp:Label>
                                                    </td>
                                                </tr>                                                      
                                                    <tr>
                                                        <td class="firstLeft active">
                                                            <asp:Label ID="lbl_FirmaNameBank" Text="lbl_FirmaNameBank" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNameBank" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="active">
                                                            <asp:Label ID="lbl_AnsprechpartnerZusatzBank" Text="lbl_AnsprechpartnerZusatzBank" runat="server"></asp:Label>
                                                        </td>
                                                        <td style="width: 100%">
                                                            <asp:TextBox ID="txtName2Bank" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="firstLeft active">
                                                            <asp:Label ID="lbl_StrasseBank" Text="lbl_StrasseBank" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="active">
                                                            <asp:TextBox ID="txtStrasseBank" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="active">
                                                            <asp:Label ID="lbl_HausnummerBank" Text="lbl_HausnummerBank" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtHausnummerBank" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="firstLeft active">
                                                            <asp:Label ID="lbl_PostleitzahlBank" Text="lbl_PostleitzahlBank" runat="server" CssClass="TextLarge"></asp:Label>
                                                        </td>
                                                        <td class="active">
                                                            <asp:TextBox ID="txtPostleitzahlBank" runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                        </td>
                                                        <td class="active">
                                                            <asp:Label ID="lbl_OrtBank" Text="lbl_OrtBank" runat="server" CssClass="TextLarge"></asp:Label>
                                                        </td>
                                                        <td class="active">
                                                            <asp:TextBox ID="txtOrtBank" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="firstLeft active">
                                                            <asp:Label ID="lbl_LandBank" Text="lbl_LandBank" runat="server" CssClass="TextLarge"></asp:Label>
                                                        </td>
                                                        <td class="active">
                                                            <asp:DropDownList ID="ddLandBank" runat="server" CssClass="DropDownNormal">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="active">
                                                            &nbsp;
                                                        </td>
                                                        <td class="active">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
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
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td class="active" style="padding-bottom: 5px">
                                    Freigabedatum:
                                    <asp:TextBox ID="txtFreigabedatumForAll" Width="77px" 
                                        CssClass="TextBoxNormal" runat="server" style="vertical-align: middle"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CE_FreigabedatumForAll" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                        Animated="false" Enabled="True" TargetControlID="txtFreigabedatumForAll">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_FreigabedatumForAll" runat="server" TargetControlID="txtFreigabedatumForAll"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                    <asp:LinkButton ID="lbtnApplyFrDat" runat="server" CssClass="TablebuttonXLarge" Width="155px" Height="16px"
                                        OnClick="lbtnApplyFrDat_Click" style="vertical-align: middle">» für alle übernehmen</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="10" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" EnableHeaderContextMenu="true" 
                                            OnExcelMLExportRowCreated="rgGrid1_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid1_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand" OnItemCreated="rgGrid1_ItemCreated" 
                                            OnNeedDataSource="rgGrid1_NeedDataSource" 
                                            OnItemDataBound="rgGrid1_ItemDataBound" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="340px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="CHASSIS_NUM" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" >
                                                        <HeaderStyle Width="140px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="DZLART" SortExpression="DZLART" >
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="AdresseToolTip" SortExpression="AdresseToolTip" Groupable="false" >
                                                        <HeaderStyle Width="60px" />
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgAdress" runat="server" ImageUrl="/services/images/info.gif" ToolTip='<%# Eval("AdresseToolTip") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="BELDT" SortExpression="BELDT" Groupable="false" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtBelegdatum" CssClass="TextBoxNormal" Text='<%# Eval("BELDT", "{0:d}") %>'
                                                                Width="75px" runat="server"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CE_Belegdatum" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                                Animated="false" Enabled="True" TargetControlID="txtBelegdatum">
                                                            </cc1:CalendarExtender>
                                                            <cc1:MaskedEditExtender ID="MEE_Belegdatum" runat="server" TargetControlID="txtBelegdatum"
                                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                            </cc1:MaskedEditExtender>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="RELDT" SortExpression="RELDT" Groupable="false" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFreigabedatum" Width="77px" Text='<%# Eval("RELDT", "{0:d}") %>'
                                                                CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CE_Freigabedatum" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                                Animated="false" Enabled="True" TargetControlID="txtFreigabedatum">
                                                            </cc1:CalendarExtender>
                                                            <cc1:MaskedEditExtender ID="MEE_Freigabedatum" runat="server" TargetControlID="txtFreigabedatum"
                                                                Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                            </cc1:MaskedEditExtender>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="BELNR" SortExpression="BELNR" Groupable="false" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRechnungsnummer" Text='<%# Eval("BELNR") %>'
                                                                MaxLength="8" Width="70px" CssClass="TextBoxShort" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="BETRAG_RE" SortExpression="BETRAG_RE"  Groupable="false" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRechnungsbetrag" Text='<%# Eval("BETRAG_RE","{0:c}") %>'
                                                                Width="75px" CssClass="TextBoxShort" onblur="formatCurr(this)" onKeyPress="return numbersonly(event, true)" runat="server"></asp:TextBox>
                                                            <cc1:TextBoxWatermarkExtender ID="TextWatermarkRGB" runat="server" TargetControlID="txtRechnungsbetrag" WatermarkCssClass="Watermarked" WatermarkText="z.B. 1185,55" ></cc1:TextBoxWatermarkExtender>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="MATNR" SortExpression="MATNR" Groupable="false" >
                                                        <HeaderStyle Width="170px" />
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlVersandoptionen" Width="170px" runat="server">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid2" Visible="false" runat="server" PageSize="15"  
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnNeedDataSource="rgGrid2_NeedDataSource" >
                                            <ClientSettings AllowKeyboardNavigation="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" AllowPaging="true" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="CHASSIS_NUM" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" >
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="BEM" SortExpression="BEM" >
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
                    <div id="dataFooter" runat="server" style="float:right;width:100%;text-align:right;margin-top:5px;margin-bottom:5px">
                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                            OnClick="cmdSave_Click">» Speichern</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
