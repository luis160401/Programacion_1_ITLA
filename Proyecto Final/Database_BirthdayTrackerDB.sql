-- ============================================================
-- BirthdayTracker - Script de Base de Datos
-- SQL Server Express 2022
-- Autor: Luis Angel Guillen
-- ============================================================

USE master;
GO

-- Crear base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'BirthdayTrackerDB')
BEGIN
    CREATE DATABASE BirthdayTrackerDB;
    PRINT 'Base de datos BirthdayTrackerDB creada.';
END
GO

USE BirthdayTrackerDB;
GO

-- ── Tabla Contact ─────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Contact' AND xtype='U')
BEGIN
    CREATE TABLE Contact (
        ContactId   INT           IDENTITY(1,1) PRIMARY KEY,
        FirstName   NVARCHAR(100) NOT NULL,
        LastName    NVARCHAR(100) NOT NULL,
        Nickname    NVARCHAR(50)  NULL,
        BirthDate   DATE          NOT NULL,
        Phone       NVARCHAR(20)  NULL,
        Email       NVARCHAR(150) NULL,
        CreatedAt   DATETIME2     NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Tabla Contact creada.';
END
GO

-- ── Tabla BirthdayNote ────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BirthdayNote' AND xtype='U')
BEGIN
    CREATE TABLE BirthdayNote (
        NoteId      INT            IDENTITY(1,1) PRIMARY KEY,
        ContactId   INT            NOT NULL,
        NoteText    NVARCHAR(500)  NOT NULL,
        CreatedAt   DATETIME2      NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Note_Contact FOREIGN KEY (ContactId)
            REFERENCES Contact(ContactId) ON DELETE CASCADE
    );
    PRINT 'Tabla BirthdayNote creada.';
END
GO

-- ── Tabla ReminderLog ─────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ReminderLog' AND xtype='U')
BEGIN
    CREATE TABLE ReminderLog (
        LogId        INT            IDENTITY(1,1) PRIMARY KEY,
        ContactId    INT            NOT NULL,
        ReminderDate DATE           NOT NULL,
        Message      NVARCHAR(300)  NOT NULL,
        CreatedAt    DATETIME2      NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Log_Contact FOREIGN KEY (ContactId)
            REFERENCES Contact(ContactId) ON DELETE CASCADE
    );
    PRINT 'Tabla ReminderLog creada.';
END
GO

-- ── Datos de prueba ───────────────────────────────────────────
IF NOT EXISTS (SELECT TOP 1 1 FROM Contact)
BEGIN
    INSERT INTO Contact (FirstName, LastName, Nickname, BirthDate, Phone, Email) VALUES
        ('Maria',   'Garcia',    'Mami',      '1975-04-25', '809-555-0001', 'maria@mail.com'),
        ('Carlos',  'Guillen',   'Carlitos',  '1998-06-15', '809-555-0002', NULL),
        ('Ana',     'Martinez',  NULL,        '2000-11-03', '809-555-0003', 'ana@mail.com'),
        ('Pedro',   'Rodriguez', 'Pete',      '1990-01-20', NULL,           NULL),
        ('Lucia',   'Fernandez', NULL,        '1985-07-08', '809-555-0005', 'lucia@mail.com'),
        ('Miguel',  'Lopez',     'Miguelito', '2002-12-31', '809-555-0006', NULL),
        ('Sofia',   'Torres',    NULL,        '1995-05-01', NULL,           'sofia@mail.com'),
        ('Diego',   'Reyes',     NULL,        '1988-09-14', '809-555-0008', NULL);
    PRINT 'Datos de prueba insertados.';
END
GO

PRINT '✓ Script ejecutado correctamente. Base de datos lista.';
GO
