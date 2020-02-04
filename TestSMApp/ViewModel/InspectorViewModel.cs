using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TestSMApp;
using System.Windows.Input;
using System.Windows;
using TestSMApp.Model;

namespace TestSMApp.ViewModel
{
    public class InspectorViewModel : INotifyPropertyChanged
    {
        private InspectorWindow _InspectorWindow;
        public InspectorWindow InspectorWindow
        {
            get { return _InspectorWindow; }
            set
            {
                if (_InspectorWindow != value)
                    _InspectorWindow = value;
                OnPropertyChanged("iw");
            }
        }
        public ObservableCollection<Inspector> _Inspectors = new ObservableCollection<Inspector>();

        public ObservableCollection<Inspector> Inspectors
        {
            get { return _Inspectors; }
            set
            {
                if (_Inspectors != value)
                    _Inspectors = value;
                OnPropertyChanged("inspectors");
            }
        }

        private Inspector _Inspector;
        public Inspector SelectInspector
        { 
            get { return _Inspector; }
            set
            {
                if (_Inspector != value)
                    _Inspector = value;
                    FIO = value.FIO;
                OnPropertyChanged("SelectInspector");
            }
        }

        private string _FIO;
        public string FIO
        {
            get { return _FIO; }
            set
            {
                if (_FIO != value)
                    _FIO = value;
                OnPropertyChanged("FIO");
            }
        }

        public InspectorViewModel()
        {
            _updateViewInspectorCommand = new RelayCommand(UpdateViewInspector);
        }

        private ICommand _updateViewInspectorCommand;
        public virtual ICommand UpdateViewInspectorCommand { get { return _updateViewInspectorCommand; } }

        public void UpdateViewInspector(object e)
        {
            if (SelectInspector != null)
            {
                using (var con = new UserContext())
                {
                    var i = con.Inspectors.Find(SelectInspector.Id);
                    i.FIO = FIO;
                    con.SaveChanges();
                }
            }
            else
            {
                using (var con = new UserContext())
                {
                    var i = con.Inspectors.Add(new Inspector { FIO = FIO });
                    con.SaveChanges();
                }
            }
            InspectorWindow.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
