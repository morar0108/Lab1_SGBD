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

namespace Lab1_SGBD_incercare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //connectionString
        string connectionString = "Data Source=DESKTOP-E7J20AH\\SQLEXPRESS03;Initial Catalog=Fahrschule;" + "Integrated Security=true";
        
        //binding source 
        BindingSource tbNameBS = new BindingSource();

        private void btnOpenConnection_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter;

            string AusbilderQuery = "select * from Fahrschule_Daten";
            string Kategorien = "select * from Ausbilders";

            //create a Dataset
            DataSet ds1 = new DataSet();

            try
            {
                //open connection and create DataAdapter
                connection.Open();
                adapter = new SqlDataAdapter(AusbilderQuery, connection);
                adapter.Fill(ds1, "Fahrschule_Daten");
                adapter = new SqlDataAdapter(Kategorien, connection);
                adapter.Fill(ds1, "Ausbilders");


                ds1.Relations.Add("Ausbilder in Fahrschule",
                    ds1.Tables["Fahrschule_Daten"].Columns["FahrschuleID"],
                    ds1.Tables["Ausbilders"].Columns["FahrschuleID"]
                    );
                connection.Close();

                dataGridView1.DataSource = ds1.Tables["Fahrschule_Daten"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                connection.Close();
            }

        }

        private void getData()
        {
            //get the row number on the DataGrid
            int iRownr = this.dataGridView1.CurrentCell.RowIndex;

         
            //get the content of the cell with the Id
            object cellvalue1 = this.dataGridView1[0, iRownr].Value;
            string IdString = cellvalue1.ToString();
            int Id = Int32.Parse(IdString);
           

            //create connection
            SqlConnection connection = new SqlConnection(connectionString);

            //create the command and parameter objects
            string childQuery = "SELECT * FROM Ausbilders WHERE FahrschuleID=@FahrschuleID";
            SqlCommand command = new SqlCommand(childQuery, connection);
            command.Parameters.Add("@FahrschuleID", SqlDbType.Int);
            command.Parameters["@FahrschuleID"].Value = Id;

           // int rowIndex = dataGridView2.CurrentRow.Index;
            //object cellvalue = dataGridView2.Rows[rowIndex].Cells[5].Value;
            //string ortIdString = cellvalue.ToString();

            //DataGridViewRow row = dataGridView2.Rows[rowIndex];

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                DataTable childTable = new DataTable();
                childTable.Load(reader);
                dataGridView2.DataSource = childTable;
                /*textBox1.Clear();
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Clear();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Clear();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Clear();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Clear();
                textBox5.Text = row.Cells[4].Value.ToString();
                textBox6.Clear();
                textBox6.Text = row.Cells[5].Value.ToString();
                textBox7.Clear();
                textBox7.Text = row.Cells[6].Value.ToString();*/

                reader.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                connection.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //get the row number on the DataGrid
            int iRownr = this.dataGridView1.CurrentCell.RowIndex;

            //get the content of the cell with the Id
            object cellvalue1 = this.dataGridView1[0, iRownr].Value;
            string IdString = cellvalue1.ToString();
            int Id = Int32.Parse(IdString);


            //create connection
            SqlConnection connection = new SqlConnection(connectionString);

            //create the command and parameter objects
            string childQuery = "SELECT * FROM Ausbilders WHERE FahrschuleID=@FahrschuleID";
            SqlCommand command = new SqlCommand(childQuery, connection);
            command.Parameters.Add("@FahrschuleID", SqlDbType.Int);
            command.Parameters["@FahrschuleID"].Value = Id;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                DataTable childTable = new DataTable();
                childTable.Load(reader);
                dataGridView2.DataSource = childTable;
               
               
                tbNameBS.DataSource = childTable; 
                textBox1.DataBindings.Clear();
                textBox1.DataBindings.Add(new Binding("Text", tbNameBS, "AusbildersID"));
                textBox2.DataBindings.Add(new Binding("Text", tbNameBS, "Name"));
                textBox2.DataBindings.Clear();
                textBox3.DataBindings.Add(new Binding("Text", tbNameBS, "Vorname"));
                textBox3.DataBindings.Clear();
                textBox4.DataBindings.Add(new Binding("Text", tbNameBS, "Geburtstag"));
                textBox4.DataBindings.Clear();
                textBox5.DataBindings.Add(new Binding("Text", tbNameBS, "FahrschuleID"));
                textBox5.DataBindings.Clear();
                textBox6.DataBindings.Add(new Binding("Text", tbNameBS, "Ort"));
                textBox6.DataBindings.Clear();
                textBox7.DataBindings.Add(new Binding("Text", tbNameBS, "Lohn"));
                textBox7.DataBindings.Clear();
                records();

                reader.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                connection.Close();
            }

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            getData();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            DialogResult dr;

            dr = MessageBox.Show("Are you sure? \n There is no undo once deleted.", "Confirm Deletion", MessageBoxButtons.YesNo);

            //get the row number on the DataGrid
            int iRownr = this.dataGridView2.CurrentCell.RowIndex;

            //get the content of the cell
            object cellvalue0 = this.dataGridView2[0, iRownr].Value;

            //create the connection
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();

            //create the command and parameter object
            string deletequery = "DELETE FROM Ausbilders WHERE AusbildersID = @AusbildersID";
            adapter.DeleteCommand = new SqlCommand(deletequery, connection);
            adapter.DeleteCommand.Parameters.Add("@AusbildersID", SqlDbType.Int).Value = btnDelete.Text;
            adapter.DeleteCommand.Parameters["@AusbildersID"].Value = cellvalue0.ToString();

            if (dr == DialogResult.Yes)
            {
                connection.Open();
                adapter.DeleteCommand.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                MessageBox.Show("Deletion canceled!");
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            //create the connection
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();


            //create the command and parameter objects
            sqlDataAdapter.InsertCommand = new SqlCommand("INSERT INTO Ausbilders VALUES(@AusbildersID, @Name, @Vorname, @Geburtstag, @FahrschuleID, @Ort, @Lohn)", connection);
            sqlDataAdapter.InsertCommand.Parameters.Add("@AusbildersID", SqlDbType.Int).Value = textBox1.Text;
            sqlDataAdapter.InsertCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = textBox2.Text;
            sqlDataAdapter.InsertCommand.Parameters.Add("@Vorname", SqlDbType.VarChar).Value = textBox3.Text;
            sqlDataAdapter.InsertCommand.Parameters.Add("@Geburtstag", SqlDbType.Date).Value = textBox4.Text;
            sqlDataAdapter.InsertCommand.Parameters.Add("@FahrschuleID", SqlDbType.Int).Value = textBox5.Text;
            sqlDataAdapter.InsertCommand.Parameters.Add("@Ort", SqlDbType.VarChar).Value = textBox6.Text;
            sqlDataAdapter.InsertCommand.Parameters.Add("@Lohn", SqlDbType.Int).Value = textBox7.Text;


            try
            {
                connection.Open();
                sqlDataAdapter.InsertCommand.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //get the row number on the DataGrid
            int iRownr = this.dataGridView2.CurrentCell.RowIndex;

            //get the content of the cell
            object cellvalue0 = this.dataGridView2[0, iRownr].Value;

            //create the connection
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();

            //create the command and parameter object
            string updatequery = "UPDATE Ausbilders SET Name = @Name, Ort = @Ort, Lohn = @Lohn WHERE AusbildersID = @AusbildersID";
            adapter.UpdateCommand = new SqlCommand(updatequery, connection);
            adapter.UpdateCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = textBox2.Text;
            adapter.UpdateCommand.Parameters.Add("@Ort", SqlDbType.VarChar).Value = textBox6.Text;
            adapter.UpdateCommand.Parameters.Add("@Lohn", SqlDbType.Int).Value = textBox7.Text;
            adapter.UpdateCommand.Parameters.Add("@AusbildersID", SqlDbType.Int).Value = btnUpdate.Text;
            adapter.UpdateCommand.Parameters["@AusbildersID"].Value = cellvalue0.ToString();

            try
            {
                connection.Open();
                adapter.UpdateCommand.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("Only numbers can be entered.");
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, "[^0-9]"))
            {
                MessageBox.Show("Only numbers can be entered.");
                textBox5.Text = textBox5.Text.Remove(textBox5.Text.Length - 1);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!textBox2.Text.All(chr => char.IsLetter(chr)))
            {
                MessageBox.Show("This textbox accepts only alphabetical characters");
                textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!textBox3.Text.All(chr => char.IsLetter(chr)))
            {
                MessageBox.Show("This textbox accepts only alphabetical characters");
                textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (!textBox6.Text.All(chr => char.IsLetter(chr)))
            {
                MessageBox.Show("This textbox accepts only alphabetical characters");
                textBox6.Text = textBox6.Text.Remove(textBox6.Text.Length - 1);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox7.Text, "[^0-9]"))
            {
                MessageBox.Show("Only numbers can be entered.");
                textBox7.Text = textBox7.Text.Remove(textBox7.Text.Length - 1);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

            //MessageBox.Show("The date you want to enter should have the following format: dd/mm//yyyy");
        }

        private void click(object sender, EventArgs e)
        {
            MessageBox.Show("The date you want to enter should have the following format: dd/mm//yyyy");
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            tbNameBS.MoveFirst();
            dgUpdate();
            records();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            tbNameBS.MovePrevious();
            dgUpdate();
            records();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tbNameBS.MoveNext();
            dgUpdate();
            records();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            tbNameBS.MoveLast();
            dgUpdate();
            records();
        }

        private void dgUpdate()
        {
            dataGridView2.ClearSelection();
            dataGridView2.Rows[tbNameBS.Position].Selected = true;
            records();
        }
        private void records()
        {
            label8.Text = "Record " + tbNameBS.Position + " of " + (tbNameBS.Count - 1);
        }
    }
}
