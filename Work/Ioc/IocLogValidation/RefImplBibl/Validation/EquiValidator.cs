using FluentValidation;
using RefImplBibl.Interfaces;
using RefImplBibl.Models;
using RefImplBibl.Resources;

namespace RefImplBibl.Validation
{
    public class EquiValidator : AbstractValidator<EQUI>
    {
        private readonly ISap _sap;

        public EquiValidator(ISap sap)
        {
            _sap = sap;
            RuleFor(x => x.NAME).NotEmpty().WithName(Properties.EQUI_NAME);
            RuleFor(x => x.EQUI_NR).NotNull().WithName(Properties.EQUI_EQUI_NR);
        }
    }
}
