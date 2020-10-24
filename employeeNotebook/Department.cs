using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace employeeNotebook
{
    class Department
    {
        static public DataTable _departmentData = new DataTable();

        //method for creating department table
        public static void CreateDepartmentData()
        {
            
            _departmentData.TableName = "Department_DATA";// name table
            //creating columns
            DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            idColumn.Unique = true; // the column will have a unique value
            idColumn.AllowDBNull = false; // cannot accept null
            idColumn.AutoIncrement = true; // will auto-increment
            idColumn.AutoIncrementSeed = 1; // initial value
            idColumn.AutoIncrementStep = 1; // incrementing when adding a new line

            DataColumn department_name = new DataColumn("Departament name", Type.GetType("System.String"));
            department_name.Unique = true; 
            department_name.AllowDBNull = false;
            //assign the primary key
            _departmentData.PrimaryKey = new DataColumn[] { _departmentData.Columns["Id"] };
                //add columns
            _departmentData.Columns.Add(idColumn);
            _departmentData.Columns.Add(department_name);
            _departmentData.ReadXml("_departmentData.xml");
        }
        //method for getting department ID by its name
        public static int GetId(string department) {
            int result = 0;
            DataRow[] currentRows = _departmentData.Select($"[Departament name] = '{department}'");
            if (currentRows.Length == 1) result = (int)currentRows[0][0];
            return result;
        }
        //method for getting department name by its ID
        public static string GetDepartmentName(int id) {
            string result = String.Empty;
            DataRow[] currentRows = _departmentData.Select($"Id = {id}", null);
            if (currentRows.Length == 1) result = currentRows[0][1].ToString();
            return result;
        }

        //method for checking if a department with the given id exists
        public static bool CheckIdDepartment(string IdStr) {
            bool result = false;
            string pattern = @"\s+|_+|-+";
            Regex regex = new Regex(pattern);
            IdStr = regex.Replace(IdStr, "");
            try
            {
                int id = Int32.Parse(IdStr);
                string NameOfDepartment = GetDepartmentName(id);
                if (!NameOfDepartment.Equals(string.Empty)) result = true;
                return result;
            }
            catch { return result; }
        }

    }
}
