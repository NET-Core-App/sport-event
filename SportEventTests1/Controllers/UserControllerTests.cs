using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportEvent.Controllers;
using SportEvent.Data;
using SportEvent.Data.Interfaces;
using SportEvent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace SportEvent.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod]
        public async Task Profile_ReturnsViewResult_WhenAuthenticated()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var apiClientMock = new Mock<HttpClient>();
            var authorizationMock = new Mock<AuthorizationService>();
            var controller = new UserController(userRepositoryMock.Object, null, authorizationMock.Object);
            var user = new UserModel(); // Mock user data

            authorizationMock.Setup(auth => auth.IsAuthorization(apiClientMock.Object))
                .Returns(true);
            userRepositoryMock.Setup(repo => repo.GetProfile(apiClientMock.Object))
                .ReturnsAsync(user);

            // Act
            var result = await controller.Profile();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(UserModel));
        }

        
    }
}