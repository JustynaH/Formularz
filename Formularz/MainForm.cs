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
    public partial class MainForm : Form
    {
        public string connetionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContactDB;Integrated Security=True";
        List<PersonalData> record = new List<PersonalData>();
       
        public MainForm()
        {
            InitializeComponent();
            listBox_Load();
        }

        //Load listBoxContacts
        private void listBox_Load()    
        {
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            String query = "SELECT Id, FirstName, LastName, Phone, Mail FROM ContactDB.dbo.Person ORDER BY LastName";
            SqlCommand command = new SqlCommand(query, cnn);

            SqlDataReader dataReader = command.ExecuteReader();

            String txt;

            while(dataReader.Read())
            {
                txt = dataReader.GetValue(1) + " " + dataReader.GetValue(2) + "\t" + dataReader.GetValue(3) + "\t" + dataReader.GetValue(4);
                record.Add(new PersonalData() { Index = (int)dataReader.GetValue(0), Text = txt });
            }

            listBoxContacts.DataSource = record;
            listBoxContacts.DisplayMember = "Text";

            dataReader.Close();
            command.Dispose();
            cnn.Close();
        }

        private void refreshList()
        {
            listBoxContacts.DataSource = null;
            listBoxContacts.DataSource = record;
            listBoxContacts.DisplayMember = "Text";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm();
            addForm.ShowDialog();
            if (addForm.DialogResult == DialogResult.OK)
            {
                int i = addForm.ReturnIndex;
                String txt = addForm.ReturnText;
                record.Add(new PersonalData() { Index = i, Text = txt });
                refreshList();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            ModifyForm modifyForm = new ModifyForm();
            PersonalData item = (PersonalData)listBoxContacts.Items[listBoxContacts.SelectedIndex];
            modifyForm.ReturnIndex = item.Index;

            modifyForm.ShowDialog();
            if (modifyForm.DialogResult == DialogResult.OK)
            {
                record[listBoxContacts.SelectedIndex].Text = modifyForm.ReturnText;
                refreshList();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedId = listBoxContacts.SelectedIndex;
            PersonalData item = (PersonalData)listBoxContacts.Items[selectedId];

            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            String query = "DELETE FROM dbo.Person";
            query += " WHERE (@Id = Id)";

            SqlCommand command = new SqlCommand(query, cnn);
            command.Parameters.AddWithValue("@Id", item.Index);
            record.Remove(item);

            refreshList();

            command.ExecuteNonQuery();
            cnn.Close();
        }
    }
}
