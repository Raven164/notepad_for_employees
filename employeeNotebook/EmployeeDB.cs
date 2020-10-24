using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace employeeNotebook
{
    class EmployeeDB
    {
        public static DataTable employeeTable = new DataTable();


        //method for creating Employee table
        public static void CreateEmployeeData() {
            employeeTable.TableName = "employee_DATA";
            DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            //creating columns
            idColumn.Unique = true; // the column will have a unique value
            idColumn.AllowDBNull = false; // cannot accept null
            idColumn.AutoIncrement = true; // will auto-increment
            idColumn.AutoIncrementSeed = 1; // initial value
            idColumn.AutoIncrementStep = 1; // incrementing when adding a new line

            DataColumn surNameColumn = new DataColumn("Surname", Type.GetType("System.String"));
            surNameColumn.AllowDBNull = false; 
            DataColumn NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            NameColumn.AllowDBNull = false; 
            DataColumn yearColumn = new DataColumn("Year", Type.GetType("System.Int32"));
            yearColumn.AllowDBNull = false;            
            DataColumn NumberColumn = new DataColumn("PhoneNumber", Type.GetType("System.String"));
            NumberColumn.AllowDBNull = false; 
            DataColumn MenagerIdColumn = new DataColumn("MenagerId", Type.GetType("System.String"));
            NumberColumn.AllowDBNull = false;

            //add columns
            employeeTable.Columns.Add(idColumn);
            employeeTable.Columns.Add(NameColumn);
            employeeTable.Columns.Add(surNameColumn);
            employeeTable.Columns.Add(yearColumn);
            employeeTable.Columns.Add(NumberColumn);
            employeeTable.Columns.Add(MenagerIdColumn);
            employeeTable.PrimaryKey = new DataColumn[] { employeeTable.Columns["Id"] };//assign the primary key
            //check for the presence of the file,else , then create a new 
            try
            {
                employeeTable.ReadXml("employee.xml");
            }
            catch {
                employeeTable.WriteXml("employee.xml");
                employeeTable.ReadXml("employee.xml");
            }           
        }

        //method for adding a row
        public static bool AddRow(string[] Arr) 
        {
            Employee em = Employee.CreateEmployee(Arr[0], Arr[1], Arr[2], Arr[3], Arr[4]);
            if (em != null)
            {
                employeeTable.Rows.Add(new object[]
                    { null, em.Name, em.SurName, em.YearOfBirth, em.PhoneNumber, em.Manager });
                Console.WriteLine("Employee added\n");
                return true;
            }
            else {
                Console.WriteLine("The employee has not been added\n");
                return false;
            }     
        }

        //method for deleted a row
        public static bool DeleteRow(DataRow row) {
            try
            {
                employeeTable.Rows.Remove(row);
                Console.WriteLine("Employee deleted");
                return true;
            }
            catch {
                Console.WriteLine("The employee has not been deleted");
                return false;
            }
        }

        //method for replacing an existing manager with a new one in the manager column
        public static void ChangeManeger(string manager, string newManager) {
          int CountRows =  employeeTable.Rows.Count;
            for (int i = 0; i < CountRows; i++) {
                if (employeeTable.Rows[i]["MenagerId"].Equals(manager))
                    employeeTable.Rows[i]["MenagerId"] = newManager;
            }
        }

        public static DataRow GetRowById(string Id) {
            DataRow[] rows = employeeTable.Select("Id =" + Id);
            DataRow result = rows[0];
            return result;
        }

    }
}
