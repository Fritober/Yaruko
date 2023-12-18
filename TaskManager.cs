using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization; // Utilisation de XmlSerializer pour la sérialisation XML

namespace Projet
{
    public class TaskManager
    {
        public List<CustomTask> tasks = new List<CustomTask>();
        public string dataFilePath = "tasks.xml"; // Nom du fichier de données

        public TaskManager()
        {
            LoadTasks(); // Charger les tâches depuis le fichier lors du démarrage
        }
        public List<CustomTask> Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }

        public void AddTask(CustomTask task)
        {
            tasks.Add(task);
        }

        public void RemoveTask(string title)
        {
            CustomTask taskToRemove = tasks.FirstOrDefault(task => task.Title == title);
            if (taskToRemove != null)
            {
                tasks.Remove(taskToRemove);
                Console.WriteLine($"Tâche '{title}' supprimée.");
            }
            else
            {
                Console.WriteLine($"Tâche '{title}' introuvable.");
            }
        }

        public void ModifyTask(string title)
        {
            CustomTask taskToModify = tasks.FirstOrDefault(t => t.Title == title);
            if (taskToModify != null)
            {
                Console.WriteLine($"Tâche '{title}' trouvée. Entrez les nouvelles informations :");

                Console.Write("Nouveau titre : ");
                string newTitle = Console.ReadLine().ToUpper();
                Console.Write("Nouvelle description : ");
                string newDescription = Console.ReadLine();
                Console.Write("Nouvelle priorité : ");
                int newPriority;
                while (!int.TryParse(Console.ReadLine(), out newPriority))
                {
                    Console.WriteLine("Veuillez entrer un nombre valide pour la priorité.");
                    Console.Write("Nouvelle priorité : ");
                }

                // Vous pouvez ajouter des options pour modifier d'autres propriétés (tags, deadline, etc.) si nécessaire.

                taskToModify.Title = newTitle;
                taskToModify.Description = newDescription;
                taskToModify.Priority = newPriority;

                Console.WriteLine($"Tâche '{title}' modifiée avec succès.");
            }
            else
            {
                Console.WriteLine($"Tâche '{title}' introuvable.");
            }
        }

        public CustomTask GetTaskByTitle(string title)
        {
            return tasks.FirstOrDefault(task => task.Title == title);
        }

        public List<CustomTask> GetTasksByPriority()
        {
            return tasks.OrderBy(task => task.Priority).ToList();
        }

        public List<CustomTask> GetTasksByTag(string tag)
        {
            return tasks.Where(task => task.Tags.Contains(tag)).ToList();
        }

        public List<CustomTask> GetTasksNotPassedDeadline()
        {
            DateTime currentDate = DateTime.Now;
            return tasks.Where(task => task.Deadline > currentDate).ToList();
        }

        public List<CustomTask> GetTasksPassedDeadline()
        {
            DateTime currentDate = DateTime.Now;
            return tasks.Where(task => task.Deadline <= currentDate).ToList();
        }

        public List<CustomTask> GetAllTasks()
        {
            return tasks.ToList();
        }


        public void ShowTasksByPriority()
        {
            var sortedTasks = tasks.OrderBy(task => task.Priority);
            foreach (var task in sortedTasks)
            {
                Console.WriteLine($"Titre : {task.Title}, Priorité : {task.Priority}");
            }
        }

        public void AddTagToTask(string title, string tag)
        {
            CustomTask task = tasks.FirstOrDefault(t => t.Title == title);
            if (task != null)
            {
                task.Tags.Add(tag);
                Console.WriteLine($"Tag '{tag}' ajouté à la tâche '{title}'.");
            }
            else
            {
                Console.WriteLine($"Tâche '{title}' introuvable.");
            }
        }

        public void AddDeadlineToTask(string title, DateTime deadline)
        {
            CustomTask task = tasks.FirstOrDefault(t => t.Title == title);
            if (task != null)
            {
                task.Deadline = deadline;
                Console.WriteLine($"Deadline ajoutée à la tâche '{title}'.");
            }
            else
            {
                Console.WriteLine($"Tâche '{title}' introuvable.");
            }
        }

        public void ShowAllTasks()
        {
            Console.WriteLine("Tâches existantes :");
            foreach (var task in tasks)
            {
                Console.WriteLine($"Titre : {task.Title}, Description : {task.Description}, Priorité : {task.Priority}");

                if (task.Tags.Count > 0)
                {
                    Console.WriteLine("Tags : " + string.Join(", ", task.Tags));
                }

                if (task.Deadline != DateTime.MinValue)
                {
                    Console.WriteLine($"Deadline : {task.Deadline}");
                }

                Console.WriteLine(); // Ajoute une ligne vide entre chaque tâche pour une meilleure lisibilité
            }
        }

        public void ShowTasksByTag(string tag)
        {
            var taggedTasks = tasks.Where(task => task.Tags.Contains(tag));
            Console.WriteLine($"Tâches avec le tag '{tag}' :");
            foreach (var task in taggedTasks)
            {
                Console.WriteLine($"Titre : {task.Title}, Priorité : {task.Priority}");
            }
        }

        public void ShowTasksByDeadlineStatus()
        {
            DateTime currentDate = DateTime.Now;
            var tasksNotPassedDeadline = tasks.Where(task => task.Deadline > currentDate);
            var tasksPassedDeadline = tasks.Where(task => task.Deadline <= currentDate);

            Console.WriteLine("Tâches dont la deadline n'a pas encore été dépassée :");
            foreach (var task in tasksNotPassedDeadline)
            {
                Console.WriteLine($"Titre : {task.Title}, Deadline : {task.Deadline}");
            }

            Console.WriteLine("\nTâches dont la deadline a été dépassée :");
            foreach (var task in tasksPassedDeadline)
            {
                Console.WriteLine($"Titre : {task.Title}, Deadline : {task.Deadline}");
            }
        }


        // Méthode pour sauvegarder les tâches dans un fichier
        public void SaveTasks()
        {
            try
            {
                using (FileStream fs = new FileStream(dataFilePath, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<CustomTask>));
                    serializer.Serialize(fs, tasks);
                    Console.WriteLine("Tâches sauvegardées avec succès.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde des tâches : {ex.Message}");
            }
        }

        // Méthode pour charger les tâches depuis un fichier
        public void LoadTasks()
        {
            if (File.Exists(dataFilePath))
            {
                try
                {
                    using (FileStream fs = new FileStream(dataFilePath, FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<CustomTask>));
                        tasks = (List<CustomTask>)serializer.Deserialize(fs);
                        Console.WriteLine("Tâches chargées avec succès.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du chargement des tâches : {ex.Message}");
                }
            }
        }
    }
}
