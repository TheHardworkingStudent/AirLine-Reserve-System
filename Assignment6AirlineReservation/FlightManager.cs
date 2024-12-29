using Assignment6AirlineReservation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;

namespace Assignment6
{
    public class FlightManager
    {
        /// <summary>
        /// Holds the passenger data.
        /// </summary>
        private FlightData PassengerData;

        /// <summary>
        /// Holds the flight data.
        /// </summary>
        private FlightData FlightData;

        /// <summary>
        /// <para>Is the array that keeps track of what seat is taken and not.</para>
        /// <para>0 = empty, 1 = seat taken, 2 = seat selected</para>
        /// </summary>
        private int[,] SeatStatus; // 0 = empty 1 = seat taken 2 = seat selected

        /// <summary>
        /// This table is for lookup between the selected index and the seat value.
        /// </summary>
        private int[] AssociationTable;

        /// <summary>
        /// Keeps track of the last deleted firstname
        /// </summary>
        private string DeletedFirstName;

        /// <summary>
        /// Keeps trakc of the last deleted lastname
        /// </summary>
        private string DeletedLastName;

        /// <summary>
        /// Creates and instance of the data classes.
        /// </summary>
        clsDataAccess clsData;

        /// <summary>
        /// Initialize the data Passenger, and Flight data.
        /// </summary>
        public FlightManager()
        {
            PassengerData = new FlightData();
            FlightData = new FlightData();
        }

        /// <summary>
        /// <para>Calls the function that makes a connection to the database to retrieve the data.</para>
        /// </summary>
        /// <param name="selected_index"></param>
        public void LoadPassengerData(ref ComboBox cbChooseFlight, ref ComboBox cbChoosePassenger, ref bool select_changed_disabled)
        {
            try
            {
                select_changed_disabled = true;
                cbChoosePassenger.Items.Clear();
                select_changed_disabled = false;
                DataSet ds = new DataSet();
                int iRet = 0;
                //I think this should be in a new class to hold SQL statments
                string sSQL = SQLStatement.GetPassengers((cbChooseFlight.SelectedIndex+1).ToString());//If the cbChooseFlight was bound to a list of Flights, the selected object would have the flight ID
                //Probably put in a new class
                ds = clsData.ExecuteSQLStatement(sSQL, ref iRet);
                PassengerData.TableOfData = ds.Tables[0];

                cbChoosePassenger.Items.Clear();//Don't need if assigning a list of passengers to the combo box

                //Would be nice if code from another class executed the SQL above, added each passenger into a Passenger object,
                //then into a list of Passengers to be returned and bound to the combo box
                for (int i = 0; i < iRet; i++)
                {
                    cbChoosePassenger.Items.Add(ds.Tables[0].Rows[i][1] + " " + ds.Tables[0].Rows[i][2]);
                }
                BuildAssociationTable();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// <para>Calls the function that makes a connection to the database to retrieve the data.</para>
        /// </summary>
        public void LoadFlightData(ref ComboBox cbChooseFlight)
        {
            try
            {
                DataSet ds = new DataSet();
                //Should probably not have SQL statements behind the UI
                string sSQL = SQLStatement.GetFlights();
                int iRet = 0;
                clsData = new clsDataAccess();

                //This should probably be in a new class.  Would be nice if this new class
                //returned a list of Flight objects that was then bound to the combo box
                //Also should show the flight number and aircraft type together
                ds = clsData.ExecuteSQLStatement(sSQL, ref iRet);
                FlightData.TableOfData = ds.Tables[0];

                //Should probably bind a list of flights to the combo box
                string RowString = "";
                for (int i = 0; i < iRet; i++)
                {
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        RowString = ds.Tables[0].Rows[i][j].ToString();
                    }
                    cbChooseFlight.Items.Add(RowString);
                    RowString = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// <para>Saves the passenger data to the database</para>
        /// <para>Starts by saving passenger then inserting link.</para>
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="selected_seat"></param>
        /// <param name="selected_flight"></param>
        /// <exception cref="Exception"></exception>
        public void SavePassengerData(string FirstName, string LastName, int selected_seat, int selected_flight)
        {
            try
            {
                string SQLInsertPassenger = SQLStatement.InsertPassenger(FirstName,LastName);
                clsData = new clsDataAccess();

                int LastID = clsData.ExecuteInsertAndGetIdentity(SQLInsertPassenger);
                string SQLInsertLink = SQLStatement.InsertLink(selected_flight, LastID, selected_seat);
                clsData.ExecuteNonQuery(SQLInsertLink);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// <para>deletes the passenger data from the database.</para>
        /// <para>starts by deleting link then deleting passenger.</para>
        /// </summary>
        /// <param name="selected_seat"></param>
        /// <param name="FlightID"></param>
        /// <exception cref="Exception"></exception>
        public void DeletePassengerData(int selected_seat,int FlightID)
        {
            try
            {
                int iRet = 0;
                string SQLPassengerID = SQLStatement.GetPassengerID(selected_seat,FlightID);
                clsData = new clsDataAccess();

                DataSet ds = new DataSet();
                ds = clsData.ExecuteSQLStatement(SQLPassengerID, ref iRet);
                int PassengerID = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                string SQLGetName = SQLStatement.GetName(PassengerID);
                ds = clsData.ExecuteSQLStatement(SQLGetName, ref iRet);
                DeletedFirstName = ds.Tables[0].Rows[0][0].ToString();
                DeletedLastName = ds.Tables[0].Rows[0][1].ToString();
                string SQLDeleteLink = SQLStatement.DeleteLink(FlightID, PassengerID);
                clsData.ExecuteNonQuery(SQLDeleteLink);
                string SQLDeletePassenger = SQLStatement.DeletePassenger(PassengerID);
                clsData.ExecuteNonQuery(SQLDeletePassenger);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Builds the Association Table.
        /// </summary>
        private void BuildAssociationTable()
        {
            try
            {
                int num_rows = PassengerData.TableOfData.Rows.Count;
                int num_columns = PassengerData.TableOfData.Columns.Count;
                AssociationTable = new int[num_rows];

                for (int i = 0; i < num_rows; i++)
                {
                    AssociationTable[i] = int.Parse(PassengerData.TableOfData.Rows[i]["Seat_Number"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Is used to look up data from the association table.
        /// </summary>
        /// <param name="ValueToLookup"></param>
        /// <param name="IndexToLookup"></param>
        /// <returns></returns>
        public int LookupValue(int ValueToLookup = -1, int IndexToLookup = -1)
        {
            try
            {
                if (ValueToLookup != -1 && IndexToLookup == -1)
                {
                    for (int i = 0; i < AssociationTable.Length; i++)
                    {
                        if (AssociationTable[i] == ValueToLookup)
                        {
                            return i;
                        }
                    }
                }
                else if (ValueToLookup == -1 && IndexToLookup != -1)
                {
                    return AssociationTable[IndexToLookup];
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Used for Retrieving data since it is private
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public Object RetrievePassengerData(int row = -1, string column = "")
        {
            try 
            {
                if (row >= 0 && column != "")
                {
                    return PassengerData.TableOfData.Rows[row][column];
                }
                return PassengerData.TableOfData;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Used for Retriving data since it is private.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public Object RetrieveFlightData(int row = -1, string column = "")
        {
            try
            {
                if (row >= 0 && column != "")
                {
                    return FlightData.TableOfData.Rows[row][column];
                }
                return FlightData.TableOfData;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Allocates memory for the SeatStatus array.
        /// </summary>
        /// <param name="num_rows"></param>
        /// <param name="num_columns"></param>
        public void CreateSeatStatusArray(int num_rows, int num_columns)
        {
            try
            {
                SeatStatus = new int[num_rows, num_columns];
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Updates the SeatStatus array.
        /// </summary>
        public void UpdateArray()
        {
            try
            {
                int num_rows = SeatStatus.GetLength(0);  // Number of rows
                int num_columns = SeatStatus.GetLength(1);  // Number of columns
                for (int i = 0; i < num_rows; i++)
                {
                    for (int j = 0; j < num_columns; j++)
                    {
                        SeatStatus[i, j] = 0;
                    }
                }

                for (int i = 0; i < PassengerData.TableOfData.Rows.Count; i++)
                {
                    if (PassengerData.TableOfData.Rows[i]["Seat_Number"].ToString() != "")
                    {
                        int index = int.Parse(PassengerData.TableOfData.Rows[i]["Seat_Number"].ToString());
                        int row = ((index - 1) / num_columns) + 1;
                        int column = ((index - 1) % num_columns) + 1;
                        SeatStatus[row - 1, column - 1] = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the SeatStatus since it is private.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public int GetSeatStatus(int row, int column)
        {
            try
            {
                return SeatStatus[row, column];
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Sets the seat status since it is private.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void SetSeatStatus(int row,int column, int value)
        {
            try
            {
                SeatStatus[row, column] = value;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the deleted first name for use in the change seat
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetDeletedFirstName()
        {
            try
            {
                return DeletedFirstName;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the deleted last name for use in the change seat.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetDeletedLastName()
        {
            try
            {
                return DeletedLastName;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
