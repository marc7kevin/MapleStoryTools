<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MapleHelper
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        LabelStatus = New Label()
        LabelHP = New Label()
        TextBoxHPThreshold = New TextBox()
        LabelMP = New Label()
        TextBoxMPThreshold = New TextBox()
        ButtonStartScript = New Button()
        ButtonStopScript = New Button()
        LabelLeftHoldTime = New Label()
        TextBoxLeftHoldTime = New TextBox()
        LabelDelay = New Label()
        TextBoxDelayBetweenActions = New TextBox()
        LabelSkillKey = New Label()
        TextBoxSkillKey = New TextBox()
        LabelSkillTimes = New Label()
        TextBoxSkillPressTimes = New TextBox()
        LabelSkillInterval = New Label()
        TextBoxSkillInterval = New TextBox()
        LabelCycleCount = New Label()
        TextBoxCycleCount = New TextBox()
        SuspendLayout()
        ' 
        ' LabelStatus
        ' 
        LabelStatus.AutoSize = True
        LabelStatus.Location = New Point(10, 10)
        LabelStatus.Name = "LabelStatus"
        LabelStatus.Size = New Size(97, 15)
        LabelStatus.TabIndex = 0
        LabelStatus.Text = "狀態: 腳本未啟動"
        ' 
        ' LabelHP
        ' 
        LabelHP.AutoSize = True
        LabelHP.Location = New Point(10, 40)
        LabelHP.Name = "LabelHP"
        LabelHP.Size = New Size(36, 15)
        LabelHP.TabIndex = 1
        LabelHP.Text = "HP: 0"
        ' 
        ' TextBoxHPThreshold
        ' 
        TextBoxHPThreshold.Location = New Point(96, 37)
        TextBoxHPThreshold.Name = "TextBoxHPThreshold"
        TextBoxHPThreshold.Size = New Size(45, 23)
        TextBoxHPThreshold.TabIndex = 2
        TextBoxHPThreshold.Text = "1000"
        ' 
        ' LabelMP
        ' 
        LabelMP.AutoSize = True
        LabelMP.Location = New Point(10, 70)
        LabelMP.Name = "LabelMP"
        LabelMP.Size = New Size(39, 15)
        LabelMP.TabIndex = 3
        LabelMP.Text = "MP: 0"
        ' 
        ' TextBoxMPThreshold
        ' 
        TextBoxMPThreshold.Location = New Point(96, 67)
        TextBoxMPThreshold.Name = "TextBoxMPThreshold"
        TextBoxMPThreshold.Size = New Size(45, 23)
        TextBoxMPThreshold.TabIndex = 4
        TextBoxMPThreshold.Text = "20"
        ' 
        ' ButtonStartScript
        ' 
        ButtonStartScript.Location = New Point(10, 100)
        ButtonStartScript.Name = "ButtonStartScript"
        ButtonStartScript.Size = New Size(82, 30)
        ButtonStartScript.TabIndex = 5
        ButtonStartScript.Text = "啟動腳本"
        ButtonStartScript.UseVisualStyleBackColor = True
        ' 
        ' ButtonStopScript
        ' 
        ButtonStopScript.Location = New Point(96, 100)
        ButtonStopScript.Name = "ButtonStopScript"
        ButtonStopScript.Size = New Size(82, 30)
        ButtonStopScript.TabIndex = 6
        ButtonStopScript.Text = "停止腳本"
        ButtonStopScript.UseVisualStyleBackColor = True
        ' 
        ' LabelLeftHoldTime
        ' 
        LabelLeftHoldTime.AutoSize = True
        LabelLeftHoldTime.Location = New Point(10, 150)
        LabelLeftHoldTime.Name = "LabelLeftHoldTime"
        LabelLeftHoldTime.Size = New Size(85, 15)
        LabelLeftHoldTime.TabIndex = 7
        LabelLeftHoldTime.Text = "起始定位 (ms):"
        ' 
        ' TextBoxLeftHoldTime
        ' 
        TextBoxLeftHoldTime.Location = New Point(124, 147)
        TextBoxLeftHoldTime.Name = "TextBoxLeftHoldTime"
        TextBoxLeftHoldTime.Size = New Size(80, 23)
        TextBoxLeftHoldTime.TabIndex = 8
        TextBoxLeftHoldTime.Text = "5000"
        ' 
        ' LabelDelay
        ' 
        LabelDelay.AutoSize = True
        LabelDelay.Location = New Point(10, 180)
        LabelDelay.Name = "LabelDelay"
        LabelDelay.Size = New Size(61, 15)
        LabelDelay.TabIndex = 9
        LabelDelay.Text = "延遲 (ms):"
        ' 
        ' TextBoxDelayBetweenActions
        ' 
        TextBoxDelayBetweenActions.Location = New Point(124, 177)
        TextBoxDelayBetweenActions.Name = "TextBoxDelayBetweenActions"
        TextBoxDelayBetweenActions.Size = New Size(80, 23)
        TextBoxDelayBetweenActions.TabIndex = 10
        TextBoxDelayBetweenActions.Text = "100"
        ' 
        ' LabelSkillKey
        ' 
        LabelSkillKey.AutoSize = True
        LabelSkillKey.Location = New Point(10, 210)
        LabelSkillKey.Name = "LabelSkillKey"
        LabelSkillKey.Size = New Size(86, 15)
        LabelSkillKey.TabIndex = 11
        LabelSkillKey.Text = "技能鍵 (ASCII):"
        ' 
        ' TextBoxSkillKey
        ' 
        TextBoxSkillKey.Location = New Point(124, 207)
        TextBoxSkillKey.Name = "TextBoxSkillKey"
        TextBoxSkillKey.Size = New Size(80, 23)
        TextBoxSkillKey.TabIndex = 12
        TextBoxSkillKey.Text = "65"
        ' 
        ' LabelSkillTimes
        ' 
        LabelSkillTimes.AutoSize = True
        LabelSkillTimes.Location = New Point(10, 240)
        LabelSkillTimes.Name = "LabelSkillTimes"
        LabelSkillTimes.Size = New Size(82, 15)
        LabelSkillTimes.TabIndex = 13
        LabelSkillTimes.Text = "技能施放次數:"
        ' 
        ' TextBoxSkillPressTimes
        ' 
        TextBoxSkillPressTimes.Location = New Point(124, 237)
        TextBoxSkillPressTimes.Name = "TextBoxSkillPressTimes"
        TextBoxSkillPressTimes.Size = New Size(80, 23)
        TextBoxSkillPressTimes.TabIndex = 14
        TextBoxSkillPressTimes.Text = "9"
        ' 
        ' LabelSkillInterval
        ' 
        LabelSkillInterval.AutoSize = True
        LabelSkillInterval.Location = New Point(10, 270)
        LabelSkillInterval.Name = "LabelSkillInterval"
        LabelSkillInterval.Size = New Size(85, 15)
        LabelSkillInterval.TabIndex = 15
        LabelSkillInterval.Text = "技能間隔 (ms):"
        ' 
        ' TextBoxSkillInterval
        ' 
        TextBoxSkillInterval.Location = New Point(124, 267)
        TextBoxSkillInterval.Name = "TextBoxSkillInterval"
        TextBoxSkillInterval.Size = New Size(80, 23)
        TextBoxSkillInterval.TabIndex = 16
        TextBoxSkillInterval.Text = "500"
        ' 
        ' LabelCycleCount
        ' 
        LabelCycleCount.AutoSize = True
        LabelCycleCount.Location = New Point(10, 300)
        LabelCycleCount.Name = "LabelCycleCount"
        LabelCycleCount.Size = New Size(58, 15)
        LabelCycleCount.TabIndex = 17
        LabelCycleCount.Text = "腳本循環:"
        ' 
        ' TextBoxCycleCount
        ' 
        TextBoxCycleCount.Location = New Point(124, 297)
        TextBoxCycleCount.Name = "TextBoxCycleCount"
        TextBoxCycleCount.Size = New Size(80, 23)
        TextBoxCycleCount.TabIndex = 18
        TextBoxCycleCount.Text = "99999"
        ' 
        ' MapleHelper
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(400, 350)
        Controls.Add(TextBoxCycleCount)
        Controls.Add(LabelCycleCount)
        Controls.Add(TextBoxSkillInterval)
        Controls.Add(LabelSkillInterval)
        Controls.Add(TextBoxSkillPressTimes)
        Controls.Add(LabelSkillTimes)
        Controls.Add(TextBoxSkillKey)
        Controls.Add(LabelSkillKey)
        Controls.Add(TextBoxDelayBetweenActions)
        Controls.Add(LabelDelay)
        Controls.Add(TextBoxLeftHoldTime)
        Controls.Add(LabelLeftHoldTime)
        Controls.Add(ButtonStopScript)
        Controls.Add(ButtonStartScript)
        Controls.Add(TextBoxMPThreshold)
        Controls.Add(LabelMP)
        Controls.Add(TextBoxHPThreshold)
        Controls.Add(LabelHP)
        Controls.Add(LabelStatus)
        Name = "MapleHelper"
        Text = "MapleHelper V1.0.0"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents LabelStatus As Label
    Friend WithEvents LabelHP As Label
    Friend WithEvents TextBoxHPThreshold As TextBox
    Friend WithEvents LabelMP As Label
    Friend WithEvents TextBoxMPThreshold As TextBox
    Friend WithEvents ButtonStartScript As Button
    Friend WithEvents ButtonStopScript As Button
    Friend WithEvents LabelLeftHoldTime As Label
    Friend WithEvents TextBoxLeftHoldTime As TextBox
    Friend WithEvents LabelDelay As Label
    Friend WithEvents TextBoxDelayBetweenActions As TextBox
    Friend WithEvents LabelSkillKey As Label
    Friend WithEvents TextBoxSkillKey As TextBox
    Friend WithEvents LabelSkillTimes As Label
    Friend WithEvents TextBoxSkillPressTimes As TextBox
    Friend WithEvents LabelSkillInterval As Label
    Friend WithEvents TextBoxSkillInterval As TextBox
    Friend WithEvents LabelCycleCount As Label
    Friend WithEvents TextBoxCycleCount As TextBox
End Class
