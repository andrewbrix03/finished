using System.Data.SqlClient;
using System.Windows.Forms;
using System;
using BillingSystemDesign;
using BillingSystemDesgin;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BillingSystemDesign
{
    public partial class LoginPage : Form
    {
        private string connectionString = "Data Source=DESKTOP-TU0VOQH\\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True";
        private const int AdminContactNumber = 011504;
        private const string AdminPassword = "1234";

        public LoginPage()
        {
            InitializeComponent();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm(this);
            registerForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int enteredContactNumber;
            string enteredPassword;

            if (!int.TryParse(textBox1.Text, out enteredContactNumber))
            {
                MessageBox.Show("Invalid input for contact number.");
                return;
            }
            enteredPassword = textBox2.Text;

            if (enteredContactNumber == AdminContactNumber && enteredPassword == AdminPassword)
            {
                MessageBox.Show("Admin login successful!");
                OpenBlankForm();
                this.Hide();
            }
            else
            {
                if (CheckRegisteredAccount(enteredContactNumber, enteredPassword))
                {
                    OpenPaymentPage(enteredContactNumber, enteredPassword);
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid credentials.");
                }
            }
        }


        private bool CheckRegisteredAccount(int enteredContactNumber, string enteredPassword)
        {
            string query = "SELECT 1 FROM Registration WHERE contact_number = @ContactNumber AND contact_password = @ContactPassword";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactNumber", enteredContactNumber);
                        command.Parameters.AddWithValue("@ContactPassword", enteredPassword);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            return reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
        }

        private void OpenBlankForm()
        {
            Blank blankForm = new Blank(this);
            blankForm.Show();
            this.Hide();
        }

        private void OpenPaymentPage(int contactNumber, string contactPassword)
        {
            PaymentPage paymentPage = new PaymentPage
            {
                ContactNumber = contactNumber,
                ContactPassword = contactPassword
            };
            paymentPage.SetContactName();
            paymentPage.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
