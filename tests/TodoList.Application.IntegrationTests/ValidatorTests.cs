using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.Results;
using TodoList.Application.Validators;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Enums;
using Xunit;

namespace TodoList.Application.IntegrationTests
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData("Item Name", Status.InProgress, 0)]
        [InlineData("Item Name", Status.Completed, 100)]
        [InlineData("a", Status.NotStarted, 12)]
        [InlineData("Item Name")]
        public async Task TodoRequestValidator_ShouldPass_WhenInputsAreAcceptable(string name, Status status = default,
            int priority = 0)
        {
            // Arrange
            var validator = new TodoRequestValidator();
            var cmd = new TodoRequest {Name = name, Status = status.ToString(), Priority = priority.ToString()};

            // Act
            var validationResult = await validator.ValidateAsync(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("Todo item field 'name' cannot be empty",            "")]
        [InlineData("is not a valid number or not within range <0,100>", "valid", "101")]
        [InlineData("is not a valid number or not within range <0,100>", "valid", "-1")]
        [InlineData("is not a valid number or not within range <0,100>", "valid", "")]
        [InlineData("Todo item field 'status' must be one of ",           "valid", "15", "invalidStatus")]
        public async Task TodoRequestValidator_ShouldFail_WhenInputsAreInvalid(string err, string name,
            string priority = "0", string status = "InProgress")
        {
            // Arrange
            var validator = new TodoRequestValidator();
            var cmd = new TodoRequest {Name = name, Status = status, Priority = priority};

            // Act
            var validationResult = await validator.ValidateAsync(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            var listOfErrors = validationResult.Errors.Select(m => m.ErrorMessage);
            listOfErrors.Any(m => m.Contains(err)).Should().BeTrue();
        }

        [Theory]
        [InlineData("Todo item field 'name' cannot be empty", "1", "")]
        [InlineData("is not a valid number or not within range <0,100>", "1", "valid", "101")]
        [InlineData("is not a valid number or not within range <0,100>", "1","valid", "-1")]
        [InlineData("is not a valid number or not within range <0,100>", "1","valid", "")]
        [InlineData("Todo item field 'status' must be one of ", "1","valid", "15", "invalidStatus")]
        [InlineData("is not a valid id or it's lower than 1", "0","valid", "15")]
        [InlineData("is not a valid id or it's lower than 1", "-1","valid", "15")]
        public async Task UpdateTodoRequestValidator_ShouldFail_WhenInputsAreInvalid(string err, string id, string name,
            string priority = "0", string status = "InProgress")
        {
            // Arrange
            var validator = new UpdateTodoRequestValidator();
            var cmd = new UpdateTodoRequest {Id = id, Name = name, Status = status, Priority = priority};
            
            // Act
            var validationResult = await validator.ValidateAsync(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            var listOfErrors = validationResult.Errors.Select(m => m.ErrorMessage);
            listOfErrors.Any(m => m.Contains(err)).Should().BeTrue();
        }
    }
}