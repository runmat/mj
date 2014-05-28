Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class kruell_02
    Inherits Base.Business.DatenimportBase
    Private strfahrgestellnummer As String
    Private strordernummer As String
    Private strDatumAnlageVon As String
    Private strDatumAnlageBis As String
    Private strdatumStornierungVon As String
    Private strdatumStornierungBis As String
    Private intWiederhergestellte As Int32
    Private strFehlschlaege As String

#Region " Properties "

    Public Property AnlageDatumVon() As String
        Get
            Return strDatumAnlageVon
        End Get
        Set(ByVal value As String)
            strDatumAnlageVon = value
        End Set
    End Property

    Public Property Wiederherstellungen() As Integer
        Get
            Return intWiederhergestellte
        End Get
        Set(ByVal value As Integer)
            intWiederhergestellte = value
        End Set
    End Property



    Public Property AnlageDatumBis() As String
        Get
            Return strDatumAnlageBis
        End Get
        Set(ByVal value As String)
            strDatumAnlageBis = value
        End Set
    End Property

    Public Property FehlgeschlageneOrdernummern() As String
        Get
            Return strFehlschlaege
        End Get
        Set(ByVal value As String)
            strFehlschlaege = value
        End Set
    End Property


    Public Property StornierDatumVon() As String
        Get
            Return strdatumStornierungVon
        End Get
        Set(ByVal value As String)
            strdatumStornierungVon = value
        End Set
    End Property


    Public Property StornierDatumBis() As String
        Get
            Return strdatumStornierungBis
        End Get
        Set(ByVal value As String)
            strdatumStornierungBis = value
        End Set
    End Property


    Public Property fahrgestellnummer() As String
        Get
            Return strfahrgestellnummer
        End Get
        Set(ByVal value As String)
            strfahrgestellnummer = value
        End Set
    End Property

    Public Property Ordernummer() As String
        Get
            Return strordernummer
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                strordernummer = value.Trim(" "c).ToUpper
            End If
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

    Public Sub sendToSAP(ByVal alOrderNRs As ArrayList)
        sendToSAP(m_strAppID, m_strSessionID, alOrderNRs)
    End Sub

    Public Sub sendToSAP(ByVal strAppID As String, ByVal strSessionID As String, ByVal alOrderNRs As ArrayList)
        m_strClassAndMethod = "kruell_02.SendToSAP"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
        End If

        Dim objSAP As New SAPProxy_Kruell.SAPProxy_Kruell()

        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        objSAP.Connection.Open()

        Dim intID As Int32 = -1

        Try

            Dim SAPTabelleHin As New SAPProxy_Kruell.ZDAD_AUFTRAG_IDENT_TABELLE()
            Dim SAPTabelleZurueck As New SAPProxy_Kruell.ZDAD_AUFTRAG_IDENT_TABELLE()
            Dim sapTabellenZeile As New SAPProxy_Kruell.ZDAD_AUFTRAG_IDENT_ZEILE()
            Dim tmpOrderNR As String
         


            For Each tmpOrderNR In alOrderNRs
                sapTabellenZeile = New SAPProxy_Kruell.ZDAD_AUFTRAG_IDENT_ZEILE()
                sapTabellenZeile.Kunnr_Ag = Right("0000000000" & m_objUser.KUNNR, 10)
                sapTabellenZeile.Order_Nr = tmpOrderNR
                SAPTabelleHin.Add(sapTabellenZeile)
            Next


            objSAP.Z_V_Storno_Zurueck(SAPTabelleZurueck, SAPTabelleHin)
            'objSAP.Z_M_Imp_Auftrdat_002(strfahrgestellnummer, strDatumBis, strDatumVon, Right("0000000000" & m_objUser.KUNNR, 10), strLeasinggeber, strordernummer, SAPdatenTable, SAPFehlerTable)
            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Stornierungen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            objSAP.CommitWork()
            If intID > -1 Then
                m_objlogApp.WriteEndDataAccessSAP(intID, True)
            End If

            intWiederhergestellte = SAPTabelleHin.Count - SAPTabelleZurueck.Count

            If Not SAPTabelleZurueck.Count = 0 Then
                strFehlschlaege = ""
                For Each sapTabellenZeile In SAPTabelleZurueck
                    strFehlschlaege = strFehlschlaege & sapTabellenZeile.Order_Nr & ", "
                Next
            End If






        Catch ex As Exception
            Select Case ex.Message
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Aufträge gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objlogApp.WriteStandardDataAccessSAP(intID)
            End If
            objSAP.Connection.Close()
            objSAP.Dispose()
            m_blnGestartet = False
        End Try

    End Sub




    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "kruell_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
        End If

        Dim objSAP As New SAPProxy_Kruell.SAPProxy_Kruell()

        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        objSAP.Connection.Open()

        Dim intID As Int32 = -1

        Try

            Dim SAPStornoTabelle As New SAPProxy_Kruell.ZDAD_AUFTR_STORNTable()


            'datum angaben in SAP format umschreiben wenn Sie nicht leer sind, wenn doch Nothing setzen
            If Not strDatumAnlageVon = "" AndAlso Not strDatumAnlageVon Is String.Empty Then
                strDatumAnlageVon = MakeDateSAP(strDatumAnlageVon)
            Else
                strDatumAnlageVon = Nothing
            End If

            If Not strDatumAnlageBis = "" AndAlso Not strDatumAnlageBis Is String.Empty Then
                strDatumAnlageBis = MakeDateSAP(strDatumAnlageBis)
            Else
                strDatumAnlageBis = Nothing
            End If

            If Not strdatumStornierungBis = "" AndAlso Not strdatumStornierungBis Is String.Empty Then
                strdatumStornierungBis = MakeDateSAP(strdatumStornierungBis)
            Else
                strdatumStornierungBis = Nothing
            End If

            If Not strdatumStornierungVon = "" AndAlso Not strdatumStornierungVon Is String.Empty Then
                strdatumStornierungVon = MakeDateSAP(strdatumStornierungVon)
            Else
                strdatumStornierungVon = Nothing

            End If

            ' leereingaben auf nothing setzen, da bapi auf nothing prüft und leeren String als wert sieht
            If strfahrgestellnummer = "" AndAlso Not strfahrgestellnummer Is String.Empty Then
                strfahrgestellnummer = Nothing
            End If

            If strordernummer = "" AndAlso Not strordernummer Is String.Empty Then
                strordernummer = Nothing
            End If



            objSAP.Z_V_Stornierungen(strfahrgestellnummer, strDatumAnlageBis, strDatumAnlageVon, strdatumStornierungBis, strdatumStornierungVon, strordernummer, SAPStornoTabelle)
            'objSAP.Z_M_Imp_Auftrdat_002(strfahrgestellnummer, strDatumBis, strDatumVon, Right("0000000000" & m_objUser.KUNNR, 10), strLeasinggeber, strordernummer, SAPdatenTable, SAPFehlerTable)
            objSAP.CommitWork()

            CreateOutPut(SAPStornoTabelle.ToADODataTable, m_strAppID)

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Stornierungen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
            If intID > -1 Then
                m_objlogApp.WriteEndDataAccessSAP(intID, True)
            End If


        Catch ex As Exception
            Select Case ex.Message
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Aufträge gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objlogApp.WriteStandardDataAccessSAP(intID)
            End If
            objSAP.Connection.Close()
            objSAP.Dispose()
            m_blnGestartet = False
        End Try
    End Sub

#End Region

End Class
' ************************************************
' $History: kruell_02.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:37
' Created in $/CKAG/Applications/AppKruell/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 7.03.08    Time: 10:22
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' Ordernummer in Großbuchstaben umsetzen
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 9.01.08    Time: 14:29
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' ITA 1580 Torso
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 9.01.08    Time: 13:57
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' ITA 1580 Report01 hinzugefügt, SS History Bodys hinzugefügt
' ************************************************
