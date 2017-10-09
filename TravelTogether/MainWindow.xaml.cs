using System.Windows;
using System.Windows.Forms;

namespace TravelTogether
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TogetherExecutor = new Executor();
        }

        internal void button_Add_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    AddFolder(dialog.SelectedPath);
                }
            }
        }

        internal Executor TogetherExecutor { get; set; }

        internal void AddFolder(string path)
        {
            TogetherExecutor.FolderComponents.Add(new FolderComponent(path));
            listBox_Folder.Items.Add(path);
        }

        internal void DeleteFolder(int i)
        {
            TogetherExecutor.FolderComponents.RemoveAt(i);
            listBox_Folder.Items.RemoveAt(i);
        }
    }
}