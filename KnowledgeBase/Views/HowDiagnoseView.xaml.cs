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
    /// Логика взаимодействия для HowDiagnoseView.xaml
    /// </summary>
    public partial class HowDiagnoseView : Window
    {
        public string Text { get; set; }
        public HowDiagnoseView()
        {
            InitializeComponent();
            InitializeText();
            DataContext = this;
        }

        private void InitializeText()
        {
            Text = @"
        Для диагностирования оборудования необходимо ввести показания (в виде чисел) 
с приборов. Слева указано название параметра (маркер в отчёте с прибора), а справа
текстовое поле, куда необходимо ввести корректные значения. Например, в диапозоне 
[0, 30] ввести '23' или, в случае с погодой, '0' или '1'.
        Возможен ввод десятичных значений в виде '2.1' или '2,1'.

•dt — { 'Номинальные': [0, 30] }
•Weather — { 'Присутствуют': 0, 'Отсутствуют': 1 }
•K2U — { 'Номинальные': [0, 2], 'Предельно допустимые': [0, 4] }
•KUn — { 'Номинальные': [1, 1.5], 'Предельно допустимые': [0.7, 1.8] }
•Ua — { 'Номинальные': [209, 231], 'Предельно допустимые': [198, 242] }
•Ub — { 'Номинальные': [209, 231], 'Предельно допустимые': [198, 242] }
•Uc — { 'Номинальные': [209, 231], 'Предельно допустимые': [198, 242] }
";
        }
    }
}
