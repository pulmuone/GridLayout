using GridLayout.Models;
using GridLayout.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GridLayout.Views
{
    public partial class CollectionViewGridView
    {
        public CollectionViewGridView()
        {
            InitializeComponent();

            Binding objBinding = new Binding()
            {
                Source = collectionView.ItemsSource,
                Path = "SelectedItem"
            };

            collectionView.SetBinding(CollectionView.SelectionChangedCommandParameterProperty, objBinding);
            collectionView.SetBinding(CollectionView.SelectionChangedCommandProperty, new Binding("SelectionChangedCommand"));
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                double number;
                bool isValid = Double.TryParse(e.NewTextValue, out number);
                ((Entry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
            }
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            GridLayoutDto selectedItem = (GridLayoutDto)(BindingContext as CollectionViewGridViewModel).GridLayouts.Where(i => i.ID == (int)((Entry)sender).ReturnCommandParameter).FirstOrDefault();
            this.collectionView.SelectedItem = selectedItem;
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            this.collectionView.SelectedItem = null;

            this.OKButton.Focus();

        }
    }
}