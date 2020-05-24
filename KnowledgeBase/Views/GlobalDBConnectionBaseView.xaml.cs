using KnowledgeBaseLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace KnowledgeBase.Views
{
    /// <summary>
    /// Логика взаимодействия для GlobalDBConnectionBaseView.xaml
    /// </summary>
    public partial class GlobalDBConnectionBaseView : Window, INotifyPropertyChanged
    {
        private string connectionString;
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; OnPropertyChanged(ConnectionString); }
        }

        private Command connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                if (connectCommand == null)
                {
                    connectCommand = new Command(Connect);
                }
                return connectCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public GlobalDBConnectionBaseView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Connect(object obj)
        {
            DBManager.ConnectionString = ConnectionString;
            this.Close();
        }
    }
}
