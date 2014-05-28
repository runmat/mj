<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Menu.ascx.vb" Inherits="CKG.PortalZLD.Menu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">

    function qsfMenuLoad(sender) {
        $telerik.$('.rmTemplateLink, .rmLink, .rmTLink');
    }

</script>
<style type="text/css">
.MyImage { position:relative;z-index: 200; }
    
</style>
<telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
<asp:Panel runat="server" ID="Panel1" CssClass="menu-container">
    <telerik:RadMenu runat="server" ID="RadMenu1" OnClientLoad="qsfMenuLoad"
        EnableSelection="false" EnableRoundedCorners="true" EnableShadows="true" 
        CssClass="RadMenu_MetroKroschke" EnableEmbeddedSkins="False" >
        <Items>
            <telerik:RadMenuItem runat="server" ImageUrl="/PortalZLD/Images/CKG_Start.png" 
             HoveredImageUrl="/PortalZLD/Images/CKG_Start.Lightpng.png" BorderStyle="None" ToolTip="Aufgaben"
              GroupSettings-OffsetX="25" GroupSettings-OffsetY="-75" CssClass="MyImage" >
                <GroupSettings Width="400px" />
                <Items>
                    <telerik:RadMenuItem ImageUrl = "/PortalZLD/Images/Navi.png" > </telerik:RadMenuItem>
                </Items>
            </telerik:RadMenuItem>
        </Items>
    </telerik:RadMenu>
</asp:Panel>