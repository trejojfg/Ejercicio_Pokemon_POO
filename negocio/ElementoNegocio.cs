using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio; // INCLUIMOS EL using DE dominio PARA PODER ENLAZARLO

namespace negocio
{
    public class ElementoNegocio // PONEMOS LA CLASE COMO public
    {
        public List<Elemento> listar() // SE CREA EL METODO "listar" EN FORMA DE LISTA
        {
			List<Elemento> lista = new List<Elemento>(); // CREAMOS EL OBJETO LISTA "lista"
            AccesoDatos datos = new AccesoDatos(); // CREAMOS datos DESDE EL OBJETO AccesoDatos
            try
			{
				// CREAMOS LA CONSULTA SOBRE SQL CON LAS FUNCIONES/METODOS
				datos.setearConsulta("Select Id, Descripcion From ELEMENTOS");
				datos.ejecutarLectura();

				// CON EL while LEEMOS EL Lector Y CARGAMOS LOS DATOS EN LA lista

				while (datos.Lector.Read())
				{
                    // CREAMOS UNA VARIABLE PARA CARGAR CON LOS DATOS DEL LECTOR EN EL OBJETO Elemento
                    Elemento aux = new Elemento();

                    // SE CARGA EL TIPO DE DATO CORRECTO SQL-VS
                    aux.Id = (int)datos.Lector["Id"]; // SE PONE EL NOMBRE DE LA COLUMNA QUE SE PUSO EN
													  // LA SENTENCIA A LA HORA DE SOLICITAR LOS DATOS EN EL COMMAND
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    // GUARDAMOS LA FILA DE LA VARIABLE "AUX" EN LA LISTA "lista" HASTA QUE YA NO
                    // HAYA MAS DATOS -- y ---> WHILE = FALSE

                    lista.Add(aux);
				}

				return lista;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			finally // INTRODUCIMOS EL finally PARA PODER CERRAR EL Lector Y LA conexion A SQL, 
			// CON EL METODO cerrarConexion
			{
				datos.cerrarConexion();
			}
        }
    }
}
