//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sideshow.Adapters
{
    
    public class IModuleInstanceViewToContractAddInAdapter : System.AddIn.Pipeline.ContractBase, Start9.Api.Contracts.IModuleInstanceContract
    {
        private Sideshow.Views.IModuleInstance _view;
        public IModuleInstanceViewToContractAddInAdapter(Sideshow.Views.IModuleInstance view)
        {
            _view = view;
        }
        public Start9.Api.Contracts.IModuleContract Instance
        {
            get
            {
                return Sideshow.Adapters.IModuleAddInAdapter.ViewToContractAdapter(_view.Instance);
            }
        }
        public int ProcessId
        {
            get
            {
                return _view.ProcessId;
            }
        }
        internal Sideshow.Views.IModuleInstance GetSourceView()
        {
            return _view;
        }
    }
}
