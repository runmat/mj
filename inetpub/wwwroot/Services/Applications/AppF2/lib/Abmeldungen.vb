Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Abmeldungen
    Inherits DatenimportBase

#Region " Declarations"
    Private m_DatumVon As Date
    Private m_DatumBis As Date
#End Region

#Region " Properties"
    Public Property DatumVon() As Date
        Get
            Return Me.m_DatumVon
        End Get
        Set(ByVal value As System.DateTime)
            Me.m_DatumVon = value
        End Set
    End Property
    Public Property DatumBis() As Date
        Get
            Return Me.m_DatumBis
        End Get
        Set(ByVal value As System.DateTime)
            Me.m_DatumBis = value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Abmeldungen.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)


            Try


                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_ABMELDUNGEN_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_AG", strKUNNR)
                myProxy.setImportParameter("I_DAT_VON", CStr(m_DatumVon))
                myProxy.setImportParameter("I_DAT_BIS", CStr(m_DatumBis))


                myProxy.callBapi()

                Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_ABMELDUNGEN")
                For Each row As DataRow In tblTemp2.Rows
                    If IsDate(row("EXPIRY_DATE").ToString) Then
                        row("EXPIRY_DATE") = FormatDateTime(CDate(row("EXPIRY_DATE").ToString), DateFormat.ShortDate)
                    End If

                Next
                CreateOutPut(tblTemp2, strAppID)

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Abmeldungen im Zeitraum gefunden."
                    Case "DAT_ERROR"
                        m_strMessage = "Selektions-Zeitraum fehlerhaft."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class
' ************************************************
' $History: Abmeldungen.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 9.10.09    Time: 10:56
' Updated in $/CKAG2/Applications/AppF2/lib
' Aufgeräumt.
' 
