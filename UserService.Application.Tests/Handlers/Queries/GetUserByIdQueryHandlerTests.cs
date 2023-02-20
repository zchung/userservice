using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using UserService.Application.Handlers.Queries;
using UserService.Application.Tests.Handlers.Queries.Helpers;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

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
            IEnumerable<IUser> testUsers = TestDataHelper.GetTestUsers();
            var id = 41;
            var expectedFullName = "Frank Zappa";

            _userDataService.Get(Arg.Any<CancellationToken>()).Returns(testUsers);

            // Act
            var result = await _sut.Handle(new GetUserByIdQuery(id), CancellationToken.None);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(id);
            result.Data.FullName.Should().Be(expectedFullName);
        }
    }
}