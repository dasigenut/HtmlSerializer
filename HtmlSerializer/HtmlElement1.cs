using System; // משמעותה שימוש בספריית המערכת
using System.Collections.Generic; // משמעותה שימוש בספריית המערכת.Collections.Generic
using System.Linq; // משמעותה שימוש בספריית המערכת.Linq
using System.Text; // משמעותה שימוש בספריית המערכת.Text
using System.Threading.Tasks; // משמעותה שימוש בספריית המערכת.Threading.Tasks

namespace HtmlSerializer // יצירת מרחב שמות בשם HtmlSerializer
{

    public class HtmlElement // יצירת מחלקה בשם HtmlElement
    {
        //יצרית משתנים
        public string Name { get; set; } // הגדרת מאפיין שממצה את שם האלמנט ב-HTML
        public string Id { get; set; } // הגדרת מאפיין שממצה את ה־ID של האלמנט ב-HTML
        public List<string> Attributes { get; set; } // הגדרת מאפיין שממצה את המאפיינים של האלמנט ב-HTML
        public List<string> Classes { get; set; } // הגדרת מאפיין שממצה את המחלקות של האלמנט ב-HTML
        public string InnerHtml { get; set; } // הגדרת מאפיין שממצה את התוכן הפנימי של האלמנט ב-HTML
        public HtmlElement Parent { get; set; } // הגדרת מאפיין שמציין את האב של האלמנט ב-HTML
        public List<HtmlElement> Children { get; set; } // הגדרת מאפיין שממצה את הילדים של האלמנט ב-HTML


        public HtmlElement() // יצירת פעולת בנאי למחלקה HtmlElement
        {
            Attributes = new List<string>(); // אתחול הרשימה של המאפיינים
            Classes = new List<string>(); // אתחול הרשימה של המחלקות
            Parent = null; // הגדרת האב לערך הברירת מחדל
            Children = new List<HtmlElement>(); // אתחול הרשימה של הילדים
        }

        public IEnumerable<HtmlElement> Descendants() // פעולת המחזירה את כל הצאצאים של האלמנט
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>(); // יצירת תור לאלמנטים
            queue.Enqueue(this); // הוספת האלמנט הנוכחי לתור
            while (queue.Count > 0) // בוצע כל עוד התור אינו ריק
            {
                HtmlElement currentElement = queue.Dequeue(); // שליפת האלמנט הראשון מהתור
                foreach (var child in currentElement.Children) // לולאה על כל הילדים של האלמנט הנוכחי
                {
                    queue.Enqueue(child); // הוספת הילד לתור
                }
                yield return currentElement; // החזרת האלמנט הנוכחי
            }
        }

        public IEnumerable<HtmlElement> Ancestors() // פעולת המחזירה את כל האבות של האלמנט
        {
            HtmlElement child = this; // הגדרת האלמנט הנוכחי כאב
            while (child.Parent != null) // בוצע כל עוד יש אב לאלמנט
            {
                yield return child.Parent; // החזרת האב
                child = child.Parent; // הגדרת האב כאלמנט הנוכחי
            }
        }

        public override string ToString() // פעולת ToString שמחזירה מחרוזת המייצגת את האלמנט באופן מאורגן
        {
            StringBuilder sb = new StringBuilder(); // יצירת מכונית ליצירת מחרוזת
            sb.Append($"nameTag: {Name}\n"); // הוספת שם האלמנט למחרוזת

            if (!string.IsNullOrEmpty(Id)) // בדיקה האם יש ערך למאפיין Id
            {
                sb.Append($"id: \"{Id}\n"); // הוספת הערך של Id למחרוזת
            }

            if (Classes.Any()) // בדיקה האם יש ערך לרשימת המחלקות
            {
                sb.Append("class: \""); // הוספת תווים לפני תווים של המחלקות
                sb.Append(string.Join(" ", Classes)); // הוספת המחלקות למחרוזת
                sb.Append("\"\n"); // הוספת תווים לאחר תווים של המחלקות
            }

            if (Attributes.Any()) // בדיקה האם יש ערך לרשימת המאפיינים
            {
                sb.Append("attribute :"); // הוספת תווים לפני המאפיינים
                foreach (var attribute in Attributes) // לולאה על כל המאפיינים
                {
                    sb.Append($" {attribute}\n"); // הוספת המאפיין למחרוזת
                }
            }

            if (!string.IsNullOrEmpty(InnerHtml)) // בדיקה האם יש ערך לתוכן הפנימי
            {
                sb.AppendLine($"InnerHtml: {InnerHtml}"); // הוספת תוכן הפנימי למחרוזת
            }

            sb.AppendLine("count of children elements: " + Children.Count()); // הוספת תוכן מידע על מספר הילדים למחרוזת

            return sb.ToString(); // החזרת המחרוזת
        }

    }
}



