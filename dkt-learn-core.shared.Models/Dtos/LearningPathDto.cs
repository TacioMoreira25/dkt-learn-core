using System.ComponentModel.DataAnnotations;

namespace dkt_learn_core.shared.Models.Dtos;

public class LearningPathDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [StringLength(300)]
    public string Description { get; set; }
    
    [Url]
    public string IconeUrl { get; set; }
    
    [Range(1, 1000)]
    public int TempoEstimado { get; set; }
}