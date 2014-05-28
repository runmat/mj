<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Dokumentencenter.aspx.cs"
  Inherits="CKG.Components.Zulassung.Dokumentencenter" MaintainScrollPositionOnPostback="true"
  MasterPageFile="~/MasterPage/Services.Master" %>

<%@ Register Assembly="CKG.Components.Controls" Namespace="CKG.Components.Controls"
  TagPrefix="custom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div id="site" class="new_layout">
    <div id="content">
      <div id="navigationSubmenu">
      </div>
      <div id="innerContent">
        <div id="innerContentRight" style="width: 100%">
          <div class="DivVersandTabContainer">
            <a class="ZulassungDokumentencenterEnabled" href="#"></a>
          </div>
          <div class="VersandTabPanel">
            <table cellpadding="0" cellspacing="0">
              <tr>
                <td style="padding-bottom: 0px;" class="PanelHead" colspan="3">
                  <asp:Label ID="lblDocuments" runat="server" Text="Bitte wählen Sie die benötigten Dokumente aus" />
                </td>
                <td rowspan="2" style="padding: 0; width: 380px;">
                  <asp:UpdatePanel ID="upValidation" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                      <asp:ValidationSummary ID="valSummary" runat="server" ShowSummary="true" DisplayMode="List"
                        CssClass="ValidationSummary" />
                    </ContentTemplate>
                  </asp:UpdatePanel>
                </td>
              </tr>
              <tr>
                <td style="padding-top: 0px;" colspan="3">
                  <asp:Label ID="lblInfo" runat="server" Text=" Je Land werden die für eine Zulassung notwendigen Dokumente aufgeführt." />
                </td>
              </tr>
              <tr valign="top">
                <td>
                  <asp:Label ID="lbl_Land" AssociatedControlID="ddlLand" runat="server">lbl_Land</asp:Label>
                </td>
                <td>
                  <asp:DropDownList ID="ddlLand" runat="server" CssClass="middle" AppendDataBoundItems="true">
                    <asp:ListItem>Auswahl</asp:ListItem>
                  </asp:DropDownList>
                  <asp:CompareValidator ID="valLand" runat="server" Display="None" SetFocusOnError="true"
                    ErrorMessage="Bitte wählen Sie ein Land." ControlToValidate="ddlLand" EnableClientScript="false"
                    ValueToCompare="Auswahl" Operator="NotEqual" />
                </td>
                <td style="vertical-align: top; text-align: right;">
                  <asp:LinkButton ID="lbSearch" runat="server" CssClass="greyButton search" Text="Suchen"
                    OnClick="OnSearch" />
                </td>
                <td>
                </td>
              </tr>
              <tr>
                <td colspan="4" style="padding-left: 7px; padding-right: 5px;">
                  <asp:UpdatePanel ID="upMain" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <ContentTemplate>
                      <custom:DocumentList ID="dlResult" runat="server" ondocumentlistcommand="OnDocumentListCommand" onlayoutcreated="OnLayoutCreated">
                        <LayoutTemplate>
                      <div class="StandardHeadDetail selected" style="height: 28px; width: 895px;">
                        <div>
                          Zulassungsdokumente für <%# Container.SelectedCountry%> auswählen.
                        </div>
                      </div>
                      <table cellpadding="0" cellspacing="0" style="width: 50%;">
                      <tr>
                      <th style="padding: 5px 0px 5px 15px; text-align: left;">
                      Dokumententyp
                      </th>
                      <th style="padding: 5px 0px 5px 15px; text-align: left;">
                      <asp:ImageButton ID="ibSelectAll" runat="server" CommandName="SelectAll" CommandArgument="Select" ImageUrl="~/Images/Zulassung/icon_checkbox_active.gif" AlternateText="Alle auswählen" />
                      <asp:ImageButton ID="ibDeselectAll" runat="server" CommandName="SelectAll" CommandArgument="Deselect" ImageUrl="~/Images/Zulassung/icon_checkbox_inactive.gif" AlternateText="Auswahl aufheben" />&nbsp;Dokument
                      </th>
                      <th style="padding: 5px 0px 5px 15px; text-align: left;">
                      Gültig bis
                      </th>
                      </tr>
                      <tr id="groupPlaceholder" runat="server"></tr>
                      <tr>
                      <td colspan="3">
                      <asp:LinkButton ID="lbSave" runat="server" CssClass="greyButton save" style="float: right;" Text="Herunterladen" CommandName="Save" />
                      </td>
                      </tr>
                      </table>
                      </LayoutTemplate>
                      <GroupTemplate>
                      <tr>
                      <td rowspan="<%# Container.GroupCount %>" valign="top">
                        <asp:Label ID="lblGroupName" runat="server" Text="<%# Container.GroupName %>" />
                      </td>
                      <td id="itemPlaceholder" runat="server"></td>
                      </tr>
                      </GroupTemplate>
                      <ItemTemplate>
                      <td>
                      <asp:CheckBox ID="cbTest" runat="server" />
                      <asp:HiddenField ID="hfTest" runat="server" Value='<%# Eval("FileName") %>' />
                      <a target="_blank" href='<%# VirtualPathUtility.ToAbsolute("~/Components/Zulassung/Dokumente/Download/" + (string)Eval("FileName")) %>'>
                      <asp:Label ID="lblTest" runat="server" Text='<%# Eval("Identifier") %>' /></a>
                      </td>
                      <td><asp:Label ID="lblTest2" runat="server" Text='<%# Eval("ValidUntil", "{0:d}") %>' /></td>
                      </ItemTemplate>
                      <ItemSeparatorTemplate>
                      </tr><tr>
                      </ItemSeparatorTemplate>
                      <EmptyDataTemplate>
                      <div class="StandardHeadDetail selected" style="height: 28px; width: 895px;">
                        <div>
                        <asp:Image ID="iFehler" runat="server" ImageUrl="~/Images/Zulassung/error_icon.png" AlternateText="Fehler" Width="16px" Height="16px" />
                          Keine Zulassungsdokumente für <%# Container.SelectedCountry%> hinterlegt.
                        </div>
                      </div>
                      </EmptyDataTemplate>
                      </custom:DocumentList>
                    </ContentTemplate>
                  </asp:UpdatePanel>
                </td>
              </tr>
            </table>
            <div id="dataFooter">
              &nbsp;
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
