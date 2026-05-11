using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;


namespace ClaseABD
{
	
	public partial class UsuarioNuevo : Form
	{
		//Paso1: Crear una cadena de conexión
		private string cadenaConexion =  "Server=localhost;Database=peducativa;Uid=root;Pwd=;";
		MySqlConnection con;
		
		public UsuarioNuevo()
		{
			
			InitializeComponent();
			CargarRoles();
			con = new MySqlConnection(cadenaConexion);
			CargarUsuarios();
			
		}
		
		
		void CargarRoles()
		{
			
			try {
				using (MySqlConnection conexion = new MySqlConnection(cadenaConexion)) {
					conexion.Open();
					string consulta = "SELECT id, nombre FROM rol";
                    MySqlDataAdapter da = new MySqlDataAdapter(consulta, conexion);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);

                    cmbRol.DataSource = dt;
                    cmbRol.DisplayMember = "nombre"; 
                    cmbRol.ValueMember = "id";       
				}
			} catch (Exception ex) {
				MessageBox.Show("Error al cargar roles: " + ex.Message);
			}
		}
		void CargarUsuarios() {
			try {
				string consulta = "SELECT * FROM usuarios";
                MySqlDataAdapter da = new MySqlDataAdapter(consulta, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt; 
			} catch (Exception ex) {
        MessageBox.Show("Error al cargar cuadro: " + ex.Message);
			}
		}
		
	
		
		void BtnAgregarClick(object sender, EventArgs e)
		{
			try {
				
				if (con.State == ConnectionState.Closed) {
					con.Open();
				}
				
				
				string consulta = "INSERT INTO usuarios (nombre, clave, rol) VALUES ('" + txtNombre.Text + "', '" + txtClave.Text + "', '" + cmbRol.Text + "')";
    
                
                MySqlCommand comando = new MySqlCommand(consulta, con);
                comando.ExecuteNonQuery();
    
                con.Close();
                MessageBox.Show("¡Usuario agregado con éxito!");
                CargarUsuarios();
    
                
                txtNombre.Clear();
                txtClave.Clear();
			} catch (Exception ex) {
				MessageBox.Show("Error al guardar: " + ex.Message);
			}
		}
		
		
		void BtnModificarClick(object sender, EventArgs e)
		{
			try {
				if (con.State == ConnectionState.Closed) con.Open();
				
				string consulta = "UPDATE usuarios SET clave='" + txtClave.Text + "', rol='" + cmbRol.Text + "' WHERE nombre='" + txtNombre.Text + "'";
				
				MySqlCommand comando = new MySqlCommand(consulta, con);
                int filasAfectadas = comando.ExecuteNonQuery();
                
                if (filasAfectadas > 0) {
                	MessageBox.Show("¡Usuario modificado con éxito!");
                	CargarUsuarios(); 
                } else{
                	MessageBox.Show("No se encontró el usuario para modificar.");
                }
                
                con.Close();
			} catch (Exception ex) {
				MessageBox.Show("Error al modificar: " + ex.Message);
			}
		}
		
		
		void BtnEliminarClick(object sender, EventArgs e)
		{
			try {
				
				DialogResult confirmar = MessageBox.Show("¿Seguro que quieres eliminar a este usuario?", "Confirmar", MessageBoxButtons.YesNo);
				
				if (confirmar == DialogResult.Yes) {
					if (con.State == ConnectionState.Closed) con.Open();
					
					string consulta = "DELETE FROM usuarios WHERE nombre='" + txtNombre.Text + "'";
					
					MySqlCommand comando = new MySqlCommand(consulta, con);
                    comando.ExecuteNonQuery();
        
                    MessageBox.Show("Usuario eliminado.");
                    CargarUsuarios(); 
                    
                    
                    txtNombre.Clear();
                    txtClave.Clear();
                    
                    con.Close();
				
				}
			} catch (Exception ex) {
				MessageBox.Show("Error al eliminar: " + ex.Message);
			}
		}
	}
}
