using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Mono.Cecil;

namespace OSGi.NET.Utils
{
    /// <summary>
    /// 
    /// </summary>
    class MonoAssemblyResolver
    {
        /// <summary>
        /// 程序集定义
        /// </summary>
        private AssemblyDefinition assembly;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="assemblyFile">程序集文件</param>
        public void Init(string assemblyFile)
        {
            assembly = AssemblyDefinition.ReadAssembly(assemblyFile);
        }


    }

    /// <summary>
    /// Mono反射器
    /// </summary>
    class MonoReflector
    {
        /// <summary>
        /// 获取程序集反射器
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public MonoAssemblyReflector LoadAssembly(string path)
        {
            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(Path.GetDirectoryName(path));
            var reader = new ReaderParameters()
            {
                AssemblyResolver = resolver
            };

            var assembly = AssemblyDefinition.ReadAssembly(path, reader);
            return new MonoAssemblyReflector(assembly);
        }
    }
    /// <summary>
    /// 程序集反射器
    /// </summary>
    class MonoAssemblyReflector
    {
        private AssemblyDefinition _assembly;

        public MonoAssemblyReflector(AssemblyDefinition assembly)
        {
            _assembly = assembly;
        }

        public IEnumerable<MonoAttributeReflector> GetAttributes<T>() where T : Attribute
        {
            if (_assembly.HasCustomAttributes)
            {
                var expectedTypeName = typeof(T).Name;
                return _assembly.CustomAttributes
                    .Where(a => a.AttributeType.Name == expectedTypeName)
                    .Select(a => new MonoAttributeReflector(a))
                    .ToList();
            }
            else
            {
                return new MonoAttributeReflector[] { };
            }
        }

        public IEnumerable<MonoTypeReflector> GetTypes()
        {
            var result = new List<MonoTypeReflector>();
            var modules = _assembly.Modules;
            foreach (var module in modules)
            {
                var types = module.GetTypes();
                foreach (var type in types)
                {
                    result.Add(new MonoTypeReflector(type));
                }
            }
            return result;
        }

        public string Location
        {
            get
            {
                return _assembly.MainModule.FullyQualifiedName;
            }
        }

        public string FileName
        {
            get
            {
                return _assembly.MainModule.Name;
            }
        }

        public string FullName
        {
            get
            {
                return _assembly.FullName;
            }
        }
    }
    /// <summary>
    /// 类型反射器
    /// </summary>
    class MonoTypeReflector
    {
        private TypeDefinition _type;

        public MonoTypeReflector(TypeDefinition type)
        {
            _type = type;
        }

        public IEnumerable<MonoTypeReflector> GetInterfaces()
        {
            return _type.Interfaces.Select(i => new MonoTypeReflector(i.Resolve()));
        }

        public IEnumerable<MonoAttributeReflector> GetAttributes<T>() where T : Attribute
        {
            if (_type.HasCustomAttributes)
            {
                var expectedTypeName = typeof(T).Name;
                return _type.CustomAttributes
                    .Where(a => a.AttributeType.Name == expectedTypeName)
                    .Select(a => new MonoAttributeReflector(a))
                    .ToList();
            }
            else
            {
                return new MonoAttributeReflector[] { };
            }
        }

        public string FullName
        {
            get
            {
                return _type.FullName;
            }
        }

        public string Name
        {
            get
            {
                return _type.Name;
            }
        }
    }

    /// <summary>
    /// 程序集特性反射器
    /// </summary>
    class MonoAttributeReflector
    {
        /// <summary>
        /// 自定义特性
        /// </summary>
        private CustomAttribute _attribute;
        /// <summary>
        /// 
        /// </summary>
        private IDictionary<string, string> _values;

        public MonoAttributeReflector(CustomAttribute attribute)
        {
            _attribute = attribute;
        }

        public IDictionary<string, string> Values
        {
            get
            {
                if (_values == null)
                {
                    _values = new Dictionary<string, string>();
                    var constructorArguments = _attribute.Constructor.Resolve().Parameters.Select(p => p.Name).ToList();
                    var constructorParameters = _attribute.ConstructorArguments.Select(a => a.Value.ToString()).ToList();
                    for (var i = 0; i < constructorArguments.Count; i++)
                    {
                        _values.Add(constructorArguments[i], constructorParameters[i]);
                    }
                    foreach (var prop in _attribute.Properties)
                    {
                        _values.Add(prop.Name, prop.Argument.Value.ToString());
                    }
                }
                return _values;
            }
        }
    }
}

