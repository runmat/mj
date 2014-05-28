<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Filialziele.aspx.vb" Inherits="KBS.Filialziele"
    MasterPageFile="../KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/Helper.js"></script>
    <style type="text/css">
        .Green
        {
            color: green;
            font-weight: bold;
        }
        
        .Red
        {
            color: red;
            font-weight: bold;
        }
        
        .ShortHeader
        {
            padding-left: 5px !important;
            padding-right: 3px !important;
        }
        
        
        .DetailTable
        {
            border-collapse: collapse;
        }
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zur&uuml;ck"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Filialziele"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="TableQuery" style="margin-bottom: 0px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label runat="server">Auswertung für den:</asp:Label>
                                        <asp:TextBox ID="txtDatum" runat="server" Width="80px" 
                                            Style="margin-left: 5px;" AutoPostBack="True"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CetxtDatum" runat="server" TargetControlID="txtDatum" Animated="False">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MeetxtDatum" runat="server" TargetControlID="txtDatum"
                                            MaskType="Date" Mask="99/99/9999" CultureName="de-DE">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="Label2" runat="server">monatl. Rahmenquote:</asp:Label>
                                        <asp:TextBox ID="txtRahmenquote" runat="server" Width="80px" 
                                            Style="margin-left: 5px; text-align: right" ReadOnly="True"></asp:TextBox>%
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2" width="100%">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Style="color: Green;"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td style="white-space: nowrap;">
                                        <asp:LinkButton ID="lbFilialauswertung" runat="server" CssClass="TabButtonBig"
                                            Width="136px" Style="margin-left: 5px;">Filialauswertung</asp:LinkButton>
                                        <asp:LinkButton ID="lbFilialvergleich" runat="server" CssClass="TabButtonBig Active" Width="136px">Filialvergleich</asp:LinkButton>
                                    </td>
                                    <td class="firstLeft active" width="100%">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="dataHeader" style="background-image: url(../Images/overflow.png); color: #ffffff;
                        font-weight: bold; float: left; height: 22px; line-height: 22px; width: 892px;
                        white-space: nowrap; background-color: #2B4C91; padding-left: 15px;">
                        Filiale
                        <asp:Label ID="lblFilialname" runat="server"></asp:Label>
                        / KST.
                        <asp:Label ID="lblKostenstelle" runat="server"></asp:Label>
                        / KW
                        <asp:Label ID="lblKW" runat="server"></asp:Label>
                        / LFB-Gebiet
                        <asp:Label ID="lblLFB" runat="server"></asp:Label>
                        / Datenstand vom: 
                        <asp:Label ID="lblDatenstand" runat="server" Style="text-align: right;"></asp:Label>
                    </div>
                    <div id="data">
                        <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True" Style="padding-left: 15px;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvFilialauswertung" runat="server" CssClass="GridView" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="False" AllowSorting="True" ShowFooter="False"
                                        GridLines="Vertical" Style="border-collapse: collapse ! important;">
                                        <PagerSettings Visible="false" />
                                        <HeaderStyle CssClass="GridTableHead" BorderColor="#595959"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <Columns>
                                            <asp:BoundField Visible="false" DataField="MATNR_PRODH" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblMatGrp" runat="server">Materialgruppe</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMatRow" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.MAKTX")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblVortag" runat="server">Vortag</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVortagRow" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.VORTAG_IST")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Woche
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWocheRow" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.WOCHE_IST")  %>'></asp:Label>(
                                                    <asp:Label runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.WOCHE_ABW")  %>'
                                                        CssClass='<% # DataBinder.Eval(Container, "DataItem.WocheDiffColor") %>'></asp:Label>)
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Monat
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonatRow" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.MONAT_IST")  %>'></asp:Label>(
                                                    <asp:Label runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.MONAT_ABW")  %>'
                                                        CssClass='<% # DataBinder.Eval(Container, "DataItem.MonatDiffColor") %>'></asp:Label>)
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-CssClass="ShortHeader">
                                                <HeaderTemplate>
                                                    Woche / Plan
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWochePlanRow" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.WOCHE_PLAN")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-CssClass="ShortHeader">
                                                <HeaderTemplate>
                                                    Monat / Plan
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonatPlanRow" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.MONAT_PLAN")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-CssClass="ShortHeader">
                                                <HeaderTemplate>
                                                    aktuell
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblaktuellRow" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.JAHR_IST")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-CssClass="ShortHeader" Visible="false" >
                                                <HeaderTemplate>
                                                    Jahr / Plan
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblJahrPlanRow" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.JAHR_PLAN")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:ImageField HeaderText="Zielerreichung / Woche" DataImageUrlField="AmpelUrl"
                                                HeaderStyle-CssClass="ShortHeader" ItemStyle-CssClass="ShortHeader" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ControlStyle-Width="16px"
                                                ControlStyle-Height="16px" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gvFilialvergleich" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                        ShowFooter="False" GridLines="Vertical" Visible="false" Style="border-collapse: collapse ! important;">
                                        <PagerSettings Visible="false" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundField HeaderText="Rang" DataField="RANG" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="KST" DataField="VKBUR" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Ort" DataField="KTEXT" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField Visible="false" DataField="MATNR_PRODH" />
                                            <asp:BoundField HeaderText="Art./ Mat." DataField="MAKTX" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Wrap="false" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Woche
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.WOCHE_IST")  %>'></asp:Label>(
                                                    <asp:Label runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.WOCHE_ABW")  %>'
                                                        CssClass='<% # DataBinder.Eval(Container, "DataItem.WocheDiffColor") %>'></asp:Label>)
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Monat
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonatRow" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.MONAT_IST")  %>'></asp:Label>(
                                                    <asp:Label ID="Label1" runat="server" Text='<% # DataBinder.Eval(Container, "DataItem.MONAT_ABW")  %>'
                                                        CssClass='<% # DataBinder.Eval(Container, "DataItem.MonatDiffColor") %>'></asp:Label>)
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Woche / Plan" DataField="WOCHE_PLAN" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ShortHeader" />
                                            <asp:BoundField HeaderText="Monat / Plan" DataField="MONAT_PLAN" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ShortHeader" />
                                            <asp:BoundField HeaderText="letzte Woche" DataField="VWOCHE_IST" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ShortHeader" />
                                            <asp:BoundField HeaderText="letzter Monat" DataField="VMONAT_IST" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ShortHeader" />
                                            <asp:BoundField HeaderText="Jahr" DataField="JAHR_IST" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ShortHeader" />
                                            <asp:BoundField HeaderText="Jahr / Plan" DataField="JAHR_PLAN" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ShortHeader" Visible="false" />
                                            <asp:BoundField HeaderText="Rahmenquote" DataField="RAHMENQUOTE" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ShortHeader" />
                                        </Columns>
                                    </asp:GridView>
                                    <div>
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dataFooter" class="dataQueryFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
