using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibrary.Models {
    interface IInfoFile {
        byte[] GetInfo();
    }
}
