using GridLayout.Controls;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GridLayout.Models
{
    public class GridLayout
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string UserID { get; set; } // PK : UserID, Menu, ColumnCode
        //[Indexed(Name = "idx_menu", Order = 1, Unique = false)]
        public string MenuCode { get; set; }
        public string ColumnCode { get; set; }
        public string ColumnName { get; set; }
        public ColumnType ColumnType { get; set; } //Enum {String, Int}
        public TextAlignment HorizontalTextAlignment { get; set; }
        public int Width { get; set; } //#사용자 수정 가능
        public string IsUse { get; set; } //#사용자 수정 가능
        public int Seq { get; set; } //#사용자 수정 가능
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
    }
}
