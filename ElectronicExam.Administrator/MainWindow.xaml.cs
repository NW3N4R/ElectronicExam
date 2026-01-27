using ElectronicExam.Administrator.Uploaders;
using ElectronicExam.Administrator.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

using System;
using System.Collections.Generic;
using System.Linq;


namespace ElectronicExam.Administrator
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
        }

        private void GlobalNavigator_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem as NavigationViewItem;
            if (selectedItem?.Tag is not string selectedTag)
                throw new InvalidOperationException("No valid selection.");

            ContentFrame.Navigate(pages[selectedTag].GetType(), null, new DrillInNavigationTransitionInfo());
        }

        private Dictionary<string, Page> pages = new Dictionary<string, Page>()
        {
            { "HomeView", new HomeView() },
            { "ExamsView", new ExamsView() },
            { "StudentsView", new StudentsView() },
            { "TeachersView", new TeachersView() },
            { "NewExam", new NewExam() },
        };

        private void GlobalNavigator_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
            var selectedType = ContentFrame.Content.GetType();
            string? key = pages.FirstOrDefault(p => p.Value.GetType() == selectedType).Key;
            if (key is not null)
            {
                var item = GlobalNavigator.MenuItems.OfType<NavigationViewItem>().First(i => (string)i.Tag == key);
                GlobalNavigator.SelectedItem = item;
            }
        }
    }
}
