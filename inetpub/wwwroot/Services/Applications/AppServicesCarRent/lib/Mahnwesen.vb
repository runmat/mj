Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Mahnwesen
    Inherits DatenimportBase

#Region "Declarations"

#End Region

#Region " Properties"
    Public Property Vorgangsart As String
    Public Property Fahrgestellnummer As String
    Public Property Kennzeichen As String
    Public Property DatumVon As String
    Public Property DatumBis As String
    Public Property Teileingang As String
    Public Property Mahnstufe As String
    Public Property Mahnsperre As String
    Public Property ExTable As DataTable
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


                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_SCHLUESSELDIFFERENZEN", m_objApp, m_objUser, page)


                myProxy.setImportParameter("I_KUNNR", m_objUser.Customer.KUNNR.PadLeft(10, "0"c))

                myProxy.callBapi()



                Dim TempTable As DataTable = myProxy.getExportTable("GT_WEB_OUT_BRIEFE")
                'Dim HerstellerTable As DataTable = myProxy.getExportTable("GT_WEB_OUT_HERST")


                If TempTable.Rows.Count > 0 Then

                    'TempTable.Columns.Add("HERST", GetType(System.String))
                    'TempTable.Columns.Add("HERST_T", GetType(System.String))

                    'TempTable.AcceptChanges()

                    TempTable.DefaultView.RowFilter = "MAHN_2 Is Not Null"

                    TempTable = TempTable.DefaultView.ToTable
                End If


                'Dim rs() As DataRow

                'For Each dr As DataRow In TempTable.Rows

                '    rs = HerstellerTable.Select("CHASSIS_NUM='" & dr("CHASSIS_NUM").ToString & "'")

                '    If rs.Length > 0 Then
                '        dr("HERST") = rs(0)("HERST")
                '        dr("HERST_T") = rs(0)("HERST_T")
                '        TempTable.AcceptChanges()
                '    End If

                'Next



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


    Public Sub GetSchluesseleingaenge(ByVal AppID As String, ByVal SessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Mahnwesen.GetSchluesseleingaenge"
        m_strAppID = AppID
        m_strSessionID = SessionID

        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_MAHN_EQSTL_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_AG", m_objUser.Customer.KUNNR.PadLeft(10, "0"c))

                If Not String.IsNullOrEmpty(Me.Vorgangsart) Then
                    myProxy.setImportParameter("I_VORG_ART", Me.Vorgangsart)
                End If

                myProxy.setImportParameter("I_CHASSIS_NUM", Fahrgestellnummer)
                myProxy.setImportParameter("I_LICENSE_NUM", Kennzeichen)
                myProxy.setImportParameter("I_ZULDAT_VON", DatumVon)
                myProxy.setImportParameter("I_ZULDAT_BIS", DatumBis)
                myProxy.setImportParameter("I_MAHNSTUFEN", Mahnstufe)
                myProxy.setImportParameter("I_MIT_MAHNSPERRE", Mahnsperre)
                myProxy.setImportParameter("I_NUR_OFFENE", "X")

                myProxy.callBapi()



                Dim TempTable As DataTable = myProxy.getExportTable("GT_OUT")



                If TempTable.Rows.Count > 0 Then

                    TempTable.Columns.Add("Adresse", GetType(System.String))
                    TempTable.Columns.Add("Teileingang", GetType(System.String))
                    TempTable.Columns.Add("MahnartText", GetType(System.String))
                    TempTable.Columns.Add("Selected", GetType(System.String))
                    TempTable.Columns.Add("ID", GetType(System.Int32))

                    TempTable.AcceptChanges()

                    Dim i As Int32 = 0

                    For Each dr As DataRow In TempTable.Rows



                        dr("Selected") = ""

                        dr("ID") = i

                        dr("Adresse") = dr("NAME1_ME").ToString() & _
                                        (IIf(String.IsNullOrEmpty(dr("NAME2_ME").ToString()), ", ", " " & dr("NAME2_ME").ToString() & ", ")).ToString() & "<br />" & _
                                        dr("STREET_ME").ToString() & _
                                        (IIf(String.IsNullOrEmpty(dr("HOUSE_NUM1_ME").ToString()), ", ", " " & dr("HOUSE_NUM1_ME").ToString() & ", ")).ToString() & _
                                        "<br />" & _
                                        dr("POST_CODE1_ME").ToString() & " " & dr("CITY1_ME").ToString() & ", " & "<br />" & _
                                        dr("SMTP_ADDR_ME").ToString()


                        dr("Teileingang") = IIf(String.IsNullOrEmpty(dr("EQUNR").ToString()) = False, "X", "").ToString()

                        Select Case dr("MAHNART").ToString()
                            Case "1"
                                dr("MahnartText") = "Kennzeichen Mahnung per E-Mail"
                            Case "2"
                                dr("MahnartText") = "Per Postbrief wenn keine E-Mail Adresse"
                            Case "3"
                                dr("MahnartText") = "Immer per Postbrief"
                            Case "4"
                                dr("MahnartText") = "Nur Anzeige"
                        End Select

                        i += 1


                    Next


                End If

                ResultTable = TempTable

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Save(ByVal AppID As String, ByVal SessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Mahnwesen.Save"
        m_strAppID = AppID
        m_strSessionID = SessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1


            Try


                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_SAVE_MAHN_EQSTL_01", m_objApp, m_objUser, page)


                myProxy.setImportParameter("I_AG", m_objUser.Customer.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_USER", m_objUser.UserName)


                Dim dt As DataTable = myProxy.getImportTable("GT_IN")


                For Each dr As DataRow In ExTable.Rows

                    Dim NewRow As DataRow = dt.NewRow()


                    For Each Col As DataColumn In dt.Columns
                        NewRow(Col.ColumnName) = dr(Col.ColumnName)
                    Next



                    dt.Rows.Add(NewRow)
                Next



                myProxy.callBapi()



            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case "ERR_CHANGE"
                        m_strMessage = "Fehler beim Speichern."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub


    Public Function GetExporttable(ByVal page As Web.UI.Page) As DataTable

        Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_SAVE_MAHN_EQSTL_01", m_objApp, m_objUser, Page)
        Dim dt As DataTable = myProxy.getImportTable("GT_IN")

        Return dt
    End Function



#End Region

End Class
' ************************************************
' $History: Mahnwesen.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 11.01.10   Time: 12:39
' Updated in $/CKAG2/Applications/AppServicesCarRent/lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 15.12.09   Time: 16:52
' Created in $/CKAG2/Applications/AppServicesCarRent/lib
' ITA: 3381
' 
