using System.Text;
using AutoAct.Entities;

namespace AutoAct.Interfaces
{
    public interface IHerstellerSteuerung
    {
        void GetHerstellerAndModel(string herstellerAngaben, Vehicle vehicle, StringBuilder sb);
    }
}
