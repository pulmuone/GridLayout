using GridLayout.Controls;
using GridLayout.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace GridLayout.ViewModels
{
    public class MemberViewModel : BaseViewModel
    {
        private ObservableRangeCollection<EmpModel> _empList = new ObservableRangeCollection<EmpModel>();
        public ICommand SearchCommand { get; }
        public ICommand SaveCommand { get; }
        public MemberViewModel()
        {
            SearchCommand = new Command(() => Search(), () => IsControlEnable);
            SaveCommand = new Command(() => Save(), () => IsControlEnable);
            SortedCommand = new Command<DataGridHeader>(Sorted);
        }

        private void Save()
        {
            IsControlEnable = false;
            IsBusy = true;
            (SaveCommand as Command).ChangeCanExecute();

            //ToDo

            foreach (var item in EmpList)
            {
                Debug.WriteLine($"EmpList : {item.EmpId}, {item.EmpName}, {item.Addr}, {item.Age}, {item.GradeCode}");
            }

            IsControlEnable = true;
            IsBusy = false;
            (SaveCommand as Command).ChangeCanExecute();
        }

        private void Search()
        {
            IsControlEnable = false;
            IsBusy = true;
            (SearchCommand as Command).ChangeCanExecute();

            List<EmpModel> Employees = new List<EmpModel>()
            {
                new EmpModel {EmpId = 1,  EmpName = "name #1", Addr="1, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 11, GradeCode = "001"},
                new EmpModel {EmpId = 2,  EmpName = "name #2", Addr="2, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 12, GradeCode = "002"},
                new EmpModel {EmpId = 3,  EmpName = "name #3", Addr="3, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 13, GradeCode = "003"},
                new EmpModel {EmpId = 4,  EmpName = "name #4", Addr="4, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 14, GradeCode = "004"},
                new EmpModel {EmpId = 5,  EmpName = "name #5", Addr="5, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 15, GradeCode = "001"},
                new EmpModel {EmpId = 6,  EmpName = "name #6", Addr="6, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 16, GradeCode = "002"},
                new EmpModel {EmpId = 7,  EmpName = "name #7", Addr="7, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 17, GradeCode = "003"},
                new EmpModel {EmpId = 8,  EmpName = "name #8", Addr="8, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 18, GradeCode = "004"},
                new EmpModel {EmpId = 9,  EmpName = "name #9", Addr="9, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 19, GradeCode = "001"},
                new EmpModel {EmpId = 10,  EmpName = "name #10", Addr="10, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 20, GradeCode = "002"},
                new EmpModel {EmpId = 11,  EmpName = "name #10", Addr="11, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 20, GradeCode = "002"},
                new EmpModel {EmpId = 12,  EmpName = "name #10", Addr="12, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 20, GradeCode = "002"},
                new EmpModel {EmpId = 13,  EmpName = "name #10", Addr="13, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 20, GradeCode = "002"},
                new EmpModel {EmpId = 14,  EmpName = "name #10", Addr="14, Hangang-daero, Yongsan-gu, Seoul, Republic of Korea", Age = 20, GradeCode = "002"}
            };

            EmpList.ReplaceRange(Employees);

            IsControlEnable = true;
            IsBusy = false;
            (SearchCommand as Command).ChangeCanExecute();
        }

        private void Sorted(DataGridHeader e)
        {
            try
            {
                if (!e.SortingEnabled)
                {
                    return;
                }

                SortingOrder sortMethod;

                if (e.SortFlag == SortingOrder.None || e.SortFlag == SortingOrder.Ascendant)
                {
                    sortMethod = SortingOrder.Descendant;
                }
                else
                {
                    sortMethod = SortingOrder.Ascendant;
                }

                e.SortFlag = sortMethod;
                List<EmpModel> lst = EmpList.ToList();

                SortData.SortList(ref lst, e.SortFlag, e.FieldName);
                EmpList.ReplaceRange(lst);
            }
            catch(Exception ex) 
            { 
                Debug.WriteLine(ex.Message);
            }           
        }

        public ObservableRangeCollection<EmpModel> EmpList
        {
            get => this._empList;

            set => SetProperty(ref this._empList, value);
        }
    }
}
