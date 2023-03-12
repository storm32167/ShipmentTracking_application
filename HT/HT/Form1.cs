using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            DateTime startDate = dtStartDate.Value;
            DateTime endDate = dtEndDate.Value.AddDays(1).Date;


            // Construct the SQL query with a parameter for the start and end dates
            string query = "SELECT s.id, s.trackingCode, s.carrier,  " +
                "p2.name as receiverName, p2.email as receiverEmail, a.street, a.city, a.zipCode, a.country, " +
                "s.trackingDate, s.status, s.estimatedDeliveryDate " +
                "FROM Shipment s " +
                "JOIN Party p ON s.senderId = p.id " +
                "JOIN Party p2 ON s.receiverId = p2.id " +
                "JOIN Address a ON s.addressFromId = a.id " +
                "WHERE s.trackingDate BETWEEN @startDate AND @endDate";

            // Create a SqlCommand object with the query and parameters
            string connectionString = "Data Source=(local);Initial Catalog=ShipmentTrackingDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);

                // Create a SqlDataAdapter to execute the command and fill a DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Set the DataGridView's DataSource property to the DataTable
                dataGridView1.DataSource = dataTable;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
 
            string connectionString = "Data Source=(local);Initial Catalog=ShipmentTrackingDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT s.id, s.trackingCode, s.carrier, s.trackingDate, s.status, s.estimatedDeliveryDate, " +
                    "p2.name as receiverName, p2.email as receiverEmail, p2.phone as receiverPhone, " +
                    "a.street, a.city, a.zipCode, a.country " +
                    "FROM Shipment s " +
                    "JOIN Party p2 ON s.receiverId = p2.id " +
                    "JOIN Address a ON s.addressFromId = a.id;", connection);


                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchValue = textBoxTrackingCode.Text.Trim();
            string sqlQuery = "SELECT s.id, s.trackingCode, s.carrier,  " +
                "p2.name as receiverName, p2.email as receiverEmail, p2.phone as receiverPhone, " +
                "a.street, a.city, a.zipCode, a.country, s.trackingDate, s.status, s.estimatedDeliveryDate " +
                "FROM Shipment s " +
                "JOIN Party p ON s.senderId = p.id " +
                "JOIN Party p2 ON s.receiverId = p2.id " +
                "JOIN Address a ON s.addressFromId = a.id ";

            if (!string.IsNullOrEmpty(searchValue))
            {
                sqlQuery += "WHERE s.trackingCode LIKE @TrackingCode";
            }

            string connectionString = "Data Source=(local);Initial Catalog=ShipmentTrackingDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                if (!string.IsNullOrEmpty(searchValue))
                {
                    command.Parameters.AddWithValue("@TrackingCode", "%" + searchValue + "%");
                }
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }

        }
    }
}