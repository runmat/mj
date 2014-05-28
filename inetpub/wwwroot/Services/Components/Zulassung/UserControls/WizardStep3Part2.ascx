<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardStep3Part2.ascx.cs" Inherits="CKG.Components.Zulassung.UserControls.WizardStep3Part2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="~/PageElements/GridNavigation.ascx" %>

<div id="Result" runat="Server">
    <div id="data" style="float: none;">
        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
            CssClass="GridView" GridLines="None" PageSize="4" AllowPaging="True" AllowSorting="True" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating">
            <PagerSettings Visible="False" />
            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
            <AlternatingRowStyle CssClass="GridTableAlternate" />
            <RowStyle CssClass="ItemStyle" />
            <EditRowStyle CssClass="EditItemStyle" />
            <EditRowStyle></EditRowStyle>
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="Nr." ReadOnly="true" />
                <asp:TemplateField Visible="false">
                    <ItemTemplate><asp:Label ID="lblFahrt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrt")  %>'></asp:Label></ItemTemplate>
                    <EditItemTemplate><asp:Label ID="lblFahrt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrt")  %>'></asp:Label></EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ZZPROTOKOLLART" HeaderText="Dokument" ReadOnly="true" />
                <asp:TemplateField HeaderText="Datei">
                    <EditItemTemplate><cc1:AsyncFileUpload runat="server" ID="AsyncFileUpload1" Width="320px" UploaderStyle="Traditional" CompleteBackColor="#99FF99"
                            OnClientUploadComplete="showUploadStartMessage" ThrobberID="Throbber" Enabled="true" />
                        <asp:Label ID="Throbber" runat="server" Style="display: none">&nbsp;<img  id="imgWait"  alt="Bitte warten.." src="/Services/Images/indicator.gif"/>&nbsp;Bitte warten... </asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate><asp:Label ID="AsyncFileUpload1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Filename")  %>'></asp:Label></ItemTemplate>
                    <HeaderStyle Width="370px" />
                    <ItemStyle Width="370px" />
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ButtonType="Image" DeleteImageUrl="~/Images/Zulassung/delete_icon.png" EditImageUrl="~/Images/Zulassung/edit_icon.png" CancelImageUrl="~/Images/Zulassung/cancel_icon.png" UpdateImageUrl="~/Images/Zulassung/save_icon.png">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:CommandField>                                                                                                                                      
            </Columns>
        </asp:GridView>
    </div>
    <div id="pagination">
        <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
    </div>
</div>