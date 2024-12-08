using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.ViewModel.Engine.Rules;

namespace CHESSGAME.ViewModel.Engine.RuleManager
{
    public class QueenRuleGroup : RuleGroup
    {
        public QueenRuleGroup()
        {
            Rules.Add(new QueenMovementRule());
            Rules.Add(new CanOnlyTakeEnemyRule());
            Rules.Add(new WillNotMakeCheck());
        }
        protected override Type Type => Type.Queen;
    }
}