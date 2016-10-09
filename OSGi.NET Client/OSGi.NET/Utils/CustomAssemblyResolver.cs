using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mono.Cecil;

namespace OSGi.NET.Utils
{
    /// <summary>
    /// 
    /// </summary>
    internal class CustomAssemblyResolver : BaseAssemblyResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public delegate AssemblyDefinition ResolveAssemblyDefinitionDelegate(string assemblyName);

        /// <summary>
        /// 
        /// </summary>
        private readonly ResolveAssemblyDefinitionDelegate resolveAssemblyDefinition;

        /// <summary>
        /// 
        /// </summary>
        public CustomAssemblyResolver(ResolveAssemblyDefinitionDelegate resolveAssemblyDefinition)
        {
            this.resolveAssemblyDefinition = resolveAssemblyDefinition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyNameReference"></param>
        /// <returns></returns>
        public override AssemblyDefinition Resolve(AssemblyNameReference assemblyNameReference)
        {
            AssemblyDefinition assemblyDefinition = null;
            if (resolveAssemblyDefinition != null)
                assemblyDefinition = resolveAssemblyDefinition(assemblyNameReference.FullName);
            if (assemblyDefinition == null)
                assemblyDefinition = base.Resolve(assemblyNameReference);
            return assemblyDefinition;
        }
    }
}
