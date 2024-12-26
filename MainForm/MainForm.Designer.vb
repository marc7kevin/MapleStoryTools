<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        TxtLoopCount = New TextBox()
        ChkLoop = New CheckBox()
        BtnCancelRun = New Button()
        BtnRunScript = New Button()
        ComConnect = New Button()
        ComSelect = New ComboBox()
        Run_status = New Label()
        BtnLoadScript = New Button()
        SuspendLayout()
        ' 
        ' TxtLoopCount
        ' 
        TxtLoopCount.Location = New Point(155, 87)
        TxtLoopCount.Name = "TxtLoopCount"
        TxtLoopCount.Size = New Size(50, 23)
        TxtLoopCount.TabIndex = 15
        ' 
        ' ChkLoop
        ' 
        ChkLoop.AutoSize = True
        ChkLoop.Location = New Point(211, 89)
        ChkLoop.Name = "ChkLoop"
        ChkLoop.Size = New Size(50, 19)
        ChkLoop.TabIndex = 14
        ChkLoop.Text = "循環"
        ChkLoop.UseVisualStyleBackColor = True
        ' 
        ' BtnCancelRun
        ' 
        BtnCancelRun.Location = New Point(282, 48)
        BtnCancelRun.Name = "BtnCancelRun"
        BtnCancelRun.Size = New Size(65, 35)
        BtnCancelRun.TabIndex = 13
        BtnCancelRun.Text = "停止腳本"
        BtnCancelRun.UseVisualStyleBackColor = True
        ' 
        ' BtnRunScript
        ' 
        BtnRunScript.Location = New Point(211, 48)
        BtnRunScript.Name = "BtnRunScript"
        BtnRunScript.Size = New Size(65, 35)
        BtnRunScript.TabIndex = 12
        BtnRunScript.Text = "運行腳本"
        BtnRunScript.UseVisualStyleBackColor = True
        ' 
        ' ComConnect
        ' 
        ComConnect.Location = New Point(93, 55)
        ComConnect.Name = "ComConnect"
        ComConnect.Size = New Size(41, 23)
        ComConnect.TabIndex = 11
        ComConnect.Text = "連接"
        ComConnect.UseVisualStyleBackColor = True
        ' 
        ' ComSelect
        ' 
        ComSelect.FormattingEnabled = True
        ComSelect.Location = New Point(12, 55)
        ComSelect.Name = "ComSelect"
        ComSelect.Size = New Size(75, 23)
        ComSelect.TabIndex = 10
        ' 
        ' Run_status
        ' 
        Run_status.AutoSize = True
        Run_status.Font = New Font("Microsoft JhengHei UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(136))
        Run_status.Location = New Point(12, 9)
        Run_status.Name = "Run_status"
        Run_status.Size = New Size(140, 30)
        Run_status.TabIndex = 9
        Run_status.Text = "Dashboard"
        ' 
        ' BtnLoadScript
        ' 
        BtnLoadScript.Location = New Point(140, 48)
        BtnLoadScript.Name = "BtnLoadScript"
        BtnLoadScript.Size = New Size(65, 35)
        BtnLoadScript.TabIndex = 8
        BtnLoadScript.Text = "載入腳本"
        BtnLoadScript.UseVisualStyleBackColor = True
        ' 
        ' MainForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(370, 124)
        Controls.Add(TxtLoopCount)
        Controls.Add(ChkLoop)
        Controls.Add(BtnCancelRun)
        Controls.Add(BtnRunScript)
        Controls.Add(ComConnect)
        Controls.Add(ComSelect)
        Controls.Add(Run_status)
        Controls.Add(BtnLoadScript)
        Name = "MainForm"
        Text = "MainForm"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents TxtLoopCount As TextBox
    Friend WithEvents ChkLoop As CheckBox
    Friend WithEvents BtnCancelRun As Button
    Friend WithEvents BtnRunScript As Button
    Friend WithEvents ComConnect As Button
    Friend WithEvents ComSelect As ComboBox
    Friend WithEvents Run_status As Label
    Friend WithEvents BtnLoadScript As Button
End Class
