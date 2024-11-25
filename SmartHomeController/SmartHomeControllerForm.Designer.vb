﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SmartHomeControllerForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Me.components = New System.ComponentModel.Container()
        Me.IncreaseMaxTempButton = New System.Windows.Forms.Button()
        Me.DecreaseMaxTempButton = New System.Windows.Forms.Button()
        Me.CurrentTempLabel = New System.Windows.Forms.Label()
        Me.MaxTempTextBox = New System.Windows.Forms.TextBox()
        Me.MinTempTextBox = New System.Windows.Forms.TextBox()
        Me.MaxTempLabel = New System.Windows.Forms.Label()
        Me.DecreaseMinTempButton = New System.Windows.Forms.Button()
        Me.IncreaseMinTempButton = New System.Windows.Forms.Button()
        Me.MinTempLabel = New System.Windows.Forms.Label()
        Me.CurrentTimeLabel = New System.Windows.Forms.Label()
        Me.SerialPort = New System.IO.Ports.SerialPort(Me.components)
        Me.TopMenuStrip = New System.Windows.Forms.MenuStrip()
        Me.SetupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConnectToQyBoardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UpdateCurrentTimeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.CurrentDateLabel = New System.Windows.Forms.Label()
        Me.TopMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'IncreaseMaxTempButton
        '
        Me.IncreaseMaxTempButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.IncreaseMaxTempButton.Location = New System.Drawing.Point(230, 167)
        Me.IncreaseMaxTempButton.Name = "IncreaseMaxTempButton"
        Me.IncreaseMaxTempButton.Size = New System.Drawing.Size(22, 22)
        Me.IncreaseMaxTempButton.TabIndex = 0
        Me.IncreaseMaxTempButton.Text = "+"
        Me.IncreaseMaxTempButton.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.IncreaseMaxTempButton.UseVisualStyleBackColor = True
        '
        'DecreaseMaxTempButton
        '
        Me.DecreaseMaxTempButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.DecreaseMaxTempButton.Location = New System.Drawing.Point(258, 167)
        Me.DecreaseMaxTempButton.Name = "DecreaseMaxTempButton"
        Me.DecreaseMaxTempButton.Size = New System.Drawing.Size(22, 22)
        Me.DecreaseMaxTempButton.TabIndex = 1
        Me.DecreaseMaxTempButton.Text = "-"
        Me.DecreaseMaxTempButton.UseVisualStyleBackColor = True
        '
        'CurrentTempLabel
        '
        Me.CurrentTempLabel.AutoSize = True
        Me.CurrentTempLabel.Font = New System.Drawing.Font("Roboto Slab", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentTempLabel.Location = New System.Drawing.Point(12, 118)
        Me.CurrentTempLabel.Name = "CurrentTempLabel"
        Me.CurrentTempLabel.Size = New System.Drawing.Size(78, 32)
        Me.CurrentTempLabel.TabIndex = 2
        Me.CurrentTempLabel.Text = "Temp"
        '
        'MaxTempTextBox
        '
        Me.MaxTempTextBox.Location = New System.Drawing.Point(124, 167)
        Me.MaxTempTextBox.Name = "MaxTempTextBox"
        Me.MaxTempTextBox.Size = New System.Drawing.Size(100, 22)
        Me.MaxTempTextBox.TabIndex = 3
        '
        'MinTempTextBox
        '
        Me.MinTempTextBox.Location = New System.Drawing.Point(124, 200)
        Me.MinTempTextBox.Name = "MinTempTextBox"
        Me.MinTempTextBox.Size = New System.Drawing.Size(100, 22)
        Me.MinTempTextBox.TabIndex = 4
        '
        'MaxTempLabel
        '
        Me.MaxTempLabel.AutoSize = True
        Me.MaxTempLabel.Location = New System.Drawing.Point(15, 170)
        Me.MaxTempLabel.Name = "MaxTempLabel"
        Me.MaxTempLabel.Size = New System.Drawing.Size(104, 15)
        Me.MaxTempLabel.TabIndex = 5
        Me.MaxTempLabel.Text = "Max Temperature:"
        '
        'DecreaseMinTempButton
        '
        Me.DecreaseMinTempButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.DecreaseMinTempButton.Location = New System.Drawing.Point(258, 200)
        Me.DecreaseMinTempButton.Name = "DecreaseMinTempButton"
        Me.DecreaseMinTempButton.Size = New System.Drawing.Size(22, 22)
        Me.DecreaseMinTempButton.TabIndex = 7
        Me.DecreaseMinTempButton.Text = "-"
        Me.DecreaseMinTempButton.UseVisualStyleBackColor = True
        '
        'IncreaseMinTempButton
        '
        Me.IncreaseMinTempButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.IncreaseMinTempButton.Location = New System.Drawing.Point(230, 200)
        Me.IncreaseMinTempButton.Name = "IncreaseMinTempButton"
        Me.IncreaseMinTempButton.Size = New System.Drawing.Size(22, 22)
        Me.IncreaseMinTempButton.TabIndex = 6
        Me.IncreaseMinTempButton.Text = "+"
        Me.IncreaseMinTempButton.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.IncreaseMinTempButton.UseVisualStyleBackColor = True
        '
        'MinTempLabel
        '
        Me.MinTempLabel.AutoSize = True
        Me.MinTempLabel.Location = New System.Drawing.Point(15, 204)
        Me.MinTempLabel.Name = "MinTempLabel"
        Me.MinTempLabel.Size = New System.Drawing.Size(103, 15)
        Me.MinTempLabel.TabIndex = 8
        Me.MinTempLabel.Text = "Min Temperature:"
        '
        'CurrentTimeLabel
        '
        Me.CurrentTimeLabel.AutoSize = True
        Me.CurrentTimeLabel.Font = New System.Drawing.Font("Roboto Slab", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentTimeLabel.Location = New System.Drawing.Point(12, 64)
        Me.CurrentTimeLabel.Name = "CurrentTimeLabel"
        Me.CurrentTimeLabel.Size = New System.Drawing.Size(72, 32)
        Me.CurrentTimeLabel.TabIndex = 9
        Me.CurrentTimeLabel.Text = "Time"
        '
        'TopMenuStrip
        '
        Me.TopMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetupToolStripMenuItem})
        Me.TopMenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.TopMenuStrip.Name = "TopMenuStrip"
        Me.TopMenuStrip.Size = New System.Drawing.Size(339, 24)
        Me.TopMenuStrip.TabIndex = 10
        Me.TopMenuStrip.Text = "MenuStrip1"
        '
        'SetupToolStripMenuItem
        '
        Me.SetupToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConnectToQyBoardToolStripMenuItem})
        Me.SetupToolStripMenuItem.Name = "SetupToolStripMenuItem"
        Me.SetupToolStripMenuItem.Size = New System.Drawing.Size(49, 20)
        Me.SetupToolStripMenuItem.Text = "&Setup"
        '
        'ConnectToQyBoardToolStripMenuItem
        '
        Me.ConnectToQyBoardToolStripMenuItem.Name = "ConnectToQyBoardToolStripMenuItem"
        Me.ConnectToQyBoardToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.ConnectToQyBoardToolStripMenuItem.Text = "&Connect to Qy@ board"
        '
        'UpdateCurrentTimeTimer
        '
        Me.UpdateCurrentTimeTimer.Enabled = True
        Me.UpdateCurrentTimeTimer.Interval = 1000
        '
        'CurrentDateLabel
        '
        Me.CurrentDateLabel.AutoSize = True
        Me.CurrentDateLabel.Font = New System.Drawing.Font("Roboto Slab", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentDateLabel.Location = New System.Drawing.Point(14, 40)
        Me.CurrentDateLabel.Name = "CurrentDateLabel"
        Me.CurrentDateLabel.Size = New System.Drawing.Size(44, 22)
        Me.CurrentDateLabel.TabIndex = 11
        Me.CurrentDateLabel.Text = "Date"
        '
        'SmartHomeControllerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(339, 263)
        Me.Controls.Add(Me.CurrentDateLabel)
        Me.Controls.Add(Me.CurrentTimeLabel)
        Me.Controls.Add(Me.MinTempLabel)
        Me.Controls.Add(Me.DecreaseMinTempButton)
        Me.Controls.Add(Me.IncreaseMinTempButton)
        Me.Controls.Add(Me.MaxTempLabel)
        Me.Controls.Add(Me.MinTempTextBox)
        Me.Controls.Add(Me.MaxTempTextBox)
        Me.Controls.Add(Me.CurrentTempLabel)
        Me.Controls.Add(Me.DecreaseMaxTempButton)
        Me.Controls.Add(Me.IncreaseMaxTempButton)
        Me.Controls.Add(Me.TopMenuStrip)
        Me.Font = New System.Drawing.Font("Roboto Slab", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainMenuStrip = Me.TopMenuStrip
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "SmartHomeControllerForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.TopMenuStrip.ResumeLayout(False)
        Me.TopMenuStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents IncreaseMaxTempButton As Button
    Friend WithEvents DecreaseMaxTempButton As Button
    Friend WithEvents CurrentTempLabel As Label
    Friend WithEvents MaxTempTextBox As TextBox
    Friend WithEvents MinTempTextBox As TextBox
    Friend WithEvents MaxTempLabel As Label
    Friend WithEvents DecreaseMinTempButton As Button
    Friend WithEvents IncreaseMinTempButton As Button
    Friend WithEvents MinTempLabel As Label
    Friend WithEvents CurrentTimeLabel As Label
    Friend WithEvents SerialPort As IO.Ports.SerialPort
    Friend WithEvents TopMenuStrip As MenuStrip
    Friend WithEvents SetupToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConnectToQyBoardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UpdateCurrentTimeTimer As Timer
    Friend WithEvents CurrentDateLabel As Label
End Class
