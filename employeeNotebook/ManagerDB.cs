using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace employeeNotebook
{
    class ManagerDB
    {
      


        static public DataTable managerTable = new DataTable();
        public static void CreateManagerData() {
            managerTable = new DataTable();
            managerTable.TableName = "manager_DATA";


            DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            idColumn.Unique = true; 
            idColumn.AllowDBNull = false; 
            idColumn.AutoIncrement = true; 
            idColumn.AutoIncrementSeed = 1; 
            idColumn.AutoIncrementStep = 1; 

            DataColumn surNameColumn = new DataColumn("Surname", Type.GetType("System.String"));
            surNameColumn.AllowDBNull = false; 
            DataColumn NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            NameColumn.AllowDBNull = false; 
            DataColumn yearColumn = new DataColumn("Year", Type.GetType("System.Int32"));
            yearColumn.AllowDBNull = false;                
            DataColumn NumberColumn = new DataColumn("PhoneNumber", Type.GetType("System.String"));
            NumberColumn.AllowDBNull = false; 
            DataColumn departmentColumn = new DataColumn("DepartmentColumn", Type.GetType("System.String"));
            departmentColumn.AllowDBNull = false; 
            managerTable.PrimaryKey = new DataColumn[] { managerTable.Columns["Id"] };

            managerTable.Columns.Add(idColumn);            
            managerTable.Columns.Add(NameColumn);
            managerTable.Columns.Add(surNameColumn);
            managerTable.Columns.Add(yearColumn);
            managerTable.Columns.Add(NumberColumn);
            managerTable.Columns.Add(departmentColumn);
            //check for the presence of the file,else , then create a new 
            try
            {
                ManagerDB.managerTable.ReadXml("managerTable.xml");
            }
            catch {
                ManagerDB.managerTable.WriteXml("managerTable.xml");
                ManagerDB.managerTable.ReadXml("managerTable.xml");
            }
           
        }

        public static bool AddRow(string[] Arr)
        {            
            Manager mng = Manager.CreateManager(Arr[0], Arr[1], Arr[2], Arr[3], Arr[4]);
            if (mng != null)
            {
                managerTable.Rows.Add(
                    new object[] { null, mng.Name, mng.SurName, mng.YearOfBirth, mng.PhoneNumber, mng.DepartmentId });
                return true;
            }
            else {
                Console.WriteLine("Error.");
                return false;              
            }
        }

        //method for deleted a row with replace
        public static bool DeleteRow(DataRow row)
        {
            string msg = "Manager is removed only with replacement\n" +
                "to replace with an a new manager press 'n'\n" +
                "to replace with an existing employee press 'e'\n" +
                "to replace with an existing manager press 'm'\n";

            string CurrentManagerId = row[0].ToString();
            string DepartmentName = row[5].ToString();
            string CurrentManagerName = $"{row[1]} {row[2]} (id: {row[0]})";
            string DepartmentID = Department.GetId(DepartmentName).ToString();
            char key = DataBase.GetChar(msg);
            

            switch (key)
            {
                case 'n'://replace with a new manager
                    if (DataBase.AddRowForReplace(managerTable, DepartmentID))//if added a new manager
                    {
                        managerTable.Rows.Remove(row);//remove the existing manager
                        //replace the value of the existing manager with the new manager in the users table
                        EmployeeDB.ChangeManeger(manager: CurrentManagerName, newManager: GetLastManager());
                        return true;
                    }
                    break;
                case 'm': // replace one existing manager with another existing manager
                    Console.WriteLine("enter ID existing manager: ");
                    string exManId = Console.ReadLine(); // getting id
                    if (CheckId(exManId)) // if value valid
                    {
                        DataRow existingManager = GetRowById(exManId);
                        if (row[5].Equals(existingManager[5])) // if the departments are the same
                        {
                            managerTable.Rows.Remove(row);//remove  manager
                            EmployeeDB.ChangeManeger(manager: CurrentManagerName, newManager: GetLastManager());
                            return true;
                        }
                        else Console.WriteLine("Manager departments must match\n");
                    }
                    else Console.WriteLine("Invalid manager ID");
                    break;
                case 'e':// replace the existing manager with an employee
                    Console.WriteLine("enter ID existing employee: ");
                    string exEmpId = Console.ReadLine(); // getting employee ID
                    if (DataBase.CheckId(EmployeeDB.employeeTable, exEmpId)) // if the ID is valid
                    {
                        DataRow rowEmployee = EmployeeDB.GetRowById(exEmpId); //geting employee row
                        // add an employee as a manager
                        AddRow(new string[] { rowEmployee[1].ToString(), rowEmployee[2].ToString(),
                            rowEmployee[3].ToString(), rowEmployee[4].ToString(),DepartmentID});
                       // delete an employee
                        EmployeeDB.DeleteRow(rowEmployee);
                        managerTable.Rows.Remove(row);
                        EmployeeDB.ChangeManeger(manager: CurrentManagerName, newManager: GetLastManager());
                        return true;
                    }
                    else Console.WriteLine("Invalid employee ID");
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
            return false;
        }
        //getting the last row of the manager to write to the employee table in the manager column
        public static string GetLastManager() {
            int lastIndexRows = managerTable.Rows.Count - 1;          
            string name = managerTable.Rows[lastIndexRows][1].ToString();
            string surname = managerTable.Rows[lastIndexRows][2].ToString();
            string id = managerTable.Rows[lastIndexRows][0].ToString();
            return $"{name} {surname} (id: {id})";
        }

        public static int GetLastId() {
            int lastIndexRows = managerTable.Rows.Count - 1;
            return (int)managerTable.Rows[lastIndexRows][0];
        }
        // checking if there is a manager with the given id
        public static bool CheckId(string IdStr)
        {
            int Id = DataBase.GetValidId(IdStr);
            if (Id != -1)
            {
                try
                {
                    DataRow[] currentRows = managerTable.Select($"Id = {Id}");
                    return currentRows.Length == 1;
                }
                catch {
                    Console.WriteLine("invalid Manager Id");
                    return false;
                }
            }
            else return false;
        }

        public static DataRow GetRowById(string Id)
        {
            Id = DataBase.GetValidStr(Id);
            DataRow[] rows = managerTable.Select("Id =" + Id);
            DataRow result = rows[0];
            return result;
        }

    }
}
