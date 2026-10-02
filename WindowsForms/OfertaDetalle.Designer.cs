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
            guardarButton = new Button();
            cancelarButton = new Button();
            errorProvider = new ErrorProvider(components);
            requisitoTextBox = new TextBox();
            EsExcluyentechk = new CheckBox();
            agregarButton = new Button();
            requisitosGridView = new DataGridView();
            btnEliminarRequisito = new Button();
            requisitoLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)requisitosGridView).BeginInit();
            SuspendLayout();
            // 
            // tituloLabel
            // 
            tituloLabel.AutoSize = true;
            tituloLabel.Location = new Point(16, 14);
            tituloLabel.Margin = new Padding(2, 0, 2, 0);
            tituloLabel.Name = "tituloLabel";
            tituloLabel.Size = new Size(41, 15);
            tituloLabel.TabIndex = 0;
            tituloLabel.Text = "Título:";
            // 
            // tituloTextBox
            // 
            tituloTextBox.Location = new Point(124, 13);
            tituloTextBox.Margin = new Padding(2, 1, 2, 1);
            tituloTextBox.Name = "tituloTextBox";
            tituloTextBox.Size = new Size(228, 23);
            tituloTextBox.TabIndex = 1;
            tituloTextBox.TextChanged += tituloTextBox_TextChanged;
            // 
            // empresaLabel
            // 
            empresaLabel.AutoSize = true;
            empresaLabel.Location = new Point(16, 40);
            empresaLabel.Margin = new Padding(2, 0, 2, 0);
            empresaLabel.Name = "empresaLabel";
            empresaLabel.Size = new Size(55, 15);
            empresaLabel.TabIndex = 2;
            empresaLabel.Text = "Empresa:";
            // 
            // empresaComboBox
            // 
            empresaComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            empresaComboBox.Location = new Point(124, 38);
            empresaComboBox.Margin = new Padding(2, 1, 2, 1);
            empresaComboBox.Name = "empresaComboBox";
            empresaComboBox.Size = new Size(228, 23);
            empresaComboBox.TabIndex = 3;
            // 
            // tipoOfertaLabel
            // 
            tipoOfertaLabel.AutoSize = true;
            tipoOfertaLabel.Location = new Point(16, 66);
            tipoOfertaLabel.Margin = new Padding(2, 0, 2, 0);
            tipoOfertaLabel.Name = "tipoOfertaLabel";
            tipoOfertaLabel.Size = new Size(84, 15);
            tipoOfertaLabel.TabIndex = 4;
            tipoOfertaLabel.Text = "Tipo de oferta:";
            // 
            // tipoOfertaComboBox
            // 
            tipoOfertaComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            tipoOfertaComboBox.Location = new Point(124, 64);
            tipoOfertaComboBox.Margin = new Padding(2, 1, 2, 1);
            tipoOfertaComboBox.Name = "tipoOfertaComboBox";
            tipoOfertaComboBox.Size = new Size(228, 23);
            tipoOfertaComboBox.TabIndex = 5;
            // 
            // tipoVinculoLabel
            // 
            tipoVinculoLabel.AutoSize = true;
            tipoVinculoLabel.Location = new Point(16, 91);
            tipoVinculoLabel.Margin = new Padding(2, 0, 2, 0);
            tipoVinculoLabel.Name = "tipoVinculoLabel";
            tipoVinculoLabel.Size = new Size(92, 15);
            tipoVinculoLabel.TabIndex = 6;
            tipoVinculoLabel.Text = "Tipo de vínculo:";
            // 
            // tipoVinculoComboBox
            // 
            tipoVinculoComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            tipoVinculoComboBox.Location = new Point(124, 90);
            tipoVinculoComboBox.Margin = new Padding(2, 1, 2, 1);
            tipoVinculoComboBox.Name = "tipoVinculoComboBox";
            tipoVinculoComboBox.Size = new Size(228, 23);
            tipoVinculoComboBox.TabIndex = 7;
            // 
            // estadoLabel
            // 
            estadoLabel.AutoSize = true;
            estadoLabel.Location = new Point(16, 117);
            estadoLabel.Margin = new Padding(2, 0, 2, 0);
            estadoLabel.Name = "estadoLabel";
            estadoLabel.Size = new Size(45, 15);
            estadoLabel.TabIndex = 8;
            estadoLabel.Text = "Estado:";
            // 
            // estadoComboBox
            // 
            estadoComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            estadoComboBox.Location = new Point(124, 116);
            estadoComboBox.Margin = new Padding(2, 1, 2, 1);
            estadoComboBox.Name = "estadoComboBox";
            estadoComboBox.Size = new Size(228, 23);
            estadoComboBox.TabIndex = 9;
            // 
            // fechaDesdeLabel
            // 
            fechaDesdeLabel.AutoSize = true;
            fechaDesdeLabel.Location = new Point(16, 143);
            fechaDesdeLabel.Margin = new Padding(2, 0, 2, 0);
            fechaDesdeLabel.Name = "fechaDesdeLabel";
            fechaDesdeLabel.Size = new Size(75, 15);
            fechaDesdeLabel.TabIndex = 10;
            fechaDesdeLabel.Text = "Fecha desde:";
            // 
            // fechaDesdePicker
            // 
            fechaDesdePicker.Format = DateTimePickerFormat.Short;
            fechaDesdePicker.Location = new Point(124, 142);
            fechaDesdePicker.Margin = new Padding(2, 1, 2, 1);
            fechaDesdePicker.Name = "fechaDesdePicker";
            fechaDesdePicker.Size = new Size(110, 23);
            fechaDesdePicker.TabIndex = 11;
            // 
            // fechaHastaLabel
            // 
            fechaHastaLabel.AutoSize = true;
            fechaHastaLabel.Location = new Point(16, 169);
            fechaHastaLabel.Margin = new Padding(2, 0, 2, 0);
            fechaHastaLabel.Name = "fechaHastaLabel";
            fechaHastaLabel.Size = new Size(72, 15);
            fechaHastaLabel.TabIndex = 12;
            fechaHastaLabel.Text = "Fecha hasta:";
            // 
            // fechaHastaPicker
            // 
            fechaHastaPicker.Format = DateTimePickerFormat.Short;
            fechaHastaPicker.Location = new Point(124, 167);
            fechaHastaPicker.Margin = new Padding(2, 1, 2, 1);
            fechaHastaPicker.Name = "fechaHastaPicker";
            fechaHastaPicker.Size = new Size(110, 23);
            fechaHastaPicker.TabIndex = 13;
            // 
            // detalleLabel
            // 
            detalleLabel.AutoSize = true;
            detalleLabel.Location = new Point(16, 195);
            detalleLabel.Margin = new Padding(2, 0, 2, 0);
            detalleLabel.Name = "detalleLabel";
            detalleLabel.Size = new Size(46, 15);
            detalleLabel.TabIndex = 14;
            detalleLabel.Text = "Detalle:";
            // 
            // detalleTextBox
            // 
            detalleTextBox.Location = new Point(124, 193);
            detalleTextBox.Margin = new Padding(2, 1, 2, 1);
            detalleTextBox.Multiline = true;
            detalleTextBox.Name = "detalleTextBox";
            detalleTextBox.Size = new Size(228, 40);
            detalleTextBox.TabIndex = 15;
            // 
            // guardarButton
            // 
            guardarButton.Location = new Point(196, 453);
            guardarButton.Margin = new Padding(2, 1, 2, 1);
            guardarButton.Name = "guardarButton";
            guardarButton.Size = new Size(75, 22);
            guardarButton.TabIndex = 18;
            guardarButton.Text = "Guardar";
            guardarButton.UseVisualStyleBackColor = true;
            guardarButton.Click += guardarButton_Click;
            // 
            // cancelarButton
            // 
            cancelarButton.Location = new Point(111, 453);
            cancelarButton.Margin = new Padding(2, 1, 2, 1);
            cancelarButton.Name = "cancelarButton";
            cancelarButton.Size = new Size(75, 22);
            cancelarButton.TabIndex = 19;
            cancelarButton.Text = "Cancelar";
            cancelarButton.UseVisualStyleBackColor = true;
            cancelarButton.Click += cancelarButton_Click;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // requisitoTextBox
            // 
            requisitoTextBox.Location = new Point(124, 237);
            requisitoTextBox.Multiline = true;
            requisitoTextBox.Name = "requisitoTextBox";
            requisitoTextBox.Size = new Size(228, 40);
            requisitoTextBox.TabIndex = 20;
            requisitoTextBox.TextChanged += textBox1_TextChanged;
            // 
            // EsExcluyentechk
            // 
            EsExcluyentechk.AutoSize = true;
            EsExcluyentechk.Location = new Point(175, 286);
            EsExcluyentechk.Name = "EsExcluyentechk";
            EsExcluyentechk.Size = new Size(96, 19);
            EsExcluyentechk.TabIndex = 21;
            EsExcluyentechk.Text = "Es excluyente";
            EsExcluyentechk.UseVisualStyleBackColor = true;
            EsExcluyentechk.CheckedChanged += EsExcluyentechk_CheckedChanged;
            // 
            // agregarButton
            // 
            agregarButton.Location = new Point(277, 283);
            agregarButton.Name = "agregarButton";
            agregarButton.Size = new Size(75, 23);
            agregarButton.TabIndex = 22;
            agregarButton.Text = "Agregar";
            agregarButton.UseVisualStyleBackColor = true;
            agregarButton.Click += agregarButton_Click;
            // 
            // requisitosGridView
            // 
            requisitosGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            requisitosGridView.Location = new Point(16, 312);
            requisitosGridView.Name = "requisitosGridView";
            requisitosGridView.Size = new Size(336, 98);
            requisitosGridView.TabIndex = 23;
            // 
            // btnEliminarRequisito
            // 
            btnEliminarRequisito.Location = new Point(222, 416);
            btnEliminarRequisito.Name = "btnEliminarRequisito";
            btnEliminarRequisito.Size = new Size(130, 23);
            btnEliminarRequisito.TabIndex = 24;
            btnEliminarRequisito.Text = "Eliminar seleccionado";
            btnEliminarRequisito.UseVisualStyleBackColor = true;
            btnEliminarRequisito.Click += btnEliminarRequisito_Click_1;
            // 
            // requisitoLabel
            // 
            requisitoLabel.AutoSize = true;
            requisitoLabel.Location = new Point(16, 240);
            requisitoLabel.Name = "requisitoLabel";
            requisitoLabel.Size = new Size(56, 15);
            requisitoLabel.TabIndex = 25;
            requisitoLabel.Text = "Requisito";
            requisitoLabel.Click += label1_Click;
            // 
            // OfertaDetalle
            // 
            AcceptButton = guardarButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelarButton;
            ClientSize = new Size(377, 483);
            Controls.Add(requisitoLabel);
            Controls.Add(btnEliminarRequisito);
            Controls.Add(requisitosGridView);
            Controls.Add(agregarButton);
            Controls.Add(EsExcluyentechk);
            Controls.Add(requisitoTextBox);
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
            Controls.Add(guardarButton);
            Controls.Add(cancelarButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(2, 1, 2, 1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OfertaDetalle";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Oferta";
            Load += OfertaDetalle_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)requisitosGridView).EndInit();
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
        private Button guardarButton;
        private Button cancelarButton;
        private ErrorProvider errorProvider;
        private TextBox requisitoTextBox;
        private Button agregarButton;
        private CheckBox EsExcluyentechk;
        private DataGridView requisitosGridView;
        private Label requisitoLabel;
        private Button btnEliminarRequisito;
    }
}
