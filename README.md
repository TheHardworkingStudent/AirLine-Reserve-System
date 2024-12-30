# Airline Flight Reservation System
![Screenshot 2024-12-30 153045](https://github.com/user-attachments/assets/a8fbc894-800a-4300-bac0-11874638307c)
![Screenshot 2024-12-30 153104](https://github.com/user-attachments/assets/bdb179ee-185c-49ff-a309-039d1f843ae6)
## Description

The Airline Flight Reservation System is a WPF-based application designed to manage and visualize flight reservations. This initial implementation provides the foundation for a fully functional reservation system. Users can select flights, view seat availability, and manage passengers through a graphical interface. The application integrates with a Microsoft Access database to store flight and passenger data, with strict separation of business logic from the UI.

This project demonstrates data binding, database interaction, and the use of WPF for creating dynamic and user-friendly GUIs.
Features
1. Main GUI

    Flight Selection:
        A ComboBox allows users to choose from two flights.
        Flights are retrieved from the Flight table in the database and displayed with both the flight number and aircraft type.
        When a flight is selected:
            The seating arrangement is displayed on the left.
            The passenger list for the selected flight is loaded into the Choose Passenger combo box.
    Seating Arrangement:
        A graphical representation of the flight's seating arrangement using a canvas and labels.
        Seat colors indicate:
            Blue: Empty seat.
            Red: Taken seat.
            Green: Selected passenger's seat.

2. Passenger Management

    Add Passenger:
        Opens a new form for entering the passenger’s first and last name.
        Includes "Save" and "Cancel" buttons (initially non-functional, closing the form only).
    Delete Passenger: (Planned functionality)
        Select a passenger and remove them from the flight.
        Updates the database and seat colors dynamically.
    Change Seat: (Planned functionality)
        Allows moving a selected passenger to a different available seat.

3. Data Integration

    Database Connectivity:
        Flights and passengers are retrieved from a Microsoft Access database.
        Data is bound to combo boxes using a List<clsFlight> for flights and a List<PassengerData> for passengers.
    Data Validation:
        Ensures proper flight and passenger data is loaded before enabling form controls.
    Data Updates:
        Adding passengers updates the database and reflects changes in the UI.

4. Exception Handling

    All methods include exception handling:
        Top-level methods handle and display user-friendly error messages.
        Lower-level methods raise exceptions to calling methods.

How to Use

    Select a Flight:
        Open the application and choose a flight from the Choose Flight combo box.
        The seating arrangement and passenger list for the flight will load automatically.

    Add a Passenger:
        Click the "Add Passenger" button to open the Add Passenger form.
        Enter the passenger's first and last name and click "Save" (non-functional in this phase).

    Manage Seats:
        View the seating arrangement with color-coded seats.
        Planned functionality includes assigning seats, deleting passengers, and changing seat assignments.

Planned Features

    Enable functionality for:
        Adding passengers to the database and assigning them seats.
        Deleting passengers and freeing up seats.
        Changing passenger seat assignments dynamically.
    Improve seat allocation logic to ensure proper data synchronization.

Class Design
clsFlight

    Holds flight data retrieved from the database, such as:
        FlightNumber
        AircraftType
        Seats

PassengerData

    Holds passenger data retrieved from the database, such as:
        FirstName
        LastName
        SeatNumber

FlightManager

    Retrieves and manages flight data from the database.
    Example method: GetFlights() returns a List<clsFlight>.

PassengerManager

    Retrieves and manages passenger data from the database.
    Example method: GetPassengers() returns a List<PassengerData>.

Error Handling

    Flight and Passenger Loading:
        Prevents crashes if the database is unavailable or data is incomplete.
    Seat Selection:
        Ensures a seat cannot be double-booked.
    Database Errors:
        Displays appropriate messages for database connection issues.

Requirements

    Environment:
        Microsoft Access database for flight and passenger data.
        .NET Framework or .NET Core (WPF).
    Development Tools:
        Visual Studio with "System.Data.OleDb" NuGet package for database connectivity.
    Platform Compatibility:
        For 64-bit systems, set the platform target to x86 in project settings.

Installation

    Database Setup:
        Ensure the Microsoft Access database is in the project folder.
        Use the provided database schema for Flight and Passenger tables.

    NuGet Package Installation:
        Install System.Data.OleDb via the NuGet package manager.

    Project Configuration:
        Set the platform target to x86 if on a 64-bit system.

Future Improvements

    Implement full functionality for "Add Passenger," "Delete Passenger," and "Change Seat" buttons.
    Enhance seat allocation logic for division of rows (e.g., window/aisle seats).
    Add additional error handling for edge cases like database corruption.

