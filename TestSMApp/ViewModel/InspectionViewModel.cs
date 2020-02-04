using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestSMApp.Model;

namespace TestSMApp.ViewModel
{
    public class InspectionViewModel : INotifyPropertyChanged
    {
        private Inspection _Inspection;
        public Inspection Inspection
        {
            get { return _Inspection; }
            set
            {
                if (_Inspection != value)
                    _Inspection = value;
                OnPropertyChanged("Inspection");
            }
        }

        private ObservableCollection<RemarkName> _RemarkNames;
        public ObservableCollection<RemarkName> RemarkNames
        {
            get { return _RemarkNames; }
            set
            {
                if (_RemarkNames != value)
                    _RemarkNames = value;
                OnPropertyChanged("RemarkNames");
            }
        }

        private ObservableCollection<InspectionName> _InspectionNames;
        public ObservableCollection<InspectionName> InspectionNames
        {
            get { return _InspectionNames; }
            set
            {
                if (_InspectionNames != value)
                    _InspectionNames = value;
                OnPropertyChanged("InspectionNames");
            }
        }

        private ObservableCollection<Inspector> _Inspectors;
        public ObservableCollection<Inspector> Inspectors
        {
            get { return _Inspectors; }
            set
            {
                if (_Inspectors != value)
                    _Inspectors = value;
                OnPropertyChanged("Inspectors");
            }
        }

        private Remark _SelectedRemark;
        public Remark SelectedRemark
        {
            get { return _SelectedRemark; }
            set
            {
                if (_SelectedRemark != value)
                    _SelectedRemark = value;
                OnPropertyChanged("SelectedRemark");
            }
        }

        private InspectionWindow _InspectionWindow;
        public InspectionWindow InspectionWindow
        {
            get { return _InspectionWindow; }
            set
            {
                if (_InspectionWindow != value)
                    _InspectionWindow = value;
                OnPropertyChanged("InspectionWindow");
            }
        }

        public InspectionViewModel()
        {
            _UpdateViewInspectionCommand = new RelayCommand(UpdateViewInspection);
            _AddNewRemarkCommand = new RelayCommand(AddNewRemark);
            _DropNewRemarkCommand = new RelayCommand(DropNewRemark);
        }

        private ICommand _AddNewRemarkCommand;
        public virtual ICommand AddNewRemarkCommand { get { return _AddNewRemarkCommand; } }

        private ICommand _DropNewRemarkCommand;
        public virtual ICommand DropNewRemarkCommand { get { return _DropNewRemarkCommand; } }

        private ICommand _UpdateViewInspectionCommand;
        public virtual ICommand UpdateViewInspectionCommand { get { return _UpdateViewInspectionCommand; } }

        public void AddNewRemark(object e)
        {
            if(SelectedRemark?.RemarkName==null)
            {
                MessageBox.Show("Не выбрано замечание");
                return;
            }
            if(SelectedRemark?.Id==0)
            { 
            Inspection.Remarks.Add(SelectedRemark);
            SelectedRemark = new Remark();
            }
            else SelectedRemark = new Remark();
        }

        public void DropNewRemark(object e)
        {
            if(SelectedRemark!=null)
            {
                Inspection.Remarks.Remove(SelectedRemark);
            }
            else SelectedRemark = new Remark();
        }

        public void UpdateViewInspection(object e)
        {
            if (Inspection.Id == 0)
            {
                if (Inspection.InspectionName.Id == 0 && Inspection.Inspector.Id == 0)
                    return;
                using (var con = new UserContext())
                {
                    List<int> element = new List<int>();
                    foreach (var ob in Inspection.Remarks)
                    {
                        var elementId = con.Remarks.Add(new Remark
                        {
                            Comment = ob.Comment,
                            Date = ob.Date,
                            RemarkName = con.RemarkNames.Find(ob.RemarkName.Id)
                        });
                        con.SaveChanges();
                        element.Add(elementId.Id);
                    }

                    var i = con.Inspections.Add(new Inspection
                    {
                        Date = Inspection.Date,
                        InspectionName = con.InspectionNames.First(x => x.Id == Inspection.InspectionName.Id),
                        Inspector = con.Inspectors.First(x => x.Id == Inspection.Inspector.Id),
                        Comment = Inspection.Comment,
                        Remarks = Inspection.Remarks.ToList().FindAll(xx=> element.Contains(xx.Id)).ToObservableCollection()
                    });
                    con.SaveChanges();
                }
            }
            if(Inspection.Id!=0)
            {
                List<int> element = new List<int>();
                foreach (var ob in Inspection.Remarks)
                {

                    if(ob.Id!=0)
                    {
                        using (var con = new UserContext())
                        {
                            var i = con.Remarks.Find(ob.Id);
                            i.RemarkName = con.RemarkNames.Find(ob.RemarkName.Id);
                            i.Date = ob.Date;
                            i.Comment = ob.Comment;                             
                            con.SaveChanges();
                            element.Add(i.Id);
                        }
                    }
                    else
                    {
                        using (var con = new UserContext())
                        {
                            var elementId = con.Remarks.Add(new Remark
                            {
                            Comment = ob.Comment,
                            Date = ob.Date,
                            RemarkName = con.RemarkNames.Find(ob.RemarkName.Id)
                            });
                            con.SaveChanges();
                            element.Add(elementId.Id);
                        }
                    }
                }

                using (var con = new UserContext())
                    {
                        var i = con.Inspections.Find(Inspection.Id);
                        i.InspectionName = con.InspectionNames.Find(Inspection.InspectionName.Id);
                        i.Inspector = con.Inspectors.Find(Inspection.Inspector.Id);
                        i.Remarks = con.Remarks.ToList().FindAll(xx => element.Contains(xx.Id)).ToObservableCollection();
                        i.Date = Inspection.Date;
                        con.SaveChanges();
                    }
               
            }
            InspectionWindow.Close();
        }
        public void Init()
        {
            using (var con = new UserContext())
            {
                var i = con.InspectionNames.Select(x => new
                {
                    Id = x.Id,
                    Name =x.Name
                }).AsEnumerable().Select(y => new InspectionName
                {
                    Id = y.Id,
                    Name = y.Name
                }).ToList();
                InspectionNames = i.ToObservableCollection();
            }

            using (var con = new UserContext())
            {
                var i = con.RemarkNames.Select(x => new 
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsEnumerable().Select(y => new RemarkName
                {
                    Id = y.Id,
                    Name = y.Name
                }).ToList();
                RemarkNames = i.ToObservableCollection();
            }
            if(Inspection==null)
            {
                Inspection = new Inspection()
                {
                    InspectionName = InspectionNames.FirstOrDefault(),
                    Remarks = new ObservableCollection<Remark>(),
                    Inspector = Inspectors.FirstOrDefault()
                };

            }
            SelectedRemark = new Remark();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
