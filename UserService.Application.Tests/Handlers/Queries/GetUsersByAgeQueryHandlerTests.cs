using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using UserService.Application.Extensions;
using UserService.Application.Handlers.Queries;
using UserService.Application.Tests.Handlers.Queries.Helpers;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Domain.Responses;

namespace UserService.Application.Tests.Handlers.Queries
{
    [TestClass]
    public class GetUsersByAgeQueryHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly IUserDataService _userDataService;
        private readonly GetUsersByAgeQueryHandler _sut;

        public GetUsersByAgeQueryHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });
            _userDataService = _fixture.Freeze<IUserDataService>();
            _sut = _fixture.Create<GetUsersByAgeQueryHandler>();
        }

        [TestMethod]
        public async Task Handle_Should_Return_The_Correct_Users()
        {
            // Arrange
            IEnumerable<IUser> testUsers = TestDataHelper.GetTestUsers();

            _userDataService.Get(Arg.Any<CancellationToken>()).Returns(Response<IEnumerable<IUser>>.GetSuccessResponse(testUsers));

            var expectedFullNames = "Bill Bryson,Frank Zappa";

            // Act
            var result = await _sut.Handle(new GetUsersByAgeQuery(23), CancellationToken.None);

            result.Should()
                .NotBeNull();
            result.Data.Should()
                .NotBeNull().And
                .NotBeEmpty();
            result.Data.Select(u => u.FullName).ToJoinedString().Should().Be(expectedFullNames);
        }
    }
}