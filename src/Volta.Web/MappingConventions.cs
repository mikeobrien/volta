using System;
using Volta.Core.Infrastructure.Framework;

namespace Volta.Web
{
    public class MappingConventions
    {
        private static readonly Action Command = Func.Lazy(true, RegisterMappings);
    
        public static void Register()
        {
            Command();
        }

        private static void RegisterMappings()
        {
            
        }
    }
}