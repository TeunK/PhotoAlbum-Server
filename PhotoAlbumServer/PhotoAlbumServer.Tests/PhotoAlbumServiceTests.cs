using NSubstitute;
using NUnit.Framework;
using PhotoAlbumServer.Data.Interfaces;
using PhotoAlbumServer.Domain;
using PhotoAlbumServer.External.DTO;
using PhotoAlbumServer.Interfaces.Factories;
using PhotoAlbumServer.Interfaces.Services;
using PhotoAlbumServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Tests
{
    public class PhotoAlbumServiceTests
    {
        private IPhotoAlbumService _service;
        private IPhotoRepository _mockPhotoRepository;
        private IAlbumRepository _mockAlbumRepository;
        private IPhotoAlbumExpressionFactory _expressionFactory;

        [SetUp]
        public void Setup()
        {
            _mockPhotoRepository = Substitute.For<IPhotoRepository>();
            _mockAlbumRepository = Substitute.For<IAlbumRepository>();
            _expressionFactory = Substitute.For<IPhotoAlbumExpressionFactory>();
            _service = new PhotoAlbumService(_mockPhotoRepository, _mockAlbumRepository, _expressionFactory);
        }

        [Test]
        public void GetAll_NoAlbumsOrPhotos_ReturnsEmptyList()
        {
            _mockPhotoRepository.GetAll().Returns(new List<PhotoDto>());
            _mockAlbumRepository.GetAll().Returns(new List<AlbumDto>());
            var result = _service.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockPhotoRepository.Received(1).GetAll();
            _mockAlbumRepository.Received(1).GetAll();
        }

        [Test]
        public void GetAll_AlbumAndPhoto_ResultContainsCompleteDetails()
        {
            var albumId = 5;
            var photoList = new List<PhotoDto> {
                new PhotoDto {
                    Id = 12,
                    AlbumId = albumId,
                    ThumbnailUrl = new Uri("http://www.exampleimage.com/image_thumbnail.jpg"),
                    Url = new Uri("http://www.exampleimage.com/image.jpg"),
                    Title = "Image Title"
                }
            };
            _mockPhotoRepository.GetAll().Returns(photoList);

            var albumList = new List<AlbumDto> {
                new AlbumDto {
                    Id = albumId,
                    Title = "Album Title",
                    UserId = 20
                }
            };
            _mockAlbumRepository.GetAll().Returns(albumList);

            var result = _service.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(albumId, result.First().Album.Id);
            Assert.AreEqual(1, result.First().Photos.Count());

            //Full Album Details
            Assert.AreEqual(albumList.First().Id, result.First().Album.Id);
            Assert.AreEqual(albumList.First().Title, result.First().Album.Title);
            Assert.AreEqual(albumList.First().UserId, result.First().Album.UserId);

            //Full Photo details
            Assert.AreEqual(photoList.First().Id, result.First().Photos.First().Id);
            Assert.AreEqual(photoList.First().AlbumId, result.First().Photos.First().AlbumId);
            Assert.AreEqual(photoList.First().ThumbnailUrl.ToString(), result.First().Photos.First().ThumbnailUrl.ToString());
            Assert.AreEqual(photoList.First().Url.ToString(), result.First().Photos.First().Url.ToString());
            Assert.AreEqual(photoList.First().Title, result.First().Photos.First().Title);

            _mockPhotoRepository.Received(1).GetAll();
            _mockAlbumRepository.Received(1).GetAll();
        }

        [Test]
        public void GetAll_AlbumWithoutPhotos_ReturnsAlbumWithoutPhotos()
        {
            var albumId = 5;
            _mockPhotoRepository.GetAll().Returns(new List<PhotoDto>());
            _mockAlbumRepository.GetAll().Returns(new List<AlbumDto> { new AlbumDto { Id = albumId } });
            var result = _service.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(albumId, result.First().Album.Id);
            Assert.AreEqual(0, result.First().Photos.Count());
            _mockPhotoRepository.Received(1).GetAll();
            _mockAlbumRepository.Received(1).GetAll();
        }

        [Test]
        public void GetAll_AlbumWithPhotos_ReturnsAlbumWithPhotos()
        {
            var photoId = 12;
            var albumId = 5;
            _mockPhotoRepository.GetAll().Returns(new List<PhotoDto> { new PhotoDto { Id = photoId, AlbumId = albumId} });
            _mockAlbumRepository.GetAll().Returns(new List<AlbumDto> { new AlbumDto { Id = albumId } });
            var result = _service.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(albumId, result.First().Album.Id);
            Assert.AreEqual(1, result.First().Photos.Count());
            Assert.AreEqual(photoId, result.First().Photos.First().Id);
            Assert.AreEqual(albumId, result.First().Photos.First().AlbumId);
            _mockPhotoRepository.Received(1).GetAll();
            _mockAlbumRepository.Received(1).GetAll();
        }

        [Test]
        public void GetAll_PhotosNotPartOfAnyAlbum_ReturnsAlbumWithoutPhotos()
        {
            var photoId = 12;
            var albumId = 5;
            _mockPhotoRepository.GetAll().Returns(new List<PhotoDto> { new PhotoDto { Id = photoId, AlbumId = albumId+1} });
            _mockAlbumRepository.GetAll().Returns(new List<AlbumDto> { new AlbumDto { Id = albumId } });
            var result = _service.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(albumId, result.First().Album.Id);
            Assert.AreEqual(0, result.First().Photos.Count());
            _mockPhotoRepository.Received(1).GetAll();
            _mockAlbumRepository.Received(1).GetAll();
        }

        [Test]
        public void GetAll_ManyPhotosAndAlbums_ReturnsAlbumWithoutPhotos()
        {
            var album1 = new AlbumDto { Id = 1 };
            var photosForAlbum1 = new List<PhotoDto>
            {
                new PhotoDto { Id=1, AlbumId = album1.Id },
                new PhotoDto { Id=2, AlbumId = album1.Id },
                new PhotoDto { Id=3, AlbumId = album1.Id },
            };

            var album2 = new AlbumDto { Id = 7 };
            var photosForAlbum2 = new List<PhotoDto>
            {
                new PhotoDto { Id=4, AlbumId = album2.Id },
                new PhotoDto { Id=5, AlbumId = album2.Id },
            };

            _mockPhotoRepository.GetAll().Returns(photosForAlbum1.Concat(photosForAlbum2).ToList());
            _mockAlbumRepository.GetAll().Returns(new List<AlbumDto> { album1, album2 });
            var result = _service.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(3, result.First(x => x.Album.Id == album1.Id).Photos.Count());
            Assert.AreEqual(2, result.First(x => x.Album.Id == album2.Id).Photos.Count());
            CollectionAssert.AreEquivalent(photosForAlbum1.Select(photo => photo.Id), result.First(x => x.Album.Id == album1.Id).Photos.Select(photo => photo.Id));
            CollectionAssert.AreEquivalent(photosForAlbum2.Select(photo => photo.Id), result.First(x => x.Album.Id == album2.Id).Photos.Select(photo => photo.Id));
            _mockPhotoRepository.Received(1).GetAll();
            _mockAlbumRepository.Received(1).GetAll();
        }

        [Test]
        public void GetByUserId_NoUserData_ReturnsEmptyList()
        {
            _mockPhotoRepository.GetAll().Returns(new List<PhotoDto>());
            _mockAlbumRepository.GetAll().Returns(new List<AlbumDto>());
            var result = _service.GetByUserId(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockPhotoRepository.Received(1).GetAll(Arg.Any<Expression<Func<PhotoDto, bool>>>());
            _mockAlbumRepository.Received(1).GetAll(Arg.Any<Expression<Func<AlbumDto, bool>>>());
        }

        [Test]
        public void GetByUserId_WithUserData_ReturnsAlbumsWithPhotos()
        {
            var photoId = 12;
            var albumId = 5;
            _mockPhotoRepository.GetAll(Arg.Any<Expression<Func<PhotoDto, bool>>>()).Returns(new List<PhotoDto> { new PhotoDto { Id = photoId, AlbumId = albumId } });
            _mockAlbumRepository.GetAll(Arg.Any<Expression<Func<AlbumDto, bool>>>()).Returns(new List<AlbumDto> { new AlbumDto { Id = albumId } });
            var result = _service.GetByUserId(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(albumId, result.First().Album.Id);
            Assert.AreEqual(1, result.First().Photos.Count());
            Assert.AreEqual(photoId, result.First().Photos.First().Id);
            Assert.AreEqual(albumId, result.First().Photos.First().AlbumId);
            _mockPhotoRepository.Received(1).GetAll(Arg.Any<Expression<Func<PhotoDto, bool>>>());
            _mockAlbumRepository.Received(1).GetAll(Arg.Any<Expression<Func<AlbumDto, bool>>>());
        }

        [Test]
        public void MergePhotosWithAlbums_EmptyPhotosAndAlbumsList_ReturnsEmptyList()
        {
            var photosList = new List<Photo>();
            var albumsList = new List<Album>();
            var result = _service.MergePhotosWithAlbums(photosList, albumsList);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void MergePhotosWithAlbums_PhotosWithoutAlbums_ReturnsEmptyList()
        {
            var photosList = new List<Photo> { new Photo { Id = 1, AlbumId = 1 }, new Photo { Id = 2, AlbumId = 2} };
            var albumsList = new List<Album>();
            var result = _service.MergePhotosWithAlbums(photosList, albumsList);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void MergePhotosWithAlbums_AlbumsWithoutPhotos_ReturnsEmptyAlbums()
        {
            var photosList = new List<Photo>();
            var albumsList = new List<Album> { new Album { Id = 1}, new Album { Id = 2 } };
            var result = _service.MergePhotosWithAlbums(photosList, albumsList);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(0, result.First(photoAlbum => photoAlbum.Album.Id == 1).Photos.Count());
            Assert.AreEqual(0, result.First(photoAlbum => photoAlbum.Album.Id == 2).Photos.Count());
            CollectionAssert.AreEquivalent(new List<int> { 1, 2 }, result.Select(photoAlbum => photoAlbum.Album.Id));
        }

        [Test]
        public void MergePhotosWithAlbums_PhotosIntersectWithAlbums_ReturnsEmptyAlbums()
        {
            var photosList = new List<Photo> {
                new Photo { Id = 1, AlbumId = 1},
                new Photo { Id = 2, AlbumId = 1},
                new Photo { Id = 3, AlbumId = 1},
                new Photo { Id = 4, AlbumId = 2},
                new Photo { Id = 5, AlbumId = 2},
            };
            var albumsList = new List<Album> { new Album { Id = 1}, new Album { Id = 2 } };
            var result = _service.MergePhotosWithAlbums(photosList, albumsList);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(3, result.First(photoAlbum => photoAlbum.Album.Id == 1).Photos.Count());
            Assert.AreEqual(2, result.First(photoAlbum => photoAlbum.Album.Id == 2).Photos.Count());
            CollectionAssert.AreEquivalent(new List<int> { 1, 2 }, result.Select(photoAlbum => photoAlbum.Album.Id));
            CollectionAssert.AreEquivalent(new List<int> { 1, 2, 3 }, result.First(photoAlbum => photoAlbum.Album.Id == 1).Photos.Select(photo => photo.Id));
            CollectionAssert.AreEquivalent(new List<int> { 4, 5 }, result.First(photoAlbum => photoAlbum.Album.Id == 2).Photos.Select(photo => photo.Id));
        }
    }
}