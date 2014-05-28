using System;

namespace CKG.Components.Zulassung.DAL
{
    public class AddressData : IEquatable<AddressData>
    {
        public string Country { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Description { get; set; }

        public AddressData()
        {
            this.Country = "DE";
        }

        public static bool operator ==(AddressData a, AddressData b)
        {
            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }
            else
            {
                if (Object.ReferenceEquals(a, null))
                {
                    return false;
                }
                else
                {
                    return a.Equals(b);
                }
            }
        }

        public static bool operator !=(AddressData a, AddressData b)
        {
            if (Object.ReferenceEquals(a, b))
            {
                return false;
            }
            else
            {
                if (Object.ReferenceEquals(a, null))
                {
                    return true;
                }
                else
                {
                    return !a.Equals(b);
                }
            }
        }

        public bool Equals(AddressData other)
        {
            return (other != null) &&
                   String.Equals(this.Country, other.Country, StringComparison.CurrentCultureIgnoreCase) &&
                   String.Equals(this.Name1, other.Name1, StringComparison.CurrentCultureIgnoreCase) &&
                   String.Equals(this.Name2, other.Name2, StringComparison.CurrentCultureIgnoreCase) &&
                   String.Equals(this.City, other.City, StringComparison.CurrentCultureIgnoreCase) &&
                   String.Equals(this.ZipCode, other.ZipCode, StringComparison.CurrentCultureIgnoreCase) &&
                   String.Equals(this.Street, other.Street, StringComparison.CurrentCultureIgnoreCase) &&
                   String.Equals(this.Description, other.Description, StringComparison.CurrentCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as AddressData);
        }

        public override int GetHashCode()
        {
            return String.Concat(new string[]
                {
                    this.Country,
                    this.Name1,
                    this.Name2,
                    this.City,
                    this.ZipCode,
                    this.Street,
                    this.Description
                }).GetHashCode();
        }
    }
}