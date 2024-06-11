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
    public partial class Customers : Form
    {
        //private string connectionString = @"Data Source= DESKTOP-56CEJQR; Initial catalog=kursacBronin; Integrated Security=True";
        private string connectionString = @"Data Source= adclg1; Initial catalog=bazaБронин; Integrated Security=True";

        private SqlConnection connection;
        public Customers()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT CustomerID, CustomerName, CustomerAddress, CustomerPhone FROM Customers";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void Customers_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string address = textBox2.Text;
            string phone = textBox3.Text;

            string query = "INSERT INTO Customers (CustomerName, CustomerAddress, CustomerPhone) VALUES (@CustomerName, @CustomerAddress, @CustomerPhone)";
            ExecuteNonQuery(query, ("@CustomerName", name), ("@CustomerAddress", address), ("@CustomerPhone", phone));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int customerID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CustomerID"].Value);

                string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                ExecuteNonQuery(query, ("@CustomerID", customerID));
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadCustomers();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int customerID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CustomerID"].Value);
                string name = textBox1.Text;
                string address = textBox2.Text;
                string phone = textBox3.Text;

                string query = "UPDATE Customers SET CustomerName = @CustomerName, CustomerAddress = @CustomerAddress, CustomerPhone = @CustomerPhone WHERE CustomerID = @CustomerID";
                ExecuteNonQuery(query, ("@CustomerName", name), ("@CustomerAddress", address), ("@CustomerPhone", phone), ("@CustomerID", customerID));
            }
            else
            {
                MessageBox.Show("Выберите клиента для обновления.");
            }
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
                    LoadCustomers(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["CustomerName"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["CustomerAddress"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["CustomerPhone"].Value.ToString();
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void Customers_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
