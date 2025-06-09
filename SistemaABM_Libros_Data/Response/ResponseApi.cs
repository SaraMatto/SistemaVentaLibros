using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaABM_Libros_Data.Response
{
    public class ResponseApi
    {
        public string Mensaje { get; set; }
        public bool Estado { get; set; }

        public ResponseApi(string mensaje, bool estado) {
           this.Mensaje = mensaje;
           this.Estado = estado;
        }
    }
}
