using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinieData
{
    public class UserK
    {
        public string objectID { get; set; }
        public string updateAt { get; set; }
        public string username { get; set; }
        public string userpassword { get; set; }
        public string userRol { get; set; }
        public Byte[] imagen { get; set; }
        public string email { get; set; }
        public string equipoId { get; set; }
        public bool useNotification { get; set; }
        public bool valido { get; set; }
    }
}
