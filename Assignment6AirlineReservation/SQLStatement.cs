using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6AirlineReservation
{
    internal class SQLStatement
    {
        /// <summary>
        /// SQL statement for getting flight information.
        /// </summary>
        /// <returns></returns>
        public static string GetFlights()
        {
            return "SELECT Flight_ID, Flight_Number, Aircraft_Type FROM FLIGHT;";
        }

        /// <summary>
        /// SQL statement for getting passenger information.
        /// </summary>
        /// <param name="selected_flight"></param>
        /// <returns></returns>
        public static string GetPassengers(string selected_flight)
        {
            return "SELECT Passenger.Passenger_ID, First_Name, Last_Name, FPL.Seat_Number " +
                              "FROM Passenger, Flight_Passenger_Link FPL " +
                              "WHERE Passenger.Passenger_ID = FPL.Passenger_ID AND " +
                              "Flight_ID = " + selected_flight + ";";
        }

        /// <summary>
        /// SQL statement for inserting passengers.
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <returns></returns>
        public static string InsertPassenger(string FirstName, string LastName)
        {
            return "INSERT INTO PASSENGER(First_Name, Last_Name) VALUES('"+FirstName+"','"+LastName+"');";
        }

        /// <summary>
        /// SQL statement for deleting passengers.
        /// </summary>
        /// <param name="PassengerID"></param>
        /// <returns></returns>
        public static string DeletePassenger(int PassengerID)
        {
            return "DELETE FROM PASSENGER WHERE PASSENGER_ID = "+PassengerID;
        }

        /// <summary>
        /// Inserts the data into the link
        /// </summary>
        /// <param name="FlightID"></param>
        /// <param name="PassengerID"></param>
        /// <param name="SeatNumber"></param>
        /// <returns></returns>
        public static string InsertLink(int FlightID, int PassengerID, int SeatNumber)
        {
            return "INSERT INTO Flight_Passenger_Link(Flight_ID, Passenger_ID, Seat_Number) " + 
                    "VALUES( "+FlightID+" , "+PassengerID+" , "+SeatNumber+");";
        }

        /// <summary>
        /// Deletes the data from the link.
        /// </summary>
        /// <param name="FlightID"></param>
        /// <param name="PassengerID"></param>
        /// <returns></returns>
        public static string DeleteLink(int FlightID, int PassengerID)
        {
            return "Delete FROM FLIGHT_PASSENGER_LINK WHERE FLIGHT_ID = "+FlightID+" AND PASSENGER_ID = "+PassengerID+";";
        }

        /// <summary>
        /// Gets the id of the passenger with flightid and seat number.
        /// </summary>
        /// <param name="Seat_Number"></param>
        /// <param name="FlightID"></param>
        /// <returns></returns>
        public static string GetPassengerID(int Seat_Number, int FlightID)
        {
            return "SELECT Passenger_ID FROM FLIGHT_PASSENGER_LINK WHERE Seat_Number = '"+Seat_Number+"' AND Flight_ID = "+FlightID+";";
        }

        /// <summary>
        /// Gets the last inserted id.
        /// </summary>
        /// <returns></returns>
        public static string GetLastInsertedID()
        {
            return "SELECT @@IDENTITY;";
        }

        /// <summary>
        /// Gets the first and last name of a passenger.
        /// </summary>
        /// <param name="PassengerID"></param>
        /// <returns></returns>
        public static string GetName(int PassengerID)
        {
            return "SELECT First_Name, Last_Name " +
                    "FROM Passenger " +
                    "WHERE Passenger_ID = " + PassengerID + ";";
        }
    }
}
