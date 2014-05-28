Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common

Public Class ZugelasseneFahrz
    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Web.UI.Page, ByVal datumAb As Date, ByVal datumBis As Date)
        m_strClassAndMethod = "ZugelasseneFahrz.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_ZUGEL_FZG_ZUM_DATUM", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_ZULDAT_VON", datumAb.ToShortDateString)
                myProxy.setImportParameter("I_ZULDAT_BIS", datumBis.ToShortDateString)

                myProxy.callBapi()

                Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_DATEN")

                tblTemp2.Columns.Add("Anschrift_ZH", String.Empty.GetType)
                tblTemp2.Columns.Add("Anschrift_ZP", String.Empty.GetType)
                tblTemp2.Columns.Add("Anschrift_ZE", String.Empty.GetType)

                'adressdaten in eine Spalte Zusammenfassen JJU20100813
                For Each tmpRow As DataRow In tblTemp2.Rows
                    tmpRow("Anschrift_ZH") = tmpRow("Name1_ZH").ToString & "<br>" & tmpRow("Ort01_ZH").ToString
                    tmpRow("Anschrift_ZE") = tmpRow("Name1_ZE").ToString & "<br>" & tmpRow("Ort01_ZE").ToString
                    tmpRow("Anschrift_ZP") = tmpRow("Name1_ZP").ToString & "<br>" & tmpRow("STREET_ZP").ToString & " " & tmpRow("HAUSNR_ZP").ToString _
                    & "<br>" & tmpRow("POSTLZ_ZP").ToString & " " & tmpRow("Ort01_ZP").ToString
                Next

                tblTemp2.AcceptChanges()

                ResultTable = tblTemp2

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: ZugelasseneFahrz.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.08.10   Time: 11:42
' Updated in $/CKAG2/Services/Components/ComCommon/lib
' ITA 4041 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 13.08.10   Time: 10:20
' Created in $/CKAG2/Services/Components/ComCommon/lib
' ITA 4041 unfertig
' 
' ************************************************