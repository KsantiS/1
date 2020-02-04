using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestSMApp.Model
{
    public class Remark : INotifyPropertyChanged
    {

        public Remark ShallowCopy()
        {
            return new Remark
            {
                Id = this.Id,
                 Comment = this.Comment,
                  Date = this.Date,
                   RemarkName = this.RemarkName.ShallowCopy()
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

        private RemarkName _RemarkName;
        public RemarkName RemarkName
        {
            get { return _RemarkName; }
            set
            {
                if (_RemarkName != value)
                    _RemarkName = value;
                OnPropertyChanged("RemarkName");
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
