using Newtonsoft.Json;

namespace moviesAPI.Models.EntityModels
{
    public partial class Client : Entity
    {
        public string Email { get; set; }
        public Guid Id { get; set; }
		public int RoleId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

		[JsonIgnore]
		public static Dictionary<string, string> TranslationMap { get; } = new Dictionary<string, string>{
			{ nameof(Id), "ID-unicode" },
			{ nameof(Email), "електронна адреса" },
            { nameof(RoleId), "id ролі" },
			{ nameof(Name), "Ім'я" },
			{ nameof(Password), "Пароль" },
		};

        [JsonIgnore]
        public Role Role { get; set; }
    }
}
