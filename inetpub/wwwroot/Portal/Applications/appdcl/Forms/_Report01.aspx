<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="_Report01.aspx.vb" Inherits="AppDCL._Report01"
    EnableEventValidation="false" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register Assembly="CuteWebUI.AjaxUploader" Namespace="CuteWebUI" TagPrefix="cc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style1 {
            height: 22px;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
    <table width="100%" align="center">
        <tr>
            <td height="25">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2" height="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Abfragekriterien)</asp:Label><asp:HyperLink
                                ID="lnkKreditlimit" runat="server" CssClass="PageNavigation" NavigateUrl="Equipment.aspx"
                                Visible="False">Abfragekriterien</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="StandardTableButtonFrame" valign="top">
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;<asp:Label ID="lblDatensatz" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="lblNoData" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LabelExtraLarge">
                                        <table id="Table9" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td valign="top" align="left" colspan="2">
                                                    <table id="Table10" cellspacing="0" cellpadding="0" width="100%" bgcolor="white"
                                                        border="0" runat="server">
                                                        <tbody>
                                                            <tr>
                                                                <td valign="top" align="left" width="100%" bgcolor="#ffffff">
                                                                    <table class="TableBackGround" id="Table4" bordercolor="#cccccc" cellspacing="1"
                                                                        cellpadding="1" width="100%" bgcolor="#ffffff" border="0">
                                                                        <tr>
                                                                            <td class="style1" id="td0" valign="middle" nowrap align="center" bgcolor="#ffffff"
                                                                                rowspan="1" runat="server">
                                                                               
                                                                                   <div style="float: left; width: 45px;">
                                                                                    <p >
                                                                                        <strong>                                                                                   
                                                                                   Hilfe</strong>
                                                                                   <asp:ImageButton ID="ibtnHelp" runat="server" ToolTip="Hilfe"
                                                                                     ImageUrl="../../../Images/fragezeichen.gif" style="height: 13px;" 
                                                                                     Height="13px" /></p></div>
                                                                                    <p align="center">
                                                                                        <strong>Auftragsliste (Fahrer:
                                                                                            <asp:Label ID="lblFahrernr" runat="server"></asp:Label>)</strong></p>

                                                                            </td>
                                                                        
                                                                            <td colspan="2" style="padding-left:15px" bgcolor="#ffffff">
                                                                                <strong>Auftrag:&nbsp;
                                                                                    <asp:Label ID="lblInfo1" runat="server"></asp:Label></strong>
                                                                            </td>
                                                                                                  
                                                                        </tr>
                                                                        <tr>
                                                                            <td id="td02" valign="top" align="center" bgcolor="#ffffff" colspan="1" rowspan="1"
                                                                                runat="server">
                                                                                <asp:ListBox ID="lbxAuftrag" runat="server" Width="350px" BackColor="White" 
                                                                                    Height="380px" AutoPostBack="True" >
                                                                                </asp:ListBox>
                                                                            </td>
                                                                            <td valign="top" id="td1" align="center" bgcolor="#ffffff" runat="server">
                                                                                <cc1:UploadAttachments CancelText="Abbrechen" runat="server" InsertText="Durchsuchen.."
                                                                                    MultipleFilesUpload="true" CancelAllMsg="Alle Uploads abbrechen" CancelUploadMsg="Upload Abbrechen"
                                                                                    MaxFilesLimitMsg="Die maximale Anzahl({0}) an Dateien wurde überschritten." UploadingMsg="Dateien werden hochgeladen.."
                                                                                    FileTooLargeMsg="{0}kann nicht hochgeladen werden! Maximale Dateigröße ({1}) überschritten . Maximale Dateigröße: {2}."
                                                                                    FlashLoadMode="True" MaxFilesLimit="100" ID="Uploader1" RemoveButtonText="Entfernen"
                                                                                    ShowCheckBoxes="False" ShowRemoveButtons="true" AutoUseSystemTempFolder="False"
                                                                                    ProgressPanelWidth="220" RemoveButtonBehavior="Delete" ShowProgressInfo="False"
                                                                                    ShowTableHeader="False" TempDirectory="~/temp/pictures" UploadProcessingMsg="Dateien werden hochgeladen.."
                                                                                    UploadType="Auto" OnAttachmentAdded="UploaderAttachmentAdded" OnAttachmentRemoveClicked="UploaderAttachmentRemove" OnUploadCompleted="UploaderCompleted">
                                                                                    <ValidateOption AllowedFileExtensions="jpeg, jpg, gif" MaxSizeKB="300" />
                                                                                    <InsertButtonStyle Width="120px" />
                                                                                </cc1:UploadAttachments>
                                                                            </td>
                                                                            <td valign="top" width="100%" bgcolor="#ffffff">
                                                                                <div style="height:380px; overflow-y:auto;">
                                                                                <asp:DataGrid ID="gridServer" runat="server" Width="100%" BackColor="White" BorderColor="Transparent"
                                                                                    CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader"
                                                                                    AllowSorting="True" AutoGenerateColumns="False">
                                                                                    <SelectedItemStyle BackColor="#FFE0C0"></SelectedItemStyle>
                                                                                    <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                                                    <HeaderStyle HorizontalAlign="Left" CssClass="GridTableHead" VerticalAlign="Top">
                                                                                    </HeaderStyle>
                                                                                    <Columns>
                                                                                        <asp:BoundColumn Visible="False" DataField="Serverpfad" SortExpression="Serverpfad"
                                                                                            HeaderText="Serverpfad"></asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="Zeit" SortExpression="Zeit" HeaderText="Erstellt am">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:TemplateColumn Visible="False" SortExpression="Filename" HeaderText="Dateiname">
                                                                                            <ItemTemplate>
                                                                                                <asp:HyperLink ID="lnkFile" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") &amp; DataBinder.Eval(Container, "DataItem.Filename") %>'
                                                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Filename") %>' Target="_blank">
                                                                                                </asp:HyperLink>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:TemplateColumn HeaderText="Vorschau (75x75)">
                                                                                            <ItemTemplate>
                                                                                                <table id="Table13" cellspacing="1" cellpadding="1" border="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Image ID="Image1" runat="server" Width="75px" Height="75px" BorderColor="Black"
                                                                                                                ImageUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") &amp; DataBinder.Eval(Container, "DataItem.Filename") %>'
                                                                                                                BorderStyle="Solid" BorderWidth="1px" ToolTip='<%# DataBinder.Eval(Container, "DataItem.Filename") %>'>
                                                                                                            </asp:Image>



                                                                                                        </td>
                                                                                                        <td valign="top">
                                                                                                            <asp:HyperLink ID="Hyperlink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Serverpfad") &amp; DataBinder.Eval(Container, "DataItem.Filename") %>'
                                                                                                                ImageUrl="/Portal/Images/lupe.gif" Target="_blank">In Originalgröße anzeigen</asp:HyperLink>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="Filename" SortExpression="Filename" HeaderText="Dateiname">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="Status" HeaderText="Status">
                                                                                            <ItemStyle Font-Size="XX-Small"></ItemStyle>
                                                                                        </asp:BoundColumn>
                                                                                        <asp:TemplateColumn HeaderText="l&#246;schen">
                                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="ibtnSRDelete" runat="server" CommandName="Delete" CausesValidation="false"
                                                                                                    ImageUrl="/Portal/Images/loesch.gif"></asp:ImageButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                    </Columns>
                                                                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                                                                </asp:DataGrid>
                                                                                </div>
                                                                                <div id="divHelp" runat="server" style="border: solid 1px #0094FF;padding: 5px;" visible="false">
                                                                               <div style="float:left;"> <strong>Arbeitsschritte für den Bilderupload:</strong></div>
                                                                                <div style="float:right;width:25%;text-align: right;">
                                                                                    <asp:LinkButton ID="lbtnClose" runat="server" 
                                                                                        Text="X" ToolTip="schliessen"  Style=" color: #0094FF;font-weight: bold; text-decoration: none;" />
                                                                                </div>
                                                                                <br /><br />
                                                                                1. Markieren Sie einen Auftrag
                                                                                <br /><br />
                                                                                2. Klicken Sie anschließen auf "Durchsuchen"  um die Dateien auszuwählen
                                                                                <br /> <br />
                                                                                3. Wählen Sie die Dateien aus Ihrem Verzeichnis aus
                                                                                <br /><br />
                                                                                4. Klicken Sie auf den  Button "Dateien hochladen"
                                                                                <br /><br />        
                                                                                5. Prüfen Sie anhand der Bilder ob diese zum Auftrag gehören<br />
                                                                                &nbsp;&nbsp;&nbsp;(Falls Sie die falschen Bilder geladen haben löschen Sie diese über den Button<br />
                                                                                &nbsp;&nbsp;&nbsp;"alle Dateien entfernen" )
                                                                                <br /><br />
                                                                                6. Klicken Sie auf den Button  "Fertig/Beenden"<br /><br />
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td id="td3" valign="top" nowrap bgcolor="#ffffff" runat="server">
                                                                               <%--<asp:LinkButton ID="btnShowPics" runat="server" CssClass="StandardButtonTable" 
                                                                                    Width="220px" Visible="False">&#149;&nbsp;Hochgeladene Bilder anzeigen</asp:LinkButton>--%>
                                                                            </td>
                                                                            <td id="td4" valign="middle" align="left" bgcolor="#ffffff" runat="server">
                                                                                <%--<asp:LinkButton ID="btnUpload" runat="server" CssClass="StandardButtonTable" 
                                                                                    Width="220px" Visible="False" OnClick="UploadClick">&#149;&nbsp;Datei(en) hochladen</asp:LinkButton>--%>
                                                                            </td>
                                                                            <td valign="top" align="right" width="100%" bgcolor="#ffffff">
                                                                                <asp:LinkButton ID="lbDeleteAll" runat="server" OnClick="lbDeleteAll_Click" CssClass="StandardButtonTable" 
                                                                                    Width="160px" Visible="False">&#149;&nbsp;alle Datei(en) entfernen</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>