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
    /// Логика взаимодействия для HowEditKnowledgeBaseView.xaml
    /// </summary>
    public partial class HowEditKnowledgeBaseView : Window
    {
        public string Text { get; set; }
        public HowEditKnowledgeBaseView()
        {
            InitializeComponent();
            InitializeText();
            DataContext = this;
        }

        private void InitializeText()
        {
            Text = @"
        Редактирование правил в базе знаний возможно двумя способами: удаление или изменение.
В случае удаления из разворачивающегося списка, расположенного сверху (в виде пустого 
прямоугольника), выберите интересующее вас правило полсе чего нажмите кнопку 'Удалить'.
        Процесс изменения схож с процессом добавления правила. Здесь также важно знать, какие
терм-множества может принимать тот или иной параметр. 
       
        Важно! При необдуманном изменении правила возможна некорректная работа всего ПО!

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
