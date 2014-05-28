Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class AvisChange05
    Inherits CKG.Base.Business.DatenimportBase
#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub


    Public Function SearchABMDat(ByVal FIN As String, ByVal Kennzeichen As String, ByVal MvaNr As String) As DataTable

        Try

            m_tblResult = S.AP.GetExportTableWithInitExecute("Z_M_READ_ABMDAT_FZG_001.GT_WEB",
                                                                           "I_KUNNR_AG, I_CHASSIS_NUM, I_MVA_NUMMER, I_LICENSE_NUM",
                                                                           m_objUser.KUNNR.ToSapKunnr(), FIN, MvaNr, Kennzeichen)

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Daten.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        End Try

        Return m_tblResult

    End Function


    Public Function GetImportTableForSave() As DataTable

        Return S.AP.GetImportTableWithInit("Z_M_SAVE_ABMDAT_FZG_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

    End Function

    Public Function SaveABMDat() As DataTable

        Try

            S.AP.Execute()
            Dim SAPTableEx As DataTable = S.AP.GetExportTable("GT_WEB")

            m_tblResult = SAPTableEx

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

#End Region
End Class
