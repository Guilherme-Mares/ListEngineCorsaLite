namespace ListEngineCorsaLite
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromArgb("#2196F3"),  // Cor da barra
                BarTextColor = Colors.White,                     // Cor do texto
                Title = "ListEngine Corsa Lite"                  // Título
            };
        }
    }
}
