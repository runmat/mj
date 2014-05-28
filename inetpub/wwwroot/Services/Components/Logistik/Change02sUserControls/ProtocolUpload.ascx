<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProtocolUpload.ascx.vb"
    Inherits="CKG.Components.Logistik.ProtocolUpload" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<table style="width: 100%; margin-top: 20px;" cellspacing="0" cellpadding="0">
    <tr>
        <td style="vertical-align: top; padding: 0 0 4px 0;" colspan="2">
            Protokolle:
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top; padding: 0;">
            <asp:GridView ID="uploadsGrid" runat="server" AutoGenerateColumns="false" GridLines="Horizontal"
                BorderColor="#AFAFAF" BorderStyle="Solid" OnRowCommand="UploadsRowCommand" Width="100%"
                BackColor="#ffffff" ShowHeader="false">
                <Columns>
                    <asp:BoundField DataField="Protokollart" HeaderText="Dokument" />
                    <asp:TemplateField HeaderText="Datei">
                        <ItemTemplate>
                            <telerik:RadAsyncUpload runat="server" ID="upload" AllowedFileExtensions=".pdf" MaxFileInputsCount="1"
                                MultipleFileSelection="Disabled" OnClientFileUploaded="onFileUploaded" OnFileUploaded="UploadedComplete"
                                Visible='<%# DataBinder.Eval(Container, "DataItem.Tempfilename")= "" %>' DisablePlugins="true"
                                Localization-Select="Hochladen" Localization-Remove="Löschen" Localization-Cancel="Abbrechen"
                                Width="100%" />
                            <asp:Label runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Tempfilename")<> "" %>'
                                Text='<%# System.IO.Path.GetFilename(DataBinder.Eval(Container, "DataItem.Tempfilename"))  %>' />
                            <asp:ImageButton runat="server" CommandName="DeleteProtokoll" CommandArgument='<%# Container.DataItemIndex %>'
                                ImageUrl="/Services/Images/del.png" Visible='<%# DataBinder.Eval(Container, "DataItem.Tempfilename")<> "" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Label ID="uploadMessage" Font-Bold="true" runat="server" />
            <asp:Button ID="submitFiles" runat="server" Text="Übernehmen" Style="display: none;" />
        </td>
        <td>
            <div class="infopanel">
                <label>
                    Tipp!</label>
                <div>
                    Die Protokolle müssen im PDF-Format hochgeladen werden.</div>
            </div>
        </td>
    </tr>
</table>
