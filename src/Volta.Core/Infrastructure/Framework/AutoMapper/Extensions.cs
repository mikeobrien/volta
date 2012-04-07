using AutoMapper;

namespace Volta.Core.Infrastructure.Framework.AutoMapper
{
    public static class Extensions
    {
        public static void IsBidirectional<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            Mapper.CreateMap<TDestination, TSource>();
        }
    }
}