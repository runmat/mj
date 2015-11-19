using System;

namespace SapORM.Contracts
{
    public class BapiField
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public int Length { get; set; }

        public int Decimals { get; set; }
    }
}
