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
    /// Логика взаимодействия для HowAddRuleToKnowledgeBaseView.xaml
    /// </summary>
    public partial class HowAddRuleToKnowledgeBaseView : Window
    {
        public string Text { get; set; }
        public HowAddRuleToKnowledgeBaseView()
        {
            InitializeComponent();
            InitializeText();
            DataContext = this;
        }

        private void InitializeText()
        {
            Text = @"
        Правила — это утверждения, которые либо верны, либо нет. Из нескольких утверждений, 
соответственно, можно делать новые, которые соединяются друг с другом с помощью логических 
соединений 'И' и 'ИЛИ'. Последнее имеет бóльшую ранг, чем у первого. Например, в правиле:

                dt — Successful И K2U — Nominal ИЛИ K2U — Maximum

проверяется сначала 'K2U — Nominal ИЛИ K2U — Maximum', а лишь потом при положительном 
результате этого выражения проверяется 'dt — Successful'.
        При добавлении правил необходимо выбрать из списков (в виде пустых прямоугольников) 
необходимые части утверждения. Для добавления двух и более правил необходимо обязательно 
выбрать логическое соединение 'И' или 'ИЛИ'. Такэе необходимо учитывать название параметра 
(маркер в отчёте с прибора) и терм-множество, которое он может принимать:

•Погодные условия (Weather) — ['Successful', 'Unsuccessful']
•Несимметрия напряжений (K2Ui) — ['Successful', 'Unsuccessful']
•Напряжения по фазе A (Ua) — ['Nominal', 'Maximum', 'Exceeding']
•Напряжения по фазе B (Ub) — ['Nominal', 'Maximum', 'Exceeding']
•Напряжения по фазе C (Uc) — ['Nominal', 'Maximum', 'Exceeding']
•Длительность провала напряжения (dt) — ['Successful', 'Unsuccessful']
•Несинусоидальность напряжения (Uns) — ['Nominal', 'Maximum', 'Exceeding']
•Грозовые импульсные напряжения (Uimp) — ['Nominal', 'Maximum', 'Exceeding']
•Отклонение напряжения по фазам А, В, С (dUabc) — ['Nominal', 'Maximum', 'Exceeding']
•Коэф. искажения синусоидальности кривой напр. (KU) — ['Nominal', 'Maximum', 'Exceeding']
•Коэф. несимметрии напр. по обр. последовательности (K2U) — ['Nominal', 'Maximum', 'Exceeding']
•Коэф. n-ой гармонической составляющей напряжения (KUn) — ['Nominal', 'Maximum', 'Exceeding']
•Исправность тех. сост. оборудования (ServiceabilityEquipment) — ['Nominal', 'Maximum', 'Exceeding']

•Successful — Отсутствует
•Unsuccessful — Присутствует
•Nominal — Номинальные значения
•Exceeding — Превышающие значения
•Maximum — Предельно допустимые значения
";
        }
    }
}
