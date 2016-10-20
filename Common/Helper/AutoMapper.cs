using AutoMapper;

namespace Mn.NewsCms.Common.Helper
{

    public static class AutoMapper
    {
        public static TSecond Map<TFirst, TSecond>(TFirst firstObject) where TSecond : class
        {
            return Mapper.DynamicMap<TFirst, TSecond>(firstObject);
        }
        public static void Map<TFirst, TSecond>(TFirst firstObject, ref TSecond secondObject) where TSecond : class
        {
            secondObject = Mapper.DynamicMap<TFirst, TSecond>(firstObject);
        }
        public static TSecond Map<TSecond>(object firstObject) where TSecond : class,new()
        {
            return Mapper.DynamicMap<TSecond>(firstObject);
        }

        public static void Map(object firstObject, object destionationObject)
        {
            Mapper.DynamicMap(firstObject, destionationObject, firstObject.GetType(), destionationObject.GetType());
        }
    }
}
