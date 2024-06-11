using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectroHub
{
    public partial class Main : Form
    {
        public static Main Instance { get; private set; }
        public Main()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            string currentRole = UserManager.CurrentUser.Role;
            string userName = UserManager.CurrentUser.Username;
            label1.Text = $"Здравстуйте {userName}, вы {currentRole} <3";
            if (currentRole != "Администратор")
            {
                пользователиToolStripMenuItem.Enabled = false;
            }
        }

        private void покупателиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customers category = new Customers();
            this.Hide();
            category.ShowDialog();
        }

        private void деталиЗаказовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderItems category = new OrderItems();
            this.Hide();
            category.ShowDialog();
        }

        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Orders category = new Orders();
            this.Hide();
            category.ShowDialog();
        }

        private void товарыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Products category = new Products();
            this.Hide();
            category.ShowDialog();
        }

        private void типыПродуктовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductCategories category = new ProductCategories();
            this.Hide();
            category.ShowDialog();
        }

        private void поставщикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Vendors category = new Vendors();
            this.Hide();
            category.ShowDialog();
        }

        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users category = new Users();
            this.Hide();
            category.ShowDialog();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
