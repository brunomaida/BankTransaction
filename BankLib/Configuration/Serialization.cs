using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankLib.Configuration
{
   /// <summary>
   /// Provides functions to Serialize to XMLFile and Deserialize to XMLObject.
   /// </summary>
   public class Serialization
   {
      /// <summary>
      /// Deserialize the file content into an Serializable Object.
      /// </summary>
      /// <typeparam name="T">The object expected.</typeparam>
      /// <param name="filepath">The path of the file.</param>
      /// <returns>The XML Object restored.</returns>
      /// <exception cref="FileNotFoundException"></exception>
      public static T DeserializeXmlToObject<T>(string filepath)
      {
         if (!System.IO.File.Exists(filepath))
         {
            throw new FileNotFoundException("File to deserialize from XML does not exit", filepath);
         }
         filepath = filepath.Replace("\\\\", "\\");
         using (var fs = new FileStream(filepath, FileMode.Open))
         {
            return DeserializeXmlToObject<T>(fs);
         }
      }

		/// <summary>
		/// Deserialize the file content into an Serializable Object.
		/// </summary>
		/// <typeparam name="T">The object expected.</typeparam>
		/// <param name="source">The FileStream used to read content from.</param>
		/// <returns>The XML Object restored.</returns>
		public static T DeserializeXmlToObject<T>(FileStream source)
      {
         if (source == null)
         {
            return default;
         }
         return DeserializeXmlToObject<T>((Stream)source);
      }

		/// <summary>
		/// Deserialize the file content into an Serializable Object.
		/// </summary>
		/// <typeparam name="T">The object expected.</typeparam>
		/// <param name="source">The FileStream used to read content from.</param>
		/// <returns>The XML Object restored.</returns>
		public static T DeserializeXmlToObject<T>(Stream source)
      {
         source.ThrowIfNull("source");
         var x = new XmlSerializer(typeof(T));
         var o = x.Deserialize(source);
			return (T)o;
      }

      /// <summary>
      /// Serialize an object to a XML file.
      /// </summary>
      /// <param name="data">The Serializable object.</param>
      /// <param name="filepath">The path of the destination file.</param>
      /// <param name="fileMode">Append or Create new file.</param>
      public static void SerializeToXml(object data, string filepath, FileMode fileMode)
      {
         filepath.ThrowIfNull("filepath");
         filepath = filepath.Replace("\\\\", "\\");
         using (var fs = new FileStream(filepath, fileMode))
         {
            SerializeToXml(data, fs);
            fs.Close();
         }
      }

		/// <summary>
		/// Serialize an object to a XML file.
		/// </summary>
		/// <param name="data">The Serializable object.</param>
		/// <param name="destination">The file stream to write to.</param>
		public static void SerializeToXml(object data, FileStream destination)
      {
         SerializeToXml(data, (Stream)destination);
      }

		/// <summary>
		/// Serialize an object to a XML file.
		/// </summary>
		/// <param name="data">The Serializable object.</param>
		/// <param name="destination">The stream to write to.</param>
		public static void SerializeToXml(object data, Stream destination)
      {
         data.ThrowIfNull("data");
         if (!(data.GetType().IsDefined(typeof(SerializableAttribute), false)))
         {
            throw new InvalidOperationException("object is not Serializable");
         }
         var x = new XmlSerializer(data.GetType());
         x.Serialize(destination, data);
      }
   }
}
