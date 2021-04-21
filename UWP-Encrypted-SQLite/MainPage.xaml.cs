using SQLite_Backend;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace UWP_Encrypted_SQLite
{
    public sealed partial class MainPage : Page
    {
        string dbpath = ApplicationData.Current.LocalFolder.Path + "\\testdb.db";
        DataAccess dbAccess;

        public MainPage()
        {
            this.InitializeComponent();

            dbAccess = new DataAccess(dbpath);
        }

        private void CreateDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (dbAccess.CreateDatabase())
                CreateDBresBlock.Text = "Database created...";
            else
                CreateDBresBlock.Text = "Database creation failed, check console...";
        }

        private void DeleteDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(dbpath))
                File.Delete(dbpath);
            CreateDBresBlock.Text = "Database deleted...";
        }

        private void RunQuery_Click(object sender, RoutedEventArgs e)
        {
            QueryResultView.ItemsSource = dbAccess.RunQuery(Querybox.Text);
        }
    }
}
