Imports KBS.KBS_BASE

Public Class Kasse
    Private mIP As String
    Private mWerk As String
    Private mLagerort As String
    Private mFirma As String
    Private mKunnr As String
    Private mCustomerID As String
    Private mMaster As Boolean
    Private mTestuser As Boolean = False
    Private Shared m_tblApplications As New DataTable()

    Private mObjBestellung As Bestellung
    Private mObjWareneingangspruefung As Wareneingangspruefung
    Private mObjPlatinen As Platinen
    Private mObjZentrallager As Zentrallager
    Private mObjVersicherungen As Versicherungen
    Private mObjEinzahlungsbelege As Einzahlungsbelege
    Private mObjUmlagerungen As Umlagerung
    Private mObjoffBestellungen As offBestellungen
    Private mObjRetoure As Retoure
    Private _mObjNeukunde As ComCommon
    Private mObjLongStringToSap As LongStringToSap

#Region "Properties"

    Public ReadOnly Property Bestellung(ByVal page As Page) As Bestellung
        Get
            If CHANGE01_GLOBALOBJHANDLING Then
                If mObjBestellung Is Nothing Then
                    mObjBestellung = New Bestellung(Me)
                End If
                Return mObjBestellung
            Else 'wenn kein globales ObjHandling, dann ins session
                If page.Session("mObjBestellung") Is Nothing Then
                    page.Session.Add("mObjBestellung", New Bestellung(Me))
                End If
                Return CType(page.Session("mObjBestellung"), Bestellung)
            End If
        End Get

    End Property

    Public Property Wareneingangspruefung(ByVal page As Page) As Wareneingangspruefung
        Set(ByVal value As Wareneingangspruefung)
            If value Is Nothing Then
                mObjWareneingangspruefung.endWareneingangspruefung()
                mObjWareneingangspruefung = value
            Else
                Throw New Exception("Ein AppObj Kann nicht zugewiesen werden, nur durch NOTHING zerstört werden")
            End If

        End Set
        Get
            If CHANGE02_GLOBALOBJHANDLING Then
                If mObjWareneingangspruefung Is Nothing Then
                    mObjWareneingangspruefung = New Wareneingangspruefung(Me)
                Else
                    If Not mObjWareneingangspruefung.currentApplikationPage Is Nothing Then
                        If Not mObjWareneingangspruefung.currentApplikationPage.AppRelativeVirtualPath = page.AppRelativeVirtualPath Then
                            page.Response.Redirect(mObjWareneingangspruefung.currentApplikationPage.AppRelativeVirtualPath, True)
                        End If
                    End If
                End If
                Return mObjWareneingangspruefung
            Else 'wenn kein globales ObjHandling, dann ins session
                If page.Session("mObjWareneingangspruefung") Is Nothing Then
                    page.Session.Add("mObjWareneingangspruefung", New Wareneingangspruefung(Me))
                End If
                Return CType(page.Session("mObjWareneingangspruefung"), Wareneingangspruefung)
            End If
        End Get
    End Property

    Public ReadOnly Property Platinen(ByVal page As Page) As Platinen
        Get
            If CHANGE03_GLOBALOBJHANDLING Then
                If mObjPlatinen Is Nothing Then
                    mObjPlatinen = New Platinen()
                End If
                Return mObjPlatinen
            Else 'wenn kein globales ObjHandling, dann ins session
                If page.Session("mObjPlatinen") Is Nothing Then
                    page.Session.Add("mObjPlatinen", New Platinen())
                End If
                Return CType(page.Session("mObjPlatinen"), Platinen)
            End If
        End Get

    End Property

    Public ReadOnly Property Zentrallager(ByVal page As Page) As Zentrallager
        Get
            If CHANGE04_GLOBALOBJHANDLING Then
                If mObjZentrallager Is Nothing Then
                    mObjZentrallager = New Zentrallager()
                End If
                Return mObjZentrallager
            Else 'wenn kein globales ObjHandling, dann ins session
                If page.Session("mObjZentrallager") Is Nothing Then
                    page.Session.Add("mObjZentrallager", New Zentrallager())
                End If
                Return CType(page.Session("mObjZentrallager"), Zentrallager)
            End If
        End Get

    End Property

    Public ReadOnly Property Versicherungen(ByVal page As Page) As Versicherungen
        Get
            If CHANGE05_GLOBALOBJHANDLING Then
                If mObjVersicherungen Is Nothing Then
                    mObjVersicherungen = New Versicherungen()
                End If
                Return mObjVersicherungen
            Else 'wenn kein globales ObjHandling, dann ins session
                If page.Session("mObjVersicherungen") Is Nothing Then
                    page.Session.Add("mObjVersicherungen", New Versicherungen())
                End If
                Return CType(page.Session("mObjVersicherungen"), Versicherungen)
            End If
        End Get

    End Property

    Public ReadOnly Property Einzahlungsbelege(ByVal page As Page) As Einzahlungsbelege
        Get
            If CHANGE06_GLOBALOBJHANDLING Then
                If mObjEinzahlungsbelege Is Nothing Then
                    mObjEinzahlungsbelege = New Einzahlungsbelege()
                End If
                Return mObjEinzahlungsbelege
            Else 'wenn kein globales ObjHandling, dann ins session
                If page.Session("mObjEinzahlungsbelege") Is Nothing Then
                    page.Session.Add("mObjEinzahlungsbelege", New Einzahlungsbelege())
                End If
                Return CType(page.Session("mObjEinzahlungsbelege"), Einzahlungsbelege)
            End If
        End Get

    End Property

    Public ReadOnly Property Umlagerungen(ByVal page As Page) As Umlagerung
        Get
            If CHANGE07_GLOBALOBJHANDLING Then
                If mObjUmlagerungen Is Nothing Then
                    mObjUmlagerungen = New Umlagerung()
                End If
                Return mObjUmlagerungen
            Else 'wenn kein globales ObjHandling, dann ins session
                If page.Session("mObjUmlagerungen") Is Nothing Then
                    page.Session.Add("mObjUmlagerungen", New Umlagerung())
                End If
                Return CType(page.Session("mObjUmlagerungen"), Umlagerung)
            End If
        End Get

    End Property

    Public ReadOnly Property LongStringToSap(ByVal page As Page) As LongStringToSap
        Get
            If CHANGE07_GLOBALOBJHANDLING Then
                If mObjLongStringToSap Is Nothing Then
                    mObjLongStringToSap = New LongStringToSap()
                End If
                Return mObjLongStringToSap
            Else 'wenn kein globales ObjHandling, dann ins session
                If page.Session("mObjUmlagerungen") Is Nothing Then
                    page.Session.Add("mObjUmlagerungen", New Umlagerung())
                End If
                Return CType(page.Session("mObjLongStringToSap"), LongStringToSap)
            End If
        End Get
    End Property

    Public ReadOnly Property offBestellung(ByVal page As Page) As offBestellungen
        Get
            If REPORT01_GLOBALOBJHANDLING Then
                If mObjoffBestellungen Is Nothing Then
                    mObjoffBestellungen = New offBestellungen()
                End If
                Return mObjoffBestellungen
            Else 'wenn kein globales ObjHandling, dann ins session
                If page.Session("mObjoffBestellungen") Is Nothing Then
                    page.Session.Add("mObjoffBestellungen", New offBestellungen())
                End If
                Return CType(page.Session("mObjoffBestellungen"), offBestellungen)
            End If
        End Get

    End Property

    Public ReadOnly Property Retoure(ByVal page As Page) As Retoure
        Get
            If CHANGE12_GLOBALOBJHANDLING Then
                If mObjRetoure Is Nothing Then
                    mObjRetoure = New Retoure(Me)
                End If
                Return mObjRetoure
            Else 'wenn kein globales ObjHandling, dann ins Session
                If page.Session("mObjRetoure") Is Nothing Then
                    page.Session.Add("mObjRetoure", New Retoure(Me))
                End If
                Return CType(page.Session("mObjRetoure"), Retoure)
            End If
        End Get
    End Property

    Public ReadOnly Property Neukunde(ByVal page As Page) As ComCommon
        Get
            If CHANGE08_GLOBALOBJHANDLING Then

                If _mObjNeukunde Is Nothing Then
                    _mObjNeukunde = New ComCommon(Me)
                End If
                Return _mObjNeukunde
            Else 'wenn kein globales ObjHandling, dann ins session
                If page.Session("mObjCommon") Is Nothing Then
                    page.Session.Add("mObjCommon", New ComCommon(Me))
                End If
                Return CType(page.Session("mObjCommon"), ComCommon)
            End If
        End Get

    End Property

    Public ReadOnly Property Applications() As DataTable
        Get
            Return m_tblApplications
        End Get
    End Property

    Protected Friend ReadOnly Property IP() As String
        Get
            Return mIP
        End Get
    End Property

    Protected Friend ReadOnly Property Werk() As String
        Get
            Return mWerk
        End Get
    End Property

    Protected Friend ReadOnly Property Lagerort() As String
        Get
            Return mLagerort
        End Get
    End Property

    Protected Friend ReadOnly Property Firma() As String
        Get
            Return mFirma
        End Get
    End Property

    Protected Friend ReadOnly Property KUNNR() As String
        Get
            Return mKunnr
        End Get
    End Property

    Protected Friend ReadOnly Property CustomerID() As String
        Get
            Return mCustomerID
        End Get
    End Property

    Protected Friend ReadOnly Property Master() As Boolean
        Get
            Return mMaster
        End Get
    End Property

    Protected Friend ReadOnly Property Testuser() As Boolean
        Get
            Return mTestuser
        End Get
    End Property

#End Region

    Protected Friend Sub New(ByVal IP As String, ByVal Werk As String, ByVal Lagerort As String, _
                             ByVal Firma As String, ByVal strKUNNR As String, _
                             ByVal tblApps As DataTable, ByVal strCustomerID As String, ByVal bMaster As Boolean)
        mIP = IP
        mWerk = Werk
        mFirma = Firma
        mLagerort = Lagerort
        mKunnr = strKUNNR
        mCustomerID = strCustomerID
        mMaster = bMaster
        m_tblApplications = tblApps
    End Sub

    Protected Friend Sub ChangeBasedValues(ByVal values As DataRow)
        'diese funktion darf niemals aus einem Report aufgerufen werden. 
        'für sicherheitsmechanismen ist aktuell keine zeit. 
        'JJU 20090507 ITA 2808
        mWerk = values("WERKS").ToString
        mLagerort = values("LGORT").ToString
        mFirma = values("Firma").ToString
        mKunnr = values("Kunnr").ToString
        mCustomerID = values("CustomerID").ToString
        mMaster = CBool(values("Master"))
    End Sub

    Protected Friend Sub SetApps(ByVal tblApps As DataTable)
        m_tblApplications = tblApps
    End Sub

    Public Sub DestroyObjNeukunde()
        _mObjNeukunde = Nothing
    End Sub

End Class


' ************************************************
' $History: Kasse.vb $
' 
' *****************  Version 19  *****************
' User: Dittbernerc  Date: 12.05.11   Time: 15:38
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 18  *****************
' User: Dittbernerc  Date: 18.03.11   Time: 13:22
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 11.11.10   Time: 10:00
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 16.09.10   Time: 11:53
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 20.04.10   Time: 18:09
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 12.03.10   Time: 9:49
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 16.02.10   Time: 17:28
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 12.02.10   Time: 16:29
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 12.02.10   Time: 13:47
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 10.02.10   Time: 17:53
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 3.06.09    Time: 11:38
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 7.05.09    Time: 13:08
' Updated in $/CKAG2/KBS/Lib
' ITA 2808 
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.05.09    Time: 12:37
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 4.05.09    Time: 11:44
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 unfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 30.04.09   Time: 13:39
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 unfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 30.04.09   Time: 11:44
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 23.04.09   Time: 17:50
' Updated in $/CKAG2/KBS/Lib
' ITA 2808
' 
'
' ************************************************