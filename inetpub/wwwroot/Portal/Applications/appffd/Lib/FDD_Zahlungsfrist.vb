Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

' Anfroderungsnummer 968
' Erstellt am: 17.04.2007 - Tim Bardenhagen
<Serializable()> _
Public Class FDD_Zahlungsfrist
    Inherits Base.Business.BankBase
    REM § Lese-/Schreibfunktion, Kunde: FFD,
    REM § Show - BAPI: Z_V_ZAHLUNGSFRIST_GET,
    REM § Change - BAPI: Z_V_ZAHLUNGSFRIST_PUT.

#Region "Declarations"

    Public Const COLUMN_KONTINGENTART As String = "Kontingentart"
    Public Const COLUMN_KONTINGENTID As String = "KontingentID"
    Public Const COLUMN_ZAHLUNGSFRIST_ALT As String = "Alte Zahlungsfrist"
    Public Const COLUMN_ZAHLUNGSFRIST_NEU As String = "Neue Zahlungsfrist"

    Private m_dtZahlungsfrist As DataTable
    Private m_Haendler As DataTable
    Private m_RowFaelligkeit As Integer
    Private objFDDBank As Base.Business.BankBaseCredit
#End Region

#Region "Properties"

    Public Property Zahlungsfristen() As DataTable
        Get
            Return Me.m_dtZahlungsfrist
        End Get
        Set(ByVal Value As DataTable)
            Me.m_dtZahlungsfrist = Value
        End Set
    End Property

    Public Property RowFaelligkeit() As Integer
        Get
            Return Me.m_RowFaelligkeit
        End Get
        Set(ByVal Value As Integer)
            Me.m_RowFaelligkeit = Value
        End Set
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        'DataTable anlegen
        Me.m_dtZahlungsfrist = New DataTable()
        With Me.m_dtZahlungsfrist.Columns
            .Add(COLUMN_KONTINGENTID)
            .Add(COLUMN_KONTINGENTART)
            .Add(COLUMN_ZAHLUNGSFRIST_ALT)
            .Add(COLUMN_ZAHLUNGSFRIST_NEU)
        End With

    End Sub

#End Region

#Region "Methods"

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_intIDSAP = -1

        Try
            m_intStatus = 0
            m_strMessage = ""

            Dim out_zahlungsfrist As String = ""
            'DataTable leeren (falls gefüllt)
            Me.m_dtZahlungsfrist.Clear()
            'Neue Row hinzufügen
            Dim i As Integer
            Dim sKKBer As String


            Dim row As DataRow = Me.m_dtZahlungsfrist.NewRow()

            For i = 1 To 4
                Try
                    sKKBer = "000" & i

                    m_intStatus = 0
                    m_strMessage = ""
                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Zahlungsfrist_Get", m_objApp, m_objUser, page)

                    'myProxy.setImportParameter("AG", m_strKUNNR)
                    'myProxy.setImportParameter("KUNNR", m_strCustomer)
                    'myProxy.setImportParameter("KKBER", sKKBer)
                    'myProxy.setImportParameter("ZZVGRUND", "")


                    'myProxy.callBapi()

                    S.AP.InitExecute("Z_V_Zahlungsfrist_Get", "AG,KUNNR,KKBER,ZZVGRUND", m_strKUNNR, m_strCustomer, sKKBer, "")

                    out_zahlungsfrist = S.AP.GetExportParameter("ZZFRIST") 'myProxy.getExportParameter("ZZFRIST")

                    row = Me.m_dtZahlungsfrist.NewRow()
                    Select Case i
                        Case 1
                            row(COLUMN_KONTINGENTART) = "Standard Temporär"
                            row(COLUMN_KONTINGENTID) = "0001"
                            row(COLUMN_ZAHLUNGSFRIST_ALT) = out_zahlungsfrist
                            Me.m_dtZahlungsfrist.Rows.Add(row)
                        Case 2
                            row(COLUMN_KONTINGENTART) = "Standard endgültig"
                            row(COLUMN_KONTINGENTID) = "0002"
                            row(COLUMN_ZAHLUNGSFRIST_ALT) = out_zahlungsfrist
                            Me.m_dtZahlungsfrist.Rows.Add(row)
                        Case 3
                            row(COLUMN_KONTINGENTART) = "Retail"
                            row(COLUMN_KONTINGENTID) = "0003"
                            row(COLUMN_ZAHLUNGSFRIST_ALT) = out_zahlungsfrist
                            Me.m_dtZahlungsfrist.Rows.Add(row)
                        Case 4
                            row(COLUMN_KONTINGENTART) = "Delayed payment"
                            row(COLUMN_KONTINGENTID) = "0004"
                            row(COLUMN_ZAHLUNGSFRIST_ALT) = out_zahlungsfrist
                            Me.m_dtZahlungsfrist.Rows.Add(row)
                    End Select

                Catch ex As Exception
                    Select Case ex.Message
                        Case "NRF"
                           
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = ex.Message
                    End Select
                End Try
            Next

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
            End If

            WriteLogEntry(True, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=", Nothing)
        Catch ex As Exception
            Select Case ex.Message
                Case "NRF"
                    m_intStatus = -1402
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If

            WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), Nothing)

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
            End If

        End Try

    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_intIDSAP = -1

        Try
            m_intStatus = 0
            m_strMessage = ""

            If Me.m_dtZahlungsfrist.Rows.Count = 0 Then
                Throw New Exception("Es sind keine Daten zum Speichern vorhanden.")
            End If

            Dim sKKBER As String = Me.m_dtZahlungsfrist.Rows(Me.m_RowFaelligkeit)(COLUMN_KONTINGENTID).ToString()
            Dim sZfrist As String = Me.m_dtZahlungsfrist.Rows(Me.m_RowFaelligkeit)(COLUMN_ZAHLUNGSFRIST_NEU).ToString()

            Me.m_dtZahlungsfrist.Rows(Me.m_RowFaelligkeit)(COLUMN_ZAHLUNGSFRIST_ALT) = Me.m_dtZahlungsfrist.Rows(Me.m_RowFaelligkeit)(COLUMN_ZAHLUNGSFRIST_NEU).ToString()

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Zahlungsfrist_Put", m_objApp, m_objUser, page)


            'myProxy.setImportParameter("KUNNR", Me.m_strCustomer)
            'myProxy.setImportParameter("KKBER", sKKBER)
            'myProxy.setImportParameter("ZZFRIST", sZfrist)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_V_Zahlungsfrist_Put", "KUNNR,KKBER,ZZFRIST", Me.m_strCustomer, sKKBER, sZfrist)

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NRF"
                    m_intStatus = -1402
                    m_strMessage = "Keine Daten gefunden."
                Case "WERROR"
                    m_intStatus = -9998
                    m_strMessage = "Fehler beim Speichern"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If
        End Try

    End Sub


#End Region

End Class

' ************************************************
' $History: FDD_Zahlungsfrist.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.06.09   Time: 17:08
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.06.07   Time: 17:03
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Portal - Startapplication 13.06.2005
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Beyond Compare
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 8.06.07    Time: 11:26
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
