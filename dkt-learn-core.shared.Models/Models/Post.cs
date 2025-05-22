namespace dkt_learn_core.shared.Models.Models;

public class Post
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Titulo { get; set; }
    public string Conteudo { get; set; }
    public DateTime CriadoEm { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<Reply> Replies { get; set; }
    public ICollection<Like> Likes { get; set; } 
}
