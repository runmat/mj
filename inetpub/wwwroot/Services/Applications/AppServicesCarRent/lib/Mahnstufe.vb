Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Mahnstufe
    Inherits DatenimportBase

#Region "Declarations"
    Private mEquiTyp As String
    Private mMahnstufe As String
#End Region

#Region " Properties"
    Public Property Equityp() As String
        Get
            Return mEquiTyp
        End Get
        Set(ByVal value As String)
            mEquiTyp = value
        End Set
    End Property

    Public Property Mahnstufe() As String
        Get
            Return mMahnstufe
        End Get
        Set(ByVal value As String)
            mMahnstufe = value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal AppID As String, ByVal SessionID As String, ByVal FileName As String)
        MyBase.New(objUser, objApp, FileName)
    End Sub

    Public Overloads Sub Fill(ByVal AppID As String, ByVal SessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Mahnstufe.FILL"
        m_strAppID = AppID
        m_strSessionID = SessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1


            Try


                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_TEMP_VERS_MAHN_005", m_objApp, m_objUser, page)


                myProxy.setImportParameter("I_KUNNR", m_objUser.Customer.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_EQTYP", Equityp)
                myProxy.setImportParameter("I_ZZMAHNS", Mahnstufe)

                myProxy.callBapi()



                Dim TempTable As DataTable = myProxy.getExportTable("GT_WEB")

                
                TempTable.Columns.Add("Adresse", GetType(System.String))

                Dim Row As DataRow
                Dim Adresse As String = ""
                Dim booFound As Boolean = False

                For Each Row In TempTable.Rows


                    'Adresse hinzufügen
                    Adresse = Row("NAME1_ZS").ToString

                    If Row("NAME2_ZS").ToString.Length > 0 Then

                        Adresse &= " " & Row("NAME2_ZS").ToString & ", "
                    Else
                        Adresse &= ", "
                    End If

                    Adresse &= Row("STRAS_ZS").ToString

                    If Row("HSNR_ZS").ToString.Length > 0 Then
                        Adresse &= " " & Row("HSNR_ZS").ToString & ", "
                    Else
                        Adresse &= ", "
                    End If

                    Adresse &= Row("PSTLZ_ZS").ToString & " "

                    Adresse &= Row("ORT01_ZS").ToString

                    Row("Adresse") = Adresse

                    TempTable.AcceptChanges()



                Next

                CreateOutPut(TempTable, AppID)


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
' $History: Mahnstufe.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 2.12.10    Time: 11:24
' Updated in $/CKAG2/Applications/AppServicesCarRent/lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 15.12.09   Time: 10:52
' Updated in $/CKAG2/Applications/AppServicesCarRent/lib
' ITA: 3384
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 15.12.09   Time: 9:20
' Created in $/CKAG2/Applications/AppServicesCarRent/lib
' 