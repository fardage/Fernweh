using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Fernweh.Models;

namespace Fernweh.Services
{
    public static class TemplateProvider
    {
        private const string TemplateFile = "Fernweh.Resources.JSON.ChecklistTemplate.json";

        public static Collection<ItemCategory> GetChecklist()
        {
            var assembly = typeof(TemplateProvider).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(TemplateFile);

            var jsonContent = "";
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                jsonContent = reader.ReadToEnd();
            }

            return JsonSerializer.Deserialize<Collection<ItemCategory>>(jsonContent);
        }
    }
}