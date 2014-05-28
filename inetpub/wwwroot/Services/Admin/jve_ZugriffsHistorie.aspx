<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="jve_ZugriffsHistorie.aspx.vb" Inherits="Admin.jve_ZugriffsHistorie" 
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
                                    <td class="firstLeft active">
                                        ab Datum:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtAbDatum" runat="server" CssClass="TextBoxNormal" Width="130px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_AbDatum" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtAbDatum">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_AbDatum" runat="server" TargetControlID="txtAbDatum"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td class="firstLeft active" width="50">
                                        Kunde:
                                    </td>
                                    <td class="active" width="160">
                                        <asp:DropDownList ID="ddlCustomer" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td width="100%">
                                    &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" width="40">
                                        bis Datum:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtBisDatum" runat="server" CssClass="TextBoxNormal" Width="130px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_BisDatum" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                            Animated="false" Enabled="True" TargetControlID="txtBisDatum">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_BisDatum" runat="server" TargetControlID="txtBisDatum"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td class="firstLeft active">
                                        Benutzer:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtFilterUserName" runat="server" Width="160px" Height="20px">*</asp:TextBox>
                                    </td>
                                    <td width="100%">
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Filter:
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbAll" runat="server" Text="Alle Vorgänge" Checked="True" GroupName="grpKriterium"
                                            AutoPostBack="True" BorderWidth="0"></asp:RadioButton><br />
                                        <asp:RadioButton ID="rbOnline" runat="server" Text="Nur Online-Benutzer" GroupName="grpKriterium"
                                            AutoPostBack="True" BorderWidth="0"></asp:RadioButton><br />
                                        <asp:RadioButton ID="rbError" runat="server" Text="nur fehlerhafte Vorgänge"
                                            GroupName="grpKriterium" AutoPostBack="True" BorderWidth="0"></asp:RadioButton>
                                    </td>
                                    <td colspan="2">
                                        <div>
                                            <font color="#ff0000">&nbsp;&nbsp;&nbsp;Länger als&nbsp;</font>
                                            <asp:DropDownList ID="ddbZeit" runat="server" Width="51px">
                                            </asp:DropDownList>
                                            <font color="#ff0000">&nbsp;Stunde(n) online.</font>
                                        </div>
                                    </td>
                                    <td width="100%"></td>
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
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnExcelMLExportRowCreated="rgGrid1_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid1_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand"  
                                            OnNeedDataSource="rgGrid1_NeedDataSource" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowKeyboardNavigation="true" >
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top" DataKeyNames="idSession" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="userName" SortOrder="Ascending" />
                                                    <telerik:GridSortExpression FieldName="startTime" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <DetailTables>
                                                    <telerik:GridTableView runat="server" DataKeyNames="StandardLogID" >
                                                        <ParentTableRelation>
                                                            <telerik:GridRelationFields DetailKeyField="idSession" MasterKeyField="idSession">
                                                            </telerik:GridRelationFields>
                                                        </ParentTableRelation>
                                                        <SortExpressions>
                                                            <telerik:GridSortExpression FieldName="Inserted" SortOrder="Ascending" />
                                                        </SortExpressions>
                                                        <DetailTables>
                                                            <telerik:GridTableView runat="server" DataKeyNames="StandardLogID" >
                                                                <ParentTableRelation>
                                                                    <telerik:GridRelationFields DetailKeyField="StandardLogID" MasterKeyField="StandardLogID">
                                                                    </telerik:GridRelationFields>
                                                                </ParentTableRelation>
                                                                <SortExpressions>
                                                                    <telerik:GridSortExpression FieldName="BAPI" SortOrder="Ascending" />
                                                                    <telerik:GridSortExpression FieldName="StartTime" SortOrder="Ascending" />
                                                                </SortExpressions>
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="BAPI" SortExpression="BAPI" >
                                                                        <HeaderStyle Width="150px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="StartTime" SortExpression="StartTime" DataFormatString="{0:d}" >
                                                                        <HeaderStyle Width="80px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="EndTime" SortExpression="EndTime" DataFormatString="{0:d}" >
                                                                        <HeaderStyle Width="80px" />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn DataField="Sucess" SortExpression="Sucess" >
                                                                        <HeaderStyle Width="100px" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" Enabled="False" Checked='<%# Eval("Sucess") %>'>
                                                                            </asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridBoundColumn DataField="ErrorMessage" SortExpression="ErrorMessage" >
                                                                        <HeaderStyle Width="100px" />
                                                                    </telerik:GridBoundColumn>
                                                                </Columns>
                                                            </telerik:GridTableView>
                                                        </DetailTables>
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="Inserted" SortExpression="Inserted" DataFormatString="{0:d}" >
                                                                <HeaderStyle Width="80px" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn DataField="Fehler" SortExpression="Fehler" >
                                                                <HeaderStyle Width="100px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox runat="server" Enabled="False" Checked='<%# Eval("Category") = "ERR" %>'>
                                                                    </asp:CheckBox>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="Task" SortExpression="Task" >
                                                                <HeaderStyle Width="100px" />
                                                                <ItemTemplate>
                                                                    <asp:HyperLink runat="server" Text='<%# Eval("Task") %>' NavigateUrl='<%# "../../LogDetails.aspx?StandardLogID=" &amp; Eval("StandardLogID") %>' Target="LogDetails" Visible='<%# IsNumeric(Eval("Anzahl")) %>'>
							                                        </asp:HyperLink>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="Identification" SortExpression="Identification" >
                                                                <HeaderStyle Width="100px" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Description" SortExpression="Description" >
                                                                <HeaderStyle Width="100px" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </telerik:GridTableView>
                                                </DetailTables>
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="userName" SortExpression="userName" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Customername" SortExpression="Customername" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="GroupName" SortExpression="GroupName" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="TestUser" SortExpression="TestUser" >
                                                        <HeaderStyle Width="50px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" Enabled="False" Checked='<%# Eval("TestUser") %>'>
                                                            </asp:CheckBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="hostName" SortExpression="hostName" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="requestType" SortExpression="requestType" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="browser" SortExpression="browser" >
                                                        <HeaderStyle Width="90px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="startTime" SortExpression="startTime" >
                                                        <HeaderStyle Width="120px" />
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# Eval("startTime") %>'
                                                                ForeColor='<%# Eval("StartColor") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="endTime" SortExpression="endTime" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="120px" />
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

