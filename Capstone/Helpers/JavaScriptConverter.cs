using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Capstone.Helpers
{
    public static class JavaScriptConvert
    {
        public static IHtmlString SerializeObject(object value)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                var serializer = new JsonSerializer
                {
                    // Let's use camelCasing as is common practice in JavaScript
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                // We don't want quotes around object names
                jsonWriter.QuoteName = false;
                serializer.Serialize(jsonWriter, value);

                return new HtmlString(stringWriter.ToString());
            }
        }
    }
    /*    USAGE EXAMPLE
     *    <script>
             var pirates = @JavaScriptConvert.SerializeObject(Model.Pirates);
            </script>

     * <script>
        var pirates = [{firstName:"Jack",lastName:"Sparrow"},{firstName:"Will",lastName:"Turner"},{firstName:"Elizabeth",lastName:"Swann"}];
        </script>
     * 
     * 
     */
}