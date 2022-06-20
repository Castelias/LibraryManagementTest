using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace LMS
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["LMS.Properties.Settings.CasteliasDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            ShowBorrowers();
            ShowBooksToBorrow();
            ShowBooks();

        }

        private void ShowBorrowers()
        {
            try
            {
                string query2 = "select firstName + ' ' + lastName as fullName, * from borrower";

                SqlDataAdapter adapter = new SqlDataAdapter(query2, sqlConnection);

                using (adapter)
                {
                    DataTable borrowerTable = new DataTable();

                    adapter.Fill(borrowerTable);

                    borrowerList.DisplayMemberPath = "fullName";
                    borrowerList.SelectedValuePath = "Id";
                    borrowerList.ItemsSource = borrowerTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }
    
        private void ShowBooks()
        {
            try
            {
                string query = "select * from Book a inner join BorrowerBooks " + "za on a.Id = za.BookId where za.BorrowerId = @BorrowerId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@BorrowerId", borrowerList.SelectedValue);

                    DataTable bookTable = new DataTable();

                    adapter.Fill(bookTable);

                    BookList.DisplayMemberPath = "Title";
                    BookList.SelectedValuePath = "Id";
                    BookList.ItemsSource = bookTable.DefaultView;
                }
            }
            catch (Exception)
            {

                // MessageBox.Show(e.ToString());
            }

        }

        private void ShowBooksToBorrow()
        {

            try
            {
                string query = " select * from Book";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    DataTable bookTable = new DataTable();
                    adapter.Fill(bookTable);

                    BooksToBorrowList.DisplayMemberPath = "Title";
                    BooksToBorrowList.SelectedValuePath = "Id";
                    BooksToBorrowList.ItemsSource = bookTable.DefaultView;
                }
            }
            catch (Exception)
            {
                // MessageBox.Show(e.ToString());
            }

        }

        private void borrowerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBooks();
        }

        private void BooksToBorrowList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void BookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // buttons

        private void Add_Borrower_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Borrower values (@firstName, @lastName)";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                cmd.Parameters.AddWithValue("@firstName", firstNameTextBox.Text);
                cmd.Parameters.AddWithValue("@lastName", lastNameTextBox.Text);
                cmd.ExecuteScalar();
                sqlConnection.Close();
                ShowBorrowers();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowBorrowers();
            }
        }

        private void Delete_Borrower_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Borrower where id = @BorrowerId";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                cmd.Parameters.AddWithValue("@BorrowerId", borrowerList.SelectedValue);
                cmd.ExecuteScalar();
                sqlConnection.Close();
                ShowBorrowers();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowBorrowers();
            }

        }
     
        private void Add_Book_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into BorrowerBooks values (@BorrowerId, @BookId)";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                cmd.Parameters.AddWithValue("@BorrowerId", borrowerList.SelectedValue);
                cmd.Parameters.AddWithValue("@BookId", BooksToBorrowList.SelectedValue);
                cmd.ExecuteScalar();
                sqlConnection.Close();
                ShowBooks();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowBooks();
                ShowBooksToBorrow();
            }
        }

        private void Delete_Book_btn_Click(object sender, RoutedEventArgs e)
        {
            // TODO

            try
            {
                string query = "delete from BorrowerBooks where id = @BookId";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                cmd.Parameters.AddWithValue("@BookId", BookList.SelectedValue);
                cmd.ExecuteScalar();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowBooks();
            }

        }

        private void Add_New_Book_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Book values (@Title, @Author)";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                cmd.Parameters.AddWithValue("@Title", Title.Text);
                cmd.Parameters.AddWithValue("@Author", Author.Text);
                cmd.ExecuteScalar();
                sqlConnection.Close();
                ShowBorrowers();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowBooksToBorrow();
            }

        }

        private void Delete_New_Book_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Book where id = @BookId";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                cmd.Parameters.AddWithValue("@BookId", BooksToBorrowList.SelectedValue);
                cmd.ExecuteScalar();
                sqlConnection.Close();
                ShowBooksToBorrow();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowBooksToBorrow();
            }
        }
    }
}
