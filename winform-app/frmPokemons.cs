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

            // UTILIZAMOS LA FUNCION cargar, PARA UTILIZARLA SIEMPRE QUE CARGUE EL FORMULARIO
            cargar();
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPokemons.CurrentRow != null)
            {//PARA QUITAR LA EXCEPCION DEL FILTRO, HACEMOS UN if PARA VALIDAR SI LA dgvPokemons
            //ESTA VACIA, NO SE ROMPA Y CONTINUE CON EL FILTRO
               
                // PARA CAMBIAR LA IMAGEN SEGUN CAMBIAS DE FILA DE LA TABLA DE POKEMON
                Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem; // CREAMOS UNA VARIABLE,
                // PARA GUARDAR  (DEL TIPO DATO POKEMON) DE LA LISTA POKEMON, LA FILA ACTUAL SELECCIONADA,
                // DAME EL OBJETO/ITEM ENLAZADO (URL)

                // PARA TERMINAR, CARGAMOS EN EL PBXPOKEMON LA IMAGEN SELECCIONADA.
                // pbxPokemon.Load(seleccionado.UrlImagen);// AQUI SE TERMINARIA.
                cargarImagen(seleccionado.UrlImagen);// SE USA EL METODO cargarImagen

            }
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
        // EL CODIGO QUE HAY DENTRO DEL METODO cargar, ES EL QUE ESTABA EN frmPokemon_Load.
        // PARA VOLVER A CARGAR EL frmPokemon SIN TENER QUE SALIR Y VOLVER A ENTRAR DE LA
        // APLICACION, SE CREA EL METODO cargar, EL CUAL SE REEMPLAZARA E INCLUIRA 
        // EN EL METODO DE frmPokemon_load (DEL INICIO DE ESTA CLASE)

        private void cargar()
        {
            // PARA INCLUIR LA IMAGEN, TENEMOS QUE GUARDAR LA TABLA VIRTUAL EN LA listaPokemon
            PokemonNegocio negocio = new PokemonNegocio();

            // PARA QUE NO SALGA EXCEPCION POR null, HAY QUE VALIDAR
            // ENTONCES VAMOS A VALIR LA COLUMNA UrlImagen CON EL TRY

            try
            {
                listaPokemon = negocio.listar(); // CARGAMOS LA LISTA
                dgvPokemons.DataSource = listaPokemon; // LA MOSTRAMOS EN DATAGRIDVIEW CON LA URL DE LA IMAGEN
                ocultarColumnas();//USAMOS EL METODO DE OCULTAR LAS COLUMNAS QUE NO NECESITO
                // AL HACER EL METODO cargarImagen, SALVAMOS POSIBLES PROBLEMAS CON LAS IMAGENES
                cargarImagen(listaPokemon[0].UrlImagen);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ocultarColumnas()
        {//METODO PARA OCLULTAR LAS COLUMNAS DEL DGV QUE NO QUIERO MOSTRAR O NO NECESITO
            dgvPokemons.Columns["UrlImagen"].Visible = false; // OCULTAMOS LA COLUMNA DE LA URL PORQUE NO ES NECESARIA
            // pbxPokemon.Load(listaPokemon[0].UrlImagen); // CUANDO SE CARGA EL PBXPOKEMON, SE CARGA CON LA
            dgvPokemons.Columns["Id"].Visible = false; // OCULTAMOS TAMBIEN LA COLUMNA DE Id
            // IMAGEN DEL PRIMEN POKEMON (DEL INDICE 0) DE LA LISTA POKEMON PERO

        }
        // PARA AGREGAR UN NUEVO ALTA DE POKEMON, AGREGAMOS BOTON CON UNA FUNCION DE ABRIR UNA 
        // NUEVA VENTANA frmAltaPokemon
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog();
            cargar();
        }
    // PARA MODIFICAR UN POKEMON, AGREGAMOS BOTON REUTILIZANDO LA FUNCION DE ABRIR LA NUEVA VENTANA
    // frmAltaPokemon PERO VARIANDO PARA QUE EN LUGAR DE ESTAR VACIA, SE LLENEN LOS CAMPOS
    // CON LOS DATOS TRAIDOS DE SQL PASADOS POR PARAMETRO DEL OBJETO Pokemon
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;//SELECCIONAMOS
            //EL Pokemon QUE QUEREMOS MODIFICAR - EL QUE ESTA SELECCIONADO EN ESE MOMENTO EN EL dgvPokemons
            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado);//PASAMOS POR PARAMETROS
            // EL Pokemon SELECCIONADO MEDIANTE EL CONSTRUCTOR SOBRECARGADO
            modificar.ShowDialog();
            cargar();
        }
        //PARA ELIMINAR FISICAMENTE Y DEFINITAVAMENTE UN POKEMON DE LA BD, USAMOS EL METODO
        // CUANDO HEMOS SELECCIONADO EL POKEMON Y DAMOS AL BOTON btnEliminarFisica
        private void btnEliminarFisica_Click(object sender, EventArgs e)
            //LO COMENTAMOS TODO PORQUE INCLUIMOS ESTA FUNCION DENTRO DEL METODO eliminar

        {
            //PokemonNegocio negocio = new PokemonNegocio();
            //Pokemon seleccionado;//CREAMOS LA VARIABLE seleccionado PARA IDENTIFICAR EL id DE
            // LA BD QUE QUEREMOS ELIMINAR
            //try
            //{               //UTILIZAMOS EL DialogResult PARA GUARDAR EL RESULTADO A LA PREGUNTA DE ELIMINAR
            //DialogResult respuesta = MessageBox.Show("¿Estas seguro de ELIMINAR el Pokemon?", "Eliminando....", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //if (respuesta == DialogResult.Yes)//CONDICIONAMOS LA CONTESTACION A Yes/No
            //{          //PARA ELIMINAR EL POKEMON SELECCIONADO TOTALMENTE DE LA BD
            //seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;//EL id DEL POKEMON SELECCIONADO
            //negocio.eliminar(seleccionado.Id);//PASAMOS POR PARAMETRO EL id AL METODO ELIMINAR Y LO ELIMINA
            //cargar();
            //}
            //}
            //catch (Exception ex)
            //{
            //MessageBox.Show(ex.ToString());
            //}
            eliminar();
        }


        //PARA ELIMINAR LOGICAMENTE PERO NO DEFINITAVAMENTE, ES DECIR, INACTIVAR UN POKEMON DE LA BD
        // EN LA COLUMNA ACTIVO CON VALOR A CERO, USAMOS ESTE METODO CUANDO HEMOS SELECCIONADO
        // EL POKEMON Y DAMOS AL BOTON btnEliminarLogico
        private void btnEliminarLogico_Click(object sender, EventArgs e)
            //LO COMENTAMOS TODO PORQUE INCLUIMOS ESTA FUNCION DENTRO DEL METODO eliminar
        {
            //PokemonNegocio negocio = new PokemonNegocio();
            //Pokemon seleccionado;//CREAMOS LA VARIABLE seleccionado PARA IDENTIFICAR EL id DE
                                    // LA BD QUE QUEREMOS INACTIVAR
            //try
            //{   //UTILIZAMOS EL DialogResult PARA GUARDAR EL RESULTADO A LA PREGUNTA DE ELIMINAR
                //DialogResult respuesta = MessageBox.Show("¿Estas seguro de ELIMINAR el Pokemon?", "Eliminando....", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //if (respuesta == DialogResult.Yes)//CONDICIONAMOS LA CONTESTACION A Yes/No
                //{                       //PARA ELIMINAR EL POKEMON SELECCIONADO TOTALMENTE DE LA BD
                    //seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;//EL id DEL POKEMON SELECCIONADO
                    //negocio.eliminar(seleccionado.Id);//PASAMOS POR PARAMETRO EL id AL METODO ELIMINAR Y LO ELIMINA
                //cargar();
                //}
            //}
            //catch (Exception ex)
            //{
                //MessageBox.Show(ex.ToString());
            //}
            eliminar(true);
        }
        // CREAMOS EL METODO ELIMINAR PARA INCLUIR LAS FUNCIONES EliminarFisica Y EliminarLogico
        private void eliminar (bool logico = false)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;//CREAMOS LA VARIABLE seleccionado PARA IDENTIFICAR EL id DE
            // LA BD QUE QUEREMOS ELIMINAR o INACTIVAR
            try
            {//UTILIZAMOS EL DialogResult PARA GUARDAR EL RESULTADO A LA PREGUNTA DE ELIMINAR
                DialogResult respuesta = MessageBox.Show("¿Estas seguro de ELIMINAR el Pokemon?", "Eliminando....", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)//CONDICIONAMOS LA CONTESTACION A Yes/No
                { 
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;//EL id DEL POKEMON SELECCIONADO

                    if (logico) // PARA INACTIVAR EL POKEMON SELECCIONADO EN LA COLUMNA ACTIVO DE LA BD
                    {
                        negocio.eliminarLogico(seleccionado.Id);//PASAMOS POR PARAMETRO EL id AL METODO ELIMINAR Y LO INACTIVA
                    }
                    else //PARA ELIMINAR EL POKEMON SELECCIONADO TOTALMENTE DE LA BD
                    {
                        negocio.eliminar(seleccionado.Id);//PASAMOS POR PARAMETRO EL id AL METODO ELIMINAR Y LO ELIMINA
                    }
                    
                    cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> listaFiltrada;// CREAMOS UNA NUEVA LISTA listaFiltrada CARGADA CON 
            //TODOS LOS POKEMOS DE LA ListPokemon (ES UN ATRIBUTO ESTA LISTA)
            string filtro = txtFiltro.Text;//CREAMOS VARIABLE PARA ALOJAR LO QUE SE ESCRIBA
            //EN EL CAMPO DEL FILTRO

            //PARA VALIDAR EL FILTRO Y VOLVER AL PRINCIPIO SIN FILTROS CON LA LISTA ORIGINAL
            //USAMOS EL if DE DOS MANERAS:
            //if (filtro != "") //SOBRE EL CAMPO DEL FILTRO SI ESTA VACIO O NO
            if (filtro.Length >= 3)//SOBRE LA LONGITUD DEL CAMPO, SEA MAYOR A UNA CANTIDAD DE CARACTERES
            {// HAY VARIOS FILTROS, PERO LOS MAS COMUNES SON:
             //-ESTE FILTRO ES PARA CUANDO SE QUIERE BUSCAR ALGO EXPRESO EN UNA COLUMNA:
             //listaFiltrada = listaPokemon.FindAll(x => x.Nombre == filtro);// ASIGNAMOS
             //EL METODO FindAll (BUSCAR TODO LO QUE HAYA EN LA LISTA) PERO USAMOS EXPRESION LANDA
             //PORQUE HAY QUE PASAR UN PARAMETRO. LA EXPRESION LANDA BUSCA LA COINCIDENCIA == DEL
             //TEXTO EXCRITO EN EL txtFiltro CON LO QUE CONTIENE EN LA COLUMNA Nombre --- OJO!!!!
             //LA COINCIDENCIA ES EXACTA, POR LO QUE TIENES QUE ESCRIBIR TODO EL NOMBRE Y EN EL 
             //MISMO FORMATO DE MAYUSCULAS Y MINUSCULAS
             //-ESTE FILTRO ES PARA CUANDO SE QUIERE BUSCAR COINCIDENCIAS EN LAS COLUMNAS
             //SIN TENER EN CUENTA MAYUSCULAS Y MINUSCULAS (SE PUEDE USAR ToUpper o ToLower):
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper() == filtro.ToUpper());
                //-ESTE FILTRO SE USA PARA QUE LO QUE SE ESTA BUSCANDO TENGA COINCIDENCIA EN
                //UNA O VARIAS COLUMNAS (UNA CANDENA CONTIENE DENTRO DE OTRA CADENA):
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));

            }
            else
            {
                listaFiltrada = listaPokemon;//SI EL CAMPO ESTA VACIO, CARGA NUEVAMENTE LA LISTA ORIGINAL
            }


            //AHORA ACTUALIZAMOS EL LISTADO CON EL FILTRO
            dgvPokemons.DataSource = null;// SIEMPRE HAY QUE INICIANIZARLO
            dgvPokemons.DataSource = listaFiltrada;//MUESTRA YA LA LISTA FILTRADA
            ocultarColumnas();
        }
        private void btnFiltro_Click(object sender, EventArgs e)
        {

        }
    }
}
