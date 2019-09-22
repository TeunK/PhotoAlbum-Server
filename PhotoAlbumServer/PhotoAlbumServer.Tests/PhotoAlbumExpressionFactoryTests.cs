using NUnit.Framework;
using PhotoAlbumServer.Domain;
using PhotoAlbumServer.External.DTO;
using PhotoAlbumServer.Factories;
using PhotoAlbumServer.Interfaces.Factories;
using System.Collections.Generic;
using System.Linq;

namespace PhotoAlbumServer.Tests
{
    public class PhotoAlbumExpressionFactoryTests
    {
        private IPhotoAlbumExpressionFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new PhotoAlbumExpressionFactory();
        }

        [Test]
        public void AlbumsByUserId_NoAlbumsContainUserId_ReturnsEmptyList()
        {
            var userId = 14;
            var queryExpression = _factory.AlbumsByUserId(userId);

            var albums = new List<AlbumDto> {
                new AlbumDto {UserId = userId+1}
            };

            List<AlbumDto> result = albums.AsQueryable().Where(queryExpression).ToList();
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void AlbumsByUserId_OneAlbumContainUserId_ReturnsEmptyList()
        {
            var userId = 14;
            var queryExpression = _factory.AlbumsByUserId(userId);

            var albums = new List<AlbumDto> {
                new AlbumDto {Id = 1, UserId = userId-1},
                new AlbumDto {Id = 2, UserId = userId},
                new AlbumDto {Id = 3, UserId = userId+1},
            };

            List<AlbumDto> result = albums.AsQueryable().Where(queryExpression).ToList();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(2, result.First().Id);
        }

        [Test]
        public void AlbumsByUserId_AllAlbumsContainUserId_ReturnsEmptyList()
        {
            var userId = 14;
            var queryExpression = _factory.AlbumsByUserId(userId);

            var albums = new List<AlbumDto> {
                new AlbumDto {Id = 1, UserId = userId},
                new AlbumDto {Id = 2, UserId = userId},
                new AlbumDto {Id = 3, UserId = userId},
            };

            List<AlbumDto> result = albums.AsQueryable().Where(queryExpression).ToList();
            Assert.AreEqual(3, result.Count());
            CollectionAssert.AreEquivalent(albums.Select(a => a.Id), result.Select(a => a.Id));
        }

        [Test]
        public void PhotosInAlbums_NoPhotosInAlbums_ReturnsEmptyList()
        {
            var albums = new List<Album> {
                new Album {Id = 1},
                new Album {Id = 2},
                new Album {Id = 3},
            };
            var photos = new List<PhotoDto>
            {
                new PhotoDto { Id = 1, AlbumId = 4 },
                new PhotoDto { Id = 2, AlbumId = 5 },
                new PhotoDto { Id = 3, AlbumId = 6 },
            };
            var queryExpression = _factory.PhotosInAlbums(albums);

            List<PhotoDto> result = photos.AsQueryable().Where(queryExpression).ToList();
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void PhotosInAlbums_OnePhotoInAlbums_ReturnsEmptyList()
        {
            var albums = new List<Album> {
                new Album {Id = 1},
                new Album {Id = 2},
                new Album {Id = 3},
            };
            var photos = new List<PhotoDto>
            {
                new PhotoDto { Id = 5, AlbumId = 1 },
                new PhotoDto { Id = 2, AlbumId = 5 },
                new PhotoDto { Id = 3, AlbumId = 6 },
            };
            var queryExpression = _factory.PhotosInAlbums(albums);

            List<PhotoDto> result = photos.AsQueryable().Where(queryExpression).ToList();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(5, result.First().Id);
        }

        [Test]
        public void PhotosInAlbums_ManyPhotosInAlbums_ReturnsEmptyList()
        {
            var albums = new List<Album> {
                new Album {Id = 1},
                new Album {Id = 2},
                new Album {Id = 3},
            };
            var photos = new List<PhotoDto>
            {
                new PhotoDto { Id = 5, AlbumId = 1 },
                new PhotoDto { Id = 2, AlbumId = 5 },
                new PhotoDto { Id = 3, AlbumId = 3 },
            };
            var queryExpression = _factory.PhotosInAlbums(albums);

            List<PhotoDto> result = photos.AsQueryable().Where(queryExpression).ToList();
            Assert.AreEqual(2, result.Count());
            CollectionAssert.AreEquivalent(new List<int> { 5, 3}, result.Select(a => a.Id));
        }
    }
}
