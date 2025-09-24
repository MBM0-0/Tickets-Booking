using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TicketsBooking.Application.DTOs.Event;
using TicketsBooking.Application.Exceptions;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Application.Services;
using TicketsBooking.Domain.Entities;
using Xunit;

namespace TicketBooking.Tests.Services.Test
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepositorie> _eventRepoMock;
        private readonly EventService _service;

        public EventServiceTests()
        {
            _eventRepoMock = new Mock<IEventRepositorie>();
            _service = new EventService(_eventRepoMock.Object);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldUpdateEvent_WhenDataIsValid()
        {
            // Arrange
            var eventId = 1;
            var existingEvent = new Event
            {
                Id = eventId,
                Name = "Old Event",
                Description = "Old Description",
                Location = "Old Location",
                Capacity = 50,
                StartsAt = DateTime.UtcNow.AddDays(1),
                EndsAt = DateTime.UtcNow.AddDays(1).AddHours(2),
                IsEnded = false
            };

            var updateDto = new UpdateEventRequest
            {
                Id = eventId,
                Name = "Updated Event",
                Description = "Updated Description",
                Location = "Updated Location",
                Capacity = 60,
                StartsAt = DateTime.UtcNow.AddDays(2),
                EndsAt = DateTime.UtcNow.AddDays(2).AddHours(3)
            };

            _eventRepoMock.Setup(r => r.GetByIdAsync(eventId))
                .ReturnsAsync(existingEvent);

            _eventRepoMock.Setup(r => r.GetDuplicateDataAsync(updateDto.Name))
                .ReturnsAsync(false);

            _eventRepoMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateEventAsync(updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(updateDto.Name);
            result.Description.Should().Be(updateDto.Description);
            result.Location.Should().Be(updateDto.Location);
            result.Capacity.Should().Be(updateDto.Capacity);
            result.StartsAt.Should().Be(updateDto.StartsAt);
            result.EndsAt.Should().Be(updateDto.EndsAt);
            _eventRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var updateDto = new UpdateEventRequest { Id = 99 };

            _eventRepoMock.Setup(r => r.GetByIdAsync(updateDto.Id))
                .ReturnsAsync((Event)null);

            // Act
            Func<Task> act = async () => await _service.UpdateEventAsync(updateDto);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Event not found or has already ended.");
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldThrowValidationException_WhenNameIsDuplicate()
        {
            // Arrange
            var eventId = 1;
            var existingEvent = new Event
            {
                Id = eventId,
                Name = "Old Event",
                Location = "Old Location",
                Description = "Old Description",
                Capacity = 50,
                StartsAt = DateTime.UtcNow.AddDays(1),
                EndsAt = DateTime.UtcNow.AddDays(1).AddHours(2),
                IsEnded = false
            };
            var updateDto = new UpdateEventRequest
            {
                Id = eventId,
                Name = "Duplicate Event"
            };

            _eventRepoMock.Setup(r => r.GetByIdAsync(eventId))
                .ReturnsAsync(existingEvent);

            _eventRepoMock.Setup(r => r.GetDuplicateDataAsync(updateDto.Name))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.UpdateEventAsync(updateDto);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("There is already an active event with this name.");
        }
    }
}
