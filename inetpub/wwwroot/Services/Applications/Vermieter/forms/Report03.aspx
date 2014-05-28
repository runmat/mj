 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report03.aspx.cs" Inherits="Vermieter.forms.Report03" MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




<div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" onclick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    
                     <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtDatumVon" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtDatumBis" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtBereitDatumVon" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtBereitDatumBis" EventName="TextChanged" />
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                        <ContentTemplate>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        <asp:Label ID="lblSelection" runat="server" Text="Selektion ausblenden..."></asp:Label>
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" 
                                                ImageUrl="/services/Images/queryArrow.gif" onclick="NewSearch_Click1" />
                                            
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <asp:Panel ID="pnlSelection" runat="server">
                        <div id="TableQuery" style="margin-bottom: 10px">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="2">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" 
                                                EnableViewState="False"></asp:Label>
                                            <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False" 
                                                EnableViewState="False"></asp:Label>
                                        </td>
                                       
                                    </tr>
                                   
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width:150px">
                                            <asp:Label ID="lbl_Carport" runat="server">lbl_Carport</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:DropDownList ID="ddlCarports" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="ddlCarports_SelectedIndexChanged" Width="350px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Fahrzeugnummer" runat="server">lbl_Fahrzeugnummer</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtFahrzeugnummer" runat="server" CssClass="TextBoxNormal" 
                                                MaxLength="17" Width="200px"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnSetFahrzeugnummer" runat="server" Height="16px" 
                                                ImageUrl="/Services/Images/haken_gruen.gif" onclick="ibtnSetFahrzeugnummer_Click" 
                                                ToolTip="Fahrzeugnummer setzen" Width="16px" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_EingangsdatumVon" runat="server">lbl_EingangsdatumVon</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtDatumVon" runat="server"  AutoPostBack="true"
                                        Width="200px" ontextchanged="txtDatum_TextChanged"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="extDatumVon" runat="server" TargetControlID="txtDatumVon" WatermarkText="Auswahl nur über Kalender." WatermarkCssClass="Watermarked"></cc1:TextBoxWatermarkExtender>
                                         <cc1:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                            Animated="false" Enabled="True" TargetControlID="txtDatumVon" PopupButtonID="btnCal1"></cc1:CalendarExtender>
                                            <asp:ImageButton ID="btnCal1" runat="server" 
                                                ImageUrl="/Services/Images/calendar.jpg" onclick="btnCal1_Click" />
                                            <asp:ImageButton ID="ibtnDelEingVon" runat="server" Height="16px" 
                                                ImageUrl="/Services/Images/del.png" ToolTip="Datum entfernen!" 
                                                Width="16px" style="padding-left:5px" onclick="ibtnDelEingVon_Click" />
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="height: 22px">
                                            <asp:Label ID="lbl_EingangsdatumBis" runat="server">lbl_EingangsdatumBis</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="height: 22px">
                                            <asp:TextBox ID="txtDatumBis" runat="server" 
                                        Width="200px" ontextchanged="txtDatum_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtDatumBis" WatermarkText="Auswahl nur über Kalender." WatermarkCssClass="Watermarked"></cc1:TextBoxWatermarkExtender>
                                        <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                            Animated="false" Enabled="True" TargetControlID="txtDatumBis" PopupButtonID="btnCal2"></cc1:CalendarExtender>
                                            <asp:ImageButton ID="btnCal2" runat="server" 
                                                ImageUrl="/Services/Images/calendar.jpg" onclick="btnCal1_Click" />
                                            <asp:ImageButton ID="ibtnDelEingBis" runat="server" Height="16px" 
                                                ImageUrl="/Services/Images/del.png" ToolTip="Datum entfernen!" 
                                                Width="16px" style="padding-left:5px" onclick="ibtnDelEingBis_Click"/>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_BereitmeldungVon" runat="server">lbl_BereitmeldungVon</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtBereitDatumVon" runat="server" CssClass="TextBoxNormal" 
                                                Width="200px" ontextchanged="txtDatum_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtBereitDatumVon" WatermarkText="Auswahl nur über Kalender." WatermarkCssClass="Watermarked"></cc1:TextBoxWatermarkExtender>
                                            <cc1:CalendarExtender ID="txtBereitDatumVon_CalendarExtender" runat="server" 
                                                Animated="false" Enabled="True" Format="dd.MM.yyyy" PopupPosition="BottomLeft" 
                                                TargetControlID="txtBereitDatumVon" PopupButtonID="btnCal3">
                                            </cc1:CalendarExtender>
                                            <asp:ImageButton ID="btnCal3" runat="server" 
                                                ImageUrl="/Services/Images/calendar.jpg" onclick="btnCal1_Click"/>
                                            <asp:ImageButton ID="ibtnDelBereitVon" runat="server" Height="16px" 
                                                ImageUrl="/Services/Images/del.png" ToolTip="Datum entfernen!" 
                                                Width="16px" style="padding-left:5px" onclick="ibtnDelBereitVon_Click"/>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_BereitmeldungBis" runat="server">lbl_BereitmeldungBis</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtBereitDatumBis" runat="server" CssClass="TextBoxNormal" 
                                                Width="200px" ontextchanged="txtDatum_TextChanged"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtBereitDatumBis" WatermarkText="Auswahl nur über Kalender." WatermarkCssClass="Watermarked"></cc1:TextBoxWatermarkExtender>
                                            <cc1:CalendarExtender ID="txtBereitDatumBis_CalendarExtender" runat="server" 
                                                Animated="false" Enabled="True" Format="dd.MM.yyyy" PopupPosition="BottomLeft" 
                                                TargetControlID="txtBereitDatumBis" PopupButtonID="btnCal4">
                                            </cc1:CalendarExtender>
                                            <asp:ImageButton ID="btnCal4" runat="server" 
                                                ImageUrl="/Services/Images/calendar.jpg" onclick="btnCal1_Click"/>
                                            <asp:ImageButton ID="ibtnDelBereitBis" runat="server" Height="16px" 
                                                ImageUrl="/Services/Images/del.png" ToolTip="Datum entfernen!" 
                                                Width="16px" style="padding-left:5px" onclick="ibtnDelBereitBis_Click"/>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Fahrzeug" runat="server">lbl_Fahrzeug</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <span>
                                            <asp:RadioButtonList ID="rblFahrzeug" runat="server" 
                                                RepeatDirection="Horizontal" style="width:250px;" AutoPostBack="True" 
                                                onselectedindexchanged="rblFahrzeug_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="0" style="white-space:nowrap;margin:0 0 0 0;">Alle</asp:ListItem>
                                                <asp:ListItem Value="1" style="white-space:nowrap;margin:0 0 0 0;">mit ZBII</asp:ListItem>
                                                <asp:ListItem Value="2" style="white-space:nowrap;margin:0 0 0 0;">ohne ZBII</asp:ListItem>
                                            </asp:RadioButtonList>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_FzgeZulBereit" runat="server">lbl_FzgeZulBereit</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                           <span>
                                            <asp:CheckBox ID="chkZulBereit" runat="server" AutoPostBack="True" oncheckedchanged="chkZulBereit_CheckedChanged" 
                                                 />
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_FzgeGesperrt" runat="server">lbl_FzgeGesperrt</asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <span>
                                            <asp:CheckBox ID="chkGesperrt" runat="server" AutoPostBack="True" 
                                                oncheckedchanged="chkGesperrt_CheckedChanged" />
                                            </span>
                                        </td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                            <%--<div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>--%>
                        </div>
                    </asp:Panel>
                   <%-- <div id="dataQueryFooter">
                        
                    </div>--%>
                    
                    <div style="height:10px">&nbsp;</div>
                    
                    <div style="background-color: #dfdfdf;width:100%;float:left;height:22px;color:#595959;font-weight:bold">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active" style="padding-left:15px;white-space:nowrap">
                                        <asp:Label ID="lblUebersicht" runat="server" Text="Übersicht ausblenden..."></asp:Label>
                                    </td>
                                    <td align="right" style="width:100%" >
                                        <div style="text-align: right;padding-right:17px;padding-top:3px">
                                            <asp:ImageButton ID="ibtUebersicht" runat="server" 
                                                ImageUrl="/services/Images/queryArrow.gif" onclick="ibtUebersicht_Click" />
                                            
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    
                    <div style="height:10px">&nbsp;</div>
                   
                    <div id="Result" runat="Server" visible="false">
                        <%--<div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                
                            </div>
                        </div>--%>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="gvCarport"
                                CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" 
                                AllowSorting="True" onsorting="gvCarport_Sorting">
                                <PagerSettings Visible="False" />
                                <HeaderStyle CssClass="GridTableHead" ForeColor="White"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                <RowStyle CssClass="ItemStyle" />
                                <Columns>
                                    <asp:TemplateField SortExpression="Carport" HeaderText="col_CarportNummer">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_CarportNummer" runat="server" CommandName="Sort" CommandArgument="Carport" ForeColor="White">col_CarportNummer</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCarportNummer" Text='<%# DataBinder.Eval(Container, "DataItem.Carport") %>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  
                                
                                    <asp:TemplateField SortExpression="CarportName" HeaderText="col_CarportName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_CarportName" runat="server" CommandName="Sort" CommandArgument="CarportName" ForeColor="White">col_CarportName</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCarportName" Text='<%# DataBinder.Eval(Container, "DataItem.CarportName") %>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField SortExpression="AnzFahrzeuge" HeaderText="col_AnzFahrzeuge">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="col_AnzFahrzeuge" runat="server" CommandName="Sort" CommandArgument="AnzFahrzeuge" ForeColor="White">col_AnzFahrzeuge</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAnzFahrzeuge" Text='<%# DataBinder.Eval(Container, "DataItem.AnzFahrzeuge") %>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <%--<div id="dataFooter">
                        &nbsp;
                        </div>--%>
                            <div style="background-color: #dfdfdf;width:100%;float:left;height:22px;color:#595959;font-weight:bold">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active" style="padding-left:15px;white-space:nowrap">
                                                <asp:Label ID="lblDetail" runat="server" Text="Details öffnen..."></asp:Label>
                                            </td>
                                            <td align="right" style="width:100%" >
                                                <div style="vertical-align:middle; text-align: right;padding-right:17px;padding-top:3px">
                                                    <asp:ImageButton ID="ibtDetail" runat="server" ImageUrl="/services/Images/queryArrow.gif"
                                                        OnClick="ibtDetail_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                    <div id="ResultDetail" runat="server" visible="false">
                    
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="/services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel1" runat="server" OnClick="lnkCreateExcel1_Click" ForeColor="#dfdfdf">Excel herunterladen</asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        
                       <div class="paginationGrid">
                                <uc2:GridNavigation ID="GridNavigation2" runat="server">
                                </uc2:GridNavigation>
                            </div>   
                        
                        
                         <div class="dataGrid">
                                    <table cellspacing="0" style="width:100%" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                            CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True" onsorting="GridView1_Sorting" > 
                                            <PagerSettings Visible="False" />
                                            <HeaderStyle CssClass="GridTableHead" Font-Underline="true"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" ForeColor="#595959" />
                                            <RowStyle CssClass="ItemStyle" ForeColor="#595959" />
                                            <Columns>
                                                
                                                <asp:TemplateField SortExpression="CarportNameDetail" HeaderText="col_CarportNameDetail">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_CarportNameDetail" runat="server" CommandName="Sort" CommandArgument="CarportNameDetail" ForeColor="White" Font-Underline="true">col_CarportNameDetail</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCarportNameDetail" Text='<%# DataBinder.Eval(Container, "DataItem.CarportNameDetail") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderStyle Width ="150px"/>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer" ForeColor="White" Font-Underline="true">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="Fahrzeugnr" HeaderText="col_Fahrzeugnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrzeugnummer" runat="server" CommandName="Sort" CommandArgument="Fahrzeugnr" ForeColor="White" Font-Underline="true">col_Fahrzeugnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFahrzeugnummerDetail" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrzeugnr") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="EingangsdatumDetail" HeaderText="col_EingangsdatumDetail">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_EingangsdatumDetail" runat="server" CommandName="Sort" CommandArgument="EingangsdatumDetail" ForeColor="White" Font-Underline="true">col_EingangsdatumDetail</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEingangsdatumDetail" Text='<%# DataBinder.Eval(Container, "DataItem.EingangsdatumDetail", "{0:d}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                <asp:TemplateField SortExpression="BereitdatumDetail" HeaderText="col_BereitdatumDetail">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_BereitdatumDetail" runat="server" CommandName="Sort" CommandArgument="BereitdatumDetail" ForeColor="White" Font-Underline="true">col_BereitdatumDetail</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBereitdatumDetail" Text='<%# DataBinder.Eval(Container, "DataItem.BereitdatumDetail", "{0:d}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField SortExpression="Briefnummer" HeaderText="col_Briefnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Briefnummer" runat="server" CommandName="Sort" CommandArgument="Briefnummer" ForeColor="White" Font-Underline="true">col_Briefnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBriefnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Briefnummer") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                
                                                
                                                <asp:TemplateField SortExpression="Gesperrt" HeaderText="col_Gesperrt">
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Gesperrt" runat="server" CommandName="Sort" CommandArgument="Gesperrt" ForeColor="White" Font-Underline="true">col_Gesperrt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkGesperrtDetail" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Gesperrt").ToString()=="X" %>'
                                                            Enabled="False" />
                                                    </ItemTemplate>
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
                </div>
                
                
                
            </div>
        </div>
    </div>
   
</asp:Content>