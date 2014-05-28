Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Vertragsdaten
    Inherits DatenimportBase

#Region "Declarations"
    Private mAG as STRING
    Private mOutputTable As DataTable
    Private mDistrikte As DataTable
    Private mSaveTable As DataTable
    Private mSuchKennzeichen As String
    Private mE_SUBRC As String
    Private mE_MESSAGE As String
#End Region

#Region " Properties"
    Public Property AG() As String
        Get
            Return mAG
        End Get
        Set(ByVal value As System.String)
            mAG = value
        End Set
    End Property
	
    Public Property OutputTable() As DataTable
        Get
            Return mOutputTable
        End Get
        Set(ByVal value As System.Data.DataTable)
            mOutputTable = value
        End Set
    End Property
    Public Property Distrikte() As DataTable
        Get
            Return mDistrikte
        End Get
        Set(ByVal value As System.Data.DataTable)
            mDistrikte = value
        End Set
    End Property
    Public Property SaveTable() As DataTable
        Get
            Return mSaveTable
        End Get
        Set(ByVal value As System.Data.DataTable)
            mSaveTable = value
        End Set
    End Property
    Public Property E_SUBRC() As String
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As String)
            mE_SUBRC = Value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal Value As String)
            mE_MESSAGE = Value
        End Set
    End Property
    Public Property SuchKennzeichen() As String
        Get
            Return mSuchKennzeichen
        End Get
        Set(ByVal Value As String)
            mSuchKennzeichen = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal AppID As String, ByVal SessionID As String, ByVal FileName As String)
        MyBase.New(objUser, objApp, FileName)
    End Sub

    Public Overloads Sub Fill(ByVal AppID As String, ByVal SessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Vertragsdaten.FILL"
        m_strAppID = AppID
        m_strSessionID = SessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1


            Try


                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_EQUI_CHANGES_001", m_objApp, m_objUser, page)
		
		
                myProxy.setImportParameter("AG", m_objUser.Customer.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("AEND_GRUND", mSuchKennzeichen)


                myProxy.callBapi()


                mOutputTable = myProxy.getExportTable("GT_CHANGES")

                mSaveTable = mOutputTable.Copy


                mOutputTable.Columns.Add("HalterAdresse", GetType(System.String))
                mOutputTable.Columns.Add("Erledigt", GetType(System.Boolean))

                Dim Row As DataRow
                Dim HalterAdresse As String = ""
                Dim booFound As Boolean = False

                Distrikte = New DataTable

                Distrikte.Columns.Add("DistriktID", GetType(System.String))
                Distrikte.Columns.Add("Distrikt", GetType(System.String))


                Dim DistriktRow As DataRow = Distrikte.NewRow

                DistriktRow("DistriktID") = "0"
                DistriktRow("Distrikt") = "- alle -"

                Distrikte.Rows.Add(DistriktRow)
                Distrikte.AcceptChanges()


                For Each Row In mOutputTable.Rows


                    Row("Erledigt") = False

                    'Halteradresse hinzufügen
                    HalterAdresse = Row("Name1").ToString
                    If HalterAdresse.Length > 0 Then

                        If Row("Name2").ToString.Length > 0 Then

                            HalterAdresse &= " " & Row("Name2").ToString & "<br />  "
                        Else
                            HalterAdresse &= "<br />  "
                        End If

                        HalterAdresse &= Row("STREET").ToString

                        If Row("HOUSE_NUM1").ToString.Length > 0 Then
                            HalterAdresse &= " " & Row("HOUSE_NUM1").ToString & "<br /> "
                        Else
                            HalterAdresse &= "<br />  "
                        End If

                        HalterAdresse &= Row("POST_CODE1").ToString & " "

                        HalterAdresse &= Row("CITY1").ToString

                    End If
                    Row("HalterAdresse") = HalterAdresse

                    mOutputTable.AcceptChanges()

                    Dim DistriktNewRow As DataRow = Distrikte.NewRow
                    booFound = False

                    'Distrikte hinzufügen

                    For Each DistriktRow In Distrikte.Rows

                        If Row("KNRZE").ToString = DistriktRow("DistriktID").ToString Then
                            booFound = True
                            Exit For
                        End If

                    Next

                    If booFound = False Then
                        DistriktNewRow("DistriktID") = Row("KNRZE").ToString
                        DistriktNewRow("Distrikt") = "-  " & Row("KNRZE").ToString & "  -"
                        Distrikte.Rows.Add(DistriktNewRow)
                        Distrikte.AcceptChanges()
                    End If


                Next

                CreateOutPut(mOutputTable, AppID)

                mOutputTable = Result






            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Save(ByVal AppID As String, ByVal SessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Vertragsdaten.Save"
        m_strAppID = AppID
        m_strSessionID = SessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1


            Try


                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_SETERL_EQUI_CHANGES_001", m_objApp, m_objUser, page)


                myProxy.setImportParameter("AG", m_objUser.Customer.KUNNR.PadLeft(10, "0"c))

                Dim SapTable As DataTable = myProxy.getImportTable("GT_CHANGES")


                For Each dr As DataRow In SaveTable.Rows

                    Dim Newrow As DataRow = SapTable.NewRow

                    For Each col As DataColumn In SaveTable.Columns

                        Newrow(col.ColumnName) = dr(col.ColumnName)

                    Next

                    SapTable.Rows.Add(Newrow)

                    SapTable.AcceptChanges()

                Next


                myProxy.callBapi()


            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class
' ************************************************
' $History: Vertragsdaten.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 6.12.10    Time: 16:56
' Updated in $/CKAG2/Applications/AppF2/lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.12.09    Time: 17:12
' Created in $/CKAG2/Applications/AppF2/lib
' 