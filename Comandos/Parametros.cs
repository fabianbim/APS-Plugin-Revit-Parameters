using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Application = Autodesk.Revit.ApplicationServices.Application;
//using Autodesk.Revit.ApplicationServices;

namespace RevitAddinforAPS.Comandos
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Parametros : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region variables globales
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application APP = uiapp.Application;
            Autodesk.Revit.DB.Document doc = uidoc.Document;


            //View3D view3D = doc.ActiveView as View3D; // Obtener la vista activa, en este caso 3D

            #endregion

            #region PREVENCION DE VISTA
            //if (view3D == null)
            //{
                //TaskDialog.Show("Error", "Por favor, activa una vista 3D para correr el comando.");
                //return Result.Failed;
            //}

            #endregion DICCIONARIO DE PARAMETROS

            #region
            Dictionary<string, string> clasificacionUnidad = new Dictionary<string, string>()
            {
                { "Acabados Cieloraso", "m2" },
                { "Acabados Muros y Columnas", "m2" },
                { "Acabados Pisos", "m2" },
                { "Aparatos Sanitarios", "und" },
                { "Arquitectura Otros", "-" },
                { "Carpinteria", "und" },
                { "Fachada", "m2" },
                { "HTV", "und" },
                { "Juntas Sismicas", "ml" },
                { "Mobiliario", "und" },
                { "N/A", "N/A" },
                { "Particiones", "m2" },
                { "Pavimentos Exteriores", "m2" },
                { "Pisos Elevados", "m2" },
                { "Protecciones y Barreras", "ml" },
                { "Sistema Cobertura", "m2" }
            };


            #endregion

            #region
            FilteredElementCollector collector = new FilteredElementCollector(doc)
                                        .WhereElementIsNotElementType()
                                        .WhereElementIsViewIndependent();

            using (Transaction trans = new Transaction(doc, "Actualizar Parametros Unidad"))
            {
                trans.Start();

                foreach (Element element in collector)
                {
                    ElementId typeId = element.GetTypeId(); // Obtener el tipo de elemento
                    if (typeId == ElementId.InvalidElementId)
                        continue;

                    Element typeElement = doc.GetElement(typeId); // Obtener el tipo del elemento

                    // Buscar los parámetros "Clasificación Elemento" y "Unidad"
                    Parameter clasificacionParam = typeElement.LookupParameter("IP_ING_Clasificación Elemento");
                    Parameter unidadParam = typeElement.LookupParameter("IP_QTY_Unidad");

                    if (clasificacionParam != null && unidadParam != null)
                    {
                        string clasificacionValor = clasificacionParam.AsString();

                        if (!string.IsNullOrEmpty(clasificacionValor) && clasificacionUnidad.ContainsKey(clasificacionValor))
                        {
                            string unidadValor = clasificacionUnidad[clasificacionValor];

                            // Si el parámetro "Unidad" está vacío, asignar el valor correspondiente
                            if (string.IsNullOrEmpty(unidadParam.AsString()))
                            {
                                unidadParam.Set(unidadValor);
                            }
                        }
                    }
                }

                trans.Commit();
            }

            TaskDialog.Show("Completado", "Se ha actualizado el parámetro 'Unidad' basado en 'Clasificación Elemento' para los elementos visibles en la vista 3D.");
            #endregion

            return Result.Succeeded;
        }
    }
}
