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
    }
}
