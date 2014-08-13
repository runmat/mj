<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogSchwellwert.aspx.vb" Inherits="Admin.LogSchwellwert"
    MasterPageFile="MasterPage/Admin.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Auswertung Schwellwertüberschreitung"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />                            
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr  class="formquery" >
                                        <td class="firstLeft active" colspan="2"  style="width:100%">
                                            <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" 
                                                Visible="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblDateVon" runat="server" Text="ab Datum:"></asp:Label>
                                        </td>
                                        <td class="active" style="width:100%">
                                            <asp:TextBox ID="txtAbDatum" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtAbDatum_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtAbDatum" >
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblDateBis" runat="server" Text="bis Datum:"></asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtBisDatum" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtBisDatum_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtBisDatum">                  
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                     <tr class="formquery">
                                         <td class="firstLeft active" colspan="2"  style="width:100%">
                                             &nbsp;</td>
                                    </tr>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                                <div id="dataQueryFooter" >
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Erstellen</asp:LinkButton>
                                </div>
                            </div>
                            <div>&nbsp;</div>                                
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <asp:GridView ID="grvLogSchwellwert" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        AllowPaging="True" Visible="true" CssClass="GridView" CellPadding="0" GridLines="None"
                                        AllowSorting="true" PageSize="10">
                                        <HeaderStyle CssClass="GridTableHead" Wrap="false"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <PagerSettings Visible="False" NextPageText="&amp;gt;&amp;gt;&amp;gt;" PreviousPageText="&amp;lt;&amp;lt;&amp;lt;" />
                                        <PagerStyle HorizontalAlign="Left" />
                                        <RowStyle Wrap="false" CssClass="ItemStyle" />
                                        <EditRowStyle Wrap="False"></EditRowStyle>
                                        <Columns>
                                            <asp:BoundField DataField="Seite" HeaderText="ASPX-Seite" />
                                            <asp:BoundField DataField="Datum" HeaderText="Datum" DataFormatString="{0:dd.MM.yyyy}" />
                                            <asp:BoundField DataField="GesamtAnz" HeaderText="Gesamtanzahl">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UeberschreitungAnz" HeaderText="Anzahl Überschreitung">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AktuellerSchwellwert" HeaderText="Aktueller Schwellwert">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Detail">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtDetail" runat="server" CommandName="Detail" Width="16px" Height="16px" ImageUrl="Images/lupe_01.gif"
                                                        CommandArgument='<%# Container.DataItemIndex %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    
                                    <div>
                                        <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" CellPadding="0" BackColor="White"
                                            AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" CssClass="GridView">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <itemstyle Wrap="false" CssClass="ItemStyle" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="Testbenutzer" HeaderText="Testbenutzer">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Testbenutzer") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="BAPI" SortExpression="BAPI" HeaderText="BAPI"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Parameter">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Start" SortExpression="Start" HeaderText="Start"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Ende" SortExpression="Ende" HeaderText="Ende"></asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="Dauer" HeaderText="Dauer">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer"))) %>'
                                                            Height="10px" BackColor="#8080FF">
                                                        </asp:Label>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Erfolg" HeaderText="Erfolg">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Erfolg") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Fehlermeldung" SortExpression="Fehlermeldung" HeaderText="Fehlermeldung">
                                                </asp:BoundColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="&amp;gt;&amp;gt;&amp;gt;" PrevPageText="&amp;lt;&amp;lt;&amp;lt;"
                                                Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </div>
                                    <div id="divDetails" runat="Server" visible="false">                                        
                                        <DBWC:HierarGrid ID="HGZ" runat="server" Width="100%" CellPadding="0" AllowPaging="false"
                                            AutoGenerateColumns="False" BackColor="White" CssClass="GridView" GridLines="None"
                                            TemplateDataMode="Table" LoadControlMode="UserControl" TemplateCachingBase="Tablename"
                                            BorderWidth="1px" AllowSorting="True">
                                            <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                            <ItemStyle CssClass="ItemStyle"></ItemStyle>
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="Seite" SortExpression="Seite" HeaderText="Seite"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Anwendung" SortExpression="Anwendung" HeaderText="Anwendung">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Zugriffe SAP" SortExpression="Zugriffe SAP" HeaderText="Zugriffe&lt;br&gt;SAP">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Benutzer" SortExpression="Benutzer" HeaderText="Benutzer">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Kunnr" SortExpression="Kunnr" HeaderText="Kundennr.">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Customername" SortExpression="Customername" HeaderText="Kundenname">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AccountingArea" SortExpression="AccountingArea" HeaderText="Accounting-&lt;br&gt;Area">
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn SortExpression="Testbenutzer" HeaderText="Test-&lt;br&gt;benutzer">
                                                    <HeaderStyle></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Checkbox4" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container, "DataItem.Testbenutzer") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Start ASPX" HeaderText="Start ASPX">
                                                    <HeaderStyle></HeaderStyle>
                                                    <ItemTemplate>
                                                       <asp:Label ID="lblStartASPX" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Start ASPX") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn SortExpression="Dauer ASPX" HeaderText="Dauer">
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <ItemStyle CssClass="ItemStyle"></ItemStyle>
                                                    <ItemTemplate>
                                                        <table id="Table18" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td style="padding: 0;">
                                                                    ASPX
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer ASPX") %>'> </asp:Label>&nbsp;
                                                                </td>
                                                                <td style="padding: 0;">
                                                                    <asp:Label ID="Label5" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer ASPX"))) %>'
                                                                        Height="10px" BackColor="#8080FF"> </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding: 0;">
                                                                    SAP
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Dauer SAP") %>'> </asp:Label>&nbsp;
                                                                </td>
                                                                <td style="padding: 0;">
                                                                    <asp:Label ID="Label7" runat="server" Width='<%# System.Web.UI.WebControls.Unit.Pixel(CInt(DataBinder.Eval(Container, "DataItem.Dauer SAP"))) %>'
                                                                        Height="10px" BackColor="Highlight"> </asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </DBWC:HierarGrid>
                                    </div>
                                </div>
                            </div>
                            <div id="Error">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
