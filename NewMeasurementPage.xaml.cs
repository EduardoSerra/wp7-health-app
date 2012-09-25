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

namespace BienStar
{
    public partial class NewMeasurementPage : PhoneApplicationPage
    {
        public NewMeasurementPage()
        {
            InitializeComponent();
            // Set the page DataContext property to the ViewModel.
            this.DataContext = App.ViewModel;
        }

        private void appBarOkButton_Click(object sender, EventArgs e)
        {
            // Confirm there is some text in the text box.
            if (newMeasurementValueTextBox.Text.Length > 0)
            {
                // Create a new to-do item.
                Measurement newMeasurement = new Measurement
                {
                    MeasurementValue = decimal.Parse(newMeasurementValueTextBox.Text),
                    FormattedValue = newMeasurementValueTextBox.Text,
                    Type = (MeasurementType)measurementTypeListPicker.SelectedItem,
                    CreationDate = DateTime.Now
                };

                // Add the item to the ViewModel.
                App.ViewModel.AddMeasurement(newMeasurement);

                // Return to the main page.
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
        }

        private void appBarCancelButton_Click(object sender, EventArgs e)
        {
            // Return to the main page.
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}