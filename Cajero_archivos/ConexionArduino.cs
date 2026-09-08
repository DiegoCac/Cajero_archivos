using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace Cajero_archivos
{
    public class ConexionArduino
    {
        private SerialPort puerto;

        public ConexionArduino()
        {
            puerto = new SerialPort("COM4", 9600);
            puerto.DataReceived += Puerto_DataReceived;
        }

        public void Conectar()
        {
            if (!puerto.IsOpen)
            {
                puerto.Open();
                Console.WriteLine("Conectado al Arduino en COM4");
            }
        }

        public void Desconectar()
        {
            if (puerto.IsOpen)
            {
                puerto.Close();
            }
        }

        private void Puerto_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string mensaje = puerto.ReadLine();
            MessageBox.Show("Arduino dice: " + mensaje);
        }
    }
}