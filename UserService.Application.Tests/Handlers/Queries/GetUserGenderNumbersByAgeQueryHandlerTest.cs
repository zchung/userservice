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
    public class GetUserGenderNumbersByAgeQueryHandlerTest
    {
        private readonly IFixture _fixture;
        private readonly IUserDataService _userDataService;
        private readonly GetUserGenderNumbersByAgeQueryHandler _sut;

        public GetUserGenderNumbersByAgeQueryHandlerTest()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });
            _userDataService = _fixture.Freeze<IUserDataService>();
            _sut = _fixture.Create<GetUserGenderNumbersByAgeQueryHandler>();
        }

        [TestMethod]
        public async Task Handle_Should_Return_The_Correct_Data()
        {
            // Arrange
            IEnumerable<IUser> testUsers = TestDataHelper.GetTestUsers();

            _userDataService.Get(Arg.Any<CancellationToken>()).Returns(Response<IEnumerable<IUser>>.GetSuccessResponse(testUsers));

            string expectedResult = "Age: 23 Female: 1 Male: 1,Age: 54 Female: 0 Male: 1,Age: 66 Female: 1 Male: 2";

            var response = await _sut.Handle(new GetUserGenderNumbersByAgeQuery(), CancellationToken.None);

            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Data.Should()
                .NotBeNull().And
                .NotBeEmpty();
            response.Data.Select(s => s.ToString()).ToJoinedString().Should().Be(expectedResult);
        }

        [TestMethod]
        public async Task Handle_Should_Return_Error_If_No_Users()
        {
            _userDataService.Get(Arg.Any<CancellationToken>()).Returns(Response<IEnumerable<IUser>>.GetSuccessResponse(Enumerable.Empty<IUser>()));

            var result = await _sut.Handle(new GetUserGenderNumbersByAgeQuery(), CancellationToken.None);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Messages.Should().NotBeEmpty();
            result.Messages.First().Should().Be(MessageConstants.NoUsersFound);
        }
    }
}