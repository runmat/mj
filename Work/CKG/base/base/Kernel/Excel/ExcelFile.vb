Option Strict On
Option Explicit On 

Namespace Kernel.Excel

    Public Class ExcelFile
        REM § Klasse zur binären Erzeugung einer Exceldatei im Format BIFF2 (Excel 2.1)
        REM § Das BIFF2 Format lässt kein Erzeugung von Workbooks zu.

        Private Declare Sub CopyMemory Lib "KERNEL32" Alias "RtlMoveMemory" (ByRef lpvDest As String, ByRef lpvSource As Long, ByVal cbCopy As Long)


        Public Enum ValueTypes
            xlsinteger = 0
            xlsnumber = 1
            xlsText = 2
            xlsdate = 4
        End Enum


        Public Enum CellAlignment
            xlsGeneralAlign = 0
            xlsLeftAlign = 1
            xlsCentreAlign = 2
            xlsrightAlign = 3
            xlsFillCell = 4
            xlsLeftBorder = 8
            xlsRightBorder = 16
            xlsTopBorder = 32
            xlsBottomBorder = 64
            xlsShaded = 128
        End Enum


        Public Enum CellFont
            xlsFont0 = 0
            xlsFont1 = 64
            xlsFont2 = 128
            xlsFont3 = 192
        End Enum

        Public Enum CellHiddenLocked
            xlsNormal = 0
            xlsLocked = 64
            xlsHidden = 128
        End Enum



        Public Enum MarginTypes
            xlsLeftMargin = 38
            xlsRightMargin = 39
            xlsTopMargin = 40
            xlsBottomMargin = 41
        End Enum


        Public Enum FontFormatting
            xlsNoFormat = 0
            xlsBold = 1
            xlsItalic = 2
            xlsUnderline = 4
            xlsStrikeout = 8
        End Enum

        Private Structure FONT_RECORD
            Dim opcode As Short '49
            Dim length As Short '5+len(fontname)
            Dim FontHeight As Short
            'bit0 bold, bit1 italic, bit2 underline, bit3 strikeout, bit4-7 reserved
            Dim FontAttributes1 As Byte
            Dim FontAttributes2 As Byte 'reserved - always 0
            Dim FontNameLength As Byte
        End Structure


        Private Structure PASSWORD_RECORD
            Dim opcode As Short '47
            Dim length As Short 'len(password)
        End Structure


        Private Structure HEADER_FOOTER_RECORD
            Dim opcode As Short '20 Header, 21 Footer
            Dim length As Short '1+len(text)
            Dim TextLength As Byte
        End Structure


        Private Structure PROTECT_SPREADSHEET_RECORD
            Dim opcode As Short '18
            Dim length As Short '2
            Dim Protect As Short
        End Structure

        Private Structure FORMAT_COUNT_RECORD
            Dim opcode As Short '1f
            Dim length As Short '2
            Dim Count As Short
        End Structure

        Private Structure FORMAT_RECORD
            Dim opcode As Short '1e
            Dim length As Short '1+len(format)
            Dim FormatLenght As Byte 'len(format)
        End Structure '+ followed by the Format-Picture



        Private Structure COLWIDTH_RECORD
            Dim opcode As Short '36
            Dim length As Short '4
            Dim col1 As Byte 'first column
            Dim col2 As Byte 'last column
            Dim ColumnWidth As Short 'at 1/256th of a character
        End Structure

        'Beginning Of File record
        Private Structure BEG_FILE_RECORD
            Dim opcode As Short
            Dim length As Short
            Dim version As Short
            Dim ftype As Short
        End Structure

        'End Of File record
        Private Structure END_FILE_RECORD
            Dim opcode As Short
            Dim length As Short
        End Structure

        'true/false to print gridlines
        Private Structure PRINT_GRIDLINES_RECORD
            Dim opcode As Short
            Dim length As Short
            Dim PrintFlag As Short
        End Structure

        'Integer record
        Private Structure tInteger
            Dim opcode As Short
            Dim length As Short
            Dim Row As Short 'unsigned integer
            Dim col As Short
            'rgbAttr1 handles whether cell is hidden and/or locked
            Dim rgbAttr1 As Byte
            'rgbAttr2 handles the Font# and Formatting assigned to this cell
            Dim rgbAttr2 As Byte
            'rgbAttr3 handles the Cell Alignment/borders/shading
            Dim rgbAttr3 As Byte
            Dim intValue As Short 'the actual integer value
        End Structure

        'Number record
        Private Structure tNumber
            Dim opcode As Short
            Dim length As Short
            Dim Row As Short
            Dim col As Short
            Dim rgbAttr1 As Byte
            Dim rgbAttr2 As Byte
            Dim rgbAttr3 As Byte
            Dim NumberValue As Double '8 Bytes
        End Structure

        'Label (Text) record
        Private Structure tText
            Dim opcode As Short
            Dim length As Short
            Dim Row As Short
            Dim col As Short
            Dim rgbAttr1 As Byte
            Dim rgbAttr2 As Byte
            Dim rgbAttr3 As Byte
            Dim TextLength As Byte
        End Structure

        Private Structure MARGIN_RECORD_LAYOUT
            Dim opcode As Short
            Dim length As Short
            Dim MarginValue As Double '8 bytes
        End Structure

        Private Structure HPAGE_BREAK_RECORD
            Dim opcode As Short
            Dim length As Short
            Dim NumPageBreaks As Short
        End Structure

        Private Structure DEF_ROWHEIGHT_RECORD
            Dim opcode As Short
            Dim length As Short
            Dim RowHeight As Short
        End Structure

        Private Structure ROW_HEIGHT_RECORD
            Dim opcode As Short '08
            Dim length As Short 'should always be 16 bytes
            Dim RowNumber As Short
            Dim FirstColumn As Short
            Dim LastColumn As Short
            Dim RowHeight As Short 'written to file as 1/20ths of a point
            Dim internal As Short
            Dim DefaultAttributes As Byte
            Dim FileOffset As Short
            Dim rgbAttr1 As Byte
            Dim rgbAttr2 As Byte
            Dim rgbAttr3 As Byte
        End Structure

        Private FileNumber As Short
        Private BEG_FILE_MARKER As ExcelFile.BEG_FILE_RECORD
        Private END_FILE_MARKER As ExcelFile.END_FILE_RECORD
        Private HORIZ_PAGE_BREAK As ExcelFile.HPAGE_BREAK_RECORD


        Private HorizPageBreakRows() As Short
        Private NumHorizPageBreaks As Short





        Public Function CreateFile(ByVal FileName As String) As Short
            Dim OpenFile As Object

            On Error GoTo Write_Error

            If Dir(FileName) > "" Then
                Kill(FileName)
            End If

            FileNumber = CShort(FreeFile())
            FileOpen(FileNumber, FileName, OpenMode.Binary)

            FilePut(FileNumber, BEG_FILE_MARKER)

            Call WriteDefaultFormats()


            ReDim HorizPageBreakRows(0)
            NumHorizPageBreaks = 0


            OpenFile = 0 'return with no error

            Exit Function

Write_Error:

            OpenFile = Err.Number
            Exit Function

        End Function

        Public Function CloseFile() As Short
            Dim x As Short

            On Error GoTo Write_Error

            If FileNumber = 0 Then Exit Function


            'write the horizontal page breaks if necessary
            Dim lLoop1 As Integer
            Dim lLoop2 As Integer
            Dim lTemp As Integer
            If NumHorizPageBreaks > 0 Then
                'the Horizontal Page Break array must be in sorted order.
                'Use a simple Bubble sort because the size of this array would
                'be pretty small most of the time. A QuickSort would probably
                'be overkill.
                For lLoop1 = UBound(HorizPageBreakRows) To LBound(HorizPageBreakRows) Step -1
                    For lLoop2 = LBound(HorizPageBreakRows) + 1 To lLoop1
                        If HorizPageBreakRows(lLoop2 - 1) > HorizPageBreakRows(lLoop2) Then
                            lTemp = HorizPageBreakRows(lLoop2 - 1)
                            HorizPageBreakRows(lLoop2 - 1) = HorizPageBreakRows(lLoop2)
                            HorizPageBreakRows(lLoop2) = CShort(lTemp)
                        End If
                    Next lLoop2
                Next lLoop1

                'write the Horizontal Page Break Record
                With HORIZ_PAGE_BREAK
                    .opcode = 27
                    .length = CShort(2 + (NumHorizPageBreaks * 2))
                    .NumPageBreaks = NumHorizPageBreaks
                End With

                FilePut(FileNumber, HORIZ_PAGE_BREAK)

                'now write the actual page break values
                'the MKI$ function is standard in other versions of BASIC but
                'VisualBasic does not have it. A KnowledgeBase article explains
                'how to recreate it (albeit using 16-bit API, I switched it
                'to 32-bit).
                For x = 1 To CShort(UBound(HorizPageBreakRows))

                    FilePut(FileNumber, MKI(HorizPageBreakRows(x)))
                Next
            End If


            FilePut(FileNumber, END_FILE_MARKER)
            FileClose(FileNumber)

            CloseFile = 0 'return with no error code

            Exit Function

Write_Error:
            CloseFile = CShort(Err.Number)
            Exit Function

        End Function



        Private Sub Class_Initialize_Renamed()

            'Set up default values for records
            'These should be the values that are the same for every record of these types

            With BEG_FILE_MARKER 'beginning of file
                .opcode = 9
                .length = 4
                .version = 2
                .ftype = 10
            End With

            With END_FILE_MARKER 'end of file marker
                .opcode = 10
            End With


        End Sub
        Public Sub New()
            MyBase.New()
            Class_Initialize_Renamed()
        End Sub


        Public Function InsertHorizPageBreak(ByRef lrow As Integer) As Short
            Dim Row As Short

            On Error GoTo Page_Break_Error

            'the row and column values are written to the excel file as
            'unsigned integers. Therefore, must convert the longs to integer.
            If lrow > 32767 Then
                Row = CShort(lrow - 65536)
            Else
                Row = CShort(lrow - 1) 'rows/cols in Excel binary file are zero based
            End If

            NumHorizPageBreaks = CShort(NumHorizPageBreaks + 1)
            ReDim Preserve HorizPageBreakRows(NumHorizPageBreaks)

            HorizPageBreakRows(NumHorizPageBreaks) = Row

            Exit Function


Page_Break_Error:
            InsertHorizPageBreak = CShort(Err.Number)
            Exit Function


        End Function



        Public Function WriteValue(ByRef ValueType As ExcelFile.ValueTypes, ByRef CellFontUsed As ExcelFile.CellFont, ByRef Alignment As ExcelFile.CellAlignment, ByRef HiddenLocked As ExcelFile.CellHiddenLocked, ByRef lrow As Integer, ByRef lcol As Integer, ByRef value As Object, Optional ByRef CellFormat As Integer = 0) As Short
            Dim a As Object
            Dim l As Short
            Dim st As String
            Dim col As Short
            Dim Row As Short

            On Error GoTo Write_Error

            'the row and column values are written to the excel file as
            'unsigned integers. Therefore, must convert the longs to integer.

            If lrow > 32767 Then
                Row = CShort(lrow - 65536)
            Else
                Row = CShort(lrow - 1) 'rows/cols in Excel binary file are zero based
            End If

            If lcol > 32767 Then
                col = CShort(lcol - 65536)
            Else
                col = CShort(lcol - 1) 'rows/cols in Excel binary file are zero based
            End If


            Dim INTEGER_RECORD As ExcelFile.tInteger
            Dim NUMBER_RECORD As ExcelFile.tNumber
            Dim b As Byte
            Dim TEXT_RECORD As ExcelFile.tText
            Select Case ValueType
                Case ValueTypes.xlsinteger
                    With INTEGER_RECORD
                        .opcode = 2
                        .length = 9
                        .Row = Row
                        .col = col
                        .rgbAttr1 = CByte(HiddenLocked)
                        .rgbAttr2 = CByte(CellFontUsed + CellFormat)
                        .rgbAttr3 = CByte(Alignment)
                        .intValue = CShort(value)
                    End With
                    FilePut(FileNumber, INTEGER_RECORD)


                Case ValueTypes.xlsnumber
                    With NUMBER_RECORD
                        .opcode = 3
                        .length = 15
                        .Row = Row
                        .col = col
                        .rgbAttr1 = CByte(HiddenLocked)
                        .rgbAttr2 = CByte(CellFontUsed + CellFormat)
                        .rgbAttr3 = CByte(Alignment)
                        .NumberValue = CDbl(value)
                    End With
                    FilePut(FileNumber, NUMBER_RECORD)


                Case ValueTypes.xlsdate
                    With NUMBER_RECORD
                        .opcode = 3
                        .length = 15
                        .Row = Row
                        .col = col
                        .rgbAttr1 = CByte(HiddenLocked)
                        ' Set Format #20 whatever happens. Override CellFormat setting
                        ' which is not used yet in this project.
                        .rgbAttr2 = CByte(CellFontUsed + 20)
                        .rgbAttr3 = CByte(Alignment)
                        Dim dblValue As Double
                        If IsDate(value) Then
                            ' Convert Date to decimal value
                            dblValue = CDate(value).ToOADate
                        End If
                        .NumberValue = dblValue
                    End With
                    FilePut(FileNumber, NUMBER_RECORD)


                Case ValueTypes.xlsText
                    st = CStr(value)
                    l = CShort(Len(st))

                    With TEXT_RECORD
                        .opcode = 4
                        .length = 10
                        'Length of the text portion of the record
                        .TextLength = CByte(l)

                        'Total length of the record
                        .length = CShort(8 + l)

                        .Row = Row
                        .col = col

                        .rgbAttr1 = CByte(HiddenLocked)
                        .rgbAttr2 = CByte(CellFontUsed + CellFormat)
                        .rgbAttr3 = CByte(Alignment)

                        'Put record header
                        FilePut(FileNumber, TEXT_RECORD)

                        'Then the actual string data
                        For a = 1 To l
                            b = CByte(Asc(Mid(st, CInt(a), 1)))
                            FilePut(FileNumber, b)
                        Next
                    End With

            End Select

            WriteValue = 0 'return with no error

            Exit Function

Write_Error:
            WriteValue = CShort(Err.Number)
            Exit Function

        End Function


        Public Function SetMargin(ByRef Margin As ExcelFile.MarginTypes, ByRef MarginValue As Double) As Short

            On Error GoTo Write_Error

            'write the spreadsheet's layout information (in inches)
            Dim MarginRecord As ExcelFile.MARGIN_RECORD_LAYOUT

            With MarginRecord
                .opcode = CShort(Margin)
                .length = 8
                .MarginValue = MarginValue 'in inches
            End With
            FilePut(FileNumber, MarginRecord)

            SetMargin = 0

            Exit Function

Write_Error:
            SetMargin = CShort(Err.Number)
            Exit Function

        End Function


        Public Function SetColumnWidth(ByRef FirstColumn As Byte, ByRef LastColumn As Byte, ByRef WidthValue As Short) As Object

            On Error GoTo Write_Error

            Dim COLWIDTH As ExcelFile.COLWIDTH_RECORD

            With COLWIDTH
                .opcode = 36
                .length = 4
                .col1 = CByte(FirstColumn - 1)
                .col2 = CByte(LastColumn - 1)
                If WidthValue * 256 > Short.MaxValue Then
                    .ColumnWidth = Short.MaxValue
                Else
                    .ColumnWidth = CShort(WidthValue * 256) 'values are specified as 1/256 of a character
                End If
            End With
            FilePut(FileNumber, COLWIDTH)

            SetColumnWidth = 0

            Exit Function

Write_Error:
            SetColumnWidth = Err.Number
            Exit Function

        End Function


        Public Function SetFont(ByRef FontName As String, ByRef FontHeight As Short, ByRef FontFormat As ExcelFile.FontFormatting) As Short
            Dim a As Object
            Dim l As Short

            On Error GoTo Write_Error

            'you can set up to 4 fonts in the spreadsheet file. When writing a value such
            'as a Text or Number you can specify one of the 4 fonts (numbered 0 to 3)

            Dim FONTNAME_RECORD As ExcelFile.FONT_RECORD

            l = CShort(Len(FontName))

            With FONTNAME_RECORD
                .opcode = 49
                .length = CShort(5 + l)
                .FontHeight = CShort(FontHeight * 20)
                .FontAttributes1 = CByte(FontFormat) 'bold/underline etc...
                .FontAttributes2 = CByte(0) 'reserved-always zero!!
                .FontNameLength = CByte(Len(FontName))
            End With
            FilePut(FileNumber, FONTNAME_RECORD)

            'Then the actual font name data
            Dim b As Byte
            For a = 1 To l
                b = CByte(Asc(Mid(FontName, CInt(a), 1)))
                FilePut(FileNumber, b)
            Next

            SetFont = 0

            Exit Function

Write_Error:
            SetFont = CShort(Err.Number)
            Exit Function


        End Function


        Public Function SetHeader(ByRef HeaderText As String) As Short
            Dim a As Object
            Dim l As Short

            On Error GoTo Write_Error

            Dim HEADER_RECORD As ExcelFile.HEADER_FOOTER_RECORD

            l = CShort(Len(HeaderText))

            With HEADER_RECORD
                .opcode = 20
                .length = CShort(1 + l)
                .TextLength = CByte(Len(HeaderText))
            End With
            FilePut(FileNumber, HEADER_RECORD)

            'Then the actual Header text
            Dim b As Byte
            For a = 1 To l
                b = CByte(Asc(Mid(HeaderText, CInt(a), 1)))
                FilePut(FileNumber, b)
            Next

            SetHeader = 0

            Exit Function

Write_Error:
            SetHeader = CShort(Err.Number)
            Exit Function

        End Function



        Public Function SetFooter(ByRef FooterText As String) As Short
            Dim a As Object
            Dim l As Short

            On Error GoTo Write_Error

            Dim FOOTER_RECORD As ExcelFile.HEADER_FOOTER_RECORD

            l = CShort(Len(FooterText))

            With FOOTER_RECORD
                .opcode = 21
                .length = CShort(1 + l)
                .TextLength = CByte(Len(FooterText))
            End With
            FilePut(FileNumber, FOOTER_RECORD)

            'Then the actual Header text
            Dim b As Byte
            For a = 1 To l
                b = CByte(Asc(Mid(FooterText, CInt(a), 1)))
                FilePut(FileNumber, b)
            Next

            SetFooter = 0

            Exit Function

Write_Error:
            SetFooter = CShort(Err.Number)
            Exit Function

        End Function



        Public Function SetFilePassword(ByRef PasswordText As String) As Short
            Dim a As Object
            Dim l As Short

            On Error GoTo Write_Error

            Dim FILE_PASSWORD_RECORD As ExcelFile.PASSWORD_RECORD

            l = CShort(Len(PasswordText))

            With FILE_PASSWORD_RECORD
                .opcode = 47
                .length = l
            End With
            FilePut(FileNumber, FILE_PASSWORD_RECORD)

            'Then the actual Password text
            Dim b As Byte
            For a = 1 To l
                b = CByte(Asc(Mid(PasswordText, CInt(a), 1)))
                FilePut(FileNumber, b)
            Next

            SetFilePassword = 0

            Exit Function

Write_Error:
            SetFilePassword = CShort(Err.Number)
            Exit Function

        End Function




        Public WriteOnly Property PrintGridLines() As Boolean
            Set(ByVal Value As Boolean)

                On Error GoTo Write_Error

                Dim GRIDLINES_RECORD As ExcelFile.PRINT_GRIDLINES_RECORD

                With GRIDLINES_RECORD
                    .opcode = 43
                    .length = 2
                    If Value = True Then
                        .PrintFlag = 1
                    Else
                        .PrintFlag = 0
                    End If

                End With
                FilePut(FileNumber, GRIDLINES_RECORD)

                Exit Property

Write_Error:
                Exit Property


            End Set
        End Property




        Public WriteOnly Property ProtectSpreadsheet() As Boolean
            Set(ByVal Value As Boolean)

                On Error GoTo Write_Error

                Dim PROTECT_RECORD As ExcelFile.PROTECT_SPREADSHEET_RECORD

                With PROTECT_RECORD
                    .opcode = 18
                    .length = 2
                    If Value = True Then
                        .Protect = 1
                    Else
                        .Protect = 0
                    End If

                End With
                FilePut(FileNumber, PROTECT_RECORD)

                Exit Property

Write_Error:
                Exit Property


            End Set
        End Property


        Public Function WriteDefaultFormats() As Short

            Dim cFORMAT_COUNT_RECORD As ExcelFile.FORMAT_COUNT_RECORD
            Dim cFORMAT_RECORD As ExcelFile.FORMAT_RECORD
            Dim lIndex As Integer
            Dim aFormat(23) As String
            Dim l As Integer
            Dim q As String
            q = Chr(34)

            aFormat(0) = "General"
            aFormat(1) = "0"
            aFormat(2) = "0.00"
            aFormat(3) = "#,##0"
            aFormat(4) = "#,##0.00"
            aFormat(5) = "#,##0\ " & q & "$" & q & ";\-#,##0\ " & q & "$" & q
            aFormat(6) = "#,##0\ " & q & "$" & q & ";[Red]\-#,##0\ " & q & "$" & q
            aFormat(7) = "#,##0.00\ " & q & "$" & q & ";\-#,##0.00\ " & q & "$" & q
            aFormat(8) = "#,##0.00\ " & q & "$" & q & ";[Red]\-#,##0.00\ " & q & "$" & q
            aFormat(9) = "0%"
            aFormat(10) = "0.00%"
            aFormat(11) = "0.00E+00"
            aFormat(12) = "dd/mm/yy"
            aFormat(13) = "dd/\ mmm\ yy"
            aFormat(14) = "dd/\ mmm"
            aFormat(15) = "mmm\ yy"
            aFormat(16) = "h:mm\ AM/PM"
            aFormat(17) = "h:mm:ss\ AM/PM"
            aFormat(18) = "hh:mm"
            aFormat(19) = "hh:mm:ss"
            aFormat(20) = "dd/mm/yy\ hh:mm"
            aFormat(21) = "##0.0E+0"
            aFormat(22) = "mm:ss"
            aFormat(23) = "@"

            With cFORMAT_COUNT_RECORD
                .opcode = &H1FS
                .length = &H2S
                .Count = CShort(UBound(aFormat))
            End With
            FilePut(FileNumber, cFORMAT_COUNT_RECORD)

            Dim b As Byte
            Dim a As Integer
            For lIndex = LBound(aFormat) To UBound(aFormat)
                l = Len(aFormat(lIndex))
                With cFORMAT_RECORD
                    .opcode = &H1ES
                    .length = CShort(l + 1)
                    .FormatLenght = CByte(CShort(l))
                End With
                FilePut(FileNumber, cFORMAT_RECORD)

                'Then the actual format
                For a = 1 To l
                    b = CByte(Asc(Mid(aFormat(lIndex), a, 1)))
                    FilePut(FileNumber, b)
                Next
            Next lIndex

            Exit Function

        End Function


        Function MKI(ByRef x As Short) As String
            Dim temp As String
            'used for writing integer array values to the disk file
            temp = Space(2)
            CopyMemory(temp, CShort(x), 2)
            MKI = temp
        End Function


        Public Function SetDefaultRowHeight(ByRef HeightValue As Short) As Object

            On Error GoTo Write_Error

            'Height is defined in units of 1/20th of a point. Therefore, a 10-point font
            'would be 200 (i.e. 200/20 = 10). This function takes a HeightValue such as
            '14 point and converts it the correct size before writing it to the file.

            Dim DEFHEIGHT As ExcelFile.DEF_ROWHEIGHT_RECORD

            With DEFHEIGHT
                .opcode = 37
                .length = 2
                .RowHeight = CShort(HeightValue * 20) 'convert points to 1/20ths of point
            End With
            FilePut(FileNumber, DEFHEIGHT)

            SetDefaultRowHeight = 0

            Exit Function

Write_Error:
            SetDefaultRowHeight = Err.Number
            Exit Function

        End Function


        Public Function SetRowHeight(ByRef lrow As Integer, ByRef HeightValue As Short) As Object
            Dim Row As Short

            On Error GoTo Write_Error

            'the row and column values are written to the excel file as
            'unsigned integers. Therefore, must convert the longs to integer.

            If lrow > 32767 Then
                Row = CShort(lrow - 65536)
            Else
                Row = CShort(lrow - 1) 'rows/cols in Excel binary file are zero based
            End If


            'Height is defined in units of 1/20th of a point. Therefore, a 10-point font
            'would be 200 (i.e. 200/20 = 10). This function takes a HeightValue such as
            '14 point and converts it the correct size before writing it to the file.

            Dim ROWHEIGHTREC As ExcelFile.ROW_HEIGHT_RECORD

            With ROWHEIGHTREC
                .opcode = 8
                .length = 16
                .RowNumber = Row
                .FirstColumn = 0
                .LastColumn = 256
                .RowHeight = CShort(HeightValue * 20) 'convert points to 1/20ths of point
                .internal = 0
                .DefaultAttributes = 0
                .FileOffset = 0
                .rgbAttr1 = 0
                .rgbAttr2 = 0
                .rgbAttr3 = 0
            End With
            FilePut(FileNumber, ROWHEIGHTREC)

            SetRowHeight = 0

            Exit Function

Write_Error:
            SetRowHeight = Err.Number
            Exit Function

        End Function
    End Class

End Namespace

' ************************************************
' $History: ExcelFile.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 29.04.09   Time: 16:30
' Updated in $/CKAG/Base/Kernel/Excel
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Excel
' 
' *****************  Version 4  *****************
' User: Uha          Date: 15.05.07   Time: 11:04
' Updated in $/CKG/Base/Base/Kernel/Excel
' Änderungen aus StartApplication vom 11.05.2007 (Aspose-Tool,
' DataTableHelper.vb, Customer.vb)
' 
' ************************************************
