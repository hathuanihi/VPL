using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.ViewModel.Engine.Rules;

namespace CHESSGAME.ViewModel.Engine.RuleManager
{
    public class RookRuleGroup : RuleGroup
    {
        public RookRuleGroup()
        {
            Rules.Add(new RookMovementRule());
            Rules.Add(new CanOnlyTakeEnemyRule());
            Rules.Add(new WillNotMakeCheck());
        }
        protected override Type Type => Type.Rook;
    }
}