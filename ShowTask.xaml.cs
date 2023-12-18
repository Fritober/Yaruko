using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Projet
{
    public partial class ShowTask : Window
    {
        // ObservableCollection pour stocker la liste des fichiers de tâches
        public ObservableCollection<string> TaskFiles { get; set; }

        public ShowTask()
        {
            InitializeComponent();

            // Initialisez la collection
            TaskFiles = new ObservableCollection<string>();

            // Chargez la liste des fichiers de tâches au démarrage de la fenêtre
            LoadTaskFiles();

            // Liez la collection à la ListBox dans l'interface graphique
            taskListBox.ItemsSource = TaskFiles;
        }

        // Méthode pour charger la liste des fichiers de tâches
        private void LoadTaskFiles()
        {
            // Assurez-vous de spécifier le chemin correct du dossier des tâches
            string tasksFolderPath = @"E:\\ESILV\\Software\\Projet\\bin\\Debug\\net7.0-windows\\TaskFiles";

            // Vérifiez si le dossier existe
            if (Directory.Exists(tasksFolderPath))
            {
                // Obtenez la liste des fichiers texte dans le dossier
                string[] taskFiles = Directory.GetFiles(tasksFolderPath, "*.txt");

                // Ajoutez les fichiers à la collection
                foreach (string taskFile in taskFiles)
                {
                    TaskFiles.Add(Path.GetFileName(taskFile));
                }
            }
            else
            {
                MessageBox.Show($"Le dossier des tâches '{tasksFolderPath}' n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Événement déclenché lorsqu'un élément de la ListBox est sélectionné
        private void taskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

    }
}
