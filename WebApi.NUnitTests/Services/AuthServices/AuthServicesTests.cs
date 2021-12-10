using Business.Abstract;
using Business.Concrete;
using Core.Entities.Concrete;
using Core.Utilities.Messages;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WebApi.NUnitTests.Helpers;

namespace WebApi.NUnitTests.Services.AuthServices
{
    [TestFixture]
    public class AuthServicesTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<ITokenHelper> _mockTokenHelper;

        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockTokenHelper = new Mock<ITokenHelper>();
            _authService = new AuthService(_mockUserService.Object, _mockTokenHelper.Object);
        }

        [Test]
        public void WhenCorrectPasswordForExistingEmailIsGiven_Login_ReturnsUserForCorrectEmail()
        {
            string expectedEmail = "burak@burak.com";
            string expectedPassword = "1";

            var user = DataHelper.GetUser(expectedEmail);
            HashingHelper.CreatePasswordHash(expectedPassword, out var passwordHash, out var passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            _mockUserService.Setup(s => s.GetByMail(expectedEmail).Data).Returns(() => user);

            _mockTokenHelper.Setup(x => x.CreateToken(user, It.IsAny<List<OperationClaim>>())).Returns(new AccessToken()
            {
                Token = It.IsAny<string>(),
                Expiration = DateTime.Now.AddHours(1)
            });

            IDataResult<User> userResult = _authService.Login(new UserForLoginDto { Email = expectedEmail, Password = expectedPassword });
            string actualEmail = userResult.Data.Email;
            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public void WhenValidInputAreGiven_Register_ShouldBeReturnAdded()
        {
            var registerUser = new UserForRegisterDto { Email = "test@test.com", Password = "1", FirstName = "Burak", LastName = "Hayırlı" };
            _mockUserService.Setup(s => s.Add(It.IsAny<User>()).Success).Returns(true);
            var result = _authService.Register(registerUser, registerUser.Password);
            result.Success.Should().BeTrue();
            result.Message.Should().Be(BusinessMessages.Added);
        }

        [Test]
        public void WhenExistUserEmailIsGiven_Register_ShouldBeReturnUserAlreadyExists()
        {
            var alreadyExistUser = DataHelper.GetUser("test@test.com");
            _mockUserService.Setup(s => s.Add(It.IsAny<User>()).Success).Returns(false);
            var result = _authService.Register(new UserForRegisterDto { Email = alreadyExistUser.Email, Password = "1" }, "1");
            result.Message.Should().Be(Core.Utilities.Messages.BusinessMessages.UserAlreadyExists);
        }
    }
}
