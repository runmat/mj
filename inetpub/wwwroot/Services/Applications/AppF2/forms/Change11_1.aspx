<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change11_1.aspx.vb" Inherits="AppF2.Change11_1"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label" />
                        </h1>
                    </div>
                    <uc1:Kopfdaten ID="kopfdaten" runat="server" />
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lbHaendler" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtNummer" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtName1" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtName2" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtPLZ" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtOrt" EventName="TextChanged" />
                            <asp:PostBackTrigger ControlID="cmdNext" />
                            <asp:PostBackTrigger ControlID="cmdDone" />
                        </Triggers>--%>
                        <ContentTemplate>
                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                <%--<AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="fzgGrid">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="fzgGrid" />
                                            <telerik:AjaxUpdatedControl ControlID="lblError" />
                                            <telerik:AjaxUpdatedControl ControlID="lblNoData" />
                                            <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                                            <telerik:AjaxUpdatedControl ControlID="cmdNext" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    <telerik:AjaxSetting AjaxControlID="selFzgGrid">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="selFzgGrid" />
                                            <telerik:AjaxUpdatedControl ControlID="cmdDone" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    <telerik:AjaxSetting AjaxControlID="cboAbrufgrund">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="cboAbrufgrund" />
                                            <telerik:AjaxUpdatedControl ControlID="selFzgGrid" />
                                            <telerik:AjaxUpdatedControl ControlID="cmdDone" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    <telerik:AjaxSetting AjaxControlID="cmdNext">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="views" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    <telerik:AjaxSetting AjaxControlID="cmdDone">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="views" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>--%>
                            </telerik:RadAjaxManager>
                            <asp:MultiView ActiveViewIndex="0" runat="server" ID="views">
                                <asp:View runat="server" ID="fahrzeugView">
                                    <%--<div id="paginationQuery">
                                    </div>--%>
                                    <div id="TableQuery">
                                        <table cellpadding="0" cellspacing="0">
                                            <tbody>
                                                <tr>
                                                    <td class="active">
                                                        <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="false" />
                                                    </td>
                                                    <td align="right">
                                                        <div id="queryImage">
                                                            <asp:ImageButton ID="NewSearch" runat="server" ToolTip="Abfrage öffnen" ImageUrl="../../../Images/queryArrow.gif"
                                                                Visible="false" OnClick="NewSearch_Click" />
                                                            <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                                                Visible="false" OnClick="NewSearchUp_Click" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_Fahrgestellnummer" runat="server">Fahrgestellnummer:</asp:Label>
                                                </td>
                                                <td class="active" style="padding-left: 10px">
                                                    <asp:TextBox ID="txt_Fahrgestellnummer" runat="server" Width="250px" MaxLength="35"></asp:TextBox>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="Tr1" runat="server">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lbl_ZBII" runat="server">Nummer ZBII:</asp:Label><br>
                                                </td>
                                                <td class="active" style="padding-left: 10px">
                                                    <asp:TextBox ID="txt_ZBII" runat="server" Width="250px" MaxLength="35"></asp:TextBox>
                                                </td>
                                                <td class="active">
                                                    <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                                        Height="16px" CausesValidation="False" Font-Underline="False" OnClick="SearchClick">» Suche</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td class="active" style="padding-left: 5px">
                                                    (Eingabe von Platzhaltern möglich, z.B. *12345 oder 12345*)
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 2px;">
                                        </div>
                                    </div>
                                    <div style="clear:both; float:none; height:0;" ></div>
                                    <telerik:RadGrid ID="fzgGrid" AllowSorting="true" AllowPaging="true" AllowAutomaticInserts="false"
                                        AutoGenerateColumns="false" PageSize="20" runat="server" GridLines="None" Width="100%"
                                        BorderWidth="0" Culture="de-DE" Visible="false" OnNeedDataSource="GridNeedDataSource">
                                        <MasterTableView CommandItemDisplay="Top" Summary="Fahrzeuge">
                                            <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                                            <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false"
                                                ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn DataField="Selected" SortExpression="Selected">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="selectedCheckbox" AutoPostBack="true" OnCheckedChanged="selectedChanged"
                                                            Checked='<%# Eval("Selected") %>' /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <%--Selektion--%><telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM"
                                                    UniqueName="CHASSIS_NUM" />
                                                <telerik:GridBoundColumn DataField="TIDNR" SortExpression="TIDNR" UniqueName="TIDNR" />
                                                <telerik:GridTemplateColumn DataField="ZZCOCKZ" SortExpression="ZZCOCKZ" UniqueName="ZZCOCKZ">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("ZZCOCKZ") = "X" %>'
                                                            Enabled="false" /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <%--EQUI-COC-Bescheinigung vorhanden=X, E--%><telerik:GridBoundColumn DataField="LICENSE_NUM"
                                                    SortExpression="LICENSE_NUM" UniqueName="LICENSE_NUM" />
                                                <telerik:GridBoundColumn DataField="ERDAT" SortExpression="ERDAT" DataFormatString="{0:dd.MM.yyyy}"
                                                    UniqueName="ERDAT" />
                                                <telerik:GridTemplateColumn DataField="ZZBEZAHLT" SortExpression="ZZBEZAHLT" UniqueName="ZZBEZAHLT">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Eval("ZZBEZAHLT") = "X" %>'
                                                            Enabled="false" /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <%--DAD FFD KFZ Brief bezahlt--%><telerik:GridBoundColumn DataField="ZZREFERENZ1"
                                                    SortExpression="ZZREFERENZ1" UniqueName="ZZREFERENZ1" />
                                                <telerik:GridBoundColumn DataField="ZZABMDAT" SortExpression="ZZABMDAT" DataFormatString="{0:dd.MM.yyyy}"
                                                    UniqueName="ZZABMDAT" />
                                                <telerik:GridBoundColumn DataField="EQUNR" SortExpression="EQUNR" UniqueName="EQUNR" />
                                                <telerik:GridBoundColumn DataField="LIZNR" SortExpression="LIZNR" UniqueName="LIZNR" />
                                                <telerik:GridBoundColumn DataField="ZZFINART" SortExpression="ZZFINART" UniqueName="ZZFINART" />
                                                <telerik:GridBoundColumn DataField="ZZLAUFZEIT" SortExpression="ZZLAUFZEIT" UniqueName="ZZLAUFZEIT" />
                                                <telerik:GridBoundColumn DataField="ZZLABEL" SortExpression="ZZLABEL" UniqueName="ZZLABEL" />
                                            </Columns>
                                        </MasterTableView><ClientSettings>
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                    <div id="dataFooter">
                                        <asp:LinkButton ID="cmdNext" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                            CausesValidation="False" Font-Underline="False" Enabled="false" OnClick="NextClick">» Weiter</asp:LinkButton>
                                    </div>
                                </asp:View>
                                <asp:View runat="server" ID="abrufgrundView">
                                    <div id="TableQuery">
                                        <table id="tab1" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label runat="server">Betreff für Empfänger:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txt_Subject" runat="server" Width="250px" MaxLength="23" />
                                                </td>
                                                <td class="active">
                                                    &nbsp;
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label runat="server">Abrufgrund:</asp:Label><br>
                                                </td>
                                                <td class="active">
                                                    <asp:DropDownList ID="cboAbrufgrund" runat="server" AutoPostBack="true" Width="250px"
                                                        DataValueField="SapWert" DataTextField="WebBezeichnung" DataSourceID="augruSource"
                                                        OnSelectedIndexChanged="augruSelectedAll" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="-- Bitte wählen --" Value="" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <telerik:RadGrid ID="selFzgGrid" AllowSorting="true" AllowPaging="true" AllowAutomaticInserts="false"
                                        AutoGenerateColumns="false" PageSize="20" runat="server" GridLines="None" Width="100%"
                                        BorderWidth="0" Culture="de-DE" Visible="false" OnNeedDataSource="GridNeedDataSource">
                                        <MasterTableView CommandItemDisplay="Top" Summary="Gewählte Fahrzeuge">
                                            <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="{4} Seite <strong>{0}</strong> von <strong>{1}</strong>, insgesamt <strong>{5}</strong> Einträge" />
                                            <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false"
                                                ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn DataField="Selected" SortExpression="Selected">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" Checked='<%# Eval("Selected") %>' Enabled="false" /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="CHASSIS_NUM" SortExpression="CHASSIS_NUM" UniqueName="CHASSIS_NUM" />
                                                <telerik:GridBoundColumn DataField="TIDNR" SortExpression="TIDNR" UniqueName="TIDNR" />
                                                <telerik:GridTemplateColumn DataField="ZZCOCKZ" SortExpression="ZZCOCKZ" UniqueName="ZZCOCKZ">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" Checked='<%# Eval("ZZCOCKZ") = "X" %>' Enabled="false" /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="LICENSE_NUM" SortExpression="LICENSE_NUM" UniqueName="LICENSE_NUM" />
                                                <telerik:GridBoundColumn DataField="ERDAT" SortExpression="ERDAT" DataFormatString="{0:dd.MM.yyyy}"
                                                    UniqueName="ERDAT" />
                                                <telerik:GridTemplateColumn DataField="ZZBEZAHLT" SortExpression="ZZBEZAHLT" UniqueName="ZZBEZAHLT">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" Checked='<%# Eval("ZZBEZAHLT") = "X" %>' Enabled="false" /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DataField="AUGRU" SortExpression="AUGRU" UniqueName="AUGRU">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="augruCombox" runat="server" AutoPostBack="true" DataValueField="SapWert"
                                                            DataTextField="WebBezeichnung" DataSourceID="augruSource" OnSelectedIndexChanged="augruSelectedItem"
                                                            SelectedValue='<%# Eval("AUGRU") %>' AppendDataBoundItems="true" Enabled='<%# Eval("AUGRU") <> "168" AndAlso Eval("AUGRU") <> "169" %>'>
                                                            <asp:ListItem Text="-- Bitte wählen --" Value="" />
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1" UniqueName="ZZREFERENZ1" />
                                                <telerik:GridBoundColumn DataField="EQUNR" SortExpression="EQUNR" UniqueName="EQUNR" />
                                                <telerik:GridBoundColumn DataField="LIZNR" SortExpression="LIZNR" UniqueName="LIZNR" />
                                                <telerik:GridBoundColumn DataField="ZZFINART" SortExpression="ZZFINART" UniqueName="ZZFINART" />
                                                <telerik:GridBoundColumn DataField="ZZLAUFZEIT" SortExpression="ZZLAUFZEIT" UniqueName="ZZLAUFZEIT" />
                                                <telerik:GridBoundColumn DataField="ZZLABEL" SortExpression="ZZLABEL" UniqueName="ZZLABEL" />
                                            </Columns>
                                        </MasterTableView><ClientSettings>
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                    <div id="dataFooter">
                                        <asp:LinkButton ID="cmdDone" runat="server" CssClass="Tablebutton" Width="78px" Height="16px"
                                            CausesValidation="False" Font-Underline="False" Enabled="false" OnClick="DoneClick">» Weiter</asp:LinkButton>
                                    </div>
                                </asp:View>
                            </asp:MultiView>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" Visible="false" />
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False" />
                            <asp:Label ID="lblNoData" runat="server" Font-Bold="true" EnableViewState="false"
                                Visible="false" />
                            <asp:SqlDataSource ID="augruSource" runat="server" CancelSelectOnNullParameter="true"
                                DataSourceMode="DataSet" EnableCaching="true" SelectCommand="SELECT GrundID, WebBezeichnung, SapWert, MitZusatzText, Zusatzbemerkung, VersandadressArt, Eingeschraenkt FROM CustomerAbrufgruende WHERE CustomerID=@cID AND GroupID=@gID AND AbrufTyp='endg' AND SAPWERT <> '000';">
                                <SelectParameters>
                                    <asp:Parameter Name="cID" />
                                    <asp:Parameter Name="gID" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
