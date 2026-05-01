

public interface IApplicationUserService
{
    Task RegisterUser(string username, string password, string role);

    Task LogoutUser();

    Task LoginUser(string username, string password);


}