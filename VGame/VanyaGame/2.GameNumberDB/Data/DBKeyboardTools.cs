using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace VanyaGame.GameNumber.Data
{
    /// <summary>
    /// Класс для загрузки даных из базы данных sql
    /// </summary>
    public class DBKeyboardTools
    {
        private SqlConnection sqlConnection;
        private void ReadDBToKeyLocationList(Dictionary<string, Point> keyLocations, string filename = @"\data\DatabaseKeyboard.mdf")
        {
            string DBpath = Game.Sets.MainDir + filename;
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + DBpath + @";Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open(); // не пользуемся async тк надо сначала загрузить потом играть

            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [KeyCoordinates]", sqlConnection);
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    double x = 0;
                    double y = 0;
                    string key = "";
                    x = (double)sqlReader["x"];
                    y = (double)sqlReader["y"];
                    key = (string)sqlReader["Key"];
                    Point point = new Point(x, y);
                    Game.Msg(Convert.ToString(sqlReader["Key"] + " " + sqlReader["x"] + " " + sqlReader["y"]));

                    keyLocations.Add(key, point);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (sqlReader != null) { sqlReader.Close(); }
                if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                {
                    sqlConnection.Close();
                }
            }
        }
        
        public void LoadKeyLocationsFromDB(Dictionary<string, Point> keyLocations, string filename = @"\data\DatabaseKeyboard.mdf")
        {
            ReadDBToKeyLocationList(keyLocations, filename);
        }
    }
}
