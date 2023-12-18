using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

namespace Projet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string dataFilePath = "tasks.xml";

        private TaskManager taskManager;
        public TaskManager TaskManager { get; set; }

        public ObservableCollection<CustomTask> Tasks { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            taskManager = new TaskManager();
            LoadTasks();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // Créez une nouvelle instance de la fenêtre AddTaskWindow
            AddTaskWindow addTaskWindow = new AddTaskWindow();

            // Affichez la fenêtre modale avec ShowDialog
            addTaskWindow.ShowDialog();
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            RemoveTaskWindow removeTaskWindow = new RemoveTaskWindow();
            removeTaskWindow.ShowDialog();
        }

        private void AddTagToTask_Click(object sender, RoutedEventArgs e)
        {
            // Instancier la fenêtre AddTagWindow
            AddTagWindow addTagWindow = new AddTagWindow();

            addTagWindow.ShowDialog();      
        }


        private void AddDeadlineToTask_Click(object sender, RoutedEventArgs e)
        {
            AddDeadlineWindow addDeadlineWindow = new AddDeadlineWindow();
            addDeadlineWindow.ShowDialog();
        }

        private void ModifyTask_Click(object sender, RoutedEventArgs e)
        {
           ModifyTaskWindow modifyTaskWindow = new ModifyTaskWindow();
           modifyTaskWindow.ShowDialog();
        }
        private void ShowAllTasks_Click(object sender, RoutedEventArgs e)
        {
            ShowTask showTask = new ShowTask();
            showTask.ShowDialog();
        }

        private void ShowTasksByPriority_Click(object sender, RoutedEventArgs e)
        {
            ShowByPriority showByPriority = new ShowByPriority();
            showByPriority.ShowDialog();
        }

        private void ShowTasksByTag_Click(object sender, RoutedEventArgs e)
        {
            ShowByTag showByTag = new ShowByTag();
            showByTag.ShowDialog();
        }

        private void ShowTasksByDeadlineStatus_Click(object sender, RoutedEventArgs e)
        {
            ShowByDeadline showByDeadline = new ShowByDeadline();
            showByDeadline.ShowDialog();
        }

        private void LoadTasks()
        {
            if (File.Exists(dataFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<CustomTask>));
                using (TextReader reader = new StreamReader(dataFilePath))
                {
                    TaskManager.Tasks = (List<CustomTask>)serializer.Deserialize(reader);
                }
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir quitter l'application?", "Confirmation de sortie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Logique pour quitter l'application
                // ...
                this.Close();
            }
        }

    }
}