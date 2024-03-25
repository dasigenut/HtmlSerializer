using HtmlSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace project2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector()
        {
            TagName = null;
            Id = null;
            Classes = new List<string>();
            Child = null;
            Parent = null;
        }

        public static Selector selectorElement(string select)                  //מקבלת את המחרוזת שאותה אני מחפשת
        {
            //מחלק לפי רמות
            List<string> levels = select.Split(' ').ToList();

            Selector rootSelector = new Selector();
            Selector currentSelector = rootSelector;
            string[] allTags = HtmlHelper.Instance.AllTags;

           /* List<string> allTags = HtmlHelper.Instance.HtmlTags;*/
            //עובר על כל רמה אבא בן וכו'
            foreach (var level in levels)
            {
                //מחלק לפי אלמנטים
                string[] filters = Regex.Split(level, @"(?=[#.])").Where(s => s.Length > 0).ToArray();
                List<string> classes = new List<string>();
                string id = null;
                string tagName = null;
                foreach (var filter in filters)
                {
                    if (filter.StartsWith("#"))
                    {
                        //לא יכלול לי את #
                        currentSelector.Id = filter.Substring(1);
                        Console.WriteLine("the filter is ok");
                    }
                    else if (filter.StartsWith("."))
                    {
                        currentSelector.Classes.Add(filter.Substring(1));
                        Console.WriteLine("the filter is ok");
                    }
                    //בדיקה אם זה שם תקין
                    else if (allTags.Contains(filter))
                    {
                        currentSelector.TagName = filter;
                        Console.WriteLine("the filter is ok");

                    }
                    else
                    {
                        Console.WriteLine("eror the filter not right ");
                    }

                    // יצירת אוביקט selector
                    Selector newSelector = new Selector();
                    //מקשרת אותו אב לבן וההפך
                    newSelector.Parent = currentSelector;
                    currentSelector.Child = newSelector;
                    //מחזיק את הסלקטור הנוכחי
                    currentSelector = newSelector;
                    
                }
            }
            return rootSelector;
        }


        //בדיקה האם אובייקט מתאים ושווה לסלקטור שלי
        public override bool Equals(object obj)
        {
            if (obj is HtmlElement)
            {
                HtmlElement element = (HtmlElement)obj;
                if ((this.Id == null || this.Id != null && this.Id.Equals(element.Id)) && (this.TagName == null || this.TagName != null && this.TagName.Equals(element.Name)))
                {
                    if (this.Classes.Count == 0)//אם אין קלאסים שונים
                        return true;
                    else
                    {
                        foreach (var c in this.Classes)
                        {
                            //עובד על כל הקלאסים ובודק שהם מוכלים
                            if (!element.Classes.Contains(c))
                            {
                                return false;
                            }
                            return true;
                        }
                    }
                }

                return false;
            }
            return false;
        }
        

        public override int GetHashCode()
        {
            // Implement your own GetHashCode method to support proper HashSet behavior
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (TagName != null ? TagName.GetHashCode() : 0);
                hash = hash * 23 + (Id != null ? Id.GetHashCode() : 0);
                foreach (var className in Classes)
                {
                    hash = hash * 23 + className.GetHashCode();
                }
                return hash;
            }
        }
    }
}

