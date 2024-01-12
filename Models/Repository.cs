namespace practice.Models;

using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public class Repository : IRepository
{
    private readonly IConfiguration _configuration;

    public string? connectionString;  

    public Repository(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    // Adding New Customer

    public string insertingUser(NewUser newUser)
    {
        if (newUser.Name == null || newUser.password == null || newUser.reenteredpassword == null || newUser.floor == null)
            return "Fail";  
        string message = "";
        SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();
            string name = "^[a-zA-z0-9]+$";
            Regex Name = new Regex(name);
            string password = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
            Regex pass = new Regex(password);
            if (Name.IsMatch(newUser.Name) && pass.IsMatch(newUser.password))
            {
                if (newUser.password == newUser.reenteredpassword)
                {
                    string CHECK = "SELECT COUNT (*) FROM [Users] where Username='" + newUser.Name + "';";
                    using (SqlCommand check = new SqlCommand(CHECK, connection))
                    {
                        // Console.WriteLine("hello");
                        int count = (int)check.ExecuteScalar();
                        if (count == 0)
                        {
                            string INSERT = "INSERT INTO [Users] (Username, Password, Floor) VALUES (@name,@pass,@floor)";
                            using (SqlCommand insert = new SqlCommand(INSERT, connection))
                            {
                                insert.Parameters.Add("@name", System.Data.SqlDbType.VarChar, 50, "Username").Value = newUser.Name;
                                insert.Parameters.Add("@pass", System.Data.SqlDbType.VarChar, 50, "Password").Value = newUser.password;
                                insert.Parameters.Add("@floor", System.Data.SqlDbType.VarChar, 50, "Floor").Value = newUser.floor;
                                insert.ExecuteNonQuery();
                                message = "Done";
                            }
                        }
                        else
                        {
                            message = "ExistingUser";
                        }
                    }
                }
                else
                {
                    message = "Password doesnot match";
                }
            }
            else
            {
                message = "Fail";
            }
        }
        catch (SqlException exception)
        {
            Console.WriteLine("SQL Exception Received " + exception);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Exception Recived: " + exception);
        }
        finally
        {
            connection.Close();
        }
        return message;

    }
    public string checkLogin(User user)
    {
        // string connectionString = "Data Source =ASHIQ\\SQLEXPRESS; Initial Catalog=StoreMS; Integrated Security=SSPI";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string loginmessage = "";
            string VALIDATE = "SELECT count(*) FROM [Users] WHERE Username ='" + user.Name + "' AND Password = '" + user.password + "';";
            using (SqlCommand validate = new SqlCommand(VALIDATE, connection))
            {
                int count1 = Convert.ToInt16(validate.ExecuteScalar());
                if (count1 == 1)
                {
                    Console.WriteLine("------------------"+_configuration["Name"]);
                    if (user.Name == _configuration["Name"].ToString())
                    {
                        loginmessage = "AdminLogin"; //S-Successfully Loggedin as Admin
                    }
                    else
                    {
                        loginmessage = "UserLogin"; //U-Successfully Loggedin as User
                    }
                }
                else
                {
                    loginmessage = "Invalid"; //N-Not a Valid User.
                }
            }
            return loginmessage;
        }
    }

    // To View the Customers credentials

    public DataTable ViewUsers(NewUser newUser)
    {
        // string connectionString = "Server=ASHIQ\\SQLEXPRESS; Initial Catalog=StoreMS; Integrated Security = SSPI";
        DataTable dataTable = new DataTable();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand insert = new SqlCommand("SELECT Username,floor,Password FROM [Users]", connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(insert);
            dataAdapter.Fill(dataTable);
        }
        return dataTable;
    }

    // Deleteing the customer
    public void deleteUser(string myString)
    {
        //Console.Write("--------------------------------------------------------"+myString);
        // string connectionString = "Server=ASHIQ\\SQLEXPRESS; Initial Catalog=StoreMS; Integrated Security = SSPI";
        using(SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand delete = new SqlCommand("DELETE FROM [Users] WHERE Username = '"+ myString + "'",connection);
            delete.ExecuteNonQuery();
        }

    }


    public DataTable getUser(string myString)
    {
        // string connectionString = "Server=ASHIQ\\SQLEXPRESS; Initial Catalog=StoreMS; Integrated Security = SSPI";
         DataTable dataTable = new DataTable();
        using(SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand select = new SqlCommand("SELECT Username,Floor FROM [Users] WHERE Username = '" + myString + "'" ,connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(select);
            dataAdapter.Fill(dataTable);
        }
        return dataTable;
    }

    // Updating the customer

    public void updateUser(NewUser newUser)
    {
        // string connectionString = "Server=ASHIQ\\SQLEXPRESS; Initial Catalog=StoreMS; Integrated Security = SSPI";
        using(SqlConnection connection = new SqlConnection(connectionString))

        {
            connection.Open();
            SqlCommand update = new SqlCommand("UPDATE Users SET Password =  '" + newUser.password + "' WHERE Username = '" + newUser.Name + "'" ,connection);
            if(newUser.password == newUser.reenteredpassword)
            {
            update.ExecuteNonQuery();
            }         
        }

    }

    public void insertingMessage(Message message)
    {
        using(SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand insert = new SqlCommand("INSERT INTO Message (Id,CustomerName,Comment,Date) VALUES(@id,@name,@message,@date)",connection);
            insert.Parameters.Add("@id",System.Data.SqlDbType.VarChar, 50, "Id").Value = message.id;
            insert.Parameters.Add("@name",System.Data.SqlDbType.VarChar, 50, "CustomerName").Value = message.customerName;
            insert.Parameters.Add("@message",System.Data.SqlDbType.VarChar,2000, "Comment").Value = message.message;
            insert.Parameters.Add("@date",System.Data.SqlDbType.VarChar, 50, "Date").Value = message.date;
            insert.ExecuteNonQuery();
        }
    }

    public DataTable viewMessage(Message message)
    {

        DataTable dataTable = new DataTable();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand select = new SqlCommand("SELECT Id,CustomerName,Comment,Date FROM Message", connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(select);
            dataAdapter.Fill(dataTable);
        }
    return dataTable;
    }


    public void deleteMessages(string id)
    {
        using(SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try{
            SqlCommand delete = new SqlCommand("DELETE FROM Message WHERE Id = '"+ id + "'",connection);
            delete.ExecuteNonQuery();
            }
            catch(SqlException exception)
            {
                Console.WriteLine("The Exception is "+exception);
            }
        }
    }
}

