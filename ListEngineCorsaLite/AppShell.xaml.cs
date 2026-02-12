namespace ListEngineCorsaLite
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(DetalhesMotorPage), typeof(DetalhesMotorPage));
        }
    }
}
