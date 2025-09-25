Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class DBHelper

    Private Shared ReadOnly connectionString As String = GetConnectionString()

    Private Shared Function GetConnectionString() As String
        Try
            Dim csSetting = ConfigurationManager.ConnectionStrings("CafeBillingConnection")
            If csSetting IsNot Nothing AndAlso Not String.IsNullOrEmpty(csSetting.ConnectionString) Then
                Return csSetting.ConnectionString
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Lỗi đọc App.config: " & ex.Message)
        End Try
        Return "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CafeBilling;Integrated Security=True"
    End Function

    ' --- Helper: tạo bản sao (clone) của các SqlParameter để tránh reuse ---
    Private Shared Function CloneParameters(paramsIn As SqlParameter()) As SqlParameter()
        If paramsIn Is Nothing Then Return Nothing
        Dim out(paramsIn.Length - 1) As SqlParameter
        For i As Integer = 0 To paramsIn.Length - 1
            Dim p = paramsIn(i)
            ' Tạo parameter mới từ tên và giá trị
            Dim np As New SqlParameter(p.ParameterName, If(p.Value Is Nothing, DBNull.Value, p.Value))
            ' copy một số thuộc tính quan trọng (nếu bạn dùng)
            np.SqlDbType = p.SqlDbType
            np.Size = p.Size
            np.Direction = p.Direction
            np.IsNullable = p.IsNullable
            np.SourceColumn = p.SourceColumn
            np.Precision = p.Precision
            np.Scale = p.Scale
            out(i) = np
        Next
        Return out
    End Function

    ' --- SELECT : trả về DataTable ---
    Public Shared Function ExecuteQuery(sql As String, params As SqlParameter()) As DataTable
        Dim dt As New DataTable()
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, conn)
                If params IsNot Nothing Then
                    ' Dùng bản sao của parameters (không thêm trực tiếp các parameter do caller cung cấp)
                    Dim cloned = CloneParameters(params)
                    cmd.Parameters.AddRange(cloned)
                End If
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function

    ' --- INSERT/UPDATE/DELETE : trả về số dòng bị ảnh hưởng ---
    Public Shared Function ExecuteNonQuery(sql As String, params As SqlParameter()) As Integer
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                If params IsNot Nothing Then
                    Dim cloned = CloneParameters(params)
                    cmd.Parameters.AddRange(cloned)
                End If
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    ' --- ExecuteScalar ---
    Public Shared Function ExecuteScalar(sql As String, params As SqlParameter()) As Object
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using cmd As New SqlCommand(sql, conn)
                If params IsNot Nothing Then
                    Dim cloned = CloneParameters(params)
                    cmd.Parameters.AddRange(cloned)
                End If
                Return cmd.ExecuteScalar()
            End Using
        End Using
    End Function

End Class
