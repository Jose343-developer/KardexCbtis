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
            Console.WriteLine("--- ROLES ---");
            using (SqlCommand cmd = new SqlCommand("SELECT IdRol, Nombre FROM Rol", conn))
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    Console.WriteLine($"IdRol: {rdr.GetInt32(0)}, Nombre: {rdr.GetString(1)}");
                }
            }

            Console.WriteLine("\n--- USUARIOS ---");
            using (SqlCommand cmd = new SqlCommand("SELECT IdUsuario, IdRol, NombreUser, Password, Estatus FROM Usuario", conn))
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    Console.WriteLine($"IdUsuario: {rdr.GetInt32(0)}, IdRol: {rdr.GetInt32(1)}, NombreUser: {rdr.GetString(2)}, Password: {rdr.GetString(3)}, Estatus: {(rdr.IsDBNull(4) ? "NULL" : rdr.GetBoolean(4))}");
                }
            }
        }
    }
}
