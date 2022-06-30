using System.ComponentModel.DataAnnotations;

namespace NewsAppNet.Models.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public List<ReplyDTO> Replies { get; set; } 
    }

    public class ReplyDTO
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

    public class CommentAddDTO
    {
        [Required]
        public int NewsId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool TopLevelComment { get; set; } = true;
    }

    public class ReplyAddDTO
    {
        [Required]
        public int NewsId { get; set; }
        [Required]
        public int ParentId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool TopLevelComment { get; set; } = false;
    }

    public class CommentEditDTO
    {
        [Required]
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class CommentDeleteDTO
    {
        public int Id { get; set; }
    }
}
