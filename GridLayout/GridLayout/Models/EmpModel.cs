using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace GridLayout.Models
{
    public class EmpModel : BindableObject
    {
        public ICommand PlusButtonCommand { get; }
        public ICommand MinusButtonCommand { get; }

        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string Addr { get; set; }

        private int _age;
        private int _selectedRow;
        private string _gradeCode;
        private List<CodeModel> lst;
        public CodeModel _grade;

        private ObservableRangeCollection<CodeModel> _grades = new ObservableRangeCollection<CodeModel>();

        public EmpModel()
        {
            PlusButtonCommand = new Command(() => PlusButton());
            MinusButtonCommand = new Command(() => MinusButton());

            lst = new List<CodeModel>
            {
                new CodeModel {Code="001", Name="1 Grade"},
                new CodeModel {Code="002", Name="2 Grade"},
                new CodeModel {Code="003", Name="3 Grade"},
                new CodeModel {Code="004", Name="4 Grade"}
            };

            Grades.AddRange(lst);
        }

        public CodeModel Grade
        {
            get { return this._grade; }
            set
            {
                if (_grade == value || value == null) return;
                _grade = value;
                GradeCode = _grade.Code;
                OnPropertyChanged();
            }
        }

        public string GradeCode
        {
            get { return this._gradeCode; }
            set
            {
                if (_gradeCode == value) return;
                this._gradeCode = value;
                var row = lst.FirstOrDefault(t => t.Code == value);
                Grade = row;
                OnPropertyChanged();
            }
        }


        public int SelectedRow
        {
            get { return this._selectedRow; }
            set
            {
                if (_selectedRow == value) return;
                this._selectedRow = value;
                OnPropertyChanged();
            }
        }

        public int Age
        {
            get { return this._age; }
            set
            {
                if (_age == value) return;
                this._age = value;
                OnPropertyChanged();
            }
        }

        private void MinusButton()
        {
            Age -= 1;
        }

        private void PlusButton()
        {
            Age += 1;
        }

        public ObservableRangeCollection<CodeModel> Grades
        {
            get { return this._grades; }
            set
            {
                if (_grades == value) return;
                this._grades = value;
                OnPropertyChanged();
            }
        }
    }
}
