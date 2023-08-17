using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winform_app
{
    class Pokemon
    {
        public int Numero { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string UrlImagen { get; set; } // DAMOS DE ALTA LA PROP PARA LA IMAGEN
        public Elemento Tipo { get; set; } // DAMOS DE ALTA LA PROP PARA EL TIPO DE POKEMON
        public Elemento Debilidad { get; set; } // DAMOS DE ALTA LA PROP PARA LA DEBILIDAD DEL POKEMON
    }
}
