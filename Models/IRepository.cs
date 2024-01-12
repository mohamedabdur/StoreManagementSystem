namespace practice.Models;


using System.Data;

public interface IRepository
{
    public string insertingUser(NewUser newUser);
    public string checkLogin(User user);
    public DataTable ViewUsers(NewUser newUser);
    public void deleteUser(string myString);
    public DataTable getUser(string myString);
    public void updateUser(NewUser newUser);
    public void insertingMessage(Message msg);
    public DataTable viewMessage(Message message);
    public void deleteMessages(string id);
}