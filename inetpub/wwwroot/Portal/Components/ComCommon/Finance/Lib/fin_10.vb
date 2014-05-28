Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class fin_10
    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_blnAll As Boolean
    Private m_datAbDatum As Date
    Private m_datBisDatum As Date
    Private m_strAction As String
    Private m_strVersand As String
    Private m_strAbrufgrund As String
    Private m_boolTempSelection As Boolean = False
    Private mReferenznummer As String

#End Region

#Region " Properties"
    Public ReadOnly Property Abrufgrund(ByVal IDGrund As String) As String
        Get
            Dim cn As SqlClient.SqlConnection
            Dim cmdAg As SqlClient.SqlCommand
            Dim dsAg As DataSet
            Dim adAg As SqlClient.SqlDataAdapter
            cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Try

                cn.Open()

                dsAg = New DataSet()

                adAg = New SqlClient.SqlDataAdapter()

                cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                                "[WebBezeichnung]" & _
                                                "FROM CustomerAbrufgruende " & _
                                                "WHERE " & _
                                                "CustomerID = " & m_objUser.Customer.CustomerId.ToString & _
                                                " AND GroupID = " & m_objUser.GroupID.ToString & _
                                                " AND SAPWert = '" & IDGrund & "'", cn)
                cmdAg.CommandType = CommandType.Text
                'AbrufTyp: 'temp' oder 'endg'
                Dim dr As SqlClient.SqlDataReader
                dr = cmdAg.ExecuteReader
                While dr.Read
                    m_strAbrufgrund = dr("WebBezeichnung").ToString
                End While
            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try
            Return m_strAbrufgrund
        End Get
    End Property

    Public Property Referenznummer() As String
        Get
            Return mReferenznummer
        End Get
        Set(ByVal Value As String)
            mReferenznummer = Value
        End Set
    End Property

    Public Property tmpSelection() As Boolean
        Get
            Return m_boolTempSelection
        End Get
        Set(ByVal Value As Boolean)
            m_boolTempSelection = Value
        End Set
    End Property

    Public Property datVON() As Date
        Get
            Return m_datAbDatum
        End Get
        Set(ByVal Value As Date)
            m_datAbDatum = Value
        End Set
    End Property

    Public Property datBIS() As Date
        Get
            Return m_datBisDatum
        End Get
        Set(ByVal Value As Date)
            m_datBisDatum = Value
        End Set
    End Property

    Public Property action() As String
        Get
            Return m_strAction
        End Get
        Set(ByVal Value As String)
            m_strAction = Value
        End Set
    End Property


    Public Property versand() As String
        Get
            Return m_strVersand
        End Get
        Set(ByVal Value As String)
            m_strVersand = Value
        End Set
    End Property

#End Region

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal datAbDatum As Date, ByVal datBisDatum As Date,
                    ByVal strAction As String, ByVal strVersand As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)

        m_blnAll = False
        m_datAbDatum = datAbDatum
        m_datBisDatum = datBisDatum
        m_strAction = strAction
        m_strVersand = strVersand
    End Sub

    'Public Overloads Overrides Sub Fill()

    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "fin_10.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0

            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                S.AP.Init("Z_DAD_DATEN_EINAUS_REPORT_002")
                S.AP.SetImportParameter("DATANF", m_datAbDatum)
                S.AP.SetImportParameter("DATEND", m_datBisDatum)
                S.AP.SetImportParameter("ACTION", m_strAction)
                S.AP.SetImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("LIZNR", mReferenznummer)
                S.AP.SetImportParameter("ABCKZ", m_strVersand)
                S.AP.Execute()

                Dim tblEINNEU As DataTable = S.AP.GetExportTable("EINNEU")

                ' Testdaten bei Bedarf puffern
                'GeneralTools.Services.XmlService.XmlSerializeToFile(tblEINNEU, "C:\Temp\tblEINNEU.xml")
                'tblEINNEU = GeneralTools.Services.XmlService.XmlDeserializeFromFile(Of DataTable)("C:\Temp\tblEINNEU.xml")

                'auswerten der exportparameter
                If tblEINNEU IsNot Nothing Then
                    Dim tblTemp2 As DataTable
                    Dim rowTemp As DataRow
                    tblTemp2 = tblEINNEU

                    For Each rowTemp In tblTemp2.Rows
                        Select Case CStr(rowTemp("ABCKZ"))
                            Case ""
                                rowTemp("ABCKZ") = ""
                            Case "1"
                                rowTemp("ABCKZ") = "temporär"
                            Case "2"
                                rowTemp("ABCKZ") = "endgültig"
                            Case "5"
                                rowTemp("ABCKZ") = "Händler Zulassung"
                        End Select
                    Next

                    For Each rowTemp In tblTemp2.Rows
                        Dim strgrund As String
                        strgrund = Abrufgrund(CStr(rowTemp("ZZVGRUND")))
                        rowTemp("ZZVGRUND") = strgrund
                    Next
                    tblTemp2.Columns.Add("Adresse", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Standort", System.Type.GetType("System.String"))
                    For Each rowTemp In tblTemp2.Rows
                        rowTemp("Adresse") = rowTemp("Name1").ToString & ", " & rowTemp("Post_Code1").ToString & " " & rowTemp("City1").ToString & ", " & rowTemp("Street").ToString & " " & rowTemp("House_NUM1").ToString
                        rowTemp("Standort") = rowTemp("NAME1_PDI").ToString & " " & rowTemp("NAME2_PDI").ToString & ", " & rowTemp("ORT01_PDI").ToString
                    Next
                    tblTemp2.AcceptChanges()

                    CreateOutPut(tblTemp2, strAppID)
                    WriteLogEntry(True, "ACTION=" & m_strAction & ", DATANF=" & m_datAbDatum.ToShortDateString & ", DATEND=" & m_datBisDatum.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

                End If

            Catch ex As Exception
                ResultTable = Nothing
                m_intStatus = -9999
                
                Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                If errormessage.Contains("NO_DATA") Then
                    m_strMessage = "Keine Daten vorhanden."
                Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End If
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
End Class
