using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Application.Helpers
{
    public class SchemaProvider
    {
        public string GetCodeAnalysisSchema()
        {
            var assembly = typeof(SchemaProvider).Assembly;
            var resourceName = "Sentinel.Application.Schemas.CodeAnalysisSchema.json";
            
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new InvalidOperationException($"schema not found: {resourceName}");
            }
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd(); 
        }
    }
}
