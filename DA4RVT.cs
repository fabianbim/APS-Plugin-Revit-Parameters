using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RevitAddinforAPS
{
    public class DA4RVT : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "APS";
            string panelName = "Parameters";
            application.CreateRibbonTab(tabName);
            RibbonPanel rpanel1 = application.CreateRibbonPanel(tabName, panelName);
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyPath = assembly.Location;
            PushButton BOTON = rpanel1.AddItem(new PushButtonData("Parameters", "Update Params", assemblyPath
                , "RevitAddinforAPS.Comandos.Parametros")) as PushButton;
            BOTON.ToolTip = "este boton executa una acutalizacion de valores de parametros";
            BOTON.LargeImage = GetResourceImage(assembly, "RevitAddinforAPS.ICONS.database32.png");
            BOTON.Image = GetResourceImage(assembly, "RevitAddinforAPS.ICONS.database16.png");
            return Result.Succeeded;
        }

        public ImageSource GetResourceImage (Assembly assembly,string imageName)
        {
            try {
                Stream resource = assembly.GetManifestResourceStream(imageName);
                return BitmapFrame.Create(resource);


            }
            catch
            {
                return null;
            }
        }
    }
}
