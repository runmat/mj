Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class MDR_03
    REM § Lese-/Schreibfunktion, Kunde: MDR, 
    REM § Show - BAPI: keine,
    REM § Change - BAPI: Z_Dad_Cs_Mdr_Import_Basis.

    Inherits BankBase

#Region " Declarations"
    Private m_strDateiname As String = String.Empty

    Private m_tblFileInput As DataTable

    Private m_datVerarbeitung_Datum As Date
    Private m_datVerarbeitung_Zeit As Date

    Private m_intGelesen As Integer
    Private m_intBereits_Angelegt As Integer
    Private m_intGespeichert As Integer
    Private m_intKundeUnbekannt As Integer

    Private dataArray As ArrayList
    Private rowToChange As DataRow
#End Region

#Region " Properties"
    Public ReadOnly Property Verarbeitung_Datum() As Date
        Get
            Return m_datVerarbeitung_Datum
        End Get
    End Property

    Public ReadOnly Property Verarbeitung_Zeit() As Date
        Get
            Return m_datVerarbeitung_Zeit
        End Get
    End Property

    Public ReadOnly Property Gelesen() As Integer
        Get
            Return m_intGelesen
        End Get
    End Property

    Public ReadOnly Property Bereits_Angelegt() As Integer
        Get
            Return m_intBereits_Angelegt
        End Get
    End Property

    Public ReadOnly Property Gespeichert() As Integer
        Get
            Return m_intGespeichert
        End Get
    End Property

    Public ReadOnly Property KundeUnbekannt() As Integer
        Get
            Return m_intKundeUnbekannt
        End Get
    End Property

    Public ReadOnly Property Unbekannt_Abgewiesen() As Integer
        Get
            Return m_intGelesen - m_intBereits_Angelegt - m_intGespeichert - m_intKundeUnbekannt
        End Get
    End Property

    Public Property rowChange() As DataRow
        Get
            Return rowToChange
        End Get
        Set(ByVal Value As DataRow)
            rowToChange = Value
        End Set
    End Property

    Public Property objData() As ArrayList
        Get
            Return dataArray
        End Get
        Set(ByVal Value As ArrayList)
            dataArray = Value
        End Set
    End Property

    Public Property Dateiname() As String
        Get
            Return m_strDateiname
        End Get
        Set(ByVal Value As String)
            m_strDateiname = Value
        End Set
    End Property

    Public Property FileInput() As DataTable
        Get
            Return m_tblFileInput
        End Get
        Set(ByVal Value As DataTable)
            m_tblFileInput = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_tblFileInput = New DataTable()
        m_tblFileInput.Columns.Add("I_Filetable_Data", System.Type.GetType("System.String"))
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try

                m_intIDSAP = -1

                m_intStatus = 0
                m_strMessage = ""


                Dim strDaten As String = "Dateiname=" & m_strDateiname

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Dad_Cs_Mdr_Import_Basis", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_DATEINAME", m_strDateiname)
                S.AP.Init("Z_Dad_Cs_Mdr_Import_Basis", "I_DATEINAME", m_strDateiname)

                Dim tblInput As DataTable = S.AP.GetImportTable("I_MDR_BASIS") 'myProxy.getImportTable("I_MDR_BASIS")

                Dim Row As DataRow
                Dim NeuRow As DataRow
                For Each Row In m_tblFileInput.Rows
                    NeuRow = tblInput.NewRow
                    NeuRow("I_FILETABLE_DATA") = Row("I_FILETABLE_DATA")
                    tblInput.Rows.Add(NeuRow)
                Next
                tblInput.AcceptChanges()

                'myProxy.callBapi()
                S.AP.Execute()

                Dim tblOutput As DataTable = S.AP.GetExportTable("O_MDR_BASIS_PROT") 'myProxy.getExportTable("O_MDR_BASIS_PROT")

                Dim rowOutput As DataRow
                For Each rowOutput In tblOutput.Rows
                    m_datVerarbeitung_Datum = CDate(rowOutput("Dat_Verarb").ToString)
                    m_datVerarbeitung_Zeit = HelpProcedures.MakeTimeStandard(rowOutput("Tim_Verarb").ToString)
                    m_intGelesen = CInt(rowOutput("Gelesen").ToString)
                    m_intBereits_Angelegt = CInt(rowOutput("Bereits_Angelegt").ToString)
                    m_intGespeichert = CInt(rowOutput("Gespeichert").ToString)
                    m_intKundeUnbekannt = CInt(rowOutput("Kunde_Unbekannt").ToString)
                Next

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden. (" & ex.Message & ")"
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region
End Class

' ************************************************
' $History: MDR_03.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 8.07.09    Time: 15:46
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 7.07.09    Time: 17:03
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 2  *****************
' User: Uha          Date: 29.08.07   Time: 10:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA 1224: Neue Ergebnisspalte "Kunde_Unbekannt" hinzugefügt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 15.08.07   Time: 16:18
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA 1224: "Hinterlegung ALM-Daten" (Change80) hinzugefügt
' 
' ************************************************
