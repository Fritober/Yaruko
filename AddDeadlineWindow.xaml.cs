using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace Projet
{
    /// <summary>
    /// Logique d'interaction pour AddDeadlineWindow.xaml
    /// </summary>
    public partial class AddDeadlineWindow : Window
    {
        public AddDeadlineWindow()
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

        private void AddDeadline_Click(object sender, RoutedEventArgs e)
        {
            // Obtenez le chemin du fichier à partir de la TextBox
            string filePath = filePathTextBox.Text;

            if (File.Exists(filePath))
            {
                try
                {
                    // Obtenez la deadline à partir du DatePicker
                    DateTime deadlineToAdd = deadlineDatePicker.SelectedDate ?? DateTime.Now;

                    // Convertissez la date en format "YYYY/MM/DD"
                    string formattedDeadline = deadlineToAdd.ToString("yyyy/MM/dd");

                    // Lisez toutes les lignes du fichier
                    string[] lines = File.ReadAllLines(filePath);

                    // Parcourez les lignes pour trouver celle qui commence par "DeadLine:"
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("DeadLine:"))
                        {
                            // Ajoutez la nouvelle deadline après "DeadLine:"
                            lines[i] += $" {formattedDeadline}";
                            break; // Sortez de la boucle une fois que vous avez ajouté la deadline
                        }
                    }

                    // Écrivez toutes les lignes mises à jour dans le fichier
                    File.WriteAllLines(filePath, lines);

                    MessageBox.Show($"Deadline '{formattedDeadline}' ajoutée avec succès au fichier '{filePath}'.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ajout de la deadline : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"Le fichier '{filePath}' n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
