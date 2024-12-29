using Assignment6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Assignment6AirlineReservation
{
    /// <summary>
    /// Interaction logic for wndAddPassenger.xaml
    /// </summary>
    public partial class wndAddPassenger : Window
    {
        /// <summary>
        /// Keeps track of the sleected seat.
        /// </summary>
        int SelectedSeat = 0;

        /// <summary>
        /// Keeps track of selected flight.
        /// </summary>
        int SelectedFlight = 0;

        /// <summary>
        /// Stops the combo box event from firing.
        /// </summary>
        bool select_changed_disabled = false;

        /// <summary>
        /// Creates a reference to flight.
        /// </summary>
        FlightManager Flight;

        /// <summary>
        /// Creates a way to refer to the flight combo box
        /// </summary>
        ComboBox cbChooseFlight;

        /// <summary>
        /// Creates a way to refer to the passenger combo box.
        /// </summary>
        ComboBox cbChoosePassenger;

        /// <summary>
        /// constructor for the add passenger window
        /// </summary>
        public wndAddPassenger(int SelectedSeat, int SelectedFlight, ref ComboBox cbChooseFlight, ref ComboBox cbChoosePassenger, ref FlightManager Flight,ref bool select_changed_disabled)
        {
            try
            {
                this.SelectedSeat = SelectedSeat;
                this.SelectedFlight = SelectedFlight;
                this.cbChooseFlight = cbChooseFlight;
                this.cbChoosePassenger = cbChoosePassenger;
                this.Flight = Flight;
                this.select_changed_disabled = select_changed_disabled;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// only allows letters to be input
        /// </summary>
        /// <param name="sender">sent object</param>
        /// <param name="e">key argument</param>
        private void txtLetterInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Only allow letters to be entered
                if (!(e.Key >= Key.A && e.Key <= Key.Z))
                {
                    //Allow the user to use the backspace, delete, tab and enter
                    if (!(e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab || e.Key == Key.Enter))
                    {
                        //No other keys allowed besides numbers, backspace, delete, tab, and enter
                        e.Handled = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// exception handler that shows the error
        /// </summary>
        /// <param name="sClass">the class</param>
        /// <param name="sMethod">the method</param>
        /// <param name="sMessage">the error message</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// Saves the data when clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            Flight.SavePassengerData(txtFirstName.Text, txtLastName.Text, SelectedSeat,SelectedFlight);
            Flight.LoadPassengerData(ref cbChooseFlight, ref cbChoosePassenger, ref select_changed_disabled);
            this.Close();
        }
    }
}
