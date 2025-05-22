namespace dkt_learn_core.shared.Models.Models;

public class Reply
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Conteudo { get; set; }
    public DateTime CriadoEm { get; set; } 

    public Guid PostId { get; set; }
    public Post Post { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

}
