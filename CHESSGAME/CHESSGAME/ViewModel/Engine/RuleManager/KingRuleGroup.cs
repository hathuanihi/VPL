using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.ViewModel.Engine.Rules;

namespace CHESSGAME.ViewModel.Engine.RuleManager
{
    public class KingRuleGroup : RuleGroup
    {
        public KingRuleGroup()
        {
            Rules.Add(new KingMovementRule());
            Rules.Add(new CanOnlyTakeEnnemyRuleKing());
            Rules.Add(new CastlingRule());
            Rules.Add(new WillNotMakeCheck());
        }

        protected override Type Type => Type.King;
    }
}