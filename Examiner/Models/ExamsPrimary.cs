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

        public DateTimeOffset ExamDateTime
        {
            get => field;
            set => SetProperty(ref field, value);
        } = DateTimeOffset.Now.AddDays(1);

        public string getDate => ExamDateTime.Date.ToString("yyyy-MM-dd");

        public decimal ExamDuration
        {
            get => field;
            set => SetProperty(ref field, value);
        }
    }
}
