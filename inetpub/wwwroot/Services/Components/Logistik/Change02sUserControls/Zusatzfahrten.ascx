<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Zusatzfahrten.ascx.vb" Inherits="CKG.Components.Logistik.Zusatzfahrten" %>
<%@ Register TagPrefix="uc" TagName="TransportAddress" Src="~/Components/Logistik/Change02sUserControls/TransportAddress.ascx" %>
<asp:UpdatePanel ID="upZusatzfahrten" runat="server" ChildrenAsTriggers="true">
  <ContentTemplate>
    <div id="data">
      <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="gvFahrten"
        CssClass="GridView" GridLines="None" AllowPaging="False" AllowSorting="False" OnSelectedIndexChanging="OnSelectedIndexChanging"
        OnSelectedIndexChanged="OnSelectedIndexChanged" OnRowDeleting="OnRowDeleting" OnDataBound="OnFahrtenDataBound"
        OnRowCommand="OnRowCommand">
        <PagerSettings Visible="False" />
        <HeaderStyle CssClass="GridTableHead" />
        <AlternatingRowStyle CssClass="GridTableAlternate" />
        <RowStyle CssClass="ItemStyle" />
        <Columns>
          <asp:TemplateField HeaderText="">
            <ItemTemplate>
              <asp:LinkButton ID="lbDelete" runat="server" CommandName="Delete" CommandArgument='<%# Databinder.Eval(Container, "DataItemIndex") %>'
                CssClass="blueButton delete" ToolTip="Löschen" />
              <asp:LinkButton ID="lbUp" runat="server" CommandName="Up" CommandArgument='<%# Databinder.Eval(Container, "DataItemIndex") %>'
                CssClass="blueButton up" ToolTip="Rauf" />
              <asp:LinkButton ID="lbDown" runat="server" CommandName="Down" CommandArgument='<%# Databinder.Eval(Container, "DataItemIndex") %>'
                CssClass="blueButton down" ToolTip="Runter" />
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
        <EmptyDataTemplate>
          <asp:Label ID="Label8" runat="server">Es sind noch keine Zusatzfahrten eingetragen.</asp:Label>
        </EmptyDataTemplate>
      </asp:GridView>
    </div>
    <div id="address">
      <uc:TransportAddress ID="TransportAddress" runat="server" ShowSearch="false" ShowTransportTypes="true" OnValidateDate="OnValidateDate" />
      <asp:LinkButton ID="lbSave" runat="server" CssClass="greyButton save" Style="margin-top: 5px; margin-right: 8px;"
        Text="Speichern" OnClick="OnSave" />
      <asp:LinkButton ID="lbCancel" runat="server" CssClass="greyButton cancel" Style="margin-top: 5px;"
        Text="Verwerfen" OnClick="OnCancel" />
    </div>
  </ContentTemplate>
</asp:UpdatePanel>
