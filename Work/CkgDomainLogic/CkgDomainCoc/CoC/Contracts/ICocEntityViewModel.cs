using System;
using System.Linq.Expressions;
using CkgDomainLogic.CoC.Models;

namespace CkgDomainLogic.CoC.Contracts
{
    public interface ICocEntityViewModel
    {
        bool InsertMode { get; set; }

        void AddItem(CocEntity newItem);

        CocEntity SaveItem(CocEntity item, Action<string, string> addModelError);

        void ValidateModel(CocEntity model, bool insertMode, Action<Expression<Func<CocEntity, object>>, string> addModelError);
    }
}
