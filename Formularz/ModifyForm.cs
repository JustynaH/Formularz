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
    public partial class ModifyForm : Form
    {
        public string connetionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContactDB;Integrated Security=True";
        public int ReturnIndex { get; set; }
        public string ReturnText { get; set; }

        public ModifyForm()
        {
            InitializeComponent();
        }

        private void ModifyForm_Load(object sender, EventArgs e)
        {
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            String query = "SELECT Id, FirstName, LastName, Phone, Mail FROM ContactDB.dbo.Person WHERE Id = " + ReturnIndex;
            SqlCommand command = new SqlCommand(query, cnn);
            SqlDataReader dataReader = command.ExecuteReader();

            dataReader.Read();
            textBoxFirstName.Text = (String)dataReader.GetValue(1);
            textBoxLastName.Text = (String)dataReader.GetValue(2);
            textBoxPhone.Text = (String)dataReader.GetValue(3);
            textBoxMail.Text = (String)dataReader.GetValue(4);

            dataReader.Close();
            command.Dispose();
            cnn.Close();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            String FN = string.IsNullOrEmpty(textBoxFirstName.Text) ? "brak" : textBoxFirstName.Text;
            String LN = string.IsNullOrEmpty(textBoxLastName.Text) ? "brak" : textBoxLastName.Text;
            String P = string.IsNullOrEmpty(textBoxPhone.Text) ? "brak" : textBoxPhone.Text;
            String M = string.IsNullOrEmpty(textBoxMail.Text) ? "brak" : textBoxMail.Text;

            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            String query = "UPDATE dbo.Person";
            query += " SET FirstName = @FirstName, LastName = @LastName, Phone = @Phone, Mail = @Mail";
            query += " WHERE ID = " + ReturnIndex;

            SqlCommand command = new SqlCommand(query, cnn);
            command.Parameters.AddWithValue("@FirstName", FN);
            command.Parameters.AddWithValue("@LastName", LN);
            command.Parameters.AddWithValue("@Phone", P);
            command.Parameters.AddWithValue("@Mail", M);
            command.ExecuteNonQuery();

            ReturnText = FN + " " + LN + "\t" + P + "\t" + M;

            command.Dispose();
            cnn.Close();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
