<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AuftraegeOnline.aspx.vb"
    Inherits="KBS.AuftraegeOnline" MasterPageFile="~/KBS.Master" %>
    
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Aufträge Online"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" Text="" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                    </div>
                    <div id="TableQuery" runat="server" style="border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">
                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                    </div>
                    <div>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </div>
                    <div id="Auftragsliste" runat="server" style="text-align: center; border-bottom: solid 1px #DFDFDF;
                        border-right: solid 1px #DFDFDF; border-left: solid 1px #DFDFDF;">   
                        <telerik:RadGrid ID="rgGrid1" runat="server" AllowSorting="True" AllowPaging="False"
                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default">
                            <ClientSettings AllowKeyboardNavigation="true" >
                                <Scrolling ScrollHeight="400px" AllowScroll="True" UseStaticHeaders="True" />
                            </ClientSettings>
                            <ItemStyle CssClass="ItemStyle" />
                            <AlternatingItemStyle CssClass="ItemStyle" />
                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="False" DataKeyNames="PRAEG_ID">
                                <HeaderStyle ForeColor="#595959" />
                                <ItemStyle HorizontalAlign="Left" />
                                <AlternatingItemStyle HorizontalAlign="Left" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Auswahl">
                                        <HeaderStyle Width="55px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkAuswahl" Visible='<%# Eval("POSNR").ToString() = "10" %>' Checked='<%# Eval("Auswahl") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="PRAEG_ID" SortExpression="PRAEG_ID" Visible="False" >
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="BSTNK" SortExpression="BSTNK" HeaderText="Bestellnummer" >
                                        <HeaderStyle Width="100px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="POSNR" SortExpression="POSNR" HeaderText="Position" >
                                        <HeaderStyle Width="60px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ERDAT" SortExpression="ERDAT" HeaderText="Bestelldatum" DataFormatString="{0:dd.MM.yyyy}" >
                                        <HeaderStyle Width="95px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ERZEIT" SortExpression="ERZEIT" HeaderText="Bestelluhrzeit" >
                                        <HeaderStyle Width="95px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MATNR" SortExpression="MATNR" HeaderText="Artikel" >
                                        <HeaderStyle Width="65px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MAKTX" SortExpression="MAKTX" HeaderText="Artikelbezeichnung" >
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MENGE" SortExpression="MENGE" HeaderText="Menge" >
                                        <HeaderStyle Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ZZKENN" SortExpression="ZZKENN" HeaderText="Kennzeichen" >
                                        <HeaderStyle Width="90px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderStyle Width="30px" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" Width="16" Height="16" ImageUrl="~/Images/iconPDF.gif"
                                                CommandName="showDocument" ToolTip="PDF herunterladen" Visible='<%# Eval("HasDocuments") AndAlso Eval("POSNR").ToString() = "10" %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                    <div id="dataFooter">
                        <asp:LinkButton ID="lbAlleDokumente" Text="alle Dokumente abrufen" Height="16px" Width="155px" runat="server"
                            CssClass="TablebuttonXLarge"></asp:LinkButton>
                        <asp:LinkButton ID="lbAuswahlAlle" Text="alle auswählen" Height="16px" Width="128px" runat="server"
                            CssClass="TablebuttonLarge"></asp:LinkButton>
                        <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                            CssClass="Tablebutton"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
