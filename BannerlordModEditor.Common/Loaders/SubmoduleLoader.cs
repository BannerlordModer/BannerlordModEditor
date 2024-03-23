using BannerlordModEditor.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BannerlordModEditor.Common.Loaders {
    public class SubmoduleLoader {
        public Module LoadSubmodule(string path) {
            XmlSerializer serializer = new XmlSerializer(typeof(Module));
            using FileStream stream = File.OpenRead(path);
            var module = serializer.Deserialize(stream) as Module;
            return module;
        }

        public async Task SaveSubmoduleAsync(Module data, string path) {
            XmlSerializer serializer = new XmlSerializer(typeof(Module));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, data);
            string xmlString = stringWriter.ToString();

            await File.WriteAllTextAsync(path, xmlString);
        }
    }
}
