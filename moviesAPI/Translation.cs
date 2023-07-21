namespace moviesAPI
{
    public class Translation
    {
        public static Dictionary<string, string> IdentityDataTranslationMap = new Dictionary<string, string> {
            { "Id", "ID-unicode" },
            { "Email", "Електронна адреса" },
            { "Name", "Ім'я" },
            { "Surname", "Прізвище" },
        };
        public static Dictionary<string, string> EntitiesDataTranslationMap = new Dictionary<string, string> {
            { "Id", "Код" },
            { "GenreId", "Код жанру" },
            { "MovieId", "Код фільму" },
            { "HallId", "Код залу" },
            { "SessionId", "Код сеансу" },

            { "Name", "Назва" },
            { "HallName", "Зал" },
            { "Title", "Назва" },
            { "MovieTitle", "Назва фільму" },

            { "SeatNumber", "Місце" },
            { "StartTime", "Час початку сеансу" },
            { "EndTime", "Час закінчення сеансу" },
            { "Price", "Ціна квитка" },
            { "Capacity", "Місткість" },
            { "IsAvailable", "Доступність" },
            { "Director", "Режисер" },
            { "ReleaseDate", "Дата виходу" },
            { "Rating", "Оцінка"},
            { "Duration", "Тривалість" },
        };
    }
}
