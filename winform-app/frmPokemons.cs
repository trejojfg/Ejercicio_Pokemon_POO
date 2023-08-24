using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;  // INCLUIMOS EL using DE dominio PARA PODER ENLAZARLO
using negocio;   // INCLUIMOS EL using DE negocio PARA PODER ENLAZARLO

namespace winform_app
{
    public partial class frmPokemons : Form
    {
        private List<Pokemon> listaPokemon; // AÑADIMOS ESTE ATRIBUTO DE TIPO LISTA
                                            // PARA TENER UNA LISTA FIJA
        public frmPokemons()
        {
            InitializeComponent();
        }

        private void frmPokemons_Load(object sender, EventArgs e)
        {
            // DE ESTA MANERA SOLO CARGAMOS LOS DATOS EN LA TABLA VIRTUAL
            // PokemonNegocio negocio = new PokemonNegocio();
            // dgvPokemons.DataSource = negocio.listar();

            // PARA INCLUIR LA IMAGEN, TENEMOS QUE GUARDAR LA TABLA VIRTUAL EN LA listaPokemon
            PokemonNegocio negocio = new PokemonNegocio();
            listaPokemon = negocio.listar(); // CARGAMOS LA LISTA
            dgvPokemons.DataSource = negocio.listar(); // LA MOSTRAMOS EN DATAGRIDVIEW CON LA URL DE LA IMAGEN
            dgvPokemons.Columns["UrlImagen"].Visible = false; // OCULTAMOS LA COLUMNA DE LA URL PORQUE NO ES NECESARIA
            // pbxPokemon.Load(listaPokemon[0].UrlImagen); // CUANDO SE CARGA EL PBXPOKEMON, SE CARGA CON LA
            // IMAGEN DEL PRIMEN POKEMON (DEL INDICE 0) DE LA LISTA POKEMON
                                         // PERO
            // AL HACER EL METODO cargarImagen, SALVAMOS POSIBLES PROBLEMAS CON LAS IMAGENES
            cargarImagen(listaPokemon[0].UrlImagen);

        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            // PARA CAMBIAR LA IMAGEN SEGUN CAMBIAS DE FILA DE LA TABLA DE POKEMON
            Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem; // CREAMOS UNA VARIABLE,
            // PARA GUARDAR  (DEL TIPO DATO POKEMON) DE LA LISTA POKEMON, LA FILA ACTUAL SELECCIONADA,
            // DAME EL OBJETO/ITEM ENLAZADO (URL)

            // PARA TERMINAR, CARGAMOS EN EL PBXPOKEMON LA IMAGEN SELECCIONADA.
            // pbxPokemon.Load(seleccionado.UrlImagen);// AQUI SE TERMINARIA.
            cargarImagen(seleccionado.UrlImagen);// SE USA EL METODO cargarImagen

        }
                                    // PERO
            // PARA QUE NO HAYA ERROR AL CARGAR LA IMAGEN (YA NO EXITE LA IMAGEN, LA URL DA FALLO,...)
            // SE GENERA EL METODO cargarImagen CON EXCEPCION, POR SI SUCEDE, QUE PONGA UNA IMAGEN GENERICA.

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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog();
        }
    }
}
