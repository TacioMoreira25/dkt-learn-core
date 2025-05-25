
namespace dkt_learn_core.shared.Models.Dtos;

public class PostDto
{ 
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Titulo { get; set; }
    public string Conteudo { get; set; }
    public DateTime CriadoEm { get; set; }
    public List<string> Likes { get; set; }
    public List<ReplyDto> Replies { get; set; }
}