namespace Cajero_archivos
{
    partial class Frloggin
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtTarjeta = new System.Windows.Forms.TextBox();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.txtRFID = new System.Windows.Forms.TextBox();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.lblEstadoRFID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTarjeta
            // 
            this.txtTarjeta.Location = new System.Drawing.Point(45, 45);
            this.txtTarjeta.Name = "txtTarjeta";
            this.txtTarjeta.Size = new System.Drawing.Size(184, 20);
            this.txtTarjeta.TabIndex = 0;
            // 
            // txtPin
            // 
            this.txtPin.Location = new System.Drawing.Point(45, 89);
            this.txtPin.Name = "txtPin";
            this.txtPin.Size = new System.Drawing.Size(184, 20);
            this.txtPin.TabIndex = 1;
            // 
            // txtRFID
            // 
            this.txtRFID.Location = new System.Drawing.Point(45, 132);
            this.txtRFID.Name = "txtRFID";
            this.txtRFID.Size = new System.Drawing.Size(184, 20);
            this.txtRFID.TabIndex = 2;
            // 
            // btnIngresar
            // 
            this.btnIngresar.Location = new System.Drawing.Point(45, 170);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(75, 23);
            this.btnIngresar.TabIndex = 3;
            this.btnIngresar.Text = "Ingresar";
            this.btnIngresar.UseVisualStyleBackColor = true;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // lblEstadoRFID
            // 
            this.lblEstadoRFID.AutoSize = true;
            this.lblEstadoRFID.Location = new System.Drawing.Point(148, 175);
            this.lblEstadoRFID.Name = "lblEstadoRFID";
            this.lblEstadoRFID.Size = new System.Drawing.Size(35, 13);
            this.lblEstadoRFID.TabIndex = 4;
            this.lblEstadoRFID.Text = "label1";
            // 
            // Frloggin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblEstadoRFID);
            this.Controls.Add(this.btnIngresar);
            this.Controls.Add(this.txtRFID);
            this.Controls.Add(this.txtPin);
            this.Controls.Add(this.txtTarjeta);
            this.Name = "Frloggin";
            this.Text = "Loggin";
            this.Load += new System.EventHandler(this.Frloggin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTarjeta;
        private System.Windows.Forms.TextBox txtPin;
        private System.Windows.Forms.TextBox txtRFID;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.Label lblEstadoRFID;
    }
}

