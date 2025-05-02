namespace dkt_learn_core.shared.Models.Models;

public class Content
{
    public int Id { get; set; }
    public string Tipo { get; set; } // "video" ou "pdf"
    public string Url { get; set; }
    public int ModuleId { get; set; }
    public Module Module { get; set; }
    
    public Content(){}
    
    public Content(int id, string tipo, string url, int moduleId, Module module)
    {
        Id = id;
        Tipo = tipo;
        Url = url;
        ModuleId = moduleId;
        Module = module;
    }
}