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
            Console.WriteLine("--- TABLES ---");
            using (SqlCommand cmd = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'", conn))
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    Console.WriteLine(rdr.GetString(0));
                }
            }

            Console.WriteLine("\n--- COLUMNS IN Empleados ---");
            PrintColumns(conn, "Empleados");

            Console.WriteLine("\n--- COLUMNS IN Usuario ---");
            PrintColumns(conn, "Usuario");

            Console.WriteLine("\n--- COLUMNS IN Materia ---");
            PrintColumns(conn, "Materia");
        }
    }

    static void PrintColumns(SqlConnection conn, string tableName)
    {
        string query = $"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
        using (SqlCommand cmd = new SqlCommand(query, conn))
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
            while (rdr.Read())
            {
                Console.WriteLine($"{rdr.GetString(0)} ({rdr.GetString(1)}, Nullable: {rdr.GetString(2)})");
            }
        }
    }
}
