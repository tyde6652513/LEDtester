using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.Tester.XML
{
    public class XmlFileSerializer
    {
        public enum ColorFormat
        {
            NamedColor,
            ARGBColor
        }

        public static void Serialize(object data, string filename)
        {
            string path = filename.Substring(0, filename.IndexOf(Path.GetFileName(filename)));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());

                XmlTextWriter xmlTextWriter = new XmlTextWriter(streamWriter);

                xmlSerializer.Serialize(xmlTextWriter, data);

                xmlTextWriter.Close();
            }
        }

        public static void Serialize2(object data, string filename)
        {
            string path = filename.Substring(0, filename.IndexOf(Path.GetFileName(filename)));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream oFileStream = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer oXmlSerializer = new System.Xml.Serialization.XmlSerializer(data.GetType());

                oXmlSerializer.Serialize(oFileStream, data);

                oFileStream.Close();
            }
        }


        public static object Deserialize(Type type, string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }
            object result = null;
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader(filename);
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                XmlTextReader xmlTextReader = new XmlTextReader(streamReader);
                result = xmlSerializer.Deserialize(xmlTextReader);
                xmlTextReader.Close();
            }
            catch
            {
                result = null;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
            return result;
        }
        public static string SerializeColor(Color color)
        {
            if (color.IsNamedColor)
            {
                return string.Format("{0}:{1}", XmlFileSerializer.ColorFormat.NamedColor, color.Name);
            }
            return string.Format("{0}:{1}:{2}:{3}:{4}", new object[]
			{
				XmlFileSerializer.ColorFormat.ARGBColor,
				color.A,
				color.R,
				color.G,
				color.B
			});
        }
        public static Color DeserializeColor(string color)
        {
            string[] array = color.Split(new char[]
			{
				':'
			});
            switch ((XmlFileSerializer.ColorFormat)Enum.Parse(typeof(XmlFileSerializer.ColorFormat), array[0], true))
            {
                case XmlFileSerializer.ColorFormat.NamedColor:
                    return Color.FromName(array[1]);
                case XmlFileSerializer.ColorFormat.ARGBColor:
                    {
                        byte alpha = byte.Parse(array[1]);
                        byte red = byte.Parse(array[2]);
                        byte green = byte.Parse(array[3]);
                        byte blue = byte.Parse(array[4]);
                        return Color.FromArgb((int)alpha, (int)red, (int)green, (int)blue);
                    }
                default:
                    return Color.Empty;
            }
        }
    }
}
