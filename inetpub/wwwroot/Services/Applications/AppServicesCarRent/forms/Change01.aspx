<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="AppServicesCarRent.Change01"
    MasterPageFile="../MasterPage/App.Master" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>


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
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <asp:UpdatePanel id="UP1" runat="server">
                    <ContentTemplate>
                    <div id="TableQuery" style="margin-bottom: 10px">
                    
                    <table id="tblDezentral" runat="server" visible="true">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblDezError" runat="server" ForeColor="Red" 
                                    EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr id="trZulassungsart" runat="server" visible="false">
                            <td class="firstLeft active" style="width:120px">
                                Zulassungsart:
                            </td>
                            <td class="active">
                                <span>
                                <asp:RadioButtonList ID="rblAuswahl" runat="server" 
                                    RepeatDirection="Horizontal" AutoPostBack="true" Width="180px">
                                    <asp:ListItem Value="1">Zentral</asp:ListItem>
                                    <asp:ListItem Value="2">Dezentral</asp:ListItem>
                                </asp:RadioButtonList>
                                </span>
                            </td>
                            
                            
                        </tr>
                        <tr id="trZulassungskreis" runat="server" visible="false">
                            <td class="firstLeft active" style="width:120px">
                                Zulassungskreis:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlZulKreis" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    
                    </table>
                    
                    
                    
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" visible="false">
                            <tbody>
                                <tr class="formquery">
                                    <td nowrap="nowrap" class="firstLeft active" colspan="2">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label></td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="width:250px;" >
                                        <asp:Panel ID="Panel2" runat="server" Height="200px"  >
                                            <div style="float: left; cursor: pointer; vertical-align: middle;">
                                                <div style="vertical-align: middle;">
                                                    <asp:ListBox Height="175px" AutoPostBack="True" ID="lstPDI" runat="server" style="width:250px;">
                                                        <asp:ListItem>TEST</asp:ListItem>
                                                        <asp:ListItem>TEST</asp:ListItem>
                                                        <asp:ListItem>TEST</asp:ListItem>
                                                        <asp:ListItem>TEST</asp:ListItem>
                                                    </asp:ListBox>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td class="firstLeft active" style="width:615px;">
                                        <asp:Panel ID="Panel1" runat="server" Height="200px">
                                            <div style="vertical-align: middle; padding-right:10px">
                                                <div style="vertical-align: middle;">
                                                    <asp:ListBox Height="175px" AutoPostBack="True" ID="lstMOD" runat="server" Width="100%">
                                                        <asp:ListItem>TEST</asp:ListItem>
                                                        <asp:ListItem>TEST</asp:ListItem>
                                                        <asp:ListItem>TEST</asp:ListItem>
                                                        <asp:ListItem>TEST</asp:ListItem>
                                                    </asp:ListBox>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td nowrap="nowrap" class="firstLeft active" colspan="2">
                                    &nbsp;
                                    </td>
                                </tr>                    
           
                            </tbody>
                        </table>
                              </div>
                    <div id="Result" runat="Server" visible="false">

                        <div id="pagination">
                        <div id="Div1" class="ZulDateDiv">
                        <table id="ZulFields" class="tblZulDateDiv" cellspacing="0" cellpadding="0" border="0" runat="server">
                            <tr>
                                <td>
                                    
                                     <span><strong>Versicherer:</strong></span>
                                </td>
                                <td colspan="2">
                                    <span><strong>Zulassungsdatum:</strong></span>
                                </td>
                            </tr>
                            <tr>

                                <td >
                                    <span>
                                    <asp:DropDownList ID="ddlVersicherer" runat="server" Width="264px" AutoPostBack="True"
                                        Enabled="False">
                                    </asp:DropDownList></span>
                                </td>
                                <td >
                               <span ><asp:TextBox ID="txtZulDate" width="105px" runat="server"></asp:TextBox></span>
                                    <ajaxToolkit:CalendarExtender ID="txtZulDate_CE" runat="server" Enabled="True"
                                        TargetControlID="txtZulDate" Animated="False" PopupPosition="BottomRight">
                                    </ajaxToolkit:CalendarExtender>
       
                                </td>
                                <td style="width:100%">
                                    <asp:LinkButton ID="lbtSelect" runat="server" CssClass="Tablebutton" Width="78px">» Zuweisen </asp:LinkButton>
                                    <asp:LinkButton ID="lbtSelectAll" runat="server" CssClass="TablebuttonLarge" Width="130px" Height="16px">» Alle ausw&auml;hlen </asp:LinkButton>
                                </td>
                            </tr>
                                <tr class="formquery">
                                    <td nowrap="nowrap" class="firstLeft active" colspan="2">
                                    &nbsp;
                                    </td>
                                </tr>                               
                        </table></div>                        
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvFahrzeuge" Width="100%" runat="server" AutoGenerateColumns="False"
                                            CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                            AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RowId") %>'
                                                            ID="lblRowId" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Art") %>'
                                                            ID="lblArt" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DADPDI") %>'
                                                            ID="lblDADPDI" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHANDELSNAME") %>'
                                                            ID="lblHandelsname" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKRAFTSTOFF_TXT") %>'
                                                            ID="lblKraftstoff" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false" ItemStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZAKTSPERRE") %>'
                                                            ID="lblAktSperre" Visible="true"> </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle ForeColor="Black" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderStyle-ForeColor="#000000" DataField="ZZREFERENZ1" SortExpression="ZZREFERENZ1" HeaderText="Unitnr.">
                                                    <HeaderStyle ForeColor="Black" />
                                                </asp:BoundField>                                                
                                                <asp:BoundField HeaderStyle-ForeColor="#000000" DataField="Eingangsdatum"  SortExpression="Eingangsdatum" HeaderText="Eingangsdatum"
                                                    DataFormatString="{0:dd.MM.yyyy}">
                                                    <HeaderStyle ForeColor="Black" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderStyle-ForeColor="#000000" DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnr." ItemStyle-Width="130px">
                                                    <HeaderStyle ForeColor="Black" Width="130px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderStyle-ForeColor="#000000" DataField="ZZKRAFTSTOFF_TXT" SortExpression="ZZKRAFTSTOFF_TXT" HeaderText="Antrieb">
                                                    <HeaderStyle ForeColor="Black" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Zul.Datum">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="calControl" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SelectedDate") %>' Width="75px"></asp:TextBox>
                                                                                                            </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField Visible="False" DataField="ZZAKTSPERRE" SortExpression="ZZAKTSPERRE"
                                                    HeaderText="Gesperrt">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Auswahl">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbxAuswahl" runat="server" 
                                                            Checked='<%# DataBinder.Eval(Container, "DataItem.SelectedEinzel") %>' 
                                                            AutoPostBack="True" oncheckedchanged="cbxAuswahl_CheckedChanged"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField Visible="False" DataField="Status" SortExpression="Status">
                                                    <ItemStyle Font-Size="XX-Small" ForeColor="Red"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Menge">
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMenge" runat="server" Width="30px" MaxLength="3" Text='<%# DataBinder.Eval(Container, "DataItem.SelectedAlle") %>'></asp:TextBox>
                                                         <asp:ImageButton ID="ImageButton1" runat="server"
                                                        CommandName="Copy" ImageUrl="../../../Images/Confirm_mini.gif" Height="16px"
                                                        Width="16px" />
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
                      

                    <div id="dataFooter">
                        <asp:LinkButton ID="btnConfirm" runat="server" CssClass="Tablebutton" 
                            Width="78px">» Weiter </asp:LinkButton>
                     </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
