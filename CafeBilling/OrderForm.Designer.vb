<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OrderForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cboCustomer = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboEmployee = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboMenu = New System.Windows.Forms.ComboBox()
        Me.nudQty = New System.Windows.Forms.NumericUpDown()
        Me.btnAddItem = New System.Windows.Forms.Button()
        Me.dgvItems = New System.Windows.Forms.DataGridView()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        CType(Me.nudQty, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboCustomer
        '
        Me.cboCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCustomer.FormattingEnabled = True
        Me.cboCustomer.Location = New System.Drawing.Point(164, 12)
        Me.cboCustomer.Name = "cboCustomer"
        Me.cboCustomer.Size = New System.Drawing.Size(368, 24)
        Me.cboCustomer.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label1.Location = New System.Drawing.Point(46, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 18)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Khách hàng:"
        '
        'cboEmployee
        '
        Me.cboEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEmployee.FormattingEnabled = True
        Me.cboEmployee.Location = New System.Drawing.Point(164, 53)
        Me.cboEmployee.Name = "cboEmployee"
        Me.cboEmployee.Size = New System.Drawing.Size(368, 24)
        Me.cboEmployee.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Location = New System.Drawing.Point(46, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 18)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Nhân Viên:"
        '
        'cboMenu
        '
        Me.cboMenu.FormattingEnabled = True
        Me.cboMenu.Location = New System.Drawing.Point(65, 100)
        Me.cboMenu.Name = "cboMenu"
        Me.cboMenu.Size = New System.Drawing.Size(368, 24)
        Me.cboMenu.TabIndex = 4
        '
        'nudQty
        '
        Me.nudQty.Location = New System.Drawing.Point(486, 101)
        Me.nudQty.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudQty.Name = "nudQty"
        Me.nudQty.Size = New System.Drawing.Size(101, 22)
        Me.nudQty.TabIndex = 5
        Me.nudQty.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'btnAddItem
        '
        Me.btnAddItem.Location = New System.Drawing.Point(646, 100)
        Me.btnAddItem.Name = "btnAddItem"
        Me.btnAddItem.Size = New System.Drawing.Size(109, 23)
        Me.btnAddItem.TabIndex = 6
        Me.btnAddItem.Text = "Thêm món"
        Me.btnAddItem.UseVisualStyleBackColor = True
        '
        'dgvItems
        '
        Me.dgvItems.AllowUserToAddRows = False
        Me.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvItems.Location = New System.Drawing.Point(56, 164)
        Me.dgvItems.Name = "dgvItems"
        Me.dgvItems.RowHeadersWidth = 51
        Me.dgvItems.RowTemplate.Height = 24
        Me.dgvItems.Size = New System.Drawing.Size(690, 226)
        Me.dgvItems.TabIndex = 7
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(110, 411)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(0, 16)
        Me.lblTotal.TabIndex = 8
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.btnSave.Location = New System.Drawing.Point(551, 408)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "LƯU"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(680, 408)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "HỦY"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'OrderForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.CafeBilling.My.Resources.Resources.THUMB1
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.dgvItems)
        Me.Controls.Add(Me.btnAddItem)
        Me.Controls.Add(Me.nudQty)
        Me.Controls.Add(Me.cboMenu)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboEmployee)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboCustomer)
        Me.Name = "OrderForm"
        Me.Text = "OrderForm"
        CType(Me.nudQty, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cboCustomer As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cboEmployee As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cboMenu As ComboBox
    Friend WithEvents nudQty As NumericUpDown
    Friend WithEvents btnAddItem As Button
    Friend WithEvents dgvItems As DataGridView
    Friend WithEvents lblTotal As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
End Class
