<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep1Part3.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep1Part3" %>
<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls" TagPrefix="custom" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<div runat="server" id="Container">

         <div id="uploadPanel">
            <div class="rightAlignedNav">
                <strong><asp:Label runat="server" ID="lbl_Datei">lbl_Datei</asp:Label>: </strong><asp:FileUpload runat="server" ID="fileUpload1" />
                <a href='<%= Page.ResolveClientUrl("~\\Components\\Zulassung\\Dokumente\\Zulassung.xls") %>' target="_blank"><img src="~/Images/Zulassung/excel_icon.png" runat="server" border="0" /></a>
                <img id="labelWait" style="display: none;" src="~/Images/Zulassung/loading2.gif" alt="Bitte warten..." runat="server" />
            </div>
         </div>
         <div id="searchPanel" style="display: none;">
            <div class="rightAlignedNav">
                <div id="filePath"></div>
                <asp:LinkButton runat="server" CssClass="greyButton search" ID="buttonSearch" OnClick="buttonSearch_Click" OnClientClick="$('#uploadPanel').attr('style', 'display:inline'); $('#searchPanel').attr('style', 'display:none');" Text="Suchen" /> 
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="greyButton" Text="Neuer Upload" OnClientClick="$('#uploadPanel').attr('style', 'display:inline'); $('#searchPanel').attr('style', 'display:none'); return false;" />
            </div>
         </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label runat="server" ID="labelError" Visible="false" Text="Es ist ein Fehler aufgetreten." CssClass="errorLabel"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="buttonSearch" />
            </Triggers>
        </asp:UpdatePanel>


    <custom:ModalOverlay runat="server" id="SearchOverlay" ParentContainer="Container">
        <Triggers>
            <custom:ModalOverlayTrigger ControlID="buttonSearch" />
        </Triggers>
        <ContentTemplate>
            <div style="background-color: #fff; width: 300px; padding: 15px; text-align: center; border: 3px solid #335393;">
                <img id="Img1" src="~/Images/Zulassung/loading.gif" align="middle" style="border-width:0px;" runat="server" />
                <br /><label style="font-size:14px;font-weight:bold;">Bitte warten...</label>
                <br /><label style="font-size:10px;font-weight:bold;">Ihr Suchanfrage wird bearbeitet.</label>
            </div>
        </ContentTemplate>
    </custom:ModalOverlay>
</div>