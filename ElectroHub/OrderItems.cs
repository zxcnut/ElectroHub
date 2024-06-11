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
    public partial class OrderItems : Form
    {
        //private string connectionString = @"Data Source= DESKTOP-56CEJQR; Initial catalog=kursacBronin; Integrated Security=True";
        private string connectionString = @"Data Source= adclg1; Initial catalog=bazaБронин; Integrated Security=True";
        private SqlConnection connection;

        public OrderItems()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            LoadOrderItems();
        }

        private void LoadOrderItems()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT OrderItemID, OrderID, ProductID, Quantity, TotalAmount FROM OrderItems";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["OrderItemID"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["ProductID"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["Quantity"].Value.ToString();
                textBox4.Text = dataGridView1.SelectedRows[0].Cells["TotalAmount"].Value.ToString();
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int orderID = Convert.ToInt32(textBox1.Text);
            int productID = Convert.ToInt32(textBox2.Text);
            int quantity = Convert.ToInt32(textBox3.Text);
            decimal amount = Convert.ToDecimal(textBox4.Text);

            string query = "INSERT INTO OrderItems (OrderItemID, ProductID, Quantity, TotalAmount) VALUES (@OrderItemID, @ProductID, @Quantity, @TotalAmount)";
            ExecuteNonQuery(query, ("@OrderItemID", orderID), ("@ProductID", productID), ("@Quantity", quantity), ("@TotalAmount", amount));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int detailID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["OrderItemID"].Value);

                string query = "DELETE FROM OrderItems WHERE OrderItemID = @DetailID";
                ExecuteNonQuery(query, ("@DetailID", detailID));
            }
            else
            {
                MessageBox.Show("Выберите деталь заказа для удаления.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int detailID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["OrderItemID"].Value);
                int orderID = Convert.ToInt32(textBox1.Text);
                int productID = Convert.ToInt32(textBox2.Text);
                int quantity = Convert.ToInt32(textBox3.Text);
                decimal amount = Convert.ToDecimal(textBox4.Text);

                string query = "UPDATE OrderItems SET OrderItemID = @OrderItemID, ProductID = @ProductID, Quantity = @Quantity, TotalAmount = @TotalAmount WHERE OrderItemID = @DetailID";
                ExecuteNonQuery(query, ("@OrderItemID", orderID), ("@ProductID", productID), ("@Quantity", quantity), ("@TotalAmount", amount), ("@DetailID", detailID));
            }
            else
            {
                MessageBox.Show("Выберите деталь заказа для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadOrderItems();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
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
                    LoadOrderItems(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void OrderItems_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }
    }
}
