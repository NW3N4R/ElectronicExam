using CommunityToolkit.Mvvm.ComponentModel;

using System;
namespace Examiner.Models
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

        public DateTimeOffset EDate
        {
            get => field;
            set => SetProperty(ref field, value);
        } = DateTimeOffset.Now.AddDays(1);

        public string getDate => EDate.Date.ToString("yyyy-MM-dd");

        public TimeSpan ETime
        {
            get => field;
            set => SetProperty(ref field, value);
        } = new TimeSpan(7, 40, 0);

        public int DurationHour
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public int DurationMin
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string Duration => $"{DurationHour.ToString().PadLeft(2, '0')}:{DurationMin.ToString().PadLeft(2, '0')}";
    }
}
