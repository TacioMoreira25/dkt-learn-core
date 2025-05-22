namespace dkt_learn_core.shared.Models.Models;

public class Like
{
    public int Id { get; set; }

    public Guid PostId { get; set; }
    public Post Post { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

}
