using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Club_EventDriven
{
    public partial class FrmClubRegistration : Form
    {
        //connection string from the service database
        string connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dapit\source\repos\Club_EventDriven\ClubDB.mdf;Integrated Security=True";

        public event EventHandler eventRefresh;
        public FrmClubRegistration()
        {
            InitializeComponent();
            clubRegistrationQuery = new ClubRegistrationQuery(connect);

        }

        //Step 9 FrmClubRegistration

        private ClubRegistrationQuery clubRegistrationQuery;

        private long StudentId;
        private int ID, Age;
        private string FirstName, MiddleName, LastName, Gender, Program;

        private void btnClear_Click(object sender, EventArgs e) //clearing the inputs after registration
        {
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            txtStudentID.Clear();
            txtAge.Clear();
            cmbProgram.Text = "";
            cmbGender.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FrmUpdateMember upm = new FrmUpdateMember(this);
            upm.Show();
        }


        public void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshListOfClubMembers();
            eventRefresh?.Invoke(this, EventArgs.Empty);
            
        }

        //       sqlCommand = new SqlCommand("INSERT INTO ClubMembers VALUES(@ID, @StudentID, @FirstName, @MiddleName, @LastName, @Age, @Gender, @Program)", sqlConnect);


        private void btnRegister_Click(object sender, EventArgs e) //Calls the registration to class
        {
            StudentId = long.Parse(txtStudentID.Text);
            ID = RegistrationId();
            FirstName = txtFirstName.Text;
            MiddleName = txtMiddleName.Text;
            LastName = txtLastName.Text;
            Age = int.Parse(txtAge.Text);
            Gender = cmbGender.Text;
            Program = cmbProgram.Text;

            clubRegistrationQuery.RegisterStudent(

                ID,
                StudentId,
                FirstName,
                MiddleName,
                LastName,
                Age,
                Gender,
                Program
            
                );

            MessageBox.Show("Success");
        }


        //This a private method that counts the id then increment
        private int RegistrationId()//this takes the registration id where it counts the total first before inserting a new
        {
            int total = 0;
            using (SqlConnection connect1 = new SqlConnection(connect)) {
                connect1.Open();

                string query = "Select Count(*) from ClubMembers";

                using (SqlCommand cmd = new SqlCommand(query, connect1)) {

                    total = Convert.ToInt32(cmd.ExecuteScalar());
                
                }
            }
            return total ++;
        }
       

        private void RefreshListOfClubMembers() {

            clubRegistrationQuery.DisplayList();
            dgMembers.DataSource = clubRegistrationQuery.bindingSource;
            dgMembers.Refresh();
        
        }

        //Double click the to generate the load event
        private void FrmClubRegistration_Load(object sender, EventArgs e)
        {
            RefreshListOfClubMembers();
        }
    }
}
