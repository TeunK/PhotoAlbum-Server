using PhotoAlbumServer.Data.Interfaces;
using PhotoAlbumServer.Domain;
using PhotoAlbumServer.External.DTO;
using PhotoAlbumServer.Interfaces.Factories;
using PhotoAlbumServer.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PhotoAlbumServer.Services
{
    public class PhotoAlbumService : IPhotoAlbumService
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IPhotoAlbumExpressionFactory _expressionFactory;

        public PhotoAlbumService(IPhotoRepository photoRepository, IAlbumRepository albumRepository, IPhotoAlbumExpressionFactory expressionFactory)
        {
            _photoRepository = photoRepository;
            _albumRepository = albumRepository;
            _expressionFactory = expressionFactory;
        }

        public IEnumerable<PhotoAlbum> GetAll()
        {
            var photos = _photoRepository.GetAll().ToList().Select(Map);
            var albums = _albumRepository.GetAll().ToList().Select(Map);

            return MergePhotosWithAlbums(photos, albums);
        }

        public IEnumerable<PhotoAlbum> GetByUserId(int userId)
        {
            Expression<Func<AlbumDto, bool>> query = album => album.UserId == userId;

            var userAlbums = _albumRepository.GetAll(_expressionFactory.AlbumsByUserId(userId)).ToList().Select(Map);
            var photosInUserAlbums = _photoRepository.GetAll(_expressionFactory.PhotosInAlbums(userAlbums)).ToList().Select(Map);

            return MergePhotosWithAlbums(photosInUserAlbums, userAlbums);
        }

        public IEnumerable<PhotoAlbum> MergePhotosWithAlbums(IEnumerable<Photo> photos, IEnumerable<Album> albums)
        {
            var photosByAlbumId = photos
                .GroupBy(photo => photo.AlbumId)
                .ToDictionary(p => p.Key, p => p.ToList());

            var photoAlbums = albums.Select(album => new PhotoAlbum()
            {
                Album = album,
                Photos = photosByAlbumId.GetValueOrDefault(album.Id) ?? new List<Photo>()
            });

            return photoAlbums;
        }

        private Album Map(AlbumDto dto)
        {
            return new Album()
            {
                Id = dto.Id,
                Title = dto.Title,
                UserId = dto.UserId
            };
        }

        private Photo Map(PhotoDto dto)
        {
            return new Photo()
            {
                Id = dto.Id,
                AlbumId = dto.AlbumId,
                Title = dto.Title,
                ThumbnailUrl = dto.ThumbnailUrl,
                Url = dto.Url
            };
        }
    }
}
