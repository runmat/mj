<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02.aspx.vb" Inherits="CKG.Components.ComCommon.DAT.Report02"
  MasterPageFile="../../../MasterPage/Services.Master" Async="true" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
  <div id="site">
    <div id="content">
      <div id="navigationSubmenu">
        <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
          Text="zurück" />
      </div>
      <div id="innerContent">
        <div id="innerContentRight" style="width: 100%">
          <div id="innerContentRightHeading">
            <h1>
              <asp:Label ID="lblHead" runat="server" Text="Label" />
            </h1>
          </div>
          <div id="paginationQuery" style="text-align: right;">
            <span style="font-weight: bold;">Bankenlinie  Version:&nbsp;</span><asp:Label ID="lbVersion" runat="server" />
          </div>
          <asp:Panel ID="Panel1" runat="server">
            <div>
              <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="false" />
            </div>
            <div id="TableQuery" style="margin-bottom: 10px">
              <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblVehicleType" runat="server" Text="Fahrzeugart:" AssociatedControlID="ddlVehicleTypes" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlVehicleTypes" runat="server" DataTextField="value" DataValueField="key"
                      CssClass="long" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="VehicleTypesSelectedIndexChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblManufacturer" runat="server" Text="Hersteller:" AssociatedControlID="ddlManufacturers" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlManufacturers" runat="server" DataTextField="value" DataValueField="key"
                      CssClass="long" AutoPostBack="true" AppendDataBoundItems="true" Enabled="false"
                      OnSelectedIndexChanged="ManufacturersSelectedIndexChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblErstzulassung" runat="server" Text="Erstzulassung (Monat/Jahr):" AssociatedControlID="ddlErstzulassungMonat" />
                  </td>
                  <td class="active">
                    <asp:DropDownList ID="ddlErstzulassungMonat" runat="server" DataTextField="value" DataValueField="key"
                      CssClass="short" AutoPostBack="true" AppendDataBoundItems="true" Enabled="false"
                      OnSelectedIndexChanged="ErstzulassungSelectedIndexChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                  <td class="active" colspan="2">
                    <asp:DropDownList ID="ddlErstzulassungJahr" runat="server" DataTextField="value" DataValueField="key"
                      CssClass="short" AutoPostBack="true" AppendDataBoundItems="true" Enabled="false"
                      OnSelectedIndexChanged="ErstzulassungSelectedIndexChanged" style="margin-left: 10px">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblBaseModel" runat="server" Text="Haupttyp:" AssociatedControlID="ddlBaseModels" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlBaseModels" runat="server" DataTextField="value" DataValueField="key"
                      CssClass="long" AutoPostBack="true" AppendDataBoundItems="true" Enabled="false"
                      OnSelectedIndexChanged="BaseModelsSelectedIndexChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblSubModel" runat="server" Text="Untertyp:" AssociatedControlID="ddlSubModels" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlSubModels" runat="server" DataTextField="value" DataValueField="key"
                      CssClass="long" AutoPostBack="true" AppendDataBoundItems="true" Enabled="false"
                      OnSelectedIndexChanged="SubModelsSelectedIndexChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblEuropaCode" runat="server" Text="Dat &#8364;uropa-Code®:" AssociatedControlID="lblEuropaCodeDisplay" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:Label ID="lblEuropaCodeDisplay" runat="server" EnableViewState="true" />
                  </td>
                </tr>
                <tr class="formquery" id="trEngineOptions" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblEngineOptions" runat="server" Text="Motortyp:" AssociatedControlID="ddlEngineOptions" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlEngineOptions" runat="server" EnableViewState="true" DataTextField="value"
                      DataValueField="key" CssClass="long" AutoPostBack="true" AppendDataBoundItems="true"
                      Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr class="formquery" id="trBodyOptions" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblbodyOptions" runat="server" Text="Karosserie:" AssociatedControlID="ddlBodyOptions" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlBodyOptions" runat="server" EnableViewState="true" DataTextField="value"
                      DataValueField="key" CssClass="long" AutoPostBack="true" AppendDataBoundItems="true"
                      Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr class="formquery" id="trGearingOptions" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblGearingOptions" runat="server" Text="Getriebe:" AssociatedControlID="ddlGearingOptions" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlGearingOptions" runat="server" EnableViewState="true" DataTextField="value"
                      DataValueField="key" CssClass="long" AutoPostBack="true" AppendDataBoundItems="true"
                      Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr class="formquery" id="trEquipmentLineOptions" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblEquipmentLineOptions" runat="server" Text="Ausstattungslinie:" AssociatedControlID="ddlEquipmentLineOptions" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlEquipmentLineOptions" runat="server" EnableViewState="true"
                      DataTextField="value" DataValueField="key" CssClass="long" AutoPostBack="true"
                      AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>

                <!-- Transporter -->
                <tr class="formquery" id="transporter1" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblWheelbase" runat="server" Text="Radstand:" AssociatedControlID="ddlWheelbase" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlWheelbase" runat="server" EnableViewState="true"
                      DataTextField="value" DataValueField="key" CssClass="long" AutoPostBack="true"
                      AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>

                <tr class="formquery" id="transporter2" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblTypeOfDrive" runat="server" Text="Antriebsart:" AssociatedControlID="ddlTypeOfDrive" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlTypeOfDrive" runat="server" EnableViewState="true"
                      DataTextField="value" DataValueField="key" CssClass="long" AutoPostBack="true"
                      AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>

                <!-- LKW -->
                <tr class="formquery" id="lkw1" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblConstructionOptions" runat="server" Text="Konstruktion:" AssociatedControlID="ddlConstructionOptions" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlConstructionOptions" runat="server" EnableViewState="true"
                      DataTextField="value" DataValueField="key" CssClass="long" AutoPostBack="true"
                      AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>

                <tr class="formquery" id="lkw2" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblNumberOfAxleOptions" runat="server" Text="Achskonfiguration:" AssociatedControlID="ddlNumberOfAxleOptions" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlNumberOfAxleOptions" runat="server" EnableViewState="true"
                      DataTextField="value" DataValueField="key" CssClass="long" AutoPostBack="true"
                      AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>

                <tr class="formquery" id="lkw3" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblDriversCabOptions" runat="server" Text="Kabine:" AssociatedControlID="ddlDriversCabOptions" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlDriversCabOptions" runat="server" EnableViewState="true"
                      DataTextField="value" DataValueField="key" CssClass="long" AutoPostBack="true"
                      AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>

                <tr class="formquery" id="lkw4" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblGrossVehicleWeightOptions" runat="server" Text="Gesamtgewicht:" AssociatedControlID="ddlGrossVehicleWeightOptions" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlGrossVehicleWeightOptions" runat="server" EnableViewState="true"
                      DataTextField="value" DataValueField="key" CssClass="long" AutoPostBack="true"
                      AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>

                <tr class="formquery" id="lkw5" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblSuspensionOptions" runat="server" Text="Federung:" AssociatedControlID="ddlSuspensionOptions" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlSuspensionOptions" runat="server" EnableViewState="true"
                      DataTextField="value" DataValueField="key" CssClass="long" AutoPostBack="true"
                      AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ExtendedOptionsChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>


                <tr class="formquery" id="trContainer" runat="server" visible="false">
                  <td class="firstLeft active">
                    <asp:Label ID="lblTyp" runat="server" Text="Typ:" AssociatedControlID="ddlContainer" />
                  </td>
                  <td class="active" colspan="3">
                    <asp:DropDownList ID="ddlContainer" runat="server" EnableViewState="true"
                      DataTextField="value" DataValueField="key" CssClass="long" AutoPostBack="true"
                      AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ContainerChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active">
                    <asp:Label ID="lblConstructionYear" runat="server" Text="Baujahr:" AssociatedControlID="ddlConstructionYear" />
                  </td>
                  <td class="active">
                    <asp:DropDownList ID="ddlConstructionYear" runat="server" AppendDataBoundItems="true" DataTextField="value" DataValueField="key"
                      CssClass="short" Enabled="false" AutoPostBack="True" OnSelectedIndexChanged="ConstructionYearChanged">
                      <asp:ListItem Value="" Text="Bitte auswählen." />
                    </asp:DropDownList>
                    <asp:HiddenField runat="server" ID="hfMinConstructionTime" />
                  </td>
                  <td class="firstLeft active">
                    <asp:Label ID="lblMileage" runat="server" Text="Kilometerstand:" AssociatedControlID="txtMileage" />
                  </td>
                  <td class="active" style="width: 60%;">
                    <asp:TextBox ID="txtMileage" runat="server" CssClass="short" Enabled="false" />
                  </td>
                </tr>
                <tr class="formquery">
                  <td class="firstLeft active" style="height: 19px" colspan="4">
                    &nbsp;
                  </td>
                </tr>
                <tr class="formquery">
                  <td colspan="4">
                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                      Width="1px" />
                  </td>
                </tr>
              </table>
              <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                &nbsp;
              </div>
            </div>
          </asp:Panel>
          <div id="dataQueryFooter">
            <asp:LinkButton ID="lbCreate" runat="server" CssClass="TablebuttonLarge" Width="128px"
              Height="16px" Text="» Preis ermitteln" OnClick="SuchenClicked" Enabled="false" />
          </div>
          <div id="Result" runat="Server" visible="false">
            <table cellpadding="0" cellspacing="0" width="100%" style="margin-bottom: 20px;">
              <tr>
                <td align="right" style="width: 50%; font-weight: bold; padding-right: 30px">
                  <asp:Label ID="lblDealerPurchasePrice" runat="server" Text="Händlereinkaufswert:"
                    AssociatedControlID="lblDealerPurchasePriceDisplay" />
                </td>
                <td>
                  <asp:Label ID="lblDealerPurchasePriceDisplay" runat="server" />
                </td>
              </tr>
              <tr>
                <td align="right" style="width: 50%; font-weight: bold; padding-right: 30px">
                  <asp:Label ID="lblDealerSalesPrice" runat="server" Text="Händlerverkaufswert:" AssociatedControlID="lblDealerSalesPriceDisplay" />
                </td>
                <td>
                  <asp:Label ID="lblDealerSalesPriceDisplay" runat="server" />
                </td>
              </tr>
              <tr style="display:none">
                <td align="right" style="width: 50%; font-weight: bold; padding-right: 30px">
                  <asp:Label ID="lblDealerSalesPrice2" runat="server" Text="Händlerverkaufswert:" AssociatedControlID="lblDealerSalesPriceDisplay" />
                </td>
                <td>
                  <asp:Label ID="lblDealerSalesPriceDisplay2" runat="server" />
                </td>
              </tr>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
