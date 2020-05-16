using KnowledgeBase.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnowledgeBase.ViewModel
{
    class MainViewModel
    {
        public IList<Parameter> Parameters { get; }
        
        private Command diagnoseCommand;
        private Command routeCommand;
        public ICommand DiagnoseCommand
        {
            get
            {
                if (diagnoseCommand == null)
                {
                    diagnoseCommand = new Command(
                        param => this.Diagnose(Parameters),
                        param => this.CanDiagnose(Parameters));
                }
                return diagnoseCommand;
            }
        }
        public ICommand RouteCommand
        {
            get
            {
                if (routeCommand == null)
                {
                    routeCommand = new Command(this.GoToView);
                }
                return routeCommand;
            }
        }
        

        public MainViewModel()
        {
            Parameters = Parameter.GetParameters();
        }

        private void Diagnose(object obj)
        {
            new DiagnosticView().Show();
        }
        private bool CanDiagnose(object obj)
        {
            IList<Parameter> parameters = (IList<Parameter>)obj;
            return parameters.All(p => !String.IsNullOrEmpty(p.Value));
        }

        private void GoToView(object obj)
        {
            try
            {
                Assembly currentAssembly = Assembly.GetExecutingAssembly();
                string titleView = obj.ToString();

                var findedType = currentAssembly
                    .GetTypes()
                    .Where(type => type.Name.StartsWith(titleView))
                    .First();

                string titleType = findedType.FullName;
                Type viewType = currentAssembly.GetType(titleType);

                Window view = (Window)Activator.CreateInstance(viewType);
                view.Show();
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(
                    "Выбранное представление не найдено! " +
                    "Параметр элемента меню должен передавать корректное название представления.", 
                    e);
            }
        }
    }
}
