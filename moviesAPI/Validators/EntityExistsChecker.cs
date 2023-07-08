using moviesAPI.Repositories;
using NuGet.Protocol.Core.Types;

namespace moviesAPI.Validators
{
    public class EntityExistsChecker
    {
        private readonly CinemaRepository repository;
        public EntityExistsChecker(CinemaRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> GenreExists(int id)
        {
            var entity = await repository.GetGenreById(id);
            if (entity == null) return false;
            return true;
        }
        public async Task<bool> MovieExists(Guid id)
        {
            var entity = await repository.GetMovieById(id);
            if (entity == null) return false;
            return true;
        }
        public async Task<bool> HallExists(int id)
        {
            var entity = await repository.GetHallById(id);
            if (entity == null) return false;
            return true;
        }
        public async Task<bool> SessionExists(Guid id)
        {
            var entity = await repository.GetSessionById(id);
            if (entity == null) return false;
            return true;
        }
        public async Task<bool> TicketExists(Guid id)
        {
            var entity = await repository.GetTicketById(id);
            if (entity == null) return false;
            return true;
        }
        public async Task<bool> ClientExists(Guid id)
        {
            return false;
        }
    }
}
