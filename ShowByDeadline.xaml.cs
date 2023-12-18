using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Diagnostics;

namespace Projet
{
    public partial class ShowByDeadline : Window
    {
        public ShowByDeadline()
        {
            InitializeComponent();
            LoadFilesByDeadline();
        }

        private void LoadFilesByDeadline()
        {
            // Chemin du dossier des tâches
            string tasksFolderPath = @"E:\\ESILV\\Software\\Projet\\bin\\Debug\\net7.0-windows\\TaskFiles";

            try
            {
                // Récupérer tous les fichiers dans le dossier
                string[] allFiles = Directory.GetFiles(tasksFolderPath);

                // Trier les fichiers par deadline
                var sortedFiles = SortFilesByDeadline(allFiles);

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

        private IEnumerable<string> SortFilesByDeadline(string[] files)
        {
            // Trier les fichiers par deadline croissante en utilisant LINQ
            var sortedFiles = files.OrderBy(file =>
            {
                // Lire toutes les lignes du fichier
                string[] lines = File.ReadAllLines(file);

                // Trouver la ligne qui commence par "Deadline:"
                string deadlineLine = lines.FirstOrDefault(line => line.StartsWith("DeadLine:"));

                // Extraire la valeur de la deadline (vous devrez peut-être ajuster cela en fonction du format réel)
                if (deadlineLine != null)
                {
                    string deadlineValue = deadlineLine.Replace("DeadLine:", "").Trim();
                    if (DateTime.TryParse(deadlineValue, out DateTime deadline))
                    {
                        return deadline;
                    }
                }

                // Valeur par défaut si la deadline n'est pas spécifiée ou n'est pas une date valide
                return DateTime.MaxValue;
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
