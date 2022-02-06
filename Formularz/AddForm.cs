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

namespace Formularz
{
    public partial class AddForm : Form
    {
        public string connetionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContactDB;Integrated Security=True";
        public int ReturnIndex { get; set; }
        public string ReturnText { get; set; }

        public AddForm()
        {
            InitializeComponent();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            String query = "INSERT INTO dbo.Person (FirstName, LastName, Phone, Mail)";
            query += " VALUES (@FirstName, @LastName, @Phone, @Mail)";

            SqlCommand command = new SqlCommand(query, cnn);
            command.Parameters.AddWithValue("@FirstName", string.IsNullOrEmpty(textBoxFirstName.Text) ? "brak" : textBoxFirstName.Text);
            command.Parameters.AddWithValue("@LastName", string.IsNullOrEmpty(textBoxLastName.Text) ? "brak" : textBoxLastName.Text);
            command.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(textBoxPhone.Text) ? "brak" : textBoxPhone.Text);
            command.Parameters.AddWithValue("@Mail", string.IsNullOrEmpty(textBoxMail.Text) ? "brak" : textBoxMail.Text);
            command.ExecuteNonQuery();

            //Extract and pass Id to MainForm
            query = "SELECT TOP 1 Id, FirstName, LastName, Phone, Mail FROM dbo.Person ORDER BY Id DESC";
            command = new SqlCommand(query, cnn);
            SqlDataReader dataReader = command.ExecuteReader();
            dataReader.Read();
            ReturnIndex = (int)dataReader.GetValue(0);
            ReturnText = dataReader.GetValue(1) + " " + dataReader.GetValue(2) + "\t" + dataReader.GetValue(3) + "\t" + dataReader.GetValue(4);

            command.Dispose();
            cnn.Close();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
