using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;
using ElectronicExam.Administrator.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;


namespace ElectronicExam.Administrator.Updaters;

public sealed partial class UpdateExam : Page
{
    ExamsPrimary exPrimary;
    public UpdateExam(ExamsPrimary _exam)
    {
        InitializeComponent();
        exPrimary = _exam;
        DateSelector.SelectedDate = _exam.ExamDateTime.Date;
        TimeSelector.SelectedTime = _exam.ExamDateTime.TimeOfDay;
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
        bool isUpdated = await ExamPrimaryHelper.UpdateExamPrimary(exPrimary);
        if (isUpdated)
        {
            ExamsView.Current.Reload();
        }
    }
}
