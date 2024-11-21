using CHESSGAME.Model.AModel;

namespace CHESSGAME.Model.IO
{
    public interface ISaver
    {
        void Save(Container container, string path);
        string Filter();
    }
}