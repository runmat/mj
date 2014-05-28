<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExcelDownload.ascx.cs"
  Inherits="AppMBB.elements.ExcelDownload" %>
<div class="ExcelDiv">
  <div align="right" class="rightPadding">
    <img src="/Services/Images/iconXLS.gif" alt="Excel herunterladen" />
    <span class="ExcelSpan">
      <asp:LinkButton ID="lnkCreateExcel" runat="server" OnClick="OnCreateExcel">Excel herunterladen</asp:LinkButton>
    </span>
  </div>
</div>
