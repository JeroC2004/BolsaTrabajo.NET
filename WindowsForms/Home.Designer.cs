namespace WindowsForms
{
    partial class Home
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            alumnosToolStripMenuItem = new ToolStripMenuItem();
            ofertasToolStripMenuItem = new ToolStripMenuItem();
            ventanaToolStripMenuItem = new ToolStripMenuItem();
            cascadaToolStripMenuItem = new ToolStripMenuItem();
            mosaicoToolStripMenuItem = new ToolStripMenuItem();
            sesionToolStripMenuItem = new ToolStripMenuItem();
            cerrarSesionToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            usuarioConectadoLabel = new ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();

            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { alumnosToolStripMenuItem, ofertasToolStripMenuItem, ventanaToolStripMenuItem, sesionToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1200, 40);
            menuStrip1.TabIndex = 0;

            alumnosToolStripMenuItem.Name = "alumnosToolStripMenuItem";
            alumnosToolStripMenuItem.Size = new Size(119, 36);
            alumnosToolStripMenuItem.Text = "Alumnos";
            alumnosToolStripMenuItem.Click += alumnosToolStripMenuItem_Click;

            ofertasToolStripMenuItem.Name = "ofertasToolStripMenuItem";
            ofertasToolStripMenuItem.Size = new Size(109, 36);
            ofertasToolStripMenuItem.Text = "Ofertas";
            ofertasToolStripMenuItem.Click += ofertasToolStripMenuItem_Click;

            ventanaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cascadaToolStripMenuItem, mosaicoToolStripMenuItem });
            ventanaToolStripMenuItem.Name = "ventanaToolStripMenuItem";
            ventanaToolStripMenuItem.Size = new Size(109, 36);
            ventanaToolStripMenuItem.Text = "Ventana";

            cascadaToolStripMenuItem.Name = "cascadaToolStripMenuItem";
            cascadaToolStripMenuItem.Size = new Size(200, 36);
            cascadaToolStripMenuItem.Text = "Cascada";
            cascadaToolStripMenuItem.Click += cascadaToolStripMenuItem_Click;

            mosaicoToolStripMenuItem.Name = "mosaicoToolStripMenuItem";
            mosaicoToolStripMenuItem.Size = new Size(200, 36);
            mosaicoToolStripMenuItem.Text = "Mosaico horizontal";
            mosaicoToolStripMenuItem.Click += mosaicoToolStripMenuItem_Click;

            sesionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cerrarSesionToolStripMenuItem });
            sesionToolStripMenuItem.Name = "sesionToolStripMenuItem";
            sesionToolStripMenuItem.Size = new Size(109, 36);
            sesionToolStripMenuItem.Text = "Sesión";

            cerrarSesionToolStripMenuItem.Name = "cerrarSesionToolStripMenuItem";
            cerrarSesionToolStripMenuItem.Size = new Size(200, 36);
            cerrarSesionToolStripMenuItem.Text = "Cerrar sesión";
            cerrarSesionToolStripMenuItem.Click += cerrarSesionToolStripMenuItem_Click;

            statusStrip1.Items.AddRange(new ToolStripItem[] { usuarioConectadoLabel });
            statusStrip1.Location = new Point(0, 728);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1200, 32);

            usuarioConectadoLabel.Name = "usuarioConectadoLabel";
            usuarioConectadoLabel.Size = new Size(200, 25);
            usuarioConectadoLabel.Text = "Conectado como: ...";

            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 760);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            IsMdiContainer = true; 
            MainMenuStrip = menuStrip1;
            Name = "Home";
            Text = "Bolsa de Trabajo Universitaria";
            WindowState = FormWindowState.Maximized;
            Load += Home_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private MenuStrip menuStrip1;
        private ToolStripMenuItem alumnosToolStripMenuItem;
        private ToolStripMenuItem ofertasToolStripMenuItem;
        private ToolStripMenuItem ventanaToolStripMenuItem;
        private ToolStripMenuItem cascadaToolStripMenuItem;
        private ToolStripMenuItem mosaicoToolStripMenuItem;
        private ToolStripMenuItem sesionToolStripMenuItem;
        private ToolStripMenuItem cerrarSesionToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel usuarioConectadoLabel;
    }
}
