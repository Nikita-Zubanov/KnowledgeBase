using KnowledgeBase.ViewModel;
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
    /// Логика взаимодействия для LogicalOutputView.xaml
    /// </summary>
    public partial class LogicalOutputView : Window
    {
        public LogicalOutputView(LogicalOutputViewModel diagnosticVM)
        {
            InitializeComponent();
            DataContext = diagnosticVM;
        }
    }
}
