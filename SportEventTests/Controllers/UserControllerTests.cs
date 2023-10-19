using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportEvent.Controllers;
using SportEvent.Data.Interfaces;
using SportEvent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportEvent.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod]
        public async Task Register_ValidModel_RedirectsToLogin()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.Register(It.IsAny<RegisterModel>())).ReturnsAsync(true);

            var loggerMock = new Mock<ILogger<UserController>>();

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.Register(new RegisterModel());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Login", redirectResult.ActionName);
            Assert.AreEqual("User", redirectResult.ControllerName);
        }

        [TestMethod]
        public async Task Register_InvalidModel_ReturnsViewWithModelError()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetProfile()).ReturnsAsync(new UserModel());

            var loggerMock = new Mock<ILogger<UserController>>();

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);
            var invalidModel = new RegisterModel { /* initialize with invalid data */ };
            controller.ModelState.AddModelError("PropertyName", "Error message");

            // Act
            var result = await controller.Register(invalidModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey(string.Empty));
        }

        [TestMethod]
        public void Register_Get_ReturnsView()
        {

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetProfile()).ReturnsAsync(new UserModel());

            var loggerMock = new Mock<ILogger<UserController>>();

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);
            // Act
            var result = controller.Register() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Register_Post_ValidModel_RedirectsToLogin()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetProfile()).ReturnsAsync(new UserModel());

            var loggerMock = new Mock<ILogger<UserController>>();

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);
            var validModel = new RegisterModel { /* initialize with valid data */ };

            // Act
            var result = await controller.Register(validModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [TestMethod]
        public async Task Register_Post_InvalidModel_ReturnsViewWithModelError()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetProfile()).ReturnsAsync(new UserModel());

            var loggerMock = new Mock<ILogger<UserController>>();

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);
            var invalidModel = new RegisterModel { /* initialize with invalid data */ };
            controller.ModelState.AddModelError("PropertyName", "Error message");

            // Act
            var result = await controller.Register(invalidModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey(string.Empty));
        }

        [TestMethod]
        public async Task Profile_ReturnsViewWithModel()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetProfile()).ReturnsAsync(new UserModel());

            var loggerMock = new Mock<ILogger<UserController>>();

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.Profile();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(UserModel));
        }

        [TestMethod]
        public async Task Profile_ExceptionOccurs_LogsErrorAndThrows()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetProfile()).ThrowsAsync(new Exception("Test exception"));

            var loggerMock = new Mock<ILogger<UserController>>();

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await controller.Profile());

            // Verify logging
            loggerMock.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }


    }
}
