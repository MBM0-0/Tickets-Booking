Solution Countan two projects
TicketBooking <== Main Project
TicketBooking <== Unit Test (XUnit)

TicketBooking Countane 3 Tables Every table Has It own Repostory, Service, countroler files

[UserController]
-----------------------------------------------------------------------------------------------------------------------------------------------------
UserTable => { Id, FullName, Username, Email, Password, CreatedAt, UpdatedAt, [Bookings] }
.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.

(GetUserByIdUser)

(CreateUser)
X <Username> -> Must have 5–16 characters, start with a letter, only letters, digits, or _, no whitespace
X <Paswword> -> Password must have a length of 8 or higher must have at less 1 cap and small later charcter and one number
X <Email> -> Must begin with a letter + Contane one of these (@yahoo.com", "@gmail.com", "@outlook.com", "@icloud.com ) <-<-<-<-||| begans with charcters + .com
X <Username + Email> -> every one of them have to be uniqe
O <CreatedAt> Will be issuend wen creting the user

(UpdateUser)
you can update only => { Fullname, Email, Password}
X <Id> -> If user you want to update not found
X <Paswword> -> Minimum 8 characters, must contain at least one uppercase, one lowercase letter, and one number
X <Email> -> Must start with a letter, and end with one of these domains: @yahoo.com, @gmail.com, @outlook.com, @icloud.com
X <Email> -> Must be unique
O <UpdatedAt> Will be issuend wen update the user
O <NOTE> All the data that are note interd will not change and keep its value

(DeleteUser) [Hard Delete]
X <Id> -> If user you want to delete not found

(GetByUserIdAsync)
X <Id> -> If user you want to get not found

(GetAllUserAsync)
X <Id> -> If There Is No users In the Database (But there is a SeedData of 2 Users)

----------------------------------------------------------------------------------------------------------------------------------------------------
[EventController]  |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
----------------------------------------------------------------------------------------------------------------------------------------------------
EventTable => { Id, Name, ?Description, Location, Capacity, StartsAt, EndsAt, IsEnded, CreatedAt, UpdatedAt, EndedAt, [Bookings]}
.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.
(CreateEvent)
X <Check Duplicate> Check if there is the a ACTIVE event with the same name
X <Seat Capacity> Capacity Must Be More than 5 at minmim
X <Time> Event cannot be in the past and must last at least 1 hour
O <CreatedAt> Will be issuend when creting the Event

(UpdateEvent)
you can update => { Name, Description, Location, Capacity, StartsAt, EndsAt}
X <Id> -> If event you want to update is not found
X <Check Duplicate> Check if there is the a ACTIVE event with the same name
X <Seat Capacity> Seat Capacity Cant be decrese
X <Time> Event cannot be in the past and must last at least 1 hour
O <UpdatedAt> Will be issuend wHen update the user
O <NOTE> All the data that are note interd will not change and keep its value

(DeleteEvent) [Hard Delete]
X <Id> -> If event you want to delete not found

(GetEventById)
X <Id> -> If event you want to get not found

(GetAllEvents)
X <Id> -> If There Is No Events In the Database

----------------------------------------------------------------------------------------------------------------------------------------------------
[BookingController] ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
----------------------------------------------------------------------------------------------------------------------------------------------------
BookingTable => { Id, EventId, [?Event], UserId, [?User], SeatBooked, CreatedAt, ?UpdatedAt, ?CancelledAt, IsCancelled }]
.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-

(CreateBooking)
X <EventId> -> If event you want to get not found
X <UserId> -> If event you want to get not found
X <Check douplicate> -> Cannot book the same event twice if an active booking exists
X <SeatBooked Limit> -> Seat Booked cant be more than 4 
X <Time> -> cant book a event that alrady ended or started
X <SeatAvailability> -> if there free seats or all of them are booked
O <CreatedAt> Will be issuend when creting the Event

(UpdateBooking)
X <Id> -> If booking you want to get not found of ended 
X <Diplacate> -> if there is no active bookings you cant update cancelld bookings
X <SeatBooked Limit> -> Seat Booked cant be more than 4 
X <SeatAvailability> -> if there free seats or all of them are booked
O <UpdatedAt> Will be issuend when update the user

(CancelBooking)
X <Id> -> If booking you want to get not found of ended
X <IsCancelled> -> it telse you if the booking is canceld or not

(GetBookingById)
X <Id> -> If booking you want to get not found

(GetAllBookings)
X <Id> -> If There Is No Bookings In the Database

----------------------------------------------------------------------------------------------------------------------------------------------------
[ExceptionMiddleware]    |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
----------------------------------------------------------------------------------------------------------------------------------------------------
ForbiddenException => 403
NotFoundException => 400
ValidationException => 404
[EventExpirationService]
<<<BROKEN>>>

----------------------------------------------------------------------------------------------------------------------------------------------------
[EventExpirationService] : BackgroundService  ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
----------------------------------------------------------------------------------------------------------------------------------------------------
the background job will check if there is a event that is started or ended and it will update the isended to true then if a event ended it will go and cancel all the tickets that is related to this event
and all the data that are canceld cant be updated or shown you can only search for them by id
and will do the same thing for the users that booked that event 
it work every 1 minut

----------------------------------------------------------------------------------------------------------------------------------------------------
[AuthService] : BackgroundService  |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
----------------------------------------------------------------------------------------------------------------------------------------------------
it creat a token and you can authontakate the Generates authentication tokens valid for 15 minutes all 
countrolers are Protected all controllers except the login endpoint

----------------------------------------------------------------------------------------------------------------------------------------------------
[SeedData] : BackgroundService  ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
----------------------------------------------------------------------------------------------------------------------------------------------------
it check the database when the program starts if the users table is emty it adds admin user
Username = admin
Password = 123456Aa

----------------------------------------------------------------------------------------------------------------------------------------------------
Missing Feachers   |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
----------------------------------------------------------------------------------------------------------------------------------------------------
1. Event => EndedAt is not funcshinal,
2. Booking By Authorized User,
3. Pagination,
4. Validation can be improved,
5. IsCancelled and IsEnded are unnassary i have EndedAt,
6. Consider a base entity for repeated fields like Id, CreatedAt, UpdatedAt, EndedAt
7. Soft delete option for events ,
8. Exception handling middleware is broken
9. Passwords are not encrypted
10. Program.cs Need a Cleanup
11. Make All DateTimes Work In IRaqi Time Zone +3
12. If the Event Ends Booking Seats Will Not Be Reset 

----------------------------------------------------------------(Unit Test)-------------------------------------------------------------------------
it tests 5 services in dirnt pathes

[BookingServiceTests]

GetSeatAvailabilityAsync_ShouldReturnCorrectAvailability_WhenEventExists
GetSeatAvailabilityAsync_ShouldThrow_WhenEventNotFound
AddBookingAsync_ShouldAddBooking_WhenDataIsValid
CancelBookingAsync_ShouldCancelBooking_WhenBookingExists
CancelBookingAsync_ShouldThrow_WhenBookingAlreadyCancelled

[EventService.Tests]

UpdateEventAsync_ShouldUpdateEvent_WhenDataIsValid
UpdateEventAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist
UpdateEventAsync_ShouldThrowValidationException_WhenNameIsDuplicate

[UserServiceTests]

AddUserAsync_ShouldAddUser_WhenDataIsValid
AddUserAsync_ShouldThrowValidationException_WhenUsernameIsInvalid
AddUserAsync_ShouldThrowValidationException_WhenEmailExists
AddUserAsync_ShouldThrowValidationException_WhenUsernameExists
