using System;

namespace employeeNotebook
{
    //class Manager extends abstract class Person
    class Manager : Person
    {

        protected string _departamentId = String.Empty;

        public string DepartmentId {
            get => _departamentId;
            set => SetDepartment(value);
        }

        protected Manager(string name, string surname, string yearOfBirth, string phoneNumber, string department)
        {
            Name = name;
            SurName = surname;
            YearOfBirth = yearOfBirth;
            PhoneNumber = phoneNumber;
            DepartmentId = department;
            Console.WriteLine("Object manager was create");
        }

        //method for creating a new object Manager with argument checking
        public static Manager CreateManager(string name, string surname, string yearOfBirth, string phoneNumber, string Id_Dep)
        {
            try
            {
                return new Manager(name, surname, yearOfBirth, phoneNumber, Id_Dep);
            }
            catch (Exception e)
            {            
                Console.WriteLine(e.Message);               
                return null;
            }
        }
        //method for setting the value of the department field
        //if there is a department with such id, then the value is assigned otherwise not
        protected void SetDepartment(string IdStr) {
            if (employeeNotebook.Department.CheckIdDepartment(IdStr))
            {
                _departamentId = Department.GetDepartmentName(int.Parse(IdStr));
            }
            else throw new ArgumentException("Invalid Department ID");
        }
           
    }
}
