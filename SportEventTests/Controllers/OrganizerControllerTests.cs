using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using SportEvent.Controllers;
using SportEvent.Data.Implementations;
using SportEvent.Data.Interfaces;
using SportEvent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace SportEvent.Controllers.Tests
{
    [TestClass]
    public class OrganizerRepositoryTests
    {
        [TestMethod]
        public void Create_Unauthorized_RedirectsToUnauthorizedAction()
        {
            var mockOrganizerRepository = new Mock<IOrganizerRepository>();
            var mockLogger = new Mock<ILogger<OrganizerController>>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockController = new Mock<OrganizerController>(mockOrganizerRepository.Object, mockLogger.Object, mockConfiguration.Object);
  /*          mockController.Setup(a => a.IsAuthorization()).Returns(false);*/

            // Act
            var result = mockController.Object.Create();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Unauthorized", result.ViewName);
            Assert.AreEqual("User", result.Model);
        }


        [TestMethod]
        public async Task Create_ValidOrganizer_ReturnsTrue()
        {
            var mockHttp = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHttp);

            string bearerToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczpcL1wvYXBpLXNwb3J0LWV2ZW50cy5waHA5LTAxLnRlc3Qudm94dGVuZW8uY29tXC9hcGlcL3YxXC91c2Vyc1wvbG9naW4iLCJpYXQiOjE2OTc2MDY4NDMsImV4cCI6MTY5NzY5MzI0MywibmJmIjoxNjk3NjA2ODQzLCJqdGkiOiJYWVNYN01TaG1lNDRMUWxaIiwic3ViIjoyOTA4LCJwcnYiOiI4N2UwYWYxZWY5ZmQxNTgxMmZkZWM5NzE1M2ExNGUwYjA0NzU0NmFhIn0.JJ3V1MCSOKd1rkWz08d-uLj1wmaf258hKpC_PaPoeYk";
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            mockHttp.When("https://api-sport-events.php9-01.test.voxteneo.com/api/v1/organizers")
              .Respond(HttpStatusCode.OK);

            var organizerRepository = new OrganizerRepository();

            var model = new Organizer
            {
                // Set valid properties for the organizer
            };

            // Act
            var result = await organizerRepository.Create(model, httpClient);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Create_InvalidOrganizer_ReturnsFalse()
        {
            var mockHttp = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHttp);

            string bearerToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczpcL1wvYXBpLXNwb3J0LWV2ZW50cy5waHA5LTAxLnRlc3Qudm94dGVuZW8uY29tXC9hcGlcL3YxXC91c2Vyc1wvbG9naW4iLCJpYXQiOjE2OTc2MDY4NDMsImV4cCI6MTY5NzY5MzI0MywibmJmIjoxNjk3NjA2ODQzLCJqdGkiOiJYWVNYN01TaG1lNDRMUWxaIiwic3ViIjoyOTA4LCJwcnYiOiI4N2UwYWYxZWY5ZmQxNTgxMmZkZWM5NzE1M2ExNGUwYjA0NzU0NmFhIn0.JJ3V1MCSOKd1rkWz08d-uLj1wmaf258hKpC_PaPoeYk";
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            mockHttp.When("https://api-sport-events.php9-01.test.voxteneo.com/api/v1/organizers")
                .Respond(HttpStatusCode.BadRequest);

            var organizerRepository = new OrganizerRepository();

            var model = new Organizer
            {
                // Set valid properties for the organizer
            };

            // Act
            var result = await organizerRepository.Create(model, httpClient);

            // Assert
            Assert.IsFalse(result);
        }

        public async Task GetAllOrganizers_ValidResponse_ReturnsListOfOrganizers()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHttp);

            mockHttp.When("https://api-sport-events.php9-01.test.voxteneo.com/api/v1/organizers")
              .Respond(HttpStatusCode.OK);
            var organizerRepository = new OrganizerRepository();

            // Act
            var result = await organizerRepository.GetAllOrganizers(httpClient);

            // Assert
            Assert.IsNotNull(result);
            // Add further assertions based on the expected data.
        }

        /* [TestMethod]
         public async Task GetAllOrganizers_ValidResponse_ReturnsListOfOrganizers()
         {
             // Arrange
             var httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.OK));
             var organizerRepository = new OrganizerRepository();

             // Act
             var result = await organizerRepository.GetAllOrganizers(httpClient);

             // Assert
             Assert.IsNotNull(result);
             // Add further assertions based on the expected data.
         }

         [TestMethod]
         public async Task GetById_ValidId_ReturnsOrganizer()
         {
             // Arrange
             var httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.OK));
             var organizerRepository = new OrganizerRepository();

             int organizerId = 1; // Replace with a valid organizer ID

             // Act
             var result = await organizerRepository.GetById(organizerId, httpClient);

             // Assert
             Assert.IsNotNull(result);
             // Add further assertions based on the expected data.
         }

         [TestMethod]
         public async Task EditOrganizer_ValidData_ReturnsTrue()
         {
             // Arrange
             var httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.OK));
             var organizerRepository = new OrganizerRepository();

             int organizerId = 1; // Replace with a valid organizer ID
             var data = new Organizer
             {
                 // Set valid properties for the organizer
             };

             // Act
             var result = await organizerRepository.EditOrganizer(organizerId, data, httpClient);

             // Assert
             Assert.IsTrue(result);
         }

         [TestMethod]
         public async Task Delete_ValidId_ReturnsTrue()
         {
             // Arrange
             var httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.OK));
             var organizerRepository = new OrganizerRepository();

             int organizerId = 1; // Replace with a valid organizer ID

             // Act
             var result = await organizerRepository.Delete(organizerId, httpClient);

             // Assert
             Assert.IsTrue(result);
         }*/

        // Implement further tests for error cases, edge cases, and additional scenarios.
    }
}   