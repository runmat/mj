Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class Vertragsdatenaenderung
    REM § Status-Report, Kunde: BPLG, BAPI: Z_M_Brief_Ohne_Daten,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

    Private mBrandings As DataTable
    Private mSucheLizNr As String

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

    Public Property SucheLiznr() As String
        Get
            Return mSucheLizNr
        End Get
        Set(ByVal value As String)
            mSucheLizNr = value
        End Set
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
        m_strClassAndMethod = "Vertragsdatenaenderung.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.InitExecute("Z_M_DATEN_ZUM_VERTRAG_001", "I_AG,I_LIZNR", Right("0000000000" & m_objUser.KUNNR, 10), SucheLiznr)

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
        m_strClassAndMethod = "Vertragsdatenaenderung.Change"
        m_intStatus = 0
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim sZB As String = ""

            Try

                For Each dRow As DataRow In sapTable.Rows

                    S.AP.Init("Z_M_Daten_Anlage_001")

                    S.AP.SetImportParameter("I_HAENDLER", dRow("HaendlerNR").ToString)
                    S.AP.SetImportParameter("I_LIZNR", dRow("Lizenznr").ToString)
                    S.AP.SetImportParameter("I_LABEL", dRow("Branding").ToString)
                    S.AP.SetImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                    S.AP.SetImportParameter("I_ENDKUNDE", dRow("EndkundenNummer").ToString)
                    S.AP.SetImportParameter("I_UNAME", Left(m_objUser.UserName, 12))
                    S.AP.SetImportParameter("I_EQUNR", dRow("EQUNR").ToString)

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
        m_strClassAndMethod = "Vertragsdatenaenderung.fillBrandings"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try

                S.AP.InitExecute("Z_V_ZDAD_LABEL_001", "I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                mBrandings = S.AP.GetExportTable("GT_WEB")

                mBrandings.Columns.Add("CMBText", Type.GetType("System.String"))

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
' $History: Vertragsdatenaenderung.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.08.08   Time: 17:54
' Updated in $/CKAG/Applications/AppBPLG/Lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 23.07.08   Time: 9:57
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2101 testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 22.07.08   Time: 14:32
' Created in $/CKAG/Applications/AppBPLG/Lib
' ITA 2101 Rohversion
' 
' ************************************************
