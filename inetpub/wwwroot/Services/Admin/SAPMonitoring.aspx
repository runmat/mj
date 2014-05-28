<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SapMonitoring.aspx.vb" Inherits="Admin.SAPMonitoring" 
    MasterPageFile="MasterPage/Admin.Master" %>

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
                </script>
                <div id="innerContentRight" style="width: 100%; margin-bottom: 10px">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="active">
                                    Neue Abfrage
                                </td>
                                <td align="right">
                                    <div id="queryImage">
                                        <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../Images/queryArrow.gif"
                                            ToolTip="Abfrage öffnen" Visible="false" OnClick="NewSearch_Click" />
                                        <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../Images/queryArrowUp.gif"
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
                                    <td class="firstLeft active" width="70">
                                        ab Datum:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtAbDatum" runat="server" Width="130px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_AbDatum" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtAbDatum">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_AbDatum" runat="server" TargetControlID="txtAbDatum"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td width="100%">
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" width="70">
                                        bis Datum:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtBisDatum" runat="server" Width="130px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_BisDatum" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtBisDatum">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_BisDatum" runat="server" TargetControlID="txtBisDatum"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td width="100%">
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" width="70">
                                        Kriterium
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbBAPI" runat="server" Text="BAPI" GroupName="grpKriterium"
                                            AutoPostBack="True" BorderWidth="0"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbASPX" runat="server" Text="ASPX-Seite" Checked="True" GroupName="grpKriterium"
                                            AutoPostBack="True" BorderWidth="0"></asp:RadioButton>
                                    </td>
                                    <td width="100%">
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" width="70">
                                        Auswahl:
                                    </td>
                                    <td width="160">
                                        <asp:DropDownList ID="ddlAuswahl" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td width="100%">
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
                            OnClientClick="Show_BusyBox1();" OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnExcelMLExportRowCreated="rgGrid_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand"  
                                            OnNeedDataSource="rgGrid1_NeedDataSource" Visible="false" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowKeyboardNavigation="true" >
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" AllowPaging="true" CommandItemDisplay="Top" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="BAPI" SortOrder="Ascending" />
                                                    <telerik:GridSortExpression FieldName="Benutzer" SortOrder="Ascending" />
                                                    <telerik:GridSortExpression FieldName="Start" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="BAPI" SortExpression="BAPI" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Description" SortExpression="Description" >
                                                        <HeaderStyle Width="300px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Benutzer" SortExpression="Benutzer" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="Testbenutzer" SortExpression="Testbenutzer" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# Eval("Testbenutzer") %>'>
                                                            </asp:CheckBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="Start" SortExpression="Start" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Ende" SortExpression="Ende" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="Dauer" SortExpression="Dauer" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("Dauer") %>'>
                                                            </asp:Label>
                                                            <asp:Label ID="Label1" runat="server" Width='<%# Unit.Pixel(CInt(Eval("Dauer"))) %>'
                                                                Height="10px" BackColor="#8080FF">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Erfolg" SortExpression="Erfolg" >
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox2" runat="server" Enabled="False" Checked='<%# Eval("Erfolg") %>'>
                                                            </asp:CheckBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="Fehlermeldung" SortExpression="Fehlermeldung" >
                                                        <HeaderStyle Width="400px" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid2" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnExcelMLExportRowCreated="rgGrid_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid2_ItemCommand"  
                                            OnNeedDataSource="rgGrid2_NeedDataSource" Visible="false" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowKeyboardNavigation="true" >
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top" DataKeyNames="ASPX_ID" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="Seite" SortOrder="Ascending" />
                                                    <telerik:GridSortExpression FieldName="Benutzer" SortOrder="Ascending" />
                                                    <telerik:GridSortExpression FieldName="Start ASPX" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <DetailTables>
                                                    <telerik:GridTableView runat="server" >
                                                        <ParentTableRelation>
                                                            <telerik:GridRelationFields DetailKeyField="ASPX_ID" MasterKeyField="ASPX_ID">
                                                            </telerik:GridRelationFields>
                                                        </ParentTableRelation>
                                                        <SortExpressions>
                                                            <telerik:GridSortExpression FieldName="Start SAP" SortOrder="Ascending" />
                                                        </SortExpressions>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn DataField="BAPI" SortExpression="BAPI" >
                                                                <HeaderStyle Width="150px" />
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="Hyperlink1" runat="server" Text='<%# Eval("BAPI") %>'
                                                                        ToolTip='<%# Eval("Beschreibung") %>'>
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="Erfolg" SortExpression="Erfolg" >
                                                                <HeaderStyle Width="60px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# Eval("Erfolg") %>' ToolTip='<%# Eval("Fehlermeldung") %>'>
                                                                    </asp:CheckBox>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="Start SAP" SortExpression="Start SAP" >
                                                                <HeaderStyle Width="90px" />
                                                                <ItemStyle Wrap="false" />
                                                                <ItemTemplate>
							                                        <asp:HyperLink id="HyperLink2" runat="server" Text='<%# Eval("Start SAP") %>' ToolTip='<%# Eval("Ende SAP") %>'>
							                                        </asp:HyperLink>
						                                        </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="Dauer SAP" SortExpression="Dauer SAP" >
                                                                <HeaderStyle Width="250px" />
						                                        <ItemStyle Wrap="false" />
                                                                <ItemTemplate>
							                                        <table id="Table18" cellspacing="0" cellpadding="0" border="0">
								                                        <tr>
									                                        <td align="right" width="50">
										                                        &nbsp;
                                                                            </td>
									                                        <td align="right" width="30">
										                                        <asp:Label id="Label6" runat="server" Text='<%# Eval("Dauer SAP") %>'>
										                                        </asp:Label>&nbsp;
                                                                            </td>
									                                        <td width="125">
										                                        <asp:Label id="Label7" runat="server" BackColor="Highlight" Width='<%# Unit.Pixel(CInt(Eval("Dauer SAP"))) %>' Height="10px">
										                                        </asp:Label>
                                                                            </td>
								                                        </tr>
							                                        </table>
						                                        </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </telerik:GridTableView>
                                                </DetailTables>
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="Seite" SortExpression="Seite" >
                                                        <HeaderStyle Width="250px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Anwendung" SortExpression="Anwendung" >
                                                        <HeaderStyle Width="200px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Zugriffe SAP" SortExpression="Zugriffe SAP" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Benutzer" SortExpression="Benutzer" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Kunnr" SortExpression="Kunnr" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Customername" SortExpression="Customername" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="AccountingArea" SortExpression="AccountingArea" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="Testbenutzer" SortExpression="Testbenutzer" >
                                                        <HeaderStyle Width="90px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox4" runat="server" Enabled="False" Checked='<%# Eval("Testbenutzer") %>'>
                                                            </asp:CheckBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Start ASPX" SortExpression="Start ASPX" >
                                                        <HeaderStyle Width="120px" />
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="Hyperlink4" runat="server" Text='<%# Eval("Start ASPX") %>'
                                                                ToolTip='<%# Eval("Ende ASPX") %>'>
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Dauer ASPX" SortExpression="Dauer ASPX" >
                                                        <HeaderStyle Width="200px" />
                                                        <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <table cellspacing="0" cellpadding="0" border="0">
                                                                <tr>
                                                                    <td width="50">
                                                                        ASPX
                                                                    </td>
                                                                    <td width="30" align="right">
                                                                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("Dauer ASPX") %>'>
                                                                        </asp:Label>&nbsp;
                                                                    </td>
                                                                    <td width="125">
                                                                        <asp:Label ID="Label5" runat="server" Width='<%# Unit.Pixel(CInt(Eval("Dauer ASPX"))) %>'
                                                                            Height="10px" BackColor="#8080FF">
                                                                        </asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="25">
                                                                        SAP
                                                                    </td>
                                                                    <td width="30" align="right">
                                                                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("Dauer SAP") %>'>
                                                                        </asp:Label>&nbsp;
                                                                    </td>
                                                                    <td width="125">
                                                                        <asp:Label ID="Label7" runat="server" Width='<%# Unit.Pixel(CInt(Eval("Dauer SAP"))) %>'
                                                                            Height="10px" BackColor="Highlight">
                                                                        </asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
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

