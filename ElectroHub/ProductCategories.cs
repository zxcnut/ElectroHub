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
    public partial class ProductCategories : Form
    {
        //private string connectionString = @"Data Source= DESKTOP-56CEJQR; Initial catalog=kursacBronin; Integrated Security=True";
        private string connectionString = @"Data Source= adclg1; Initial catalog=bazaБронин; Integrated Security=True";
        private SqlConnection connection;
        public ProductCategories()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string typeName = textBox1.Text;
            if (!string.IsNullOrWhiteSpace(typeName))
            {


                string query = "INSERT INTO ProductCategories (CategoryName) VALUES (@CategoryName)";
                ExecuteNonQuery(query, ("@CategoryName", typeName));
                LoadProductCategories();
                MessageBox.Show("Категория успешно добавлена.");
            }
            else
            {
                MessageBox.Show("Введите название категории.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productTypeID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CategoryID"].Value);

                string query = "DELETE FROM ProductCategories WHERE CategoryID = @CategoryID";
                ExecuteNonQuery(query, ("@CategoryID", productTypeID));
            }
            else
            {
                MessageBox.Show("Выберите тип продукта для удаления.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productTypeID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CategoryID"].Value);
                string typeName = textBox1.Text;

                string query = "UPDATE ProductCategories SET CategoryName = @CategoryName WHERE CategoryID = @CategoryID";
                ExecuteNonQuery(query, ("@CategoryName", typeName), ("@CategoryID", productTypeID));
            }
            else
            {
                MessageBox.Show("Выберите тип продукта для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadProductCategories();
            textBox1.Text = "";
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
                    LoadProductCategories(); // Перезагружаем данные после выполнения операции
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
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["CategoryName"].Value.ToString();
            }
        }

        private void ProductCategories_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }

        private void ProductCategories_Load(object sender, EventArgs e)
        {
            LoadProductCategories();
        }

        private void LoadProductCategories()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT CategoryID, CategoryName FROM ProductCategories";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }
    }
}
