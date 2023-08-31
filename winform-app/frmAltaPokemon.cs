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

namespace winform_app
{
    public partial class frmAltaPokemon : Form
    {
        public frmAltaPokemon()
        {
            InitializeComponent();
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
            Pokemon poke = new Pokemon();
            // TAMBIEN SE CREA  UNA VARIABLE negocio DEL TIPO DATO POKEMONNEGOCIO
            // PARA MANDAR LOS DATOS A LA DB DE SQL
            PokemonNegocio negocio = new PokemonNegocio();

            try
            {
                // SE GUARDAN LOS DATOS EN LA VARIABLE poke
                poke.Numero = int.Parse(txtNumero.Text);
                poke.Nombre = txtNombre.Text;
                poke.Descripcion = txtDescripcion.Text;
                poke.UrlImagen = txtUrlImagen.Text;//SE INCLUYE LA UrlImagen PARA QUE SEA CARGADA CON EL pbxPokemon
                poke.Tipo = (Elemento)cboTipo.SelectedItem;//SE CARGA UN OBJETO DEL ELEMENTO SELECCIONADO
                poke.Debilidad = (Elemento)cboDebilidad.SelectedItem;//SE CARGA UN OBJETO DEL ELEMENTO SELECCIONADO
                // SE MANDAN LOS DATOS A DB POR MEDIO DE LA FUNCION AGREGAR QUE HAY EN 
                // POKEMONNEGOCIO
                negocio.agregar(poke);
                // Y SI HA IDO TODO BIEN, LANZAMOS ESTE MENSAJE DE CONFIRMACION
                MessageBox.Show("Correctamente Agregado");
                // SE CERRARIA EL FORMULARIO Y VOLVERIA A LA VENTANA PRINCIPAL
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
                cboDebilidad.DataSource = elementoNegocio.listar();
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
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxPokemon.Load("https://enteracloud.mx/wp-content/uploads/2021/08/placeholder.png");
            }
        }
    }
}
