using Aurora.Domain.Entities;
using Aurora.Domain.Enums;
using Aurora.Shared.Exceptions;

namespace Aurora.Domain.Services
{
    public class CombatManager
    {
        private readonly GameState _gameState;
        private Player _attackingPlayer;
        private List<Creature> _attackingCreatures;
        private Dictionary<Creature, Creature> _blockingCreatures;

        public CombatManager(GameState gameState)
        {
            _gameState = gameState;
        }

        public void DeclareAttackers(Player attackingPlayer, List<Creature> attackingCreatures)
        {
            if (_gameState.CurrentPhase != Phase.Combat)
            {
                throw new InvalidPhaseException("Can only declare attackers in the combat phase.");
            }

            foreach (var creature in attackingCreatures)
            {
                if (creature.IsTapped)
                {
                    throw new InvalidOperationException("Cannot declare a tapped creature as an attacker.");
                }
                creature.IsAttacking = true;
                creature.IsTapped = true;
            }

            _attackingPlayer = attackingPlayer;
            _attackingCreatures = attackingCreatures;
        }

        public void AssignBlockers(Player defendingPlayer, Dictionary<Creature, Creature> blockingCreatures)
        {
            if (_gameState.CurrentPhase != Phase.Combat)
            {
                throw new InvalidPhaseException("Can only assign blockers in the combat phase.");
            }

            _blockingCreatures = blockingCreatures;
            ResolveCombat(defendingPlayer);
        }

        private void ResolveCombat(Player defendingPlayer)
        {
            foreach (var attackingCreature in _attackingCreatures)
            {
                if (_blockingCreatures.TryGetValue(attackingCreature, out var blockingCreature))
                {
                    attackingCreature.DealDamage(blockingCreature);
                    blockingCreature.DealDamage(attackingCreature);
                    RemoveDeadCreature(attackingCreature, _attackingPlayer);
                    RemoveDeadCreature(blockingCreature, defendingPlayer);
                }
                else
                {
                    defendingPlayer.TakeDamage(attackingCreature.Power);
                }
                attackingCreature.IsAttacking = false;
            }

            _attackingCreatures.Clear();
            _blockingCreatures.Clear();
            _gameState.HasAttackedThisTurn = true;
        }

        private void RemoveDeadCreature(Creature creature, Player owner)
        {
            if (creature.Toughness <= 0)
            {
                owner.Battlefield.Remove(creature);
                owner.Graveyard.Add(creature);
            }
        }
    }
}