using NSubstitute;
using NUnit.Framework;
using PhotoAlbumServer.Clients.Interfaces;
using PhotoAlbumServer.Data.Repositories;
using PhotoAlbumServer.External.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Tests
{
    public class PhotoRepositoryTests
    {
        private PhotoRepository _repository;
        private ITypicodeClient _mockClient;

        [SetUp]
        public void Setup()
        {
            _mockClient = Substitute.For<ITypicodeClient>();
            _repository = new PhotoRepository(_mockClient);
        }


        [Test]
        public void GetAll_NoClientResults_ReturnsEmptyList()
        {
            IReadOnlyCollection<PhotoDto> clientResponsePhotos = new List<PhotoDto>();
            _mockClient.GetPhotos().Returns(Task.FromResult(clientResponsePhotos));

            var result = _repository.GetAll();

            Assert.AreEqual(0, result.Count);
            _mockClient.Received(1).GetPhotos();
        }

        [Test]
        public void GetAll_WithClientResultsWithoutFilter_ReturnsEmptyList()
        {
            IReadOnlyCollection<PhotoDto> clientResponsePhotos = new List<PhotoDto> {
                new PhotoDto {Id=1},
                new PhotoDto {Id=2},
                new PhotoDto {Id=3},
            };
            _mockClient.GetPhotos().Returns(Task.FromResult(clientResponsePhotos));

            var result = _repository.GetAll();

            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEquivalent(new List<int> { 1, 2, 3 }, new List<PhotoDto>(result).Select(photo => photo.Id));
            _mockClient.Received(1).GetPhotos();
        }

        [Test]
        public void GetAll_WithClientResultsWithFilter_ReturnsFilteredList()
        {
            IReadOnlyCollection<PhotoDto> clientResponsePhotos = new List<PhotoDto> {
                new PhotoDto {Id=1},
                new PhotoDto {Id=2},
                new PhotoDto {Id=3},
            };
            _mockClient.GetPhotos().Returns(Task.FromResult(clientResponsePhotos));

            var result = _repository.GetAll(photoDto => photoDto.Id > 2);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result.First().Id);
            _mockClient.Received(1).GetPhotos();
        }
    }
}