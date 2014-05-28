Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class AvisChange06
    Inherits CKG.Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strAvisID As String
    Private m_strName As String
    Private m_strPLZ As String
    Private m_strOrt As String
    Private m_strMail As String
    Private m_strLoesch As String
#End Region

#Region " Properties"
    Public ReadOnly Property Kennung() As String
        Get
            Return "EINBAUFIRMA"
        End Get
    End Property
    Public Property Name() As String
        Get
            Return m_strName
        End Get
        Set(ByVal Value As String)
            m_strName = Value
        End Set
    End Property
    Public Property AvisID() As String
        Get
            Return m_strAvisID
        End Get
        Set(ByVal Value As String)
            m_strAvisID = Value
        End Set
    End Property
    Public Property PLZ() As String
        Get
            Return m_strPLZ
        End Get
        Set(ByVal Value As String)
            m_strPLZ = Value
        End Set
    End Property
    Public Property Ort() As String
        Get
            Return m_strOrt
        End Get
        Set(ByVal Value As String)
            m_strOrt = Value
        End Set
    End Property
    Public Property Mail() As String
        Get
            Return m_strMail
        End Get
        Set(ByVal Value As String)
            m_strMail = Value
        End Set
    End Property
    Public Property Loesch() As String
        Get
            Return m_strLoesch
        End Get
        Set(ByVal Value As String)
            m_strLoesch = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Function SaveFirmenDat() As DataTable

        Dim SAPTable As DataTable
        Dim SAPTableEx As DataTable

        Try
            S.AP.Init("Z_M_SAVE_AUFTRDAT_006", "I_KUNNR", m_objUser.KUNNR.ToSapKunnr())

            SAPTable = S.AP.GetImportTable("GT_WEB")

            Dim row As DataRow = SAPTable.NewRow

            row("KENNUNG") = Kennung
            row("POS_KURZTEXT") = m_strAvisID
            row("POS_TEXT") = m_strName & ";" & m_strOrt
            row("EMAIL") = m_strMail
            row("LOEVM") = m_strLoesch
            row("AENDT") = Now
            row("AENUS") = m_objUser.UserName
            SAPTable.Rows.Add(row)
            SAPTable.AcceptChanges()

            S.AP.Execute()

            SAPTableEx = S.AP.GetExportTable("GT_WEB")

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Speichern.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        End Try

        Return m_tblResult

    End Function

    Private Function CreateTableForOutput() As DataTable

        Dim tblTemp As New DataTable

        With tblTemp.Columns
            .Add("ID", System.Type.GetType("System.String"))
            .Add("Name", System.Type.GetType("System.String"))
            .Add("Ort", System.Type.GetType("System.String"))
            .Add("Mail1", System.Type.GetType("System.String"))
            .Add("Mail2", System.Type.GetType("System.String"))
            .Add("Mail3", System.Type.GetType("System.String"))
        End With

        Return tblTemp

    End Function

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "AvisChange06.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            Dim tblTemp2 As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_READ_AUFTRDAT_006.GT_WEB",
                                                            "I_KUNNR, I_POS_KURZTEXT",
                                                            m_objUser.KUNNR.ToSapKunnr(), Kennung)

            Dim row As DataRow
            Dim NewRow As DataRow
            m_tblResult = CreateTableForOutput()
            For Each row In tblTemp2.Rows
                NewRow = m_tblResult.NewRow

                NewRow("ID") = row("POS_KURZTEXT").ToString

                Dim splitarr() As String

                splitarr = Split(row("POS_TEXT").ToString, ";")
                Dim i As Integer
                For i = 0 To splitarr.Length - 1
                    If i = 0 Then NewRow("Name") = splitarr(i).ToString
                    If i = 1 Then NewRow("Ort") = splitarr(i).ToString
                Next
                splitarr = Split(row("EMAIL").ToString, ";")
                For i = 0 To splitarr.Length - 1
                    If i = 0 Then NewRow("Mail1") = splitarr(i).ToString
                    If i = 1 Then NewRow("Mail2") = splitarr(i).ToString
                    If i = 2 Then NewRow("Mail3") = splitarr(i).ToString
                Next
                m_tblResult.Rows.Add(NewRow)
            Next
            m_tblResult.AcceptChanges()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            m_blnGestartet = False
        End Try
    End Sub

#End Region
End Class
