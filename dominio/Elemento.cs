using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio // CAMBIAMOS winform-app POR EL NUEVO NAMESPACE dominio
{
    public class Elemento // PONEMOS LA CLASE COMO public
    {
        // CREAMOS LA CLASE ELEMENTO Y AÑADIMOS LAS 2 PROPIEDADES QUE VA A TENER
        public int Id { get; set; }
        public string Descripcion { get; set; } // ESTA PROPIEDAD VA ACTUAR DOBLE:
        // 1) VA A SER EL TIPO DE POKEMON
        // 2) VA A SER LA DEBILIDAD DEL POKEMON


        // SOBRE-ESCRIBIMOS EL METODO ToString PARA PODER PONER EL Tipo DE POKEMON
        // DE LA Descripcion DE LA TABLA DEL OBJETO ELEMENTO,
        // SINO SE HACE ESTO, SE PONE UN NOMBRE GENERICO DEL OBJETO ELEMENTO (windows......)
        public override string ToString()
        {
            return Descripcion; // ESTO VALE PARA EL Tipo Y TAMBIEN PARA LA Debilidad
                                // YA QUE LA TABLA SERIA LA MISMA
        }

    }
}
