Imports System.Data

Public Class OrderForm

    ' Nếu form dùng để Edit thì ta truyền MaHD vào
    Public Property EditingMaHD As Integer? = Nothing

    Private Sub OrderForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCustomers()
        LoadEmployees()
        LoadMenuItems()
        SetupItemsGrid()

        If EditingMaHD.HasValue Then
            LoadOrderForEdit(EditingMaHD.Value)
        End If

        UpdateTotal()
    End Sub

    Private Sub LoadCustomers()
        Dim dt As DataTable = DBHelper.ExecuteQuery("SELECT MaKH, TenKH FROM KHACHHANG", Nothing)
        cboCustomer.DataSource = dt
        cboCustomer.DisplayMember = "TenKH"
        cboCustomer.ValueMember = "MaKH"
    End Sub

    Private Sub LoadEmployees()
        Dim dt As DataTable = DBHelper.ExecuteQuery("SELECT MaNV, TenNV FROM NHANVIEN", Nothing)
        cboEmployee.DataSource = dt
        cboEmployee.DisplayMember = "TenNV"
        cboEmployee.ValueMember = "MaNV"
    End Sub

    Private Sub LoadMenuItems()
        Dim dt As DataTable = DBHelper.ExecuteQuery("SELECT MaMon, TenMon, Gia FROM MON", Nothing)
        cboMenu.DataSource = dt
        cboMenu.DisplayMember = "TenMon"
        cboMenu.ValueMember = "MaMon"
    End Sub

    Private Sub SetupItemsGrid()
        dgvItems.Columns.Clear()
        dgvItems.Columns.Add("MaMon", "MaMon")
        dgvItems.Columns("MaMon").Visible = False
        dgvItems.Columns.Add("TenMon", "Tên món")
        dgvItems.Columns.Add("Gia", "Giá")
        dgvItems.Columns.Add("SoLuong", "Số lượng")
        dgvItems.Columns.Add("SubTotal", "Thành tiền")

        dgvItems.Columns("Gia").DefaultCellStyle.Format = "N0"
        dgvItems.Columns("SubTotal").DefaultCellStyle.Format = "N0"
    End Sub

    Private Sub UpdateTotal()
        Dim total As Decimal = 0
        For Each row As DataGridViewRow In dgvItems.Rows
            total += Convert.ToDecimal(row.Cells("SubTotal").Value)
        Next
        lblTotal.Text = total.ToString("N0")
    End Sub



    '








    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        If cboMenu.SelectedValue Is Nothing Then Return

        ' Lấy dữ liệu từ combobox và numeric
        Dim maMon As Integer = CInt(cboMenu.SelectedValue)
        Dim tenMon As String = cboMenu.Text
        Dim gia As Decimal = CDec(CType(cboMenu.SelectedItem, DataRowView)("Gia"))
        Dim qty As Integer = CInt(nudQty.Value)
        Dim subtotal As Decimal = gia * qty

        ' Kiểm tra nếu món đã tồn tại trong dgvItems thì cộng số lượng
        Dim found As Boolean = False
        For Each row As DataGridViewRow In dgvItems.Rows
            If CInt(row.Cells("MaMon").Value) = maMon Then
                row.Cells("SoLuong").Value = CInt(row.Cells("SoLuong").Value) + qty
                row.Cells("SubTotal").Value = CDec(row.Cells("SoLuong").Value) * CDec(row.Cells("Gia").Value)
                found = True
                Exit For
            End If
        Next

        If Not found Then
            dgvItems.Rows.Add(maMon, tenMon, gia, qty, subtotal)
        End If

        UpdateTotal()
    End Sub

















    '
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Validate: phải có ít nhất 1 món
        If dgvItems.Rows.Count = 0 Then
            MessageBox.Show("Chưa có món trong order.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim maKH As Object = If(cboCustomer.SelectedValue, DBNull.Value)
        Dim maNV As Object = If(cboEmployee.SelectedValue, DBNull.Value)

        ' Khởi tạo kết nối và transaction
        Dim connStr As String = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CafeBilling;Integrated Security=True"
        Using conn As New SqlClient.SqlConnection(connStr)
            conn.Open()
            Dim tran As SqlClient.SqlTransaction = conn.BeginTransaction()
            Try
                ' 1) Nếu là thêm mới (EditingMaHD = Nothing)
                Dim newMaHD As Integer
                If Not EditingMaHD.HasValue Then
                    Dim sqlInsert As String = "INSERT INTO HOADON (NgayLap, MaKH, MaNV) VALUES (GETDATE(), @MaKH, @MaNV); SELECT SCOPE_IDENTITY();"
                    Using cmd As New SqlClient.SqlCommand(sqlInsert, conn, tran)
                        cmd.Parameters.AddWithValue("@MaKH", If(maKH, DBNull.Value))
                        cmd.Parameters.AddWithValue("@MaNV", If(maNV, DBNull.Value))
                        newMaHD = Convert.ToInt32(cmd.ExecuteScalar())
                    End Using
                Else
                    ' 2) Nếu là sửa: cập nhật HOADON
                    newMaHD = EditingMaHD.Value
                    Dim sqlUpdate As String = "UPDATE HOADON SET MaKH=@MaKH, MaNV=@MaNV WHERE MaHD=@MaHD"
                    Using cmd As New SqlClient.SqlCommand(sqlUpdate, conn, tran)
                        cmd.Parameters.AddWithValue("@MaKH", If(maKH, DBNull.Value))
                        cmd.Parameters.AddWithValue("@MaNV", If(maNV, DBNull.Value))
                        cmd.Parameters.AddWithValue("@MaHD", newMaHD)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Xóa chi tiết cũ, sẽ insert lại bên dưới
                    Dim sqlDeleteCT As String = "DELETE FROM CHITIETHOADON WHERE MaHD=@MaHD"
                    Using cmdDel As New SqlClient.SqlCommand(sqlDeleteCT, conn, tran)
                        cmdDel.Parameters.AddWithValue("@MaHD", newMaHD)
                        cmdDel.ExecuteNonQuery()
                    End Using
                End If

                ' 3) Insert từng dòng chi tiết
                For Each row As DataGridViewRow In dgvItems.Rows
                    Dim maMon As Integer = CInt(row.Cells("MaMon").Value)
                    Dim soLuong As Integer = CInt(row.Cells("SoLuong").Value)
                    Dim sqlInsertCT As String = "INSERT INTO CHITIETHOADON (MaHD, MaMon, SoLuong) VALUES (@MaHD, @MaMon, @SoLuong)"
                    Using cmd2 As New SqlClient.SqlCommand(sqlInsertCT, conn, tran)
                        cmd2.Parameters.AddWithValue("@MaHD", newMaHD)
                        cmd2.Parameters.AddWithValue("@MaMon", maMon)
                        cmd2.Parameters.AddWithValue("@SoLuong", soLuong)
                        cmd2.ExecuteNonQuery()
                    End Using
                Next

                ' Nếu ko lỗi -> commit
                tran.Commit()
                MessageBox.Show("Lưu thành công.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As Exception
                tran.Rollback()
                MessageBox.Show("Lỗi khi lưu: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub
















    Private Sub LoadOrderForEdit(maHD As Integer)
        ' Load HOADON
        Dim sql As String = "SELECT MaHD, MaKH, MaNV FROM HOADON WHERE MaHD=@MaHD"
        Dim p As SqlClient.SqlParameter() = {New SqlClient.SqlParameter("@MaHD", maHD)}
        Dim dt As DataTable = DBHelper.ExecuteQuery(sql, p)
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            If Not IsDBNull(r("MaKH")) Then cboCustomer.SelectedValue = CInt(r("MaKH"))
            If Not IsDBNull(r("MaNV")) Then cboEmployee.SelectedValue = CInt(r("MaNV"))
        End If

        ' Load CHITIETHOADON
        Dim sqlCT As String = "SELECT ct.MaMon, m.TenMon, m.Gia, ct.SoLuong, (ct.SoLuong * m.Gia) AS SubTotal FROM CHITIETHOADON ct JOIN MON m ON ct.MaMon = m.MaMon WHERE ct.MaHD=@MaHD"
        Dim dtct As DataTable = DBHelper.ExecuteQuery(sqlCT, p)
        For Each r As DataRow In dtct.Rows
            dgvItems.Rows.Add(CInt(r("MaMon")), r("TenMon").ToString(), CDec(r("Gia")), CInt(r("SoLuong")), CDec(r("SubTotal")))
        Next
    End Sub


















End Class
