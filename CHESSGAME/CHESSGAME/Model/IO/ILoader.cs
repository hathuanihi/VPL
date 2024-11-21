using CHESSGAME.Model.AModel;

namespace CHESSGAME.Model.IO
{
    public interface ILoader
    {
        Container Load(string path);

        string Filter();
    }
}