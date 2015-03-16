<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dokumentenanforderung_2.aspx.cs"
    Inherits="AutohausPortal.forms.Dokumentenanforderung_2" MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>

<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Form.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 10px">
        <h3>
            <asp:Label ID="lblError" runat="server" Style="color: #B54D4D" Text=""></asp:Label>
            <asp:Label ID="lblMessage" runat="server" Style="color: #269700" Text=""></asp:Label>      
        </h3>
    </div>
    <div style="margin-bottom: 5px">
        <b>Ortskennzeichen:&nbsp;</b><asp:Label ID="lblKreisKZ" runat="server"></asp:Label>
    </div>
    <table class="GridView" id="gvZuldienst" style="width: 960px; border-collapse: collapse;"
        border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr class="GridTableHead" style="color: White;">
                <th scope="col" style="width: 200px;">
                    Kategorie/Dokument**
                </th>
                <th scope="col" style="padding-left:0px">
                    ZB1
                </th>
                <th scope="col" style="padding-left:0px">
                    ZB2
                </th>
                <th scope="col" style="padding-left:0px">
                    CoC
                </th>
                <th scope="col" style="padding-left:0px">
                    eVB
                </th>
                <th scope="col" style="padding-left:0px">
                    VM
                </th>
                <th scope="col" style="padding-left:0px">
                    PA
                </th>
                <th scope="col" style="padding-left:0px">
                    GewA
                </th>
                <th scope="col" style="padding-left:0px">
                    HRA
                </th>
                <th scope="col" style="padding-left:0px">
                    <asp:HyperLink ID="lnkEinzug" Style="text-decoration: underline!important; height: 18px" runat="server">SEPA</asp:HyperLink>
                </th>
                <th scope="col" style="width: 360px;" style="padding-left:0px">
                    Bemerkung
                </th>
            </tr>
            <tr class="ItemStyle subheaderrow">
                <td>
                    <b>Privat</b>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr class="ItemStyle2">
                <td>
                    <b>Zulassung</b>
                </td>
                <td>
                    <asp:Label ID="Label01" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label00" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label02" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label03" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label04" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label05" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label06" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label07" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label08" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label09" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="ItemStyle2">
                <td>
                    <b>Umschreibung</b>
                </td>
                <td>
                    <asp:Label ID="Label11" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label10" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label12" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label13" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label14" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label15" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label16" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label17" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label18" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label19" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="ItemStyle2">
                <td>
                    <b>Umkennzeichnung</b>
                </td>
                <td>
                    <asp:Label ID="Label21" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label20" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label22" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label23" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label24" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label25" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label26" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label27" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label28" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label29" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="ItemStyle2">
                <td>
                    <b>Ersatzfahrzeugschein</b>
                </td>
                <td>
                    <asp:Label ID="Label31" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label30" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label32" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label33" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label34" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label35" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label36" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label37" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label38" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label39" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="ItemStyle subheaderrow">
                <td>
                    <b>Unternehmen</b>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr class="ItemStyle2">
                <td>
                    <b>Zulassung</b>
                </td>
                <td>
                    <asp:Label ID="Label41" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label40" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label42" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label43" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label44" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label45" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label46" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label47" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label48" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label49" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="ItemStyle2">
                <td>
                    <b>Umschreibung</b>
                </td>
                <td>
                    <asp:Label ID="Label51" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label50" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label52" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label53" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label54" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label55" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label56" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label57" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label58" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label59" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="ItemStyle2">
                <td>
                    <b>Umkennzeichnung</b>
                </td>
                <td>
                    <asp:Label ID="Label61" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label60" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label62" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label63" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label64" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label65" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label66" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label67" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label68" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label69" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="ItemStyle2">
                <td>
                    <b>Ersatzfahrzeugschein</b>
                </td>
                <td>
                    <asp:Label ID="Label71" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label70" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label72" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label73" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label74" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label75" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label76" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label77" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label78" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label79" runat="server"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
    <div class="legendcontent">
        <h3>
            **Legende</h3>
        <div class="legenditem">
            <b>O</b> = Original</div>
        <div class="legenditem">
            <b>K</b> = Kopie</div>
        <div class="legenditem">
            <b>F</b> = Formular Zulassungsstelle</div>
        <div class="clear">
        </div>
        <div class="legenditem">
            <b>ZB1</b> = Fahrzeugschein</div>
        <div class="legenditem">
            <b>ZB2</b> = Fahrzeugbrief</div>
        <div class="legenditem">
            <b>CoC</b> = Certificate of Conformity</div>
        <div class="legenditem">
            <b>eVB</b> = elektronische Versicherungsbestätigung</div>
        <div class="legenditem">
            <b>VM</b> = Vollmacht</div>
        <div class="legenditem">
            <b>PA</b> = Personalausweis</div>
        <div class="legenditem">
            <b>GewA</b> = Gewerbeanmeldung</div>
        <div class="legenditem">
            <b>HRA</b> = Handelsregister</div>
        <div class="legenditem">
            <b>SEPA</b> = SEPA-Mandat für Kfz-Steuer</div>
        <div class="clear">
        </div>
    </div>
    <p class="advice">
        Wir weisen daruaf hin, dass diese Angaben unverbindliche Auskünfte der entsprechenden
        Zulassungskreise sind.</p>
    <div class="linkelements">
        <div class="linkelement">
            <b>Links:</b></div>
        <div class="linkelement">
            <asp:LinkButton ID="cmdAmt" runat="server" OnClick="cmdAmt_Click" Enabled="False">Amt</asp:LinkButton></div>
        <div class="linkelement">
            <asp:LinkButton ID="cmdWunsch" runat="server" OnClick="cmdWunsch_Click" Enabled="False">Wunschkennzeichen</asp:LinkButton></div>
        <div class="linkelement">
            <asp:LinkButton ID="cmdFormulare" runat="server" OnClick="cmdFormulare_Click" Enabled="False">Formulare</asp:LinkButton></div>
        <div class="linkelement">
            <asp:LinkButton ID="cmdGebuehr" runat="server" OnClick="cmdGebuehr_Click" Enabled="False">Gebühren</asp:LinkButton></div>
        <div class="clear">
            <asp:Label ID="lblInfo" runat="server" Style="color: #B54D4D" ></asp:Label>
        </div>
    </div>
    <h3>
        Haftungsausschuss</h3>
    <p class="haftung">
        DAD / Christoph Kroschke GmbH übernimmt keine Haftung oder Garantie für die Aktualität,
        Richtigkeit oder Vollständigkeit der auf dieser Website bereitgestellten Informationen.
        Die Inhalte dieser Internet-Seiten basieren teilweise auf gesetzlichen Grundlagen
        und werden regelmäßig überprüft. Es kann nicht garantiert werden, dass nach einer
        gesetzlichen Änderung eine sofortige Anpassung der Internet-Seiten erfolgt. DAD
        / Christoph Kroschke GmbH übernimmt keine Haftung für direkte oder indirekte Schäden,
        die aus der Benutzung dieser Website entstehen können.
    </p>
</asp:Content>
