using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using UserService.Application.Extensions;
using UserService.Application.Handlers.Queries;
using UserService.Application.Tests.Handlers.Queries.Helpers;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

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

            _userDataService.Get(Arg.Any<CancellationToken>()).Returns(testUsers);

            string expectedResult = "Age: 23 Female: 1 Male: 1,Age: 54 Female: 0 Male: 1,Age: 66 Female: 1 Male: 2";

            var response = await _sut.Handle(new GetUserGenderNumbersByAgeQuery(), CancellationToken.None);

            response.Should().NotBeNull();
            response.Data.Should()
                .NotBeNull().And
                .NotBeEmpty();
            response.Data.Select(s => s.ToString()).ToJoinedString().Should().Be(expectedResult);
        }
    }
}