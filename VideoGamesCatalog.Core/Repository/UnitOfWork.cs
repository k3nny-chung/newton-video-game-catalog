using VideoGamesCatalog.Core.Data.Context;
using VideoGamesCatalog.Core.Data.Models;

namespace VideoGamesCatalog.Core.Data.Repository
{
    public interface IUnitOfWork
    {
        IRepository<AgeRating> AgeRatingRepository { get; }
        IRepository<Genre> GenreRepository { get; }
        IRepository<Platform> PlatformRepository { get; }
        IRepository<VideoGameImage> VideoGameImageRepository { get; }
        IRepository<VideoGame> VideoGameRepository { get; }
        Task Save();
    }

    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly VideoGameContext _dbContext;
        private IRepository<VideoGame> _videoGameRepository;
        private IRepository<VideoGameImage> _videoGameImageRepository;
        private IRepository<Genre> _genreRepository;
        private IRepository<AgeRating> _ageRatingRepository;
        private IRepository<Platform> _platformRepository;
        private bool _disposed = false;


        public UnitOfWork(VideoGameContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<VideoGame> VideoGameRepository => _videoGameRepository ??= new Repository<VideoGame>(_dbContext);
        public IRepository<VideoGameImage> VideoGameImageRepository => _videoGameImageRepository ??= new Repository<VideoGameImage>(_dbContext);
        public IRepository<Genre> GenreRepository => _genreRepository ??= new Repository<Genre>(_dbContext);
        public IRepository<AgeRating> AgeRatingRepository => _ageRatingRepository ??= new Repository<AgeRating>(_dbContext);
        public IRepository<Platform> PlatformRepository => _platformRepository ??= new Repository<Platform>(_dbContext);

        public async Task Save() => await _dbContext.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
