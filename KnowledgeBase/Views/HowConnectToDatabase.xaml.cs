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
using System.Windows.Shapes;

namespace KnowledgeBase.Views
{
    /// <summary>
    /// Логика взаимодействия для HowConnectToDatabase.xaml
    /// </summary>
    public partial class HowConnectToDatabase : Window
    {
        public string Text { get; set; }
        public HowConnectToDatabase()
        {
            InitializeComponent();
            InitializeText();
            DataContext = this;
        }

        private void InitializeText()
        {
            Text = @"
        Подключение к базе данных необходимо для использования базы знаний и её редактирования.
Строка подключения имеет вид, например:

Data Source=[название сервера]; Initial Catalog=[название базы данных]; Integrated Security=True.

        Однако в данном ПО предусмотрено автономное использование, но исключена возможность 
добавления или изменения БЗ. В таком случае нет необходимости в подключении для основного 
функционирования ПО.";
        }
    }
}
