using PhotoAlbumServer.Clients.Interfaces;
using PhotoAlbumServer.Data.Interfaces;
using PhotoAlbumServer.External.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PhotoAlbumServer.Data.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly ITypicodeClient _client;

        public PhotoRepository(ITypicodeClient client)
        {
            _client = client;
        }

        public IReadOnlyCollection<PhotoDto> GetAll(Expression<Func<PhotoDto, bool>> predicate = null)
        {
            var photos = _client.GetPhotos().Result;
            return (predicate == null)
                ? photos.ToList()
                : photos.AsQueryable().Where(predicate).ToList();
        }
    }
}
