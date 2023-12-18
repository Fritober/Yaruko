using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace Projet
{
    public partial class AddTaskWindow : Window
    {
        public TaskManager TaskManager { get; set; }
        public ObservableCollection<CustomTask> Tasks { get; set; }

        public AddTaskWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<CustomTask>();
            DataContext = this;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour ajouter une tâche
            // Utilisez les contrôles XAML pour obtenir les entrées de l'utilisateur
            string title = taskTitleTextBox.Text.ToUpper();
            string description = taskDescriptionTextBox.Text;
            int priority;

            if (!int.TryParse(taskPriorityTextBox.Text, out priority))
            {
                MessageBox.Show("Veuillez entrer un nombre valide pour la priorité.");
                return;
            }

            // Créez une nouvelle tâche
            CustomTask newTask = new CustomTask(title, description, priority);

            // Ajoutez la tâche à la collection de la fenêtre
            Tasks.Add(newTask);

            // Ajoutez la tâche au TaskManager de la fenêtre principale
            if (TaskManager != null)
            {
                TaskManager.AddTask(newTask);
                TaskManager.SaveTasks(); // Sauvegardez les tâches après l'ajout
            }

            // Effacez les champs de saisie après l'ajout d'une tâche
            taskTitleTextBox.Clear();
            taskDescriptionTextBox.Clear();
            taskPriorityTextBox.Clear();

            // Créez un fichier texte avec les informations de la tâche
            CreateTxtFile(newTask);

            // Affichez un message de confirmation
            MessageBox.Show("Tâche ajoutée avec succès.");
        }

        private void CreateTxtFile(CustomTask task)
        {
            // Utilisez le titre de la tâche comme nom de fichier
            string fileName = $"{task.Title}.txt";

            try
            {
                // Chemin du dossier où les fichiers texte seront sauvegardés
                string folderPath = "TaskFiles";

                // Assurez-vous que le dossier existe, s'il n'existe pas, créez-le
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Chemin complet du fichier texte
                string filePath = Path.Combine(folderPath, fileName);

                // Écrivez les informations de la tâche dans le fichier texte
                File.WriteAllText(filePath, $"Title: {task.Title}\nDescription: {task.Description}\nPriority: {task.Priority}\nTag: \nDeadLine:");

                // Obtenez le chemin complet du dossier
                string fullFolderPath = Path.GetFullPath(folderPath);

                // Ouvrir l'explorateur de fichiers dans le dossier
                Process.Start("explorer.exe", fullFolderPath);

                // Affichez un message de confirmation avec le chemin du dossier
                MessageBox.Show($"Tâche ajoutée avec succès. Fichier créé dans : {fullFolderPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création du fichier texte : {ex.Message}");
            }
        }

        private void OpenTaskFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Chemin du dossier où les fichiers texte sont sauvegardés
                string folderPath = "TaskFiles";

                // Assurez-vous que le dossier existe
                if (Directory.Exists(folderPath))
                {
                    // Ouvrir l'explorateur de fichiers dans le dossier
                    Process.Start("explorer.exe", folderPath);
                }
                else
                {
                    MessageBox.Show("Le dossier des tâches n'existe pas encore. Veuillez créer une tâche d'abord.", "Dossier introuvable", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du dossier : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
