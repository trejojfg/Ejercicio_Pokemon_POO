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
                comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo AND D.Id = P.IdDebilidad AND P.Activo = 1";
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
                    aux.Id = (int)lector["Id"];//NOS TRAEMOS EL Id DEL POKEMON PARA USARLO AL MODIFICAR POKEMON

                    // SE PUEDE CARGAR DE DOS FORMAS CON EL TIPO DE DATO CORRECTO SQL-VS
                    // a) CON EL INDICE DE LA COLUMNA
                    aux.Numero = lector.GetInt32(0);// ESTO ES UN INT EN VS Y GETINT32 EN SQL
                    // b) CON EL NOMBRE DE LA COLUMNA
                    aux.Nombre = (string)lector["Nombre"];// SE PONE EL NOMBRE DE LA COLUMNA QUE SE PUSO EN LA SENTENCIA A LA HORA DE SOLICITAR LOS DATOS EN EL COMMAND
                    aux.Descripcion = (string)lector["Descripcion"];

                    // HAY DOS MANERAS DE VALIDACION DE LOS DATOS CON UN null DE UNA COLUMNA,
                    // CUANDO EN DB DE SQL TIENE NULL Y EL CAMPO ADMITE NULL:
                    // a) UTILIZAMOS UN if CON NEGACION ! CONTRA LA COLUMNA
                    // if(!(lector.IsDBNull(lector.GetOrdinal("UrlImagen"))))// CONSULTA: si no es null en la columna UrlImagen
                    //    aux.UrlImagen = (string)lector["UrlImagen"];// condicional SI - EJECUTA ESTA ACCION
                    // b) UTILIZAMOS UN if CON NEGACION ! PERO DIRECTAMENTE CONTRA EL OBJETO 
                    if(!(lector["UrlImagen"] is DBNull))// CONSULTA: si no es null el objeto UrlImagen
                        aux.UrlImagen = (string)lector["UrlImagen"];// condicional SI - EJECUTA ESTA ACCION

                    aux.Tipo = new Elemento(); // CREAMOS LA VARIABLE aux.Tipo DEL OBJETO ELEMENTO
                    aux.Tipo.Id = (int)lector["idTipo"]; // SEÑALAMOS QUE ES LA COLUMNA IdTipo (PARA modificar EL pokemon)
                    aux.Tipo.Descripcion = (string)lector["Tipo"];// SEÑALAMOS QUE ES LA COLUMNA Descripcion DE LA TABLA ELEMENTO
                    aux.Debilidad = new Elemento(); // CREAMOS LA VARIABLE aux.Debilidad DEL OBJETO ELEMENTO
                    aux.Debilidad.Id = (int)lector["idDebilidad"];// SEÑALAMOS QUE ES LA COMUMNA IdDebilidad (PARA modificar EL pokemon)
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
        // PARA AGREGAR UN POKEMON NUEVO EN BD SQL, SE TIENE QUE HACER MEDIANTE UNA FUNCION
        public void agregar(Pokemon nuevo)
        {
            AccesoDatos datos = new AccesoDatos(); // SE GENERA LA VARIABLE PARA EL NUEVO POKEMON

            try
            {
            // SE REALIZA LA CONSULTA A SQL MEDIANTE SU FUNCION setearConsulta, Y SOLO SE HARIA ESTO
            // PORQUE NO VA A DEVOLVER DATOS, YA QUE ES SOLO DE INSERTAR DATOS
                datos.setearConsulta ("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen)values(" + nuevo.Numero + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', 1, @idTipo, @idDebilidad, @idUrlImagen)");
                // LOS values PUEDEN SER POR DOS VIAS:
                // a) CONCATENADOS (Ejm:  '" + nuevo.Nombre + "'  )
                // b) PARAMETROS CON EL SIGNO @ (Ejm: @idTipo )
                datos.setearParametro("@idTipo", nuevo.Tipo.Id);// SE LLAMA A LA FUNCION setearParametro PARA VALIDAR
                datos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);// SE LLAMA A LA FUNCION setearParametro PARA VALIDAR
                datos.setearParametro("@idUrlImagen", nuevo.UrlImagen);

                // Y SE EJECUTA EL METODO ejecutarAccion PARA INSERTAR LOS NUEVOS DATOS A LA BD SQL, YA QUE 
                // EL METODO ejecutarLectura NO VALE,
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally // SE CIERRA LA CONEXION
            {
                datos.cerrarConexion();// COMO NO ES UNA CONSULTA DE LECTURA, HAY QUE CERRARLA
            }
        }
        public void modificar(Pokemon poke) // AL IGUAL QUE EN EL METODO agregar, PASAMOS POR
            // PARAMETRO UNA NUEVA VARIABLE poke PARA SER CARGADA CON LOS NUEVOS DATOS (SI
            // HAY ALGUNA MODIFICACION SINO SE VOLVERAN A SOBREESCRIBIR EN LA BD LOS DATOS 
            // ANTIGUOS QUE SON IGUALES QUE LOS NUEVOS)
        {
            AccesoDatos datos = new AccesoDatos();// INICIAMOS CONEXION
            try
            {   // REALIZAMOS LA CONSULTA A LA BD Y CARGAMOS LOS PARAMETROS NUEVOS EN SUS
                // CORRESPONDIENTES COLUMNAS
                datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @desc, UrlImagen = @img, IdTipo = @idTipo, IdDebilidad = @idDebilidad where Id = @id");
                datos.setearParametro("@numero", poke.Numero);
                datos.setearParametro("@nombre", poke.Nombre);
                datos.setearParametro("@desc", poke.Descripcion);
                datos.setearParametro("@img", poke.UrlImagen);
                datos.setearParametro("@idTipo", poke.Tipo.Id);
                datos.setearParametro("@idDebilidad", poke.Debilidad.Id);
                datos.setearParametro("@id", poke.Id);
                // EJECUTAMOS LA ACCION DE MODIFICAR LOS DATOS EN LA BD
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {// COMO SIEMPRE CERRAMOS LA CONEXION CON LA BD
                datos.cerrarConexion();
            }
        }
        public void eliminar(int id)
        {// ESTE METODO DE eliminar SIRVE PARA ELIMINAR FISICAMENTE DE LA BD LOS DATOS
         // PARA SIEMPRE DEL POKEMON CON id ELEGIDO Y PASADO POR PARAMETRO
            try
            {
                AccesoDatos datos = new AccesoDatos();//ACCEDEMOS A LA BD
                datos.setearConsulta("delete from POKEMONS where id = @id");//SELECCIONAMOS LA CONSULTA
                datos.setearParametro("@id", id);//SELECCIONAMOS POR PARAMETRO EL id QUE VAMOS A ELIMINAR
                datos.ejecutarAccion();//EJECUTAMOS LA ACCION DE ELIMINAR
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void eliminarLogico(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("update POKEMONS set Activo = 0 Where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
