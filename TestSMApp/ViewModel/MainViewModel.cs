using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSMApp.Model;
using TestSMApp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TestSMApp;
using System.Windows.Input;
using System.Windows;

namespace TestSMApp.ViewModel
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col)
        {
            return new ObservableCollection<T>(col);
        }
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        //public DelegateCommand AddActionCommand { get; set; }
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

        private ObservableCollection<Inspector> _InfoInspectors = new ObservableCollection<Inspector>();
        public ObservableCollection<Inspector> InfoInspectors
        {
            get { return _InfoInspectors; }
            set
            {
                if (_InfoInspectors != value)
                    _InfoInspectors = value;
                OnPropertyChanged("InfoInspectors");
            }
        }

        private Inspector _FilterInspector;
        public Inspector FilterInspector
        {
            get { return _FilterInspector; }
            set
            {
                if (_FilterInspector != value)
                    _FilterInspector = value;
                Filter();
                OnPropertyChanged("FilterInspector");
            }
        }

        private string _NameInfo;
        public string NameInfo
        {
            get { return _NameInfo; }
            set
            {
                if (_NameInfo != value)
                    _NameInfo = value;
                Filter();
                OnPropertyChanged("NameInfo");
            }
        }

        private Inspection _SelectedInspection;
        public Inspection SelectedInspection
        {
            get { return _SelectedInspection; }
            set
            {
                if (_SelectedInspection != value)
                     _SelectedInspection = value;
                OnPropertyChanged("SelectedInspection");
            }
        }

        public ObservableCollection<Inspection> _Inspections = new ObservableCollection<Inspection>();
        public ObservableCollection<Inspection> Inspections
        {
            get { return _Inspections; }
            set
            {
                if (_Inspections != value)
                    _Inspections = value;
                OnPropertyChanged("Inspections");
            }
        }

        public ObservableCollection<Inspection> _FilterInspections = new ObservableCollection<Inspection>();
        public ObservableCollection<Inspection> FilterInspections
        {
            get { return _FilterInspections; }
            set
            {
                if (_FilterInspections != value)
                    _FilterInspections = value;
                OnPropertyChanged("FilterInspections");
            }
        }

        public string Cont
        {
            get { return " Инспекции"; }
        }

        public MainViewModel()
        {
            _ShowViewInspectorCommand = new RelayCommand(ShowViewInspector);
            _AddInspectionCommand = new RelayCommand(AddInspection);
            _DropInspectionCommand = new RelayCommand(DropInspection);
            _EditInspectionCommand = new RelayCommand(EditInspection);
        }

        private ICommand _ShowViewInspectorCommand;
        public virtual ICommand ShowViewInspectorCommand { get { return _ShowViewInspectorCommand; } }
        private ICommand _AddInspectionCommand;
        public virtual ICommand AddInspectionCommand { get { return _AddInspectionCommand; } }
        private ICommand _DropInspectionCommand;
        public virtual ICommand DropInspectionCommand { get { return _DropInspectionCommand; } }
        private ICommand _EditInspectionCommand;
        public virtual ICommand EditInspectionCommand { get { return _EditInspectionCommand; } }

        public void Filter()
        {
            
            if (NameInfo != null && FilterInspector != null)
            {
                if(FilterInspector.Id!=-1)
                {
                    FilterInspections.Clear();
                    foreach (var element in Inspections)
                    {
                        if (element.Inspector.Id == FilterInspector.Id && element.InspectionName.Name.Contains(NameInfo))
                        {
                            FilterInspections.Add(element.ShallowCopy());
                        }
                    }
                    return;
                }
            }
            if (NameInfo!=null)
            {
                FilterInspections.Clear();
                foreach (var element in Inspections)
                {
                    if (element.InspectionName.Name.Contains(NameInfo))
                    {
                        FilterInspections.Add(element.ShallowCopy());
                    }
                }
                return;
            }
            if(FilterInspector != null)
            {
                if (FilterInspector.Id != -1)
                {
                    FilterInspections.Clear();
                    foreach (var element in Inspections)
                    {
                        if (element.Inspector.Id == FilterInspector.Id)
                        {
                            FilterInspections.Add(element.ShallowCopy());
                        }
                    }
                }
                else
                {
                    foreach (var element in Inspections)
                    {
                            FilterInspections.Add(element.ShallowCopy());
                    }
                }

            }
        }

        public void AddInspection(object e)
        {
            InspectionWindow inspectionWindow = new InspectionWindow();
            InspectionViewModel inspectionViewModel = new InspectionViewModel();
            inspectionViewModel.Inspection = null;
            inspectionViewModel.Inspectors = Inspectors;
            inspectionViewModel.InspectionWindow = inspectionWindow;
            inspectionViewModel.Init();
            inspectionWindow.DataContext = inspectionViewModel;
            if (inspectionWindow.ShowDialog() != true)
            {
                Init();
            }
        }
        public void DropInspection(object e)
        {
            if (SelectedInspection == null)
            {
                MessageBox.Show("Не выбрана инспекция");
            }
            else
            {
                using (var con = new UserContext())
                {
                    try
                    {
                        foreach(var i in SelectedInspection.Remarks)
                        {
                            con.Remarks.Remove(con.Remarks.Find(i.Id));
                            con.SaveChanges();
                        }
                        con.Inspections.Remove(con.Inspections.Find(SelectedInspection.Id));
                        con.SaveChanges();

                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString());
                    }
                    FilterInspections.Remove(SelectedInspection);
                    Inspections.Remove(SelectedInspection);
                }
            }
        }
        
        public void EditInspection(object e)
        {
            if(SelectedInspection==null)
            {
                MessageBox.Show("Не выбрана инспекция");
            }
            else
            {
                InspectionWindow inspectionWindow = new InspectionWindow();
                InspectionViewModel inspectionViewModel = new InspectionViewModel();
                inspectionViewModel.Inspection = SelectedInspection;
                inspectionViewModel.Inspectors = Inspectors;
                inspectionViewModel.InspectionWindow = inspectionWindow;
                inspectionViewModel.Init();
                inspectionWindow.DataContext = inspectionViewModel;
                if(inspectionWindow.ShowDialog()!=true)
                {
                    Init();
                }
            }
        }

        public void ShowViewInspector(object e)
        {
                InspectorWindow inspectorWindow = new InspectorWindow();
                InspectorViewModel inspectorViewModel = new InspectorViewModel();
                inspectorViewModel.Inspectors = Inspectors;
                inspectorViewModel.InspectorWindow = inspectorWindow;
                inspectorWindow.DataContext = inspectorViewModel;
                if (inspectorWindow.ShowDialog() != true)
                {
                    Init();
                }
        }

        public void Init()
        {

            using (var con = new UserContext())
            {
                var i = con.Inspections.Include("InspectionName")
                                           .Include("Remarks").Select(x => new
                                           {
                                               Id = x.Id,
                                               Date = x.Date,
                                               Comment = x.Comment,
                                               Rm = x.Remarks.Select(o => new
                                               {
                                                   Id = o.Id,
                                                   Date = o.Date,
                                                   Comment = o.Comment,
                                                   RemarkName = o.RemarkName != null ? new { Id = o.RemarkName.Id, Name = o.RemarkName.Name } : null
                                               }),
                                               InsName = x.InspectionName != null ? new { Id = x.InspectionName.Id, Name = x.InspectionName.Name } : null,
                                               Inspect = x.Inspector != null ? new { Id = x.Inspector.Id, FIO = x.Inspector.FIO } : null
                                           })
               .AsEnumerable().Select(y => new Inspection
               {
                   Id = y.Id,
                   Date = y.Date,
                   Comment = y.Comment,
                   InspectionName = new InspectionName
                   {
                       Id = y.InsName.Id,
                       Name = y.InsName.Name
                   },
                   Inspector = new Inspector
                   {
                       Id = y.Inspect.Id,
                       FIO = y.Inspect.FIO
                   },
                   Remarks = y.Rm.Select(o => new Remark
                   {
                       Id = o.Id,
                       Date = o.Date,
                       Comment = o.Comment,
                       RemarkName = o.RemarkName != null ? new RemarkName { Id = o.RemarkName.Id, Name = o.RemarkName.Name } : null
                   }).ToList().ToObservableCollection()
               }).ToList();
                Inspections = i.ToObservableCollection();
                foreach(var copy in Inspections)
                {
                    FilterInspections.Add(copy.ShallowCopy());
                }
            }

            using (var con = new UserContext())
            {
                var i = con.Inspectors.Select(x=> new
                {
                     Id = x.Id,
                     FIO = x.FIO
                }).AsEnumerable().Select(y => new Inspector
                {
                     Id=y.Id,
                     FIO =y.FIO
                }).ToList();
                Inspectors = i.ToObservableCollection();
                //InfoInspectors = Inspectors.Select(x => x.ShallowCopy()).ToObservableCollection();
                InfoInspectors.Add(new Inspector
                {
                    Id = -1,
                    FIO = "Все"
                });
                foreach (var index in Inspectors)
                {
                    InfoInspectors.Add(index.ShallowCopy());
                }
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
