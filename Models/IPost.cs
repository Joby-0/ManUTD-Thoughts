namespace Models;

public interface IPost
{
    int Id { get; set; }
    string Title { get; set; }
    string Content { get; set; }
    DateTime CreatedAt { get; set; }

    IUser Author { get; set; }
}