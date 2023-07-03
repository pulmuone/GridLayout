using GridLayout.Data;
using GridLayout.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GridLayout.ViewModels
{
    public class CollectionViewGridViewModel : BaseViewModel
    {
        public Action<bool> ClosedEvent;
        INavigation Navigation => Application.Current.MainPage.Navigation;
        private CancellationTokenSource _source = new CancellationTokenSource();
        public ICommand OKayCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ResetCommand { get; }

        public ICommand ItemDragged { get; }

        public ICommand ItemDraggedOver { get; }

        public ICommand ItemDragLeave { get; }

        public ICommand ItemDropped { get; }
        public ICommand CheckedChangedCommand { get; }
        public ICommand SelectionChangedCommand { get; }

        private string _menuCode = string.Empty;
        private GridLayoutDto _selectedItem;

        private ObservableRangeCollection<GridLayoutDto> _gridLayouts = new ObservableRangeCollection<GridLayoutDto>();

        public ObservableRangeCollection<GridLayoutDto> GridLayouts
        {
            get => _gridLayouts;
            set => SetProperty(ref this._gridLayouts, value);
        }

        public GridLayoutDto SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref this._selectedItem, value);
        }

        public CollectionViewGridViewModel(string menuCode)
        {
            this._menuCode = menuCode;

            OKayCommand = new Command<object>(async (obj) => await Okay(obj), (obj) => IsControlEnable);
            CancelCommand = new Command<object>(async (obj) => await Cancel(obj), (obj) => IsControlEnable);
            ResetCommand = new Command<object>(async (obj) => await Reset(obj), (obj) => IsControlEnable);
            CheckedChangedCommand = new Command<int>((obj) => CheckedChanged(obj), (obj) => IsControlEnable);
            SelectionChangedCommand = new Command<GridLayoutDto>((obj) => SelectionChanged(obj), (obj) => IsControlEnable);

            ItemDragged = new Command<GridLayoutDto>(OnItemDragged);
            ItemDraggedOver = new Command<GridLayoutDto>(OnItemDraggedOver);
            ItemDragLeave = new Command<GridLayoutDto>(OnItemDragLeave);
            ItemDropped = new Command<GridLayoutDto>(i => OnItemDropped(i));

            MainThread.BeginInvokeOnMainThread(async () => await Init());
        }

        private void SelectionChanged(GridLayoutDto obj)
        {
            Debug.WriteLine(obj?.ColumnName);
        }

        private void CheckedChanged(int ID)
        {
            Debug.WriteLine(ID);
            GridLayoutDto selectedItem = this.GridLayouts.Where(i => i.ID == ID).FirstOrDefault();
            this.SelectedItem = selectedItem;
        }

        private void OnStateRefresh()
        {
            OnPropertyChanged(nameof(GridLayouts));
        }

        private void OnItemDragged(GridLayoutDto item)
        {
            GridLayouts.ForEach(i => i.IsBeingDragged = item == i);
        }

        private void OnItemDraggedOver(GridLayoutDto item)
        {
            //Debug.WriteLine($"OnItemDraggedOver: {item?.Title}");
            var itemBeingDragged = _gridLayouts.FirstOrDefault(i => i.IsBeingDragged);
            GridLayouts.ForEach(i => i.IsBeingDraggedOver = item == i && item != itemBeingDragged);
        }

        private void OnItemDragLeave(GridLayoutDto item)
        {
            GridLayouts.ForEach(i => i.IsBeingDraggedOver = false);
        }

        private void OnItemDropped(GridLayoutDto item)
        {
            var itemToMove = _gridLayouts.First(i => i.IsBeingDragged);
            var itemToInsertBefore = item;

            if (itemToMove == null || itemToInsertBefore == null || itemToMove == itemToInsertBefore)
                return;

            GridLayouts.Remove(itemToMove);

            var insertAtIndex = GridLayouts.IndexOf(itemToInsertBefore);

            GridLayouts.Insert(insertAtIndex, itemToMove);
            itemToMove.IsBeingDragged = false;
            itemToInsertBefore.IsBeingDraggedOver = false;
        }

        public async Task Init()
        {
            GridDatabase database = await GridDatabase.Instance;
            List<Models.GridLayout> dbGridColumns = await database.GetColumnsAsync("admin", this._menuCode);

            GridLayouts.Clear();
            foreach (var item in dbGridColumns)
            {
                GridLayouts.Add(new GridLayoutDto()
                {
                    ID = item.ID,
                    UserID = "admin",
                    MenuCode = item.MenuCode,
                    ColumnCode = item.ColumnCode,
                    ColumnName = item.ColumnName,
                    ColumnType = item.ColumnType,
                    HorizontalTextAlignment = item.HorizontalTextAlignment,
                    HorTextAlignmentCode = (int)item.HorizontalTextAlignment,
                    Width = item.Width,
                    IsUse = item.IsUse == "Y" ? true : false,
                    Seq = item.Seq,
                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate
                });
            }
        }


        private async Task Okay(object obj)
        {
            IsControlEnable = false;
            IsBusy = true;
            (OKayCommand as Command).ChangeCanExecute();

            GridDatabase database = await GridDatabase.Instance;
            for (int i = 0; i < GridLayouts.Count; i++)
            {
                GridLayouts[i].Seq = i;
                GridLayouts[i].IsUseValue = GridLayouts[i].IsUse ? "Y" : "N";
                //GridColumns[i].HorizontalTextAlignment = GridColumns[i].HorTextAlignment.Code;

                if (GridLayouts[i].HorTextAlignmentCode == 0)
                {
                    GridLayouts[i].HorizontalTextAlignment = TextAlignment.Start;
                }
                else if (GridLayouts[i].HorTextAlignmentCode == 1)
                {
                    GridLayouts[i].HorizontalTextAlignment = TextAlignment.Center;
                }
                else if (GridLayouts[i].HorTextAlignmentCode == 2)
                {
                    GridLayouts[i].HorizontalTextAlignment = TextAlignment.End;
                }
            }

            database.UpdateItem(GridLayouts.ToList());

            ClosedEvent?.Invoke(true);

            ((Xamarin.CommunityToolkit.UI.Views.Popup)obj).Dismiss(true);

            IsControlEnable = true;
            IsBusy = false;
            (OKayCommand as Command).ChangeCanExecute();
        }

        private async Task Reset(object obj)
        {
            IsControlEnable = false;
            IsBusy = true;
            (ResetCommand as Command).ChangeCanExecute();

            GridDatabase database = await GridDatabase.Instance;

            await database.DeleteItemAsync("admin", this._menuCode);


            ClosedEvent?.Invoke(true);

            //ToDo
            ((Xamarin.CommunityToolkit.UI.Views.Popup)obj).Dismiss(true);

            IsControlEnable = true;
            IsBusy = false;
            (ResetCommand as Command).ChangeCanExecute();
        }

        private async Task Cancel(object obj)
        {
            IsControlEnable = false;
            IsBusy = true;
            (CancelCommand as Command).ChangeCanExecute();

            ClosedEvent?.Invoke(false);

            //ToDo
            ((Xamarin.CommunityToolkit.UI.Views.Popup)obj).Dismiss(false);

            IsControlEnable = true;
            IsBusy = false;
            (CancelCommand as Command).ChangeCanExecute();
        }
    }
}
