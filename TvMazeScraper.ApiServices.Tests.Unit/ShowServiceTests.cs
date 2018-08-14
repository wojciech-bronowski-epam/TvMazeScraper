
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TvMazeScraper.ApiPresentation;
using TvMazeScraper.Common.Helper;
using TvMazeScraper.Data.Model;
using TvMazeScraper.Data.Read;

namespace TvMazeScraper.ApiServices.Tests.Unit
{
    [TestFixture]
    public class ShowServiceTests
    {
        private IShowService _showService;
        private Mock<ITvMazeDataReader> _tvMazeDataReaderMock;
        private Mock<IDataParse> _dataParseMock;

        [SetUp]
        public void SetUp()
        {
            _tvMazeDataReaderMock = new Mock<ITvMazeDataReader>();
            _dataParseMock = new Mock<IDataParse>();
            _showService = new ShowService(_tvMazeDataReaderMock.Object, _dataParseMock.Object);
        }
        [Test]
        public async Task ShouldGetShowsWithActorsOrdered()
        {
            _tvMazeDataReaderMock.Setup(d => d.GetShowsWithActorsAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(GetShows()));
            _dataParseMock.Setup(d => d.DateToString(It.IsAny<DateTime?>())).Returns(ParsedValue());
            var shows = await _showService.GetShowsWithActorsAsync(1, 1);
            CollectionAssert.AreEqual(GetExpectedShows(), shows.ToList());
        }

        private static Func<DateTime?, string> ParsedValue()
        {
            return date =>
            { 
                if (!date.HasValue) return null;
                if (date.Value.Year.ToString() == "1981") return "1981-07-01";
                if (date.Value.Year.ToString() == "1980") return "1980-07-01";
                if (date.Value.Year.ToString() == "1979") return "1979-07-01";
                return null;
            };
        }

        private IEnumerable<Show> GetShows()
        {
            var show1 = new Show {Id = 1, ShowId = 1, Name = "Matrix"};
            var show2 = new Show {Id = 2, ShowId = 2, Name = "Kill Bill" };
            var actor1 = new Actor {Id = 1, ActorId = 1, Name = "Bill", BirthDay = new DateTime(1980, 7, 1)};
            var actor2 = new Actor {Id = 2, ActorId = 2, Name = "Tom", BirthDay = new DateTime(1979, 7, 1)};
            var actor3 = new Actor {Id = 3, ActorId = 3, Name = "Nick", BirthDay = new DateTime(1981, 7, 1)};
            var showActor11 = new ShowActor {ShowId = 1, ActorId = 1, Show = show1, Actor = actor1};
            var showActor22 = new ShowActor {ShowId = 2, ActorId = 2, Show = show2, Actor = actor2};
            var showActor23 = new ShowActor {ShowId = 2, ActorId = 3, Show = show2, Actor = actor3};
            //show1.ShowActors = new List<ShowActor> { showActor11 };
            //show2.ShowActors = new List<ShowActor> { showActor22, showActor23 };
            show1.ShowActors = new List<ShowActor>();
            show2.ShowActors = new List<ShowActor>();
            return new List<Show> {show1};
        }

        private IEnumerable<ShowDto> GetExpectedShows()
        {
            var show1 = new ShowDto { Id = 1, Name = "Matrix" };
            var show2 = new ShowDto { Id = 2, Name = "Kill Bill" };
            var actor1 = new ActorDto { Id = 1, Name = "Bill", Birthday = "1980-07-01" };
            var actor2 = new ActorDto { Id = 3, Name = "Nick", Birthday = "1981-07-01" };
            var actor3 = new ActorDto { Id = 2, Name = "Tom", Birthday = "1979-07-01" };
            show1.Cast = new List<ActorDto>();
            //show1.Cast = new List<ActorDto> { actor1 };
            //show2.Cast = new List<ActorDto> { actor2, actor3 };
            return new List<ShowDto> { show1 };
        }
    }
}