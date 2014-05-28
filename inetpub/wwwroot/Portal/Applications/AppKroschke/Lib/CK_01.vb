Option Explicit On 
Option Strict On

Imports CKG.Base.Kernel
Imports System

<Serializable()> Public Class CK_01
    REM § Lese-/Schreibfunktion, Kunde: Kroschke, 
    REM § Show - BAPI: Z_V_UEBERF_VERFUEGBARKEIT1,
    REM § Change - BAPI: Z_V_UEBERF_VERFUEGBARKEIT2.

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private m_tblFahrerTabelle As DataTable
    Private m_datVon As Date
    Private m_datBis As Date
    Private m_strFahrerNummer As String

    'InputWerte
    Private m_datVerfdat As Date
    Private m_strStunden As String
    Private m_ExportTable As DataTable
#End Region

#Region " Properties"
    Public Property FahrerNummer() As String
        Get
            Return m_strFahrerNummer
        End Get
        Set(ByVal Value As String)
            m_strFahrerNummer = Value
        End Set
    End Property

    Public Property VonDatum() As Date
        Get
            Return m_datVon
        End Get
        Set(ByVal Value As Date)
            m_datVon = Value
        End Set
    End Property

    Public Property BisDatum() As Date
        Get
            Return m_datBis
        End Get
        Set(ByVal Value As Date)
            m_datBis = Value
        End Set
    End Property

    Public Property FahrerTabelle() As DataTable
        Get
            Return m_tblFahrerTabelle
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrerTabelle = Value
        End Set
    End Property

    'InputWerte
    Public Property Verfdat() As Date
        Get
            Return m_datVerfdat
        End Get
        Set(ByVal Value As Date)
            m_datVerfdat = Value
        End Set
    End Property

    Public Property Stunden() As String
        Get
            Return m_strStunden
        End Get
        Set(ByVal Value As String)
            m_strStunden = Value
        End Set
    End Property

    Public Property ExportTable() As DataTable
        Get
            Return m_ExportTable
        End Get
        Set(ByVal Value As DataTable)
            m_ExportTable = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String,
                   ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overloads Overrides Sub show()

    End Sub
    
    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "CK_01.Show"

        'Dim intID As Int32 = -1
        m_intStatus = 0

        Try
            If m_objLogApp Is Nothing Then
                m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Ueberf_Verfuegbarkeit1", m_objApp, m_objUser, page)
            S.AP.Init("Z_V_Ueberf_Verfuegbarkeit1")

            S.AP.SetImportParameter("I_FAHRER", m_objUser.Reference.PadLeft(10, "0"c))
            S.AP.SetImportParameter("I_VONDAT", Replace(m_datVon.ToShortDateString, ".", ""))
            S.AP.SetImportParameter("I_BISDAT", Replace(m_datBis.ToShortDateString, ".", ""))

            'myProxy.callBapi()
            S.AP.Execute()

            Dim TempTable As DataTable = S.AP.GetExportTable("T_VERFUEG1")


            TempTable.DefaultView.Sort = "VERFDAT"

            TempTable = TempTable.DefaultView.ToTable


            'Spanne berechnen
            Dim Startdate As Date = m_datVon
            Dim EndDate As Date = m_datBis

            Dim NewRow As DataRow

            m_tblFahrerTabelle = New DataTable()
            m_tblFahrerTabelle.Columns.Add("VERFDAT", GetType(DateTime))
            m_tblFahrerTabelle.Columns.Add("ANZ_FAHRER", GetType(System.String))


            m_tblFahrerTabelle.AcceptChanges()
            Dim RowSet() As DataRow

            EndDate = EndDate.AddDays(1)

            Do Until Startdate = EndDate
                NewRow = m_tblFahrerTabelle.NewRow

                RowSet = TempTable.Select("VERFDAT = '" & Startdate & "'")


                If RowSet.Length = 0 Then
                    NewRow("VERFDAT") = Startdate
                Else
                    NewRow("VERFDAT") = CDate(RowSet(0)("VERFDAT"))
                    NewRow("ANZ_FAHRER") = RowSet(0)("ANZ_FAHRER")
                End If

                m_tblFahrerTabelle.Rows.Add(NewRow)

                m_tblFahrerTabelle.AcceptChanges()

                Startdate = Startdate.AddDays(1)
            Loop
            
        Catch ex As Exception
            m_intStatus = -9999
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden. "
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
        
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Sub ChangeNew(ByVal AppID As String, ByVal SessionID As String)

        m_strClassAndMethod = "CK_01.ChangeNew"
        m_strAppID = AppID
        m_strSessionID = SessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True
            'Dim intID As Int32 = -1

            Try


                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_UEBERF_VERFUEGBARKEIT2", m_objApp, m_objUser, page)


                'myProxy.setImportParameter("I_FAHRER", m_objUser.Reference.PadLeft(10, "0"c))

                S.AP.Init("Z_V_UEBERF_VERFUEGBARKEIT2", "I_FAHRER", m_objUser.Reference.PadLeft(10, "0"c))

                Dim SapTable As DataTable = S.AP.GetImportTable("GT_FAHRER")


                For Each dr As DataRow In ExportTable.Rows

                    Dim Newrow As DataRow = SapTable.NewRow

                    Newrow("VERDAT") = (CType(dr("VERFDAT"), DateTime)).ToString("yyyyMMdd")
                    Newrow("ANZ_FAHRER") = dr("ANZ_FAHRER")


                    SapTable.Rows.Add(Newrow)

                    SapTable.AcceptChanges()

                Next


                'myProxy.callBapi()
                S.AP.Execute()

            Catch ex As Exception
                m_intStatus = -9999
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case Else
                        m_strMessage = "Beim Speichern des Reportes ist ein Fehler aufgetreten."
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: CK_01.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 13.01.10   Time: 11:45
' Updated in $/CKAG/Applications/AppKroschke/Lib
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 12.01.10   Time: 17:18
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA: 3332
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 14.10.08   Time: 13:07
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA: 2301 & Warnungen bereinigt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:30
' Created in $/CKAG/Applications/AppKroschke/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 18:01
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 9:25
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' 
' ************************************************
