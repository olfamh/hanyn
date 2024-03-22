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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "Data Source=OLFAMHEDHBI\\SQLEXPRESS;Initial Catalog=Garage;Integrated Security=True;Encrypt=False";

                // Créer une SqlConnection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Ouvrir la connexion
                    connection.Open();

                    // Vérifier si textBox1.Text est une valeur numérique
                    if (int.TryParse(textBox1.Text, out int idValue))
                    {
                        // Créer une SqlCommand avec des paramètres
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Employes (Id, Nom, Prenom, Poste, Salaire) VALUES (@Id, @Nom, @Prenom, @Poste, @Salaire)", connection))
                        {
                            // Ajouter des valeurs aux paramètres
                            cmd.Parameters.AddWithValue("@Id", idValue);
                            cmd.Parameters.AddWithValue("@Nom", textBox2.Text);
                            cmd.Parameters.AddWithValue("@Prenom", textBox3.Text);
                            cmd.Parameters.AddWithValue("@Poste", textBox5.Text);
                            cmd.Parameters.AddWithValue("@Salaire", textBox4.Text);


                            // Exécuter la commande
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Données ajoutées avec succès!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("La valeur de l'ID n'est pas une valeur numérique valide.");
                    }
                    RefreshDataGridView();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void buttondelete_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Récupérer la première ligne sélectionnée (si plusieurs lignes sont sélectionnées, vous pouvez ajuster cela selon vos besoins)
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                try
                {
                    string connectionString = "Data Source=OLFAMHEDHBI\\SQLEXPRESS;Initial Catalog=Garage;Integrated Security=True;Encrypt=False";

                    // Créer une SqlConnection
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Ouvrir la connexion
                        connection.Open();

                        // Récupérer la valeur de l'ID de la ligne sélectionnée
                        int idToDelete = Convert.ToInt32(selectedRow.Cells["ColId"].Value);

                        // Créer une SqlCommand pour supprimer la ligne correspondante dans la base de données
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Employes WHERE Id = @Id", connection))
                        {
                            // Ajouter la valeur de l'ID en tant que paramètre
                            cmd.Parameters.AddWithValue("@Id", idToDelete);

                            // Exécuter la commande
                            cmd.ExecuteNonQuery();

                            // Supprimer la ligne du DataGridView
                            dataGridView1.Rows.Remove(selectedRow);

                            MessageBox.Show("Ligne supprimée avec succès!");
                            RefreshDataGridView();


                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une ligne à supprimer.");
            }

        }
        private void RefreshDataGridView()
        {
            string connectionString = "Data Source=OLFAMHEDHBI\\SQLEXPRESS;Initial Catalog=Garage;Integrated Security=True;Encrypt=False";

            // Vider le DataGridView
            dataGridView1.Rows.Clear();

            // Charger à nouveau les données depuis la base de données
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Employes", connection))
                {
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(dr["Id"], dr["Nom"], dr["Prenom"], dr["Poste"], dr["Salaire"]);
                    }

                    dr.Close();
                }
            }
        }

        private void buttonmodifier_Click(object sender, EventArgs e)
        {

            // Vérifier si une ligne est sélectionnée dans le dataGridView1
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Récupérer la ligne sélectionnée
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Récupérer les valeurs des cellules dans la ligne sélectionnée
                string id = selectedRow.Cells["ColId"].Value.ToString();
                string nom = selectedRow.Cells["ColNom"].Value.ToString();
                string prenom = selectedRow.Cells["ColPrenom"].Value.ToString();
                string poste = selectedRow.Cells["ColPoste"].Value.ToString();
                if (selectedRow.Cells["ColSalaire"].Value != null)
                {
                    decimal salaire;
                    if (decimal.TryParse(selectedRow.Cells["ColSalaire"].Value.ToString(), out salaire))
                    {
                        textBox4.Text = salaire.ToString();
                    }
                    else
                    {
                        // Gestion d'une conversion non réussie
                        MessageBox.Show("La valeur du salaire n'est pas un nombre valide.");
                    }
                }

                // Afficher les valeurs dans des contrôles d'entrée
                textBox1.Text = id;
                textBox2.Text = nom;
                textBox3.Text = prenom;
                textBox5.Text = poste;
                // textBox4.Text = salaire.ToString();


                // Activer les contrôles d'entrée pour permettre la modification
                textBox1.Enabled = false; // Si l'ID ne peut pas être modifié
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                // Changer le texte du bouton "Enregistrer" pour refléter l'opération de modification
                //  buttonmodifier.Text = "Enregistrer les modifications";
                Enregistrer.Visible = true;

            }
        }

        private void Enregistrer_Click(object sender, EventArgs e)
        {


            // Vérifier si l'opération actuelle est une modification

            // Récupérer les valeurs modifiées des contrôles d'entrée
            //  buttonmodifier.Visible = false;

            string nom = textBox2.Text;
            string prenom = textBox3.Text;
            string poste = textBox5.Text;
            decimal salaire;

            // Vérifier si la valeur du salaire est valide
            if (decimal.TryParse(textBox4.Text, out salaire))
            {
                // Mettre à jour la ligne dans la base de données
                using (SqlConnection connection = new SqlConnection("Data Source=OLFAMHEDHBI\\SQLEXPRESS;Initial Catalog=Garage;Integrated Security=True;Encrypt=False"))
                {
                    connection.Open();

                    string query = "UPDATE Employes SET Nom = @Nom, Prenom = @Prenom, Poste = @Poste, Salaire = @Salaire WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nom", nom);
                        command.Parameters.AddWithValue("@Prenom", prenom);
                        command.Parameters.AddWithValue("@Poste", poste);
                        command.Parameters.AddWithValue("@Salaire", salaire);
                        command.Parameters.AddWithValue("@Id", textBox1.Text);

                        // Exécuter la commande SQL
                        command.ExecuteNonQuery();
                    }
                }

                // Actualiser le DataGridView
                EffectuerRecherche();

                // Réinitialiser les contrôles d'entrée
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox1.Enabled = true;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                buttonmodifier.Text = "Modifier";
            }
            else
            {
                // Gestion d'une valeur de salaire non valide
                MessageBox.Show("La valeur du salaire n'est pas un nombre valide.");
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            try
            {
                Enregistrer.Visible = false;

                // Remplacer "connectionstring" par votre chaîne de connexion
                string connectionString = "Data Source=OLFAMHEDHBI\\SQLEXPRESS;Initial Catalog=Garage;Integrated Security=True;Encrypt=False";

                // Créer une SqlConnection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Ouvrir la connexion
                    connection.Open();

                    // Créer une SqlCommand
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Employes", connection);

                    // Créer un SqlDataReader
                    SqlDataReader dr = cmd.ExecuteReader();

                    // Si le DataGridView a déjà des colonnes, effacez-les
                    dataGridView1.Columns.Clear();

                    // Ajouter des colonnes à la DataGridView si nécessaire
                    if (dataGridView1.Columns.Count == 0)
                    {
                        dataGridView1.Columns.Add("ColId", "Id");
                        dataGridView1.Columns.Add("ColNom", "Nom");
                        dataGridView1.Columns.Add("ColPrenom", "Prenom");
                        dataGridView1.Columns.Add("ColPoste", "Poste");
                        dataGridView1.Columns.Add("ColSalaire", "Salaire");
                    }

                    while (dr.Read())
                    {
                        // Ajouter des lignes à la DataGridView
                        dataGridView1.Rows.Add(dr["Id"], dr["Nom"], dr["Prenom"], dr["Poste"], dr["Salaire"]);

                        // Ajouter des éléments au ComboBox
                        //comboBox1.Items.Add(dr["Id"]);
                    }

                    // Fermer le DataReader
                    dr.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }
        private void EffectuerRecherche()
        {
            // Obtenez le texte de recherche depuis textBox6
            string texteRecherche = textBox6.Text;

            // Connexion à la base de données SQL Server
            using (SqlConnection connection = new SqlConnection("Data Source=OLFAMHEDHBI\\SQLEXPRESS;Initial Catalog=Garage;Integrated Security=True;Encrypt=False"))
            {
                connection.Open();

                // Créer une commande SQL en fonction du texte de recherche
                string query = "SELECT * FROM Employes WHERE Nom LIKE @Nom";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nom", "%" + texteRecherche + "%");

                    // Exécuter la commande SQL
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Vider dataGridView
                        dataGridView1.Rows.Clear();

                        // Ajouter les résultats de la recherche dans dataGridView Employes
                        while (reader.Read())
                        {
                            // Ajoutez la ligne dans le dataGridView
                            dataGridView1.Rows.Add(reader["Id"], reader["Nom"], reader["Prenom"], reader["Poste"], reader["Salaire"]);
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "Nom":
                    textBox6.Visible = true;
                    break;


            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            EffectuerRecherche();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();

        }
    }
    }

