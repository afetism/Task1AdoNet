using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Input;
using Task1AdoNet.Command;
using Task1AdoNet.Models;


namespace Task1AdoNet
{
   
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection=null;
        SqlDataReader dataReader=null;
        public ICommand AddCommand { get; }
        public ICommand FillCommand { get; }
        public ObservableCollection<Author> Authors {  get; set; }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            sqlConnection = new(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            Authors = [];
            AuthorsView.ItemsSource = Authors;
            AddCommand = new RelayCommand(AddAuthor);
            FillCommand = new RelayCommand(FillAuthors);
            DataContext = this;
        }

        private void FillAuthors()
        {
            
            
        }

        private void AddAuthor()
        {
            try
            {
                sqlConnection.Open();
                var query = "Insert INTO Authors(Id,Firstname,Lastname) Values (@id,@firstname,@lastname)";
                var cmd=new SqlCommand(query,sqlConnection);
                cmd.Parameters.AddWithValue("id", Id);
                cmd.Parameters.AddWithValue("firstname", FirstName);
                cmd.Parameters.AddWithValue("lastname", LastName);


                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void WindowLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                var query = "Select * from Authors";
                var cmd=new SqlCommand(query,sqlConnection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Authors.Add(new Author
                    {
                        Id = dataReader.GetInt32(0),
                        FirstName = dataReader.GetString(1),
                        LastName = dataReader.GetString(2),
                        
                    });
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error:" + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                dataReader.Close(); 
            }
        }
    }
}