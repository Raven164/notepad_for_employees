using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace employeeNotebook
{    public  delegate bool MethodAddRow(string[] Arr);
     public delegate bool MethodDeleteRow(DataRow row);
    class Program
    {
        internal delegate void SignalHandler(ConsoleSignal consoleSignal);
        internal enum ConsoleSignal
        {
            CtrlC = 0,
            CtrlBreak = 1,
            Close = 2,
            LogOff = 5,
            Shutdown = 6
        }
        
        internal static class ConsoleHelper
        {
            [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler")]
            public static extern bool SetSignalHandler(SignalHandler handler, bool add);
        }
        //method for writing to a file if the program terminated incorrectly
        private static void HandleConsoleSignal(ConsoleSignal consoleSignal)
        {
            EmployeeDB.employeeTable.WriteXml("employee.xml");
            ManagerDB.managerTable.WriteXml("managerTable.xml");
        }

        private static SignalHandler signalHandler;
        public static DataTable tempTable = EmployeeDB.employeeTable;

        static void Main(string[] args)
        {
          
            signalHandler += HandleConsoleSignal;
            ConsoleHelper.SetSignalHandler(signalHandler, true);
            //create tables           
            Department.CreateDepartmentData();
            ManagerDB.CreateManagerData();
            EmployeeDB.CreateEmployeeData();
            Console.WriteLine("Programm begin. For help press Alt + 'h'. Enter command : ");

            ConsoleKeyInfo res;
            char r;
            Console.WriteLine();
            //we accept commands until we press alt + q
            do
            {
                res = Console.ReadKey(true);
                r = res.KeyChar;
                switch (res.Modifiers) {
                    case ConsoleModifiers.Alt:
                        AltSelection(res);
                        break;
                    case ConsoleModifiers.Shift:
                        ShiftSelection(res);
                        break;
                    default: Console.WriteLine("Unknown command");
                        break;
                }
            }
            while ((r != 'q') || (res.Modifiers != ConsoleModifiers.Alt));

            EmployeeDB.employeeTable.WriteXml("employee.xml");
            ManagerDB.managerTable.WriteXml("managerTable.xml");
            Console.Clear();
            Console.WriteLine("program terminated");           
        }

        //if the button Alt is pressed
        public static void AltSelection(ConsoleKeyInfo input) {
            if (input.Modifiers == ConsoleModifiers.Alt) {
                char key = input.KeyChar;
                switch (key) {
                    case 'e':
                        DataBase.ShowTable(EmployeeDB.employeeTable);
                        tempTable = EmployeeDB.employeeTable;
                        Console.WriteLine();
                        break;
                    case 'd':
                        DataBase.ShowTable(Department._departmentData);
                        Console.WriteLine();
                        break;
                    case 'm':
                        DataBase.ShowTable(ManagerDB.managerTable);
                        tempTable = ManagerDB.managerTable;
                        Console.WriteLine();
                        break;
                    case 'h':
                        ShowHelp();
                        Console.WriteLine();
                        break;
                    case 'q': 
                        Console.WriteLine("Terminating the program");
                        break;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }
        ////if the button Shift is pressed
        public static void ShiftSelection(ConsoleKeyInfo input)
        {
            if (input.Modifiers == ConsoleModifiers.Shift)
            {
                char key = input.KeyChar;
                switch (key)
                {
                    case 'A':
                        Console.WriteLine("to add a Manager, click - 'm' / to add an employee, click - 'e'\n");
                        ConsoleKeyInfo keyA = Console.ReadKey(true);
                        if (keyA.KeyChar == 'e') DataBase.AddRow(EmployeeDB.employeeTable, "Manager Id");
                        else if (keyA.KeyChar == 'm') DataBase.AddRow(ManagerDB.managerTable, "Department Id");
                        else Console.WriteLine("Uncknown command\n");
                        break;
                    case 'F':
                        Console.WriteLine("To search in the Manager table, click - 'm' /" +
                      " to search in the employee table, click - 'e'\n");
                        ConsoleKeyInfo keyB = Console.ReadKey(true);
                        if (keyB.KeyChar == 'e') DataBase.FindRow(EmployeeDB.employeeTable);
                        else if (keyB.KeyChar == 'm') DataBase.FindRow(ManagerDB.managerTable);
                        else Console.WriteLine("Uncknown command\n");
                        break;
                    case 'D':
                        Console.WriteLine("to delete a Manager, click - 'm' / to delete an employee, click - 'e'\n");
                        ConsoleKeyInfo keyD = Console.ReadKey(true);
                        if (keyD.KeyChar == 'e')
                        {
                            Console.Write("Enter ID employee for delete: ");
                            string id = Console.ReadLine();
                            DataBase.Delete(EmployeeDB.employeeTable, id);
                        }
                        else if (keyD.KeyChar == 'm')
                        {
                            Console.Write("Enter ID manager for delete: ");
                            string id = Console.ReadLine();
                            DataBase.Delete(ManagerDB.managerTable, id);
                        }
                        else Console.WriteLine("Uncknown command\n");
                        break;
                    case 'S':
                 
                        DataBase.Sort(GetCharForSort(), tempTable);
                        Console.WriteLine();
                        break;
                    case 'C': Console.Clear(); break;
                    default: Console.WriteLine("Uncknown command\n"); break;
                }
            }
        }

        public static char GetCharForSort()
        {
            Console.WriteLine("to sort by year of birth, press - 'y'\n" +
                              "to sort by surname press - 's'\n" +
                              "to sort in descending order enter a value in uppercase\n");
            ConsoleKeyInfo key = Console.ReadKey(true);
            return key.KeyChar;
        }

        public static void ShowHelp() {
            try
            {
                FileStream stream = new FileStream("help.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamReader str = new StreamReader(stream);
                string data = str.ReadToEnd();
                string[] dataArray = data.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < dataArray.Length; i++) Console.WriteLine(dataArray[i]);   
                str.Close();
            }
            catch {
                Console.WriteLine("error is can not display help.\n");
            }
        }

    }
}
