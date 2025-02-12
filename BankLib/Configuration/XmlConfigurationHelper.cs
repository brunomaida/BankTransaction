using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace BankLib.Configuration
{
   /// <summary>
   /// Provides static methods to Read from Xml files and Write to Xml file.
   /// </summary>
   public class XmlConfigurationHelper
   {
      /// <summary>
      /// Loads content from the file into the Serializable object.
      /// </summary>
      /// <typeparam name="T">Type of the object.</typeparam>
      /// <param name="filePath">The file path to read from.</param>
      /// <returns>A serializable object.</returns>
      public static T Load<T>(string filePath)
      {
         var cfg = Serialization.DeserializeXmlToObject<T>(filePath);
         return cfg;
      }

      /// <summary>
      /// Saves content from a Serializable object into a XML file.
      /// </summary>
      /// <typeparam name="T">Type of the object</typeparam>
      /// <param name="settings">The Seriable object.</param>
      /// <param name="filePath">The file path to write to.</param>
      public static void Save<T>(T settings, string filePath)
      {
         Serialization.SerializeToXml(settings, filePath, FileMode.Create);
      }
   }
}
