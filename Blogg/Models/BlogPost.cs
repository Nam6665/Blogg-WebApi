using Blogg.Models;

public class BlogPost
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<Comment> Comments { get; set; } = new List<Comment>();
}
