Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common
Imports System
Imports CKG.Base.Business
Imports Microsoft.Data.SAPClient


Public Class ZBIIEingänge
    Inherits DatenimportBase

#Region "Declarations"
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

#Region "Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "ZBIIEingänge.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

        Try


            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_BRIEFE_AUS_EQUI", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", strKUNNR)
            myProxy.setImportParameter("I_ERDAT_VON", CStr(m_DatumVon))
            myProxy.setImportParameter("I_ERDAT_BIS", CStr(m_DatumBis))


            myProxy.callBapi()

            Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")

            CreateOutPut(tblTemp2, strAppID)

        Catch ex As Exception
            m_intStatus = -9999
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine ZBII-Eingänge im Zeitraum gefunden."
                Case "DAT_ERROR"
                    m_strMessage = "Selektions-Zeitraum fehlerhaft."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
    End Sub
#End Region

End Class
