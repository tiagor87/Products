using System;

namespace Products.Application.Adapters
{
    public interface IAdapter<TContract, TModel> {
        TContract Convert(TModel model);
        TModel Convert(TContract contract);
    }
}