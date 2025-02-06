using System.ComponentModel;

public class UserLoginVM
{
    public string UserName {get;set;}
    public string Password {get;set;}

    public string Role {get;set;} = UserRole.User;
}


public class UserRole {
    public static string Admin = "admin";
    public static string User = "user";
}