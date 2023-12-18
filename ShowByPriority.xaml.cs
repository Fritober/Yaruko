using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Diagnostics;

namespace Projet
{
    public partial class ShowByPriority : Window
    {
        public ShowByPriority()
        {
            InitializeComponent();
            LoadFilesByPriority();
        }

        private void LoadFilesByPriority()
        {
            // Chemin du dossier des tâches
            string tasksFolderPath = @"E:\\ESILV\\Software\\Projet\\bin\\Debug\\net7.0-windows\\TaskFiles";

            try
            {
                // Récupérer tous les fichiers dans le dossier
                string[] allFiles = Directory.GetFiles(tasksFolderPath);

                // Trier les fichiers par priorité
                var sortedFiles = SortFilesByPriority(allFiles);

                // Afficher les noms des fichiers triés
                foreach (var fileName in sortedFiles)
                {
                    taskListBox.Items.Add(Path.GetFileName(fileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des fichiers : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private IEnumerable<string> SortFilesByPriority(string[] files)
        {
            // Trier les fichiers par priorité décroissante en utilisant LINQ
            var sortedFiles = files.OrderBy(file =>
            {
                // Lire toutes les lignes du fichier
                string[] lines = File.ReadAllLines(file);

                // Trouver la ligne qui commence par "Priority:"
                string priorityLine = lines.FirstOrDefault(line => line.StartsWith("Priority:"));

                // Extraire la valeur de priorité (vous devrez peut-être ajuster cela en fonction du format réel)
                if (priorityLine != null)
                {
                    string priorityValue = priorityLine.Replace("Priority:", "").Trim();
                    if (int.TryParse(priorityValue, out int priority))
                    {
                        return priority;
                    }
                }

                // Valeur par défaut si la priorité n'est pas spécifiée ou n'est pas un entier valide
                return int.MaxValue;
            });

            return sortedFiles;
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
