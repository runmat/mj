Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Kernel

Namespace Kernel
    Public Class Change_04
        Inherits Base.Business.DatenimportBase

        Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
            MyBase.New(objUser, objApp, strFilename)
        End Sub

        Public Function FillTeam(ByVal strAppID As String, ByVal strSessionID As String) As DataTable
            m_strClassAndMethod = "Change_04.FillTeam"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Dim tmpDataTable As New DataTable

            If Not m_blnGestartet Then
                m_blnGestartet = True
                m_intStatus = 0

                Try
                    S.AP.InitExecute("Z_M_GET_LN_TEAM", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                    tmpDataTable = S.AP.GetExportTable("ET_TEAM")

                    WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, tmpDataTable, False)

                Catch ex As Exception
                    m_intStatus = -5555
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case Else
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Select
                    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), tmpDataTable, False)

                Finally
                    m_blnGestartet = False
                End Try
            End If

            Return tmpDataTable

        End Function

        Public Sub FillLNKundendaten(ByVal strAppID As String, ByVal strSessionID As String, ByVal strSucheKunnr As String,
                                     ByRef tblStammdaten As DataTable, ByRef tblAnsprechpartner As DataTable, ByRef tblTeam As DataTable,
                                     ByRef tblVersandbedingungen As DataTable)
            m_strClassAndMethod = "Change_04.FillLNKundendaten"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            If Not m_blnGestartet Then
                m_blnGestartet = True
                m_intStatus = 0

                Try
                    S.AP.InitExecute("Z_M_GET_LN_KUNDE", "I_AG, I_EXKUNNR_ZL",
                                     Right("0000000000" & m_objUser.KUNNR, 10), strSucheKunnr)

                    tblStammdaten = S.AP.GetExportTable("ES_LN_KUNDE")
                    tblAnsprechpartner = S.AP.GetExportTable("ET_ANSPP")
                    tblTeam = S.AP.GetExportTable("ET_TEAM")
                    tblVersandbedingungen = S.AP.GetExportTable("ET_VSBD")

                    WriteLogEntry(True, "AG=" & m_objUser.KUNNR & ", EXKUNNR_ZL=" & strSucheKunnr, tblStammdaten, False)

                Catch ex As Exception
                    m_intStatus = -5555
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case Else
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Select
                    WriteLogEntry(False, "AG=" & m_objUser.KUNNR & ", EXKUNNR_ZL=" & strSucheKunnr & ", " & Replace(m_strMessage, "<br>", " "), tblStammdaten, False)

                Finally
                    m_blnGestartet = False
                End Try
            End If

        End Sub

        Public Sub SaveLNKundendaten(ByVal strAppID As String, ByVal strSessionID As String, ByVal tblStammdaten As DataTable,
                                     ByVal tblAnsprechpartner As DataTable, ByVal tblVersandbedingungen As DataTable)
            m_strClassAndMethod = "Change_04.SaveLNKundendaten"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            If Not m_blnGestartet Then
                m_blnGestartet = True
                m_intStatus = 0

                Try
                    S.AP.Init("Z_M_PUT_LN_KUNDE", "I_USER", m_objUser.UserName)

                    Dim tblTmpStammdaten As DataTable = S.AP.GetImportTable("IS_KUNDE")
                    Dim tblTmpAnsprechpartner As DataTable = S.AP.GetImportTable("ET_ANSPP")
                    Dim tblTmpVersandbedingungen As DataTable = S.AP.GetImportTable("ET_VSBD")

                    For Each stdaRow As DataRow In tblStammdaten.Rows
                        Dim newStdaRow As DataRow = tblTmpStammdaten.NewRow()
                        newStdaRow.ItemArray = CType(stdaRow.ItemArray.Clone(), Object())
                        tblTmpStammdaten.Rows.Add(newStdaRow)
                    Next

                    For Each ansppRow As DataRow In tblAnsprechpartner.Rows
                        Dim newAnsppRow As DataRow = tblTmpAnsprechpartner.NewRow()
                        newAnsppRow.ItemArray = CType(ansppRow.ItemArray.Clone(), Object())
                        tblTmpAnsprechpartner.Rows.Add(newAnsppRow)
                    Next

                    For Each versRow As DataRow In tblVersandbedingungen.Rows
                        Dim newVersRow As DataRow = tblTmpVersandbedingungen.NewRow()
                        newVersRow.ItemArray = CType(versRow.ItemArray.Clone(), Object())
                        tblTmpVersandbedingungen.Rows.Add(newVersRow)
                    Next

                    S.AP.Execute()

                    WriteLogEntry(True, "USER=" & m_objUser.UserName, tblStammdaten, False)

                Catch ex As Exception
                    m_intStatus = -5555
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case Else
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Select
                    WriteLogEntry(False, "USER=" & m_objUser.UserName & ", " & Replace(m_strMessage, "<br>", " "), tblStammdaten, False)

                Finally
                    m_blnGestartet = False
                End Try
            End If

        End Sub

    End Class
End Namespace

