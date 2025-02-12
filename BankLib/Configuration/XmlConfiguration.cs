using System;
using System.IO;

namespace BankLib.Configuration
{
   /// <summary>
   /// A Serializable base Xml Configuration class.
   /// </summary>
   [Serializable]
   public abstract class XmlConfiguration : Configuration
   {
      protected XmlConfiguration()
      { }

      /// <summary>
      /// Initializes a new instance of XmlConfiguration.
      /// </summary>
      /// <param name="filePath">The file path to read/save the configuration.</param>
      protected XmlConfiguration(string filePath)
         : base(filePath) { }

      /// <summary>
      /// Loads the configuration from the specified file to the a Serializable object.
      /// </summary>
      /// <typeparam name="T">The Serializable object able o receive file data.</typeparam>
      /// <returns>The Serializable object with data.</returns>
      public virtual T Load<T>()
      {
         var settings = XmlConfigurationHelper.Load<T>(FilePath);
         return settings;
      }

      /// <summary>
      /// Saves this object's content into a new File.
      /// </summary>
      public override void Save()
      {
         Serialization.SerializeToXml(this, FilePath, FileMode.Create);
      }

		/// <summary>
		/// Saves this object's content into a new File.
		/// </summary>
		/// <typeparam name="T">The Serializable Type able o receive file data.</typeparam>
		/// <param name="data">The Serializable data.</param>
		public virtual void Save<T>(T data)
      {
         Serialization.SerializeToXml(data, FilePath, FileMode.Create);
      }
   }
}
