using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; // SE AÑADE LA LIBRERIA DE SQLCLIENT PARA LA CONECTIVIDAD A SQL
//using System.Windows.Forms; // QUITAMOS ESTE using PORQUE NO ES NECESARIO YA
using dominio;  // INCLUIMOS EL using DE dominio PARA PODER ENLAZARLO

namespace negocio // CAMBIAMOS winform-app POR EL NUEVO NAMESPACE negocio
{
    public class PokemonNegocio  // PONEMOS LA CLASE COMO public
    {
        public List<Pokemon> listar() // SE CREA EL METODO "listar" EN FORMA DE LISTA
        {
            List<Pokemon> lista = new List<Pokemon>(); // CREAMOS EL OBJETO LISTA "lista"
            SqlConnection conexion = new SqlConnection(); // OBJETO PARA CONECTARSE
            SqlCommand comando = new SqlCommand();// OBJETO PARA REALIZAR ACCIONES
            SqlDataReader lector; // CREAMOS LA VARIABLE PARA ALOJAR LOS DATOS

            try
            {
                // 1º CONFIGURAMOS LA CADENA DE CONEXION CON LA BD. SE PUEDE HACER DE VARIAS MANERAS:
                //  a) CON LA DIRECCION DE SQL = "server=DESKTOP-CGKTRI6\\SQLEXPRESS"
                //  b) CON LA DIRECCION DE SQL = "server=(local)\\SQLEXPRESS"
                //  c) CON LA DIRECCION DE SQL = "server=.\\SQLEXPRESS"
                //  d) CON LA DIRECCION DE IP = "IP=100.0.0.0\\"
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                // 2º CONFIGURAMOS EL COMANDO PARA REALIZAR LA ACCION (SENTENCIA SQL QUE QUEREMOS EJECUTAR Y SU FORMATO TYPE.
                //  a) CON TYPE.TEXT - SE MANDA EN FORMA DE TEXTO (SENTENCIA DE SQL)
                //  b) CON TYPE.STOREDPROCEDURE - SE MANDA CON UN PROCEDIMIENTO ALMACENADO (FUNCION)
                comando.CommandType = System.Data.CommandType.Text;
                // 3º REALIZAMOS LA CONSULTA SQL
                comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad  From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo AND D.Id = P.IdDebilidad";
                // 4º LLAMAR A LA EJECUCION DEL COMANDO DE CONEXION
                comando.Connection = conexion;
                // 5º ABRIMOS LA CONEXION
                conexion.Open();
                // 6º REALIZAMOS LA LECTURA Y LA GUARDAMOS EN LA VARIABLE DE DATAREADER "lector"
                lector = comando.ExecuteReader();

                // CON ESTO GENERAMOS UNA TABLA VIRTUAL CON TODOS LOS DATOS SOLICITADOS A SQL


                // AHORA CON EL "WHILE" LEEMOS LA TABLA VIRTUAL, SIEMPRE QUE HAYA DATOS
                while (lector.Read())
                {
                // CREAMOS UNA VARIABLE PARA CARGAR CON LOS DATOS DEL LECTOR EN EL OBJETO POKEMON
                    Pokemon aux = new Pokemon();
                    // SE PUEDE CARGAR DE DOS FORMAS CON EL TIPO DE DATO CORRECTO SQL-VS
                    // a) CON EL INDICE DE LA COLUMNA
                    aux.Numero = lector.GetInt32(0);// ESTO ES UN INT EN VS Y GETINT32 EN SQL
                    // b) CON EL NOMBRE DE LA COLUMNA
                    aux.Nombre = (string)lector["Nombre"];// SE PONE EL NOMBRE DE LA COLUMNA QUE SE PUSO EN LA SENTENCIA A LA HORA DE SOLICITAR LOS DATOS EN EL COMMAND
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.UrlImagen = (string)lector["UrlImagen"];
                    aux.Tipo = new Elemento(); // CREAMOS LA VARIABLE aux.Tipo DEL OBJETO ELEMENTO
                    aux.Tipo.Descripcion = (string)lector["Tipo"];// SEÑALAMOS QUE ES LA COLUMNA Descripcion DE LA TABLA ELEMENTO
                    aux.Debilidad = new Elemento(); // CREAMOS LA VARIABLE aux.Debilidad DEL OBJETO ELEMENTO
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"]; // SEÑALAMOS QUE ES LA COLUMNA Descripcion DE LA TABLA ELEMENTO

                // GUARDAMOS LA FILA DE LA VARIABLE "AUX" EN LA LISTA "lista" HASTA QUE YA NO
                // HAYA MAS DATOS -- y ---> WHILE = FALSE
                    lista.Add(aux);
                }
                // CERRAMOS LA CONEXION - PERO SE SUELE PONER EN EL FINALLY DE LA FUNCION TRY
                conexion.Close();
                // NOS DEVULVE LA LISTA RELLENADA CON LOS DATOS Y SELECCIONABLE
                return lista;
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Hay un problema...." + ex);
                throw ex;

            }
        }
    }
}
