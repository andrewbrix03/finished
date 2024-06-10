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
    public partial class ElectricalForm : Form
    {
        public int ContactNumber { get; set; }
        public string ContactPassword { get; set; }
        public string ContactName { get; set; }
        private string connectionString = "Data Source=DESKTOP-TU0VOQH\\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True";

        public ElectricalForm()
        {
            InitializeComponent();
            this.Load += ElectricalForm_Load;
        }

        private void ElectricalForm_Load(object sender, EventArgs e)
        {
            SetElectricBill();
            SetLastPaymentDateElectric();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PaymentPage paymentPage = new PaymentPage
            {
                ContactNumber = ContactNumber,
                ContactPassword = ContactPassword
            };
            paymentPage.SetContactName();
            paymentPage.Show();
            this.Close();
        }

        public void SetElectricBill()
        {
            label5.Text = GetElectricBill(ContactNumber, ContactPassword);
        }

        private string GetElectricBill(int contactNumber, string contactPassword)
        {
            string query = "SELECT electric_bill FROM Registration WHERE contact_number = @ContactNumber AND contact_password = @ContactPassword";
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

        private void SetLastPaymentDateElectric()
        {
            string query = "SELECT last_payment_date_electric FROM Registration WHERE contact_number = @ContactNumber AND contact_password = @ContactPassword";
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
                                MessageBox.Show("Your electric bill payment due date has passed.", "Electric Bill Due", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void UpdateElectricBill(decimal newBillAmount)
        {
            string query = "UPDATE Registration SET electric_bill = @NewElectricBill, last_payment_date_electric = @CurrentDate WHERE contact_number = @ContactNumber AND contact_password = @ContactPassword";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewElectricBill", newBillAmount);
                        command.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
                        command.Parameters.AddWithValue("@ContactNumber", ContactNumber);
                        command.Parameters.AddWithValue("@ContactPassword", ContactPassword);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Electric bill updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Electric bill update unsuccessful.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBox2.Text, out decimal paymentAmount))
            {
                if (decimal.TryParse(label5.Text, out decimal currentBillAmount))
                {
                    decimal newBillAmount = currentBillAmount - paymentAmount;

                    if (newBillAmount < 0)
                    {
                        newBillAmount = 0;
                    }

                    UpdateElectricBill(newBillAmount);
                    label5.Text = newBillAmount.ToString();

                    string message = $"Dear {ContactName},\n\n";
                    message += $"You have successfully paid {paymentAmount:C} for your electric bill.\n";
                    message += $"Your remaining electric bill amount is now {newBillAmount:C}.\n";
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
    }
}