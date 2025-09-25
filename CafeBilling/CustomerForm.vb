Imports System.Data
Imports System.Data.SqlClient

Public Class CustomerForm

    ' Khi form load gọi LoadCustomers để nạp dữ liệu lên DataGridView
    Private Sub CustomerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupGrid()
        LoadCustomers()
    End Sub

    ' Thiết lập cột cho DataGridView
    Private Sub SetupGrid()
        dgvCustomers.Columns.Clear()
        dgvCustomers.Columns.Add("MaKH", "Mã KH")
        dgvCustomers.Columns.Add("TenKH", "Tên KH")
        dgvCustomers.Columns.Add("SDT", "SĐT")
        dgvCustomers.Columns("MaKH").Visible = False ' ẩn ID nếu muốn
        dgvCustomers.ReadOnly = True
        dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    ' Load toàn bộ khách hàng từ DB vào dgv
    Private Sub LoadCustomers()
        Dim sql As String = "SELECT MaKH, TenKH, SDT FROM KHACHHANG ORDER BY TenKH"
        Dim dt As DataTable = DBHelper.ExecuteQuery(sql, Nothing)
        dgvCustomers.Rows.Clear()
        For Each r As DataRow In dt.Rows
            dgvCustomers.Rows.Add(r("MaKH"), r("TenKH"), If(IsDBNull(r("SDT")), "", r("SDT")))
        Next
        ClearInputs()
    End Sub

    ' Xóa rỗng input
    Private Sub ClearInputs()
        txtMaKH.Text = ""
        txtTenKH.Text = ""
        txtSDT.Text = ""
    End Sub

    ' Khi user chọn 1 hàng trong dgv, show dữ liệu lên input để sửa/xóa
    Private Sub dgvCustomers_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCustomers.SelectionChanged
        If dgvCustomers.CurrentRow Is Nothing Then
            ClearInputs()
            Return
        End If
        Dim row = dgvCustomers.CurrentRow
        txtMaKH.Text = row.Cells("MaKH").Value.ToString()
        txtTenKH.Text = row.Cells("TenKH").Value.ToString()
        txtSDT.Text = row.Cells("SDT").Value?.ToString()
    End Sub

    ' Thêm khách hàng
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim ten As String = txtTenKH.Text.Trim()
        Dim sdt As String = txtSDT.Text.Trim()
        If ten = "" Then
            MessageBox.Show("Nhập tên khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim sql As String = "INSERT INTO KHACHHANG (TenKH, SDT) VALUES (@TenKH, @SDT)"
        Dim p() As SqlParameter = {
            New SqlParameter("@TenKH", ten),
            New SqlParameter("@SDT", If(sdt = "", DBNull.Value, CType(sdt, Object)))
        }
        Try
            DBHelper.ExecuteNonQuery(sql, p)
            MessageBox.Show("Thêm khách hàng thành công.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadCustomers()
            ' thông báo để form cha (OrderForm) có thể refresh nếu cần
        Catch ex As Exception
            MessageBox.Show("Lỗi thêm khách: " & ex.Message)
        End Try
    End Sub

    ' Sửa khách hàng
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If txtMaKH.Text = "" Then
            MessageBox.Show("Chọn khách hàng để sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim ma As Integer = CInt(txtMaKH.Text)
        Dim ten As String = txtTenKH.Text.Trim()
        Dim sdt As String = txtSDT.Text.Trim()
        If ten = "" Then
            MessageBox.Show("Tên không được rỗng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim sql As String = "UPDATE KHACHHANG SET TenKH=@TenKH, SDT=@SDT WHERE MaKH=@MaKH"
        Dim p() As SqlParameter = {
            New SqlParameter("@TenKH", ten),
            New SqlParameter("@SDT", If(sdt = "", DBNull.Value, CType(sdt, Object))),
            New SqlParameter("@MaKH", ma)
        }
        Try
            DBHelper.ExecuteNonQuery(sql, p)
            MessageBox.Show("Cập nhật thành công.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadCustomers()
        Catch ex As Exception
            MessageBox.Show("Lỗi cập nhật: " & ex.Message)
        End Try
    End Sub

    ' Xóa khách hàng
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If txtMaKH.Text = "" Then
            MessageBox.Show("Chọn khách hàng để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim ma As Integer = CInt(txtMaKH.Text)
        If MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Dim sql As String = "DELETE FROM KHACHHANG WHERE MaKH=@MaKH"
        Dim p() As SqlParameter = {New SqlParameter("@MaKH", ma)}
        Try
            DBHelper.ExecuteNonQuery(sql, p)
            MessageBox.Show("Xóa thành công.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadCustomers()
        Catch ex As Exception
            MessageBox.Show("Lỗi xóa: " & ex.Message)
        End Try
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearInputs()
    End Sub

End Class
