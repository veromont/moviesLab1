using moviesAPI.CustomAttributes;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels;

public partial class Session
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [DisplayName("Id фільму")]
    public Guid MovieId { get; set; }

    [DisplayName("Id залу")]
    public int HallId { get; set; }

    [DisplayName("Початок сеансу")]
    public DateTime StartTime { get; set; }

    [DisplayName("Кінець сеансу")]
    public DateTime EndTime { get; set; }

    [DisplayName("Ціна квитка на сеанс")]
    public double Price { get; set; }

    [JsonIgnore]
    [TableIgnore]
    public virtual Hall? Hall { get; set; }

    [JsonIgnore]
    [TableIgnore]
    public virtual Movie? Movie { get; set; }

    [JsonIgnore]
    [TableIgnore]
    public virtual ICollection<Ticket>? SessionTickets { get; set; } = new List<Ticket>();

}
