using CajeroATM;
using System.Data.SqlClient; 
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Cajero_archivos
{
    public partial class FormRegistroUsuario : Form
    {
        
       

        // Instancia de la clase de conexión a la BD
        private ConexionBD conexionBD = new ConexionBD();

        public FormRegistroUsuario()
        {
            EstilarFormulario();
            InicializarComponentes();
        }

        private void EstilarFormulario()
        {
            this.ClientSize = new Size(480, 580);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = ColorTranslator.FromHtml("#0F172A");
            this.Text = "Administración - Registro de Nuevo Usuario";
        }

        private void InicializarComponentes()
        {
            Label lblHeader = new Label
            {
                Text = "REGISTRAR NUEVO USUARIO",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#F8FAFC"),
                Location = new Point(30, 20),
                AutoSize = true
            };

            // Campo Nombre
            AgregarCampo("Nombre Completo:", 70, out txtNombre);

            // Campo Número de Tarjeta (16 dígitos)
            AgregarCampo("Número de Tarjeta (16 dígitos):", 140, out txtTarjeta);
            txtTarjeta.MaxLength = 16;

            // Campo PIN (4 dígitos)
            AgregarCampo("PIN Inicial (4 dígitos):", 210, out txtPin);
            txtPin.MaxLength = 4;
            txtPin.PasswordChar = '●';

            // Campo Límite Diario de Retiro
            AgregarCampo("Límite Máximo de Retiro Diario (Q.):", 280, out txtLimite);

            // Campo Token RFID (Capturado del Arduino)
            AgregarCampo("Token RFID (Asignar aproximando tarjeta):", 350, out txtTokenRFID);
            txtTokenRFID.ReadOnly = true;
            txtTokenRFID.ForeColor = ColorTranslator.FromHtml("#10B981");

            // Checkbox Administrador
            chkEsAdmin = new CheckBox
            {
                Text = "Es Usuario Administrador",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                Location = new Point(40, 425),
                AutoSize = true
            };

            // Botón Guardar
            btnGuardar = new Button
            {
                Text = "GUARDAR USUARIO",
                Location = new Point(40, 480),
                Size = new Size(400, 45),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#2563EB"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += btnGuardar_Click;

            this.Controls.Add(lblHeader);
            this.Controls.Add(chkEsAdmin);
            this.Controls.Add(btnGuardar);
        }

        private void AgregarCampo(string etiqueta, int posY, out TextBox campoTexto)
        {
            Label lbl = new Label
            {
                Text = etiqueta,
                Font = new Font("Segoe UI", 9),
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                Location = new Point(40, posY),
                AutoSize = true
            };

            campoTexto = new TextBox
            {
                Location = new Point(40, posY + 22),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = ColorTranslator.FromHtml("#1E293B"),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            this.Controls.Add(lbl);
            this.Controls.Add(campoTexto);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones básicas de entrada
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtTarjeta.Text) ||
                string.IsNullOrWhiteSpace(txtPin.Text) ||
                string.IsNullOrWhiteSpace(txtLimite.Text))
            {
                MessageBox.Show("Por favor complete todos los campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtTarjeta.Text.Trim().Length != 16)
            {
                MessageBox.Show("El número de tarjeta debe contener exactamente 16 dígitos.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPin.Text.Trim().Length != 4)
            {
                MessageBox.Show("El PIN debe contener exactamente 4 dígitos.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtLimite.Text.Trim(), out decimal limiteDiario) || limiteDiario <= 0)
            {
                MessageBox.Show("Ingrese un monto válido para el límite diario.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Inserción en la Base de Datos SQL Server
            string query = @"INSERT INTO Usuarios (Nombre, NumeroTarjeta, PIN, TokenRFID, SaldoActual, LimiteDiario, EsAdmin) 
                            VALUES (@Nombre, @Tarjeta, @PIN, @TokenRFID, 0.00, @Limite, @EsAdmin)";

            using (SqlConnection con = conexionBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Tarjeta", txtTarjeta.Text.Trim());
                    cmd.Parameters.AddWithValue("@PIN", txtPin.Text.Trim());

                    // Manejo del valor nulo si no se escaneó tarjeta RFID
                    string rfidVal = txtTokenRFID.Text.Trim();
                    cmd.Parameters.AddWithValue("@TokenRFID", string.IsNullOrEmpty(rfidVal) ? (object)DBNull.Value : rfidVal);

                    cmd.Parameters.AddWithValue("@Limite", limiteDiario);
                    cmd.Parameters.AddWithValue("@EsAdmin", chkEsAdmin.Checked ? 1 : 0);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                        // Guardar en el archivo de texto según las consideraciones generales
                        GuardarEnArchivoTexto(txtNombre.Text.Trim(), txtTarjeta.Text.Trim(), limiteDiario, chkEsAdmin.Checked);

                        MessageBox.Show("Usuario registrado correctamente en la base de datos y archivo.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar en la base de datos: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Método para guardar el log de la actividad en archivo de texto
        private void GuardarEnArchivoTexto(string nombre, string tarjeta, decimal limite, bool esAdmin)
        {
            try
            {
                string path = "registro_usuarios.txt";
                string linea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | Nombre: {nombre} | Tarjeta: {tarjeta} | Límite: Q.{limite:F2} | Admin: {esAdmin}";
                File.AppendAllText(path, linea + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo escribir en el archivo de texto: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Método para recibir el Token RFID desde el Login o evento del Arduino
        public void AsignarTokenRFID(string uidHex)
        {
            txtTokenRFID.Text = uidHex;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtTarjeta.Clear();
            txtPin.Clear();
            txtLimite.Clear();
            txtTokenRFID.Clear();
            chkEsAdmin.Checked = false;
        }

        private void FormRegistroUsuario_Load(object sender, EventArgs e)
        {

        }
    }
}