using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Projet
{
    public partial class RemoveTaskWindow : Window
    {
        private string selectedFilePath;

        public RemoveTaskWindow()
        {
            InitializeComponent();
        }

        private void SelectFileToDelete_Click(object sender, RoutedEventArgs e)
        {
            var folderPath = Path.Combine(Environment.CurrentDirectory, "TaskFiles");

            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Sélectionner le fichier à supprimer",
                Filter = "Fichiers texte (*.txt)|*.txt",
                InitialDirectory = folderPath
            };

            bool? result = openFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                selectedFilePath = openFileDialog.FileName;
                filePathTextBox.Text = selectedFilePath;
            }
        }


        private void RemoveSelectedTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedFilePath) && File.Exists(selectedFilePath))
            {
                try
                {
                    File.Delete(selectedFilePath);
                    MessageBox.Show($"Fichier supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression du fichier : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un fichier à supprimer.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
