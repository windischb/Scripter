using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reflectensions;

namespace ScripterTestCmd
{
    public class VariableRepository: IVariablesRepository
    {
        public ITreeNode GetVariable(string parent, string name)
        {
            var treenode = new TreeNode();
            treenode.Parent = parent;
            treenode.Name = name;
            treenode.Content = Json.Converter.ToJToken("{\r\n    \"VKZ\": \"BMI\",\r\n    \"BereichsKennung\": \"urn:publicid:gv.at:cdid+ZP\"\r\n}");

            return treenode;
        }

        public Task<ITreeNode> GetFolderTree()
        {
            throw new NotImplementedException();
        }
    }
}
