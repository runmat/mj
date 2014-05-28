Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

<Serializable()> Public Class BankBaseCredit
    REM § Lese-/Schreibfunktion, Kunde: Übergreifend, 
    REM § Show - BAPI: Z_M_Creditlimit_Detail_001,
    REM § Change - BAPI: Z_M_Creditlimit_Change_001.

    Inherits BankBase

#Region " Declarations"
    Private m_tblKontingente As DataTable
#End Region

#Region " Properties"
    Public Property Kontingente() As DataTable
        Get
            Return m_tblKontingente
        End Get
        Set(ByVal Value As DataTable)
            m_tblKontingente = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_hez = False
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overrides Sub Change()
       
    End Sub
#End Region
End Class

' ************************************************
' $History: BankBaseCredit.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 18.07.08   Time: 12:51
' Created in $/CKAG/Applications/AppBPLG/Lib
' Klassen erstellt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 11.03.08   Time: 14:48
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1765
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 1.03.08    Time: 13:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Änderung der Händlernummer auf 10 stellen mit führenden 0 
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.01.08    Time: 18:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Bugfix RTFS
' 
' *****************  Version 4  *****************
' User: Uha          Date: 13.12.07   Time: 15:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Kontingentart "HEZ (in standard temporär enthalten)" in lokale
' BankBaseCredit wieder eingefügt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 13.12.07   Time: 13:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1481/1509 (Änderung / Sperrung Händlerkontingent) Testversion
' 
' *****************  Version 2  *****************
' User: Uha          Date: 13.12.07   Time: 10:31
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' In BankBaseCredit überflüssige Methoden und Kontingentarten entfernt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.12.07   Time: 9:49
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' fin_06.vb durch BankBaseCredit.vb ersetzt
' 
' ************************************************
