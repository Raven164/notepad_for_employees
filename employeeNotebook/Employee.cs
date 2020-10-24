using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace employeeNotebook
{
    //class Employee extends abstract class Person
    class Employee : Person
    {
        protected string _manager = String.Empty;

        protected Employee(string name, string surname, string yearOfBirth, string phoneNumber, string managerID)
        {
            Name = name;
            SurName = surname;
            YearOfBirth = yearOfBirth;
            PhoneNumber = phoneNumber;
            Manager = managerID;
        }

        //method for creating a new object Employee with argument checking
        public static Employee CreateEmployee(string name, string surname, string yearOfBirth,
            string phoneNumber, string manager) {
            try
            {
                return new Employee(name, surname, yearOfBirth, phoneNumber, manager);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string Manager {
            get => _manager;
            set => SetManager(value);
        }
        //method for setting the value of the manager field
        //if there is a manager with such id, then the value is assigned otherwise not
        public void SetManager(string managerId)
        {
            if (ManagerDB.CheckId(managerId)) {
                DataRow manager = ManagerDB.GetRowById(managerId);
                _manager = $"{manager[1]} {manager[2]} (id: {manager[0]})";
            }
            else
            {
                Console.WriteLine("No manager with this ID\n");
                throw new ArgumentException();
            }
        }

    }
}
