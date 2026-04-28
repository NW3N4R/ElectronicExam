using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;
using ElectronicExam.Administrator.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectronicExam.Administrator.Uploaders
{
    public sealed partial class NewExam : Page
    {
        ExamsPrimary exPrimary = new();
        public NewExam()
        {
            InitializeComponent();
            this.DataContext = exPrimary;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateSelector.SelectedDate is DateTimeOffset date)
            {
                DateTimeOffset dto = new DateTimeOffset(
                    date.Date + TimeSelector.Time,
                    date.Offset
                );
                exPrimary.ExamDateTime = dto;
            }
            else
            {
                return;
            }
            bool isInserted = await ExamPrimaryHelper.InsertExamPrimary(exPrimary);
            if (isInserted)
            {
                ExamsView.Current.Reload();
            }
        }
    }
}
