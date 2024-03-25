using System;
using System.IO;
using System.Text.Json;

namespace HtmlSerializer
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        public string[] AllTags { get; private set; }
        public string[] HtmlVoidTags { get; private set; }
        public List<string> HtmlTags { get; internal set; }

        private HtmlHelper()
        {
            string allTagsJson = File.ReadAllText("HtmlTags.json");
            string htmlVoidTagsJson = File.ReadAllText("HtmlVoidTags.json");
            AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(htmlVoidTagsJson);
        }

        // פונקציה להדפסת המערכים
        public void PrintTags()
        {
            Console.WriteLine("All Tags:");
            foreach (var tag in AllTags)
            {
                Console.WriteLine(tag);
            }

            Console.WriteLine("\nHTML Void Tags:");
            foreach (var voidTag in HtmlVoidTags)
            {
                Console.WriteLine(voidTag);
            }
        }
    }
    
}
