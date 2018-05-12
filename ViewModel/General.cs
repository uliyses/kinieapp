using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KiniApp.ViewModel
{
    public static class General
    {
        public static List<object> GetListaEquipos()
        {
            var listaEquipos = new List<object>();
            try
            {
                XmlDocument XmlConf = new XmlDocument();

                XmlConf.Load(AppDomain.CurrentDomain.BaseDirectory+"\\AppK.xml");
                XmlNode equipos = XmlConf.SelectSingleNode("AppK//TEAMS");

                foreach (XmlNode Nodo in equipos.ChildNodes)
                {
                    var temp = new TempArchivos();
                    temp.Nombre = Nodo.Name;
                    temp.Ruta = Nodo.InnerText;
                    listaEquipos.Add(temp);
                }


                return listaEquipos;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
    }
    public class TempArchivos
    {
        private string nombre;
        private string ruta;

        public string Nombre { get => nombre; set => nombre = value; }
        public string Ruta { get => ruta; set => ruta = value; }
    }
}
