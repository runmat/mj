Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class MDR_02
    REM § Status-Report, Kunde: MDR, BAPI: Z_Dad_Cs_Mdr_Zulassungen_Bank,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Dim m_strAbdatum As String
    Dim m_strBisdatum As String
    Dim m_strDateiname As String
#End Region

#Region " Properties"
    Property Dateiname() As String
        Get
            Return m_strDateiname
        End Get
        Set(ByVal Value As String)
            m_strDateiname = Value
        End Set
    End Property

    Property datAb() As String
        Get
            Return m_strAbdatum
        End Get
        Set(ByVal Value As String)
            m_strAbdatum = Value
        End Set
    End Property

    Property datBis() As String
        Get
            Return m_strBisdatum
        End Get
        Set(ByVal Value As String)
            m_strBisdatum = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "MDR_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Dad_Cs_Mdr_Zulassungen_Bank", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("DATEINAME", m_strDateiname)

                S.AP.Init("Z_Dad_Cs_Mdr_Zulassungen_Bank", "DATEINAME", m_strDateiname)

                If IsDate(m_strBisdatum) And IsDate(m_strAbdatum) Then
                    'myProxy.setImportParameter("DATUM_BIS", m_strBisdatum)
                    'myProxy.setImportParameter("DATUM_VON", m_strAbdatum)
                    S.AP.SetImportParameter("DATUM_BIS", m_strBisdatum)
                    S.AP.SetImportParameter("DATUM_VON", m_strAbdatum)
                End If

                'myProxy.callBapi()

                S.AP.Execute()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_MDR_ZULASSUNGEN_BANK") 'myProxy.getExportTable("GT_MDR_ZULASSUNGEN_BANK")
                CreateOutPut(tblTemp2, strAppID)

                If m_tblResult.Rows.Count > 0 Then
                    Dim tmpView As DataView = m_tblResult.DefaultView
                    tmpView.Sort = "Datum, Zeit, Dateiname"

                    m_tableResult = New DataTable()
                    m_tableResult.Columns.Add("Dateiname", System.Type.GetType("System.String"))
                    m_tableResult.Columns.Add("Anzeige", System.Type.GetType("System.String"))

                    Dim strDateiname As String = "XXX"
                    Dim viewLoop As Integer
                    Dim tmpRow As DataRow

                    For viewLoop = 0 To tmpView.Count - 1
                        If Not tmpView(viewLoop)("Dateiname").ToString = strDateiname Then
                            tmpRow = m_tableResult.NewRow
                            tmpRow("Dateiname") = tmpView(viewLoop)("Dateiname").ToString
                            tmpRow("Anzeige") = CDate(tmpView(viewLoop)("Datum")).ToShortDateString & " " & tmpView(viewLoop)("Dateiname").ToString
                            m_tableResult.Rows.Add(tmpRow)
                            strDateiname = tmpView(viewLoop)("Dateiname").ToString
                        End If
                    Next
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
 
#End Region
End Class

' ************************************************
' $History: MDR_02.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 3  *****************
' User: Uha          Date: 13.08.07   Time: 10:39
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' CSV-Ausgabe in MDR Report "Versendete Zulassungsdaten" inegriert
' 
' *****************  Version 2  *****************
' User: Uha          Date: 9.08.07    Time: 15:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Report "Versendete Zulassungsdaten" - 1. Version ohne Excel Download
' 
' *****************  Version 1  *****************
' User: Uha          Date: 9.08.07    Time: 11:12
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Report "Versendete Zulassungsdaten" vorbereitet
' 
' ************************************************
