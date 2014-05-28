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
    Public Property Name1 As String
    Public Property Name2 As String
    Public Property Strasse As String
    Public Property PLZ As String
    Public Property Ort As String
    Public Property Fahrgestellnummer As String
    Public Property Kennzeichen As String

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

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_SENDTAB_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_ZZLSDAT_VON", DatumAb)
                myProxy.setImportParameter("I_ZZLSDAT_BIS", DatumBis)
                myProxy.setImportParameter("I_NAME1", Name1)
                myProxy.setImportParameter("I_NAME2", Name2)
                myProxy.setImportParameter("I_STRAS", Strasse)
                myProxy.setImportParameter("I_PSTLZ", PLZ)
                myProxy.setImportParameter("I_CITY1", Ort)
                myProxy.setImportParameter("I_LICENSE", Kennzeichen)
                myProxy.setImportParameter("I_CHASSIS", Fahrgestellnummer)

                myProxy.callBapi()

                TableVersendungen = myProxy.getExportTable("GT_OUT")
                TableVersendungen.Columns.Add("Adresse", GetType(String))
                TableVersendungen.AcceptChanges()

                For Each dr As DataRow In TableVersendungen.Rows
                    dr("Adresse") = dr("NAME1").ToString & " " & dr("NAME2").ToString & ", <br/>" & dr("STRAS").ToString & " " & dr("HSNM1").ToString & ", <br/>" & dr("PSTLZ").ToString & " " & dr("CITY1").ToString
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

    ''' <summary>
    ''' Liefert den Status einer Versendung zur Poolnummer
    ''' </summary>
    ''' <param name="Poolnummer">Poolnummer</param>
    ''' <param name="page">Page-Objekt</param>
    ''' <returns>Statustabelle</returns>
    Public Function GetVersendungsStatus(ByVal Poolnummer As String, ByVal page As System.Web.UI.Page) As DataTable
        Dim TableStatus As DataTable = Nothing

        Try
            m_intStatus = 0
            m_strMessage = ""

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_GET_SEND_STATUS", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_POOLNR", Poolnummer)

            myProxy.callBapi()

            TableStatus = myProxy.getExportTable("GT_WEB")
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = ex.Message
        Finally
            m_blnGestartet = False
        End Try

        Return TableStatus
    End Function

    Public Sub GetVersandtexte(ByVal page As System.Web.UI.Page)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_VERSCODE_01", m_objApp, m_objUser, page)

                myProxy.callBapi()

                TableVersandtexte = myProxy.getExportTable("GT_OUT")

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message

            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

End Class
