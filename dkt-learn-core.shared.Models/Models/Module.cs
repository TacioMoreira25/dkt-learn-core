namespace dkt_learn_core.shared.Models.Models;

public class Module
{
    public int Id { get; set; }
    public string Titulo { get; set; } 
    public string Descricao { get; set; }
    public int LearningPathId { get; set; }
    public LearningPath LearningPath { get; set; }
    public ICollection<Content> Contents { get; set; }

    public Module()
    {
        Contents = new List<Content>();
    }

    public Module(int id, string titulo, string descricao, 
        int learningPathId, LearningPath learningPath)
    {
        Id = id;
        Titulo = titulo;
        Descricao = descricao;
        LearningPathId = learningPathId;
        LearningPath = learningPath;
        Contents = new List<Content>(); 
    }
}