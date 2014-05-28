Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

<Serializable()> Public Class Briefversand
    REM §  BAPI: Z_M_Gesperrte_Auftraege_001
    REM §  BAPI:Z_M_Freigeben_Auftrag_001
    Inherits Base.Business.BankBase

#Region " Declarations"
    Dim mDatumAb As String = ""
    Dim mDatumBis As String = ""
    Dim mID_LIZNR As String = ""
    Dim mBriefversandFehlerTabelle As DataTable
    Dim mSapTable As DataTable
    
#End Region

#Region " Properties"

    Public Property DatumAb() As String
        Get
            Return mDatumAb
        End Get
        Set(ByVal Value As String)
            mDatumAb = Value
        End Set
    End Property

    Public ReadOnly Property BriefversandFehlerTabelle() As DataTable
        Get
            Return mBriefversandFehlerTabelle
        End Get
    End Property

    Public ReadOnly Property SapTabelle() As DataTable
        Get
            Return mSapTable
        End Get
    End Property

    Public Property ID_LIZNR() As String
        Get
            Return mID_LIZNR
        End Get
        Set(ByVal Value As String)
            mID_LIZNR = Value
        End Set
    End Property

    Public Property DatumBis() As String
        Get
            Return mDatumBis
        End Get
        Set(ByVal Value As String)
            mDatumBis = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String,
                    ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overloads Overrides Sub show()
        show(AppID, SessionID)
    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Briefversand.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            ClearError()

            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                S.AP.InitExecute("Z_M_BRIEFANF_ERROR_LIST", "AG, VON, BIS, ID_LIZNR",
                          Right("0000000000" & m_objUser.KUNNR, 10), DatumAb, DatumBis, mID_LIZNR)

                Dim tblTemp As DataTable = S.AP.GetExportTable("GT_ERROR")
                HelpProcedures.killAllDBNullValuesInDataTable(tblTemp)
                mSapTable = tblTemp 'ist die tabelle die auch wieder ins sap zurück geschrieben wird

                mBriefversandFehlerTabelle = CreateOutPut(tblTemp, strAppID) 'ist die tabelle die fürs grid genutzt wird. 
                mBriefversandFehlerTabelle.Columns.Add("Index", String.Empty.GetType)

                For Each tmprow As DataRow In mBriefversandFehlerTabelle.Rows
                    tmprow("Index") = mBriefversandFehlerTabelle.Rows.IndexOf(tmprow)
                    Select Case tmprow("Versandart").ToString
                        Case "2"
                            tmprow("Versandart") = "endgültig"
                        Case Else
                            tmprow("Versandart") = "unbekannt"
                    End Select
                Next

                mBriefversandFehlerTabelle.AcceptChanges()

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")
                    Case "NO_DATE_FROM"
                        RaiseError("-9999", "Kein von-Datum angegeben")
                    Case "NO_AG"
                        RaiseError("-9999", "Keine Kundennummer")
                    Case "NO_DATA"
                        RaiseError("-1111", "Es wurden keine Daten gefunden")
                    Case Else
                        RaiseError("-9999", "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")")
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub change()
        change(AppID, SessionID)
    End Sub

    Public Overloads Sub change(ByVal strAppID As String, ByVal strSessionID As String)

        m_strClassAndMethod = "Briefversand.change"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            ClearError()

            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                S.AP.InitExecute("Z_M_BRIEFANF_ERROR_DEL")

                Dim strAnz As String = S.AP.GetExportParameter("ANZ")
                Dim strDel As String = S.AP.GetExportParameter("DEL")

                'auswerten der exportparameter
                If Not String.IsNullOrEmpty(strAnz) AndAlso Not String.IsNullOrEmpty(strDel) Then
                    m_strMessage = "Es wurden " & strDel & " von " & strAnz & " erfolgreich gelöscht."
                Else
                    RaiseError("-9998", "Fehler: es konnten nicht alle Löschungen verarbeitet werden. ")
                End If

            Catch ex As Exception
                RaiseError("-9999", "Beim Löschen der Aufträge ist ein Fehler aufgetreten: " & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: Briefversand.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 9.11.10    Time: 10:01
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 19.12.08   Time: 11:03
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2500 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.09.08   Time: 10:42
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2079 fertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 28.07.08   Time: 15:12
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2079 fast fertig
'
' ************************************************