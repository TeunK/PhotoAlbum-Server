using PhotoAlbumServer.External.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PhotoAlbumServer.Data.Interfaces
{
    public interface IAlbumRepository
    {
        IReadOnlyCollection<AlbumDto> GetAll(Expression<Func<AlbumDto, bool>> predicate = null);
    }
}
