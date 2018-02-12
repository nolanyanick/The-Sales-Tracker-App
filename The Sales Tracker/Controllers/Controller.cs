using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace The_Sales_Tracker
{
    /// <summary>
    /// MVC Controller class
    /// </summary>
    public class Controller
    {
        #region FIELDS

        private bool _usingApplication;

        //
        // declare ConsoleView and Salesperson objects for the Controller to use
        // Note: There is no need for a Salesperson or ConsoleView property given only the Controller 
        //       will access the ConsoleView object and will pass the Salesperson object to the ConsoleView.
        //
        private ConsoleView _consoleView;
        private Salesperson _salesperson;

        #endregion

        #region PROPERTIES


        #endregion
        
        #region CONSTRUCTORS

        public Controller()
        {
            InitializeController();

            //
            // instantiate a Salesperson object
            //
            _salesperson = new Salesperson();

            //
            // instantiate a ConsoleView object
            //
            _consoleView = new ConsoleView();

            //
            // begins running the application UI
            //
            ManageApplicationLoop();
        }

        #endregion
        
        #region METHODS

        /// <summary>
        /// initialize the controller 
        /// </summary>
        private void InitializeController()
        {
            _usingApplication = true;
        }

        /// <summary>
        /// method to manage the application setup and control loop
        /// </summary>
        private void ManageApplicationLoop()
        {
            MenuOption userMenuChoice;

            _consoleView.DisplayWelcomeScreen();

            // application loop
            //
            while (_usingApplication)
            {

                //
                // get a menu choice from the ConsoleView object
                //
                userMenuChoice = _consoleView.DisplayGetUserMenuChoice();

                //
                // choose an action based on the user's menu choice
                //
                switch (userMenuChoice)
                {
                    case MenuOption.None:
                        break;
                    case MenuOption.SetupAccount:
                        DisplaySetupAccount();
                        break;
                    case MenuOption.UpdateAccount:
                        DisplayUpdateAccount();
                        break;
                    case MenuOption.Travel:
                        Travel();
                        break;
                    case MenuOption.Buy:
                        Buy();
                        break;
                    case MenuOption.Sell:
                        Sell();
                        break;
                    case MenuOption.DisplayInventory:
                        DisplayInventory();
                        break;
                    case MenuOption.DisplayCities:
                        DisplayCities();
                        break;
                    case MenuOption.DisplayAccountInfo:
                        DisplayAccountInfo();
                        break;
                    case MenuOption.LoadAccountInfo:
                        DisplayLoadAccountInfo();
                        break;
                    case MenuOption.SaveAccountInfo:
                        DisplaySaveAccountInfo();
                        break;                        
                    case MenuOption.Exit:
                        _usingApplication = false;
                        break;
                    default:
                        break;
                }
            }

            _consoleView.DisplayClosingScreen();

            //
            // close the application
            //
            Environment.Exit(1);
        }

        /// <summary>
        /// add the next city location to the list of cities
        /// </summary>
        private void Travel()
        {
            string nextCity = _consoleView.DisplayGetNextCity();

            //
            // do not add empty strings to list for city names
            //
            if (nextCity != "")
            {
                _salesperson.CitiesVisited.Add(nextCity);
            }
        }

        /// <summary>
        /// display all cities traveled to
        /// </summary>
        private void DisplayCities()
        {
            _consoleView.DisplayCitiesTraveled(_salesperson);
        }

        /// <summary>
        /// display account information
        /// </summary>
        private void DisplayAccountInfo()
        {
            _consoleView.DisplayAccountInfo(_salesperson);
        }

        /// <summary>
        /// purchase products
        /// </summary>
        private void Buy()
        {
            int numberOfUnits = _consoleView.DisplayGetNumberUnitsToBuy(_salesperson.CurrentStock);
            _salesperson.CurrentStock.AddProducts(numberOfUnits);
        }

        /// <summary>
        /// sell products
        /// </summary>
        private void Sell()
        {
            int numberOfUnits = _consoleView.DisplayGetNumberUnitsToSell(_salesperson.CurrentStock);
            _salesperson.CurrentStock.SubtractProducts(numberOfUnits);

            if (_salesperson.CurrentStock.OnBackorder)
            {
                _consoleView.DisplayBackorderNotification(_salesperson.CurrentStock, numberOfUnits);
            }
        }

        /// <summary>
        /// display inventory
        /// </summary>
        private void DisplayInventory()
        {
            _consoleView.DisplayInventory(_salesperson, _salesperson.CurrentStock);
        }

        /// <summary>
        /// setup account
        /// </summary>
        private void DisplaySetupAccount()
        {
            _salesperson = _consoleView.DisplaySetupAccount();
        }

        /// <summary>
        /// update account
        /// </summary>
        private void DisplayUpdateAccount()
        {
            bool maxAttemptsExceeded = false;
            _salesperson = _consoleView.DisplayUpdateAcountInfo(_salesperson, out maxAttemptsExceeded);
        }

        /// <summary>
        /// load account info and trvael log
        /// </summary>
        private void DisplayLoadAccountInfo()
        {
            bool maxAttemptsExceeded = false;
            bool loadAccountInfo = false;

            if (_salesperson.AccountID == null)
            {
                loadAccountInfo = _consoleView.DisplayLoadAccountInfo(out maxAttemptsExceeded);
            }
            else
            {
                loadAccountInfo = _consoleView.DisplayLoadAccountInfo(_salesperson, out maxAttemptsExceeded);
            }

            if (loadAccountInfo && !maxAttemptsExceeded)
            {
                CsvServices csvServices = new CsvServices(DataSettings.dataFilePathCsv);

                _salesperson = csvServices.ReadSalespersonFromDataFile();

                _consoleView.DisplayConfirmLoadAccountInfo(_salesperson);
            }
        }

        /// <summary>
        /// save account info and trvael log
        /// </summary>
        private void DisplaySaveAccountInfo()
        {
            bool maxAttemptsExceeded = false;
            bool saveAccountInfo = false;

            saveAccountInfo = _consoleView.DisplaySaveAccountInfo(_salesperson, out maxAttemptsExceeded);


            if (saveAccountInfo && !maxAttemptsExceeded)
            {
                CsvServices csvServices = new CsvServices(DataSettings.dataFilePathCsv);

                csvServices.WriteSalespersonToDataFile(_salesperson);

                _consoleView.DisplayConfirmSaveAccountInfo();
            }
        }

        #endregion
    }
}
