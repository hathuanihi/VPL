using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.ViewModel.Engine.Rules;

namespace CHESSGAME.ViewModel.Engine.RuleManager
{
    public class KnightRuleGroup : RuleGroup
    {
        public KnightRuleGroup()
        {
            Rules.Add(new CanOnlyTakeEnnemyRule());
            Rules.Add(new KnightMovementRule());
            Rules.Add(new WillNotMakeCheck());
        }

        protected override Type Type => Type.Knight;
    }
}