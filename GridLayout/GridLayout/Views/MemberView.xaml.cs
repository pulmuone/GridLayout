using GridLayout.Controls;
using GridLayout.Converters;
using GridLayout.Data;
using GridLayout.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace GridLayout.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MemberView : ContentPage
    {
        private readonly string _VIEW_NAME;
        public MemberView()
        {
            InitializeComponent();
            _VIEW_NAME = this.ToString();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();


            await GridColumnInit();
        }


        private async Task GridColumnInit()
        {
            try
            {
                GridDatabase database = await GridDatabase.Instance;

                List<Models.GridLayout> _localDbGridColumns = new List<Models.GridLayout>();

                //##############
                // 1.
                //##############

                gridCV.Children.Clear();


                //gridCV.ColumnDefinitions.Clear();

                //##############
                // 2. 컬럼 선언, 신규 컬럼은 마지막에 추가해야 한다.
                //##############
                List<Models.GridLayout> _gridColumns = new List<Models.GridLayout>()
                {
                new Models.GridLayout() {UserID = "admin", MenuCode = _VIEW_NAME, Seq = 0, ColumnCode = "EmpId", ColumnName = "ID", ColumnType = ColumnType.Label, HorizontalTextAlignment = TextAlignment.Start, Width = 70, IsUse = "Y", CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), ModifiedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") },
                new Models.GridLayout() {UserID = "admin", MenuCode = _VIEW_NAME, Seq = 1, ColumnCode = "EmpName", ColumnName = "Name", ColumnType = ColumnType.Label, HorizontalTextAlignment = TextAlignment.Start, Width = 100, IsUse = "Y", CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), ModifiedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") },
                new Models.GridLayout() {UserID = "admin", MenuCode = _VIEW_NAME, Seq = 2, ColumnCode = "Addr", ColumnName = "Address", ColumnType = ColumnType.Label, HorizontalTextAlignment = TextAlignment.Start, Width = 80, IsUse = "Y", CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), ModifiedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") },
                new Models.GridLayout() {UserID = "admin", MenuCode = _VIEW_NAME, Seq = 3, ColumnCode = "MinusButtonCommand", ColumnName = "-", ColumnType = ColumnType.Button, HorizontalTextAlignment = TextAlignment.End, Width = 50, IsUse = "Y", CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), ModifiedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") },
                new Models.GridLayout() {UserID = "admin", MenuCode = _VIEW_NAME, Seq = 4, ColumnCode = "Age", ColumnName = "Age", ColumnType = ColumnType.Entry, HorizontalTextAlignment = TextAlignment.End, Width = 50, IsUse = "Y", CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), ModifiedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") },
                new Models.GridLayout() {UserID = "admin", MenuCode = _VIEW_NAME, Seq = 5, ColumnCode = "PlusButtonCommand", ColumnName = "+", ColumnType = ColumnType.Button, HorizontalTextAlignment = TextAlignment.End, Width = 50, IsUse = "Y", CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), ModifiedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") },
                new Models.GridLayout() {UserID = "admin", MenuCode = _VIEW_NAME, Seq = 6, ColumnCode = "GradeCode", ColumnName = "Grade", ColumnType = ColumnType.Picker, HorizontalTextAlignment = TextAlignment.Start, Width = 90, IsUse = "Y", CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), ModifiedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") }
                };

                //######################
                // 3. 컬럼 기본값 적용 순서
                //######################
                _localDbGridColumns = await database.GetColumnsAsync("admin", _VIEW_NAME);

                foreach (var item in _gridColumns)
                {
                    var col = _localDbGridColumns.Where(i => i.UserID == item.UserID && i.ColumnCode == item.ColumnCode);
                    //로컬DB에 없으면 저장한다.
                    if (col == null || col.Count() == 0)
                    {
                        await database.SaveItemAsync(item);
                    }
                }

                //###################
                // 4. 사용 할 컬럼 조회
                //###################
                _localDbGridColumns = await database.GetColumnsAsync("admin", _VIEW_NAME);

                //###################
                // 5. 헤더
                //###################
                Grid _gridHeader = new Grid();
                _gridHeader.RowSpacing = 0;
                _gridHeader.ColumnSpacing = 0;
                _gridHeader.Padding = new Thickness(0, 0, 0, 0);
                _gridHeader.Margin = new Thickness(0, 0, 0, 0);
                _gridHeader.BackgroundColor = Color.Transparent;

                _gridHeader.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute) });
                _gridHeader.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Absolute) });

                for (int i = 0; i < _localDbGridColumns.Count; i++)
                {
                    if (_localDbGridColumns[i].Seq == i)
                    {
                        if (_localDbGridColumns[i].IsUse == "Y")
                        {
                            _gridHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_localDbGridColumns[i].Width, GridUnitType.Absolute) });
                        }
                        else
                        {
                            _gridHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Absolute) });
                        }

                        DataGridHeader dataGridHeader = new DataGridHeader()
                        {
                            FontSize = 12,
                            TextColor = Color.White,
                            BackgroundColor = Color.FromHex("#598CB5"),
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        };

                        dataGridHeader.Text = _localDbGridColumns[i].ColumnName;
                        dataGridHeader.FieldName = _localDbGridColumns[i].ColumnCode;

                        _gridHeader.Children.Add(dataGridHeader, i, 0);

                    }
                }

                BoxView boxView = new BoxView();
                boxView.CornerRadius = 5;
                boxView.Color = Color.DarkGray;
                _gridHeader.Children.Add(boxView, 0, _localDbGridColumns.Count, 1, 2);

                gridCV.Children.Add(_gridHeader);


                //###########################
                //6. CollectionView-DataTemplate
                //###########################

                CollectionView collectionView = new CollectionView();
                collectionView.SelectionMode = SelectionMode.Single;
                collectionView.SetBinding(CollectionView.ItemsSourceProperty, new Binding("EmpList"));

                var dataTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid();
                    grid.Style = Application.Current.Resources["gridStyle"] as Style;
                    grid.Padding = new Thickness(0, 0, 0, 0);
                    grid.Margin = new Thickness(0, 0, 0, 0);
                    grid.RowSpacing = 0;
                    grid.ColumnSpacing = 0;

                    //Alternating Row Colors- current bug, 두번째 gridCV.Children.Clear() 할 때 에러 발생
                    //Binding binding = new Binding();
                    //IndexToColorConverter indexToColorConverter = new IndexToColorConverter()
                    //{
                    //    EvenColor = Color.Transparent,
                    //    OddColor = Color.FromHex("#E3F2FD"),
                    //};
                    //binding.Converter = indexToColorConverter;
                    //binding.ConverterParameter = collectionView;
                    //binding.Path = ".";
                    //grid.SetBinding(Grid.BackgroundColorProperty, binding);

                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Absolute) });

                    for (int i = 0; i < _localDbGridColumns.Count; i++)
                    {
                        if (_localDbGridColumns[i].Seq == i)
                        {
                            if (_localDbGridColumns[i].IsUse == "Y")
                            {
                                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_localDbGridColumns[i].Width, GridUnitType.Absolute) });
                            }
                            else
                            {
                                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Absolute) });
                            }

                            if (_localDbGridColumns[i].ColumnType == ColumnType.Label)
                            {
                                Label label = new Label()
                                {
                                    VerticalTextAlignment = TextAlignment.Center,
                                    HorizontalTextAlignment = _localDbGridColumns[i].HorizontalTextAlignment,
                                    FontSize = 12,
                                    //BackgroundColor = Color.White,
                                    //Style = Application.Current.Resources["labelStyle"] as Style,
                                    TextColor = Color.Black,

                                };
                                label.SetBinding(Label.TextProperty, new Binding(_localDbGridColumns[i].ColumnCode));

                                grid.Children.Add(label, i, 0);
                            }
                            else if (_localDbGridColumns[i].ColumnType == ColumnType.Entry)
                            {
                                Entry entry = new Entry()
                                {
                                    VerticalTextAlignment = TextAlignment.Center,
                                    HorizontalTextAlignment = _localDbGridColumns[i].HorizontalTextAlignment,
                                    FontSize = 12,
                                    TextColor = Color.Black,
                                };
                                entry.SetBinding(Entry.TextProperty, _localDbGridColumns[i].ColumnCode);
                                //entry.SetBinding(Label.TextProperty, _localDbGridColumns[i].ColumnCode, stringFormat: "{0:#,##0}");

                                grid.Children.Add(entry, i, 0);
                            }
                            else if (_localDbGridColumns[i].ColumnType == ColumnType.Button)
                            {
                                Button button = new Button();
                                button.BackgroundColor = Color.Transparent;
                                button.TextColor = Color.Black;
                                button.Text = _localDbGridColumns[i].ColumnName;
                                button.SetBinding(Button.CommandProperty, _localDbGridColumns[i].ColumnCode);

                                grid.Children.Add(button, i, 0);
                            }
                            else if (_localDbGridColumns[i].ColumnType == ColumnType.Picker)
                            {
                                Picker picker = new Picker();
                                picker.FontSize = 12;
                                picker.ItemDisplayBinding = new Binding("Name");
                                //picker.SetBinding(Picker.TitleProperty, _localDbGridColumns[i].ColumnName);
                                picker.SetBinding(Picker.ItemsSourceProperty, "Grades");
                                picker.SetBinding(Picker.SelectedItemProperty, "Grade");
                                grid.Children.Add(picker, i, 0);
                            }
                        }
                    }

                    BoxView boxView = new BoxView();
                    boxView.CornerRadius = 5;
                    boxView.Color = Color.DarkGray;

                    grid.Children.Add(boxView, 0, _localDbGridColumns.Count, 1, 2); //하단 Merge

                    return grid;
                });

                collectionView.ItemTemplate = dataTemplate;

                gridCV.Children.Add(collectionView, 0, 1);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        private async void ToolbarItemGridCV_Clicked(object sender, EventArgs e)
        {
            ToolbarItemGridCV.IsEnabled = false;

            CollectionViewGridViewModel vm = new CollectionViewGridViewModel(_VIEW_NAME);
            vm.ClosedEvent += ClosedEvent;

            CollectionViewGridView page = new CollectionViewGridView();
            page.BindingContext = vm;

            await this.Navigation.ShowPopupAsync(page);

            ToolbarItemGridCV.IsEnabled = true;
        }

        private async void ClosedEvent(bool result)
        {
            if (result)
            {
                await GridColumnInit();

                //Task.Run(async () =>
                //{
                //    EMPDatabase database = await EMPDatabase.Instance;
                //    var path = await database.DBToExport();

                //    await Share.RequestAsync(new ShareFileRequest
                //    {
                //        Title = "DB File",
                //        File = new ShareFile(path)
                //    });

                //});
            }
        }
    }
}