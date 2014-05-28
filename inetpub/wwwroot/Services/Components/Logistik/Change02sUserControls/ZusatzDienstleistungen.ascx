<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ZusatzDienstleistungen.ascx.vb" Inherits="CKG.Components.Logistik.ZusatzDienstleistungen" %>
<%@ Register TagPrefix="uc" TagName="Services" Src="~/Components/Logistik/Change02sUserControls/Services.ascx" %>
<asp:UpdatePanel ID="upZusatzdienstleistungen" runat="server" ChildrenAsTriggers="true">
  <ContentTemplate>
    <div id="data">
      <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="gvFahrten"
        CssClass="GridView" GridLines="None" AllowPaging="False" AllowSorting="False" OnSelectedIndexChanging="OnSelectedIndexChanging"
        OnSelectedIndexChanged="OnSelectedIndexChanged">
        <PagerSettings Visible="False" />
        <HeaderStyle CssClass="GridTableHead" />
        <AlternatingRowStyle CssClass="GridTableAlternate" />
        <RowStyle CssClass="ItemStyle" />
        <Columns>
          <asp:TemplateField HeaderText="">
            <ItemTemplate>
              <asp:LinkButton ID="lbEdit" runat="server" CommandName="Select" CommandArgument='<%# Databinder.Eval(Container, "DataItemIndex") %>'
                CssClass="blueButton edit" ToolTip="Bearbeiten" />
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Transporttyp">
            <ItemTemplate>
              <asp:Label ID="lblTransporttyp" runat="server" Text='<%# Eval("Transporttyp") %>' />
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Firma">
            <ItemTemplate>
              <asp:Label ID="lblFirma" runat="server" Text='<%# Eval("Adresse.Name") %>' />
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="PLZ">
            <ItemTemplate>
              <asp:Label ID="lblPlz" runat="server" Text='<%# Eval("Adresse.Postleitzahl") %>' />
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Ort">
            <ItemTemplate>
              <asp:Label ID="lblOrt" runat="server" Text='<%# Eval("Adresse.Ort") %>' />
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Datum">
            <ItemTemplate>
              <asp:Label ID="lblDatum" runat="server" Text='<%# Eval("Datum", "{0:d}") %>' />
            </ItemTemplate>
          </asp:TemplateField>
        </Columns>
      </asp:GridView>
    </div>
    <asp:Panel id="ServicesPanel" runat="server" Enabled="false">
      <uc:Services ID="Services" runat="server" />
      <asp:LinkButton ID="lbSave" runat="server" CssClass="greyButton save" Style="margin-top: 5px; margin-right: 8px;"
        Text="Speichern" OnClick="OnSave" />
      <asp:LinkButton ID="lbCancel" runat="server" CssClass="greyButton cancel" Style="margin-top: 5px;"
        Text="Verwerfen" OnClick="OnCancel" />
    </asp:Panel>
  </ContentTemplate>
</asp:UpdatePanel>
