using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PracriceAPI.Tests.Integration.StepDefinitions
{
    [Binding]
    public class EmployeeStepDefinitions
    {
        // These fields store data between steps in the same scenario
        private HttpClient _httpClient;
        private object _employeeData;
        private HttpResponseMessage _response;

        // This method runs when Reqnroll sees "Given I have the employee API available"
        [Given(@"I have the employee API available")]
        public void GivenIHaveTheEmployeeAPIAvailable()
        {
            // Setup: Create HTTP client to call our API
            _httpClient = new HttpClient();
            // IMPORTANT: Your API must be running on this port for the test to work!
            _httpClient.BaseAddress = new Uri("https://localhost:7262");
        }

        // This method runs when Reqnroll sees "And I have valid employee data"
        [Given(@"I have valid employee data")]
        public void GivenIHaveValidEmployeeData()
        {
            // Setup: Create test employee data matching your CreateEmployeeDto
            _employeeData = new
            {
                Name = "John Doe",
                EmployeeId = 101,
                Salary = 50000m
            };
        }

        // This method runs when Reqnroll sees "When I send a POST request to add the employee"
        [When(@"I send a POST request to add the employee")]
        public async Task WhenISendAPostRequestToAddTheEmployee()
        {
            // Action: Convert employee data to JSON and send POST request
            var json = JsonSerializer.Serialize(_employeeData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // This calls your actual API endpoint!
            _response = await _httpClient.PostAsync("/api/employee", content);
        }

        // This method runs when Reqnroll sees "Then the response should be successful"
        [Then(@"the response should be successful")]
        public void ThenTheResponseShouldBeSuccessful()
        {
            // Verification: Check if the HTTP response indicates success
            _response.Should().NotBeNull();
            _response.IsSuccessStatusCode.Should().BeTrue();
        }

        // This method runs when Reqnroll sees "And the response should contain a success message"
        [Then(@"the response should contain a success message")]
        public async Task ThenTheResponseShouldContainASuccessMessage()
        {
            // Verification: Check if the response contains the expected message
            var responseContent = await _response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("success");
            responseContent.Should().Contain("Employee successfully added");
        }

        // Clean up after each scenario
        public void Dispose()
        {
            _httpClient?.Dispose();
            _response?.Dispose();
        }
    }
}
