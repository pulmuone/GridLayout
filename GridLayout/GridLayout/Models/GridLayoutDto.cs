using GridLayout.Controls;
using GridLayout.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace GridLayout.Models
{
    public class GridLayoutDto : ObservableObject
    {
        private List<TextAlignmentModel> _lst;
        private int _horTextAlignmentCode = -1;
        public TextAlignmentModel _horTextAlignment;

        public GridLayoutDto()
        {

            _lst = new List<TextAlignmentModel>
            {
                new TextAlignmentModel {Code = (int)TextAlignment.Start, Name = "Start"},
                new TextAlignmentModel {Code = (int)TextAlignment.Center, Name = "Center"},
                new TextAlignmentModel {Code = (int)TextAlignment.End, Name = "End"}
            };

            TextAlignments.AddRange(_lst);
        }

        public TextAlignmentModel HorTextAlignment
        {
            get { return this._horTextAlignment; }
            set
            {
                if (_horTextAlignment == value || value == null) return;
                _horTextAlignment = value;
                HorTextAlignmentCode = _horTextAlignment.Code;
                OnPropertyChanged();
            }
        }

        public int HorTextAlignmentCode
        {
            get { return this._horTextAlignmentCode; }
            set
            {
                if (_horTextAlignmentCode == value) return;
                this._horTextAlignmentCode = value; 
                var row = _lst.FirstOrDefault(t => t.Code == value);
                HorTextAlignment = row;
                OnPropertyChanged();
            }
        }

        public int ID { get; set; }
        public string UserID { get; set; } // PK : UserID, Menu, ColumnCode
        public string MenuCode { get; set; }
        public string ColumnCode { get; set; }
        public string ColumnName { get; set; }
        public ColumnType ColumnType { get; set; }
        public string ColumnTypeName
        {
            get
            {
                string result = string.Empty;

                if (ColumnType == ColumnType.Label)
                {
                    result = "Label";
                }
                else if (ColumnType == ColumnType.Entry)
                {
                    result = "Entry";
                }
                else if (ColumnType == ColumnType.Button)
                {
                    result = "Button";
                }
                else if (ColumnType == ColumnType.Picker)
                {
                    result = "Picker";
                }
                else
                {
                    result = "";
                }

                return result;
            }
        }

        public TextAlignment HorizontalTextAlignment { get; set; }

        private int _width { get; set; } //#사용자 수정 가능
        public int Width
        {
            get { return this._width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public string IsUseValue { get; set; }
        private bool _isUse { get; set; } //#사용자 수정 가능
        public bool IsUse
        {
            get { return this._isUse; }
            set
            {
                _isUse = value;
                OnPropertyChanged();
            }
        }

        public int Seq { get; set; } //#사용자 수정 가능
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }


        private ObservableRangeCollection<TextAlignmentModel> _textAlignments = new ObservableRangeCollection<TextAlignmentModel>();

        public ObservableRangeCollection<TextAlignmentModel> TextAlignments
        {
            get => _textAlignments;
            set => SetProperty(ref this._textAlignments, value);
        }

        private bool _isBeingDragged;
        public bool IsBeingDragged
        {
            get { return _isBeingDragged; }
            set { SetProperty(ref _isBeingDragged, value); }
        }

        private bool _isBeingDraggedOver;
        public bool IsBeingDraggedOver
        {
            get { return _isBeingDraggedOver; }
            set { SetProperty(ref _isBeingDraggedOver, value); }
        }
    }
}
