using PhotoAlbumServer.Domain;
using PhotoAlbumServer.External.DTO;
using PhotoAlbumServer.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PhotoAlbumServer.Factories
{
    public class PhotoAlbumExpressionFactory : IPhotoAlbumExpressionFactory
    {
        public Expression<Func<AlbumDto, bool>> AlbumsByUserId(int userId) => album => album.UserId == userId;
        public Expression<Func<PhotoDto, bool>> PhotosInAlbums(IEnumerable<Album> albumsByUser) => photo => albumsByUser.Select(album => album.Id).Contains(photo.AlbumId);
    }
}
