using System.ComponentModel;

namespace MangaDexSharp.Collections
{
    public class FakeClasses
    {
        public class ObservableDouble : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            public double _data = 1;

            public double Data
            {
                get => _data;
                set
                {
                    if (_data != value)
                    {
                        _data = value;
                        OnPropertyChanged();
                    }
                }
            }

            public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
            {
                // Raise the PropertyChanged event, passing the name of the property whose value has changed.
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public override string ToString()
            {
                return Data.ToString();
            }
        }
    }
}