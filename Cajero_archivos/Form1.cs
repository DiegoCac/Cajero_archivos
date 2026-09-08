using CajeroATM;
using System.Data.SqlClient; // Asegúrate de tener instalado el paquete NuGet Microsoft.Data.SqlClient
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace Cajero_archivos
{
    public partial class Frloggin : Form
    {
        

        // Conexión a la Base de Datos y Puerto Serie del Arduino
        private ConexionBD conexionBD = new ConexionBD();
        private SerialPort arduinoPort;

        public Frloggin()
        {
            EstilarFormulario();
            InicializarComponentesUI();
            ConfigurarPuertoSerial();
        }

        private void EstilarFormulario()
        {
            this.ClientSize = new Size(400, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ColorTranslator.FromHtml("#BBC8E6"); // Tu color asignado
            this.Text = "Cajero Automático - Iniciar Sesión";
        }

        private void InicializarComponentesUI()
        {
            // Título
            Label lblTitulo = new Label
            {
                Text = "BIENVENIDO",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#1E293B"),
                Size = new Size(360, 40),
                Location = new Point(20, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Campo: Número de Tarjeta
            Label lblTarjeta = new Label
            {
                Text = "Número de Tarjeta (16 dígitos):",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#1E293B"),
                Location = new Point(40, 90),
                AutoSize = true
            };

            txtTarjeta = new TextBox
            {
                Location = new Point(40, 115),
                Size = new Size(320, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = ColorTranslator.FromHtml("#1E293B"),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                MaxLength = 16
            };

            // Campo: PIN
            Label lblPin = new Label
            {
                Text = "PIN (4 dígitos):",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#1E293B"),
                Location = new Point(40, 160),
                AutoSize = true
            };

            txtPin = new TextBox
            {
                Location = new Point(40, 185),
                Size = new Size(320, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = ColorTranslator.FromHtml("#1E293B"),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '●',
                MaxLength = 4
            };

            // Campo Oculto/Lectura: Token RFID
            Label lblRFID = new Label
            {
                Text = "Tarjeta / Token RFID:",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#1E293B"),
                Location = new Point(40, 230),
                AutoSize = true
            };

            txtRFID = new TextBox
            {
                Location = new Point(40, 255),
                Size = new Size(320, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = ColorTranslator.FromHtml("#1E293B"),
                ForeColor = ColorTranslator.FromHtml("#10B981"), // Texto verde
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true
            };

            lblEstadoRFID = new Label
            {
                Text = "📲 Acerque su tarjeta al lector...",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = ColorTranslator.FromHtml("#065F46"),
                Location = new Point(40, 290),
                AutoSize = true
            };

            // Botón de Ingreso
            btnIngresar = new Button
            {
                Text = "INICIAR SESIÓN",
                Location = new Point(40, 340),
                Size = new Size(320, 45),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = ColorTranslator.FromHtml("#2563EB"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.Click += btnIngresar_Click;

            // Agregar controles a la pantalla
            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblTarjeta);
            this.Controls.Add(txtTarjeta);
            this.Controls.Add(lblPin);
            this.Controls.Add(txtPin);
            this.Controls.Add(lblRFID);
            this.Controls.Add(txtRFID);
            this.Controls.Add(lblEstadoRFID);
            this.Controls.Add(btnIngresar);
        }

        private void Frloggin_Load(object sender, EventArgs e)
        {
            // Código al cargar el formulario si es necesario
        }

        private void ConfigurarPuertoSerial()
        {
            try
            {
                // Reemplazar "COM3" con el puerto COM al que esté conectado tu Arduino
                arduinoPort = new SerialPort("COM4", 9600);
                arduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                arduinoPort.Open();
            }
            catch (Exception ex)
            {
                lblEstadoRFID.Text = "⚠️ Lector RFID desconectado";
                lblEstadoRFID.ForeColor = Color.Red;
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string uidLeido = arduinoPort.ReadLine().Trim();
                RecibirDatosRFID(uidLeido);
            }
            catch { }
        }

        public void RecibirDatosRFID(string uidHex)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(() => {
                    txtRFID.Text = uidHex;
                    lblEstadoRFID.Text = "✓ Tarjeta detectada con éxito";
                    lblEstadoRFID.ForeColor = ColorTranslator.FromHtml("#065F46");

                    // Buscar automáticamente el número de tarjeta asociado a este Token RFID en la BD
                    AutenticarPorRFID(uidHex);
                }));
            }
        }

        private void AutenticarPorRFID(string uidHex)
        {
            string query = "SELECT NumeroTarjeta FROM Usuarios WHERE TokenRFID = @Token";

            using (SqlConnection con = conexionBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Token", uidHex);
                    try
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            txtTarjeta.Text = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al consultar token RFID: " + ex.Message);
                    }
                }
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string tarjeta = txtTarjeta.Text.Trim();
            string pin = txtPin.Text.Trim();

            if (string.IsNullOrEmpty(tarjeta) || string.IsNullOrEmpty(pin))
            {
                MessageBox.Show("Por favor complete los campos de tarjeta y PIN.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidarCredenciales(tarjeta, pin))
            {
                MessageBox.Show("¡Acceso concedido!", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abre el formulario de Registro como tenías configurado
                FormRegistroUsuario fr2 = new FormRegistroUsuario();
                this.Hide();
                fr2.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Número de tarjeta o PIN incorrectos.", "Error de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCredenciales(string numeroTarjeta, string pin)
        {
            string query = "SELECT COUNT(1) FROM Usuarios WHERE NumeroTarjeta = @Tarjeta AND PIN = @PIN";

            using (SqlConnection con = conexionBD.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Tarjeta", numeroTarjeta);
                    cmd.Parameters.AddWithValue("@PIN", pin);

                    try
                    {
                        con.Open();
                        int matchCount = Convert.ToInt32(cmd.ExecuteScalar());
                        return matchCount > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al conectar con la Base de Datos: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }

        private void Frloggin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cerrar el puerto serie al salir para liberar el puerto COM
            if (arduinoPort != null && arduinoPort.IsOpen)
            {
                arduinoPort.Close();
            }
        }
    }
}