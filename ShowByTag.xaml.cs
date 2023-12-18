using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Diagnostics;

namespace Projet
{
    public partial class ShowByTag : Window
    {
        public ShowByTag()
        {
            InitializeComponent();
        }

        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupérez le tag à partir de la TextBox
            string enteredTag = tagTextBox.Text;

            // Vérifiez que le tag n'est pas vide
            if (string.IsNullOrEmpty(enteredTag))
            {
                MessageBox.Show("Veuillez entrer un tag.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Effacez les éléments précédents de la ListBox
            taskListBox.Items.Clear();

            // Chargez les fichiers avec le tag spécifié
            LoadFilesByTag(enteredTag);
        }

        private void LoadFilesByTag(string tag)
        {
            // Chemin du dossier des tâches
            string tasksFolderPath = @"E:\\ESILV\\Software\\Projet\\bin\\Debug\\net7.0-windows\\TaskFiles";

            try
            {
                // Récupérer tous les fichiers dans le dossier
                string[] allFiles = Directory.GetFiles(tasksFolderPath);

                // Filtrer les fichiers qui contiennent le tag spécifié
                var filteredFiles = allFiles.Where(file =>
                {
                    // Lire toutes les lignes du fichier
                    string[] lines = File.ReadAllLines(file);

                    // Vérifier si le tag est présent dans le fichier
                    return lines.Any(line => line.Contains($"Tag: {tag.ToLower()}"));
                });

                // Afficher les noms des fichiers filtrés
                foreach (var fileName in filteredFiles)
                {
                    taskListBox.Items.Add(Path.GetFileName(fileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des fichiers : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void taskListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Récupérez le chemin du fichier sélectionné
            string selectedFileName = taskListBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedFileName))
            {
                // Construisez le chemin complet du fichier
                string filePath = Path.Combine(@"E:\\ESILV\\Software\\Projet\\bin\\Debug\\net7.0-windows\\TaskFiles", selectedFileName);

                try
                {
                    // Ouvrez le fichier avec l'application associée
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ouverture du fichier : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Fermer la fenêtre
            Close();
        }
    }
}
