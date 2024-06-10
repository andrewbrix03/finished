using BillingSystemDesgin;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace BillingSystemDesign
{
    public partial class Blank : Form
    {
        private LoginPage loginPage;
        private string connectionString = "Data Source=DESKTOP-TU0VOQH\\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True";

        public Blank(LoginPage page)
        {
            InitializeComponent();
            loginPage = page;
            LoadRegisteredAccounts();
            button2.Click += button2_Click;
            button3.Click += button3_Click;
            button1.Click += button1_Click;
        }

        private void LoadRegisteredAccounts()
        {
            string query = "SELECT contact_number, contact_name, contact_password, electric_bill, water_bill, cable_bill, last_payment_date_water,last_payment_date_cable ,last_payment_date_electric FROM Registration";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["contact_number"].Value.ToString();
                textBox2.Text = row.Cells["contact_name"].Value.ToString();
                textBox3.Text = row.Cells["contact_password"].Value.ToString();
                textBox4.Text = row.Cells["electric_bill"].Value.ToString();
                textBox5.Text = row.Cells["water_bill"].Value.ToString();
                textBox6.Text = row.Cells["cable_bill"].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Registration SET contact_name = @contact_name, contact_password = @contact_password, electric_bill = @electric_bill, water_bill = @water_bill, cable_bill = @cable_bill, last_payment_date_water = @last_payment_date_water, last_payment_date_cable = @last_payment_date_cable, last_payment_date_electric = @last_payment_date_electric WHERE contact_number = @contact_number";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@contact_number", int.Parse(textBox1.Text));
                cmd.Parameters.AddWithValue("@contact_name", textBox2.Text);
                cmd.Parameters.AddWithValue("@contact_password", textBox3.Text);
                cmd.Parameters.AddWithValue("@electric_bill", decimal.Parse(textBox4.Text));
                cmd.Parameters.AddWithValue("@water_bill", decimal.Parse(textBox5.Text));
                cmd.Parameters.AddWithValue("@cable_bill", decimal.Parse(textBox6.Text));
                cmd.Parameters.AddWithValue("@last_payment_date_water", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@last_payment_date_cable", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@last_payment_date_electric", dateTimePicker3.Value);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    LoadRegisteredAccounts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Registration WHERE contact_number = @contact_number";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@contact_number", int.Parse(textBox1.Text));
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    LoadRegisteredAccounts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loginPage.Show();
            this.Close();
        }
    }
}