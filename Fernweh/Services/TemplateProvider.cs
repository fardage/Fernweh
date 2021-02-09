using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Fernweh.Models;
using Xamarin.Forms;

namespace Fernweh.Services
{
    public static class TemplateProvider
    {
        private const string TemplateFile = "Fernweh.Resources.JSON.ChecklistTemplate.json";
        private const string CustomTemplateFile = "UserChecklistTemplate.json";

        public static string CustomTemplatePath
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..",
                            "Library", CustomTemplateFile);
                    case Device.Android:
                        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                            CustomTemplateFile);
                    default:
                        throw new NotImplementedException("Platform not supported");
                }
            }
        }

        public static List<ItemCategory> GetChecklist()
        {
            return File.Exists(CustomTemplatePath) ? GetUserCheckList() : GetResourceChecklist();
        }

        private static List<ItemCategory> GetUserCheckList()
        {
            var jsonContent = File.ReadAllText(CustomTemplatePath);
            return JsonSerializer.Deserialize<List<ItemCategory>>(jsonContent);
        }

        public static List<ItemCategory> GetResourceChecklist()
        {
            var assembly = typeof(TemplateProvider).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(TemplateFile);

            var jsonContent = "";
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                jsonContent = reader.ReadToEnd();
            }

            return JsonSerializer.Deserialize<List<ItemCategory>>(jsonContent);
        }

        public static void SetChecklist(List<ItemCategory> template)
        {
            var json = JsonSerializer.Serialize(template);
            File.WriteAllText(CustomTemplatePath, json);
        }
    }
}