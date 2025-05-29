USE [WeddingManagementSystem];
GO

-- Disable all foreign key constraints
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";
GO

-- Clear existing data (optional, uncomment if you want to start fresh)
/*
DELETE FROM [dbo].[Payments];
DELETE FROM [dbo].[Reviews];
DELETE FROM [dbo].[Notifications];
DELETE FROM [dbo].[AgendaItems];
DELETE FROM [dbo].[Agendas];
DELETE FROM [dbo].[GuestCategories];
DELETE FROM [dbo].[WeddingServices];
DELETE FROM [dbo].[WeddingTasks];
DELETE FROM [dbo].[Bookings];
DELETE FROM [dbo].[Weddings];
DELETE FROM [dbo].[Users];
DELETE FROM [dbo].[__EFMigrationsHistory];
GO

-- Reseed identity columns (optional, uncomment if you deleted data)
DBCC CHECKIDENT ('[dbo].[Payments]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Reviews]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Notifications]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[AgendaItems]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Agendas]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[GuestCategories]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[WeddingServices]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[WeddingTasks]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Bookings]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Weddings]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Users]', RESEED, 0);
GO
*/

-- 1. Users
INSERT INTO [dbo].[Users] ([FullName], [Email], [Password], [Role], [CreatedAt], [PasswordResetToken], [PasswordResetTokenExpiry])
VALUES
('Alice Wonderland', 'alice@example.com', 'hashed_password_alice', 'Client', GETDATE(), NULL, NULL),
('Bob The Builder', 'bob@example.com', 'hashed_password_bob', 'Client', GETDATE(), NULL, NULL),
('Charlie Brown', 'charlie@example.com', 'hashed_password_charlie', 'Admin', GETDATE(), NULL, NULL),
('Diana Prince', 'diana@example.com', 'hashed_password_diana', 'Vendor', GETDATE(), NULL, NULL),
('Eve Harrington', 'eve@example.com', 'hashed_password_eve', 'Planner', GETDATE(), NULL, NULL);
GO

-- 2. Weddings
-- Assuming Alice (UserId=1) and Bob (UserId=2) are clients planning weddings.
-- UserId in Weddings table is nvarchar(max), so we'll insert the ID as a string.
INSERT INTO [dbo].[Weddings] ([Title], [Description], [WeddingDate], [StartTime], [Location], [Theme], [GuestCount], [Budget], [UserId], [CreatedAt], [UpdatedAt], [CoverImagePath], [EnableRSVP])
VALUES
('Alice & Alex''s Grand Celebration', 'A beautiful summer garden wedding.', '2025-08-15', '2025-08-15 14:00:00', 'The Grand Ballroom', 'Classic Romance', 150, 25000.00, '1', GETDATE(), NULL, '/images/weddings/alice_alex.jpg', 1),
('Bob & Bella''s Rustic Barn Wedding', 'A charming and intimate rustic wedding.', '2025-09-20', '2025-09-20 16:00:00', 'Old Willow Barn', 'Rustic Chic', 80, 15000.00, '2', GETDATE(), NULL, '/images/weddings/bob_bella.jpg', 1),
('Upcoming Spring Wedding', 'Details to be finalized.', '2026-04-10', '2026-04-10 15:00:00', 'To Be Determined', 'Modern Minimalist', 100, 20000.00, '1', GETDATE(), NULL, NULL, 0);
GO

-- 3. Bookings
-- WeddingId 1 for Alice's wedding, WeddingId 2 for Bob's wedding.
-- One booking not yet linked to a wedding (initial inquiry).
INSERT INTO [dbo].[Bookings] ([CoupleName], [Email], [Phone], [WeddingDate], [Service], [RequestedOn], [Budget], [GuestCount], [Status], [SpecialRequests], [Theme], [Venue], [Priority], [Timeline], [WeddingId])
VALUES
('Alice & Alex', 'alice@example.com', '555-0101', '2025-08-15', 'Full Planning Package', '2024-10-01', 25000.00, 150, 'Confirmed', 'Need a vegan menu option for 10 guests.', 'Classic Romance', 'The Grand Ballroom', 'High', '10 months', 1),
('Bob & Bella', 'bob@example.com', '555-0102', '2025-09-20', 'Day-Of Coordination', '2025-01-15', 15000.00, 80, 'Confirmed', 'Ensure sound system is top-notch for the band.', 'Rustic Chic', 'Old Willow Barn', 'Medium', '8 months', 2),
('Clara & Chris', 'clara.chris@example.com', '555-0103', '2026-06-01', 'Venue Scouting', '2025-05-20', 18000.00, 120, 'Pending', 'Looking for a beachfront venue.', 'Beach Bohemian', 'Undecided', 'High', '12 months', NULL),
('Alice & Alex (Catering)', 'alice@example.com', '555-0101', '2025-08-15', 'Catering Service', '2024-11-01', 8000.00, 150, 'Confirmed', 'Include a chocolate fountain.', 'Classic Romance', 'The Grand Ballroom', 'High', '9 months', 1);
GO

-- 4. Payments
-- Assuming BookingId 1 for Alice's main package, BookingId 4 for Alice's catering. BookingId 2 for Bob.
INSERT INTO [dbo].[Payments] ([Amount], [PaidOn], [Method], [BookingId])
VALUES
(10000.00, '2024-10-15', 'Credit Card', 1),
(5000.00, '2025-02-01', 'Bank Transfer', 1),
(4000.00, '2024-11-10', 'Credit Card', 4), -- Payment for Alice's catering booking
(7500.00, '2025-01-30', 'PayPal', 2);
GO

-- 5. Agendas
-- For WeddingId 1 (Alice & Alex)
INSERT INTO [dbo].[Agendas] ([Title], [Description], [StartTime], [EndTime], [Location], [WeddingId], [CreatedAt], [UpdatedAt])
VALUES
('Wedding Day Schedule - Alice & Alex', 'Full day timeline for Alice & Alex''s wedding.', '2025-08-15 10:00:00', '2025-08-15 23:00:00', 'The Grand Ballroom & Surrounds', 1, GETDATE(), NULL),
('Rehearsal Dinner Plan', 'Schedule for the rehearsal dinner.', '2025-08-14 18:00:00', '2025-08-14 21:00:00', 'The Grand Ballroom - West Wing', 1, GETDATE(), NULL);
GO

-- 6. AgendaItems
-- For WeddingId 1 (Alice & Alex)
INSERT INTO [dbo].[AgendaItems] ([Time], [Event], [Location], [Responsible], [Notes], [WeddingId])
VALUES
('10:00 AM', 'Bridal Party Hair & Makeup', 'Bridal Suite, The Grand Ballroom', 'GlamSquad Stylists', 'Ensure all items are ready.', 1),
('01:00 PM', 'Groom & Groomsmen Photos', 'Hotel Gardens', 'Lead Photographer', 'Candid and posed shots.', 1),
('02:00 PM', 'Ceremony Starts', 'Main Hall, The Grand Ballroom', 'Officiant Michael', 'Music cues ready.', 1),
('03:00 PM', 'Cocktail Hour', 'Terrace, The Grand Ballroom', 'Catering Manager', 'Signature cocktails available.', 1),
('04:30 PM', 'Reception Dinner', 'Main Hall, The Grand Ballroom', 'MC & Catering Team', 'Speeches after main course.', 1),
('07:00 PM', 'First Dance', 'Main Hall, The Grand Ballroom', 'DJ Smooth', 'Spotlight ready.', 1);
GO

-- 7. GuestCategories
-- For WeddingId 1 (Alice & Alex) and WeddingId 2 (Bob & Bella)
INSERT INTO [dbo].[GuestCategories] ([Name], [Count], [WeddingId])
VALUES
('Bride''s Family', 40, 1),
('Groom''s Family', 35, 1),
('Friends of the Couple', 70, 1),
('Work Colleagues', 5, 1),
('Bride''s Side', 30, 2),
('Groom''s Side', 30, 2),
('Mutual Friends', 20, 2);
GO

-- 8. Notifications
-- For UserId 1 (Alice) and UserId 3 (Admin Charlie)
INSERT INTO [dbo].[Notifications] ([Message], [SentAt], [IsRead], [UserId])
VALUES
('Your payment of $10000.00 has been confirmed.', '2024-10-15 10:00:00', 1, 1),
('New booking request from Clara & Chris.', '2025-05-20 09:30:00', 0, 3), -- Admin notification
('Reminder: Wedding dress final fitting next week.', GETDATE(), 0, 1),
('A new review has been submitted for "Diana''s Floral Designs".', GETDATE(), 0, 3); -- Admin notification
GO

-- 9. Reviews
-- Assuming Diana (UserId=4) is a vendor. User Alice (UserId=1) reviews Diana.
-- The schema has VendorId, which is unusual as there's no Vendor table.
-- I'll assume VendorId refers to the UserId of a user with a 'Vendor' role (like Diana, UserId=4).
INSERT INTO [dbo].[Reviews] ([Rating], [Comment], [CreatedAt], [VendorId], [UserId])
VALUES
(5, 'Diana''s floral arrangements were absolutely stunning! Exceeded all expectations.', GETDATE(), 4, 1), -- Alice reviews Diana
(4, 'Great service, very professional photography by PhotoPro Inc (represented by Vendor UserID 4 for sample).', '2025-09-25', 4, 2); -- Bob reviews Diana (acting as a generic vendor for this sample)
GO

-- 10. WeddingServices
-- For WeddingId 1 (Alice & Alex) and WeddingId 2 (Bob & Bella)
INSERT INTO [dbo].[WeddingServices] ([ServiceName], [WeddingId])
VALUES
('Full Wedding Planning', 1),
('Catering', 1),
('Photography & Videography', 1),
('Floral Design', 1),
('DJ & Entertainment', 1),
('Day-Of Coordination', 2),
('Venue Rental', 2),
('Live Band', 2),
('Catering', 2);
GO

-- 11. WeddingTasks
-- For WeddingId 1 (Alice & Alex)
INSERT INTO [dbo].[WeddingTasks] ([Title], [Description], [DueDate], [IsCompleted], [WeddingId])
VALUES
('Book Florist', 'Finalize contract with Diana''s Floral Designs.', '2025-02-01', 1, 1),
('Send Invitations', 'Mail out all wedding invitations.', '2025-05-01', 1, 1),
('Finalize Guest List & RSVP', 'Compile final guest count after RSVP deadline.', '2025-07-15', 0, 1),
('Menu Tasting', 'Attend menu tasting session with caterer.', '2025-03-10', 1, 1),
('Arrange Transportation', 'Book transport for bridal party and key guests.', '2025-06-01', 0, 1),
('Confirm Band Playlist', 'Finalize song list with the live band.', '2025-08-01', 1, 2);
GO

-- __EFMigrationsHistory (Typically managed by Entity Framework, but adding a sample if needed manually)
-- Check your actual migration history if you are using EF Core.
-- This is just a placeholder.
IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = 'InitialCreatePlaceholder')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('InitialCreatePlaceholder', '7.0.0'); -- Replace with your EF Core version if applicable
END
GO

GO

PRINT 'Sample data insertion complete.';
GO