Imports System.IO.Ports
Imports System.IO
Imports System.Threading
Imports System.ComponentModel

Public Class MainForm
    Private commandSender As CommandSender
    Private SerialPort As SerialPort
    Private Monitor As ExecutionMonitor
    Private LoadedActions As List(Of Func(Of Task))
    Private executionTokenSource As CancellationTokenSource
    Private nameChangeTimer As Timer

    Public Shared Function IsInDesignMode() As Boolean
        Return LicenseManager.UsageMode = LicenseUsageMode.Designtime
    End Function

    Private Async Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If IsInDesignMode() Then Return
        nameChangeTimer = New Timer(AddressOf OnNameChangeTimerElapsed, Nothing, 0, 10000)
        Await InitializeRuntimeResources()
    End Sub
    Private Function GenerateRandomProgramName(minWords As Integer, maxWords As Integer) As String
        Dim wordDictionary As String() = {
            "apple", "orange", "table", "chair", "window", "flower", "river", "mountain", "ocean", "beach",
            "forest", "desert", "camera", "guitar", "piano", "mirror", "bottle", "castle", "garden", "dragon",
            "rainbow", "thunder", "candle", "coffee", "school", "teacher", "student", "library", "computer", "keyboard",
            "screen", "picture", "summer", "winter", "autumn", "spring", "bicycle", "station", "airport", "journey",
            "vacation", "suitcase", "passport", "ticket", "subway", "tunnel", "village", "city", "island", "meadow",
            "valley", "galaxy", "universe", "planet", "rocket", "astronaut", "science", "engineer", "doctor", "lawyer",
            "artist", "dancer", "singer", "writer", "poet", "leader", "friend", "family", "brother", "sister",
            "father", "mother", "cousin", "uncle", "aunt", "nephew", "niece", "elephant", "tiger", "lion", "zebra",
            "monkey", "parrot", "whale", "dolphin", "penguin", "butterfly", "rainbow", "sunlight", "raindrop",
            "moonlight", "starlight", "diamond", "emerald", "sapphire", "ruby", "gold", "silver", "pearl", "treasure"
        }

        Dim random As New Random()
        Dim wordCount As Integer = random.Next(minWords, maxWords + 1)
        Dim programName As New List(Of String)()

        For i As Integer = 1 To wordCount
            Dim index As Integer = random.Next(0, wordDictionary.Length)
            programName.Add(wordDictionary(index))
        Next
        Return String.Join(" ", programName)
    End Function
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If nameChangeTimer IsNot Nothing Then
            nameChangeTimer.Dispose()
        End If
        MyBase.OnFormClosing(e)
    End Sub
    Private Sub OnNameChangeTimerElapsed(state As Object)
        Me.Invoke(New Action(Sub()
                                 Me.Text = GenerateRandomProgramName(9, 16)
                             End Sub))
    End Sub
    Private Async Function InitializeRuntimeResources() As Task
        Try
            Dim ports As String() = SerialPort.GetPortNames()
            ComSelect.Items.Clear()
            ComSelect.Items.AddRange(ports)

            If ComSelect.Items.Count > 0 Then
                ComSelect.SelectedIndex = 0
            Else
                MessageBox.Show("無可用的 COM 接口，請檢查硬體連接。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show($"獲取 COM 端口時出現錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function


    Public Sub New()
        InitializeComponent()
        If IsInDesignMode() Then Return
        Monitor = New ExecutionMonitor(Run_status)
    End Sub

    Private Sub InitializeSerialPort()
        If IsInDesignMode() Then Return

        Try
            Dim selectedPort As String = ComSelect.SelectedItem.ToString()
            SerialPort = New SerialPort(selectedPort, 115200, Parity.None, 8, StopBits.One)
            SerialPort.Open()
            SerialPort.DtrEnable = True
            commandSender = New CommandSender(SerialPort)

            ComConnect.Text = "斷開"
            BtnRunScript.Enabled = True
        Catch ex As Exception
            MessageBox.Show($"無法連接到 COM 接口：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ReleaseSerialPort()
    End Sub

    Private Sub ReleaseSerialPort()
        If IsInDesignMode() Then Return

        If SerialPort IsNot Nothing Then
            If SerialPort.IsOpen Then
                SerialPort.Close()
            End If
            SerialPort.Dispose()
            BtnRunScript.Enabled = False
            SerialPort = Nothing
        End If
    End Sub

    Private Sub ComConnect_Click(sender As Object, e As EventArgs) Handles ComConnect.Click
        Try
            If ComSelect.SelectedItem Is Nothing Then
                MessageBox.Show("請先選擇一個 COM 接口。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If SerialPort IsNot Nothing AndAlso SerialPort.IsOpen Then
                SerialPort.Close()
                SerialPort.Dispose()
                SerialPort = Nothing
                commandSender = Nothing
                ComConnect.Text = "連接"
                BtnLoadScript.Enabled = False
                BtnRunScript.Enabled = False
                Return
            End If

            Dim selectedPort As String = ComSelect.SelectedItem.ToString()
            SerialPort = New SerialPort(selectedPort, 115200, Parity.None, 8, StopBits.One)
            SerialPort.Open()
            SerialPort.DtrEnable = True
            commandSender = New CommandSender(SerialPort)

            ComConnect.Text = "斷開"
            BtnLoadScript.Enabled = True
            BtnRunScript.Enabled = False
        Catch ex As Exception
            MessageBox.Show($"無法連接到 COM 接口：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnLoadScript_Click(sender As Object, e As EventArgs) Handles BtnLoadScript.Click
        If SerialPort Is Nothing OrElse Not SerialPort.IsOpen Then
            MessageBox.Show("請先連接到 Arduino 再載入腳本。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim openFileDialog As New OpenFileDialog() With {
            .Filter = "腳本文件 (*.txt)|*.txt",
            .Title = "選擇掛機腳本"
        }

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                Dim script = File.ReadAllText(openFileDialog.FileName)
                LoadedActions = ScriptParser.ParseScript(script, commandSender, Monitor)

                If LoadedActions IsNot Nothing AndAlso LoadedActions.Count > 0 Then
                    Monitor.UpdateStatus("腳本已載入，等待執行")
                    BtnRunScript.Enabled = True
                Else
                    Monitor.UpdateStatus("腳本載入失敗或內容為空")
                    BtnRunScript.Enabled = False
                End If
            Catch ex As Exception
                MessageBox.Show($"載入腳本時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Monitor.UpdateStatus("腳本載入失敗")
                BtnRunScript.Enabled = False
            End Try
        End If
    End Sub

    Private Async Sub BtnRunScript_Click(sender As Object, e As EventArgs) Handles BtnRunScript.Click
        If LoadedActions Is Nothing OrElse LoadedActions.Count = 0 Then
            MessageBox.Show("請先載入腳本。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        BtnRunScript.Enabled = False
        BtnLoadScript.Enabled = False
        ComConnect.Enabled = False
        BtnCancelRun.Enabled = True

        executionTokenSource = New CancellationTokenSource()
        Dim token = executionTokenSource.Token

        Dim loopCount As Integer = 1
        If ChkLoop.Checked Then
            If Not Integer.TryParse(TxtLoopCount.Text, loopCount) OrElse loopCount < 1 Then
                loopCount = 1
            End If
        End If

        Try
            Monitor.UpdateStatus("準備執行腳本，等待 2 秒...")
            Await Task.Delay(2000, token)

            For i As Integer = 1 To loopCount
                If loopCount > 1 Then
                    Monitor.UpdateStatus($"腳本執行中...(第 {i} 次，共 {loopCount} 次)")
                Else
                    Monitor.UpdateStatus("腳本執行中...")
                End If

                For Each action In LoadedActions
                    token.ThrowIfCancellationRequested()
                    Await action.Invoke()
                Next
            Next

            Monitor.UpdateStatus("腳本執行完成")

        Catch ex As OperationCanceledException
            Monitor.UpdateStatus("腳本執行已取消")
            MessageBox.Show("腳本執行已被取消。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Monitor.UpdateStatus($"腳本執行失敗：{ex.Message}")
            MessageBox.Show($"執行腳本時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            BtnRunScript.Enabled = True
            BtnLoadScript.Enabled = True
            ComConnect.Enabled = True
            BtnCancelRun.Enabled = False
        End Try
    End Sub

    Private Sub BtnCancelRun_Click(sender As Object, e As EventArgs) Handles BtnCancelRun.Click
        If executionTokenSource IsNot Nothing AndAlso Not executionTokenSource.IsCancellationRequested Then
            executionTokenSource.Cancel()
        End If
    End Sub
End Class
