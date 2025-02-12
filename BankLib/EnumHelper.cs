using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib
{
   public class EnumHelper
   {
      /// <summary>
      /// Returns all values from an Enum sequence in the specified Type.
      /// </summary>
      /// <typeparam name="T">The result type.</typeparam>
      /// <returns>An IEnumerable collection of the specified type</returns>
      /// <exception cref="InvalidOperationException"></exception>
      public static IEnumerable<T> ValuesFrom<T>()
      {
         var t = typeof(T);
         if (typeof(T).IsEnum)
         {
            return Enum.GetValues(t).Cast<T>();
         }
         throw new InvalidOperationException(string.Format("Type {0} is not an ENUM.", t.Name));
      }
   }
}
