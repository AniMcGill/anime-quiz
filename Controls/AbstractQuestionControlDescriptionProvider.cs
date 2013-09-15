using System;
using System.ComponentModel;

namespace Anime_Quiz_3.Controls
{
    /// <summary>
    /// http://stackoverflow.com/questions/6817107/abstract-usercontrol-inheritance-in-visual-studio-designer
    /// </summary>
    /// <typeparam name="TAbstract"></typeparam>
    /// <typeparam name="TBase"></typeparam>
    class AbstractQuestionControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public AbstractQuestionControlDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(TAbstract))
                return typeof(TBase);
            return base.GetReflectionType(objectType, instance);
        }
        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
                objectType = typeof(TBase);
            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }
}
