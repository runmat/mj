<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="telerik01.aspx.cs" Inherits="AppRemarketing.forms.telerik01" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <%--<asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="lb_zurueck_Click1"></asp:LinkButton>--%>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                                        ToolTip="Abfrage öffnen" Visible="false" onclick="NewSearch_Click" />
                                                    <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen"
                                                        ImageUrl="../../../Images/queryArrowUp.gif" 
                                                        Visible="false" onclick="NewSearchUp_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False" EnableViewState="False"></asp:Label>
                                                </td>
                                                <td style="width: 100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trVermieter" class="formquery">
                                                <td class="firstLeft active">
                                                    Autovermieter:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:DropDownList ID="ddlVermieter" runat="server" Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr id="trHC" runat="server" visible="false" class="formquery">
                                                <td class="firstLeft active" nowrap="nowrap" style="height: 22px" width="150px">
                                                    Hereinnahmecenter
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:DropDownList ID="ddlHC" runat="server" Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                        Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Kennzeichen:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                                                        Width="200px"></asp:TextBox>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Inventarnummer:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtInventarnummer" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                                                        Width="200px"></asp:TextBox>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Eingangsdatum von:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumVon" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                        Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Eingangsdatum bis:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumBis" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                        Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Vertragsjahr:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtVertragsjahr" runat="server" CssClass="TextBoxNormal" MaxLength="4"
                                                        Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" />&nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>


                            </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                                    OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" runat="server" OnClick="lnkCreateExcel1_Click">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                              
                                             
                                           </td>
                                            
                                        </tr>
                                    </table>
                                </div>
                            </div>
                               <div>
                                 <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" 
                                                    AllowSorting="True" AutoGenerateColumns="False" 
                                       CellSpacing="0" Culture="de-DE" 
                                                    GridLines="None" onpageindexchanged="RadGrid1_PageIndexChanged" 
                                                    onpagesizechanged="RadGrid1_PageSizeChanged" 
                                                    onsortcommand="RadGrid1_SortCommand" 
                                       oncolumnsreorder="RadGrid1_ColumnsReorder">
                                                    <ClientSettings AllowColumnsReorder="True">
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                                            Visible="True">
                                                            <HeaderStyle Width="20px" />
                                                        </RowIndicatorColumn>
                                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                                            Visible="True">
                                                            <HeaderStyle Width="20px" />
                                                        </ExpandCollapseColumn>
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="FAHRGNR" 
                                                                FilterControlAltText="Filter column column" HeaderText="Fahrgestellnummer" 
                                                                UniqueName="column">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="KENNZ" 
                                                                FilterControlAltText="Filter column1 column" HeaderText="Kennzeichen" 
                                                                UniqueName="column1">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="INVENTAR" 
                                                                FilterControlAltText="Filter column2 column" HeaderText="Inventarnummer" 
                                                                UniqueName="column2">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Autovermieter" 
                                                                FilterControlAltText="Filter column3 column" HeaderText="Autovermieter" 
                                                                UniqueName="column3">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="HCORT" 
                                                                FilterControlAltText="Filter column4 column" HeaderText="Hereinnahmecenter" 
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="HCEINGDAT" DataFormatString="{0:d}" 
                                                                FilterControlAltText="Filter column5 column" HeaderText="Eingangsdatum" 
                                                                UniqueName="column5">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        <EditFormSettings>
                                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                            </EditColumn>
                                                        </EditFormSettings>
                                                    </MasterTableView>
                                                    <FilterMenu EnableImageSprites="False">
                                                        <WebServiceSettings>
                                                            <ODataSettings InitialContainerName="">
                                                            </ODataSettings>
                                                        </WebServiceSettings>
                                                    </FilterMenu>
                                                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                        <WebServiceSettings>
                                                            <ODataSettings InitialContainerName="">
                                                            </ODataSettings>
                                                        </WebServiceSettings>
                                                    </HeaderContextMenu>
                                                </telerik:RadGrid>
                            </div> 
                            <div id="dataFooter">
                                &nbsp;<asp:HiddenField ID="hField" runat="server" Value="0" />
                            </div>
                            
                        </ContentTemplate>
                         <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                    </asp:UpdatePanel>
                    
                </div>
                
                
                
            </div>
           
        </div>
       
    </div>
  
</asp:Content>