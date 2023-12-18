using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace Projet
{
    /// <summary>
    /// Logique d'interaction pour AddTagWindow.xaml
    /// </summary>
    public partial class AddTagWindow : Window
    {
        public AddTagWindow()
        {
            InitializeComponent();
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Sélectionner le fichier à modifier",
                Filter = "Fichiers texte (*.txt)|*.txt"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                string selectedFilePath = openFileDialog.FileName;
                filePathTextBox.Text = selectedFilePath;
            }
        }


        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            // Obtenez le chemin du fichier à partir de la TextBox
            string filePath = filePathTextBox.Text;

            if (File.Exists(filePath))
            {
                try
                {
                    // Obtenez le tag à partir de la TextBox
                    string tagToAdd = tagTextBox.Text;

                    // Lisez toutes les lignes du fichier
                    string[] lines = File.ReadAllLines(filePath);

                    // Parcourez les lignes pour trouver celle qui commence par "Tag:"
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("Tag:"))
                        {
                            // Ajoutez le nouveau tag après "Tag:"
                            lines[i] += $"{tagToAdd.ToLower()}, ";
                            break; // Sortez de la boucle une fois que vous avez ajouté le tag
                        }
                    }

                    // Écrivez toutes les lignes mises à jour dans le fichier
                    File.WriteAllLines(filePath, lines);

                    MessageBox.Show($"Tag '{tagToAdd.ToLower()}' ajouté avec succès au fichier '{filePath}'.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ajout du tag : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"Le fichier '{filePath}' n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
