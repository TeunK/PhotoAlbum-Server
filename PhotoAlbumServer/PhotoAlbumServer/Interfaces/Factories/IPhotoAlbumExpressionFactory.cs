using PhotoAlbumServer.Domain;
using PhotoAlbumServer.External.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PhotoAlbumServer.Interfaces.Factories
{
    public interface IPhotoAlbumExpressionFactory
    {
        Expression<Func<AlbumDto, bool>> AlbumsByUserId(int userId);
        Expression<Func<PhotoDto, bool>> PhotosInAlbums(IEnumerable<Album> albumsByUser);
    }
}
