using System.Threading.Tasks;

namespace ScripterTestCmd
{
    public interface IVariablesRepository
    {
        ITreeNode GetVariable(string parent, string name);
        Task<ITreeNode> GetFolderTree();
    }
}