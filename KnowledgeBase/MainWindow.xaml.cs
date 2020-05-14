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
using KnowledgeBase.Views;

namespace KnowledgeBase
{
    enum EditCommand
    {
        AddRule,
        EditRule,
        DeleteRule,
        AddParameter,

        HowDiagnose,
        HowEditKnowledgeBase,
        HowAddParameter,
    }

    public partial class MainWindow : Window
    {
        private Grid mainGrid;
        private Menu mainMenu;
        private Button diagnoseButton;
        private IList<Parameter> parameters;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();

            this.Content = new Grid { Children = { mainMenu, mainGrid, diagnoseButton } };
        }

        private void Initialize()
        {
            InitializeGrid();
            InitializeMenu();
            diagnoseButton = new Button
            {
                Content = "Диагностировать",
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 10, 10),
                Width = 120,
                Height = 20,
                VerticalAlignment = VerticalAlignment.Bottom,
            };
            diagnoseButton.Click += Diagnose_Сlick;
        }
        private void InitializeMenu()
        {
            mainMenu = new Menu
            {
                Margin = new Thickness(0, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
            };

            #region EditItems
            MenuItem addRuleItem = new MenuItem { Name = "AddRule", Header = "Добавить правило" };
            addRuleItem.Click += MenuItem_Click;
            MenuItem editRuleItem = new MenuItem { Name = "EditRule", Header = "Изменить правило" };
            editRuleItem.Click += MenuItem_Click;
            MenuItem deleteRuleItem = new MenuItem { Name = "DeleteRule", Header = "Удалить правило" };
            deleteRuleItem.Click += MenuItem_Click;
            MenuItem addParameterItem = new MenuItem { Name = "AddParameter", Header = "Добавить параметр" };
            addParameterItem.Click += MenuItem_Click;
            MenuItem editItems = new MenuItem
            {
                Header = "Редактировать",
                ItemsSource = new List<Control> { addRuleItem, editRuleItem, deleteRuleItem, new Separator(), addParameterItem }
            };
            #endregion
            #region HelpItems
            MenuItem howDiagnoseItem = new MenuItem { Name = "HowDiagnose", Header = "Как диагностировать?" };
            howDiagnoseItem.Click += MenuItem_Click;
            MenuItem howEditKnowledgeBaseItem = new MenuItem { Name = "HowEditKnowledgeBase", Header = "Как редактировать БЗ?" };
            howEditKnowledgeBaseItem.Click += MenuItem_Click;
            MenuItem howAddParameterItem = new MenuItem { Name = "HowAddParameter", Header = "Как добавить параметр?" };
            howAddParameterItem.Click += MenuItem_Click;
            MenuItem helpItems = new MenuItem
            {
                Header = "Помощь",
                ItemsSource = new List<MenuItem> { howDiagnoseItem, howEditKnowledgeBaseItem, howAddParameterItem }
            };
            #endregion
            MenuItem exitItem = new MenuItem
            {
                Name = "Exit",
                Header = "Выйти",
            };
            exitItem.Click += Exit_Click;

            mainMenu.ItemsSource = new List<MenuItem> { editItems, helpItems, exitItem };
        }
        private void InitializeGrid()
        {
            mainGrid = new Grid
            {
                Margin = new Thickness(0, 25, 0, 0),
                DataContext = parameters
            };

            parameters = new List<Parameter>
            {
                new Parameter { Title = "Длительность провала напряжения" },
                new Parameter { Title = "Коэффициент несимметрии напряжений по обратной последовательности, К2U" },
                new Parameter { Title = "Несимметрия напряжений" },
                new Parameter { Title = "Коэффициент n-ой гармонической составляющей напряжения, КU" },
                new Parameter { Title = "Погодные условия" },
                new Parameter { Title = "Напряжения по фазе С, UC" },
                new Parameter { Title = "Напряжения по фазе А, UA" },
                new Parameter { Title = "Напряжения по фазе В, UB" },
                new Parameter { Title = "Грозовые импульсные напряжения" },
                new Parameter { Title = "Отклонение напряжения по фазам А, В, С" },
                new Parameter { Title = "коэффициент искажения синусоидальности кривой напряжения" },
                new Parameter { Title = "Несинусоидальность напряжения" },
                new Parameter { Title = "Исправность технического состояния оборудования" }
            };

            RowDefinitionCollection rows = mainGrid.RowDefinitions;
            ColumnDefinitionCollection columns = mainGrid.ColumnDefinitions;

            for (int i = 0; i < parameters.Count; i++)
            {
                rows.Add(new RowDefinition() { Height = GridLength.Auto });
                columns.Add(new ColumnDefinition() { Width = GridLength.Auto });
                columns.Add(new ColumnDefinition() { Width = GridLength.Auto });

                Label label = new Label();
                label.SetBinding(
                    Label.ContentProperty,
                    new Binding
                    {
                        Source = parameters[i],
                        Path = new PropertyPath("Title"),
                        Mode = BindingMode.TwoWay
                    });
                mainGrid.Children.Add(label);
                Grid.SetColumn(label, 0);

                TextBox textBox = new TextBox { Width = 100, Height = 25 };
                textBox.SetBinding(
                    TextBox.TextProperty,
                    new Binding
                    {
                        Source = parameters[i],
                        Path = new PropertyPath("Value"),
                        Mode = BindingMode.TwoWay
                    });
                mainGrid.Children.Add(textBox);
                Grid.SetColumn(textBox, 1);

                Grid.SetRow(label, i);
                Grid.SetRow(textBox, i);
            }
        }

        private void Diagnose_Сlick(object sender, RoutedEventArgs e)
        {
            var a = mainGrid.Children;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string commandName = ((MenuItem)sender).Name;
            EditCommand command = (EditCommand)Enum.Parse(typeof(EditCommand), commandName);
            Window window;

            switch (command)
            {
                case EditCommand.AddRule:
                    window = new AddRuleWindow();
                    break;
                case EditCommand.EditRule:
                    window = new EditRuleWindow();
                    break;
                case EditCommand.DeleteRule:
                    window = new DeleteRuleWindow();
                    break;
                case EditCommand.AddParameter:
                    window = new AddParameterWindow();
                    break;
                default:
                    window = new Window();
                    break;
            }

            window.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
