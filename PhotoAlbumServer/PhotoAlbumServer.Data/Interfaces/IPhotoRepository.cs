using PhotoAlbumServer.External.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PhotoAlbumServer.Data.Interfaces
{
    public interface IPhotoRepository
    {
        IReadOnlyCollection<PhotoDto> GetAll(Expression<Func<PhotoDto, bool>> predicate = null);
    }
}
