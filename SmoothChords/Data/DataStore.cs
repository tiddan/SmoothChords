using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmoothChords.ViewModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace SmoothChords.Data
{
    public class DataStore
    {
        // Test seam. Set to true to disable popups.
        protected bool TEST_MODE = false;

        public virtual void Save(DocumentViewModel document)
        {
            if (document.DocumentPath != "")
            {
                SerializeAsFile(document);
            }
            else
            {
                SaveAs(document);
            }
        }

        public virtual void SaveAs(DocumentViewModel document)
        {
            bool? ok;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Data files (*.chords)|*.chords;";

            if (!TEST_MODE)
            {
                ok = saveDialog.ShowDialog();
            }
            else
            {
                ok = true;
            }

            if (ok.HasValue && ok.Value == true)
            {
                document.DocumentPath = saveDialog.FileName;
                SerializeAsFile(document);
            }
        }

        public virtual void Open(ref DocumentViewModel document)
        {
            bool? ok;
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Data files (*.chords)|*.chords;";

            if (!TEST_MODE)
            {
                ok = openDialog.ShowDialog();
            }
            else
            {
                ok = true;
            }

            if (ok.HasValue && ok.Value == true)
            {
                document.DocumentPath = openDialog.FileName;
                OpenSerializedFile(ref document);
            }
        }

        protected virtual void OpenSerializedFile(ref DocumentViewModel document)
        {
            using (Stream fs = new FileStream(document.DocumentPath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(document.GetType());
                document = (DocumentViewModel)serializer.Deserialize(fs);
            }
        }

        protected virtual void SerializeAsFile(DocumentViewModel document)
        {
            using (FileStream fs = new FileStream(document.DocumentPath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(document.GetType());
                serializer.Serialize(fs, document);
            }
        }

    }
}
