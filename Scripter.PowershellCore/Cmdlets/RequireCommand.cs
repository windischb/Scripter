using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Scripter.Shared;

namespace Scripter.PowershellCore.Cmdlets
{
    [Cmdlet("Require","Module")]
    public class RequireCommand: PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {

            
            var registeredModules = (ScripterModulesProvider)this.GetVariableValue("ModulesProvider");

            WriteObject(registeredModules.GetModule(Name));
        }

    }
}
