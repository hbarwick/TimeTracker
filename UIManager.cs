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

        internal string[] menuChoices = { "0", "1", "2", "3", "4", "5" };
        internal void MainMenu()
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

        internal int GetUserInput(string[] choices)
        {
            string? userInput = string.Empty;
            while (!choices.Contains(userInput))
            {
                MainMenu();
                userInput = Console.ReadLine();
            }
            int intUserInput = int.Parse(userInput);
            return intUserInput;
        }

        internal void FunctionSelect(int intUserInput)
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
                    Console.WriteLine("Enter new session...");
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
                UserInput = GetUserInput(menuChoices);
                FunctionSelect(UserInput);
            }

        }
    }
}
