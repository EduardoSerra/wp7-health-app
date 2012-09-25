using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BienStar.Model;


namespace BienStar.ViewModel
{
    public class MeasurementViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private MeasurementDataContext measurementDB;

        // Class constructor, create the data context object.
        public MeasurementViewModel(string measurementDBConnectionString)
        {
            measurementDB = new MeasurementDataContext(measurementDBConnectionString);
        }

        // All measurements.
        private ObservableCollection<Measurement> _allMeasurements;
        public ObservableCollection<Measurement> AllMeasurements
        {
            get { return _allMeasurements; }
            set
            {
                _allMeasurements = value;
                NotifyPropertyChanged("AllMeasurements");
            }
        }

        // Measurements associated with the weight type.
        private ObservableCollection<Measurement> _weightMeasurements;
        public ObservableCollection<Measurement> WeightMeasurements
        {
            get { return _weightMeasurements; }
            set
            {
                _weightMeasurements = value;
                NotifyPropertyChanged("WeightMeasurements");
            }
        }

        // Measurements associated with the pulse type.
        private ObservableCollection<Measurement> _pulseMeasurements;
        public ObservableCollection<Measurement> PulseMeasurements
        {
            get { return _pulseMeasurements; }
            set
            {
                _pulseMeasurements = value;
                NotifyPropertyChanged("PulseMeasurements");
            }
        }

        // Measurements associated with the pressure type.
        private ObservableCollection<Measurement> _pressureMeasurements;
        public ObservableCollection<Measurement> PressureMeasurements
        {
            get { return _pressureMeasurements; }
            set
            {
                _pressureMeasurements = value;
                NotifyPropertyChanged("PressureMeasurements");
            }
        }

        // A list of all measurement types, used by the add measurement page.
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

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            // Specify the query for all measurements in the database.
            var measurementsInDB = from Measurement measurement in measurementDB.Measurements
                                   select measurement;

            // Query the database and load all measurements.
            AllMeasurements = new ObservableCollection<Measurement>(measurementsInDB);

            // Specify the query for all categories in the database.
            var measurementTypesInDB = from MeasurementType types in measurementDB.MeasurementTypes
                                       select types;


            // Query the database and load all associated items to their respective collections.
            foreach (MeasurementType type in measurementTypesInDB)
            {
                switch (type.Name)
                {
                    case "Weight":
                        WeightMeasurements = new ObservableCollection<Measurement>(type.Measurements);
                        break;
                    case "Pulse":
                        PulseMeasurements = new ObservableCollection<Measurement>(type.Measurements);
                        break;
                    case "Pressure":
                        PressureMeasurements = new ObservableCollection<Measurement>(type.Measurements);
                        break;
                    default:
                        break;
                }
            }

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
            AllMeasurements.Add(newMeasurement);

            // Add a measurement to the appropriate filtered collection.
            switch (newMeasurement.Type.Name)
            {
                case "Weight":
                    WeightMeasurements.Add(newMeasurement);
                    break;
                case "Pulse":
                    PulseMeasurements.Add(newMeasurement);
                    break;
                case "Pressure":
                    PressureMeasurements.Add(newMeasurement);
                    break;
                default:
                    break;
            }
            // Save changes to the database.
           measurementDB.SubmitChanges(); 
        }

        // Remove a measurement from the database and collections.
        public void DeleteMeasurement(Measurement measurementForDelete)
        {

            // Remove the measurement from the "all" observable collection.
            AllMeasurements.Remove(measurementForDelete);

            // Remove the measurement from the data context.
            measurementDB.Measurements.DeleteOnSubmit(measurementForDelete);

            // Remove the measurement from the appropriate category.   
            switch (measurementForDelete.Type.Name)
            {
                case "Weight":
                    WeightMeasurements.Remove(measurementForDelete);
                    break;
                case "Pulse":
                    PulseMeasurements.Remove(measurementForDelete);
                    break;
                case "Pressure":
                    PressureMeasurements.Remove(measurementForDelete);
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