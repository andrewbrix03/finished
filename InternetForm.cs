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



namespace BillingSystemDesgin
{
    public partial class InternetForm : Form
    {
        public int ContactNumber { get; set; }
        public string ContactPassword { get; set; }
        public string ContactName { get; set; }

        private string connectionString = "Data Source=DESKTOP-TU0VOQH\\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True";

        public InternetForm()
        {
            InitializeComponent();
            this.Load += InternetForm_Load;
        }

        private void InternetForm_Load(object sender, EventArgs e)
        {
            SetInternetBill();
            SetLastPaymentDateCable();
        }

        private void SetInternetBill()
        {
            label4.Text = GetInternetBill(ContactNumber, ContactPassword);
        }

        private string GetInternetBill(int contactNumber, string contactPassword)
        {
            string query = "SELECT cable_bill FROM Registration WHERE contact_number = @ContactNumber AND contact_password = @ContactPassword";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactNumber", contactNumber);
                        command.Parameters.AddWithValue("@ContactPassword", contactPassword);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            return "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return "Error";
            }
        }

        private void SetLastPaymentDateCable()
        {
            string query = "SELECT last_payment_date_cable FROM Registration WHERE contact_number = @ContactNumber AND contact_password = @ContactPassword";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactNumber", ContactNumber);
                        command.Parameters.AddWithValue("@ContactPassword", ContactPassword);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            DateTime lastPaymentDate = Convert.ToDateTime(result);
                            label6.Text = lastPaymentDate.ToString("yyyy-MM-dd");

                            if (DateTime.Now > lastPaymentDate)
                            {
                                MessageBox.Show("Your payment due is passed", "Cutting Internet", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            label6.Text = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void UpdateInternetBill(decimal newBillAmount)
        {
            string query = "UPDATE Registration SET cable_bill = @NewInternetBill, last_payment_date_cable = @CurrentDate WHERE contact_number = @ContactNumber AND contact_password = @ContactPassword";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewInternetBill", newBillAmount);
                        command.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
                        command.Parameters.AddWithValue("@ContactNumber", ContactNumber);
                        command.Parameters.AddWithValue("@ContactPassword", ContactPassword);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Internet bill updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Internet bill update unsuccessful.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void buttonPay_Click_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBox2.Text, out decimal paymentAmount))
            {
                if (decimal.TryParse(label4.Text, out decimal currentBillAmount))
                {
                    decimal newBillAmount = currentBillAmount - paymentAmount;

                    if (newBillAmount < 0)
                    {
                        newBillAmount = 0;
                    }

                    UpdateInternetBill(newBillAmount);
                    label4.Text = newBillAmount.ToString();

                    string message = $"Dear {ContactName},\n\n";
                    message += $"You have successfully paid {paymentAmount:C} for your internet bill.\n";
                    message += $"Your remaining internet bill amount is now {newBillAmount:C}.\n";
                    message += $"Thank you for your payment.";

                    MessageBox.Show(message);
                }
                else
                {
                    MessageBox.Show("The current bill amount is invalid.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid payment amount.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PaymentPage paymentPage = new PaymentPage();
            paymentPage.ContactNumber = ContactNumber;
            paymentPage.ContactPassword = ContactPassword;
            paymentPage.SetContactName();
            paymentPage.Show();
            this.Close();
        }
    }
}