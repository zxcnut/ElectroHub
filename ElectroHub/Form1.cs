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
    public partial class Form1 : Form
    {
        //string connectionString = @"Data Source= DESKTOP-56CEJQR; Initial catalog=kursacBronin; Integrated Security=True";
        private string connectionString = @"Data Source= adclg1; Initial catalog=bazaБронин; Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UserRole FROM Users WHERE Username = @Username and PasswordHash = @PasswordHash";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", login);
                command.Parameters.AddWithValue("@PasswordHash", password);

                connection.Open();
                object role = command.ExecuteScalar();
                if (role != null)
                {
                    string userRole = (string)role;
                    UserManager.SetCurrentUser(login, userRole);

                    Main form2 = new Main();
                    form2.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
