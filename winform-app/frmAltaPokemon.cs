using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio; // INCLUIMOS EL using dominio PARA PODER UTILIZAR EL OBJETO POKEMON
using negocio; // INCLUIMOS EL using negocio PARA PODER UTILIZAR EL OBJETO POKEMONNEGOCIO
using System.Configuration;// AÑADIMOS ESTE using PARA PODER GUARDAR LA IMAGEN EN LA BD MEDIANTE REFERENCIAS
using System.IO;// SE CREA LA CLASE File, QUE ES ESTATICA, PARA PODER GUARDAR LA IMAGEN EN LA BD

namespace winform_app
{
    public partial class frmAltaPokemon : Form
    {
        // CREAMOS UN ATRIBUTO PRIVADO DEL OBJETO Pokemon, PARA PODER UTILIZARLO EN EL FORMULARIO
        // A LA HORA DE DISTIRNGUIR ENTRE agregar (VALOR A null) Y modificar (!= null YA QUE NOS
        // TRAEMOS LOS VALORES DE LA BD).
        private Pokemon pokemon = null;
        // CREAMOS UN ATRIBUTO PRIVADO DEL OBJETO OpenFileDialog, PARA PODER GUARDAR LA IMAGEN EN LOCAL
        private OpenFileDialog archivo = null;
        public frmAltaPokemon()
        {
            InitializeComponent();
        }
        // PARA PODER MODIFICAR UN POKEMON, HAY QUE SOBRECARGAR EL CONSTRUCTOR Y PODER ASI
        // MANDAR POR PARAMETROS LOS VALORES DEL POKEMON
        public frmAltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;// PARA DECIR QUE ES EL pokemon QUE TRAEMOS DE LA BD POR PARAMETRO
            Text = "Modificar Pokemon"; // SE CAMBIA EL NOMBRE DE LA frmAltaPokemon CUANDO SEA modificar
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close(); // NO HACE FALTA PONER this, PORQUE HACE REFERENCIA A LA VENTANA EN LA QUE ESTAS
        }

        // PARA AGREGAR UN NUEVO POKEMON, HAY QUE CAPTURA LOS DATOS QUE SE INTRODUCEN EN 
        // LOS TEXTBOX DEL FORMULARIO frmAltaPokemon
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // SE GENERA UNA VARIABLE poke DEL TIPO DATO POKEMON, PARA ALMACENAR
            // LOS DATOS
            // Pokemon poke = new Pokemon(); // ELIMINAMOS LA VARIABLE PORQUE SE CREO pokemon PARA modificar
            // TAMBIEN SE CREA  UNA VARIABLE negocio DEL TIPO DATO POKEMONNEGOCIO
            // PARA MANDAR LOS DATOS A LA DB DE SQL
            PokemonNegocio negocio = new PokemonNegocio();

            try
            {
                // SE GUARDAN LOS DATOS EN LA VARIABLE poke // LUEGO CAMBIAMOS poke POR pokemon PARA DIFERENCIAR SI 
                // QUEREMOS agregar O modificar EN EL FORMULARIO frmAltaPokemon

                if (pokemon == null) // SE DECLARÓ LA VARIABLE pokemon COMO null
                    // SE DA POR ENTENDIDO QUE SI ESTAMOS AQUI, ES PORQUE SE LE DIO A agregar
                {
                    pokemon = new Pokemon(); // CARGAMOS LA VARIABLE CON EL POKEMON NUEVO
                }
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                pokemon.UrlImagen = txtUrlImagen.Text;//SE INCLUYE LA UrlImagen PARA QUE SEA CARGADA CON EL pbxPokemon
                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem;//SE CARGA UN OBJETO DEL ELEMENTO SELECCIONADO
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem;//SE CARGA UN OBJETO DEL ELEMENTO SELECCIONADO
                // SE MANDAN LOS DATOS A DB POR MEDIO DE LA FUNCION AGREGAR QUE HAY EN 
                // POKEMONNEGOCIO
                if (pokemon.Id != 0)
                {
                    negocio.modificar(pokemon);
                    // Y SI HA IDO TODO BIEN, LANZAMOS ESTE MENSAJE DE CONFIRMACION
                    MessageBox.Show("Modificado Exitosamente");
                    // SE CERRARIA EL FORMULARIO Y VOLVERIA A LA VENTANA PRINCIPAL
                }
                else
                {
                    negocio.agregar(pokemon);
                    // Y SI HA IDO TODO BIEN, LANZAMOS ESTE MENSAJE DE CONFIRMACION
                    MessageBox.Show("Correctamente Agregado");
                    // IGUAL QUE agregar PERO CON modificar
                }
                // GUARDO LA IMAGEN SI LA LEVANTO LOCALMENTE Y CONDICIONO QUE TENGA HTTP PARA GUARDAR
                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {// TRAEMOS DE LA FUNCION btnAgregarImagen_Click PARA GUARDAR LA IMAGEN
                File.Copy(archivo.FileName, ConfigurationManager.AppSettings["poke-app-img"] + archivo.SafeFileName);
                }
                
                Close();

            }
            catch (Exception ex)
            {
                // EN EL CASO DE QUE HAYA IDO ALGO MAL, NO SE MANDARIAN LOS DATOS A DB NI
                // SE MOSTARIA EL MENSAJE DE "correctamente Agregado" Y MOSTRARIA LA EXCEPCION
                // CON EL ERROR QUE HAYA DADO
                MessageBox.Show(ex.ToString());
            }

        }

        // CARGAMOS LOS COMBOBOX DE LOS ELEMENTOS DESPLEGABLES (tipo y Debilidad) CUANDO 
        // SE CARGUE EL FORMULARIO frmAltaPokemon
        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            // SE GENERA LA VARIABLE elementoNegocio DEL TIPO DATO ElementoNegocio,
            ElementoNegocio elementoNegocio = new ElementoNegocio();

            // VAMOS A UTILIZAR EL METODO listar QUE HAY EN LA CLASE ElementoNegocio PARA
            // TRAER LOS DATOS DE SQL A LOS COMBOBOX

            try
            {
                cboTipo.DataSource = elementoNegocio.listar();
                cboTipo.ValueMember = "Id";// INDICAMOS EL VALOR ESCONDIDO DEL cboTipo DEL OBJETO Elemento
                cboTipo.DisplayMember = "Descripcion"; // INDICAMOS EL VALOR QUE SE MUESTRA DEL cboTipo DEL OBJETO Elemento
                cboDebilidad.DataSource = elementoNegocio.listar();
                cboDebilidad.ValueMember = "Id";// INDICAMOS EL VALOR ESCONDIDO DEL cboDebilidad  DEL OBJETO Elemento
                cboDebilidad.DisplayMember = "Descripcion";// INDICAMOS EL VALOR QUE SE MUESTRA DEL cboDebilidad  DEL OBJETO Elemento

                // HACEMOS ESTA VALIDACION PARA SABER SI EL pokemon ES null (ENTONCES ES agregar) Y
                // SI ES !=null (ENTONCES ES modificar)
                if (pokemon != null)
                {
                    // CARGO LOS DATOS DE LA DB EN LOS CAMPOS CORRESPONDIENTES
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    cargarImagen(pokemon.UrlImagen);
                    cboTipo.SelectedValue = pokemon.Tipo.Id;
                    cboDebilidad.SelectedValue = pokemon.Debilidad.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // PARA CARGAR LA IMAGEN CON pbxPokemon, UTILIZAMOS EL METODO leave DEL txtUrlImagen.
        // USAMOS LA FUNCION cargarImagen DE LA CLASE frmPokemons
        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);// CARGAMOS LA IMAGEN QUE HAY DENTRO DEL txtUrlImagen
        }
        private void cargarImagen(string imagen)
        {// CREAMOS EL METODO cargarImagen PARA CARGAR LA IMAGEN SI LA TENEMOS O TENEMOS LA URL,
            // O SI NO LA TENEMOS, CARGAR UNA IMAGEN STANDAR
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxPokemon.Load("https://enteracloud.mx/wp-content/uploads/2021/08/placeholder.png");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {// AL DAR AL BOTON " + " PARA AGREGAR IMAGEN TENEMOS QUE CREAR EL OBJETO "archivo" QUE
            // PERTENECE AL OBEJTO OpenFileDialog (ABRIR UNA VENTANA DE DIALOGO)
            // OpenFileDialog archivo = new OpenFileDialog();
            
            archivo = new OpenFileDialog();// ARRANCAMOS EL ATRIBUTO OpenFileDialog en NULL

            archivo.Filter = "jpg|*.jpg;|png|*.png";//FILTRAMOS LOS ARCHIVOS QUE QUEREMOS QUE APAREZCAN SEGUN FORMATO
            if(archivo.ShowDialog() == DialogResult.OK)//PARA GUARDAR LA IMAGEN, TENEMOS QUE 
                //DAR AL OK DE LA VENTANA DE DIALOGO
            {
                txtUrlImagen.Text = archivo.FileName;//ASIGNAMOS IMAGEN AL CUADRO DE TEXTO DE LA UrlImagen
                // CON EL ARCHIVO QUE HEMOS SELECCIONADO
                cargarImagen(archivo.FileName);//CARGAMOS LA IMAGEN, CON LA IMAGEN QUE HABIAMOS SELECCIONADO


                //  -PARA LEVANTAR LOCALMENTE LA IMAGEN Y GUARDAR LA RUTA EN LA APP
                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["poke-app-img"] + archivo.SafeFileName);
                    //FORZAMOS EL OBJETO File Y CREAMOS SU Using IO. LUEGO USAMOS EL METODO
                    // ConfigurationManager.AppSetting (INCLUYENDO LA Key DE LA RUTA DE LA IMAGEN.
                //  -PARA LEVANTAR LA IMAGEN Y GUARDAR EL ARCHIVO EN LA DB CON VALIDACION, LO 
                // TENEMOS QUE INCLUIR EN EL METODO cargarImagen


            }

        }
    }
}
