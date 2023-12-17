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

namespace Club_EventDriven
{
    public partial class FrmUpdateMember : Form
    {
        public FrmUpdateMember(FrmClubRegistration reg)
        {
            InitializeComponent();
            this.register = reg;
        }
        string connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dapit\source\repos\Club_EventDriven\ClubDB.mdf;Integrated Security=True";

        FrmClubRegistration register;

        private static long Stud_id;//this is global access for student id, it will be assign later
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void FrmUpdateMember_Load(object sender, EventArgs e)
        {
            showList(); //shows the list of the student id
        }
        private void btnConfirm_Click(object sender, EventArgs e) //It updates the database after confirming the changes
        {
            try {

                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();

                    string query1 = "Update ClubMembers set FirstName = @fName, MiddleName = @middlName, " +
                                "LastName = @LName, Age = @age, Gender = @gnder, Program = @program where StudentId = @stud_id";

                    using (SqlCommand cmd = new SqlCommand(query1, connect))
                    {

                        cmd.Parameters.AddWithValue("@fName", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@middlName", txtMiddleName.Text);
                        cmd.Parameters.AddWithValue("@LName", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@age", txtAge.Text);
                        cmd.Parameters.AddWithValue("@gnder", cmbGender.Text);
                        cmd.Parameters.AddWithValue("@program", cmbProgram.Text);
                        cmd.Parameters.AddWithValue("@stud_id", Stud_id);

                        cmd.ExecuteNonQuery();
                    }

                    Console.WriteLine("Update Success");

                }

                register.btnRefresh_Click(sender, e);//It activates the button refresh in registration club

            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            
            }
        }


        private void showList() {//shows the list of the StudentId
            try {

                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();

                    string query = "Select StudentId from ClubMembers";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                cmbStudentID.Items.Add(reader["StudentId"]);

                            }
                        }
                    }
                }

            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            
            }
        }

        private void cmbStudentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbStudentID.Text))
            {

                return;

            }
            else {

                try {

                    using (SqlConnection connect = new SqlConnection(connection))
                    {

                        connect.Open();

                        string query = "Select * from ClubMembers where StudentId = @stud_id";

                        using (SqlCommand cmd = new SqlCommand(query, connect))
                        {
                            cmd.Parameters.AddWithValue("@stud_id", Stud_id);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {

                                while (reader.Read())
                                {

                                    txtFirstName.Text = reader["FirstName"].ToString();
                                    txtMiddleName.Text = reader["MiddleName"].ToString();
                                    txtLastName.Text = reader["LastName"].ToString();
                                    txtAge.Text = reader["Age"].ToString();
                                    cmbGender.Text = reader["Gender"].ToString();
                                    cmbProgram.Text = reader["Program"].ToString();

                                }
                            }
                        }
                    }


                }
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                
                }
                Stud_id = Convert.ToInt64(cmbStudentID.SelectedItem.ToString());//this takes the selected item in combo box
            }
        }
    }
}
