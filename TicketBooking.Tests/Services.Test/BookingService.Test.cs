using FluentAssertions;
using Moq;
using TicketsBooking.Application.DTOs.Booking;
using TicketsBooking.Application.Exceptions;
using TicketsBooking.Application.Interfaces;
using TicketsBooking.Application.Services;
using TicketsBooking.Domain.Entities;

namespace TicketBooking.Tests
{
    namespace TicketsBooking.Tests.Services
    {
        public class BookingServiceTests
        {
            private readonly Mock<IBookingRepositorie> _bookingRepoMock;
            private readonly Mock<IEventRepositorie> _eventRepoMock;
            private readonly Mock<IUserRepositorie> _userRepoMock;
            private readonly BookingService _service;

            public BookingServiceTests()
            {
                _bookingRepoMock = new Mock<IBookingRepositorie>();
                _eventRepoMock = new Mock<IEventRepositorie>();
                _userRepoMock = new Mock<IUserRepositorie>();

                _service = new BookingService(
                    _bookingRepoMock.Object,
                    _eventRepoMock.Object,
                    _userRepoMock.Object
                );
            }

            [Fact]
            public async Task GetSeatAvailabilityAsync_ShouldReturnCorrectAvailability_WhenEventExists()
            {
                // Arrange
                var eventId = 1;
                var testEvent = new Event
                {
                    Id = eventId,
                    Capacity = 100,
                    Name = "Test Event",
                    Location = "Test Location",
                    StartsAt = DateTime.UtcNow.AddDays(1)
                };

                _eventRepoMock.Setup(r => r.GetByIdAsync(eventId))
                    .ReturnsAsync(testEvent);
                _bookingRepoMock.Setup(r => r.GetBookedSeatsAsync(eventId))
                    .ReturnsAsync(30);

                // Act
                var result = await _service.GetSeatAvailabilityAsync(eventId);

                // Assert
                result.SeatCapacity.Should().Be(100);
                result.SeatsBooked.Should().Be(30);
                result.SeatsAvailable.Should().Be(70);
            }

            [Fact]
            public async Task GetSeatAvailabilityAsync_ShouldThrow_WhenEventNotFound()
            {
                // Arrange
                var eventId = 99;
                _eventRepoMock.Setup(r => r.GetByIdAsync(eventId))
                    .ReturnsAsync((Event)null);

                // Act
                var act = async () => await _service.GetSeatAvailabilityAsync(eventId);

                // Assert
                await act.Should().ThrowAsync<NotFoundException>()
                    .WithMessage("There is No Event Found Whith This Id.");
            }

            [Fact]
            public async Task AddBookingAsync_ShouldAddBooking_WhenDataIsValid()
            {
                // Arrange
                var createRequest = new CreateBookingRequest
                {
                    UserId = 1,
                    EventId = 1,
                    SeatBooked = 2
                };

                var testUser = new User
                {
                    Id = 1,
                    FullName = "Test User",
                    Username = "testuser",
                    Password = "Password123!",
                    Email = "test@example.com"
                };
                var testEvent = new Event
                {
                    Id = 1,
                    Capacity = 10,
                    Name = "Test Event",
                    Location = "Test Location",
                    StartsAt = DateTime.UtcNow.AddDays(1)
                };

                _userRepoMock.Setup(r => r.GetByIdAsync(createRequest.UserId))
                    .ReturnsAsync(testUser);
                _eventRepoMock.Setup(r => r.GetByIdAsync(createRequest.EventId))
                    .ReturnsAsync(testEvent);
                _bookingRepoMock.Setup(r => r.GetDuplicateDataAsync(createRequest.UserId, createRequest.EventId))
                    .ReturnsAsync(false);
                _bookingRepoMock.Setup(r => r.GetBookedSeatsAsync(createRequest.EventId))
                    .ReturnsAsync(3);
                _bookingRepoMock.Setup(r => r.AddAsync(It.IsAny<Booking>())).Returns(Task.CompletedTask);
                _bookingRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

                // Act
                var result = await _service.AddBookingAsync(createRequest);

                // Assert
                result.Should().NotBeNull();
                result.SeatBooked.Should().Be(createRequest.SeatBooked);
            }
            [Fact]
            public async Task CancelBookingAsync_ShouldCancelBooking_WhenBookingExists()
            {
                // Arrange
                var bookingId = 1;
                var existingBooking = new Booking
                {
                    Id = bookingId,
                    UserId = 1,
                    EventId = 1,
                    SeatBooked = 2,
                    IsCancelled = false
                };

                _bookingRepoMock.Setup(r => r.GetByIdAsync(bookingId))
                    .ReturnsAsync(existingBooking);
                _bookingRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

                // Act
                await _service.CancelBookingAsync(bookingId);

                // Assert
                existingBooking.IsCancelled.Should().BeTrue();
                existingBooking.SeatBooked.Should().Be(0);
                existingBooking.CancelledAt.Should().NotBe(default);
            }

            [Fact]
            public async Task CancelBookingAsync_ShouldThrow_WhenBookingAlreadyCancelled()
            {
                // Arrange
                var bookingId = 1;
                var existingBooking = new Booking
                {
                    Id = bookingId,
                    IsCancelled = true
                };

                _bookingRepoMock.Setup(r => r.GetByIdAsync(bookingId))
                    .ReturnsAsync(existingBooking);

                // Act
                var act = async () => await _service.CancelBookingAsync(bookingId);

                // Assert
                await act.Should().ThrowAsync<ValidationException>()
                    .WithMessage("Booking is already Cancelle.");
            }
        }
    }
}