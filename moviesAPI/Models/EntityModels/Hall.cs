using moviesAPI.CustomAttributes;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels;

public partial class Hall
{
    [DisplayName("Id")]
    public int Id { get; set; }

    [DisplayName("Назва")]
    public string Name { get; set; } = null!;

    [DisplayName("Місткість")]
    public int Capacity { get; set; }

    [DisplayName("Доступний")]
    public bool IsAvailable { get; set; }

    [JsonIgnore]
    [TableIgnore]
    public virtual ICollection<Session>? Sessions { get; set; } = new List<Session>();

}
