using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.ViewModel.Engine.Rules;

namespace CHESSGAME.ViewModel.Engine.RuleManager
{
    public class PawnRuleGroup : RuleGroup
    {
        public PawnRuleGroup()
        {
            Rules.Add(new PawnMovementRule());
            Rules.Add(new CanOnlyTakeEnemyRule());
            Rules.Add(new WillNotMakeCheck());
        }
        protected override Type Type => Type.Pawn;
    }
}