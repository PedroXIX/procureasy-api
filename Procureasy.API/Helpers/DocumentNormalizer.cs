using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Procureasy.API.Helpers
{
    public class DocumentNormalizer
    {
        public string Normalize(string? documento)
        {
            return string.IsNullOrWhiteSpace(documento)
                ? string.Empty
                : new string(documento.Where(char.IsDigit).ToArray());
        }
    }
}