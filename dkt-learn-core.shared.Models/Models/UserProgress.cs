namespace dkt_learn_core.shared.Models.Models;

public class UserProgress
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public DateTime ConcluidoEm { get; set; }
    
    public UserProgress(){}

    public UserProgress(int id, int userId, int contentId, DateTime concluidoEm)
    {
        Id = id;
        UserId = userId;
        ContentId = contentId;
        ConcluidoEm = concluidoEm;
    }
}