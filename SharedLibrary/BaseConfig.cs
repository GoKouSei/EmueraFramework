using System;
using System.Linq;
using System.Reflection;
using YeongHun.Common.Config;

namespace YeongHun.EmueraFramework
{
    public abstract class LoadableConfig
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class LoadableFieldAttribute : Attribute
        {
            public string Key { get; set; } = null;
            public string DefaultValue { get; set; } = null;
        }
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        public class LoadablePropertyAttribute : Attribute
        {
            public string Key { get; set; } = null;
            public string DefaultValue { get; set; } = null;
        }

        protected abstract void AddParser(ConfigDic config);

        public void Load(ConfigDic config)
        {
            AddParser(config);
            Type type = GetType();
            string tag = type.Name;
            var fields = type.GetRuntimeFields().Where(field => field.IsDefined(typeof(LoadableFieldAttribute)));
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<LoadableFieldAttribute>();
                var key = attr.Key ?? field.Name;
                try
                {
                    if (!config.HasTag(tag))
                    {
                        config.AddTag(tag);
                    }
                    if (!config.HasKey(tag, key))
                    {
                        if (attr.DefaultValue != null)
                            config[tag, key] = attr.DefaultValue;
                        else
                            continue;
                    }
                    field.SetValue(this,
                        typeof(ConfigDic).GetRuntimeMethod("GetValue", new[] { typeof(string), typeof(string) })
                        .MakeGenericMethod(field.FieldType)
                        .Invoke(config, new object[] { tag, key }));
                }
                catch
                {
                    continue;
                }
            }
            var properties = type.GetRuntimeProperties().Where(property => property.IsDefined(typeof(LoadablePropertyAttribute)) && property.SetMethod != null);
            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<LoadablePropertyAttribute>();
                var key = attr.Key ?? property.Name;
                try
                {
                    if (!config.HasTag(tag))
                    {
                        config.AddTag(tag);
                    }
                    if (!config.HasKey(tag, key))
                    {
                        if (attr.DefaultValue != null)
                            config[tag, key] = attr.DefaultValue;
                        else
                            continue;
                    }
                    property.SetValue(this,
                        typeof(ConfigDic).GetRuntimeMethod("GetValue", new[] { typeof(string), typeof(string) })
                        .MakeGenericMethod(property.PropertyType)
                        .Invoke(config, new object[] { tag, key }));
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
