namespace Models;

public class User : IUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }

    public bool CanWrite { get; set; }
    public IUser Author { get; set; }
}