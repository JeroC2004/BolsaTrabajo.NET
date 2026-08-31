namespace WindowsForms
{
    partial class Home
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            alumnosToolStripMenuItem = new ToolStripMenuItem();
            ofertasToolStripMenuItem = new ToolStripMenuItem();
            sesionToolStripMenuItem = new ToolStripMenuItem();
            cerrarSesionToolStripMenuItem = new ToolStripMenuItem();
            usuarioConectadoLabel = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            //
            // menuStrip1
            //
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { alumnosToolStripMenuItem, ofertasToolStripMenuItem, sesionToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1000, 40);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            //
            // alumnosToolStripMenuItem
            //
            alumnosToolStripMenuItem.Name = "alumnosToolStripMenuItem";
            alumnosToolStripMenuItem.Size = new Size(119, 36);
            alumnosToolStripMenuItem.Text = "Alumnos";
            alumnosToolStripMenuItem.Click += alumnosToolStripMenuItem_Click;
            //
            // ofertasToolStripMenuItem
            //
            ofertasToolStripMenuItem.Name = "ofertasToolStripMenuItem";
            ofertasToolStripMenuItem.Size = new Size(109, 36);
            ofertasToolStripMenuItem.Text = "Ofertas";
            ofertasToolStripMenuItem.Click += ofertasToolStripMenuItem_Click;
            //
            // sesionToolStripMenuItem
            //
            sesionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cerrarSesionToolStripMenuItem });
            sesionToolStripMenuItem.Name = "sesionToolStripMenuItem";
            sesionToolStripMenuItem.Size = new Size(109, 36);
            sesionToolStripMenuItem.Text = "Sesión";
            //
            // cerrarSesionToolStripMenuItem
            //
            cerrarSesionToolStripMenuItem.Name = "cerrarSesionToolStripMenuItem";
            cerrarSesionToolStripMenuItem.Size = new Size(200, 36);
            cerrarSesionToolStripMenuItem.Text = "Cerrar sesión";
            cerrarSesionToolStripMenuItem.Click += cerrarSesionToolStripMenuItem_Click;
            //
            // usuarioConectadoLabel
            //
            usuarioConectadoLabel.AutoSize = true;
            usuarioConectadoLabel.Location = new Point(20, 60);
            usuarioConectadoLabel.Name = "usuarioConectadoLabel";
            usuarioConectadoLabel.Size = new Size(200, 32);
            usuarioConectadoLabel.TabIndex = 1;
            usuarioConectadoLabel.Text = "Conectado como: ...";
            //
            // Home
            //
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 600);
            Controls.Add(usuarioConectadoLabel);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Home";
            Text = "Bolsa de Trabajo Universitaria";
            WindowState = FormWindowState.Maximized;
            Load += Home_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem alumnosToolStripMenuItem;
        private ToolStripMenuItem ofertasToolStripMenuItem;
        private ToolStripMenuItem sesionToolStripMenuItem;
        private ToolStripMenuItem cerrarSesionToolStripMenuItem;
        private Label usuarioConectadoLabel;
    }
}
