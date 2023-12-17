using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;

namespace Club_EventDriven
{
    internal class ClubRegistrationQuery
    {
        private SqlConnection sqlConnect;

        private SqlCommand sqlCommand;
        private SqlDataAdapter sqlDataAdapter;
        private SqlDataReader sqlReader;

        public DataTable dataTable;
        public BindingSource bindingSource;

        private string connectionString;

        public string _FirstName, _MiddleName, _LastName, _Gender, _Program;
        int _age;

        //Step 6 where initialize connection string, data table and binding source
        public ClubRegistrationQuery(String connection1) {

            sqlConnect = new SqlConnection(connection1);

            

            dataTable = new DataTable();

            bindingSource = new BindingSource();

        }

        public Boolean DisplayList() {

            string ViewClubMembers = "Select StudentID, FirstName, MiddleName, LastName, " +
                "Age, Gender, Program from ClubMembers";

            sqlDataAdapter = new SqlDataAdapter(ViewClubMembers, sqlConnect);

            dataTable.Clear();
            sqlDataAdapter.Fill(dataTable);

            bindingSource.DataSource = dataTable;

            return true;
        }


        public bool RegisterStudent(int ID, long StudentID, string FirstName, string
                                MiddleName, string LastName, int Age, string Gender, string Program){

            sqlConnect.Open();
            sqlCommand = new SqlCommand("INSERT INTO ClubMembers VALUES(@ID, @StudentID, @FirstName, @MiddleName, @LastName, @Age, @Gender, @Program)", sqlConnect);
            sqlCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
            sqlCommand.Parameters.Add("@RegistrationID", SqlDbType.BigInt).Value = StudentID;
            sqlCommand.Parameters.Add("@StudentID", SqlDbType.VarChar).Value = StudentID;
            sqlCommand.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
            sqlCommand.Parameters.Add("@MiddleName", SqlDbType.VarChar).Value = MiddleName;
            sqlCommand.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;
            sqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = Age;
            sqlCommand.Parameters.Add("@Gender", SqlDbType.VarChar).Value = Gender;
            sqlCommand.Parameters.Add("@Program", SqlDbType.VarChar).Value = Program;
            sqlCommand.ExecuteNonQuery();
            sqlConnect.Close();
            return true;

        }

    }
}
