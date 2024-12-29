using Assignment6;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Add passenger window
        /// </summary>
        wndAddPassenger wndAddPass;
        /// <summary>
        /// An instance of flight manager that manages flights and is the business logic.
        /// </summary>
        FlightManager Flight;
        /// <summary>
        /// Keeps track of the number of rows in the grid.
        /// </summary>
        int num_rows = 0;
        /// <summary>
        /// Keeps track of the number of columns in the grid.
        /// </summary>
        int num_columns = 0;
        /// <summary>
        /// Serves as the base name string which will be used for finding xaml elements.
        /// </summary>
        string BaseName = "";
        /// <summary>
        /// Stops the combo box changed event from firing if false.
        /// </summary>
        bool select_changed_disabled = false;
        /// <summary>
        /// Stops the change seat event from firing if false.
        /// </summary>
        bool change_seat_event = false;
        public MainWindow()
        {
            try
            {
                Flight = new FlightManager();

                InitializeComponent();
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Flight.LoadFlightData(ref cbChooseFlight);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event for when the choose flight was changed, and loads up a new grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbChooseFlight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cbChoosePassenger.IsEnabled = true;
                gPassengerCommands.IsEnabled = true;

                Flight.LoadPassengerData(ref cbChooseFlight, ref cbChoosePassenger, ref select_changed_disabled);

                //Should be using a flight object to get the flight ID here
                if (cbChooseFlight.SelectedIndex == 0)
                {
                    Canvas767.Visibility = Visibility.Hidden;
                    CanvasA380.Visibility = Visibility.Visible;
                    num_rows = 5;
                    num_columns = 3;
                    BaseName = "A380";
                    lblPassengersSeatNumber.Content = "";
                    Flight.CreateSeatStatusArray(num_rows, num_columns);
                    Flight.UpdateArray();
                    Update_Grid();
                }
                else if (cbChooseFlight.SelectedIndex == 1)
                {
                    CanvasA380.Visibility = Visibility.Hidden;
                    Canvas767.Visibility = Visibility.Visible;
                    num_rows = 4;
                    num_columns = 4;
                    BaseName = "B767";
                    lblPassengersSeatNumber.Content = "";
                    Flight.CreateSeatStatusArray(num_rows, num_columns);
                    Flight.UpdateArray();
                    Update_Grid();
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Changes the selected passenger on the grid and in the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbChoosePassenger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(select_changed_disabled == false)
                {
                    lblPassengersSeatNumber.Content = Flight.RetrievePassengerData(cbChoosePassenger.SelectedIndex, "Seat_Number");
                    int index = Flight.LookupValue(-1, cbChoosePassenger.SelectedIndex);
                    int row = ((index - 1) / num_columns) + 1;
                    int column = ((index - 1) % num_columns) + 1;

                    Flight.UpdateArray();
                    Flight.SetSeatStatus(row - 1, column - 1, 2);
                    Update_Grid();
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Opens a new box to add a passenger.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddPassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndAddPass = new wndAddPassenger(int.Parse(lblPassengersSeatNumber.Content.ToString()), (cbChooseFlight.SelectedIndex+1),ref cbChooseFlight,ref cbChoosePassenger,ref Flight, ref select_changed_disabled);
                wndAddPass.ShowDialog();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Attempts to delete a passenger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeletePassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Flight.DeletePassengerData(int.Parse(lblPassengersSeatNumber.Content.ToString()), (cbChooseFlight.SelectedIndex + 1));
                Flight.LoadPassengerData(ref cbChooseFlight, ref cbChoosePassenger, ref select_changed_disabled);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// turns on the change seat event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdChangeSeat_Click(object sender, RoutedEventArgs e)
        {
            if(lblPassengersSeatNumber.Content.ToString() != "")
            {
                cbChooseFlight.IsEnabled = false;
                cbChoosePassenger.IsEnabled = false;
                cmdAddPassenger.IsEnabled = false;
                cmdDeletePassenger.IsEnabled = false;
                change_seat_event = true;
            }
        }

        /// <summary>
        /// Reads an array to update the colors of the grid.
        /// </summary>
        private void Update_Grid()
        {
            try
            {
                string LabelName = "";
                for (int i = 0; i < num_rows; i++)
                {
                    for (int j = 0; j < num_columns; j++)
                    {
                        LabelName = BaseName + "_" + i + "_" + j; // add a numbering system to the grid
                        System.Windows.Controls.Label element = this.FindName(LabelName) as System.Windows.Controls.Label;
                        if (Flight.GetSeatStatus(i, j) == 0)
                        {
                            element.Background = new SolidColorBrush(Colors.Blue);
                        }
                        else if (Flight.GetSeatStatus(i, j) == 1)
                        {
                            element.Background = new SolidColorBrush(Colors.Red);
                        }
                        else if (Flight.GetSeatStatus(i, j) == 2)
                        {
                            element.Background = new SolidColorBrush(Colors.Green);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// <para>Handles tile click.</para>
        /// <para>Also can change who the selected customer is.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {

                System.Windows.Controls.Label label = (System.Windows.Controls.Label)sender;
                int index = int.Parse(label.Content.ToString());
                int row = ((index - 1) / num_columns) + 1;
                int column = ((index - 1) % num_columns) + 1;

                if (change_seat_event == true)
                {
                    //delete the passenger data
                    Flight.DeletePassengerData(int.Parse(lblPassengersSeatNumber.Content.ToString()), (cbChooseFlight.SelectedIndex + 1));
                    Flight.LoadPassengerData(ref cbChooseFlight, ref cbChoosePassenger, ref select_changed_disabled);
                }

                select_changed_disabled = true;
                cbChoosePassenger.SelectedIndex = Flight.LookupValue(int.Parse(label.Content.ToString()));
                select_changed_disabled = false;
                lblPassengersSeatNumber.Content = label.Content.ToString();

                Flight.UpdateArray();
                Flight.SetSeatStatus(row - 1, column - 1, 2);
                Update_Grid();

                if(change_seat_event == true)
                {
                    //save passenger data
                    Flight.SavePassengerData(Flight.GetDeletedFirstName(), Flight.GetDeletedLastName(), int.Parse(lblPassengersSeatNumber.Content.ToString()), (cbChooseFlight.SelectedIndex + 1));
                    Flight.LoadPassengerData(ref cbChooseFlight, ref cbChoosePassenger, ref select_changed_disabled);
                    cbChooseFlight.IsEnabled = true;
                    cbChoosePassenger.IsEnabled = true;
                    cmdAddPassenger.IsEnabled = true;
                    cmdDeletePassenger.IsEnabled = true;
                    change_seat_event = false;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Reset some of the game stats to fix errors.
        /// </summary>
        private void ResetGame()
        {
            select_changed_disabled = false;
            change_seat_event = false;

            Flight.CreateSeatStatusArray(num_rows, num_columns);
            Flight.UpdateArray();
            Update_Grid();
        }

        /// <summary>
        /// Prints error message and does some error handling
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
                ResetGame();
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}
