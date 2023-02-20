using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using UserService.Application.Handlers.Queries;
using UserService.Domain.Models;
using UserService.Infrastructure.Services.Interfaces;

namespace UserService.Application.Tests.Handlers.Queries
{
    [TestClass]
    public class GetUserByIdQueryHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly IUserDataService _userDataService;
        private readonly GetUserByIdQueryHandler _sut;

        public GetUserByIdQueryHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });
            _userDataService = _fixture.Freeze<IUserDataService>();
            _sut = _fixture.Create<GetUserByIdQueryHandler>();
        }

        [TestMethod]
        public async Task Handle_Should_Return_The_Correct_User()
        {
            // Arrange
            IEnumerable<IUser> testUsers = new List<User>()
            {
                new User { Id = 53, First = "Bill", Last = "Bryson", Age = 23, Gender = "M" },
                new User { Id = 62, First = "John", Last = "Travolta", Age = 54, Gender = "M" },
                new User { Id = 41, First = "Frank", Last = "Zappa", Age = 23, Gender = "T" },
                new User { Id = 31, First = "Jill", Last = "Scott", Age = 66, Gender = "Y" },
                new User { Id = 31, First = "Anna", Last = "Meredith", Age = 66, Gender = "Y" },
                new User { Id = 31, First = "Janet", Last = "Jackson", Age = 66, Gender = "F" }
            };
            var firstUser = testUsers.First();
            var id = 41;
            var fullName = "Frank Zappa";

            _userDataService.Get(Arg.Any<CancellationToken>()).Returns(testUsers);

            // Act
            var result = await _sut.Handle(new GetUserByIdQuery(id), CancellationToken.None);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(id);
        }
    }
}