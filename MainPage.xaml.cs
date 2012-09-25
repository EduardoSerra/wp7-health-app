using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BienStar.Model;
using BienStar.ViewModel;

namespace BienStar
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Set the page DataContext property to the ViewModel.
            this.DataContext = App.ViewModel;
        }

        private void newMeasurementAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewMeasurementPage.xaml", UriKind.Relative));
        }

        private void graphViewAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewMeasurementPage.xaml", UriKind.Relative));
        }


        private void deleteMeasurementButton_Click(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                // Get a handle for the measurement bound to the button.
                Measurement measurementForDelete = button.DataContext as Measurement;

                App.ViewModel.DeleteMeasurement(measurementForDelete);
            }

            // Put the focus back to the main page.
            this.Focus();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Save changes to the database.
            App.ViewModel.SaveChangesToDB();
        }
    }
}