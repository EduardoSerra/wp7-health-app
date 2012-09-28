using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BienStar.Model;

namespace BienStar.ViewModel
{
    public class NewMeasurementPageViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private MeasurementDataContext measurementDB;

        // Class constructor, create the data context object.
        public NewMeasurementPageViewModel(string measurementDBConnectionString)
        {
            measurementDB = new MeasurementDataContext(measurementDBConnectionString);
        }

        private List<MeasurementType> _measurementTypeList;
        public List<MeasurementType> MeasurementTypeList
        {
            get { return _measurementTypeList; }
            set
            {
                _measurementTypeList = value;
                NotifyPropertyChanged("MeasurementTypeList");
            }
        }

        // Query database and load the list to be used in NewMeasurementPage.
        public void LoadCollectionsFromDatabase()
        {
            // Load a list of all categories.
            MeasurementTypeList = measurementDB.MeasurementTypes.ToList();

        }

        // Add a measurement to the database and collections.
        public void AddMeasurement(Measurement newMeasurement)
        {
            // Add a measurement to the data context.
            measurementDB.Measurements.InsertOnSubmit(newMeasurement);

            // Save changes to the database.
            measurementDB.SubmitChanges();

            // Add a measurement to the "all" observable collection.
            App.MainPageViewModel.AllMeasurements.Add(newMeasurement);

            // Add a measurement to the appropriate filtered collection.
            switch (newMeasurement.Type.Name)
            {
                case "Weight":
                    App.MainPageViewModel.WeightMeasurements.Add(newMeasurement);
                    break;
                case "Pulse":
                    App.MainPageViewModel.PulseMeasurements.Add(newMeasurement);
                    break;
                case "Pressure":
                    App.MainPageViewModel.PressureMeasurements.Add(newMeasurement);
                    break;
                default:
                    break;
            }
            // Save changes to the database.
            measurementDB.SubmitChanges();
        }

        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            measurementDB.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}