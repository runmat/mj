Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class ZLD_Suche
    Inherits Base.Business.DatenimportBase

#Region "Declarations"
    Private m_strKennzeichen As String
    Private m_strPLZ As String
    Private m_strZulassungspartner As String
    Private m_strZulassungspartnerNr As String
    Private m_tblResultRaw As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ResultRaw() As DataTable
        Get
            Return m_tblResultRaw
        End Get
    End Property
    Public Property PLZ() As String
        Get
            Return m_strPLZ
        End Get
        Set(ByVal Value As String)
            m_strPLZ = Value
        End Set
    End Property
    Public Property Zulassungspartner() As String
        Get
            Return m_strZulassungspartner
        End Get
        Set(ByVal Value As String)
            m_strZulassungspartner = Value
        End Set
    End Property
    Public Property ZulassungspartnerNr() As String
        Get
            Return m_strZulassungspartnerNr
        End Get
        Set(ByVal Value As String)
            m_strZulassungspartnerNr = Value
        End Set
    End Property
    Public Property Kennzeichen() As String
        Get
            Return m_strKennzeichen
        End Get
        Set(ByVal Value As String)
            m_strKennzeichen = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, _
                 ByVal strSessionID As String, _
                 ByVal page As Web.UI.Page)

        m_strClassAndMethod = "ZLD_Suche.Fill"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BAPIRDZ", m_objApp, m_objUser, page)

                myProxy.setImportParameter("IZKFZKZ", m_strKennzeichen)
                myProxy.setImportParameter("IPOST_CODE1", m_strPLZ)
                myProxy.setImportParameter("INAME1", m_strZulassungspartner)
                myProxy.setImportParameter("IREMARK", m_strZulassungspartnerNr)

                myProxy.callBapi()

                m_tblResultRaw = New DataTable
                m_tblResultRaw = myProxy.getExportTable("ITAB")
                CreateOutPut(m_tblResultRaw, strAppID)
                m_tblResult.Columns.Add("Details", System.Type.GetType("System.String"))

                Dim tmpRow As DataRow
                For Each tmpRow In m_tblResult.Rows
                    tmpRow("Details") = "Report30_2s.aspx?ID=" & tmpRow("ID").ToString
                Next
                m_tblResult.Columns.Remove("ID")
                WriteLogEntry(True, "ZKFZKZ=" & m_strKennzeichen & ", POST_CODE1=" & m_strPLZ & ", NAME1=" & m_strZulassungspartner & ", REMARK=" & m_strZulassungspartnerNr, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                WriteLogEntry(False, "ZKFZKZ=" & m_strKennzeichen & ", POST_CODE1=" & m_strPLZ & ", NAME1=" & m_strZulassungspartner & ", REMARK=" & m_strZulassungspartnerNr & ", " & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class
