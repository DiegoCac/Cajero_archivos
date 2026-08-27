using System.Data.SqlClient; // O System.Data.SqlClient
using System;
using System.Data;
using System.Data.SqlClient;

namespace CajeroATM
{
    public class ConexionBD
    {
        // Ajusta la cadena con el nombre de tu servidor
        private readonly string cadenaConexion = "Server=localhost;Database=CajeroATM;Integrated Security=True;TrustServerCertificate=True;";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        // Método de prueba para verificar que SSMS responde correctamente
        public bool ProbarConexion()
        {
            using (SqlConnection conexion = ObtenerConexion())
            {
                try
                {
                    conexion.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error al conectar con SQL Server: " + ex.Message,
                        "Error de Conexión", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
        }
    }
}