using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DSAxProgrammingFinals
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Main important variables


            //-----ARRAY USED-----         

            string[] adminNames = new string[] { "Rose Tumayan", "Julian Laspona" }; // fixed admin names for simplicity

            string[] adminIDs = new string[] { "ADM-001", "ADM-002" }; // fixed admin IDs for simplicity

            int[] billDenominations = { 1000, 500, 200, 100, 50, 20 }; // ATM bill denominations in descending order

            int[] billCounts = { 200, 300, 400, 500, 600, 1000 }; // initial counts of each bill denomination in the ATM (total of 530,000)

            int atmCash = 0; // initial ATM cash amount ( set to 0 first)

            double[] userLoanAmounts = new double[100]; // to store loan amounts for each user (up to 100 users just fixed)

            int[] userLoanTerms = new int[100]; // to store loan terms (in months) for each user (up to 100 users just fixed)

            bool[] userHasActiveLoan = new bool[100]; // to store  if there are active loan terms (up to 100 users just fixed)

            DateTime[] userLoanStartDates = new DateTime[100]; // to store loan start dates for each user (up to 100 users just fixed)

            //-----LIST USED-----  

            // User messaging system variables

            //List that store the messaging system when account is locked

            List<string> transactionHistory = new List<string>();  // dynamic global log (for admin)

            List<int> transactionCounts = new List<int>(); // per-user transaction count

            List<string> userTransactionHistories = new List<string>(); // to store transaction history for each user

            List<string> LoggedSystem = new List<string>(); // to store logged in users' names 


            List<string> userMessageAccounts = new List<string>(); // to store account numbers of users who sent messages

            List<string> userMessageNames = new List<string>(); // to store names of users who sent messages

            List<string> userMessages = new List<string>(); // to store messages sent by users

            List<string> lockedAccount = new List<string>(); // to store accounts that are locked and awaiting admin assistance

            List<string> adminReplies = new List<string>(); // to store replies from admins to user messages

            List<DateTime> userMessageTimes = new List<DateTime>(); // to store timestamps of when messages were sent

            List<bool> hasAdminReply = new List<bool>(); // to track if admin has replied to user messages


            //List that store the user information

            List<string> accountNames = new List<string> { "Yuan Mendoza", "Chris Magbuhos", "Eudrick Velasquez" }; // start with 3 sample account names

            List<string> accountNumbers = new List<string> { "10000001", "10000002", "10000003" }; // start with 3 sample account numbers

            List<string> pins = new List<string> { "1234", "5678", "9012" }; // start with 3 sample PINs for the accounts

            List<double> balances = new List<double> { 5000.0, 15000.0, 50000.0 }; // start with 3 sample balances for the accounts

            List<bool> accountStatus = new List<bool> { true, true, true }; // start with 3 sample account statuses (all active)

            //-----OTHER VARIABLES-----

            // variable logic functions

            int role; // to store the role of the user (1 for User, 2 for Admin)

            string roleInput; // to store the role input from the user

            string inputAccount; // to store the account number input by the user

            int matchedIndex; // to store the index of the matched account number in the accountNumbers list

            int pinAttempts; // to track the number of PIN attempts during login

            bool loginSuccess; // to indicate if the login was successful

            int loggedInIndex; // to store the index of the logged-in user in the account lists

            string userName; // to store the name of the logged-in user

       

            string currentUser = ""; // to store the current user name

            int currentUserIndex = -1; // to store the index of the current user in the account lists ( initialize to -1 to indicate no user logged in yet)

            // file paths

            string mainDirectory = "SourceATMCode"; // directory for storing files

            string physicalCard = "card_input.txt"; // file to store physical card input

            string accountData = "account.csv"; // file to store account data

            string balanceData = " balance.csv"; // file to store balance data 

            string billsData = "atm_bills.csv"; // file to store ATM bills data

            string receiptData = "receipt.csv"; // file to store receipt data

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();

            // ASCII Art Display
            Console.SetCursorPosition(35, 8);
            Console.WriteLine("        ______   ________  __       __ ");
            Console.WriteLine("                                          /      \\ /        |/  \\     /  |");
            Console.WriteLine("                                         /$$$$$$  |$$$$$$$$/ $$  \\   /$$ |");
            Console.WriteLine("                                         $$ |__$$ |   $$ |   $$$  \\ /$$$ |");
            Console.WriteLine("                                         $$    $$ |   $$ |   $$$$  /$$$$ |");
            Console.WriteLine("                                         $$$$$$$$ |   $$ |   $$ $$ $$/$$ |");
            Console.WriteLine("                                         $$ |  $$ |   $$ |   $$ |$$$/ $$ |");
            Console.WriteLine("                                         $$ |  $$ |   $$ |   $$ | $/  $$ |");
            Console.WriteLine("                                         $$/   $$/    $$/    $$/      $$/ ");
            Console.WriteLine("                                                                              ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("                                                                       Simulator");

            Console.ResetColor();

            Console.SetCursorPosition(44, 25);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            Console.SetCursorPosition(49, 28);
            Console.Write("Loading");

            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                Thread.Sleep(300);
            }

            Console.Clear();

            // Initialize messaging system


            // Initialize transaction history
            if (transactionHistory.Count == 0)
            {
                Console.WriteLine("No Transaction Yet.");
            }
            else
            


            // Create directory and files
            if (!Directory.Exists(mainDirectory))
            {
                Directory.CreateDirectory(mainDirectory);
            }

            physicalCard = Path.Combine(mainDirectory, "card_input.txt");

            accountData = Path.Combine(mainDirectory, "account.csv");

            balanceData = Path.Combine(mainDirectory, "balances.csv");

            billsData = Path.Combine(mainDirectory, "atm_bills.csv");

            receiptData = Path.Combine(mainDirectory, "receipt.csv");

            if (!File.Exists(physicalCard))
            {
                File.Create(physicalCard).Close();
            }
            else if (File.Exists(physicalCard))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nFiles already exist. Loading data...");
                Console.ResetColor();
            }

            if (!File.Exists(accountData))
            {
                File.Create(accountData).Close();

              
            }
            else if (File.Exists(accountData))
            {
                string[] lines = File.ReadAllLines(accountData);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(',');
                    if (fields.Length == 4)
                    {
                        accountNumbers.Add(fields[0].Trim());
                        accountNames.Add(fields[1].Trim());
                        pins.Add(fields[2].Trim());
                        accountStatus.Add(fields[3].Trim().ToLower() == "true");
                    }
                }
            }

            if (!File.Exists(balanceData))
            {
                File.Create(balanceData).Close();
            }

            else if (File.Exists(balanceData))
            {
                string[] balanceLines = File.ReadAllLines(balanceData);
                for (int i = 0; i < balanceLines.Length; i++)
                {
                    string[] parts = balanceLines[i].Split(',');
                    string acct = parts[0];
                    int balance = int.Parse(parts[1]);

                    int existingIndex = accountNumbers.IndexOf(acct);
                    if (existingIndex != -1)
                    {
                        // After loading accountNumbers, ensure balances has the same count
                        while (balances.Count < accountNumbers.Count)
                        {
                            balances.Add(0); // or any default initial value
                        }


                        balances[existingIndex] = balance;
                    }
                }
            }



            if (!File.Exists(billsData)) File.Create(billsData).Close();

            if (!File.Exists(receiptData)) File.Create(receiptData).Close();

            bool exitProgram = false;

            // Main login loop
            do
            {
                Console.Clear();

                pinAttempts = 0;

                loginSuccess = false;

                loggedInIndex = -1;

                userName = "";

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nATM LOGIN");
                Console.WriteLine("========================");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nSelect Role:");
                Console.WriteLine("\n1 - User");
                Console.WriteLine("\n2 - Admin");
                Console.Write("\nEnter role number: ");
                Console.ResetColor();

                roleInput = Console.ReadLine();

                if (roleInput == "1" || roleInput == "2")
                {
                    role = int.Parse(roleInput);
                    Console.Clear();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid role selected. Press any key to retry...");
                    Console.ReadKey();
                    continue;
                }

                do
                {

                    if (role == 1) // User login
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nUSER LOGIN");
                        Console.WriteLine("========================");
                        Console.ResetColor();
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("\nEnter Account Number: ");
                        Console.ResetColor();
                        inputAccount = Console.ReadLine().Trim();

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("\nChecking ATM database");
                        for (int i = 0; i < 5; i++)
                        {
                            Thread.Sleep(500); 
                            Console.Write(".");
                        }
                        Console.ResetColor();


                        matchedIndex = accountNumbers.IndexOf(inputAccount);

                        if (matchedIndex == -1)
                        {
                            matchedIndex = accountNumbers.IndexOf(inputAccount);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine();
                            Console.WriteLine("\nAccount not found in the ATM database.");
                            Console.ResetColor();
                            Console.WriteLine("\nPress any key to try again...");
                            Console.ReadKey();
                            continue;

                        }
                        else
                        {

                    
                            List<string> updatedCards = new List<string>(); // to store updated card information
                            for (int i = 0; i < accountNumbers.Count; i++)
                            {
                                string line = $"{accountNumbers[i]},{accountNames[i]}";

                                if (!updatedCards.Contains(line))
                                {
                                    updatedCards.Add(line);
                                }
                           
                            }
                            File.WriteAllLines(physicalCard, updatedCards);


                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine();
                            Console.WriteLine("\nAccount found: " + accountNames[matchedIndex]);
                            Console.ReadKey();
                        }



                        // Check if this account is locked and awaiting admin
                        bool isLockedAwaitingAdmin = lockedAccount.Contains(inputAccount);

                        if (isLockedAwaitingAdmin && role == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nYour account is locked. Please contact admin for assistance.");
                            Console.ResetColor();

                            loginSuccess = true;
                            loggedInIndex = matchedIndex;
                            userName = accountNames[matchedIndex];
                            break;
                        }

                        while (!loginSuccess && pinAttempts < 3)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("\nEnter PIN: ");
                            Console.ResetColor();
                            string inputPin = Console.ReadLine().Trim();

                            if (pins[matchedIndex] == inputPin)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nLogin successful!");
                                Console.ResetColor();

                                loginSuccess = true;
                                loggedInIndex = matchedIndex;
                                userName = accountNames[matchedIndex];
                            }
                            else
                            {
                                pinAttempts++;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"\nIncorrect PIN. Attempt {pinAttempts}/3");
                                Console.ResetColor();
                                Console.ReadKey();

                                if (pinAttempts >= 3)
                                {
                                    accountStatus[matchedIndex] = false; // update the status in memory

                                    // Now update the CSV file using the updated status list
                                    List<string> updatedAccounts = new List<string>();
                                    for (int i = 0; i < accountNumbers.Count; i++)
                                    {
                                        string line = $"{accountNumbers[i]},{accountNames[i]},{pins[i]},{accountStatus[i].ToString().ToLower()}";
                                        updatedAccounts.Add(line);
                                    }
                                    File.WriteAllLines(accountData, updatedAccounts);

                                    if (!lockedAccount.Contains(inputAccount))
                                        lockedAccount.Add(inputAccount);

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nAccount has been locked due to 3 incorrect PIN attempts.");
                                    Console.ResetColor();
                                    Console.Write("\nPress Any Key to Contact Admin...");
                                    Console.ReadLine();

                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("\nYou can now contact admin for assistance.");
                                    Console.ResetColor();
                                    Console.WriteLine("\nPress any key to continue...");
                                    Console.ReadKey();

                                    loginSuccess = true;
                                    loggedInIndex = matchedIndex;
                                    userName = accountNames[matchedIndex];
                                }
                            }

                            // In either success or lock case, always update account file once
                            if (loginSuccess)
                            {
                                List<string> updatedAccounts = new List<string>();
                                for (int i = 0; i < accountNumbers.Count; i++)
                                {
                                    string line = $"{accountNumbers[i]},{accountNames[i]},{pins[i]},{accountStatus[i].ToString().ToLower()}";
                                    updatedAccounts.Add(line);
                                }
                                File.WriteAllLines(accountData, updatedAccounts);
                            }
                        }



                        if (!loginSuccess && !exitProgram)
                        {
                            Console.WriteLine("\nPress any key to try again...");
                            Console.ReadKey();
                        }

                        Console.ForegroundColor = ConsoleColor.White;
                        LoggedSystem.Add($"{accountNumbers[loggedInIndex]}: logged in at {DateTime.Now}");
                        Console.ResetColor();
                    }
                    else // Admin login
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nADMIN LOGIN");
                        Console.WriteLine("========================");
                        Console.ResetColor();

                        Console.Write("\nEnter Admin ID (e.g., ADM-001): ");
                        string inputAdminID = Console.ReadLine().Trim();

                        Console.Write("\nEnter Admin Name (First and Last): ");
                        string inputAdminName = Console.ReadLine().Trim();

                        bool isAdminValid = false;

                        for (int i = 0; i < adminIDs.Length; i++)
                        {
                            if (adminIDs[i] == inputAdminID && adminNames[i] == inputAdminName)
                            {
                                isAdminValid = true;
                                userName = adminNames[i];
                                break;
                            }
                        }

                        if (isAdminValid)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nAdmin login successful!");
                            Console.ResetColor();

                            loginSuccess = true;
                            loggedInIndex = -1; // Indicates it's not a user account
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nInvalid Admin ID or Name.");
                            Console.ResetColor();
                            Console.WriteLine("\nPress any key to try again...");
                            Console.ReadKey();
                        }


                    }


                } while (!loginSuccess && !exitProgram);

                // After Login Validation
                bool validUser = false;

                currentUserIndex = -1;

                if (role == 1 && loggedInIndex >= 0)
                {
                    currentUser = accountNames[loggedInIndex];
                    currentUserIndex = loggedInIndex;
                    validUser = true;
                }
                else if (role == 2)
                {
                    for (int i = 0; i < adminNames.Length; i++)
                    {
                        if (userName.ToUpper() == adminNames[i].ToUpper())
                        {
                            validUser = true;
                            currentUser = adminNames[i];
                            break;
                        }
                    }
                }

                if (!validUser)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid name or role. Try again.");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to return to login...");
                    Console.ReadKey();
                    continue;
                }

                Console.Clear();
                Console.Write("\nLoading Please Wait");

                for (int j = 0; j < 5; j++)
                {
                    Console.Write(".");
                    Thread.Sleep(300);
                }
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nWelcome {currentUser}! You are logged in as {(role == 1 ? "User" : "Admin")}.");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();

                bool usingATM = true;

                while (usingATM) // MAIN ATM Loop
                {
                    if (!usingATM) // go back to log in
                    {
                        Console.WriteLine("\nLogging in again");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }


                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    if (role == 1) // User menu
                    {
                        // Check if user is locked and awaiting admin
                        bool isUserLocked = lockedAccount.Contains(accountNumbers[currentUserIndex]);

                        if (isUserLocked)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            // Show only Contact Admin option
                            Console.WriteLine("\n=== Account Locked - Limited Menu ===");
                            Console.WriteLine("\n9. Customer Service");
                            Console.WriteLine("\n10. Log Out");
                            Console.ResetColor();
                            Console.Write("\nEnter your choice: ");
                        }
                        else
                        {
                            // Show full menu
                            Console.WriteLine("\n=== ATM Menu ===");
                            Console.WriteLine("\n1. Check Balance");
                            Console.WriteLine("\n2. Deposit");
                            Console.WriteLine("\n3. Withdraw");
                            Console.WriteLine("\n4. Pay Bills");
                            Console.WriteLine("\n5. Fund Transfer");
                            Console.WriteLine("\n6. Change PIN");
                            Console.WriteLine("\n7. Loan/Pay Loan");
                            Console.WriteLine("\n8. View Transaction History");
                            Console.WriteLine("\n9. Customer Service");
                            Console.WriteLine("\n10. Log Out");
                            Console.WriteLine("\n11. Exit Program");
                            Console.ResetColor();
                            Console.Write("\nEnter your choice: ");
                        }
                    }
                    else // Admin menu
                    {
                        Console.WriteLine("\n--- ADMIN MENU ---");
                        Console.WriteLine("\n1. Create/Delete User");
                        Console.WriteLine("\n2. Transaction History of User");
                        Console.WriteLine("\n3. Account Management");
                        Console.WriteLine("\n4. View ATM Cash Status");
                        Console.WriteLine("\n5. Restock Cash");              
                        Console.WriteLine("\n6. View All Customer Inquiries");
                        Console.WriteLine("\n7. View System Logs");
                        Console.WriteLine("\n8. View Accounts");
                        Console.WriteLine("\n9. Logout");
                        Console.WriteLine("\n10. Exit Program");
                        Console.ResetColor();
                        Console.Write("\nEnter your choice: ");
                    }

                    string userChoice = Console.ReadLine();

                    if (int.TryParse(userChoice, out int choice))
                    {
                        if (role == 1) // User operations
                        {
                            bool isUserLocked = lockedAccount.Contains(accountNumbers[currentUserIndex]);

                            if (isUserLocked) // If user gets locked
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                // Limited menu for locked users
                                Console.WriteLine("\n=== Account Locked - Limited Menu ===");
                                Console.WriteLine("\n9. Customer Service"); ;
                                Console.WriteLine("\n10. Log Out");
                                Console.Write("\nEnter your choice: ");


                                if (int.TryParse(userChoice, out int lockedChoice)) // limit to case 9 and 10 only
                                {
                                    switch (lockedChoice)
                                    {
                                        case 9: // Contact Admin
                                            Console.Clear();
                                            Console.ForegroundColor = ConsoleColor.Cyan;
                                            Console.WriteLine("\n=== Contact Admin ===");
                                            Console.WriteLine("\n1. Send Message to Admin");
                                            Console.WriteLine("\n2. View Admin Replies");
                                            Console.WriteLine("\n3. Back to Main Menu");
                                            Console.Write("\nChoose option: ");

                                            Console.ForegroundColor = ConsoleColor.White;
                                            LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Accessed Contact Admin Option (locked account) at {DateTime.Now}");
                                            Console.ResetColor();


                                            if (int.TryParse(Console.ReadLine(), out int contactChoice))
                                            {
                                                switch (contactChoice) // 3 cases for case 9
                                                {
                                                    case 1: // Send Message
                                                        Console.Clear();
                                                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                        Console.WriteLine("\n=== Send Message to Admin ===");
                                                        Console.WriteLine($"\nAccount Name: {accountNames[currentUserIndex]}");
                                                        Console.WriteLine($"\nAccount Number: {accountNumbers[currentUserIndex]}");
                                                        Console.Write("\nEnter your message: ");
                                                        string userMessage = Console.ReadLine();

                                                        if (!string.IsNullOrWhiteSpace(userMessage)) // if not emtpy
                                                        {
                                                            userMessageAccounts.Add(accountNumbers[currentUserIndex]); //add to the list (account)

                                                            userMessageNames.Add(accountNames[currentUserIndex]); // add to the list (Names)

                                                            userMessages.Add(userMessage); // storage list of user message

                                                            userMessageTimes.Add(DateTime.Now); // add to the list ( Date and Time)

                                                            adminReplies.Add("");  // blank admin  

                                                            hasAdminReply.Add(false); // no admin reply yet

                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("\nMessage sent successfully!");

                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Sent an Message to Admin (locked account) at {DateTime.Now}");
                                                            Console.ResetColor();
                                                        }
                                                        else
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("\nMessage cannot be empty.");
                                                            Console.ResetColor();
                                                        }
                                                        Console.WriteLine("\nPress any key to return...");
                                                        Console.ReadKey();
                                                        break;

                                                    case 2: // View Replies
                                                        Console.Clear();
                                                        Console.WriteLine("=== Admin Replies ===");
                                                        bool foundReplies = false;

                                                        for (int i = 0; i < userMessageAccounts.Count; i++)
                                                        {
                                                            if (userMessageAccounts[i] == accountNumbers[currentUserIndex])
                                                            {
                                                                Console.WriteLine($"\nMessage Sent: {userMessages[i]}");
                                                                Console.WriteLine($"\nSent On: {userMessageTimes[i]}");

                                                                if (hasAdminReply[i])
                                                                {
                                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                                    Console.WriteLine($"\nAdmin Reply: {adminReplies[i]}");
                                                                }
                                                                else
                                                                {
                                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                                    Console.WriteLine("\nNo reply yet.");
                                                                }
                                                                Console.ResetColor();
                                                                foundReplies = true;
                                                            }
                                                        }

                                                        if (!foundReplies)
                                                        {
                                                            Console.WriteLine("\nNo messages or replies found for your account.");
                                                        }

                                                        Console.WriteLine("\nPress any key to return...");
                                                        Console.ForegroundColor = ConsoleColor.White;
                                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: view Admin Reply (locked account) at {DateTime.Now}");
                                                        Console.ResetColor();
                                                        Console.ReadKey();
                                                        break;

                                                    case 3: // Back
                                                        Console.Clear();
                                                        break;

                                                    default:
                                                        Console.WriteLine("\nInvalid option. Press any key to return...");
                                                        Console.ReadKey();
                                                        break;
                                                }
                                            }
                                            else
                                            {

                                                Console.WriteLine("\nInvalid input. Press any key to return...");
                                                Console.ReadKey();
                                                continue;

                                            }

                                            break;


                                        case 10:
                                            usingATM = false; // Safely log out
                                            Console.ForegroundColor = ConsoleColor.White;
                                            LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: logged out (locked account) at {DateTime.Now}");
                                            Console.ResetColor();
                                            break;

                                        default:
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine();
                                            Console.WriteLine("\nInvalid option. You can only choose option 9 and 10 since your account is locked");
                                            Console.ReadKey();
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("\nInvalid input. Press any key...");
                                    Console.ReadKey();
                                }
                            }
                            else // if not lock, show all cases from 1-11
                            {
                                switch (choice)
                                {
                                    case 1: // Check Balance
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine($"\nYour current balance is: PHP {balances[currentUserIndex]:F2}");

                                        Console.ForegroundColor = ConsoleColor.White;
                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Access Checked Balance at {DateTime.Now}");
                                        Console.ResetColor();



                                        Console.ReadKey();
                                        break;

                                    case 2: // Deposit
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Access Deposit at {DateTime.Now}");
                                        Console.ResetColor();

                                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                                        Console.Write("\nEnter deposit amount: PHP ");
                                        if (double.TryParse(Console.ReadLine(), out double depositAmount) && depositAmount > 0)
                                        {
                                            if (depositAmount % 1 != 0) // if not whole number
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nDeposit must be in whole numbers.");
                                                Console.ResetColor();
                                                Console.ReadKey();
                                                break;
                                            }

                                            int depositWhole = (int)depositAmount;

                                            int remaining = depositWhole;

                                            int[] billsDeposited = new int[billDenominations.Length];

                                            for (int i = 0; i < billDenominations.Length; i++)
                                            {
                                                while (remaining >= billDenominations[i])
                                                {
                                                    remaining -= billDenominations[i];
                                                    billsDeposited[i]++;
                                                }
                                            }

                                            if (remaining == 0)
                                            {
                                                for (int i = 0; i < billDenominations.Length; i++) // add it to atmCash (530,000)
                                                {
                                                    billCounts[i] += billsDeposited[i];
                                                    atmCash += billsDeposited[i] * billDenominations[i];
                                                }

                                                balances[currentUserIndex] += depositWhole;

                                                // Save updated balances to file
                                                try
                                                {
                                                    string updated = "";
                                                    for (int i = 0; i < accountNumbers.Count; i++)
                                                    {
                                                        updated += $"{accountNumbers[i]},{balances[i]:F2}\n";
                                                    }
                                                    File.WriteAllText("balances.csv", updated); // Save to file
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine($"Error saving balances: {ex.Message}");
                                                }


                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine($"\nSuccessfully deposited PHP {depositWhole}.00");
                                                Console.WriteLine($"\nNew balance: PHP {balances[currentUserIndex]:F2}");


                                                string log = $"{accountNumbers[currentUserIndex]}: Deposited PHP {depositWhole} on {DateTime.Now}";
                                                transactionHistory.Add(log);
                                                userTransactionHistories.Add(log);
                                                LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Made a deposit at {DateTime.Now}");

                                                try
                                                {
                                                    string receipt = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},DEPOSIT,{depositWhole:F2},{balances[currentUserIndex]:F2},{accountNumbers[currentUserIndex]}\n";
                                                    File.AppendAllText(receiptData, receipt);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine($"Error generating receipt: {ex.Message}");
                                                }
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nInvalid deposit amount (not divisible by available denominations).");
                                            }
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nInvalid deposit amount.");
                                        }

                                        Console.ResetColor();
                                        Console.ReadKey();
                                        break;


                                    case 3: // Withdraw
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Access Withdraw at {DateTime.Now}");
                                        Console.ResetColor();

                                        Console.Write("\nEnter withdrawal amount: PHP ");
                                        if (double.TryParse(Console.ReadLine(), out double withdrawAmount) && withdrawAmount > 0)
                                        {
                                            if (withdrawAmount % 1 != 0)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nWithdrawal must be in whole numbers.");
                                                Console.ResetColor();
                                                Console.ReadKey();
                                                break;
                                            }

                                            int amountWhole = (int)withdrawAmount;

                                            if (amountWhole > balances[currentUserIndex])
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nInsufficient balance.");
                                                Console.ResetColor();
                                                Console.ReadKey();
                                                break;
                                            }

                                            int totalAvailable = 0;
                                            for (int i = 0; i < billDenominations.Length; i++)
                                            {
                                                totalAvailable += billDenominations[i] * billCounts[i];
                                            }

                                            if (amountWhole > totalAvailable)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nATM does not have enough cash.");
                                                Console.ResetColor();
                                                Console.ReadKey();
                                                break;
                                            }

                                            int remaining = amountWhole;
                                            int[] billsToDispense = new int[billDenominations.Length];
                                            int[] tempBillCounts = new int[billDenominations.Length];

                                            for (int i = 0; i < billDenominations.Length; i++)
                                                tempBillCounts[i] = billCounts[i];

                                            for (int i = 0; i < billDenominations.Length; i++)
                                            {
                                                while (remaining >= billDenominations[i] && tempBillCounts[i] > 0)
                                                {
                                                    remaining -= billDenominations[i];
                                                    tempBillCounts[i]--;
                                                    billsToDispense[i]++;
                                                }
                                            }

                                            if (remaining == 0)
                                            {
                                                for (int i = 0; i < billDenominations.Length; i++)
                                                {
                                                    billCounts[i] -= billsToDispense[i];
                                                    atmCash -= billsToDispense[i] * billDenominations[i];
                                                }

                                                balances[currentUserIndex] -= amountWhole;

                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine($"\nSuccessfully withdrew PHP {amountWhole}.00");
                                                Console.WriteLine($"New balance: PHP {balances[currentUserIndex]:F2}");

                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine("\nBill Denomination Breakdown:");
                                                for (int i = 0; i < billsToDispense.Length; i++)
                                                {
                                                    if (billsToDispense[i] > 0)
                                                        Console.WriteLine($"PHP {billDenominations[i],4}: x {billsToDispense[i]}");
                                                }

                                                string log = $"{accountNumbers[currentUserIndex]}: Withdrew PHP {amountWhole} on {DateTime.Now}";
                                                transactionHistory.Add(log);
                                                userTransactionHistories.Add(log);
                                                LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Made a withdrawal at {DateTime.Now}");

                                                try
                                                {
                                                    string receipt = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},WITHDRAWAL,{amountWhole:F2},{balances[currentUserIndex]:F2},{accountNumbers[currentUserIndex]}\n";
                                                    File.AppendAllText(receiptData, receipt);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine($"Error generating receipt: {ex.Message}");
                                                }
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nATM cannot dispense this amount with available denominations.");
                                            }
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nInvalid withdrawal amount.");
                                        }

                                        Console.ResetColor();
                                        Console.ReadKey();
                                        break;



                                    case 4: // Pay Bills
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                                        Console.WriteLine("\n=== Bill Payment Service ===");
                                        Console.WriteLine("\n1. Electricity");
                                        Console.WriteLine("2. Water");
                                        Console.WriteLine("3. Internet");
                                        Console.Write("\nSelect bill type: ");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Access Pay Bills at {DateTime.Now}");
                                        Console.ResetColor();

                                        if (int.TryParse(Console.ReadLine(), out int billType) && billType >= 1 && billType <= 3)
                                        {
                                            Console.Clear();

                                            string billName = "";
                                            double bill = 0;

                                            if (billType == 1)
                                            {
                                                billName = "Electricity";
                                                bill = new Random().Next(1000, 10001); // 1000 to 10000
                                            }
                                            else if (billType == 2)
                                            {
                                                billName = "Water";
                                                bill = new Random().Next(500, 2001); // 500 to 2000
                                            }
                                            else if (billType == 3)
                                            {
                                                billName = "Internet";
                                                bill = new Random().Next(1000, 3001); // 1000 to 3000
                                            }

                                            int surcharge = 20;
                                            double totalBill = bill + surcharge;

                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            Console.WriteLine($"\n{billName} Bill");
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine($"\nYour {billName.ToLower()} bill is PHP {bill:F2} with a surcharge of PHP {surcharge:F2}.");
                                            Console.WriteLine($"\nTotal payable amount: PHP {totalBill:F2}.");

                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.Write("\nDo you want to pay now? Press 'y' for yes or any key to cancel: ");
                                            string userPayBill = Console.ReadLine().ToLower();

                                            LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Accessed {billName} Bill at {DateTime.Now}");
                                            Console.ResetColor();

                                            if (userPayBill == "y")
                                            {
                                                Console.Clear();

                                                if (balances[currentUserIndex] >= totalBill)
                                                {
                                                    balances[currentUserIndex] -= totalBill;

                                                    string log = $"{accountNumbers[currentUserIndex]}: Paid {billName} Bill Worth PHP {bill:F2} + (Surcharge: PHP {surcharge:F2}) on {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                                                    transactionHistory.Add(log);
                                                    userTransactionHistories.Add(log);
                                                    LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Paid {billName} Bill at {DateTime.Now}");

                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine($"\nSuccessfully paid PHP {totalBill:F2}.");
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    Console.WriteLine($"New balance: PHP {balances[currentUserIndex]:F2}");
                                                    Console.ResetColor();

                                                    // Only reflect surcharge in ATM machine
                                                    int remainingSurcharge = surcharge;
                                                    for (int i = 0; i < billDenominations.Length; i++)
                                                    {
                                                        while (remainingSurcharge >= billDenominations[i])
                                                        {
                                                            billCounts[i]++;
                                                            atmCash += billDenominations[i];
                                                            remainingSurcharge -= billDenominations[i];
                                                        }
                                                    }

                                                    try
                                                    {
                                                        File.AppendAllText(billsData, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},BILL PAYMENT - {billName.ToUpper()},{bill:F2},{balances[currentUserIndex]:F2},{accountNumbers[currentUserIndex]}\n");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("Error saving receipt: " + ex.Message);
                                                        Console.ResetColor();
                                                    }
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("\nInsufficient balance.");
                                                }
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nPayment cancelled.");
                                                Console.ReadKey();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nInvalid bill type.");
                                        }

                                        Console.WriteLine("\nPress any key to return...");
                                        Console.ReadKey();
                                        break;



                                    case 5: // Fund Transfer
                                        Console.Clear();

                                        Console.ForegroundColor = ConsoleColor.White;
                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Access Fund Transfer at {DateTime.Now}");
                                        Console.ResetColor();

                                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                                        Console.Write("\nEnter recipient account number: ");
                                        string recipientAccount = Console.ReadLine();
                                        int recipientIndex = -1;

                                        for (int i = 0; i < accountNumbers.Count; i++)
                                        {
                                            if (accountNumbers[i] == recipientAccount && i != currentUserIndex)
                                            {
                                                recipientIndex = i;
                                                break;
                                            }
                                        }

                                        if (recipientIndex != -1)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.Write("\nEnter transfer amount (whole number): PHP ");
                                            if (int.TryParse(Console.ReadLine(), out int transferAmount) && transferAmount > 0 && transferAmount <= balances[currentUserIndex])
                                            {
                                                int remaining = transferAmount;
                                                int[] billsTransferred = new int[billDenominations.Length];

                                                for (int i = 0; i < billDenominations.Length; i++)
                                                {
                                                    while (remaining >= billDenominations[i])
                                                    {
                                                        remaining -= billDenominations[i];
                                                        billsTransferred[i]++;
                                                    }
                                                }

                                                if (remaining == 0)
                                                {
                                                    balances[currentUserIndex] -= transferAmount;
                                                    balances[recipientIndex] += transferAmount;

                                                    // Add to sender and recipient transaction history
                                                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                    string senderLog = $"{accountNumbers[currentUserIndex]}: Sent PHP {transferAmount:F2} to {accountNumbers[recipientIndex]} on {timestamp}";
                                                    string recipientLog = $"{accountNumbers[recipientIndex]}: Received PHP {transferAmount:F2} from {accountNumbers[currentUserIndex]} on {timestamp}";

                                                    userTransactionHistories.Add(senderLog);
                                                    userTransactionHistories.Add(recipientLog);
                                                    transactionHistory.Add(senderLog);
                                                    transactionHistory.Add(recipientLog);
                                                    LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Performed Fund Transfer at {timestamp}");

                                                    // Display success
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine($"\nSuccessfully transferred PHP {transferAmount:F2} to {accountNames[recipientIndex]}.");
                                                    Console.WriteLine($"New balance: PHP {balances[currentUserIndex]:F2}");

                                                   
                                                    // Save updated balances to file
                                                    try
                                                    {
                                                        string updated = "";
                                                        for (int i = 0; i < accountNumbers.Count; i++)
                                                        {
                                                            updated += $"{accountNumbers[i]},{balances[i]:F2}\n";
                                                        }
                                                        File.WriteAllText("balances.csv", updated);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine($"Error saving balances: {ex.Message}");
                                                    }

                                                    // Write receipt
                                                    try
                                                    {
                                                        string receipt = $"{timestamp},FUND TRANSFER,{transferAmount:F2},{balances[currentUserIndex]:F2},{accountNumbers[currentUserIndex]}\n";
                                                        File.AppendAllText(receiptData, receipt);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine($"Error generating receipt: {ex.Message}");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("\nTransfer amount must match available denominations (e.g., 100s, 500s, etc.).");
                                                }
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nInvalid amount (must be whole numbers) or insufficient balance.");
                                            }
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nRecipient account not found or cannot transfer to same account.");
                                        }

                                        Console.ResetColor();
                                        Console.ReadKey();
                                        break;


                                    case 6: // Change PIN
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\nEnter current PIN: ");
                                        string currentPin = Console.ReadLine();

                                        Console.ForegroundColor = ConsoleColor.White;
                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Access Change Pin at {DateTime.Now}"); // add to logged system
                                        Console.ResetColor();

                                        if (currentPin == pins[currentUserIndex])
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.Write("\nEnter new PIN (4 digits): ");
                                            string newPin = Console.ReadLine();

                                            Console.Write("\nRe-enter new PIN to confirm: ");
                                            string confirmPin = Console.ReadLine();

                                            bool validPin = newPin.Length == 4 && newPin.All(char.IsDigit); // check if newPin is 4 digits

                                            if (!validPin)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nInvalid PIN format. PIN must be 4 digits.");
                                            }
                                            else if (newPin != confirmPin)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nPIN confirmation does not match.");
                                            }
                                            else if (newPin == currentPin)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nNew PIN must be different from the current PIN.");
                                            }
                                            else
                                            {
                                                pins[currentUserIndex] = newPin;
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine("\nPIN successfully changed.");

                                                Console.ForegroundColor = ConsoleColor.White;
                                                LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Succesfully Changed Pin at {DateTime.Now}"); // add to logged system
                                                Console.ResetColor();

                                            }
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nIncorrect current PIN.");
                                        }

                                        Console.ResetColor();
                                        Console.WriteLine("\nPress any key to return...");
                                        Console.ReadKey();
                                        break;



                                    case 7: // Loan/Pay Loan

                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("\n=== Loan Service ===");
                                        Console.ResetColor();
                                        Console.WriteLine("\n1. Apply for Loan");
                                        Console.WriteLine("\n2. Pay Loan");
                                        Console.Write("\nChoose option: ");

                                        Console.ForegroundColor = ConsoleColor.White;
                                        LoggedSystem.Add(accountNumbers[currentUserIndex] + ": Access Loan/Pay Loan at " + DateTime.Now);
                                        Console.ResetColor();

                                        if (int.TryParse(Console.ReadLine(), out int loanOption))
                                        {
                                            switch (loanOption)
                                            {
                                                case 1:
                                                    Console.Clear();
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    LoggedSystem.Add(accountNumbers[currentUserIndex] + ": Access Loan at " + DateTime.Now);
                                                    Console.ResetColor();

                                                    if (userHasActiveLoan[currentUserIndex])
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("\nYou already have an active loan. Please pay it off before applying again.");
                                                        Console.ResetColor();
                                                        break;
                                                    }

                                                    Console.WriteLine("\nLoan Application - Simple Interest");
                                                    Console.Write("\nEnter loan amount (whole number): PHP ");

                                                    if (int.TryParse(Console.ReadLine(), out int loanAmount) && loanAmount > 0)
                                                    {
                                                        Console.Write("Enter loan term (in months): ");
                                                        if (int.TryParse(Console.ReadLine(), out int loanTerm) && loanTerm > 0)
                                                        {
                                                            double interestRate = 0.05;
                                                            double totalInterest = loanAmount * interestRate * loanTerm;
                                                            double totalAmount = loanAmount + totalInterest;
                                                            double monthlyPayment = totalAmount / loanTerm;
                                                            DateTime dueDate = DateTime.Now.AddMonths(1);

                                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                                            Console.WriteLine("\nLoan Summary:");
                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("\nPrincipal Amount: PHP " + loanAmount.ToString("F2"));
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("\nInterest Rate: 5% per month");
                                                            Console.WriteLine("\nLoan Term: " + loanTerm + " months");
                                                            Console.WriteLine("\nTotal Interest: PHP " + totalInterest.ToString("F2"));
                                                            Console.WriteLine("\nTotal Amount to Pay: PHP " + totalAmount.ToString("F2"));
                                                            Console.WriteLine("\nMonthly Payment: PHP " + monthlyPayment.ToString("F2"));
                                                            Console.WriteLine("\nFirst Due Date: " + dueDate.ToString("yyyy-MM-dd"));
                                                            Console.ResetColor();

                                                            Console.Write("\nApprove loan? (y/n): ");
                                                            string approve = Console.ReadLine();

                                                            if (approve.ToLower() == "y")
                                                            {
                                                                balances[currentUserIndex] += loanAmount;
                                                                userLoanAmounts[currentUserIndex] = loanAmount;
                                                                userLoanTerms[currentUserIndex] = loanTerm;
                                                                userHasActiveLoan[currentUserIndex] = true;

                                                                int remainingLoan = loanAmount;
                                                                for (int i = 0; i < billDenominations.Length; i++)
                                                                {
                                                                    while (remainingLoan >= billDenominations[i])
                                                                    {
                                                                        if (billCounts[i] > 0)
                                                                        {
                                                                            remainingLoan -= billDenominations[i];
                                                                            billCounts[i]--;
                                                                            atmCash -= billDenominations[i];
                                                                        }
                                                                        else
                                                                        {
                                                                            break;
                                                                        }
                                                                    }
                                                                }

                                                                transactionHistory.Add(accountNumbers[currentUserIndex] + ": Loan approved PHP " + loanAmount);
                                                                userTransactionHistories.Add(accountNumbers[currentUserIndex] + ": Loan approved PHP " + loanAmount);

                                                                LoggedSystem.Add(accountNumbers[currentUserIndex] + ": Loan approved at " + DateTime.Now);
                                                                Console.ForegroundColor = ConsoleColor.Green;
                                                                Console.WriteLine("\nLoan amount added to your balance.\n");
                                                                Console.ResetColor();
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("\nLoan application cancelled.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("\nInvalid loan term.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("\nInvalid loan amount.");
                                                    }
                                                    break;

                                                case 2:
                                                    Console.Clear();
                                                    Console.WriteLine("\n=== Pay Loan ===");

                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    LoggedSystem.Add(accountNumbers[currentUserIndex] + ": Access Pay Loan at " + DateTime.Now);
                                                    Console.ResetColor();

                                                    if (!userHasActiveLoan[currentUserIndex] || userLoanAmounts[currentUserIndex] <= 0)
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("\nNo active loan to pay.");
                                                        Console.ResetColor();
                                                        break;
                                                    }

                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.WriteLine("\nChoose Payment Option:");
                                                    Console.WriteLine("1. Pay Full Amount");
                                                    Console.WriteLine("2. Simulate Monthly Payments");
                                                    Console.ResetColor();
                                                    Console.Write("\nEnter option (1 or 2): ");
                                                    string option = Console.ReadLine();

                                                    if (option == "1")
                                                    {
                                                        Console.Write("\nEnter how many days have passed since loan approval: ");
                                                        if (int.TryParse(Console.ReadLine(), out int daysPassed) && daysPassed >= 0)
                                                        {
                                                            double interestRate = 0.05;
                                                            double dailyRate = interestRate / 30.0;
                                                            double principal = userLoanAmounts[currentUserIndex];
                                                            int loanTermMonths = userLoanTerms[currentUserIndex];
                                                            double fullTermInterest = principal * interestRate * loanTermMonths;
                                                            double baseTotalAmount = principal + fullTermInterest;
                                                            double dailyInterest = principal * dailyRate * daysPassed;
                                                            double finalTotalAmount = baseTotalAmount + dailyInterest;

                                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                                            Console.WriteLine("\nLoan aged for " + daysPassed + " day(s)");
                                                            Console.WriteLine("\nBase Amount Due: PHP " + baseTotalAmount.ToString("N0"));
                                                            Console.WriteLine("\nDaily Interest (" + daysPassed + " days): PHP " + dailyInterest.ToString("N0"));
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("\nFINAL PAYMENT REQUIRED: PHP " + finalTotalAmount.ToString("N0"));
                                                            Console.ResetColor();

                                                            Console.Write("\nEnter payment amount: PHP ");
                                                            if (double.TryParse(Console.ReadLine(), out double payAmount) && payAmount > 0)
                                                            {
                                                                if ((int)payAmount != (int)finalTotalAmount) // check if payAMount is equal to the final total amount 

                                                                {
                                                                        Console.WriteLine("\nYou must pay the full updated loan amount.");
                                                                }
                                                                else if (payAmount <= balances[currentUserIndex])
                                                                {
                                                                    balances[currentUserIndex] -= payAmount;

                                                                    string payLog = $"{accountNumbers[currentUserIndex]}: Loan Paid - PHP {payAmount:F2} on {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

                                                                    transactionHistory.Add(payLog);
                                                                    userTransactionHistories.Add(payLog);

                                                                    Console.ForegroundColor = ConsoleColor.White;
                                                                    LoggedSystem.Add(accountNumbers[currentUserIndex] + ": Paid Loan at " + DateTime.Now);
                                                                    Console.ResetColor();

                                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                                    Console.WriteLine("\nSuccessfully paid PHP " + payAmount.ToString("F2") + ".");
                                                                    Console.WriteLine("New balance: PHP " + balances[currentUserIndex].ToString("F2"));
                                                                    Console.ResetColor();

                                                                    userHasActiveLoan[currentUserIndex] = false;
                                                                    userLoanAmounts[currentUserIndex] = 0;
                                                                    userLoanTerms[currentUserIndex] = 0;

                                                                    // Re-add cash to ATM with denominations
                                                                    int remainingCash = (int)payAmount;
                                                                    for (int i = 0; i < billDenominations.Length; i++)
                                                                    {
                                                                        while (remainingCash >= billDenominations[i])
                                                                        {
                                                                            billCounts[i]++;
                                                                            atmCash += billDenominations[i];
                                                                            remainingCash -= billDenominations[i];
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                                    Console.WriteLine("\nInsufficient balance.");
                                                                    Console.ResetColor();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("\nInvalid payment amount.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("\nInvalid days input.");
                                                        }
                                                    }
                                                    else if (option == "2")
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.White;
                                                        LoggedSystem.Add(accountNumbers[currentUserIndex] + ": Access Loan Monthly Simulation at " + DateTime.Now);
                                                        Console.ResetColor();

                                                        double principal = userLoanAmounts[currentUserIndex];
                                                        int termMonths = userLoanTerms[currentUserIndex];
                                                        double interestRate = 0.05;

                                                        double monthlyInterest = principal * interestRate;
                                                        double monthlyPrincipal = principal / termMonths;
                                                        double monthlyPayment = monthlyPrincipal + monthlyInterest;
                                                        double remainingBalance = principal;

                                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                                        Console.WriteLine("\nSimulating fixed monthly payments over " + termMonths + " month(s):");
                                                        Console.ResetColor();

                                                        for (int month = 1; month <= termMonths; month++)
                                                        {
                                                            remainingBalance -= monthlyPrincipal;
                                                            if (remainingBalance < 0) remainingBalance = 0;

                                                            Console.WriteLine($"\nMonth {month}: Paid PHP {monthlyPayment:F2} | Interest: PHP {monthlyInterest:F2} | Remaining Balance: PHP {remainingBalance:F2}");


                                                        }

                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.WriteLine("\nSimulation complete. Final balance after payments: PHP 0.00");
                                                        Console.ResetColor();
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("\nInvalid option selected.");
                                                    }

                                                    break;

                                                default:
                                                    Console.WriteLine("\nInvalid choice.");
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nInvalid input.");
                                        }

                                        Console.ReadKey();
                                        break;








                                    case 8: // View Transaction History
                                        Console.Clear();
                                        Console.WriteLine("\n=== Your Transaction History ===\n");

                                        // Log the access
                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Accessed Transaction History at {DateTime.Now}");

                                        string currentUserHistory = $"{accountNumbers[currentUserIndex]}:";

                                        bool foundTransaction = false;

                                        foreach (string log in userTransactionHistories)
                                        {
                                            if (log.StartsWith(currentUserHistory))
                                            {
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine(log);
                                                Console.WriteLine();
                                                foundTransaction = true;
                                            }
                                        }

                                        if (!foundTransaction)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("No transactions found.");
                                        }

                                        Console.ResetColor();
                                        Console.WriteLine("\nPress any key to return...");
                                        Console.ReadKey();
                                        break;
                   
                                    case 9: // Contact Admin
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.WriteLine("\n=== Contact Admin ===");
                                        Console.WriteLine("\n1. Send Message to Admin");
                                        Console.WriteLine("\n2. View Admin Replies");
                                        Console.WriteLine("\n3. Back to Main Menu");
                                        Console.Write("\nChoose option: ");

                                        Console.ForegroundColor = ConsoleColor.White;
                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Accessed Contact Admin Option at {DateTime.Now}");
                                        Console.ResetColor();

                                        if (int.TryParse(Console.ReadLine(), out int contactChoice))
                                        {
                                            switch (contactChoice) // 3 cases for case 9
                                            {
                                                case 1: // Send Message
                                                    Console.Clear();
                                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                    Console.WriteLine("\n=== Send Message to Admin ===");
                                                    Console.WriteLine($"\nAccount Name: {accountNames[currentUserIndex]}");
                                                    Console.WriteLine($"\nAccount Number: {accountNumbers[currentUserIndex]}");
                                                    Console.Write("\nEnter your message: ");
                                                    string userMessage = Console.ReadLine();

                                                    if (!string.IsNullOrWhiteSpace(userMessage))
                                                    {
                                                        userMessageAccounts.Add(accountNumbers[currentUserIndex]); //add to the list (account)

                                                        userMessageNames.Add(accountNames[currentUserIndex]); // add to the list (Names)

                                                        userMessages.Add(userMessage); // storage list of user message

                                                        userMessageTimes.Add(DateTime.Now); // add to the list ( Date and Time)

                                                        adminReplies.Add("");  // blank admin  

                                                        hasAdminReply.Add(false); // no admin reply yet

                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.WriteLine("\nMessage sent successfully!");
                                                        Console.ResetColor();

                                                        Console.ForegroundColor = ConsoleColor.White;
                                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: Sent an Message to Admin at {DateTime.Now}");
                                                        Console.ResetColor();
                                                    }
                                                    else
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("\nMessage cannot be empty.");
                                                        Console.ResetColor();
                                                    }
                                                    Console.WriteLine("\nPress any key to return...");
                                                    Console.ReadKey();
                                                    break;

                                                case 2: // View Replies
                                                    Console.Clear();
                                                    Console.WriteLine("=== Admin Replies ===");
                                                    bool foundReplies = false;

                                                    for (int i = 0; i < userMessageAccounts.Count; i++)
                                                    {
                                                        if (userMessageAccounts[i] == accountNumbers[currentUserIndex])
                                                        {
                                                            Console.WriteLine($"\nMessage Sent: {userMessages[i]}");
                                                            Console.WriteLine($"\nSent On: {userMessageTimes[i]}");

                                                            if (hasAdminReply[i])
                                                            {
                                                                Console.ForegroundColor = ConsoleColor.Green;
                                                                Console.WriteLine($"\nAdmin Reply: {adminReplies[i]}");
                                                            }
                                                            else
                                                            {
                                                                Console.ForegroundColor = ConsoleColor.Red;
                                                                Console.WriteLine("\nNo reply yet.");
                                                            }
                                                            Console.ResetColor();
                                                            foundReplies = true;
                                                        }
                                                    }

                                                    if (!foundReplies)
                                                    {
                                                        Console.WriteLine("\nNo messages or replies found for your account.");
                                                    }

                                                    Console.WriteLine("\nPress any key to return...");
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: view Admin Reply at {DateTime.Now}");
                                                    Console.ResetColor();

                                                    Console.WriteLine("\nPress any key to return...");
                                                    Console.ReadKey();
                                                    break;

                                                case 3: // Back
                                                    Console.Clear();
                                                    break;

                                                default:
                                                    Console.WriteLine("\nInvalid option. Press any key to return...");
                                                    Console.ReadKey();
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nInvalid input. Press any key to return...");
                                            Console.ReadKey();
                                        }
                                        break;



                                    case 10: // log out and back to login
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Yellow;

                                        Console.ForegroundColor = ConsoleColor.White;
                                        LoggedSystem.Add($"{accountNumbers[currentUserIndex]}: logged out at {DateTime.Now}");
                                        Console.ResetColor();

                                        Console.WriteLine("\nLogging out...");
                                        Console.ResetColor();
                                        usingATM = false;
                                        break;


                                }
                            }


                        }
                        else // Admin operations
                        {

                            switch (choice)
                            {
                                // Complete the missing admin cases - insert these into your switch statement for admin operations

                                case 1: // Create/Delete User
                                    Console.Clear();
                                    Console.WriteLine("\n1. Create User");
                                    Console.WriteLine("\n2. Delete User");
                                    Console.Write("\nChoose option: ");
                                    if (int.TryParse(Console.ReadLine(), out int userOption))
                                    {
                                        if (userOption == 1) // Create User
                                        {
                                            Console.Write("\nEnter new user name (First and Last): ");
                                            string newUserName = Console.ReadLine().Trim();

                                            bool validNewNameFormat = false;
                                            bool isDuplicate = false;

                                            if (!string.IsNullOrWhiteSpace(newUserName))
                                            {
                                                string[] newNameParts = newUserName.Split(' '); // ensures the first and last name are separated by a space and first name letter and last name letter is capital letter
                                                if (newNameParts.Length == 2)
                                                {
                                                    bool firstPartValid = !string.IsNullOrEmpty(newNameParts[0]) && char.IsUpper(newNameParts[0][0]);
                                                    bool secondPartValid = !string.IsNullOrEmpty(newNameParts[1]) && char.IsUpper(newNameParts[1][0]);
                                                    bool allLettersFirst = newNameParts[0].All(char.IsLetter);
                                                    bool allLettersSecond = newNameParts[1].All(char.IsLetter);

                                                    if (accountNames.Contains(newUserName)) // check if the new user name already exists
                                                    {
                                                        isDuplicate = true;
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("\nUser name already exists. Please choose a different name.");
                                                    }
                                                    else
                                                    {
                                                        Console.ResetColor();
                                                        validNewNameFormat = firstPartValid && secondPartValid && allLettersFirst && allLettersSecond && !isDuplicate;
                                                    }
                                                
                                                }
                                            }

                                            if (validNewNameFormat)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.Write("\nEnter initial balance: PHP ");
                                                if (double.TryParse(Console.ReadLine(), out double initialBalance) && initialBalance >= 0)
                                                {
                                                    Console.Write("Enter PIN (4 digits): ");
                                                    string newPin = Console.ReadLine();

                                                    if (newPin.Length == 4 && newPin.All(char.IsDigit))
                                                    {
                                                        int highestAccountNumber = 10000000; // base

                                                        for (int i = 0; i < accountNumbers.Count; i++) // maintain order when adding account
                                                        {
                                                            int accNum;

                                                            if (int.TryParse(accountNumbers[i], out accNum))
                                                            {
                                                                if (accNum > highestAccountNumber)
                                                                {
                                                                    highestAccountNumber = accNum;
                                                                }
                                                            }
                                                        }
                                                        string newAccountNumber = "" + (highestAccountNumber + 1);



                                                        accountNames.Add(newUserName);
                                                        accountNumbers.Add(newAccountNumber);
                                                        pins.Add(newPin);
                                                        balances.Add(initialBalance);
                                                        accountStatus.Add(true);

                                                        // Save account info
                                                        List<string> updatedAccounts = new List<string>();
                                                        for (int i = 0; i < accountNumbers.Count; i++)
                                                        {
                                                            updatedAccounts.Add($"{accountNumbers[i]},{accountNames[i]},{pins[i]},{accountStatus[i]}");
                                                        }
                                                        File.WriteAllLines(accountData, updatedAccounts);

                                                        // Save balances
                                                        List<string> updatedBalances = new List<string>();
                                                        for (int i = 0; i < accountNumbers.Count; i++)
                                                        {
                                                            updatedBalances.Add($"{accountNumbers[i]},{balances[i]}");
                                                        }
                                                        File.WriteAllLines(balanceData, updatedBalances);

                                                        // Save card input
                                                        List<string> updatedCards = new List<string>();
                                                        for (int i = 0; i < accountNumbers.Count; i++)
                                                        {
                                                            string line = $"{accountNumbers[i]}:{accountNames[i]}";
                                                            if (!updatedCards.Contains(line)) updatedCards.Add(line);
                                                        }
                                                        File.WriteAllLines(physicalCard, updatedCards);


                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.WriteLine("\nUser created successfully!");
                                                        Console.WriteLine($"Name: {newUserName}");
                                                        Console.WriteLine($"Account Number: {newAccountNumber}");
                                                        Console.WriteLine($"Initial Balance: PHP {initialBalance:F2}");

                                                        balances.Add(initialBalance);

                                                        // Automatically update bill counts from initial balance
                                                        int tempBalance = (int)initialBalance;
                                                        for (int i = 0; i < billDenominations.Length; i++)
                                                        {
                                                            int count = tempBalance / billDenominations[i];
                                                            billCounts[i] += count;
                                                            tempBalance %= billDenominations[i]; // use modulo to break down the bill denomination accurately
                                                        }

                                                    }
                                                    else
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("\nInvalid PIN format. Must be 4 digits.");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("\nInvalid balance amount.");
                                                }
                                            }
                                            else if (!isDuplicate)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nInvalid name format. Use 'First Last' with proper capitalization.");
                                            }
                                        }
                                        else if (userOption == 2) // Delete User
                                        {
                                            Console.Clear();
                                            Console.WriteLine("\n=== Delete User ===");
                                            Console.WriteLine("\nCurrent Users:");
                                            for (int i = 0; i < accountNames.Count; i++)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Cyan;
                                                Console.WriteLine($"{i + 1}. {accountNames[i]} - {accountNumbers[i]}");
                                            }

                                            Console.ResetColor();
                                            Console.Write("\nEnter account number to delete: ");
                                            string accountToDelete = Console.ReadLine().Trim();
                                            int deleteIndex = accountNumbers.IndexOf(accountToDelete);

                                            if (deleteIndex != -1)
                                            {
                                                Console.Write($"\n Are you sure you want to delete {accountNames[deleteIndex]}? (y/n): ");
                                                string confirm = Console.ReadLine().ToLower();

                                                if (confirm == "y") // confirm deletion
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;

                                                    // Get balance before removing it from the list
                                                    int tempBalance = (int)balances[deleteIndex]; // Get deleted user's balance

                                                    // Remove from in-memory lists
                                                    accountNames.RemoveAt(deleteIndex);
                                                    accountNumbers.RemoveAt(deleteIndex);
                                                    pins.RemoveAt(deleteIndex);
                                                    balances.RemoveAt(deleteIndex);
                                                    accountStatus.RemoveAt(deleteIndex);
                                                    lockedAccount.Remove(accountToDelete);

                                                    // Automatically deduct bill counts from deleted user's balance
                                                    for (int i = 0; i < billDenominations.Length; i++)
                                                    {
                                                        int count = tempBalance / billDenominations[i];
                                                        billCounts[i] -= count;
                                                        tempBalance %= billDenominations[i]; // use modulo to break down the bill denomination accurately
                                                    }



                                                    string accountFile = Path.Combine(mainDirectory, "account.csv");

                                                    // Rebuild the file content
                                                    List<string> updatedAccounts = new List<string>();
                                                    for (int i = 0; i < accountNumbers.Count; i++)
                                                    {
                                                        string status = "";
                                                        if (accountStatus[i] == true)
                                                        {
                                                            status = "true";
                                                        }
                                                        else
                                                        {
                                                            status = "false";
                                                        }

                                                        updatedAccounts.Add(accountNumbers[i] + "," + accountNames[i] + "," + pins[i] + "," + status);
                                                    }

                                                
                                                    File.WriteAllLines(accountFile, updatedAccounts);

                                                    string cardFile = Path.Combine(mainDirectory, "card_input.txt");

                                                    // Update card_input.txt after deletion
                                                    List<string> updatedCards = new List<string>();
                                                    for (int i = 0; i < accountNumbers.Count; i++)
                                                    {
                                                        string line = accountNumbers[i] + "," + accountNames[i];
                                                        if (!updatedCards.Contains(line))
                                                        {
                                                            updatedCards.Add(line);
                                                        }
                                                    }
                                                    File.WriteAllLines(cardFile, updatedCards); 


                                                    Console.WriteLine("\nUser deleted successfully.");
                                                    Console.ResetColor();
                                                }

                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.WriteLine("\nUser deletion cancelled.");
                                                }
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nAccount number not found.");
                                            }
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nInvalid option. Please select 1 or 2.");
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\nInvalid input. Enter a number.");
                                    }

                                    Console.ResetColor();
                                    Console.WriteLine("\nPress any key to return to menu...");
                                    Console.ReadKey();
                                    break;


                                case 2: // Transaction History of User (Admin View)
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("\n=== User Transaction History ===");
                                    Console.Write("\nEnter account number: ");
                                    string searchAccount = Console.ReadLine().Trim();
                                    int searchIndex = accountNumbers.IndexOf(searchAccount);

                                    if (searchIndex != -1)
                                    {
                                      Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.WriteLine($"\nTransaction History for {accountNames[searchIndex]} ({searchAccount}):");
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"\nCurrent Balance: PHP {balances[searchIndex]:F2}");
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("\nTransactions:");
                                        Console.WriteLine();

                                        bool hasUserTransactions = false;
                                        int count = 1;

                                        foreach (string log in transactionHistory)
                                        {
                                            // Show any transaction involving the user's account number
                                            if (log.StartsWith(searchAccount))
                                            {
                                                Console.WriteLine($"{count++}. {log}");
                                                hasUserTransactions = true;
                                            }
                                        }

                                        if (!hasUserTransactions)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("No transactions found for this user.");
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\nAccount not found.");
                                    }
                                    Console.ResetColor();
                                    Console.WriteLine("\nPress any key to return...");
                                    Console.ReadKey();
                                    break;



                                case 3: // Account Management
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("\n=== Account Management ===");
                                    Console.WriteLine("\n1. Lock Account");
                                    Console.WriteLine("\n2. Unlock Account & Reset PIN");
                                    Console.WriteLine("\n3. View Locked Accounts");
                                    Console.Write("\nChoose option: ");

                                    if (int.TryParse(Console.ReadLine(), out int lockOption))
                                    {
                                        switch (lockOption)
                                        {
                                            case 1: // Lock Account
                                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                Console.Write("\nEnter account number to lock: ");
                                                string lockAccount = Console.ReadLine().Trim();
                                                int lockIndex = accountNumbers.IndexOf(lockAccount);

                                                if (lockIndex != -1)
                                                {
                                                    accountStatus[lockIndex] = false;
                                                    if (!lockedAccount.Contains(lockAccount))
                                                    {
                                                        lockedAccount.Add(lockAccount);
                                                    }
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine($"\nAccount {accountNames[lockIndex]} has been locked.");
                                              
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("\nAccount not found.");
                                                }
                                                break;

                                            case 2: // Unlock Account & Reset PIN
                                                Console.Clear();
                                                Console.ForegroundColor = ConsoleColor.Cyan;
                                                Console.WriteLine("\n=== Unlock Account & Reset PIN ===");
                                                Console.Write("\nEnter account number: ");
                                                string resetAccount = Console.ReadLine().Trim();
                                                int resetIndex = accountNumbers.IndexOf(resetAccount);

                                                if (resetIndex != -1)
                                                {
                                                    // Check if the account is actually locked
                                                    if (!accountStatus[resetIndex])
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                                        Console.Write("\nEnter account holder's name for verification: ");
                                                        string verifyName = Console.ReadLine().Trim();

                                                        if (verifyName.ToLower() == accountNames[resetIndex].ToLower())
                                                        {
                                                            Console.Write("\nEnter new PIN (4 digits): ");
                                                            string newPin = Console.ReadLine();

                                                            if (newPin.Length == 4 && newPin.All(char.IsDigit))
                                                            {
                                                                pins[resetIndex] = newPin;
                                                                accountStatus[resetIndex] = true;
                                                                lockedAccount.Remove(resetAccount);
                                                                Console.ForegroundColor = ConsoleColor.Green;
                                                                Console.WriteLine($"\nPIN reset and account {accountNames[resetIndex]} has been unlocked.");
                                                            }
                                                            else
                                                            {
                                                                Console.ForegroundColor = ConsoleColor.Red;
                                                                Console.WriteLine("\nInvalid PIN format. Must be 4 digits.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("\nName verification failed. PIN not reset.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("\nThis account is not locked.");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("\nAccount not found.");
                                                }
                                                break;

                                            case 3: // View Locked Accounts
                                                Console.Clear();
                                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                                Console.WriteLine("\n=== Locked Accounts ===");
                                                if (lockedAccount.Count > 0)
                                                {
                                                    foreach (string locked in lockedAccount)
                                                    {
                                                        int lockedIndex = accountNumbers.IndexOf(locked); // find the index of the locked account

                                                        if (lockedIndex != -1)
                                                        {
                                                            Console.WriteLine($"- {accountNames[lockedIndex]} ({locked})");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("\nNo locked accounts.");
                                                }
                                                break;
                                        }
                                    }
                                    Console.ReadKey();
                                    break;



                                case 5: // Restock Cash
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine("\n=== ATM Cash Restock ===");

                                    double totalCash = 0;

                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine("\nCurrent Bill Distribution:");
                                    for (int i = 0; i < billDenominations.Length; i++)
                                    {
                                        double denomTotal = billDenominations[i] * billCounts[i];
                                        totalCash += denomTotal;
                                        Console.WriteLine($"\nPHP {billDenominations[i]}: {billCounts[i]} bills = PHP {denomTotal:F2}");
                                    }

                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine($"\nCurrent ATM Cash: PHP {totalCash:F2}");

                                    Console.Write("\nEnter denomination to restock (1000, 500, 200, 100, 50, 20): ");
                                    if (int.TryParse(Console.ReadLine(), out int restockDenom))
                                    {
                                        int denomIndex = Array.IndexOf(billDenominations, restockDenom);

                                        if (denomIndex != -1)
                                        {
                                            Console.Write($"\nEnter number of PHP {restockDenom} bills to add: ");
                                            if (int.TryParse(Console.ReadLine(), out int addBills) && addBills > 0)
                                            {
                                                billCounts[denomIndex] += addBills;

                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine($"\nSuccessfully added {addBills} x PHP {restockDenom} bills.");

                                                // Recalculate total ATM cash
                                                totalCash = 0;
                                                for (int i = 0; i < billDenominations.Length; i++)
                                                {
                                                    totalCash += billDenominations[i] * billCounts[i];
                                                }

                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine($"\nNew ATM Cash Total: PHP {totalCash:F2}");
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nInvalid number of bills.");
                                            }
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("\nInvalid denomination.");
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\nInvalid input.");
                                    }

                                    Console.ResetColor();
                                    Console.WriteLine("\nPress any key to return...");
                                    Console.ReadKey();
                                    break;

                                case 4: // View ATM Cash Status
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\n=== ATM Cash Status ===");

                                    double totalCashStatus = 0;

                                    Console.WriteLine("\nBill Distribution:");
                                    for (int i = 0; i < billDenominations.Length; i++)
                                    {
                                        int billValue = billDenominations[i];
                                        int count = billCounts[i];
                                        double denomTotal = billValue * count;
                                        totalCashStatus += denomTotal;

                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.WriteLine($"\nPHP {billValue}: {count} bill(s) = PHP {denomTotal:F2}");
                                    }

                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine($"\nTotal ATM Cash (calculated from bill counts): PHP {totalCashStatus:F2}");

                                    Console.ResetColor();
                                    Console.WriteLine("\nPress any key to return...");
                                    Console.ReadKey();
                                    break;



                                case 6: // View All User Questions and Summary
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("\n=== User Messages and Admin Management ===");
                                    Console.WriteLine("\n1. View All User Messages");
                                    Console.WriteLine("\n2. Reply to User Message");
                                    Console.WriteLine("\n3. View Message Summary");
                                    Console.Write("\nChoose option: ");

                                    if (int.TryParse(Console.ReadLine(), out int messageOption))
                                    {
                                        switch (messageOption)
                                        {
                                            case 1: // View All Messages
                                                Console.Clear();
                                                Console.WriteLine("\n=== All User Messages ===");
                                                if (userMessages.Count > 0)
                                                {
                                                    for (int i = 0; i < userMessages.Count; i++)
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                                        Console.WriteLine($"\n--- Message {i + 1} ---");
                                                        Console.WriteLine($"\nFrom: {userMessageNames[i]} ({userMessageAccounts[i]})");
                                                        Console.WriteLine($"\nSent: {userMessageTimes[i]}");
                                                        Console.WriteLine($"\nMessage: {userMessages[i]}");
                                                        Console.WriteLine($"\nAdmin Reply: ");

                                                            if (hasAdminReply[i])
                                                            {
                                                            // show the reply
                                                            }
                                                            else if (hasAdminReply[i])
                                                            {
                                                            Console.WriteLine("\nNo reply yet");
                                                            }




                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\nNo user messages found.");
                                                }
                                                Console.WriteLine("\nPress any key to return...");
                                                Console.ReadKey();
                                                break;

                                            case 2: // Reply to Message
                                                Console.Clear();
                                                Console.WriteLine("\n=== Reply to User Message ===");
                                                if (userMessages.Count > 0)
                                                {
                                                    for (int i = 0; i < userMessages.Count; i++)
                                                    {
                                                        Console.WriteLine($"\n{i + 1}. {userMessageNames[i]}: {userMessages[i].Substring(0, Math.Min(40, userMessages[i].Length))}...");
                                                    }

                                                    Console.Write("\nEnter message number to reply to: ");

                                                    if (int.TryParse(Console.ReadLine(), out int replyIndex) && replyIndex > 0 && replyIndex <= userMessages.Count)
                                                    {
                                                        replyIndex--; // convert to index
                                                        Console.Clear();
                                                        Console.WriteLine($"\nMessage from: {userMessageNames[replyIndex]}");
                                                        Console.WriteLine($"\nAccount: {userMessageAccounts[replyIndex]}");
                                                        Console.WriteLine($"\nMessage: {userMessages[replyIndex]}");
                                                        Console.WriteLine($"\nCurrent Reply: {(hasAdminReply[replyIndex] ? adminReplies[replyIndex] : "No reply yet")}");
                                                        Console.Write("\nEnter your reply: ");
                                                        string adminReply = Console.ReadLine();

                                                        if (!string.IsNullOrWhiteSpace(adminReply))
                                                        {
                                                            adminReplies[replyIndex] = adminReply;
                                                            hasAdminReply[replyIndex] = true;

                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("\nReply sent successfully!");
                                                            Console.ReadKey();
                                                            Console.ResetColor();
                                                        }
                                                        else
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("\nReply cannot be empty.");
                                                            Console.ReadKey();
                                                            Console.ResetColor();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("\nInvalid message number.");
                                                        Console.ReadKey();
                                                    }
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("\nNo messages to reply to.");
                                                    Console.ReadKey();
                                                }
                                                Console.WriteLine("\nPress any key to return...");
                                                Console.ReadKey();
                                                break;

                                            case 3: // Message Summary
                                                Console.Clear();
                                                Console.WriteLine("\n=== Message Summary ===");
                                                Console.WriteLine($"\nTotal Messages: {userMessages.Count}");

                                                int repliedCount = 0;

                                                for (int i = 0; i < hasAdminReply.Count; i++)
                                                {
                                                    if (hasAdminReply[i]) repliedCount++;
                                                }
                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.WriteLine($"\nReplied Messages: {repliedCount}");
                                                Console.WriteLine($"\nPending Messages: {userMessages.Count - repliedCount}");
                                                Console.WriteLine($"\nLocked Accounts Awaiting Admin: {lockedAccount.Count}");

                                                if (lockedAccount.Count > 0)
                                                {
                                                    Console.WriteLine("\nAccounts Needing Attention:");
                                                    for (int i = 0; i < lockedAccount.Count; i++)
                                                    {
                                                        string acc = lockedAccount[i];
                                                        for (int j = 0; j < accountNumbers.Count; j++)
                                                        {
                                                            if (acc == accountNumbers[j])
                                                            {
                                                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                                                Console.WriteLine($"\n- {accountNames[j]} ({acc})");
                                                            }
                                                        }
                                                    }
                                                }

                                                Console.WriteLine("\nPress any key to return...");
                                                Console.ReadKey();
                                                break;
                                        }
                                    }
                                    break;



                                case 7: // View System Logs
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.WriteLine("\n=== System Logs ===");

                            
                                    Console.WriteLine("\nRecent Activity Logs:");
                                    Console.WriteLine();

                                    Console.ForegroundColor = ConsoleColor.White;
                                    foreach (string log in LoggedSystem)
                                    {
                                        Console.WriteLine($"- {log}");
                                        Console.WriteLine();
                                    }
                                    Console.ReadKey();
                                    break;



                                case 8: // View All Accounts
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine("\n=== All Accounts ===");

                                    if (accountNames.Count == 0)

                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\nNo active accounts found.");
                                    }
                                    else
                                    {
                                        for (int i = 0; i < accountNames.Count; i++)
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            Console.Write($"\n{i + 1}. ");

                                            Console.ForegroundColor = ConsoleColor.Cyan;
                                            Console.Write($"Name: {accountNames[i]}, ");

                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.Write($"Account Number: {accountNumbers[i]}, ");

                                            if (accountStatus[i]) // if true
                                            {
                                              Console.ForegroundColor = ConsoleColor.Green;
                                              Console.Write("Active");
                                            }
                                            else // if false
                                            {
                                              Console.ForegroundColor = ConsoleColor.Red;
                                              Console.Write("Locked");
                                            }

                                                Console.ResetColor(); // Reset to default color after status

                                            Console.WriteLine();
                                        }
                                        Console.ResetColor();
                                        Console.WriteLine("\nPress any key to return to menu...");
                                    }
                                    Console.ReadKey();                              
                                    break;

                                case 9: // Logout
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("\nAdmin logging out...");
                                    Console.ResetColor();
                                    usingATM = false;
                                    break;

                                case 10: // Exit Program
                                    exitProgram = true;
                                    usingATM = false;
                                    break;

                                default:
                                    Console.WriteLine("\nInvalid choice. Please try again.");
                                    break;
                            }
                        }


                    }
                }


                if (exitProgram) // Exit Program
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nExiting program...");
                    Console.ResetColor();
                    break;
                }
            }
            while (!exitProgram);
            {
                // Exit Program
            }
        }
    }
}
