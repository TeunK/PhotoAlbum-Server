using PhotoAlbumServer.Clients.Interfaces;
using PhotoAlbumServer.Data.Interfaces;
using PhotoAlbumServer.External.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PhotoAlbumServer.Data.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ITypicodeClient _client;

        public AlbumRepository(ITypicodeClient client)
        {
            _client = client;
        }

        public IReadOnlyCollection<AlbumDto> GetAll(Expression<Func<AlbumDto, bool>> predicate = null)
        {
            var albums = _client.GetAlbums().Result;
            return (predicate == null)
                ? albums.ToList()
                : albums.AsQueryable().Where(predicate).ToList();
        }
    }
}
