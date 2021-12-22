using EasyXrm.Models;

namespace EasyXrm.Tests.Entities.FoobarEntity
{
    public class GenderCode:Enumeration<GenderCode>
    {
        public static readonly GenderCode Male = new GenderCode(1, "Мужчина");
        public static readonly GenderCode Female = new GenderCode(2, "Женщина");


        public GenderCode(int id, string name) : base(id, name)
        {

        }
    }
}
