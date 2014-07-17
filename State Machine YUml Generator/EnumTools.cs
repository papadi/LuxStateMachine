using System;
using System.Collections.Generic;
using System.Linq;

namespace LuxStateMachine.YUmlGenerator
{
    public class EnumTools
    {
        /// <summary>
        /// A typed implementation of <see cref="Enum.GetValues"/> method
        /// </summary>
        /// <typeparam name="TEnumType">An enum type</typeparam>
        public static IEnumerable<TEnumType> GetValues<TEnumType>()
        {
            if (!typeof(TEnumType).IsEnum) throw new InvalidOperationException("Expected enum type");
            return from object value in Enum.GetValues(typeof(TEnumType)) select (TEnumType)value;
        }
    }
}
