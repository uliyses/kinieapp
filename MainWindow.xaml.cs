using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KiniApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string BasePath = "https://kinieapp-fire.firebaseio.com/";
        private const string FirebaseSecret = "bTTsVUtjiyJBtyRhCf8KlfdZHifExxuBbmJ7vDTk";
        private static FirebaseClient client;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePath
            };
            
            client = new FirebaseClient(config);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            PushResponse response = await client.PushAsync("kinieapp/", this.MessageText.Text);
            this.MessageText.Text = string.Empty;
        }
    }
}
