namespace Blogg.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; } // Sätts automatiskt i controller
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}