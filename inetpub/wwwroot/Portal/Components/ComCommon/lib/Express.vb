Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

<Serializable()> _
Public Class Express
    Inherits Base.Business.BankBase

    Public Property TableVersendungen As DataTable
    Public Property TableVersandtexte As DataTable
    Public Property DatumAb As String
    Public Property DatumBis As String
    Public Property Fahrgestellnummer As String
    Public Property Kennzeichen As String
    Public Property Name1 As String
    Public Property Name2 As String
    Public Property Strasse As String
    Public Property PLZ As String
    Public Property Ort As String

#Region "Constructor"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub
#End Region

    Public Overrides Sub Change()

    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Sub GetVersendungen(ByVal page As System.Web.UI.Page)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                S.AP.Init("Z_DPM_READ_SENDTAB_01")
                S.AP.SetImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, "0"c))
                S.AP.SetImportParameter("I_ZZLSDAT_VON", DatumAb)
                S.AP.SetImportParameter("I_ZZLSDAT_BIS", DatumBis)
                S.AP.SetImportParameter("I_NAME1", Name1)
                S.AP.SetImportParameter("I_NAME2", Name2)
                S.AP.SetImportParameter("I_STRAS", Strasse)
                S.AP.SetImportParameter("I_PSTLZ", PLZ)
                S.AP.SetImportParameter("I_CITY1", Ort)
                S.AP.SetImportParameter("I_LICENSE", Kennzeichen)
                S.AP.SetImportParameter("I_CHASSIS", Fahrgestellnummer)
                S.AP.Execute()

                TableVersendungen = S.AP.GetExportTable("GT_OUT")
                TableVersendungen.Columns.Add("Adresse", GetType(String))
                TableVersendungen.AcceptChanges()

                For Each dr As DataRow In TableVersendungen.Rows
                    dr("Adresse") = dr("NAME1").ToString & " " & dr("NAME2").ToString & ", " & dr("STRAS").ToString & " " & dr("HSNM1").ToString & ", " & dr("PSTLZ").ToString & " " & dr("CITY1").ToString
                    If dr("ZZLSDAT") IsNot DBNull.Value Then
                        dr("ZZLSDAT") = Left(dr("ZZLSDAT").ToString, 10)
                    End If

                    If dr("ZZLSTIM").ToString <> "" Then
                        dr("ZZLSTIM") = dr("ZZLSTIM").ToString.Remove(4).Insert(2, ":")
                    End If
                Next

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

    Public Sub GetVersandtexte(ByVal page As System.Web.UI.Page)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                S.AP.InitExecute("Z_DPM_READ_VERSCODE_01")

                TableVersandtexte = S.AP.GetExportTable("GT_OUT")

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

End Class
