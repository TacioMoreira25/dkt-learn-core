
namespace dkt_learn_core.shared.Models.Models
{
    public class LearningPath
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconeUrl { get; set; }
        public int TempoEstimadoMinutos { get; set; }
        
        public ICollection<Module> Modules { get; set; }
        
        public LearningPath(){}
        
        public LearningPath(int id, string name, string description, string iconeUrl, 
            int tempoEstimadoMinutos)
        {
            Id = id;
            Name = name;
            Description = description;
            IconeUrl = iconeUrl;
            TempoEstimadoMinutos = tempoEstimadoMinutos;
            Modules = new List<Module>();
        }
    }
}
