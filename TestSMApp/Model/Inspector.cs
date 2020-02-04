using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestSMApp.Model
{
    public class Inspector : INotifyPropertyChanged
    {
        public Inspector ShallowCopy()
        {
            return new Inspector
            {
                Id = this.Id,
                FIO = this.FIO
            };
        }

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                    _Id = value;
                OnPropertyChanged("Id");
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

        private string _FullInfo;
        public string FullInfo
        {
            get { return _FIO + " ( " + _Id + " ) "; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
