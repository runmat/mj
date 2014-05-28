Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class Nacherfassung
    REM § Status-Report, Kunde: BPLG, BAPI: Z_M_Brief_Ohne_Daten,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

    Private mBrandings As DataTable

#End Region

#Region " Properties"

    Public ReadOnly Property Brandings() As DataTable
        Get
            If mBrandings Is Nothing Then
                fillBrandings()
            End If
            Return mBrandings
        End Get
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID)

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Nacherfassung.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.InitExecute("Z_M_Brief_Ohne_Daten_001", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                Dim tmpDataTable As DataTable = S.AP.GetExportTable("GT_WEB")

                CreateOutPut(tmpDataTable, strAppID)

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "NOT_FOUND"
                        m_intStatus = -1111
                        m_strMessage = "Keine Ergebnisse zu den Kriterien gefunden."
                    Case "NO_DATA"
                        m_intStatus = -12
                        m_strMessage = "Keine Dokumente gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

    Public Sub Change(ByRef sapTable As DataTable)
        m_strClassAndMethod = "Nacherfassung.Change"
        m_intStatus = 0
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim sZB As String = ""

            Try
                Dim dRow As DataRow
                Dim dRows() As DataRow

                dRows = sapTable.Select("Zuordnen=True")

                For Each dRow In dRows

                    S.AP.Init("Z_M_Daten_Anlage_001")

                    S.AP.SetImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                    S.AP.SetImportParameter("I_EQUNR", dRow("EQUNR").ToString)
                    S.AP.SetImportParameter("I_LIZNR", dRow("Lizenznr").ToString)
                    S.AP.SetImportParameter("I_HAENDLER", dRow("HaendlerNR").ToString)
                    S.AP.SetImportParameter("I_ENDKUNDE", dRow("EndkundenNummer").ToString())
                    S.AP.SetImportParameter("I_LABEL", dRow("Branding").ToString)
                    S.AP.SetImportParameter("I_UNAME", Left(m_objUser.UserName, 12))

                    S.AP.Execute()

                    Dim blnEndkundeNotFound As Boolean = (S.AP.GetExportParameter("ENDKUNDE_NOT_FOUND") = "X")
                    Dim blnHaendlerNotFound As Boolean = (S.AP.GetExportParameter("HAENDLER_NOT_FOUND") = "X")

                    If blnEndkundeNotFound Then
                        dRow("Status") = "Endkundennummer nicht gefunden"
                    ElseIf blnHaendlerNotFound Then
                        dRow("Status") = "Händlernummer nicht gefunden"
                    Else
                        dRow("Status") = "Vorgang Ok"
                    End If

                    sapTable.AcceptChanges()
                Next

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "HAENDLER_NOT_FOUND"
                        m_intStatus = -1111
                        m_strMessage = "Händler nicht gefunden.(" & sZB & ")"
                    Case "NO_EQUI"
                        m_intStatus = -12
                        m_strMessage = "Fehler beim Speichern der Daten(" & sZB & ").<br>(" & ex.Message & ")"
                    Case "ERROR_EQUI_UPDATE"
                        m_intStatus = -2222
                        m_strMessage = "Fehler beim Speichern der Daten(" & sZB & ").<br>(" & ex.Message & ")"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Fehler beim Speichern der Daten(" & sZB & ").<br>(" & ex.Message & ")"
                End Select

            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

    Public Sub fillBrandings()
        m_strClassAndMethod = "Nacherfassung.fillBrandings"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try

                S.AP.InitExecute("Z_V_ZDAD_LABEL_001", "I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                mBrandings = S.AP.GetExportTable("GT_WEB")

                mBrandings.Columns.Add("CMBText", System.Type.GetType("System.String"))

                For Each tmpRow As DataRow In mBrandings.Rows
                    tmpRow.Item("CMBText") = tmpRow("BRANDING").ToString & " - " & tmpRow("BEZEICHNUNG_1").ToString
                Next
                mBrandings.AcceptChanges()

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select

            Finally

                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region
End Class

' ************************************************
' $History: Nacherfassung.vb $
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 24.09.08   Time: 16:29
' Updated in $/CKAG/Applications/AppBPLG/Lib
' SAP Logging ergänzt
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 5.08.08    Time: 8:59
' Updated in $/CKAG/Applications/AppBPLG/Lib
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 30.07.08   Time: 16:43
' Updated in $/CKAG/Applications/AppBPLG/Lib
' logging nachgebessert
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 30.07.08   Time: 16:21
' Updated in $/CKAG/Applications/AppBPLG/Lib
' Benutzerlogging hinzugefügt
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 30.07.08   Time: 16:16
' Updated in $/CKAG/Applications/AppBPLG/Lib
' Nachbesserung BPLG
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 18.07.08   Time: 10:36
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2069 fertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 17.07.08   Time: 15:52
' Updated in $/CKAG/Applications/AppBPLG/Lib
' killAllDBNullValuesInDataTable methode hinzugefügt
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.07.08   Time: 12:39
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2069
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.07.08   Time: 12:33
' Created in $/CKAG/Applications/AppBPLG/Lib
' Erstellung ITA 2069
' 
' ************************************************
