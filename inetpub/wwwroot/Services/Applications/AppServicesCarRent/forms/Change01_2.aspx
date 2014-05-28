<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_2.aspx.vb" Inherits="AppServicesCarRent.Change01_2"  MaintainScrollPositionOnPostback="true"   MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" runat="server" class="firstLeft active"
                    Text="Fahrzeugauswahl"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>

                    <div id="TableQuery" style="margin-bottom: 10px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                <tr class="formquery">
                                    <td nowrap="nowrap" class="firstLeft active" >
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>&nbsp;</td>
                                </tr>
                                <tr class="formquery">
                                    <td nowrap="nowrap" class="firstLeft active" >
                                    <asp:Label ID="lblMessage" runat="server" ></asp:Label>&nbsp;</td>
                                </tr>  
                                <tr class="formquery">
                                    <td nowrap="nowrap" class="firstLeft active" >
                                    &nbsp;</td>
                                </tr>                                                               
                        </table>
                    </div>    
                    <div id="Result" runat="Server">
                        <div class="ExcelDiv" id="ExcelDiv" runat="server" visible="false"> 
                            <div align="right" class="rightPadding">
                                <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel1" runat="server">Excel herunterladen</asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                        </div>
                        <div id="data">
                            <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvFahrzeuge" Width="100%" runat="server" AutoGenerateColumns="False"
                                            CellPadding="1" CellSpacing="1" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                            AllowSorting="True" AllowPaging="True" CssClass="GridView" PageSize="20">
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
                                                    <HeaderStyle ForeColor="Black" Width="55px" />
                                                </asp:BoundField>                                                
                                                <asp:BoundField HeaderStyle-ForeColor="#000000" DataField="Eingangsdatum"  SortExpression="Eingangsdatum" HeaderText="Eingangsdatum"
                                                    DataFormatString="{0:dd.MM.yyyy}">
                                                    <HeaderStyle ForeColor="Black" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderStyle-ForeColor="#000000" DataField="ZZFAHRG" SortExpression="ZZFAHRG" HeaderText="Fahrgestellnr.">
                                                    <HeaderStyle ForeColor="Black" Width="123px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderStyle-ForeColor="#000000" DataField="ZZHERST_TEXT" SortExpression="ZZHERST_TEXT" HeaderText="Hersteller" />
                                                <asp:BoundField HeaderStyle-ForeColor="#000000" DataField="ZZHANDELSNAME" SortExpression="ZZHANDELSNAME" HeaderText="Typ" />
                                                <asp:BoundField HeaderStyle-ForeColor="#000000" DataField="ZZKRAFTSTOFF_TXT" SortExpression="ZZKRAFTSTOFF_TXT" HeaderText="Antrieb">
                                                    <HeaderStyle ForeColor="Black" Width="50px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Zul.Datum">
                                                <HeaderStyle width="80px"/>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblZulDat" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SelectedDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField Visible="False" DataField="ZZAKTSPERRE" SortExpression="ZZAKTSPERRE"
                                                    HeaderText="Gesperrt">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundField>
                                                 <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>                        
                     
                    <div id="dataFooter">
                        <asp:LinkButton ID="btnConfirm" runat="server" CssClass="Tablebutton" Width="78px">» Absenden </asp:LinkButton>
                     </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
