using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels;

public partial class Hall
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; }

    [JsonIgnore]
    public virtual ICollection<Session>? Sessions { get; set; } = new List<Session>();

}
