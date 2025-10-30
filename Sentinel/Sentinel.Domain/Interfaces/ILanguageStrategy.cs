using Sentinel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Domain.Interfaces
{
    //MVP would focus only on method level analysis
    // Future Implementraion would involve Class level analysis and property level analysis
    public interface ILanguageStrategy
    {
        List<CodeChunk> ExtractMethods(string code);
        string LanguadeName {  get; }
    }
}
