using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace BienStar.Model
{
    [Table]
    public class Measurement : INotifyPropertyChanged, INotifyPropertyChanging
    {

        // Define ID: private field, public property, and database column.
        private int _measurementId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int MeasurementId
        {
            get { return _measurementId; }
            set
            {
                if (_measurementId != value)
                {
                    NotifyPropertyChanging("MeasurementId");
                    _measurementId = value;
                    NotifyPropertyChanged("MeasurementId");
                }
            }
        }

        // Define measurement value: private field, public property, and database column.
        private decimal _measurementValue;

        [Column]
        public decimal MeasurementValue
        {
            get { return _measurementValue; }
            set
            {
                if (_measurementValue != value)
                {
                    NotifyPropertyChanging("MeasurementValue");
                    _measurementValue = value;
                    NotifyPropertyChanged("MeasurementValue");
                }
            }
        }

        //Define formatted value to be used for printing in views.
        private string _formattedValue;

        [Column]
        public string FormattedValue
        {
            get { return _formattedValue; }
            set
            {
                if (_formattedValue != value)
                {
                    NotifyPropertyChanging("FormattedValue");
                    _formattedValue = value;
                    NotifyPropertyChanged("FormattedValue");
                }
            }

        }

        // Define creation date: private field, public property, and database column.
        private DateTime _creationDate;

        [Column]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set
            {
                if (_creationDate != value)
                {
                    NotifyPropertyChanging("CreationDate");
                    _creationDate = value;
                    NotifyPropertyChanged("CreationDate");
                }
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        protected void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        // Internal column for the associated MeasurementType ID value
        [Column]
        internal int _measurementTypeId;

        // Entity reference, to identify the MeasurementType "storage" table
        private EntityRef<MeasurementType> _type;

        // Association, to describe the relationship between this key and that "storage" table
        [Association(Storage = "_type", ThisKey = "_measurementTypeId", OtherKey = "Id", IsForeignKey = true)]
        public MeasurementType Type
        {
            get { return _type.Entity; }
            set
            {
                NotifyPropertyChanging("Type");
                _type.Entity = value;

                if (value != null)
                {
                    _measurementTypeId = value.Id;
                }

                NotifyPropertyChanging("Type");
            }
        }
    }

    [Table]
    public class PressureMeasurement : Measurement
    {
        // Define Dyastolic: private field, public property, and database column.
        private int _diastolic;

        [Column]
        public int Diastolic
        {
            get { return _diastolic; }
            set
            {
                if (_diastolic != value)
                {
                    NotifyPropertyChanging("Diastolic");
                    _diastolic = value;
                    NotifyPropertyChanged("Diastolic");
                }
            }
        }    
    }

    [Table]
    public class MeasurementType : INotifyPropertyChanged, INotifyPropertyChanging
    {

        // Define ID: private field, public property, and database column.
        private int _id;

        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                NotifyPropertyChanging("Id");
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        // Define category name: private field, public property, and database column.
        private string _name;

        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                NotifyPropertyChanging("Name");
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        // Define the entity set for the collection side of the relationship.
        private EntitySet<Measurement> _measurements;

        [Association(Storage = "_measurements", OtherKey = "_measurementTypeId", ThisKey = "Id")]
        public EntitySet<Measurement> Measurements
        {
            get { return this._measurements; }
            set { this._measurements.Assign(value); }
        }


        // Assign handlers for the add and remove operations, respectively.
        public MeasurementType()
        {
            _measurements = new EntitySet<Measurement>(
                new Action<Measurement>(this.attach_Measurement), 
                new Action<Measurement>(this.detach_Measurement)
                );
        }

        // Called during an add operation
        private void attach_Measurement(Measurement measurement)
        {
            NotifyPropertyChanging("Measurement");
            measurement.Type = this;
        }

        // Called during a remove operation
        private void detach_Measurement(Measurement measurement)
        {
            NotifyPropertyChanging("Measurement");
            measurement.Type = null;
        }
    }

    public class MeasurementDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public MeasurementDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the measurements items.
        public Table<Measurement> Measurements;

        // Specify a table for the measurement types.
        public Table<MeasurementType> MeasurementTypes;
    }
}
