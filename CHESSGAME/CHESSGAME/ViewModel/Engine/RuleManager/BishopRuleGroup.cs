using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.ViewModel.Engine.Rules;

namespace CHESSGAME.ViewModel.Engine.RuleManager
{
    public class BishopRuleGroup : RuleGroup
    {
        public BishopRuleGroup()
        {
            Rules.Add(new CanOnlyTakeEnemyRule());
            Rules.Add(new BishopMovementRule());
            Rules.Add(new WillNotMakeCheck());
        }
        protected override Type Type => Type.Bishop;
    }
}