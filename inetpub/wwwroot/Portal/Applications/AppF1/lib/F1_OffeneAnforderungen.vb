Option Explicit On
Option Strict On

Imports System
Imports CKG.Base
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class F1_OffeneAnforderungen
    Inherits BankBase

#Region " Declarations"
    Private m_strHaendler As String
    Private m_strHaendlerGruppe As String
    Private m_tblAuftraege As DataTable
    Private m_strAuftragsNummer As String
    Private m_strEqui As String
    Private m_tblRaw As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property Auftraege() As DataTable
        Get
            Return m_tblAuftraege
        End Get
    End Property

    Public Property AuftragsNummer() As String
        Get
            Return m_strAuftragsNummer
        End Get
        Set(ByVal Value As String)
            m_strAuftragsNummer = Right("0000000000" & Value, 10)
        End Set
    End Property

    Public Property Haendler() As String
        Get
            Return m_strHaendler
        End Get
        Set(ByVal Value As String)
            m_strHaendler = Value
        End Set
    End Property
    Public Property HaendlerGruppe() As String
        Get
            Return m_strHaendlerGruppe
        End Get
        Set(ByVal Value As String)
            m_strHaendlerGruppe = Value
        End Set
    End Property
    Public Property Equinr() As String
        Get
            Return m_strEqui
        End Get
        Set(ByVal Value As String)
            m_strEqui = Value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As User, ByRef objApp As App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_strHaendler = ""
        m_hez = hez
    End Sub

#End Region


    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "F1_OffeneAnforderungen.Change"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True

            '1
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_OFFENE_ANFORDER_STORNO_STD", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_HAENDLER_EX", m_strHaendler)
                'myProxy.setImportParameter("I_EQUNR", m_strEqui)
                'myProxy.setImportParameter("I_VBELN", m_strAuftragsNummer)
                'myProxy.setImportParameter("I_ERNAM", Left(m_objUser.UserName, 12))


                ' Redundant? ^1
                'If m_objLogApp Is Nothing Then
                '    m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                'End If
                '------------
                m_intIDSAP = -1

                m_intStatus = 0
                m_strMessage = ""

                'myProxy.callBapi()
                S.AP.InitExecute("Z_M_OFFENE_ANFORDER_STORNO_STD", "I_AG,I_HAENDLER_EX,I_EQUNR,I_VBELN,I_ERNAM",
                                 m_strKUNNR, m_strHaendler, m_strEqui, m_strAuftragsNummer, Left(m_objUser.UserName, 12))

                WriteLogEntry(True, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KUNNR=" & Right(m_strHaendler, 5), Nothing)

            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_UPDATE"
                        m_intStatus = -3501
                        m_strMessage = "Kein EQUI-UPDATE."
                    Case "NO_ZDADVERSAND"
                        m_intStatus = -3502
                        m_strMessage = "Kein ZDADVERSAND-STORNO."
                    Case "NO_UPDATE_SALESDOCUMENT"
                        m_intStatus = -3503
                        m_strMessage = "Keine Auftragsänderung."
                    Case "ZVERSAND_SPERRE"
                        m_intStatus = -3504
                        m_strMessage = "ZVERSAND vom DAD gesperrt."
                    Case "NO_PICKLISTE"
                        m_intStatus = -3505
                        m_strMessage = "Kein Picklisteneintrag gefunden."
                    Case "NO_ZCREDITCONTROL"
                        m_intStatus = -3506
                        m_strMessage = "Kein Creditcontroleintrag gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select

                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ",   " & Replace(m_strMessage, "<br>", " "), Nothing)

            End Try

        End If
        m_blnGestartet = False
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String)

        m_strClassAndMethod = "F1_OffeneAnforderungen.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_OFFENE_ANFORDERUNGEN_STD", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_HDGRP", m_strHaendlerGruppe)
                'myProxy.setImportParameter("I_HAENDLER_EX", m_strHaendler)
                'myProxy.setImportParameter("I_KKBER", "")

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_OFFENE_ANFORDERUNGEN_STD", "I_AG,I_HDGRP,I_HAENDLER_EX,I_KKBER",
                                 m_strKUNNR, m_strHaendlerGruppe, m_strHaendler, "")

                m_tblRaw = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")


                m_tblRaw.Columns.Add("KONTART", System.Type.GetType("System.String"))
                m_tblRaw.AcceptChanges()

                Dim rowTemp As DataRow
                For Each rowTemp In m_tblRaw.Rows
                    Select Case rowTemp("ZZKKBER").ToString
                        Case "0001"
                            rowTemp("KONTART") = "Standard temporär"
                        Case "0002"
                            rowTemp("KONTART") = "Standard endgültig"
                        Case "0003"
                            rowTemp("KONTART") = "Retail"
                        Case "0004"
                            rowTemp("KONTART") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                        Case "0005"
                            rowTemp("KONTART") = "Händlereigene Zulassung"
                        Case "0006"
                            rowTemp("KONTART") = "KF/KL"
                    End Select

                    If CStr(rowTemp("Status")) = "G" Then
                        rowTemp("Status") = "X"
                    Else
                        rowTemp("Status") = ""
                    End If
                Next

                m_tblRaw.AcceptChanges()
                m_tblAuftraege = CreateOutPut(m_tblRaw, m_strAppID)

                If m_tblAuftraege.Rows.Count = 0 Then
                    m_intStatus = 0
                    m_strMessage = "Keine Daten gefunden."
                End If

                If m_strHaendler.Length > 0 Then
                    Haendler = m_strHaendler
                End If

            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "EXCEPTION NO_DATA RAISED"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_DATA"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select

                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ",   " & Replace(m_strMessage, "<br>", " "), Nothing)

            End Try

        End If

        m_blnGestartet = False
    End Sub

End Class
