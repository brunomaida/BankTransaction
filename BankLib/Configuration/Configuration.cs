using System.Collections;
using System.Xml.Serialization;

namespace BankLib.Configuration
{
   /// <summary>
   /// Represents an Abstract instance of a basic Configuration Engine that loads and saves information
   /// </summary>
   [Serializable]
   [XmlType("configuration")]
   public abstract class Configuration
   {
      private string _filePath;

      internal Configuration()
         : this("") { }

      /// <summary>
      /// Inititalizes the object
      /// </summary>
      /// <param name="filePath">The configuration file</param>
      public Configuration(string filePath)
      {
         FilePath = filePath;
      }

      /// <summary>
      /// Gets the Directory where the file is located.
      /// </summary>
      [XmlIgnore]
      public string FileDirectory
      {
         get
         {
            if (FilePath == null) return "";
            if (FilePath.Length == 0) return "";
            return Path.GetDirectoryName(FilePath);
         }
      }

      /// <summary>
      /// Returns if the File exists.
      /// </summary>
      [XmlIgnore]
      public bool FileExists
      {
         get
         {
            return System.IO.File.Exists(FilePath);
         }
      }

      /// <summary>
      /// Gets/Sets the path of the configuration file.
      /// </summary>
      /// <value>The new path of the file.</value>
      /// <returns>The path of the file.</returns>
      [XmlIgnore]
      public string FilePath
      {
         get => _filePath;
         set { value.ThrowIfNull("FilePath"); _filePath = value; }
      }

		/// <summary>
		/// Loads the configuration from a source file.
		/// </summary>
		public abstract void Load();

      /// <summary>
      /// Saves the configuration into a file.
      /// </summary>
      public abstract void Save();

   }
}