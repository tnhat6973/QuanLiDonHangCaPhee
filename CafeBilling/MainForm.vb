Imports System.Data


Public Class MainForm
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOrders.CellContentClick

    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadOrders()
    End Sub

    ' Hàm LoadOrders: lấy dữ liệu từ DB và hiển thị lên DataGridView
    Private Sub LoadOrders()
        ' 1. Viết câu SELECT: join các bảng để tính tổng tiền
        Dim sql As String = "
            SELECT h.MaHD, h.NgayLap, k.TenKH, n.TenNV,
                   ISNULL(SUM(ct.SoLuong * m.Gia), 0) AS Total
            FROM HOADON h
            LEFT JOIN KHACHHANG k ON h.MaKH = k.MaKH
            LEFT JOIN NHANVIEN n ON h.MaNV = n.MaNV
            LEFT JOIN CHITIETHOADON ct ON h.MaHD = ct.MaHD
            LEFT JOIN MON m ON ct.MaMon = m.MaMon
            GROUP BY h.MaHD, h.NgayLap, k.TenKH, n.TenNV
            ORDER BY h.NgayLap DESC
        "

        ' 2. Gọi DBHelper để thực hiện SELECT
        Dim dt As DataTable = DBHelper.ExecuteQuery(sql, Nothing)

        ' 3. Gán DataTable làm nguồn dữ liệu cho DataGridView
        dgvOrders.DataSource = dt

        ' 4. Format cột Total thành dạng tiền (vd: 25,000)
        If dgvOrders.Columns.Contains("Total") Then
            dgvOrders.Columns("Total").DefaultCellStyle.Format = "N0"
        End If

        ' 5. Ẩn cột MaHD nếu bạn muốn (nhưng cần nó để Edit/Delete)
        If dgvOrders.Columns.Contains("MaHD") Then
            dgvOrders.Columns("MaHD").Visible = True  ' nếu muốn hiển thị ID thì True
        End If
    End Sub







    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Using f As New OrderForm()
            If f.ShowDialog() = DialogResult.OK Then
                LoadOrders()
            End If
        End Using
    End Sub












    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If dgvOrders.CurrentRow Is Nothing Then Return
        Dim maHD As Integer = CInt(dgvOrders.CurrentRow.Cells("MaHD").Value)
        Using f As New OrderForm()
            f.EditingMaHD = maHD
            If f.ShowDialog() = DialogResult.OK Then
                LoadOrders()
            End If
        End Using
    End Sub









    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvOrders.CurrentRow Is Nothing Then Return
        Dim maHD As Integer = CInt(dgvOrders.CurrentRow.Cells("MaHD").Value)
        If MessageBox.Show("Bạn có chắc muốn xóa hóa đơn " & maHD & "?", "Xác nhận", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
            Return
        End If

        Dim connStr As String = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CafeBilling;Integrated Security=True"
        Using conn As New SqlClient.SqlConnection(connStr)
            conn.Open()
            Dim tran = conn.BeginTransaction()
            Try
                Using cmd1 As New SqlClient.SqlCommand("DELETE FROM CHITIETHOADON WHERE MaHD=@MaHD", conn, tran)
                    cmd1.Parameters.AddWithValue("@MaHD", maHD)
                    cmd1.ExecuteNonQuery()
                End Using
                Using cmd2 As New SqlClient.SqlCommand("DELETE FROM HOADON WHERE MaHD=@MaHD", conn, tran)
                    cmd2.Parameters.AddWithValue("@MaHD", maHD)
                    cmd2.ExecuteNonQuery()
                End Using
                tran.Commit()
                MessageBox.Show("Xóa thành công.")
                LoadOrders()
            Catch ex As Exception
                tran.Rollback()
                MessageBox.Show("Lỗi xóa: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub btnManageCustomers_Click(sender As Object, e As EventArgs) Handles btnManageCustomers.Click
        Using f As New CustomerForm()
            f.ShowDialog()
        End Using
    End Sub
End Class
