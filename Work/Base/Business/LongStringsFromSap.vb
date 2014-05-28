Option Explicit On
Option Strict On

Imports CKG.Base.Common
Imports CKG.Base.Kernel.Security
Imports System.Web.UI

Namespace Business

    Public Class LongStringsFromSap
        Dim mDynSAPProxy As DynSapProxyObj

        Dim mLText As String = ""
        Dim mLTextID As String = ""
        Dim mLTextNr As String = ""
        Dim mUName As String = ""
        Dim mE_SUBRC As String = ""
        Dim mErrorText As String = ""

#Region "Properties"
        Public Property UName() As String
            Get
                Return mUName
            End Get
            Set(ByVal value As String)
                mUName = value
            End Set
        End Property

        Public Property LText() As String
            Get
                Return mLText
            End Get
            Set(ByVal value As String)
                mLText = value
            End Set
        End Property

        Public Property LTextID() As String
            Get
                Return mLTextID
            End Get
            Set(ByVal value As String)
                mLTextID = value
            End Set
        End Property

        Public Property LTextNr() As String
            Get
                Return mLTextNr
            End Get
            Set(ByVal value As String)
                mLTextNr = value
            End Set
        End Property

        Public ReadOnly Property E_SUBRC() As String
            Get
                Return mE_SUBRC
            End Get
        End Property

        Public ReadOnly Property ErrorText() As String
            Get
                Return mErrorText
            End Get
        End Property

#End Region

#Region "Methods"

        Public Sub UpdateString(ByVal m_objAPP As App, ByVal m_objUser As User, ByVal page As Page, ByVal Text As String, ByVal LTextNr As String, Optional ByVal UName As String = "")
            mLText = Text
            mLTextNr = LTextNr
            mErrorText = ""

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_BC_LTEXT_UPDATE", m_objAPP, m_objUser, page)

                myProxy.setImportParameter("I_LTEXT_NR", LTextNr)
                myProxy.setImportParameter("I_STRING", mLText)
                If (mUName <> "") Then
                    myProxy.setImportParameter("I_UNAME", UName)
                End If

                myProxy.callBapi()

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mErrorText = myProxy.getExportParameter("E_MESSAGE")
            Catch ex As Exception
                mErrorText = ex.InnerException.Message
            End Try
        End Sub

        Public Sub DeleteString(ByVal m_objAPP As App, ByVal m_objUser As User, ByVal page As Page, ByVal LTextNr As String)
            mLTextNr = LTextNr
            mErrorText = ""

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_BC_LTEXT_DELETE", m_objAPP, m_objUser, page)

                myProxy.setImportParameter("I_LTEXT_NR", mLTextNr)

                myProxy.callBapi()

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mErrorText = myProxy.getExportParameter("E_MESSAGE")
            Catch ex As Exception
                mErrorText = ex.InnerException.Message
            End Try
        End Sub


#End Region

#Region "Functions"

        Public Function ReadString(ByVal m_objAPP As App, ByVal m_objUser As User, ByVal page As Page, ByVal LTextNr As String) As String
            mLTextNr = LTextNr
            mErrorText = ""

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_BC_LTEXT_READ", m_objAPP, m_objUser, page)

                myProxy.setImportParameter("I_LTEXT_NR", mLTextID)

                myProxy.callBapi()

                mLText = myProxy.getExportParameter("E_STRING")
                mLTextID = myProxy.getExportParameter("E_LTEXT_ID")
                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mErrorText = myProxy.getExportParameter("E_MESSAGE")

            Catch ex As Exception
                mErrorText = ex.InnerException.Message
            End Try

            Return mLText
        End Function

        Public Function InsertString(ByVal m_objAPP As App, ByVal m_objUser As User, ByVal page As Page, ByVal Text As String, ByVal LTextID As String, Optional ByVal UName As String = "") As String
            mLText = Text
            mLTextID = LTextID
            mErrorText = ""

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_BC_LTEXT_INSERT", m_objAPP, m_objUser, page)

                myProxy.setImportParameter("I_STRING", mLText)
                myProxy.setImportParameter("I_LTEXT_ID", mLTextID)
                If (mUName <> "") Then
                    myProxy.setImportParameter("I_UNAME", UName)
                End If

                myProxy.callBapi()

                mLTextNr = myProxy.getExportParameter("E_LTEXT_NR")
                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mErrorText = myProxy.getExportParameter("E_MESSAGE")
            Catch ex As Exception
                mErrorText = ex.InnerException.Message
            End Try

            Return mLTextNr
        End Function


#End Region

    End Class

End Namespace