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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ElectroHub
{
    public partial class Users : Form
    {
        //private string connectionString = @"Data Source= DESKTOP-56CEJQR; Initial catalog=kursacBronin; Integrated Security=True";
        private string connectionString = @"Data Source= adclg1; Initial catalog=bazaБронин; Integrated Security=True";
        private SqlConnection connection;
        public Users()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            LoadUsers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            string role = textBox3.Text;

            string query = "INSERT INTO Users (Username, PasswordHash, UserRole) VALUES (@Username, @PasswordHash, @UserRole)";
            ExecuteNonQuery(query, ("@Username", login), ("@PasswordHash", password), ("@UserRole", role));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int userID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["UserID"].Value);

                string query = "DELETE FROM Users WHERE UserID = @UserID";
                ExecuteNonQuery(query, ("@UserID", userID));
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int userID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["UserID"].Value);
                string login = textBox1.Text;
                string password = textBox2.Text;
                string role = textBox3.Text;

                string query = "UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, UserRole = @UserRole WHERE UserID = @UserID";
                ExecuteNonQuery(query, ("@Username", login), ("@PasswordHash", password), ("@UserRole", role), ("@UserID", userID));
            }
            else
            {
                MessageBox.Show("Выберите пользователя для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadUsers();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["Username"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["PasswordHash"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["UserRole"].Value.ToString();
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void Users_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }

        private void LoadUsers()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT UserID, Username, PasswordHash, UserRole FROM Users";
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
                    LoadUsers(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }

        }
    }
}
