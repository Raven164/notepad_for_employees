using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace employeeNotebook
{
    static class  DataBase
    {
        // table lookup method
        public static void FindRow(DataTable table)
        {
            try
            {
                DataRow[] currentRows = table.Select();
                DataTable resultSearch = currentRows.CopyToDataTable();
                char paramOfFind;
                do
                {
                    string message = "select a search option (y = year of birth, n = first name, l = last name, p = phone): \n" +
                   "press 's' to sort result / press 'q' to exit find";
                    paramOfFind = GetChar(message);
                    string input;
                    switch (paramOfFind)
                    {
                        case 'y'://search by year of birth
                            Console.Write("Enter year of birth: ");
                            input = GetValidStr(Console.ReadLine());
                            currentRows = resultSearch.Select(
                            $"Year = '{input}'", null);
                            break;
                        case 'n'://search by name
                            Console.Write("Enter name: ");
                            input = GetValidStr(Console.ReadLine());
                            currentRows = resultSearch.Select(
                            $"Name = '{input}'", null);
                            break;
                        case 'l'://search by surname
                            Console.Write("Enter surname: ");
                            input = GetValidStr(Console.ReadLine());
                            currentRows = resultSearch.Select(
                            $"Surname = '{input}'", null);
                            break;
                        case 'p'://search by phone number
                            Console.Write("Enter phone: ");
                            input = GetValidStr(Console.ReadLine());
                            currentRows = resultSearch.Select(
                            $"PhoneNumber = '{input}'", null);
                            break;
                        case 's': //sorting the result
                            Sort(Program.GetCharForSort(), resultSearch);
                            break;
                        default:
                            if(paramOfFind != 'q') Console.WriteLine("you entered a nonexistent search parameter");
                            paramOfFind = 'q';
                            break;
                    }
                    if ( paramOfFind != 'q' && currentRows.Length != 0)
                        //if there is a search result and the button 'q' is not pressed
                    {
                        resultSearch = currentRows.CopyToDataTable();
                        //if  button 's' is not pressed show table in console
                        if (paramOfFind != 's') ShowTable(resultSearch);
                    }
                    else
                    { //if there is no search result, the search ends
                        Console.WriteLine(" search gave no result. ");
                        paramOfFind = 'q';
                    }
                } while (paramOfFind != 'q' || (currentRows == null));
            }
            catch { Console.WriteLine("No data to search"); }
            Console.WriteLine("Exit search");
        }


        //private static void ShowResultOfSearch(DataTable table, DataRow[] currentRows) {
        //    if (currentRows.Length > 0)
        //    {
        //        foreach (DataRow row in currentRows)
        //        {
        //            foreach (DataColumn column in table.Columns)
        //                Console.Write("{0-20}", row[column]);
        //            Console.WriteLine("\t" + row.RowState);
        //        }
        //    }
        //    else Console.WriteLine(" search gave no result. ");
        //}

        public static char GetChar(string message)
        {
            Console.WriteLine(message + " ");
            ConsoleKeyInfo result = Console.ReadKey(true);
            return result.KeyChar;
        }

       public static bool AddRow(DataTable table, string Msg )
        {
            Console.Write("Enter the user's details. Data separator '|'. " +
                $"\n Format: name|surname|year of birth|phone number|{Msg} : \n");
            string txt = Console.ReadLine();
            string[] Arr = txt.Split("|");
            if (Arr.Length != 5)
            {
                Console.WriteLine("the data entered is incorrect");
                return false;
            }
            else return AddRowMethods[table.TableName](Arr);
        }
        
        public static bool AddRowForReplace(DataTable table,string Id)
        {
            bool result = false;
            Console.Write("Enter the new user's details. Data separator '|'. " +
                "\n Format: name|surname|year of birth|phone number : \n");
            string txt = Console.ReadLine();
            string[] Arr = txt.Split("|");
            Array.Resize(ref Arr, Arr.Length + 1);
            Arr[Arr.Length - 1] = Id;
            if (Arr.Length != 5) Console.WriteLine("the data entered is incorrect");
            else
            {
                if (AddRowMethods[table.TableName](Arr)) return true;
            }
            return result;
        }

        public static void ShowTable(DataTable employeeTable)
        {
            foreach (DataRow row in employeeTable.Rows)
            {
                foreach (var cell in row.ItemArray)
                    Console.Write("{0,-20}", cell);
                Console.WriteLine();
            }
        }

        public static void Delete(DataTable mytable, string IdStr) {
           int Id = GetValidId(IdStr);
            try
            {
                if (CheckId(mytable, IdStr))
                {
                    DataRow[] currentRows = mytable.Select($"Id = {Id}");
                    DataRow b = currentRows[0];
                    DeleteRowMethods[mytable.TableName](b);
                }
                else Console.WriteLine($" No user with id equal to {IdStr}. ");
             }
            catch (FormatException)
            {
                Console.WriteLine($" No user with ID equal to '{IdStr}'. ");
            }
}

       
        public static Dictionary<string, MethodAddRow> AddRowMethods = 
            new Dictionary<string, MethodAddRow> {
                {"employee_DATA",EmployeeDB.AddRow},
                {"manager_DATA", ManagerDB.AddRow},
        };

        public static Dictionary<string, MethodDeleteRow> DeleteRowMethods =
        new Dictionary<string, MethodDeleteRow> {
                {"employee_DATA",EmployeeDB.DeleteRow},
                {"manager_DATA", ManagerDB.DeleteRow}
         };

     


        public static void Sort(char key, DataTable table) {
            DataRow[] currentRows;
            string result = "";
            switch (key)
            {
                case 's': result = "Surname"; break;
                case 'S': result = "Surname DESC"; break;
                case 'y': result = "Year"; break;
                case 'Y': result = "Year DESC"; break;
            }               
            if (result.Equals("")) Console.WriteLine("Uncknown command\n");
            else {
                currentRows = table.Select(null, result);                    
                //ShowResultOfSearch(table,currentRows);
                ShowTable(currentRows.CopyToDataTable());
            }
        }

        public static bool CheckId( DataTable Table, string IdStr)
        {
            string pattern = @"\s+|_+|-+";
            Regex regex = new Regex(pattern);
            IdStr = regex.Replace(IdStr, "");
            try
            {
                int Id = int.Parse(IdStr);
                DataRow[] currentRows = Table.Select($"Id = {Id}", null);
                return currentRows.Length == 1;
            }
            catch { return false; }
        }

        public static int GetValidId(string numStr)
        {
            string pattern = @"\s+|_+|-+";
            Regex regex = new Regex(pattern);
            numStr = regex.Replace(numStr, "");
            int Id;
            try { Id = int.Parse(numStr); }
            catch { Id = -1; };
            return Id;
        }

        public static string GetValidStr(string Str)
        {
            string pattern = @"\s+|_+|-+";
            Regex regex = new Regex(pattern);
            Str = regex.Replace(Str, "");          
            return Str;
        }

    }
}
