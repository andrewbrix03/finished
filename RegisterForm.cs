using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BillingSystemDesign
{
        public partial class RegisterForm : Form
        {
            private LoginPage loginPage;

            public RegisterForm(LoginPage page)
            {
                InitializeComponent();
                loginPage = page;
            }

            private void button2_Click(object sender, EventArgs e)
            {
                loginPage.Show();
                this.Close();
            }

        private void button1_Click(object sender, EventArgs e)
        {
            string contactName = textBox1.Text;
            int contactNumber;
            string contactPassword = textBox2.Text;

            if (!int.TryParse(textBox3.Text, out contactNumber))
            {
                MessageBox.Show("Invalid input for contact number.");
                return;
            }
            if (string.IsNullOrEmpty(contactPassword))
            {
                MessageBox.Show("Password cannot be empty.");
                return;
            }

            string connectionString = "Data Source=DESKTOP-TU0VOQH\\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Registration (contact_name, contact_number, contact_password) VALUES (@ContactName, @ContactNumber, @ContactPassword)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactName", contactName);
                        command.Parameters.AddWithValue("@ContactNumber", contactNumber);
                        command.Parameters.AddWithValue("@ContactPassword", contactPassword);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Successfully Registered!");
                        }
                        else
                        {
                            MessageBox.Show("Unsuccessfully Registered.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            loginPage.Show();
            this.Close();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}