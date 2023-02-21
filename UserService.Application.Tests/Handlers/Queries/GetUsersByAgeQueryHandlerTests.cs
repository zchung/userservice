using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using UserService.Application.Extensions;
using UserService.Application.Handlers.Queries;
using UserService.Application.Tests.Handlers.Queries.Helpers;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Domain.Models.Constants;
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

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should()
                .NotBeNull().And
                .NotBeEmpty();
            result.Data.Select(u => u.FullName).ToJoinedString().Should().Be(expectedFullNames);
        }

        [TestMethod]
        public async Task Handle_Should_Return_Error_If_No_Users()
        {
            _userDataService.Get(Arg.Any<CancellationToken>()).Returns(Response<IEnumerable<IUser>>.GetSuccessResponse(Enumerable.Empty<IUser>()));

            var result = await _sut.Handle(new GetUsersByAgeQuery(23), CancellationToken.None);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Messages.Should().NotBeEmpty();
            result.Messages.First().Should().Be(MessageConstants.NoUsersFound);
        }

        [TestMethod]
        public async Task Handle_Should_Page_A_User_List_Correctly()
        {
            // Arrange
            IEnumerable<User> testUsers = _fixture
                .Build<User>()
                .With(w => w.Age, 23)
                .CreateMany(1000).ToList();
            int pageSize = 10;
            _userDataService.Get(Arg.Any<CancellationToken>()).Returns(Response<IEnumerable<IUser>>.GetSuccessResponse(testUsers));

            // Act
            var result = await _sut.Handle(new GetUsersByAgeQuery(23, pageSize, 1), CancellationToken.None);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should()
                .NotBeEmpty().And
                .HaveCount(pageSize);
        }
    }
}