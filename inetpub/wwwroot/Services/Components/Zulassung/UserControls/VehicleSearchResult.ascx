<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VehicleSearchResult.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.VehicleSearchResult" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="~/PageElements/GridNavigation.ascx" %>

    <style type="text/css">
        
    .GridTableHead2
    {
        /*background-image: url(../Images/overflowThead.png);*/
	    background-color: #9b9b9b;
	    height:23px;
	    padding-left=15px !importand;
	    color:#FFFFFF;
	    text-align:left;  
	
    }
    </style>


<div id="Result" runat="Server">
    <div id="data">
        <asp:GridView AutoGenerateColumns="False" Width="100%" BackColor="White" 
            runat="server" ID="GridView1"
            CssClass="GridView" GridLines="None" PageSize="3" AllowPaging="True" 
            AllowSorting="True" EnableModelValidation="True">
            <PagerSettings Visible="False" />
            <HeaderStyle CssClass="GridTableHead2"></HeaderStyle>
            <AlternatingRowStyle CssClass="GridTableAlternate" />
            <RowStyle CssClass="ItemStyle" />
            <EditRowStyle></EditRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="Equipment" SortExpression="Equnr" Visible="False">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("EQUNR") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblEqunr" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="MANDT" HeaderText="col_Auswahl" HeaderStyle-Width="40px">
                    <HeaderTemplate>
                        <asp:ImageButton ID="ibtAuswahl" runat="server" Height="12px" 
                            ImageUrl="~/images/haken_gruen24x24.gif" onclick="ibtAuswahl_Click" 
                            Width="12px" ToolTip="Alle Fahrzeuge auswählen" style="padding-left:4px" />
                        <asp:ImageButton ID="ibtnAbwahl" runat="server" Height="12px" 
                            ImageUrl="~/images/del.png" onclick="ibtnAbwahl_Click" 
                            Width="12px" ToolTip="Alle Fahrzeuge abwählen" style="padding-left:4px" />                                                            
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk0000" Checked='<%# DataBinder.Eval(Container, "DataItem.AUSWAHL") == "99" %>' runat="server" OnCheckedChanged="CheckBox_Check" AutoPostBack="true" Visible='<%# DataBinder.Eval(Container, "DataItem.Bem") == string.Empty %>' ></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer" >
                    <HeaderStyle Width="125px" />
                    <ItemStyle Width="125px" />
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="TIDNR" HeaderText="col_NummerZB2">
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_NummerZB2" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_NummerZB2</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="ZZHERST_TEXT" HeaderText="col_Hersteller">
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="ZZHERST_TEXT">col_Hersteller</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHERST_TEXT") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="ZZKLARTEXT_TYP" HeaderText="col_Typ">
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="ZZKLARTEXT_TYP">col_Typ</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKLARTEXT_TYP") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>          
                <asp:TemplateField SortExpression="ZZHANDELSNAME" HeaderText="col_Handelsname">
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Handelsname" runat="server" CommandName="Sort" CommandArgument="ZZHANDELSNAME">col_Handelsname</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHANDELSNAME") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>   
                <asp:TemplateField SortExpression="LIZNR" HeaderText="col_Vertragsnummer">
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Vertragsnummer</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                                                                                                                                           
                <asp:TemplateField SortExpression="Bem" HeaderText="col_Status">
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Status" runat="server" CommandName="Sort" CommandArgument="Bem">col_Status</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Bem") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="ZFAHRZEUGART" HeaderText="col_Fahrzeugart">
                    <HeaderTemplate>
                        <asp:LinkButton ID="col_Fahrzeugart" runat="server" CommandName="Sort" CommandArgument="ZFAHRZEUGART">col_Fahrzeugart</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZFAHRZEUGART") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Label ID="Label8" runat="server" ForeColor="Red">Es konnte kein Fahrzeug gefunden werden.</asp:Label>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <div id="pagination">
        <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
    </div>
</div>