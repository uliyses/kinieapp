using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace KiniApp.ViewModel
{
    public class GeneralVM: ViewModelBase, INotifyDataErrorInfo
    {
        private Control controlG;
        public async Task<T> RunProcess<T>(Task<T> tarea, string textoLoading = "Procesando")
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken cancelationToker = tokenSource2.Token;
            try
            {

                //LoadingPanelGeneral = true;
                //TextoLoadingPanel = textoLoading;
                var resultadoFinal = await Task.Run(async () =>
                {
                    tarea.Start();
                    var resultadoTarea = await tarea;
                    return resultadoTarea;
                }, cancelationToker);
                //LoadingPanelGeneral = false;
                tokenSource2.Dispose();
                return resultadoFinal;
            }
            catch (Exception)
            {
                //LoadingPanelGeneral = false;
                tokenSource2.Cancel();
                throw;
            }
        }

        public async Task RunProcess(Task tarea, string textoLoading = "Procesando")
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken cancelationToker = tokenSource2.Token;
            //LoadingPanelGeneral = true;
            await Task.Run(async () =>
            {
                tarea.Start();
                await tarea;
                return tarea;
            }, cancelationToker);
            //Cronometro = string.Empty;
            //LoadingPanelGeneral = false;

        }

        #region errors
        public Dictionary<String, List<String>> errors =
            new Dictionary<string, List<string>>();

        public Dictionary<int, String> OrdenErrors = new Dictionary<int, string>();

        public String GetFirstError()
        {
            return OrdenErrors.OrderBy(i => i.Key).FirstOrDefault().Value;
        }

        public void AddError(string propertyName, string error, bool isWarning, Object Item = null, String NameCampo = "")
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (!errors.ContainsKey(propertyName))
                    errors[propertyName] = new List<string>();

                if (!errors[propertyName].Contains(error))
                {
                    if (isWarning)
                        errors[propertyName].Add(error);
                    else
                        errors[propertyName].Insert(0, error);

                    if (OrdenErrors.Count(e => e.Value == propertyName) <= 0)
                        OrdenErrors.Add(errors.Count, propertyName);

                    RaiseErrorsChanged(propertyName);
                }
                String Tmp;
                UIElement ctl = FindBoundElement(propertyName, controlG, out Tmp) as UIElement;

              
                //if ( /*(this.balloon == null || !this.balloon.IsLoaded) &&*/ ctl != null)
                //{
                //    var balloon = new Balloon((System.Windows.Controls.Control)ctl, error, BalloonType.Error, false,
                //        false);
                //    balloon.Show();
                //}
            });
        }
        public void RemoveError(string propertyName, string error = "", Object Item = null, String NameCampo = "")
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (!errors.ContainsKey(propertyName))
                    return;
                if (error == "")
                {
                    errors.Remove(propertyName);
                    var llave = OrdenErrors.FirstOrDefault(e => e.Value == propertyName).Key;
                    OrdenErrors.Remove(llave);
                    Dictionary<int, String> Temp = new Dictionary<int, string>();
                    int ini = 1;
                    foreach (var item in OrdenErrors)
                    {
                        Temp.Add(ini++, item.Value);
                    }
                    OrdenErrors.Clear();

                    foreach (var item in Temp)
                    {
                        OrdenErrors.Add(item.Key, item.Value);
                    }

                    RaiseErrorsChanged(propertyName);
                }
                else
                {
                    if (errors.ContainsKey(propertyName) &&
                        errors[propertyName].Contains(error))
                    {
                        errors[propertyName].Remove(error);
                        if (errors[propertyName].Count == 0) errors.Remove(propertyName);
                        RaiseErrorsChanged(propertyName);
                    }
                }
               
            });
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName) ||
                  !errors.ContainsKey(propertyName)) return null;
            return errors[propertyName];
        }

        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }

        public void ClearErrors()
        {
            if (HasErrors)
            {
                while (errors.Count > 0)
                {
                    RemoveError(errors.FirstOrDefault().Key);
                }
            }
        }
        #endregion

        #region FindBoundElement
        public DependencyObject FindBoundElement(String fullPath, DependencyObject parent, out String PropertyOriginal)
        {
            DependencyObject boundElement = null;
            PropertyOriginal = null;

            LocalValueEnumerator localValues = parent.GetLocalValueEnumerator();
            while (localValues.MoveNext())
            {
                LocalValueEntry entry = localValues.Current;
                if (BindingOperations.IsDataBound(parent, entry.Property))
                {
                    BindingExpression expression = BindingOperations.GetBindingExpression(parent, entry.Property);
                    //Binding binding = BindingOperations.GetBinding(parent, entry.Property);

                    if (expression == null)
                    {
                        continue;
                    }
                    try
                    {
                        //VNEmpresa.WriteLine(entry.Property.Name + ", " + expression.ParentBinding.Path.Path);
                    }
                    catch (Exception)
                    {
                    }

                    if (String.Equals(expression.ParentBinding.Path?.Path, fullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        PropertyOriginal = expression.TargetProperty.Name;
                        return parent;
                    }
                }
            }

            if (boundElement != null)
                return boundElement;

            // Iterate through all children
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                DependencyObject result = FindBoundElement(fullPath, child, out PropertyOriginal);
                if (result != null)
                {

                    boundElement = result;
                    return boundElement;
                }

            }

            return boundElement;
        }
        #endregion


    }
}
