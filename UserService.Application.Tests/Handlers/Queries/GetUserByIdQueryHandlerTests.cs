using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using UserService.Application.Handlers.Queries;
using UserService.Application.Tests.Handlers.Queries.Helpers;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Domain.Models.Constants;
using UserService.Domain.Responses;

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

            _userDataService.Get(Arg.Any<CancellationToken>()).Returns(Response<IEnumerable<IUser>>.GetSuccessResponse(testUsers));

            // Act
            var result = await _sut.Handle(new GetUserByIdQuery(id), CancellationToken.None);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(id);
            result.Data.FullName.Should().Be(expectedFullName);
        }

        [TestMethod]
        public async Task Handle_Should_Return_Error_If_No_User()
        {
            _userDataService.Get(Arg.Any<CancellationToken>()).Returns(Response<IEnumerable<IUser>>.GetSuccessResponse(Enumerable.Empty<IUser>()));

            var result = await _sut.Handle(new GetUserByIdQuery(1), CancellationToken.None);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Messages.Should().NotBeEmpty();
            result.Messages.First().Should().Be(MessageConstants.NoUserByThatId);
        }
    }
}