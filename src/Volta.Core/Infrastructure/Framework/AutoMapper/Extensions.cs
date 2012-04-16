using AutoMapper;

namespace Volta.Core.Infrastructure.Framework.AutoMapper
{
    public static class Extensions
    {
        public static IMappingExpression<TDestination, TSource> ToBidirectional<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            return Mapper.CreateMap<TDestination, TSource>();
        }
    }
}