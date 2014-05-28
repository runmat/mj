Imports CKG.Base.Kernel
Imports CKG.Base.Common

'-------------------
'Zugriffsklasse auf Addressen in SAP
'-BAPI: Z_V_KCL_Adressen
'-------------------
Public Class Adressen

    Public Function GetData(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal city As String,
                            ByVal plz As String, ByVal name As String) As DataSets.AddressDataSet.ADDRESSEDataTable

        Try

            'intID = objLogApp.WriteStartDataAccessSAP(objUser.UserName, objUser.IsTestUser, "Z_V_Kcl_Adressen", "", objUser.SessionID, objUser.CurrentLogAccessASPXID)

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Kcl_Adressen", objApp, objUser, page)

            S.AP.Init("Z_V_Kcl_Adressen")

            S.AP.setImportParameter("KUNNR", Right("0000000000" & objUser.KUNNR, 10))
            S.AP.setImportParameter("NAME1", name)
            S.AP.setImportParameter("POST_CODE1", plz)
            S.AP.setImportParameter("CITY1", city)

            'myProxy.callBapi()
            S.AP.Execute()

            Dim sapTable As DataTable = S.AP.GetExportTable("GT_WEB")


            Dim resultAddresses As New DataSets.AddressDataSet.ADDRESSEDataTable()
            For Each row As DataRow In sapTable.Rows
                resultAddresses.AddADDRESSERow(row("Name1").ToString(), row("Post_Code1").ToString(), row("City1").ToString(), row("Street").ToString(), _
                                   row("Name1").ToString() + "_" + row("Post_Code1").ToString() + "_" + row("City1").ToString(), _
                                   row("House_Num1").ToString(), row("Tel_Number_Def").ToString(), row("Tel_Number_2").ToString(), row("Name2").ToString(), "", row("Kunnr").ToString(), "")
            Next

            Return resultAddresses

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    Return (New DataSets.AddressDataSet.ADDRESSEDataTable())
                Case Else
                    Throw New Exception("Beim Suchen nach Adressen ist ein Fehler aufgetreten.<br>(" & ex.Message & ")")
            End Select
        End Try

    End Function

End Class

' ************************************************
' $History: AdressenDataAccess.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************