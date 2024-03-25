using HtmlSerializer; // יבוא של הקוד לספרייה HtmlSerializer
using project2; // יבוא של הקוד לפרוייקט 2
using System; // יבוא של המרחב השמות System
using System.Collections.Generic; // יבוא של המרחב השמות System.Collections.Generic
using System.Linq; // יבוא של המרחב השמות System.Linq
using System.Net.Http; // יבוא של המרחב השמות System.Net.Http
using System.Security.Cryptography; // יבוא של המרחב השמות System.Security.Cryptography
using System.Text.RegularExpressions; // יבוא של המרחב השמות System.Text.RegularExpressions
using System.Threading.Tasks; // יבוא של המרחב השמות System.Threading.Tasks
using System.Xml.Linq; // יבוא של המרחב השמות System.Xml.Linq

async Task<string> Load(string url) // מימוש של פונקציה אסינכרונית ששולפת תוכן מה-URL המתקבל כארגומנט
{
    HttpClient client = new HttpClient(); // יצירת אינסטנס חדש של HttpClient
    var response = await client.GetAsync(url); // שליחת בקשת GET ל-URL הנתון והמתנה לתגובה
    var html = await response.Content.ReadAsStringAsync(); // קריאת תוכן התגובה כמחרוזת
    return html; // החזרת ה-HTML
}

void PrintTree(HtmlElement root, int level) // פונקציה שמדפיסה את עץ ה-HTML
{
    if (root == null) // בדיקה אם השורש הוא null
        return; // יציאה מהפונקציה אם השורש הוא null

    // קביעת הצבע על פי הרמה
    switch (level % 5) // מתקבל רמה מתוך חמישה
    {
        case 0: // אם השארית היא 0
            Console.ForegroundColor = ConsoleColor.Red; // הצבת הצבע של הטקסט בקונסול לאדום
            break;
        case 1: // אם השארית היא 1
            Console.ForegroundColor = ConsoleColor.Green; // הצבת הצבע של הטקסט בקונסול לירוק
            break;
        case 2: // אם השארית היא 2
            Console.ForegroundColor = ConsoleColor.Blue; // הצבת הצבע של הטקסט בקונסול לכחול
            break;
        case 3: // אם השארית היא 3
            Console.ForegroundColor = ConsoleColor.Yellow; // הצבת הצבע של הטקסט בקונסול לצהוב
            break;
        case 4: // אם השארית היא 4
            Console.ForegroundColor = ConsoleColor.Magenta; // הצבת הצבע של הטקסט בקונסול לסגול
            break;
    }
    Console.WriteLine("-----------------"); // הדפסת קו מופרד
    Console.WriteLine(root); // הדפסת האלמנט הנוכחי של ה-HTML
    Console.ResetColor(); // איפוס הצבע בקונסול

    foreach (var child in root.Children) // לולאה שעוברת על כל הילדים של האלמנט הנוכחי
    {
        PrintTree(child, level + 1); // קריאה רקורסיבית לפונקציה עבור כל ילד
    }
}



HtmlElement BuildTree(List<string> htmlLines) // פונקציה שבונה את עץ ה-HTML משורות ה-HTML
{
    var root = new HtmlElement(); // יצירת אלמנט ראשי של HTML

    var currentElement = root; // הגדרת האלמנט הנוכחי כאלמנט הראשי

    foreach (var line in htmlLines) // לולאה שעוברת על כל שורת ה-HTML
    {
        var firstWord = line.Split(' ')[0]; // חילוץ המילה הראשונה של השורה

        if (firstWord == "/html") // בדיקה אם זו סיום של HTML
        {
            break; // יציאה מהלולאה אם זו סיום של HTML
        }
        else if (firstWord.StartsWith("/")) // בדיקה אם זו תג סגירה
        {
            if (currentElement.Parent != null) // בדיקה אם יש תג הורה תקין
            {
                currentElement = currentElement.Parent; // מעבר לרמה הקודמת בעץ
            }
        }
        else if (HtmlHelper.Instance.AllTags.Contains(firstWord)) // בדיקה אם זו תג פתיחה תקין
        {
            var newElement = new HtmlElement(); // יצירת אלמנט HTML חדש
            newElement.Name = firstWord; // הגדרת שם האלמנט

            // Handle attributes
            var restOfString = line.Remove(0, firstWord.Length); // הסרת שם התג מהשורה
            var attributes = Regex.Matches(restOfString, "([a-zA-Z]+)=\\\"([^\\\"]*)\\\"") // חיפוש מאפיינים באמצעות regex
                .Cast<Match>()
                .Select(m => $"{m.Groups[1].Value}=\"{m.Groups[2].Value}\"")
                .ToList();

            if (attributes.Any(attr => attr.StartsWith("class"))) // בדיקה אם ישנם מאפייני מחלקה
            {
                // Handle class attribute
                var classAttr = attributes.First(attr => attr.StartsWith("class")); // חילוץ מאפיין מחלקה
                var classes = classAttr.Split('=')[1].Trim('"').Split(' '); // פיצול המחלקות
                newElement.Classes.AddRange(classes); // הוספת המחלקות לאלמנט החדש
            }

            newElement.Attributes.AddRange(attributes); // הוספת המאפיינים לאלמנט החדש

            // Handle ID
            var idAttribute = attributes.FirstOrDefault(attr => attr.StartsWith("id")); // חילוץ מאפיין ה-ID
            if (!string.IsNullOrEmpty(idAttribute)) // בדיקה אם יש מאפיין ID
            {
                newElement.Id = idAttribute.Split('=')[1].Trim('"'); // הגדרת ה-ID של האלמנט החדש
            }

            newElement.Parent = currentElement; // הגדרת ההורה של האלמנט החדש
            currentElement.Children.Add(newElement); // הוספת האלמנט החדש כילד לאלמנט הנוכחי

            // Check if it's a self-closing tag
            if (line.EndsWith("/") || HtmlHelper.Instance.HtmlVoidTags.Contains(firstWord)) // בדיקה אם זו תג סגירה עצמית
            {
                currentElement = newElement.Parent; // מעבר לרמה הקודמת
            }
            else
            {
                currentElement = newElement; // העברה של האלמנט הנוכחי לאלמנט החדש
            }
        }
        else // אחרת, מדובר בתוכן טקסטואלי
        {
            // Text content
            currentElement.InnerHtml = line; // הגדרת התוכן הפנימי של האלמנט הנוכחי
        }
    }

    return root; // החזרת השורש של העץ
}

var html = await Load("https://learn.malkabruk.co.il/practicode/projects/pract-2/#_5"); // קריאה אסינכרונית לטעינת ה-HTML מה-URL הנתון
var cleanHtml = new Regex("\\s+").Replace(html, " "); // ניקוי ה-HTML מרווחים ריקים
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToList(); // פיצול ה-HTML לשורות והסרת רווחים ריקים
var root = BuildTree(htmlLines); // בניית עץ ה-HTML
PrintTree(root, 0); // הדפסת העץ

Selector sel = new Selector(); // יצירת אובייקט של קלאס Selector
string search = ".md-typeset"; // אובייקט המכיל את מחרוזת החיפוש
Selector rooti = Selector.selectorElement(search); // יצירת אובייקט Selector לפי המחרוזת הנתונה
List<HtmlElement> result = root.func1(rooti); // קריאה לפונקציה שבודקת מספר צעצועים של האלמנט
Console.WriteLine("number = " + result.Count); // הדפסת מספר הצעצעים שמצאנו

public static class exmentionMetod
{
    //מחפשת בתוך העץ שלנו את כל האלמנטים שעונים למה שאנחנו מחפשים
    public static List<HtmlElement> func1(this HtmlElement element, Selector selector)
    {
        HashSet<HtmlElement> matcheSet = new HashSet<HtmlElement>();
        func2(element, selector, matcheSet);
        return matcheSet.ToList();
    }
    //מקבלת אלמנט נוכחי ועוברת על הילדים שלו ומחפשת את אלו שעונים לסלקטור הנוכחי
    //וכל פעם היא מופעלת עבור סלקטור אחר או אלנט אחר לפי המצב של החיפוש
    public static void func2(HtmlElement currentElement, Selector selector, HashSet<HtmlElement> set)
    {
        //כל רשימת הצאצים של האלמנט הנוכחי
        IEnumerable<HtmlElement> children = currentElement.Descendants();
        List<HtmlElement> listOfMatches = new List<HtmlElement>();
        foreach (var child in children)
        {
            //בדיקות האם האלמנט עומד בדרישות הסלקטור
            if (selector.Equals(child))
            {
                listOfMatches.Add(child);
            }
        }
        //בדיקה אם הגענו לרמה האחרונה
        if (selector.Child.Child == null)
        {
            set.UnionWith(listOfMatches);
            return;
        }

        else
        {
            foreach (var match in listOfMatches)
            {
                func2(match, selector.Child, set);
            }
        }
    }
}







