namespace dkt_learn_core.shared.Models.Dtos;

public class LearningPathResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconeUrl { get; set; } 
    public int TempoEstimado { get; set; }
}