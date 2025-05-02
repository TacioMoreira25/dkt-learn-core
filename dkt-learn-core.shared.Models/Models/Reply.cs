namespace dkt_learn_core.shared.Models.Models;

public class Reply
{
    public int Id { get; set; }
    public string Texto { get; set; }
    public DateTime CriadoEm { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; }

    public int UserId { get; set; }

}
