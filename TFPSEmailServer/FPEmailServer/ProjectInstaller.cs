using System.ComponentModel;
using System.Configuration.Install;


namespace TFPSEmailServer
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
