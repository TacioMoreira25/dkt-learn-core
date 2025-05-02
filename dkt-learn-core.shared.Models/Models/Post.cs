namespace dkt_learn_core.shared.Models.Models;

public class Post
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Texto { get; set; }
    public DateTime CriadoEm { get; set; }
    

    public ICollection<Reply> Respostas { get; set; }
    public ICollection<Like> Curtidas { get; set; }
}
