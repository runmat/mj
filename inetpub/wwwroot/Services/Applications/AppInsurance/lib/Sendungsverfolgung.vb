Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports Microsoft.Data.SAPClient
Imports CKG.Base.Common

Public Class Sendungsverfolgung
    Inherits BankBase

    Private m_DatumVon As String
    Private m_DatumBis As String
    Private m_Agentur As String
    Private m_SendungsDaten As DataTable
    Private mE_SUBRC As String
    Private mE_MESSAGE As String


    Public Property DatumVon() As String
        Get
            Return m_DatumVon
        End Get
        Set(ByVal Value As String)
            m_DatumVon = Value
        End Set
    End Property

    Public Property DatumBis() As String
        Get
            Return m_DatumBis
        End Get
        Set(ByVal Value As String)
            m_DatumBis = Value
        End Set
    End Property

    Public Property Agentur() As String
        Get
            Return m_Agentur
        End Get
        Set(ByVal Value As String)
            m_Agentur = Value
        End Set
    End Property

    Public Property SendungsDaten() As DataTable
        Get
            Return m_SendungsDaten
        End Get
        Set(ByVal Value As DataTable)
            m_SendungsDaten = Value
        End Set
    End Property

    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub

    Public Overloads Overrides Sub show()
        'nur wegen bankbase
    End Sub

    Public Overloads Overrides Sub Change()
        'nur wegen bankbase
    End Sub

    Public Sub GetData(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strClassAndMethod = "Sendungsverfolgung.GetData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = m_objUser.Customer.KUNNR.PadLeft(10, "0"c)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_MOFA_01", m_objApp, m_objUser, page)


                myProxy.setImportParameter("AG", strKUNNR)
                myProxy.setImportParameter("AB_ERDAT", m_DatumVon.ToString)
                myProxy.setImportParameter("BIS_ERDAT", m_DatumBis.ToString)
                myProxy.setImportParameter("AGENTUR", m_Agentur.ToString)

                myProxy.callBapi()

                m_SendungsDaten = myProxy.getExportTable("GT_OUT")


                ResultExcel = New DataTable

                With ResultExcel.Columns
                    .Add("Agenturnr.", GetType(System.String))
                    .Add("Name1", GetType(System.String))
                    .Add("Straße", GetType(System.String))
                    .Add("Nummer", GetType(System.String))
                    .Add("PLZ", GetType(System.String))
                    .Add("Ort", GetType(System.String))
                    .Add("beauftragt am", GetType(System.String))
                    .Add("versendet am", GetType(System.String))
                    .Add("versendet um", GetType(System.String))
                    .Add("Sendungsnummer", GetType(System.String))
                End With
                m_SendungsDaten.Columns("PACKZEIT").MaxLength = 8
                m_SendungsDaten.Columns("AEZEIT").MaxLength = 8
                For Each LastOrderRow As DataRow In m_SendungsDaten.Rows
                    Dim NewRow As DataRow = ResultExcel.NewRow
                    NewRow("Agenturnr.") = LastOrderRow("AGENTUR")


                    NewRow("Name1") = LastOrderRow("NAME1")
                    NewRow("Straße") = LastOrderRow("STREET")
                    NewRow("Nummer") = LastOrderRow("HOUSE_NUM1")
                    NewRow("PLZ") = LastOrderRow("POST_CODE1")
                    NewRow("Ort") = LastOrderRow("CITY1")
                    NewRow("beauftragt am") = LastOrderRow("AEDAT")
                    If IsDate(LastOrderRow("AEDAT").ToString) Then
                        NewRow("beauftragt am") = CDate(LastOrderRow("AEDAT")).ToShortDateString
                    End If

                    NewRow("versendet am") = LastOrderRow("PACKDAT")
                    If IsDate(LastOrderRow("PACKDAT").ToString) Then
                        NewRow("versendet am") = CDate(LastOrderRow("PACKDAT")).ToShortDateString
                    End If
                    If Not LastOrderRow("PACKZEIT").ToString = "000000" Then
                        LastOrderRow("PACKZEIT") = LastOrderRow("PACKZEIT").ToString.Substring(0, 2) & ":" & LastOrderRow("PACKZEIT").ToString.Substring(2, 2) & ":" & LastOrderRow("PACKZEIT").ToString.Substring(4, 2)
                    Else
                        LastOrderRow("PACKZEIT") = ""
                    End If
                    NewRow("versendet um") = LastOrderRow("PACKZEIT")
                    NewRow("Sendungsnummer") = LastOrderRow("ZZTRACK")

                    ResultExcel.Rows.Add(NewRow)
                    ResultExcel.AcceptChanges()
                Next



                If mE_SUBRC <> "0" Then
                    m_intStatus = CInt(mE_SUBRC)
                    m_strMessage = mE_MESSAGE
                End If

            Catch ex As Exception
                m_strMessage = "Keine Daten gefunden."
            Finally
                m_blnGestartet = False
            End Try
        End If


    End Sub
End Class
