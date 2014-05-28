Option Explicit On
Option Strict On
Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

<Serializable()> Public Class F1_BankBase
    Inherits F1_BankBasis

#Region " Declarations"
    Protected m_tblKontingente As DataTable
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

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_hez = hez
    End Sub

#End Region

End Class
' ************************************************
' $History: F1_BankBase.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 20.03.09   Time: 12:34
' Updated in $/CKAG/Applications/AppF1/lib
' auskommentierungen entfernt
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.03.09    Time: 17:30
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 4.03.09    Time: 11:12
' Created in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugefgt
' 
' ************************************************
