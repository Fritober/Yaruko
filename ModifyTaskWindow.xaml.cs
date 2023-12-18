using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace Projet
{
    /// <summary>
    /// Logique d'interaction pour ModifyTaskWindow.xaml
    /// </summary>
    public partial class ModifyTaskWindow : Window
    {
        public ModifyTaskWindow()
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

        private void ApplyChanges_Click(object sender, RoutedEventArgs e)
        {
            // Obtenez le chemin du fichier à partir de la fenêtre principale
            string filePath = filePathTextBox.Text;  // Assurez-vous de définir le chemin du fichier

            if (File.Exists(filePath))
            {
                try
                {
                    // Obtenez les nouvelles valeurs à partir des champs d'édition
                    string newDescription = descriptionTextBox.Text;
                    int newPriority;
                    do
                    {
                        string priorityInput = priorityTextBox.Text;

                        if (int.TryParse(priorityInput, out newPriority))
                        {
                            // La conversion a réussi, sortez de la boucle
                            break;
                        }
                        else
                        {
                            // La conversion a échoué, demandez à l'utilisateur de saisir à nouveau
                            MessageBox.Show("Veuillez entrer une valeur numérique pour la priorité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            // Vous pouvez également choisir de vider la boîte de priorité ici
                            priorityTextBox.Text = string.Empty;
                        }
                    } while (true);

                    DateTime? newDeadline = deadlineDatePicker.SelectedDate;
                    string newTitle = titleTextBox.Text;

                    // Lisez toutes les lignes du fichier
                    string[] lines = File.ReadAllLines(filePath);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("Description:"))
                        {
                            // Mettez à jour la ligne de la description
                            lines[i] = $"Description: {newDescription}";
                        }
                        else if (lines[i].StartsWith("Priority:"))
                        {
                            // Mettez à jour la ligne de la priorité
                            lines[i] = $"Priority: {newPriority}";
                        }
                        else if (lines[i].StartsWith("DeadLine:"))
                        {
                            // Mettez à jour la ligne de la deadline
                            lines[i] = $"DeadLine: {newDeadline?.ToString("yyyy/MM/dd")}";
                        }
                        else if (lines[i].StartsWith("Title:"))
                        {
                            // Mettez à jour la ligne du titre
                            lines[i] = $"Title: {newTitle}";
                        }
                    }

                    
                    // Écrivez toutes les lignes mises à jour dans le fichier
                    File.WriteAllLines(filePath, lines);

                    // Renommez le fichier avec le nouveau titre
                    string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), $"{newTitle}.txt");
                    File.Move(filePath, newFilePath);

                    MessageBox.Show("Modifications appliquées avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'application des modifications : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"Le fichier '{filePath}' n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
