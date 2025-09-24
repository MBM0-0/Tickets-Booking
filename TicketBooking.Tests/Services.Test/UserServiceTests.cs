using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TicketsBooking.Application.DTOs.User;
using TicketsBooking.Application.Exceptions;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Application.Services;
using TicketsBooking.Domain.Entities;
using Xunit;

namespace TicketBooking.Tests.Services.Test
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepositorie> _userRepoMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepositorie>();
            _service = new UserService(_userRepoMock.Object);
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUser_WhenDataIsValid()
        {
            // Arrange
            var createDto = new CreateUserRequest
            {
                FullName = "Test User",
                Username = "TestUser1",
                Password = "Password123",
                Email = "testuser@gmail.com"
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(createDto.Email))
                .ReturnsAsync((User)null);
            _userRepoMock.Setup(r => r.GetByUsernameAsync(createDto.Username))
                .ReturnsAsync((User)null);
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddUserAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.FullName.Should().Be(createDto.FullName);
            result.Email.Should().Be(createDto.Email);
            result.Username.Should().Be(createDto.Username);
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_ShouldThrowValidationException_WhenUsernameIsInvalid()
        {
            // Arrange
            var createDto = new CreateUserRequest
            {
                FullName = "Test User",
                Username = "1Invalid", // starts with number
                Password = "Password123",
                Email = "testuser@gmail.com"
            };

            // Act
            Func<Task> act = async () => await _service.AddUserAsync(createDto);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Username Shoud Be betwen 4 to 17 later begans with a letter exaptonly _ from charcters cant have white space.");
        }

        [Fact]
        public async Task AddUserAsync_ShouldThrowValidationException_WhenEmailExists()
        {
            // Arrange
            var createDto = new CreateUserRequest
            {
                FullName = "Test User",
                Username = "TestUser1",
                Password = "Password123",
                Email = "testuser@gmail.com"
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(createDto.Email))
.ReturnsAsync(new User
{
    Id = 1,
    FullName = "Existing User",
    Username = "ExistingUser",
    Password = "Password123",
    Email = "existing@gmail.com"
});

            // Act
            Func<Task> act = async () => await _service.AddUserAsync(createDto);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("The Email You Entered is Used Try Another One.");
        }

        [Fact]
        public async Task AddUserAsync_ShouldThrowValidationException_WhenUsernameExists()
        {
            // Arrange
            var createDto = new CreateUserRequest
            {
                FullName = "Test User",
                Username = "TestUser1",
                Password = "Password123",
                Email = "newemail@gmail.com"
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(createDto.Email))
                .ReturnsAsync((User)null);
            _userRepoMock.Setup(r => r.GetByUsernameAsync(createDto.Username))
    .ReturnsAsync(new User
    {
        Id = 2,
        FullName = "Existing User 2",
        Username = createDto.Username,
        Password = "Password123",
        Email = "other@gmail.com"
    });

            // Act
            Func<Task> act = async () => await _service.AddUserAsync(createDto);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("The username You Entered is Used Try Another One.");
        }
    }
}
