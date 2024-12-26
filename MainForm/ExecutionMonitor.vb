Public Class ExecutionMonitor
    Private StatusLabel As Label

    Public Sub New(statusLabel As Label)
        Me.StatusLabel = statusLabel
    End Sub

    Public Sub UpdateStatus(action As String)
        If StatusLabel.InvokeRequired Then
            StatusLabel.Invoke(Sub() StatusLabel.Text = action)
        Else
            StatusLabel.Text = action
        End If
    End Sub

    Public Function TranslateAction(line As String) As String
        Dim parts = line.Split(" "c, StringSplitOptions.RemoveEmptyEntries)
        If parts.Length = 0 Then Return "未知指令"

        Dim command = parts(0).ToLower()

        Select Case command
            Case "press"
                If parts.Length < 2 Then Return "未知指令: " & line
                Dim key = parts(1)
                Dim time = If(parts.Length >= 3, parts(2), "100")
                Return $"按下鍵盤 {key} 並保持 {time} 毫秒"

            Case "combo"
                If parts.Length < 3 Then Return "未知指令: " & line
                Dim key1 = parts(1)
                Dim key2 = parts(2)
                Dim time = If(parts.Length >= 4, parts(3), "100")
                Return $"按下組合鍵 {key1} + {key2} 並保持 {time} 毫秒"

            Case "mouse"
                If parts.Length < 2 Then Return "未知指令: " & line
                Dim activity = parts(1).ToLower()
                If activity = "move" Then
                    If parts.Length < 3 Then Return "未知指令: " & line
                    Dim coords = parts(2).Split(","c)
                    If coords.Length <> 2 Then Return "未知指令: " & line
                    Return $"移動滑鼠至 ({coords(0)}, {coords(1)})"
                ElseIf activity = "click" Then
                    If parts.Length < 3 Then Return "未知指令: " & line
                    Dim clicks = parts(2)
                    Dim t = If(parts.Length >= 4, parts(3), "50")
                    Return $"滑鼠點擊 {clicks} 次並保持 {t} 毫秒"
                Else
                    Return $"未知的 'Mouse' 活動 '{activity}'"
                End If

            Case "wait"
                If parts.Length < 2 Then
                    Return "未知指令: " & line
                End If
                Dim t = parts(1)
                Return $"等待 {t} 毫秒"

            Case Else
                Return $"未知指令: {line}"
        End Select
    End Function
End Class
