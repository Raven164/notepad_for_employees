using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace employeeNotebook
{
    abstract class Person
    {
        protected string _name;
        protected string _surname;
        protected int _yearOfBirth;
        protected string _phoneNumber;

       public string Name
        {
            get => this._name;
            set => SetName(value);
        }

         public string SurName
        {
            get => this._surname;
            set => SetSurName(value);
        }

         public string YearOfBirth
        {
            get => this._yearOfBirth.ToString();
            set => SetYearOfBirth(value);
        }

       public string PhoneNumber
        {
            get => this._phoneNumber;
            set => SetPhoneNumber(value);
        }
        //method for assigning a value  with a check for validity
        protected void SetName(string Name)
        {
            string errorMessage = "Invalid Name. The name must consist of one to 30 Latin letters";
            Name = Name.Trim().ToUpper().Replace(" ", "_");
            if (!Regex.IsMatch(Name, @"[^A-Z_]"))
            {
                Regex regex = new Regex(@"_+");
                Name = regex.Replace(Name, "_").Trim('_');
                if (Name.Equals(String.Empty) || Name.Length > 30) throw new ArgumentException(errorMessage);         
                else this._name = Name;
            }
            else throw new ArgumentException(errorMessage);
        }
        // //method for assigning a value with a check for validity
        protected void SetSurName(string SurName)
        {
            string errorMessage = "Invalid SurName. The surname must consist of one to 30 Latin letters";
            SurName = SurName.Trim().ToUpper().Replace(" ", "_");
            if (!Regex.IsMatch(SurName, @"[^A-Z_]"))
            {
                Regex regex = new Regex(@"_+");
                SurName = regex.Replace(SurName, "_").Trim('_');
                if (SurName.Equals(String.Empty) || SurName.Length > 30)
                {
                    throw new ArgumentException(errorMessage);
                }
                else this._surname = SurName;
            }
            else
            {
                throw new ArgumentException(errorMessage);
            }
        }

        protected void SetYearOfBirth(string YearOfBirth)
        {
            int yearNow = DateTime.Now.Year;
            int YearOfBirthrToInt;
            try
            {
                YearOfBirth = YearOfBirth.Trim().Trim('_');
                YearOfBirthrToInt = Int32.Parse(YearOfBirth);
                if (YearOfBirthrToInt > (yearNow - 80) && YearOfBirthrToInt < (yearNow - 18))
                    this._yearOfBirth = YearOfBirthrToInt;
                else
                {                   
                    throw new ArgumentException("Invalid year of birth. The employee must be no more than" +
                        " 80 years old and at least 18 years old");
                }
            }
            catch (FormatException)
            {        
                throw new FormatException("Invalid year of birth.Enter this correctly");
            }
        }

        protected void SetPhoneNumber(string phone)
        {
            string errorMessage = "the phone number must consist of six digits";
            string pattern = @"\s+|_+|-+";
            Regex regex = new Regex(pattern);
            phone = regex.Replace(phone, "");
            if (phone.Length == 6)
            {
                if (Regex.IsMatch(phone, @"\d{6}")) _phoneNumber = phone;
                else throw new ArgumentException(errorMessage);
            }
            else throw new ArgumentException(errorMessage);
        }




    }
}
