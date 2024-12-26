Imports System.Text.RegularExpressions

Public Class ScriptParser
    Private Shared ReadOnly rnd As New Random()

    Public Shared Function ParseScript(script As String, sender As CommandSender, monitor As ExecutionMonitor) As List(Of Func(Of Task))
        Dim actions As New List(Of Func(Of Task))()
        Dim lines = script.Split({Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)

        Dim isInMayBlock As Boolean = False
        Dim mayBlockActions As New List(Of Func(Of Task))()
        Dim mayProbability As Integer = 0

        For lineNumber As Integer = 1 To lines.Length
            Dim line = lines(lineNumber - 1).Trim()

            If line.StartsWith("#") OrElse String.IsNullOrWhiteSpace(line) Then
                Continue For
            End If

            Dim parts = Regex.Split(line, "\s+")
            If parts.Length = 0 Then
                Continue For
            End If

            Dim command = parts(0).ToLower()
            Dim action As Func(Of Task) = Nothing

            Select Case command
                Case "may"
                    If isInMayBlock Then
                        monitor.UpdateStatus($"行 {lineNumber}: 未關閉的 'may' 區塊中嵌套另一個 'may'。")
                        Continue For
                    End If

                    If parts.Length < 2 OrElse Not Integer.TryParse(parts(1), mayProbability) OrElse mayProbability < 0 OrElse mayProbability > 100 Then
                        monitor.UpdateStatus($"行 {lineNumber}: 'may' 指令缺少有效的百分比參數 (0-100)。")
                        Continue For
                    End If

                    isInMayBlock = True
                    mayBlockActions.Clear()
                    Continue For

                Case "mayend"
                    If Not isInMayBlock Then
                        monitor.UpdateStatus($"行 {lineNumber}: 找到 'mayend' 但未對應 'may'。")
                        Continue For
                    End If

                    isInMayBlock = False
                    Dim probability = mayProbability
                    actions.Add(Async Function()
                                    If rnd.Next(0, 100) < probability Then
                                        monitor.UpdateStatus($"機率塊執行: {probability}%")
                                        For Each action In mayBlockActions
                                            Await action()
                                        Next
                                    Else
                                        monitor.UpdateStatus($"機率塊未執行: {probability}%")
                                    End If
                                End Function)
                    Continue For

                Case "press"
                    If parts.Length < 2 Then
                        monitor.UpdateStatus($"行 {lineNumber}: 'Press' 指令缺少按鍵參數。")
                        Continue For
                    End If
                    Dim key = parts(1)
                    Dim time As Integer
                    If parts.Length >= 3 Then
                        If Not TryParseTime(parts(2), time) Then
                            monitor.UpdateStatus($"行 {lineNumber}: 無效的時間 '{parts(2)}'。")
                            Continue For
                        End If
                    Else
                        time = 100
                    End If

                    action = Async Function()
                                 Try
                                     monitor.UpdateStatus(monitor.TranslateAction(line))
                                     Await sender.ExecutePressAsync(key, time)
                                 Catch ex As Exception
                                     monitor.UpdateStatus($"行 {lineNumber}: 執行 'Press' 錯誤：{ex.Message}")
                                 End Try
                             End Function

                Case "combo"
                    If parts.Length < 3 Then
                        monitor.UpdateStatus($"行 {lineNumber}: 'Combo' 指令缺少按鍵參數。")
                        Continue For
                    End If
                    Dim key1 = parts(1)
                    Dim key2 = parts(2)
                    Dim comboTime As Integer
                    If parts.Length >= 4 Then
                        If Not TryParseTime(parts(3), comboTime) Then
                            monitor.UpdateStatus($"行 {lineNumber}: 無效的時間 '{parts(3)}'。")
                            Continue For
                        End If
                    Else
                        comboTime = 100
                    End If

                    action = Async Function()
                                 Try
                                     monitor.UpdateStatus(monitor.TranslateAction(line))
                                     Await sender.ExecuteComboAsync(key1, key2, comboTime)
                                 Catch ex As Exception
                                     monitor.UpdateStatus($"行 {lineNumber}: 執行 'Combo' 錯誤：{ex.Message}")
                                 End Try
                             End Function

                Case "mouse"
                    If parts.Length < 2 Then
                        monitor.UpdateStatus($"行 {lineNumber}: 'Mouse' 指令缺少活動參數。")
                        Continue For
                    End If
                    Dim activity = parts(1).ToLower()

                    If activity = "move" Then
                        If parts.Length < 3 Then
                            monitor.UpdateStatus($"行 {lineNumber}: 'Mouse Move' 指令缺少座標參數。")
                            Continue For
                        End If
                        Dim coords = parts(2).Split(","c)
                        If coords.Length <> 2 OrElse Not Integer.TryParse(coords(0), Nothing) OrElse Not Integer.TryParse(coords(1), Nothing) Then
                            monitor.UpdateStatus($"行 {lineNumber}: 無效的座標 '{parts(2)}'。")
                            Continue For
                        End If
                        Dim x As Integer = Integer.Parse(coords(0))
                        Dim y As Integer = Integer.Parse(coords(1))

                        action = Async Function()
                                     Try
                                         monitor.UpdateStatus(monitor.TranslateAction(line))
                                         Await sender.ExecuteMouseMoveAsync(x, y)
                                     Catch ex As Exception
                                         monitor.UpdateStatus($"行 {lineNumber}: 執行 'Mouse Move' 錯誤：{ex.Message}")
                                     End Try
                                 End Function

                    ElseIf activity = "click" Then
                        If parts.Length < 3 Then
                            monitor.UpdateStatus($"行 {lineNumber}: 'Mouse Click' 指令缺少點擊次數參數。")
                            Continue For
                        End If
                        Dim clicks As Integer
                        If Not Integer.TryParse(parts(2), clicks) Then
                            monitor.UpdateStatus($"行 {lineNumber}: 無效的點擊次數 '{parts(2)}'。")
                            Continue For
                        End If
                        Dim clickTime As Integer = 50
                        If parts.Length >= 4 Then
                            If Not TryParseTime(parts(3), clickTime) Then
                                monitor.UpdateStatus($"行 {lineNumber}: 無效的時間 '{parts(3)}'。")
                                Continue For
                            End If
                        End If

                        action = Async Function()
                                     Try
                                         monitor.UpdateStatus(monitor.TranslateAction(line))
                                         Await sender.ExecuteMouseClickAsync(clicks, clickTime)
                                     Catch ex As Exception
                                         monitor.UpdateStatus($"行 {lineNumber}: 執行 'Mouse Click' 錯誤：{ex.Message}")
                                     End Try
                                 End Function
                    Else
                        monitor.UpdateStatus($"行 {lineNumber}: 未知的 'Mouse' 活動 '{activity}'。")
                        Continue For
                    End If

                Case "wait"
                    If parts.Length < 2 Then
                        monitor.UpdateStatus($"行 {lineNumber}: 'Wait' 指令缺少時間參數。")
                        Continue For
                    End If
                    Dim waitTime As Integer
                    If Not TryParseTime(parts(1), waitTime) Then
                        monitor.UpdateStatus($"行 {lineNumber}: 無效的等待時間 '{parts(1)}'。")
                        Continue For
                    End If

                    action = Async Function()
                                 Try
                                     monitor.UpdateStatus(monitor.TranslateAction(line))
                                     Await Task.Delay(waitTime)
                                 Catch ex As Exception
                                     monitor.UpdateStatus($"行 {lineNumber}: 執行 'Wait' 錯誤：{ex.Message}")
                                 End Try
                             End Function

                Case Else
                    monitor.UpdateStatus($"行 {lineNumber}: 未知指令 '{command}'。")
                    Continue For
            End Select

            If action IsNot Nothing Then
                If isInMayBlock Then
                    mayBlockActions.Add(action)
                Else
                    actions.Add(action)
                End If
            End If
        Next

        If isInMayBlock Then
            monitor.UpdateStatus("腳本結束時發現未封閉的 'may' 區塊。")
        End If

        Return actions
    End Function

    Private Shared Function TryParseTime(timeSpec As String, ByRef time As Integer) As Boolean
        Try
            If timeSpec.Contains("-") Then
                Dim range = timeSpec.Split("-"c)
                If range.Length <> 2 Then Return False

                Dim min, max As Integer
                If Not Integer.TryParse(range(0), min) OrElse Not Integer.TryParse(range(1), max) Then
                    Return False
                End If

                If min > max Then
                    Throw New ArgumentException("'minValue' cannot be greater than 'maxValue'.")
                End If

                time = RandomRange(min, max)
                Return True
            Else
                Return Integer.TryParse(timeSpec, time)
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Shared Function RandomRange(min As Integer, max As Integer) As Integer
        Return rnd.Next(min, max + 1)
    End Function
End Class
