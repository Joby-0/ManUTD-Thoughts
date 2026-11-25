namespace Models;

public class Post : IPost
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual IUser Author { get; set; }
}