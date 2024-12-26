Imports System.Threading
Imports System.Runtime.InteropServices

Public Class MapleHelper
    ' 模擬鍵盤輸入
    <DllImport("user32.dll")>
    Public Shared Sub keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
    End Sub
    <StructLayout(LayoutKind.Sequential)>
    Public Structure INPUT
        Public type As Integer
        Public ki As KEYBDINPUT
        Public padding1 As Integer
        Public padding2 As Integer
    End Structure

    ' 定義結構：KEYBDINPUT
    <StructLayout(LayoutKind.Sequential)>
    Public Structure KEYBDINPUT
        Public wVk As UShort
        Public wScan As UShort
        Public dwFlags As UInteger
        Public time As UInteger
        Public dwExtraInfo As IntPtr
    End Structure

    ' 宣告 SendInput 函數
    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function SendInput(nInputs As Integer, pInputs As INPUT(), cbSize As Integer) As UInteger
    End Function

    ' SendInput 模擬按鍵按下
    Private Sub SimulateKeyDown(vkCode As UShort)
        Dim inputs(0) As INPUT
        inputs(0).type = 1 ' 設置為鍵盤輸入
        inputs(0).ki.wVk = vkCode
        inputs(0).ki.dwFlags = 0 ' 按下按鍵
        SendInput(1, inputs, Marshal.SizeOf(GetType(INPUT)))
    End Sub

    ' SendInput 模擬按鍵釋放
    Private Sub SimulateKeyUp(vkCode As UShort)
        Dim inputs(0) As INPUT
        inputs(0).type = 1 ' 設置為鍵盤輸入
        inputs(0).ki.wVk = vkCode
        inputs(0).ki.dwFlags = 2 ' 放開按鍵
        SendInput(1, inputs, Marshal.SizeOf(GetType(INPUT)))
    End Sub

    ' SendInput 模擬按鍵點擊（按下+釋放）
    Private Sub SimulateKeyPress(vkCode As UShort)
        SimulateKeyDown(vkCode)
        Threading.Thread.Sleep(50) ' 模擬真實按鍵的短暫延遲
        SimulateKeyUp(vkCode)
    End Sub
    Private Const KEYEVENTF_KEYDOWN As Integer = 0
    Private Const KEYEVENTF_KEYUP As Integer = &H2
    Private monitoring As Boolean = False ' 用於控制腳本啟動或停止
    Private currentCycle As Integer = 0 ' 當前完成的循環次數

    ' 初始化
    Private Sub MapleHelper_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler TextBoxSkillKey.KeyDown, AddressOf TextBoxSkillKey_KeyDown
    End Sub

    ' 技能鍵文本框按鍵事件
    Private Sub TextBoxSkillKey_KeyDown(sender As Object, e As KeyEventArgs)
        ' 獲取按下的按鍵鍵碼
        Dim keyCode As Integer = e.KeyCode
        Dim keyName As String = e.KeyCode.ToString()

        ' 顯示按鍵名稱和鍵碼
        TextBoxSkillKey.Text = $"{keyName} ({keyCode})"
    End Sub

    ' 啟動腳本按鈕邏輯
    Private Sub ButtonStartScript_Click(sender As Object, e As EventArgs) Handles ButtonStartScript.Click
        If Not monitoring Then
            monitoring = True
            currentCycle = 0
            Task.Run(AddressOf RunAutomation)
            UpdateStatus("腳本已啟動")
        Else
            UpdateStatus("腳本已經在運行")
        End If
    End Sub

    ' 停止腳本按鈕邏輯
    Private Sub ButtonStopScript_Click(sender As Object, e As EventArgs) Handles ButtonStopScript.Click
        monitoring = False
        UpdateStatus("腳本已停止")
    End Sub

    ' 掛機邏輯
    Private Sub RunAutomation()
        ' 獲取用戶設置的參數
        Dim resetPositionTime As Integer = GetThresholdValue(TextBoxLeftHoldTime.Text, 5000) ' 重置位置時間 (ms)
        Dim delayBetweenActions As Integer = GetThresholdValue(TextBoxDelayBetweenActions.Text, 1000) ' 動作延遲 (ms)
        Dim skillKey As Integer = ParseSkillKey() ' 技能按鍵
        Dim skillPressTimes As Integer = GetThresholdValue(TextBoxSkillPressTimes.Text, 5) ' 技能按鍵次數
        Dim skillInterval As Integer = GetThresholdValue(TextBoxSkillInterval.Text, 500) ' 技能按鍵間隔 (ms)
        Dim cycleCount As Integer = GetThresholdValue(TextBoxCycleCount.Text, 10) ' 總循環次數

        ' Step 1: 重置角色位置到最左邊
        SimulateKeyDown(&H25) ' 按住左鍵 (Arrow Left)
        Threading.Thread.Sleep(resetPositionTime)
        SimulateKeyUp(&H25) ' 放開左鍵
        Threading.Thread.Sleep(delayBetweenActions)

        ' Step 2: 循環執行掛機邏輯
        While monitoring AndAlso currentCycle < cycleCount
            ' Step 2.1: 面向左並執行技能
            SimulateKeyPress(&H25) ' 左鍵 (Arrow Left)
            Threading.Thread.Sleep(delayBetweenActions)

            For i As Integer = 1 To skillPressTimes
                SimulateKeyPress(skillKey) ' 用戶自定義的技能按鍵
                Threading.Thread.Sleep(skillInterval)
            Next

            ' Step 2.2: 面向右並執行技能
            SimulateKeyPress(&H27) ' 右鍵 (Arrow Right)
            Threading.Thread.Sleep(delayBetweenActions)

            For i As Integer = 1 To skillPressTimes
                SimulateKeyPress(skillKey) ' 用戶自定義的技能按鍵
                Threading.Thread.Sleep(skillInterval)
            Next

            ' 記錄循環次數
            currentCycle += 1
            UpdateStatus($"完成 {currentCycle}/{cycleCount} 次循環")
        End While

        ' 循環結束
        If currentCycle >= cycleCount Then
            UpdateStatus("腳本完成所有循環")
        End If
        monitoring = False
    End Sub




    ' 模擬長時間按住按鍵
    Private Sub SimulateKeyHold(keyCode As Integer, holdTime As Integer)
        keybd_event(keyCode, 0, KEYEVENTF_KEYDOWN, 0) ' 按下按鍵
        Thread.Sleep(holdTime) ' 按住指定時間
        keybd_event(keyCode, 0, KEYEVENTF_KEYUP, 0) ' 放開按鍵
    End Sub

    ' 模擬短按按鍵
    Private Sub SimulateKeyPress(keyCode As Integer)
        keybd_event(keyCode, 0, KEYEVENTF_KEYDOWN, 0) ' 按下按鍵
        Thread.Sleep(50) ' 短暫延遲模擬真實按鍵
        keybd_event(keyCode, 0, KEYEVENTF_KEYUP, 0) ' 放開按鍵
    End Sub

    ' 解析技能按鍵
    Private Function ParseSkillKey() As Integer
        Dim text As String = TextBoxSkillKey.Text
        If text.Contains("(") And text.Contains(")") Then
            Dim startIdx As Integer = text.IndexOf("(") + 1
            Dim endIdx As Integer = text.IndexOf(")")
            Dim keyCode As String = text.Substring(startIdx, endIdx - startIdx)
            Return Integer.Parse(keyCode)
        End If
        Return 65 ' 默認為 A 鍵
    End Function

    ' 解析文本框內容，獲取整數值
    Private Function GetThresholdValue(text As String, defaultValue As Integer) As Integer
        Dim value As Integer
        If Integer.TryParse(text, value) Then
            Return value
        Else
            Return defaultValue
        End If
    End Function

    ' 更新狀態標籤
    Private Sub UpdateStatus(message As String)
        If LabelStatus.InvokeRequired Then
            LabelStatus.Invoke(Sub() LabelStatus.Text = "狀態: " & message)
        Else
            LabelStatus.Text = "狀態: " & message
        End If
    End Sub
End Class
