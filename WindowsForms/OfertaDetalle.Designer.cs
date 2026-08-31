namespace WindowsForms
{
    partial class OfertaDetalle
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
            components = new System.ComponentModel.Container();
            tituloLabel = new Label();
            tituloTextBox = new TextBox();
            empresaLabel = new Label();
            empresaComboBox = new ComboBox();
            tipoOfertaLabel = new Label();
            tipoOfertaComboBox = new ComboBox();
            tipoVinculoLabel = new Label();
            tipoVinculoComboBox = new ComboBox();
            estadoLabel = new Label();
            estadoComboBox = new ComboBox();
            fechaDesdeLabel = new Label();
            fechaDesdePicker = new DateTimePicker();
            fechaHastaLabel = new Label();
            fechaHastaPicker = new DateTimePicker();
            detalleLabel = new Label();
            detalleTextBox = new TextBox();
            requisitosLabel = new Label();
            requisitosTextBox = new TextBox();
            guardarButton = new Button();
            cancelarButton = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // tituloLabel
            //
            tituloLabel.AutoSize = true;
            tituloLabel.Location = new Point(30, 30);
            tituloLabel.Name = "tituloLabel";
            tituloLabel.Size = new Size(90, 32);
            tituloLabel.Text = "Título:";
            //
            // tituloTextBox
            //
            tituloTextBox.Location = new Point(230, 27);
            tituloTextBox.Name = "tituloTextBox";
            tituloTextBox.Size = new Size(420, 39);
            //
            // empresaLabel
            //
            empresaLabel.AutoSize = true;
            empresaLabel.Location = new Point(30, 85);
            empresaLabel.Name = "empresaLabel";
            empresaLabel.Size = new Size(120, 32);
            empresaLabel.Text = "Empresa:";
            //
            // empresaComboBox
            //
            empresaComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            empresaComboBox.Location = new Point(230, 82);
            empresaComboBox.Name = "empresaComboBox";
            empresaComboBox.Size = new Size(420, 40);
            //
            // tipoOfertaLabel
            //
            tipoOfertaLabel.AutoSize = true;
            tipoOfertaLabel.Location = new Point(30, 140);
            tipoOfertaLabel.Name = "tipoOfertaLabel";
            tipoOfertaLabel.Size = new Size(180, 32);
            tipoOfertaLabel.Text = "Tipo de oferta:";
            //
            // tipoOfertaComboBox
            //
            tipoOfertaComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            tipoOfertaComboBox.Location = new Point(230, 137);
            tipoOfertaComboBox.Name = "tipoOfertaComboBox";
            tipoOfertaComboBox.Size = new Size(420, 40);
            //
            // tipoVinculoLabel
            //
            tipoVinculoLabel.AutoSize = true;
            tipoVinculoLabel.Location = new Point(30, 195);
            tipoVinculoLabel.Name = "tipoVinculoLabel";
            tipoVinculoLabel.Size = new Size(180, 32);
            tipoVinculoLabel.Text = "Tipo de vínculo:";
            //
            // tipoVinculoComboBox
            //
            tipoVinculoComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            tipoVinculoComboBox.Location = new Point(230, 192);
            tipoVinculoComboBox.Name = "tipoVinculoComboBox";
            tipoVinculoComboBox.Size = new Size(420, 40);
            //
            // estadoLabel
            //
            estadoLabel.AutoSize = true;
            estadoLabel.Location = new Point(30, 250);
            estadoLabel.Name = "estadoLabel";
            estadoLabel.Size = new Size(110, 32);
            estadoLabel.Text = "Estado:";
            //
            // estadoComboBox
            //
            estadoComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            estadoComboBox.Location = new Point(230, 247);
            estadoComboBox.Name = "estadoComboBox";
            estadoComboBox.Size = new Size(420, 40);
            //
            // fechaDesdeLabel
            //
            fechaDesdeLabel.AutoSize = true;
            fechaDesdeLabel.Location = new Point(30, 305);
            fechaDesdeLabel.Name = "fechaDesdeLabel";
            fechaDesdeLabel.Size = new Size(150, 32);
            fechaDesdeLabel.Text = "Fecha desde:";
            //
            // fechaDesdePicker
            //
            fechaDesdePicker.Format = DateTimePickerFormat.Short;
            fechaDesdePicker.Location = new Point(230, 302);
            fechaDesdePicker.Name = "fechaDesdePicker";
            fechaDesdePicker.Size = new Size(200, 39);
            //
            // fechaHastaLabel
            //
            fechaHastaLabel.AutoSize = true;
            fechaHastaLabel.Location = new Point(30, 360);
            fechaHastaLabel.Name = "fechaHastaLabel";
            fechaHastaLabel.Size = new Size(150, 32);
            fechaHastaLabel.Text = "Fecha hasta:";
            //
            // fechaHastaPicker
            //
            fechaHastaPicker.Format = DateTimePickerFormat.Short;
            fechaHastaPicker.Location = new Point(230, 357);
            fechaHastaPicker.Name = "fechaHastaPicker";
            fechaHastaPicker.Size = new Size(200, 39);
            //
            // detalleLabel
            //
            detalleLabel.AutoSize = true;
            detalleLabel.Location = new Point(30, 415);
            detalleLabel.Name = "detalleLabel";
            detalleLabel.Size = new Size(110, 32);
            detalleLabel.Text = "Detalle:";
            //
            // detalleTextBox
            //
            detalleTextBox.Location = new Point(230, 412);
            detalleTextBox.Multiline = true;
            detalleTextBox.Name = "detalleTextBox";
            detalleTextBox.Size = new Size(420, 80);
            //
            // requisitosLabel
            //
            requisitosLabel.AutoSize = true;
            requisitosLabel.Location = new Point(30, 505);
            requisitosLabel.Name = "requisitosLabel";
            requisitosLabel.Size = new Size(160, 32);
            requisitosLabel.Text = "Requisitos:";
            //
            // requisitosTextBox
            //
            requisitosTextBox.Location = new Point(230, 502);
            requisitosTextBox.Multiline = true;
            requisitosTextBox.Name = "requisitosTextBox";
            requisitosTextBox.Size = new Size(420, 80);
            //
            // guardarButton
            //
            guardarButton.Location = new Point(280, 610);
            guardarButton.Name = "guardarButton";
            guardarButton.Size = new Size(140, 48);
            guardarButton.Text = "Guardar";
            guardarButton.UseVisualStyleBackColor = true;
            guardarButton.Click += guardarButton_Click;
            //
            // cancelarButton
            //
            cancelarButton.Location = new Point(440, 610);
            cancelarButton.Name = "cancelarButton";
            cancelarButton.Size = new Size(140, 48);
            cancelarButton.Text = "Cancelar";
            cancelarButton.UseVisualStyleBackColor = true;
            cancelarButton.Click += cancelarButton_Click;
            //
            // errorProvider
            //
            errorProvider.ContainerControl = this;
            //
            // OfertaDetalle
            //
            AcceptButton = guardarButton;
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelarButton;
            ClientSize = new Size(700, 690);
            Controls.Add(tituloLabel);
            Controls.Add(tituloTextBox);
            Controls.Add(empresaLabel);
            Controls.Add(empresaComboBox);
            Controls.Add(tipoOfertaLabel);
            Controls.Add(tipoOfertaComboBox);
            Controls.Add(tipoVinculoLabel);
            Controls.Add(tipoVinculoComboBox);
            Controls.Add(estadoLabel);
            Controls.Add(estadoComboBox);
            Controls.Add(fechaDesdeLabel);
            Controls.Add(fechaDesdePicker);
            Controls.Add(fechaHastaLabel);
            Controls.Add(fechaHastaPicker);
            Controls.Add(detalleLabel);
            Controls.Add(detalleTextBox);
            Controls.Add(requisitosLabel);
            Controls.Add(requisitosTextBox);
            Controls.Add(guardarButton);
            Controls.Add(cancelarButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OfertaDetalle";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Oferta";
            Load += OfertaDetalle_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label tituloLabel;
        private TextBox tituloTextBox;
        private Label empresaLabel;
        private ComboBox empresaComboBox;
        private Label tipoOfertaLabel;
        private ComboBox tipoOfertaComboBox;
        private Label tipoVinculoLabel;
        private ComboBox tipoVinculoComboBox;
        private Label estadoLabel;
        private ComboBox estadoComboBox;
        private Label fechaDesdeLabel;
        private DateTimePicker fechaDesdePicker;
        private Label fechaHastaLabel;
        private DateTimePicker fechaHastaPicker;
        private Label detalleLabel;
        private TextBox detalleTextBox;
        private Label requisitosLabel;
        private TextBox requisitosTextBox;
        private Button guardarButton;
        private Button cancelarButton;
        private ErrorProvider errorProvider;
    }
}
