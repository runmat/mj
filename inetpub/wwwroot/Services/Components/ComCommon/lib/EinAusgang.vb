Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class EinAusgang
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datAbDatum As Date
    Private m_datBisDatum As Date
    Private m_strAction As String
#End Region


#Region "Properties"

    Public Property ABC As String = "A"

#End Region


#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal datAbDatum As Date, ByVal datBisDatum As Date, ByVal strAction As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_datAbDatum = datAbDatum
        m_datBisDatum = datBisDatum
        m_strAction = strAction
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "EinAusgang.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DAD_DATEN_EINAUS_REPORT_002", m_objApp, m_objUser, page)

                myProxy.setImportParameter("KUNNR", strKUNNR)
                myProxy.setImportParameter("ACTION", m_strAction)
                myProxy.setImportParameter("DATANF", m_datAbDatum.ToShortDateString)
                myProxy.setImportParameter("DATEND", m_datBisDatum.ToShortDateString)
                myProxy.setImportParameter("ABCKZ", ABC)

                myProxy.callBapi()

                Dim tblTemp2 As DataTable = myProxy.getExportTable("EINNEU")


                tblTemp2.Columns.Add("IDENT5", GetType(System.String))

                If tblTemp2.Rows.Count > 0 Then



                    For Each dr As DataRow In tblTemp2.Rows


                        If m_strAction = "AUS" Then
                            dr("ERDAT") = dr("ZZTMPDT")
                        End If

                        dr("IDENT5") = Right(dr("CHASSIS_NUM").ToString, 5)

                    Next
                    

                End If


                ResultTable = tblTemp2

                'CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "ACTION=" & m_strAction & ", DATANF=" & m_datAbDatum.ToShortDateString & ", DATEND=" & m_datBisDatum.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -1111
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "ACTION=" & m_strAction & ", DATANF=" & m_datAbDatum.ToShortDateString & ", DATEND=" & m_datBisDatum.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region
End Class

' ************************************************
' $History: EinAusgang.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 12.08.10   Time: 14:42
' Updated in $/CKAG2/Services/Components/ComCommon/lib
' ITA 3814 testfertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 5.07.10    Time: 17:06
' Created in $/CKAG2/Services/Components/ComCommon/lib
' 