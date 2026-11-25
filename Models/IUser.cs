namespace Models;

public interface IUser
{
    int Id { get; set; }
    string Name { get; set; }
    string PasswordHash { get; set; }
    string Email { get; set; }

    bool CanWrite { get; set; }

    IUser Author { get; set; }
}