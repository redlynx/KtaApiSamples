using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TotalAgility.Sdk;

namespace KtaLib
{
    public class ExportService
    {
        public void ExportFolderImages(string sessionId, string folderId, string outputFolder, string extension)
        {
            var cds = new CaptureDocumentService();
            var folder = cds.GetFolder(sessionId, null, folderId);
            string targetFolder = Path.Combine(outputFolder, folderId);

            Directory.CreateDirectory(targetFolder);
            folder.Documents.ForEach(doc =>
            {
                ExportDocumentImages(sessionId, doc.Id, targetFolder, extension);
            });
        }


        public string ExportDocumentImages(string sessionId, string documentId, string outputFolder, string extension)
        {
            var cds = new CaptureDocumentService();
            var doc = cds.GetDocument(sessionId, null, documentId);

            Directory.CreateDirectory(outputFolder);
            var fileName = Path.Combine(
                outputFolder,
                doc.Id + extension);

            using (var fs = File.Create(fileName))
            {
                var s = cds.GetDocumentFile(sessionId, null, doc.Id, "");
                s.CopyTo(fs);
            }
            return fileName;
        }


        public void ExportFolderJson(string sessionId, string folderId, string outputFolder)
        {
            var serializer = new DataContractJsonSerializer(typeof(ExportDocument));
            var cds = new CaptureDocumentService();
            var exportDoc = new ExportDocument();
            var folder = cds.GetFolder(sessionId, null, folderId);
            string targetFolder = Path.Combine(outputFolder, folderId);

            exportDoc.FolderFields = folder.Fields;
            Directory.CreateDirectory(targetFolder);
            folder.Documents.ForEach(doc =>
            {
                ExportDocumentJson(sessionId, doc.Id, targetFolder);
            });
        }

        public string ExportDocumentJson(string sessionId, string documentId, string outputFolder)
        {
            var serializer = new DataContractJsonSerializer(typeof(ExportDocument));
            var cds = new CaptureDocumentService();
            var exportDoc = new ExportDocument();
            var doc = cds.GetDocument(sessionId, null, documentId);
            string folderId = doc.ParentId;
            var folder = cds.GetFolder(sessionId, null, folderId);
            string targetFolder = outputFolder;

            exportDoc.FolderFields = folder.Fields;
            Directory.CreateDirectory(targetFolder);
            exportDoc.Fields = doc.Fields;
            var fileName = Path.Combine(
                targetFolder,
                doc.Id + ".json");

            using (var fs = File.Create(fileName))
            {
                serializer.WriteObject(fs, exportDoc);
            }

            return fileName;

        }
    }
}
