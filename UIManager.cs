namespace CodingTracker
{
    public class UIManager
    {
        public bool isSessionActive;
        public DatabaseManager db;
        public UIManager(DatabaseManager db)
        {
            this.db = db;
        }

        public void MenuLoop()
        {
            int UserInput = 1;
            while (UserInput != 0)
            {
                isSessionActive = db.CheckForActiveSession();
                UserInput = GetUserInput(menuChoices, MainMenu);
                MainMenuFunctionSelect(UserInput);
            }

        }

        private string[] menuChoices = { "0", "1", "2", "3", "4", "5" };
        private string[] sessionMenuChoices = { "0", "1", "2" };

        private void MainMenu()
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("         MAIN MENU         ");
            Console.WriteLine("---------------------------\n");

            Console.WriteLine("0 - Exit Application");
            if (!isSessionActive)
                Console.WriteLine("1 - Clock On");
            else
                Console.WriteLine("1 - Clock Off");
            Console.WriteLine("2 - Enter new session");
            Console.WriteLine("3 - Update Session");
            Console.WriteLine("4 - Delete Session");
            Console.WriteLine("5 - View Reports");
            Console.Write("\nEnter option number: ");
        }

        private int GetUserInput(string[] choices, Action MenuToPrint)
        {
            string? userInput = string.Empty;
            while (!choices.Contains(userInput))
            {
                MenuToPrint();
                userInput = Console.ReadLine();
            }
            int intUserInput = int.Parse(userInput);
            return intUserInput;
        }

        private void MainMenuFunctionSelect(int intUserInput)
        {
            switch (intUserInput)
            {
                case 0: 
                    // Exit Program
                    Console.WriteLine("Goodbye!");
                    break;
                case 1: 
                    // Clock On/Off
                    if (!isSessionActive)
                    {
                        Console.WriteLine("New session started");
                        StartSession();
                    }
                    else
                    {
                        Console.WriteLine("Session Ended");
                        EndSession();
                    }
                    break;
                case 2:
                    // Enter new session
                    EnterNewSession();
                    break;
                case 3:
                    // Update session
                    UpdateSession();
                    break;
                case 4:
                    // Delete session
                    DeleteSession();
                    break;
                case 5:
                    // View reports
                    Console.WriteLine("View Reports...");


                    break;
            }
        }

        private void DeleteSession()
        {
            int id = GetUserInput(db.GetArrayOfIdsAsStrings(), DisplayActiveSessions);
            if (id != 0)
            {
                db.DeleteSession(id);
                Console.Write($"Session {id} deleted. Press enter to return to Main Menu. ");
                Console.ReadLine();
            }
        }

        private void UpdateSession()
        {
            int id = GetUserInput(db.GetArrayOfIdsAsStrings(), DisplayActiveSessions);
            if (id != 0)
            {
                int updateChoice = GetUserInput(sessionMenuChoices, UpdateSessionMenu);
                string date = DateOperations.EnterNewDate();
                string time = DateOperations.EnterNewTime();
                DateTime dt = DateOperations.ParseDateTime(date, time);

                db.UpdateSession(id, updateChoice, dt);
                Console.Write($"Session {id} updated. Press enter to return to Main Menu. ");
                Console.ReadLine();
            }
        }

        private void DisplayActiveSessions()
        {
            Console.WriteLine("\nSESSION LIST");
            ReportGeneration.DisplayAllRecords(db.RetrieveSessionList());
            Console.Write("\nEnter Id of session, or 0 to return to Main Menu: ");
        }

        private void EndSession()
        {
            db.ToggleActiveSession();
            db.UpdateSession(db.RetrieveActiveSessionId(), 2, DateTime.Now);
        }

        private void StartSession()
        {
            Session newSession = new();
            newSession.StartTime = DateTime.Now;
            db.WriteSessionToDatabase(newSession);
            db.ToggleActiveSession();
        }

        private void EnterNewSessionMenu()
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("        New Session        ");
            Console.WriteLine("---------------------------\n");
            Console.WriteLine("0 - Back to Main Menu");
            Console.WriteLine("1 - Use today's date");
            Console.WriteLine("2 - Enter date");
            Console.Write("\nEnter option number: ");
        }

        private void UpdateSessionMenu()
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("        Update Session     ");
            Console.WriteLine("---------------------------\n");
            Console.WriteLine("0 - Back to Main Menu");
            Console.WriteLine("1 - Update Starting Date/Time");
            Console.WriteLine("2 - Update Ending Date/Time");
            Console.Write("\nEnter option number: ");
        }

        private void EnterNewSession()
        {
            int input = GetUserInput(sessionMenuChoices, EnterNewSessionMenu);
            NewSessionFunctionSelect(input);

        }

        private void NewSessionFunctionSelect(int intUserInput)
        {
            switch (intUserInput)
            {
                case 0:
                    break;
                case 1:
                    BuildNewSession(DateOperations.GetTodaysDate);
                    break;
                case 2:
                    BuildNewSession(DateOperations.EnterNewDate);
                    break;
            }
        }

        private Session BuildNewSession(Func<String> GetDate)
        {
            Session newSession = new();
            string date = GetDate();
            Console.WriteLine($"Date Entered: {date}");
            string startTime = DateOperations.EnterNewTime();
            Console.WriteLine("Enter Session end time.");
            string endTime = DateOperations.EnterNewTime();

            newSession.StartTime = DateOperations.ParseDateTime(date, startTime);
            newSession.EndTime = DateOperations.ParseDateTime(date, endTime);

            db.WriteSessionToDatabase(newSession);

            return newSession;
        }
    }
}
