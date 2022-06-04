using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    public class UIManager
    {
        public bool session;
        public DatabaseManager db;
        public UIManager(DatabaseManager db)
        {
            this.db = db;
        }

        private string[] menuChoices = { "0", "1", "2", "3", "4", "5" };
        private string[] sessionMenuChoices = { "0", "1", "2"};

        private void MainMenu()
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("         MAIN MENU         ");
            Console.WriteLine("---------------------------\n");

            Console.WriteLine("0 - Exit Application");
            if (!session)
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
                    Console.WriteLine("Goodbye!");
                    break;
                case 1:
                    db.ToggleActiveSession();
                    Console.WriteLine("Toggling session");
                    break;
                case 2:
                    EnterNewSession();
                    break;
                case 3:
                    Console.WriteLine("Update session...");
                    break;
                case 4:
                    Console.WriteLine("Delete session...");
                    break;
                case 5:
                    Console.WriteLine("View Reports...");
                    break;
            }
        }

        public void MenuLoop()
        {
            int UserInput = 1;
            while (UserInput != 0)
            {
                session = db.CheckForActiveSession();
                UserInput = GetUserInput(menuChoices, MainMenu);
                MainMenuFunctionSelect(UserInput);
            }

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

        private void EnterNewSession()
        {
            int input = GetUserInput(sessionMenuChoices, EnterNewSessionMenu);

        }
    }
}
