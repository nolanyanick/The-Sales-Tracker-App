using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Sales_Tracker
{
    /// <summary>
    /// MVC View class
    /// </summary>
    public class ConsoleView
    {
        #region FIELDS

        private const int MAXIMUM_ATTEMPTS = 5;
        private const int MAXIMUM_BUYSELL_AMOUNT = 100;
        private const int MINIMUM_BUYSELL_AMOUNT = 0;

        #endregion

        #region PROPERTIES

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// default constructor to create the console view objects
        /// </summary>
        public ConsoleView()
        {
            InitializeConsole();
        }

        #endregion

        #region METHODS

        /// <summary>
        /// initialize all console settings
        /// </summary>
        private void InitializeConsole()
        {
            ConsoleUtil.WindowTitle = "Laughing Leaf Productions";
            ConsoleUtil.HeaderText = "The Traveling Salesperson Application";
        }

        /// <summary>
        /// display the Continue prompt
        /// </summary>
        public void DisplayContinuePrompt()
        {
            Console.CursorVisible = false;

            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayMessage("Press any key to continue.");
            ConsoleKeyInfo response = Console.ReadKey();

            ConsoleUtil.DisplayMessage("");

            Console.CursorVisible = true;
        }

        /// <summary>
        /// display the Exit prompt on a clean screen
        /// </summary>
        public void DisplayExitPrompt()
        {
            ConsoleUtil.DisplayReset();

            Console.CursorVisible = false;

            ConsoleUtil.DisplayMessage("");
            ConsoleUtil.DisplayMessage("Thank you for using the application. Press any key to Exit.");

            Console.ReadKey();

            System.Environment.Exit(1);
        }

        /// <summary>
        /// display the welcome screen
        /// </summary>
        public void DisplayWelcomeScreen()
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Written by Nolan Yanick");
            ConsoleUtil.DisplayMessage("Northwestern Michigan College");
            ConsoleUtil.DisplayMessage("");

            sb.Clear();
            sb.AppendFormat("You are a traveling salesperson buying and selling products ");
            sb.AppendFormat("around the country. You will be prompted regarding which city ");
            sb.AppendFormat("you wish to travel to and will then be asked whether you wish to buy ");
            sb.AppendFormat("or sell products.");
            ConsoleUtil.DisplayMessage(sb.ToString());
            ConsoleUtil.DisplayMessage("");

            sb.Clear();
            sb.AppendFormat("NOTE: Your first task will be to set up your account details.");
            ConsoleUtil.DisplayMessage(sb.ToString());

            DisplayContinuePrompt();
        }

        /// <summary>
        /// setup the new salesperson object with the initial data
        /// Note: To maintain the pattern of only the Controller changing the data this method should
        ///       return a Salesperson object with the initial data to the controller. For simplicity in 
        ///       this demo, the ConsoleView object is allowed to access the Salesperson object's properties.
        /// </summary>
        public Salesperson DisplaySetupAccount()
        {
            Salesperson salesperson = new Salesperson();
            Product.ProductType productType;
            int numberOfUnits;          

            ConsoleUtil.HeaderText = "Account Setup";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Setup your account now.");
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayPromptMessage("Enter your first name: ");
            salesperson.FirstName = Console.ReadLine();
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayPromptMessage("Enter your last name: ");
            salesperson.LastName = Console.ReadLine();
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayPromptMessage("Enter your account ID: ");
            salesperson.AccountID = Console.ReadLine();
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayPromptMessage("Enter your Starting City: ");
            salesperson.StartingCity = Console.ReadLine();
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.HeaderText = "Account Setup";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Please select which type of product you want to work with from below.");
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayMessage("Product Types:");
            ConsoleUtil.DisplayMessage("");

            Console.Write(
                "\t- Gilded" + Environment.NewLine +
                "\t- Spiked" + Environment.NewLine +
                "\t- Colorful" + Environment.NewLine +
                "\t- Vintage" + Environment.NewLine +
                "\t- Striped" + Environment.NewLine +
                "\t- Used" + Environment.NewLine);         

            ConsoleUtil.DisplayMessage("");
            ConsoleUtil.DisplayPromptMessage("Enter product selection: ");

            //
            // get product type from user
            //
            if (Enum.TryParse<Product.ProductType>(UppercaseFirst(Console.ReadLine()), out productType))
            {
                salesperson.CurrentStock.Type = productType;               
            }
            else
            {
                ConsoleUtil.DisplayReset();
                ConsoleUtil.DisplayMessage("Seems like you entered an invalid product type.");
                ConsoleUtil.DisplayMessage("By default, your product type has been set to None.");
                salesperson.CurrentStock.Type = Product.ProductType.None;                
                DisplayContinuePrompt();
            }

            //
            // get number of products in inventory
            //
            ConsoleUtil.DisplayReset();
            ConsoleUtil.DisplayMessage($"You have selected {productType} as your product type.");

            if (ConsoleValidator.TryGetIntegerFromUser(MINIMUM_BUYSELL_AMOUNT, MAXIMUM_BUYSELL_AMOUNT, MAXIMUM_ATTEMPTS, $"{productType} products to add to your inventory", out numberOfUnits))
            {
                ConsoleUtil.DisplayReset();
                salesperson.CurrentStock.AddProducts(numberOfUnits);
                ConsoleUtil.DisplayMessage($"Thank you! {numberOfUnits} {productType} products are now in your inventory!");
                DisplayContinuePrompt();
            }
            else
            {
                ConsoleUtil.DisplayReset();
                ConsoleUtil.DisplayMessage("Maximum attempts exceeded!");
                ConsoleUtil.DisplayMessage($"By default, the number of {productType} products in your inventory are now set to zero.");
                salesperson.CurrentStock.AddProducts(0);
                DisplayContinuePrompt();
            }
            
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Your account is now setup!");

            DisplayContinuePrompt();

            return salesperson;
        }

        /// <summary>
        /// display a closing screen when the user quits the application
        /// </summary>
        public void DisplayClosingScreen()
        {
            ConsoleUtil.HeaderText = "The Traveling Salesperson Appplication";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Thank you for using The Traveling Salesperson Application.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// get the menu choice from the user
        /// </summary>
        public MenuOption DisplayGetUserMenuChoice()
        {
            MenuOption userMenuChoice = MenuOption.None;
            bool usingMenu = true;

            while (usingMenu)
            {
                //
                // set up display area
                //
                ConsoleUtil.HeaderText = "Main Menu";
                ConsoleUtil.HeaderBackgroundColor = ConsoleColor.DarkYellow;
                ConsoleUtil.HeaderForegroundColor = ConsoleColor.Black;
                ConsoleUtil.BodyForegroundColor = ConsoleColor.Gray;
                ConsoleUtil.DisplayReset();
                Console.CursorVisible = false;

                //
                // display the menu
                //
                ConsoleUtil.DisplayMessage("Please type the number of your menu choice.");
                ConsoleUtil.DisplayMessage("");
                Console.Write(
                    "\t" + "0. Setup Account" + Environment.NewLine +
                    "\t" + "1. Update Account" + Environment.NewLine +
                    "\t" + "2. Travel" + Environment.NewLine +
                    "\t" + "3. Buy" + Environment.NewLine +
                    "\t" + "4. Sell" + Environment.NewLine +
                    "\t" + "5. Display Inventory" + Environment.NewLine +
                    "\t" + "6. Display Cities" + Environment.NewLine +
                    "\t" + "7. Display Account Info" + Environment.NewLine +
                    "\t" + "8. Save Account Info" + Environment.NewLine +
                    "\t" + "9. Load Account Info" + Environment.NewLine +
                    "\t" + "E. Exit" + Environment.NewLine);

                //
                // get and process the user's response
                // note: ReadKey argument set to "true" disables the echoing of the key press
                //
                ConsoleKeyInfo userResponse = Console.ReadKey(true);
                switch (userResponse.KeyChar)
                {
                    case '0':
                        userMenuChoice = MenuOption.SetupAccount;
                        usingMenu = false;
                        break;
                    case '1':
                        userMenuChoice = MenuOption.UpdateAccount;
                        usingMenu = false;
                        break;
                    case '2':
                        userMenuChoice = MenuOption.Travel;
                        usingMenu = false;
                        break;
                    case '3':
                        userMenuChoice = MenuOption.Buy;
                        usingMenu = false;
                        break;
                    case '4':
                        userMenuChoice = MenuOption.Sell;
                        usingMenu = false;
                        break;
                    case '5':
                        userMenuChoice = MenuOption.DisplayInventory;
                        usingMenu = false;
                        break;
                    case '6':
                        userMenuChoice = MenuOption.DisplayCities;
                        usingMenu = false;
                        break;
                    case '7':
                        userMenuChoice = MenuOption.DisplayAccountInfo;
                        usingMenu = false;
                        break;
                    case '8':
                        userMenuChoice = MenuOption.SaveAccountInfo;
                        usingMenu = false;
                        break;
                    case '9':
                        userMenuChoice = MenuOption.LoadAccountInfo;
                        usingMenu = false;
                        break;
                    case 'E':
                    case 'e':
                        userMenuChoice = MenuOption.Exit;
                        usingMenu = false;
                        break;
                    default:
                        ConsoleUtil.DisplayMessage(
                            "It appears you have selected an incorrect choice." + Environment.NewLine +
                            "Press any key to continue or the ESC key to quit the application.");

                        userResponse = Console.ReadKey(true);
                        if (userResponse.Key == ConsoleKey.Escape)
                        {
                            usingMenu = false;
                        }
                        break;
                }
            }
            Console.CursorVisible = true;

            return userMenuChoice;
        }

        /// <summary>
        /// get the next city to travel to from the user
        /// </summary>
        /// <returns>string City</returns>
        public string DisplayGetNextCity()
        {           
            string nextCity = "";

            ConsoleUtil.HeaderText = "Travel";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayPromptMessage("Enter the name of the next city:");
            nextCity = Console.ReadLine();

            return nextCity;
        }

        /// <summary>
        /// display a list of the cities traveled
        /// </summary>
        public void DisplayCitiesTraveled(Salesperson salesperson)

        {
            ConsoleUtil.HeaderText = "Cities Visited";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage($"You began you journey in {salesperson.StartingCity}.");
            ConsoleUtil.DisplayMessage($"Since then you have traveled to the following cities:");
            ConsoleUtil.DisplayMessage("");

            if (salesperson.CitiesVisited.Count == 0)
            {
                ConsoleUtil.DisplayMessage("You have not traveled anywhere yet.");
            }
            else
            {
                foreach (string city in salesperson.CitiesVisited)
                {
                    ConsoleUtil.DisplayMessage(city);
                }
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display the current account information
        /// </summary>
        public void DisplayAccountInfo(Salesperson salesperson)
        {
            ConsoleUtil.HeaderText = "Account Info";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("First Name: " + salesperson.FirstName);
            ConsoleUtil.DisplayMessage("Last Name: " + salesperson.LastName);
            ConsoleUtil.DisplayMessage("Account ID: " + salesperson.AccountID);
            ConsoleUtil.DisplayMessage("Starting City: " + salesperson.StartingCity);

            if (!salesperson.CurrentStock.OnBackorder)
            {
                ConsoleUtil.DisplayMessage("Products in Inventory: " + salesperson.CurrentStock.NumberOfUnits);
            }
            else
            {
                ConsoleUtil.DisplayMessage("Products on Backorder: " + Math.Abs(salesperson.CurrentStock.NumberOfUnits));
            }
            ConsoleUtil.DisplayMessage("Product Type: ");
            ConsoleUtil.DisplayMessage("");
            ConsoleUtil.DisplayMessage($"\t- {salesperson.CurrentStock.Type}");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// displays backorder information
        /// </summary>
        public void DisplayBackorderNotification(Product product, int numberOfUnitsSold)
        {
            ConsoleUtil.HeaderText = "!ALERT!";
            ConsoleUtil.HeaderBackgroundColor = ConsoleColor.Red;
            ConsoleUtil.HeaderForegroundColor = ConsoleColor.White;
            ConsoleUtil.BodyForegroundColor = ConsoleColor.Red;
            ConsoleUtil.DisplayReset();        

            int numberOfUnitsBackordered = Math.Abs(product.NumberOfUnits);
            int numberOfUnitsShipped = numberOfUnitsSold - numberOfUnitsBackordered;

            ConsoleUtil.DisplayMessage("Inventory Backorder Notification");
            ConsoleUtil.DisplayMessage("");
            ConsoleUtil.DisplayMessage($"Products Sold: {numberOfUnitsSold}");
            ConsoleUtil.DisplayMessage($"Products Shipped: {numberOfUnitsShipped}");
            ConsoleUtil.DisplayMessage($"Products on Backorder: {numberOfUnitsBackordered}");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// gets the number of untis to buy
        /// </summary>        
        public int DisplayGetNumberUnitsToBuy(Product product)
        {
            //
            // declare int variable to hold the number of units to sell
            //
            int numberOfUnitsToBuy;

            ConsoleUtil.HeaderText = "Purchase Inventory";
            ConsoleUtil.DisplayReset();

            if (!ConsoleValidator.TryGetIntegerFromUser(MINIMUM_BUYSELL_AMOUNT, MAXIMUM_BUYSELL_AMOUNT, MAXIMUM_ATTEMPTS, "products", out numberOfUnitsToBuy))
            {
                ConsoleUtil.DisplayMessage("Maximum attemps exceeded!");
                ConsoleUtil.DisplayMessage("By default, the number of products to sell will be set to zero.");
                numberOfUnitsToBuy = 0;
                DisplayContinuePrompt();
            }

            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage(numberOfUnitsToBuy + " products have been added to the inventory.");

            DisplayContinuePrompt();

            return numberOfUnitsToBuy;
        }

        /// <summary>
        /// gets the number of untis to sell
        /// </summary>       
        public int DisplayGetNumberUnitsToSell(Product product)
        {
            //
            // declare int variable to hold the number of units to sell
            //
            int numberOfUnitsToSell;

            ConsoleUtil.HeaderText = "Sell Inventory";
            ConsoleUtil.DisplayReset();         

            if (!ConsoleValidator.TryGetIntegerFromUser(MINIMUM_BUYSELL_AMOUNT, MAXIMUM_BUYSELL_AMOUNT, MAXIMUM_ATTEMPTS, "products", out numberOfUnitsToSell))
            {
                ConsoleUtil.DisplayMessage("Maximum sttempts exceeded!");
                ConsoleUtil.DisplayMessage("By default, the number of products to sell will be set to zero.");
                numberOfUnitsToSell = 0;
                DisplayContinuePrompt();
            }

            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage(numberOfUnitsToSell + " products have been subtracted from the inventory.");

            DisplayContinuePrompt();

            return numberOfUnitsToSell;
        }

        /// <summary>
        /// displays the current inventory
        /// </summary>
        public void DisplayInventory(Salesperson salesperson, Product units)
        {          
            ConsoleUtil.HeaderText = "Current Inventory";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("");
            ConsoleUtil.DisplayMessage("Products:");
            ConsoleUtil.DisplayMessage("");
            ConsoleUtil.DisplayPromptMessage($"\t- {salesperson.CurrentStock.Type}, # of units: {units.NumberOfUnits.ToString()}");
            ConsoleUtil.DisplayMessage("");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// changes string to lowercase with first letter uppercase
        /// adapted from: https://www.dotnetperls.com/uppercase-first-letter
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concatenation substring.
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }

        /// <summary>
        /// displays the current account information
        /// </summary>       
        public void DisplayAccountDetail(Salesperson salesperson)
        {
            ConsoleUtil.DisplayMessage("First Name: " + salesperson.FirstName);
            ConsoleUtil.DisplayMessage("Last Name: " + salesperson.LastName);
            ConsoleUtil.DisplayMessage("Account ID: " + salesperson.AccountID);
            ConsoleUtil.DisplayMessage("Starting City: " + salesperson.StartingCity);

            if (!salesperson.CurrentStock.OnBackorder)
            {
                ConsoleUtil.DisplayMessage("Products in Inventory: " + salesperson.CurrentStock.NumberOfUnits);
            }
            else
            {
                ConsoleUtil.DisplayMessage("Products on Backorder: " + Math.Abs(salesperson.CurrentStock.NumberOfUnits));
            }
            ConsoleUtil.DisplayMessage("Product Type: " + salesperson.CurrentStock.Type);
            ConsoleUtil.DisplayMessage("");            
            
            ConsoleUtil.DisplayMessage("Cities Traveled: ");
            ConsoleUtil.DisplayMessage("");

            foreach (string city in salesperson.CitiesVisited)
            {
                ConsoleUtil.DisplayMessage(city);
            }
        }

        /// <summary>
        /// displays a notification upon successful load of the account and travel log info
        /// </summary>    
        public void DisplayConfirmLoadAccountInfo(Salesperson salesperson)
        {
            ConsoleUtil.HeaderText = "Load Account";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Account information loaded.");
            ConsoleUtil.DisplayMessage("");

            DisplayAccountDetail(salesperson);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// displays a notification upon successful save of the account and travel log info
        /// </summary>
        public void DisplayConfirmSaveAccountInfo()
        {
            ConsoleUtil.HeaderText = "Save Account";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Account information saved.");            

            DisplayContinuePrompt();
        }

        /// <summary>
        /// prompts the user to load the account and travel log info
        /// </summary>
        public bool DisplayLoadAccountInfo(out bool maxAttemptsExceeded)
        {
            string userResponse;
            maxAttemptsExceeded = false;

            ConsoleUtil.HeaderText = "Load Account";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("");
            userResponse = ConsoleValidator.GetYesNoFromUser(MAXIMUM_ATTEMPTS, "Load account info?", out maxAttemptsExceeded);

            if (maxAttemptsExceeded)
            {
                ConsoleUtil.DisplayMessage("Max attempts exceeded! You will now be returned to the main menu.");
                return false;
            }
            else if (userResponse.ToUpper() == "YES")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// prompts the user to load the account and travel log info (method is called if current account info is present)
        /// </summary>
        public bool DisplayLoadAccountInfo(Salesperson salesperson, out bool maxAttemptsExceeded)
        {
            string userResponse;
            maxAttemptsExceeded = false;

            ConsoleUtil.HeaderText = "!WARNING!";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Loading previously saved data will overwrite the current information:");
            ConsoleUtil.DisplayMessage("");

            DisplayAccountDetail(salesperson);

            ConsoleUtil.DisplayMessage("");
            userResponse = ConsoleValidator.GetYesNoFromUser(MAXIMUM_ATTEMPTS, "Load account info?", out maxAttemptsExceeded);

            if (maxAttemptsExceeded)
            {
                ConsoleUtil.DisplayMessage("Max attempts exceeded! You will now be returned to the main menu.");
                return false;
            }
            else if (userResponse.ToUpper() == "YES")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// prompts the user to save the account and travel log info
        /// </summary>
        public bool DisplaySaveAccountInfo(Salesperson salesperson, out bool maxAttemptsExceeded)
        {
            string userResponse; 
            maxAttemptsExceeded = false;

            ConsoleUtil.HeaderText = "Save Account";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Current account information:");
            DisplayAccountDetail(salesperson);

            ConsoleUtil.DisplayMessage("");
            userResponse = ConsoleValidator.GetYesNoFromUser(MAXIMUM_ATTEMPTS, "Save account info?", out maxAttemptsExceeded);

            if (maxAttemptsExceeded)
            {
                ConsoleUtil.DisplayMessage("Max attempts exceeded! You will now be returned to the main menu.");
                return false;
            }
            else if (userResponse.ToUpper() == "YES")
            {
                return true;
            }
            {
                return false;
            }
        }
        
        /// <summary>
        /// allows the user to update their account information
        /// </summary>        
        public Salesperson DisplayUpdateAcountInfo (Salesperson salesperson, out bool maxAttemptsExceeded)
        {            
            string userResponse;
            bool editingAccount = true;
            maxAttemptsExceeded = false;

            ConsoleUtil.HeaderText = "Update Account";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("");
            ConsoleUtil.DisplayMessage(" NOTE: You cannot change the number units here. Use the Buy/Sell options \nfrom the main menu to do that.");

            DisplayContinuePrompt();           

            while (editingAccount)
            {
                ConsoleUtil.HeaderText = "Update Account";
                ConsoleUtil.DisplayReset();

                //
                // displays current account info wihtout cities and number of units
                //
                #region Current Account Info

                ConsoleUtil.DisplayMessage("Current account info:");
                ConsoleUtil.DisplayMessage("");

                ConsoleUtil.DisplayMessage("First Name: " + salesperson.FirstName);
                ConsoleUtil.DisplayMessage("Last Name: " + salesperson.LastName);
                ConsoleUtil.DisplayMessage("Account ID: " + salesperson.AccountID);
                ConsoleUtil.DisplayMessage("Starting City: " + salesperson.StartingCity);
                ConsoleUtil.DisplayMessage("Product Type: " + salesperson.CurrentStock.Type);

                #endregion

                ConsoleUtil.DisplayMessage("");
                ConsoleUtil.DisplayMessage("Please select a corresonding number to change that part of your account.");
                ConsoleUtil.DisplayMessage("");

                ConsoleUtil.DisplayPromptMessage("1)First Name | 2)Last Name | 3)Account ID | 4)Starting City | 5)Product");

                ConsoleUtil.DisplayMessage("");
                ConsoleUtil.DisplayMessage("");
                ConsoleUtil.DisplayMessage("");
                ConsoleUtil.DisplayPromptMessage("Enter your choice here: ");
                userResponse = Console.ReadLine();
                ConsoleUtil.DisplayMessage("");

                //
                // get a valid answer form user
                //
                while (userResponse != "1" && userResponse != "2" && userResponse != "3" && userResponse != "4" && userResponse != "5")
                {
                    ConsoleUtil.HeaderText = "Invalid Input";
                    ConsoleUtil.DisplayReset();

                    ConsoleUtil.DisplayPromptMessage("1)First Name | 2)Last Name | 3)Account ID | 4)Starting City | 5)Product");
                    ConsoleUtil.DisplayMessage("");
                    ConsoleUtil.DisplayPromptMessage("Please enter a correct corresponding number: ");
                    userResponse = Console.ReadLine();

                    if (userResponse == "1" && userResponse == "2" && userResponse == "3" && userResponse == "4" && userResponse == "5")
                    {
                        break;
                    }
                }

                //
                // edit account info
                //
                switch (userResponse)
                {
                    case "1":
                        ConsoleUtil.HeaderText = "Update First Name";
                        ConsoleUtil.DisplayReset();

                        ConsoleUtil.DisplayPromptMessage("Enter your new first name: ");
                        salesperson.FirstName = Console.ReadLine();
                        ConsoleUtil.DisplayMessage("");
                        break;

                    case "2":
                        ConsoleUtil.HeaderText = "Update Last Name";
                        ConsoleUtil.DisplayReset();

                        ConsoleUtil.DisplayPromptMessage("Enter your new last name: ");
                        salesperson.LastName = Console.ReadLine();
                        ConsoleUtil.DisplayMessage("");
                        break;

                    case "3":
                        ConsoleUtil.HeaderText = "Update Account ID";
                        ConsoleUtil.DisplayReset();

                        ConsoleUtil.DisplayPromptMessage("Enter your new Account ID: ");
                        salesperson.AccountID = Console.ReadLine();
                        ConsoleUtil.DisplayMessage("");
                        break;

                    case "4":
                        ConsoleUtil.HeaderText = "Update Starting City";
                        ConsoleUtil.DisplayReset();

                        ConsoleUtil.DisplayPromptMessage("Enter your new starting city: ");
                        salesperson.StartingCity = Console.ReadLine();
                        ConsoleUtil.DisplayMessage("");
                        break;

                    case "5":
                        ConsoleUtil.HeaderText = "Update Product Type";
                        ConsoleUtil.DisplayReset();

                        Product.ProductType productType;

                        ConsoleUtil.DisplayMessage("");
                        ConsoleUtil.DisplayPromptMessage("Enter you new product selection: ");

                        //
                        // get new product type from user
                        //
                        if (Enum.TryParse<Product.ProductType>(UppercaseFirst(Console.ReadLine()), out productType))
                        {
                            salesperson.CurrentStock.Type = productType;                            
                        }
                        else
                        {
                            ConsoleUtil.DisplayReset();
                            ConsoleUtil.DisplayMessage("Seems like you entered an invalid product type.");
                            ConsoleUtil.DisplayMessage("By default, your product type has been set to None.");
                            salesperson.CurrentStock.Type = Product.ProductType.None;                          
                            DisplayContinuePrompt();
                        }                        
                        break;

                    default:
                        break;
                }
                ConsoleUtil.HeaderText = "Update Account";
                ConsoleUtil.DisplayReset();
                ConsoleUtil.DisplayMessage("Your account has been updated!");
                ConsoleUtil.DisplayMessage("");
                userResponse = ConsoleValidator.GetYesNoFromUser(MAXIMUM_ATTEMPTS, "\tWould you like to make more changes or continue on?", out maxAttemptsExceeded);

                if (maxAttemptsExceeded)
                {
                    ConsoleUtil.DisplayMessage("Max attempts exceeded! You will now be returned to the main menu.");
                    return salesperson;
                }
                else if (userResponse.ToUpper() == "YES")
                {
                    editingAccount = true;
                }
                else
                {
                    editingAccount = false;
                    return salesperson;
                }
            }            
            return salesperson;
        }

        #endregion
    }
}
