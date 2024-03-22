using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string connString = ("Data Source=OLFAMHEDHBI\\SQLEXPRESS;Initial Catalog=Garage;Integrated Security=True;Encrypt=False");

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {
                MessageBox.Show("connection aux serveur seccesfully0");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "Data Source=OLFAMHEDHBI\\SQLEXPRESS;Initial Catalog=Garage;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Utilisateurs WHERE NomUtilisateur = @NomUtilisateur AND MotDePasse = @MotDePasse";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NomUtilisateur", textBox1.Text);
                        command.Parameters.AddWithValue("@MotDePasse", textBox2.Text);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            // Les informations d'identification sont correctes
                            Form3 form3 = new Form3();
                            form3.Show();

                            MessageBox.Show("Correct");
                        }
                        else
                        {
                            MessageBox.Show("User and password are incorrect");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
