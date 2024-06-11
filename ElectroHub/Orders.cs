using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ElectroHub
{
    public partial class Orders : Form
    {
        //private string connectionString = @"Data Source= DESKTOP-56CEJQR; Initial catalog=kursacBronin; Integrated Security=True";
        private string connectionString = @"Data Source= adclg1; Initial catalog=bazaБронин; Integrated Security=True";
        private SqlConnection connection;
        public Orders()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
        }

        private void Orders_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime date = dateTimePicker1.Value;

            string status = textBox1.Text;
            int customerID;
            if (!int.TryParse(textBox2.Text, out customerID))
            {
                MessageBox.Show("Невозможно преобразовать идентификатор заказчика в число.");
                return;
            }

            string query = "INSERT INTO Orders (OrderDate, OrderStatus, CustomerID) VALUES (@OrderDate, @OrderStatus, @CustomerID)";
            ExecuteNonQuery(query, ("@OrderDate", date), ("@OrderStatus", status), ("@CustomerID", customerID));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["OrderID"].Value);

                string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
                string query1 = "DELETE FROM OrderItems WHERE OrderID = @OrderID";
                ExecuteNonQuery(query1, ("@OrderID", orderID));
                ExecuteNonQuery(query, ("@OrderID", orderID));

            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["OrderID"].Value);
                DateTime date = dateTimePicker1.Value;
                string status = textBox2.Text;
                int customerID = Convert.ToInt32(textBox2.Text);

                string query = "UPDATE Orders SET OrderDate = @OrderDate, OrderStatus = @OrderStatus, CustomerID = @CustomerID WHERE OrderID = @OrderID";
                ExecuteNonQuery(query, ("@OrderDate", date), ("@OrderStatus", status), ("@CustomerID", customerID), ("@OrderID", orderID));
            }
            else
            {
                MessageBox.Show("Выберите заказ для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadOrders();
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string dateString = dataGridView1.SelectedRows[0].Cells["OrderDate"].Value.ToString();
                DateTime date;

                if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    dateTimePicker1.Value = date;
                }


                textBox1.Text = dataGridView1.SelectedRows[0].Cells["OrderStatus"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["CustomerID"].Value.ToString();
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
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
                    LoadOrders(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void LoadOrders()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT OrderID, OrderDate, OrderStatus, CustomerID FROM Orders";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            LoadOrders();
            // Устанавливаем формат даты для txtOrderDate
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
