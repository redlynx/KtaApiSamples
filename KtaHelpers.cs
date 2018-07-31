using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalAgility.Sdk;

public static class KtaHelpers
{
    public static void GetAnnotationInfo(string sessionId, string docId)
    {
        var cds = new CaptureDocumentService();

        // get all annotations
        var annotations = cds.GetAnnotations(sessionId, docId);
        annotations.ForEach(x =>
        {
            Console.WriteLine($"{x.GetType()} @ {x.Location.ToString()}");
        });

        // get sticky notes
        var doc = cds.GetDocument(sessionId, null, docId);
        doc.Pages.ForEach(p =>
        {
            p.Annotations.ForEach(x =>
            {
                Console.WriteLine($"{x.Text} @ {x.Top}, {x.Left}");
            });
        });
    }
}
