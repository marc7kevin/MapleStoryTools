Imports System.IO.Ports

Public Class CommandSender
    Private SerialPort As SerialPort

    Public Sub New(serialPort As SerialPort)
        Me.SerialPort = serialPort
    End Sub

    Public Async Function SendCommandAsync(commandId As Byte, ParamArray args() As Byte) As Task
        Dim packet As New List(Of Byte) From {&HFE, commandId}
        packet.AddRange(args)
        packet.Add(&HFF)

        Console.WriteLine("Sending packet: " & BitConverter.ToString(packet.ToArray()))

        If SerialPort Is Nothing OrElse Not SerialPort.IsOpen Then
            Throw New InvalidOperationException("SerialPort 尚未開啟。")
        End If

        SerialPort.Write(packet.ToArray(), 0, packet.Count)
        Await Task.Run(Sub()
                           Dim line As String = ""
                           Do
                               line = SerialPort.ReadLine().Trim()
                           Loop While line <> "DONE"
                       End Sub)
    End Function

    ' Press
    Public Async Function ExecutePressAsync(key As String, holdTime As Integer) As Task
        Dim keyByte As Byte = GetKeyByte(key)
        Dim highByte = CByte((holdTime >> 8) And &HFF)
        Dim lowByte = CByte(holdTime And &HFF)
        Await SendCommandAsync(&H3, keyByte, highByte, lowByte)
    End Function

    ' Combo
    Public Async Function ExecuteComboAsync(key1 As String, key2 As String, holdTime As Integer) As Task
        Dim key1Byte As Byte = GetKeyByte(key1)
        Dim key2Byte As Byte = GetKeyByte(key2)
        Dim highByte = CByte((holdTime >> 8) And &HFF)
        Dim lowByte = CByte(holdTime And &HFF)
        Await SendCommandAsync(&HA, key1Byte, key2Byte, highByte, lowByte)
    End Function

    ' Mouse
    Public Async Function ExecuteMouseClickAsync(clickType As Integer, holdTime As Integer) As Task
        Dim button As Byte
        Select Case clickType
            Case 1 : button = &H1
            Case 2 : button = &H2
            Case 3 : button = &H3
            Case Else : button = &H1
        End Select

        Dim highByte = CByte((holdTime >> 8) And &HFF)
        Dim lowByte = CByte(holdTime And &HFF)
        Await SendCommandAsync(&H6, button, highByte, lowByte)
    End Function
    Public Async Function ExecuteMouseMoveAsync(x As Integer, y As Integer) As Task
        Dim xBytes = BitConverter.GetBytes(x)
        Dim yBytes = BitConverter.GetBytes(y)
        Array.Reverse(xBytes)
        Array.Reverse(yBytes)
        Await SendCommandAsync(&H5, xBytes(0), xBytes(1), yBytes(0), yBytes(1))
    End Function

    Public Async Function InitMouseAsync(x As Integer, y As Integer) As Task
        Dim xBytes = BitConverter.GetBytes(x)
        Dim yBytes = BitConverter.GetBytes(y)
        Array.Reverse(xBytes)
        Array.Reverse(yBytes)
        Await SendCommandAsync(&H4, xBytes(0), xBytes(1), yBytes(0), yBytes(1))
    End Function
    Private Function GetKeyByte(key As String) As Byte
        Select Case key.ToLower()
            Case "left"
                Return &H50
            Case "up"
                Return &H52
            Case "right"
                Return &H4F
            Case "down"
                Return &H51

            Case "a" To "z"
                Return CByte(AscW(key.ToUpper()) - AscW("A"c) + &H4)

            Case "0"
                Return &H27
            Case "1" To "9"
                Return CByte(AscW(key) - AscW("1"c) + &H1E)

            Case "f1" To "f12"
                Dim fNumber As Integer = Integer.Parse(key.Substring(1))
                Return CByte(&H3A + fNumber - 1)

            Case "ctrl"
                Return &HE0 ' 左Ctrl
            Case "shift"
                Return &HE1 ' 左Shift
            Case "alt"
                Return &HE2 ' 左Alt
            Case "leftalt", "lalt"
                Return &HE2 ' 左Alt
            Case "rightalt", "ralt"
                Return &HE6 ' 右Alt

            Case Else
                Throw New ArgumentException($"未知的按鍵名稱: {key}")
        End Select
    End Function
End Class
