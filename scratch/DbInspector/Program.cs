using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connString = "Server=localhost,1435;Database=CbtisKardex;User Id=sa;Password=P@ssw0rd2026!;TrustServerCertificate=True;";
        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();
            Console.WriteLine("\n--- SP_LOGINUSUARIO STORED PROCEDURE ---");
            PrintSp(conn, "sp_LoginUsuario");
        }
    }

    static void PrintSp(SqlConnection conn, string spName)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand($"EXEC sp_helptext '{spName}'", conn))
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    Console.Write(rdr.GetString(0));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error printing SP {spName}: {ex.Message}");
        }
    }
}
