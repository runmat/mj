Option Explicit On
Option Infer On
Option Strict On

Imports CKG.Base.Common


Public Class Block

#Region "Declarations"


#End Region


    Public Function GetBlockData(ByRef objUser As Base.Kernel.Security.User, _
                               ByRef objApp As Base.Kernel.Security.App, _
                               ByVal page As Page, _
                               ByVal Blocknummer As String, _
                               ByVal BankTreuhand As String, _
                               ByVal ImportTable As DataTable) As DataTable

        Dim intID As Int32 = -1

        Dim KUNNR As String = Right("0000000000" & objUser.Customer.KUNNR, 10)
        'Exportparameter
        Dim ExportTable As New DataTable


        Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_FZG_DATEN_BLOCK", objApp, objUser, page)

        Try


            'Importparameter

            myProxy.setImportParameter("I_AG", KUNNR)
            myProxy.setImportParameter("I_BLOCK_NR_SEL", Blocknummer)
            If BankTreuhand <> "0" Then myProxy.setImportParameter("I_BANK_TH_SEL", BankTreuhand)




            Dim ImportSAPTable As DataTable = myProxy.getImportTable("GT_IN")

            If Not ImportTable Is Nothing Then
                If ImportTable.Rows.Count > 0 Then

                    Dim dr As DataRow


                    For Each dRow As DataRow In ImportTable.Rows

                        dr = ImportSAPTable.NewRow

                        dr("CHASSIS_NUM") = dRow("Fahrgestellnummer")
                        dr("LICENSE_NUM") = dRow("Kennzeichen")
                        dr("LIZNR") = dRow("Leasingvertragsnummer")
                        dr("BLOCK_NR_NEU") = dRow("Blocknummer")

                        ImportSAPTable.Rows.Add(dr)

                    Next


                    ImportSAPTable.AcceptChanges()


                End If
            End If


            myProxy.callBapi()


            ExportTable = myProxy.getExportTable("GT_OUT")



        Catch ex As Exception

        Finally
            myProxy = Nothing
        End Try

        Return ExportTable
    End Function

    Public Function SetBlockData(ByRef objUser As Base.Kernel.Security.User, _
                               ByRef objApp As Base.Kernel.Security.App, _
                               ByVal page As Page, _
                               ByVal ImportTable As DataTable) As DataTable

        Dim intID As Int32 = -1

        Dim KUNNR As String = Right("0000000000" & objUser.Customer.KUNNR, 10)
        'Exportparameter
        Dim ExportTable As New DataTable


        Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_SAVE_FZG_DATEN_BLOCK", objApp, objUser, page)

        Try


            'Importparameter

            myProxy.setImportParameter("I_AG", KUNNR)



            Dim ImportSAPTable As DataTable = myProxy.getImportTable("GT_WEB")

            If Not ImportTable Is Nothing Then
                If ImportTable.Rows.Count > 0 Then

                    Dim dr As DataRow


                    For Each dRow As DataRow In ImportTable.Rows

                        If dRow("BLOCK_ALT_LOE").ToString = "X" OrElse dRow("BLOCK_NR_NEU").ToString.Length > 0 Then
                            dr = ImportSAPTable.NewRow

                            dr("EQUNR") = dRow("EQUNR")
                            dr("BLOCK_NR_NEU") = dRow("BLOCK_NR_NEU")

                            ImportSAPTable.Rows.Add(dr)
                        End If



                    Next


                    ImportSAPTable.AcceptChanges()


                End If
            End If


            myProxy.callBapi()


            Dim ExportError As String = myProxy.getExportParameter("FLAG_ERROR").ToUpper

            ExportTable = myProxy.getExportTable("GT_WEB")

            ExportTable.DefaultView.RowFilter = "ZBEM <> ''"
            ExportTable = ExportTable.DefaultView.ToTable


        Catch ex As Exception

        Finally
            myProxy = Nothing
        End Try

        Return ExportTable



    End Function


End Class

