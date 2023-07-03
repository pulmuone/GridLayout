using GridLayout.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GridLayout.Data
{
    public class GridDatabase
    {

        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<GridDatabase> Instance = new AsyncLazy<GridDatabase>(async () =>
        {
            var instance = new GridDatabase();
            CreateTableResult result = await Database.CreateTableAsync<Models.GridLayout>();

            instance.CreateIndex();

            return instance;
        });

        public GridDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }


        private void CreateIndex()
        {
            Database.CreateIndexAsync("GridLayout", new string[] { "UserID", "MenuCode", "ColumnCode" }, true);
        }

        public async Task<string> DBToExport()
        {
            var backupPath = Path.Combine(FileSystem.CacheDirectory, "GRID.db3");

            await Database.BackupAsync(backupPath);

            return backupPath;
        }


        public async Task<int> SaveItemAsync(GridLayout.Models.GridLayout item)
        {
            int result = -1;
            result = await Database.UpdateAsync(item);
            if (result == 0)
            {
                result = await Database.InsertAsync(item);
            }

            return result;
        }


        public int UpdateItem(List<GridLayoutDto> items)
        {
            int rowCount = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePath))
                {
                    connection.BeginTransaction();

                    SQLiteCommand command = new SQLiteCommand(connection)
                    {
                        CommandText = @"UPDATE GridLayout 
                                        SET
                                            HorizontalTextAlignment = @HorizontalTextAlignment
                                        ,   Width = @Width
                                        ,   IsUse = @IsUse
                                        ,   Seq = @Seq
                                        WHERE UserID = @UserID
                                        AND MenuCode = @MenuCode 
                                        AND ColumnCode = @ColumnCode
                                        "
                    };

                    int affected = 0;

                    foreach (var item in items)
                    {
                        command.Bind("@UserID", item.UserID);
                        command.Bind("@MenuCode", item.MenuCode);
                        command.Bind("@ColumnCode", item.ColumnCode);
                        command.Bind("@Width", item.Width);
                        command.Bind("@IsUse", item.IsUseValue);
                        command.Bind("@Seq", item.Seq);
                        command.Bind("@HorizontalTextAlignment", item.HorizontalTextAlignment);

                        affected = command.ExecuteNonQuery();
                        rowCount += affected;
                        affected = 0;
                    }
                    connection.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return rowCount;
        }

        public int InsertAllItem(List<GridLayout.Models.GridLayout> items)
        {
            int rowCount = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePath))
                {
                    connection.BeginTransaction();

                    SQLiteCommand command = new SQLiteCommand(connection)
                    {
                        CommandText =
                        @"INSERT INTO GridLayout (
                           UserID,
                           MenuCode,
                           ColumnCode,
                           ColumnName,
                           ColumnType,
                           HorizontalTextAlignment,
                           Width,
                           IsUse,
                           Seq,
                           CreatedDate,
                           ModifiedDate)
                      VALUES (
                           @UserID,
                           @MenuCode,
                           @ColumnCode,
                           @ColumnName,
                           @ColumnType,
                           @HorizontalTextAlignment,
                           @Width,
                           @IsUse,
                           @Seq,
                           @CreatedDate,
                           @ModifiedDate
                       );"
                    };

                    int affected = 0;

                    foreach (var item in items)
                    {
                        command.Bind("@UserID", item.UserID);
                        command.Bind("@MenuCode", item.MenuCode);
                        command.Bind("@ColumnCode", item.ColumnCode);
                        command.Bind("@ColumnName", item.ColumnName);
                        command.Bind("@ColumnType", item.ColumnType);
                        command.Bind("@HorizontalTextAlignment", item.HorizontalTextAlignment);
                        command.Bind("@Width", item.Width);
                        command.Bind("@IsUse", item.IsUse);
                        command.Bind("@Seq", item.Seq);
                        command.Bind("@CreatedDate", item.CreatedDate);
                        command.Bind("@ModifiedDate", item.ModifiedDate);

                        affected = command.ExecuteNonQuery();
                        rowCount += affected;
                        affected = 0;
                    }
                    connection.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return rowCount;
        }

        public async Task<int> DeleteItemAsync(string UserID, string menuCode)
        {
            var result = await Database.ExecuteAsync(string.Format("DELETE FROM [GridLayout] WHERE UserID = '{0}' AND MenuCode = '{1}'", UserID, menuCode));

            return result;
        }

        //서버값을 반영 할 때 로컬에 있는것을 삭제하고 입력한다.
        public async Task<int> DeleteAllItemAsync(string UserID)
        {
            var result = await Database.ExecuteAsync(string.Format("DELETE FROM [GridLayout] WHERE UserID = '{0}'", UserID));

            return result;
        }


        public async Task<List<Models.GridLayout>> GetColumnsAsync(string UserID, string menuCode)
        {
            string sql =
                @"
                    SELECT
                        ID,
                        UserID,
                        MenuCode,
                        ColumnCode,
                        ColumnName,
                        ColumnType,
                        HorizontalTextAlignment,
                        Width,
                        IsUse,
                        Seq,
                        CreatedDate,
                        ModifiedDate
                    FROM
                        [GridLayout]
                    WHERE UserID = '{0}' 
                    AND MenuCode = '{1}'
                    ORDER BY Seq
                ";

            return await Database.QueryAsync<Models.GridLayout>(string.Format(sql, UserID, menuCode));
        }
    }
}
