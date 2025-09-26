using Microsoft.EntityFrameworkCore;
using WinFormsApp1.Data;
using WinFormsApp1.Models;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();

        public Form1()
        {
            InitializeComponent();
        }

        void Clear()
        {
            txtFirstName.Text = txtLastName.Text = txtCity.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerId = 0;
        }

        void LoadData()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using AppDbContext db = new();
            dgvCustomer.DataSource = db.Customers.ToList<Customer>();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            this.ActiveControl = txtFirstName;
            LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();
            using AppDbContext db = new();
            if (model.CustomerId == 0)
            {
                db.Customers.Add(model);
            }
            else
            {
                db.Entry(model).State = EntityState.Modified;
            }
            db.SaveChanges();
            Clear();
            LoadData();
            MessageBox.Show("Added Successfully!");
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerId = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["dgCustomerID"].Value);
                using AppDbContext db = new();
                model = db.Customers.Where(x => x.CustomerId == model.CustomerId).FirstOrDefault();
                txtFirstName.Text = model.FirstName;
                txtLastName.Text = model.LastName;
                txtCity.Text = model.City;
                txtAddress.Text = model.Address;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to delete this record?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using AppDbContext db = new();
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                {
                    db.Customers.Attach(model);
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    LoadData();
                    Clear();
                    MessageBox.Show("Deleted Successfully!");
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}