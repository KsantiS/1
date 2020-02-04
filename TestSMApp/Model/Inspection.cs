using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using TestSMApp.ViewModel;

namespace TestSMApp.Model
{
    public class Inspection: INotifyPropertyChanged
    {
        public Inspection ShallowCopy()
        {
            return new Inspection
            {
                Id = this.Id,
                Comment = this.Comment,
                Date = this.Date,
                InspectionName = this.InspectionName.ShallowCopy(),
                Inspector = this.Inspector.ShallowCopy(),
                Remarks = this.Remarks.Select(x=> x.ShallowCopy()).ToObservableCollection()
            };
        }

        public int CountRemark
        {
            get { return Remarks.Count; }
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

        private InspectionName _InspectionName;
        public InspectionName InspectionName
        {
            get { return _InspectionName; }
            set
            {
                if (_InspectionName != value)
                    _InspectionName = value;
                OnPropertyChanged("InspectionName");
            }
        }

        private DateTime? _Date;
        public DateTime? Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                    _Date = value;
                OnPropertyChanged("Date");
            }
        }

        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set
            {
                if (_Comment != value)
                    _Comment = value;
                OnPropertyChanged("Comment");
            }
        }

        private ObservableCollection<Remark> _Remarks;
        public ObservableCollection<Remark> Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                    _Remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        private Inspector _Inspector;
        public Inspector Inspector
        {
            get { return _Inspector; }
            set
            {
                if (_Inspector != value)
                    _Inspector = value;
                OnPropertyChanged("Inspector");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
