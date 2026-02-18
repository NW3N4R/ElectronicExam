using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.Generic;

namespace ElectronicExam.Administrator.Models
{
    public class Students : ObservableObject
    {
        public int id { get; set; }

        public string FirstName
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        }
        public string MiddleName
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        }
        public string LastName
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        }
        public string fullName => string.Concat(FirstName, " ", MiddleName, " ", LastName);
        public string Phone
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        }
        public string? Email
        {
            get => field;
            set => SetProperty(ref field, value);
        } = "";
        public string? Code
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public string Group
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        } = "A";
        public byte Stage
        {
            get => field;
            set => SetProperty(ref field, value);
        } = 1;
        public string Gender
        {
            get => field;
            set => SetProperty(ref field, value);
        } = "Male";

        public List<string> LetterGroups = new List<string>() { "A", "B", "C" };
        public List<string> DepGroups = new List<string>() { "Programming", "Web Design", "Networking" };
    }
}
