using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ClassHelper
{
    internal class Class1
    {
        public static EF.RentEquipmentEntities1 Context { get; } = new EF.RentEquipmentEntities1();
    }
}
