<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report015.aspx.vb" Inherits="CKG.Components.ComCommon.Report015" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI"  TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
    <meta content="JavaScript" name="vs_defaultClientScript"/>
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        #File1
        {
            width: 436px;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server" enctype="multipart/form-data">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>
        <table width="100%" align="center">
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%;">
                        <div class="PageNavigation" style="float: none">
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                            <asp:LinkButton ID="lbtnEditDocTypes" runat="server" Text="Dokumentarten pflegen" style="float: right; text-decoration: underline" ></asp:LinkButton>
                        </div>
                        <div id="TableQuery">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                        </div>
                        <div id="Result" runat="Server" style="width: 100%">
                            <div style="display: none">
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                            </div>
                            <div id="data" runat="server">
                                <div>
                                    <telerik:RadGrid ID="rgDokumente" runat="server" PageSize="100" AllowPaging="true" AllowSorting="true" 
                                        AutoGenerateColumns="False" GridLines="None" CssClass="tableMain">
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                        <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="false">
                                            <SortExpressions>
                                                <telerik:GridSortExpression FieldName="DocTypeName" SortOrder="Ascending" />
                                                <telerik:GridSortExpression FieldName="LastEdited" SortOrder="Descending" />
                                            </SortExpressions>
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldName="DocTypeId"></telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="DocTypeId"></telerik:GridGroupByField>
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <HeaderStyle CssClass="GridTableHead"/>
                                            <Columns>
                                                <telerik:GridBoundColumn SortExpression="DocumentId" DataField="DocumentId" visible="false" />
                                                <telerik:GridTemplateColumn SortExpression="FileName" HeaderText="Dokument" HeaderButtonType="TextButton">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lbtPDF" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../../images/pdf.gif" 
                                                            ToolTip="PDF" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString() = "pdf" %>' />
                                                        <asp:ImageButton ID="lbtExcel" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../../Images/excel.gif" 
                                                            ToolTip="Excel" ImageAlign="Middle" Visible='<%# Eval("FileType").StartsWith("xls") %>' />
                                                        <asp:ImageButton ID="lbtWord" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../../Images/Word_Logo.jpg"
                                                            ToolTip="Word" ImageAlign="Middle" Visible='<%# Eval("FileType").StartsWith("doc") %>' />
                                                        <asp:ImageButton ID="lbtJepg" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../../Images/jpg-file.png"
                                                            ToolTip="JPG" ImageAlign="Middle" Visible='<%# Eval("FileType").StartsWith("jp") %>' />
                                                        <asp:ImageButton ID="lbtGif" runat="server" CommandName="showDocument" Height="18px" ImageUrl="../../Images/Gif_Logo.gif"
                                                            ToolTip="GIF" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString() = "gif" %>' />                                         
                                                        <span>
                                                            <asp:LinkButton ID="lbtDateiOeffnen" runat="server" CommandName="showDocument" Text='<%# Eval("FileName") %>' >
                                                            </asp:LinkButton>
                                                        </span>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn SortExpression="FileType" DataField="FileType" visible="false" />
                                                <telerik:GridBoundColumn SortExpression="DocTypeId" DataField="docTypeId" Visible="false" />
                                                <telerik:GridBoundColumn SortExpression="DocTypeName" HeaderText="Art" AllowSorting="false" 
                                                    DataField="DocTypeName" HeaderStyle-Width="200px" />
                                                <telerik:GridButtonColumn UniqueName="colBearbeiten" HeaderStyle-Width="50px" ButtonType="ImageButton" ImageUrl="../../Images/EditTableHS.png" Text="bearbeiten" CommandName="editDocument" >
                                                </telerik:GridButtonColumn>
                                                <telerik:GridBoundColumn SortExpression="LastEdited" HeaderText="Letzte Änderung" HeaderButtonType="TextButton" 
                                                    DataField="LastEdited" DataFormatString="{0:dd.MM.yyyy HH:mm:ss}" HeaderStyle-Width="150px">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="colLoeschen" HeaderStyle-Width="100px">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="ckb_SelectAll" runat="server" ToolTip="Auf dieser Seite alle Dokumente löschen setzen."
                                                            AutoPostBack="True" OnCheckedChanged="ckb_SelectAll_CheckedChanged" />
                                                        <asp:Label ID="Label1" runat="server" Text="Löschen"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="rb_sel" runat="server" GroupName="Auswahl" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                            <div style="float: right">
                                <asp:Panel ID="PanelUpload" runat="server">
                                    <table>
                                        <tr style="height: 25px">
                                            <td>
                                                <asp:FileUpload ID="upFile" runat="server"/>
                                            </td>
                                            <td width="100" align="center">
                                                <asp:LinkButton ID="lbtnUpload" runat="server" CssClass="StandardButton" 
                                                    style="vertical-align: baseline; height: 16px" >» hochladen</asp:LinkButton>
                                            </td>
                                            <td width="100" align="center">
                                                <asp:LinkButton ID="lbtnLoeschen" runat="server" CssClass="StandardButton"  
                                                    style="vertical-align: baseline; height: 16px" >» Löschen</asp:LinkButton>
                                                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Wollen Sie die markierten Dateien wirklich löschen?"
                                                    OnClientCancel="cancelClick" TargetControlID="lbtnLoeschen" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </div>
                        <div id="divEditDocument" runat="server" visible="false" style="width: 100%">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4">
                                        <div style="color: #ffffff; font-weight: bold; height: 22px; line-height: 22px; width: 100%; background-color: #576B96">
                                            <asp:Label ID="lbl_EditDocumentHeader" runat="server" Text="Dokument bearbeiten" style="margin-left: 15px" ></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trDocumentRights" runat="server">
                                    <td style="width: 90px">
                                        Dokument:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFileName" runat="server" ReadOnly="true" Width="300px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 90px">
                                        Dokumentart:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDokumentart" runat="server" Width="300px" />
                                    </td>
                                    <td style="width: 90px; text-align: right">
                                        Gruppen:&nbsp;
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lbxDocumentGroups" runat="server" Width="300px" Rows="8" SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panelDocumentEditButtons" runat="server" style="margin-top: 10px; float: right">
                                <table>
                                    <tr>
                                        <td width="100" align="center">
                                            <asp:LinkButton class="StandardButton" ID="lbtnSaveDocument" runat="server" Text="Speichern" style="height: 16px" ></asp:LinkButton>
                                        </td>
                                        <td width="100" align="center">
                                            <asp:LinkButton class="StandardButton" ID="lbtnCancelEditDocument" runat="server" Text="Verwerfen" style="height: 16px" ></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div>
                                &nbsp;
                            </div>
                            <input type="hidden" id="ihSelectedDocumentId" runat="server" />
                        </div>
                        <div id="divEditDocTypes" runat="server" visible="false" style="width: 100%">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4">
                                        <div style="color: #ffffff; font-weight: bold; height: 22px; line-height: 22px; width: 100%; background-color: #576B96">
                                            <asp:Label ID="lbl_EditDocTypesHeader" runat="server" Text="Dokumentarten pflegen" style="margin-left: 15px" ></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trEditDocType" runat="server">
                                    <td style="width: 90px">
                                        Dokumentart:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDocTypeSelection" runat="server" Width="300px" AutoPostBack="true" />
                                    </td>
                                    <td style="width: 90px; text-align: right">
                                        Bezeichnung:&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDocTypeName" runat="server" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trNewDocType" runat="server" visible="false">
                                    <td style="width: 90px">
                                        Bezeichnung:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewDocType" runat="server" Width="300px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panelDocTypeEditButtons" runat="server" style="margin-top: 10px; float: right">
                                <table>
                                    <tr>
                                        <td id="tdAddNewDocType" runat="server" width="100" align="center">
                                            <asp:LinkButton class="StandardButton" ID="lbtnAddNewDocType" runat="server" Text="Neue Dok.art anlegen" style="height: 28px" ></asp:LinkButton>
                                        </td>
                                        <td id="tdSaveNewDocType" runat="server" width="100" align="center" Visible="false">
                                            <asp:LinkButton class="StandardButton" ID="lbtnSaveNewDocType" runat="server" Text="Neue Dok.art speichern" style="height: 28px" ></asp:LinkButton>
                                        </td>
                                        <td width="100" align="center" valign="middle">
                                            <asp:LinkButton class="StandardButton" ID="lbtnDeleteDocType" runat="server" Text="Löschen" style="height: 28px" ></asp:LinkButton>
                                            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" ConfirmText="Wollen Sie die Dokumentenart wirklich löschen?"
                                                OnClientCancel="cancelClick" TargetControlID="lbtnDeleteDocType" />
                                        </td>
                                        <td width="100" align="center" valign="middle">
                                            <asp:LinkButton class="StandardButton" ID="lbtnSaveDocType" runat="server" Text="Speichern" style="height: 28px" ></asp:LinkButton>
                                        </td>
                                        <td width="100" align="center">
                                            <asp:LinkButton class="StandardButton" ID="lbtnCancelEditDocType" runat="server" Text="Verwerfen / Zurück" style="height: 28px" ></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div>
                                &nbsp;
                            </div>
                        </div>
                    </div>
                    <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                    
                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnFake"
                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="false">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="mb" runat="server" BackColor="White" Width="280" Height="160"
                        Style="display: none; border: solid 2px #C0C0C0">
                        <div style="padding-top: 10px; padding-bottom: 10px; text-align: center">
                            <asp:Label ID="lblFileUploadDialog" runat="server" Font-Bold="True">Die hochzuladende Datei existiert bereits.<br />Wie wollen Sie vorgehen?</asp:Label>
                        </div>
                        <div style="margin-left: 5px">
                            <asp:RadioButtonList ID="rblPopupOptions" runat="server">
                                <asp:ListItem Value="Beibehalten" Text="die vorhandene Datei beibehalten und die neue Datei unter anderem Namen speichern" Selected="True"/>
                                <asp:ListItem Value="Ersetzen" Text="die vorhandene Datei ersetzen" />
                            </asp:RadioButtonList>
                        </div>
                        <div>
                            &nbsp;
                        </div>
                        <div>
                            <table align="center">
                                <tr>
                                    <td width="100" align="center">
                                        <asp:LinkButton class="StandardButton" ID="lbtnPopupOK" style="height: 16px" runat="server" Text="OK" />
                                    </td>
                                    <td width="100" align="center">
                                        <asp:LinkButton class="StandardButton" ID="lbtnPopupCancel" style="height: 16px" runat="server" Text="Abbrechen" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
