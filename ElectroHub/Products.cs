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
    public partial class Products : Form
    {
        //private string connectionString = @"Data Source= DESKTOP-56CEJQR; Initial catalog=kursacBronin; Integrated Security=True";
        private string connectionString = @"Data Source= adclg1; Initial catalog=bazaБронин; Integrated Security=True";
        private SqlConnection connection;
        public Products()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            LoadProducts();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string description = textBox2.Text;
            decimal price = Convert.ToDecimal(textBox3.Text);
            int quantity = Convert.ToInt32(textBox4.Text);
            int productTypeID = Convert.ToInt32(textBox5.Text);

            string query = "INSERT INTO Products (ProductName, ProductDescription, ProductPrice, ProductQuantity, CategoryID) VALUES (@ProductName, @ProductDescription, @ProductPrice, @ProductQuantity, @CategoryID)";
            ExecuteNonQuery(query, ("@ProductName", name), ("@ProductDescription", description), ("@ProductPrice", price), ("@ProductQuantity", quantity), ("@CategoryID", productTypeID));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProductID"].Value);

                string query = "DELETE FROM Products WHERE ProductID = @ProductID";
                ExecuteNonQuery(query, ("@ProductID", productID));
            }
            else
            {
                MessageBox.Show("Выберите продукт для удаления.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProductID"].Value);
                string name = textBox1.Text;
                string description = textBox2.Text;
                decimal price = Convert.ToDecimal(textBox3.Text);
                int quantity = Convert.ToInt32(textBox4.Text);
                int productTypeID = Convert.ToInt32(textBox5.Text);

                string query = "UPDATE Products SET ProductName = @ProductName, ProductDescription = @ProductDescription, ProductPrice = @ProductPrice, ProductQuantity = @ProductQuantity, CategoryID = @CategoryID WHERE ProductID = @ProductID";
                ExecuteNonQuery(query, ("@ProductName", name), ("@ProductDescription", description), ("@ProductPrice", price), ("@ProductQuantity", quantity), ("@CategoryID", productTypeID), ("@ProductID", productID));
            }
            else
            {
                MessageBox.Show("Выберите продукт для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadProducts();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["ProductName"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["ProductDescription"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["ProductPrice"].Value.ToString();
                textBox4.Text = dataGridView1.SelectedRows[0].Cells["ProductQuantity"].Value.ToString();
                textBox5.Text = dataGridView1.SelectedRows[0].Cells["CategoryID"].Value.ToString();
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void LoadProducts()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT ProductID, ProductName, ProductDescription, ProductPrice, ProductQuantity, CategoryID FROM Products";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void Products_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }
    }
}
