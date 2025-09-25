namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            txtFirstName = new TextBox();
            btnSave = new Button();
            dgvCustomer = new DataGridView();
            dgCustomerID = new DataGridViewTextBoxColumn();
            dgFirstName = new DataGridViewTextBoxColumn();
            dgLastName = new DataGridViewTextBoxColumn();
            dgCity = new DataGridViewTextBoxColumn();
            txtLastName = new TextBox();
            label2 = new Label();
            txtCity = new TextBox();
            label3 = new Label();
            txtAddress = new TextBox();
            label4 = new Label();
            btnDelete = new Button();
            btnCancel = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvCustomer).BeginInit();
            SuspendLayout();
            // 
            // txtFirstName
            // 
            txtFirstName.Location = new Point(103, 66);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(245, 23);
            txtFirstName.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(33, 251);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(101, 23);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // dgvCustomer
            // 
            dgvCustomer.AllowUserToDeleteRows = false;
            dgvCustomer.BackgroundColor = Color.White;
            dgvCustomer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCustomer.Columns.AddRange(new DataGridViewColumn[] { dgCustomerID, dgFirstName, dgLastName, dgCity });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(192, 255, 255);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvCustomer.DefaultCellStyle = dataGridViewCellStyle1;
            dgvCustomer.Location = new Point(387, 8);
            dgvCustomer.Name = "dgvCustomer";
            dgvCustomer.ReadOnly = true;
            dgvCustomer.RowTemplate.Height = 30;
            dgvCustomer.Size = new Size(401, 430);
            dgvCustomer.TabIndex = 3;
            dgvCustomer.DoubleClick += dgvCustomer_DoubleClick;
            // 
            // dgCustomerID
            // 
            dgCustomerID.DataPropertyName = "CustomerID";
            dgCustomerID.HeaderText = "CustomerID";
            dgCustomerID.Name = "dgCustomerID";
            dgCustomerID.ReadOnly = true;
            dgCustomerID.Visible = false;
            // 
            // dgFirstName
            // 
            dgFirstName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgFirstName.DataPropertyName = "FirstName";
            dgFirstName.HeaderText = "First Name";
            dgFirstName.Name = "dgFirstName";
            dgFirstName.ReadOnly = true;
            // 
            // dgLastName
            // 
            dgLastName.DataPropertyName = "LastName";
            dgLastName.HeaderText = "Last Name";
            dgLastName.Name = "dgLastName";
            dgLastName.ReadOnly = true;
            // 
            // dgCity
            // 
            dgCity.DataPropertyName = "City";
            dgCity.HeaderText = "City";
            dgCity.Name = "dgCity";
            dgCity.ReadOnly = true;
            // 
            // txtLastName
            // 
            txtLastName.Location = new Point(102, 95);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(246, 23);
            txtLastName.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(33, 103);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 6;
            label2.Text = "Last Name";
            // 
            // txtCity
            // 
            txtCity.Location = new Point(103, 129);
            txtCity.Name = "txtCity";
            txtCity.Size = new Size(245, 23);
            txtCity.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(33, 132);
            label3.Name = "label3";
            label3.Size = new Size(28, 15);
            label3.TabIndex = 8;
            label3.Text = "City";
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(103, 158);
            txtAddress.Multiline = true;
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(245, 87);
            txtAddress.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(33, 166);
            label4.Name = "label4";
            label4.Size = new Size(49, 15);
            label4.TabIndex = 10;
            label4.Text = "Address";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(140, 251);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(101, 23);
            btnDelete.TabIndex = 12;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(247, 251);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(101, 23);
            btnCancel.TabIndex = 13;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 69);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 14;
            label1.Text = "First Name";
            label1.Click += label1_Click_1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(128, 128, 255);
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnDelete);
            Controls.Add(txtAddress);
            Controls.Add(label4);
            Controls.Add(txtCity);
            Controls.Add(label3);
            Controls.Add(txtLastName);
            Controls.Add(label2);
            Controls.Add(dgvCustomer);
            Controls.Add(btnSave);
            Controls.Add(txtFirstName);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EF CRUD Operations";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCustomer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtFirstName;
        private Button btnSave;
        private DataGridView dgvCustomer;
        private TextBox txtLastName;
        private Label label2;
        private TextBox txtCity;
        private Label label3;
        private TextBox txtAddress;
        private Label label4;
        private Button btnDelete;
        private Button btnCancel;
        private Label label1;
        private DataGridViewTextBoxColumn dgCustomerID;
        private DataGridViewTextBoxColumn dgFirstName;
        private DataGridViewTextBoxColumn dgLastName;
        private DataGridViewTextBoxColumn dgCity;
    }
}
