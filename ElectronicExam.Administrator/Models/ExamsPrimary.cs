using CommunityToolkit.Mvvm.ComponentModel;

using System;

namespace ElectronicExam.Administrator.Models
{
    public class ExamsPrimary : ObservableObject
    {
        public int id { get; set; }

        public string TeacherName
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        }

        public string SubjectName
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        }

        public string Title
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        }


        public DateTimeOffset? EDate
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public string getDate => EDate.Value.Date.ToString("yyyy-MM-dd");
        public TimeSpan ETime
        {
            get => field;
            set => SetProperty(ref field, value);
        }
    }
}
