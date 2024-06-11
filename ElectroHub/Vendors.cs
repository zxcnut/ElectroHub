using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectroHub
{
    public partial class Vendors : Form
    {
        //private string connectionString = @"Data Source= DESKTOP-56CEJQR; Initial catalog=kursacBronin; Integrated Security=True";
        private string connectionString = @"Data Source= adclg1; Initial catalog=bazaБронин; Integrated Security=True";
        private SqlConnection connection;
        public Vendors()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            LoadVendors();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["VendorName"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["VendorAddress"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["VendorPhone"].Value.ToString();
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string address = textBox2.Text;
            string phone = textBox3.Text;
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Ошибка, недопустимые значения, заполните все поля!");
            }

            string query = "INSERT INTO Vendors (VendorName, VendorAddress, VendorPhone) VALUES (@VendorName, @VendorAddress, @VendorPhone)";
            ExecuteNonQuery(query, ("@VendorName", name), ("@VendorAddress", address), ("@VendorPhone", phone));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int supplierID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["VendorID"].Value);

                string query = "DELETE FROM Vendors WHERE VendorID = @VendorID";
                ExecuteNonQuery(query, ("@VendorID", supplierID));
            }
            else
            {
                MessageBox.Show("Выберите поставщика для удаления.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int supplierID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["VendorID"].Value);
                string name = textBox1.Text;
                string address = textBox2.Text;
                string phone = textBox3.Text;

                string query = "UPDATE Vendors SET VendorName = @VendorName, VendorAddress = @VendorAddress, VendorPhone = @VendorPhone WHERE VendorID = @VendorID";
                ExecuteNonQuery(query, ("@VendorName", name), ("@VendorAddress", address), ("@VendorPhone", phone), ("@VendorID", supplierID));
            }
            else
            {
                MessageBox.Show("Выберите поставщика для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadVendors();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void ExecuteNonQuery(string query, params (string, object)[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                }

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Операция успешно выполнена.");
                    LoadVendors(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void LoadVendors()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT VendorID, VendorName, VendorAddress, VendorPhone FROM Vendors";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Vendors_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }
    }
}
