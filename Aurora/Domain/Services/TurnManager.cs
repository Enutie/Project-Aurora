using Aurora.Application.DTO;
using Aurora.Domain.Entities;
using Aurora.Domain.Enums;
using Aurora.Shared.Exceptions;
using Aurora.Shared.Utils;

namespace Aurora.Domain.Services
{
    public class TurnManager
    {
        private readonly GameState _gameState;
        private readonly ICardConverter _cardConverter;

        public TurnManager(GameState gameState, ICardConverter cardConverter)
        {
            _gameState = gameState;
            _cardConverter = cardConverter;
        }

        public void StartGame()
        {
            foreach (Player player in _gameState.Players)
            {
                player.Deck.Shuffle();
                DrawStartingHand(player);
            }
            StartBeginningPhase();
        }

        public void AdvanceToNextPhase()
        {
            switch (_gameState.CurrentPhase)
            {
                case Phase.Beginning:
                    StartMainPhase1();
                    break;
                case Phase.MainPhase1:
                    StartCombatPhase();
                    break;
                case Phase.Combat:
                    StartMainPhase2();
                    break;
                case Phase.MainPhase2:
                    StartEndPhase();
                    break;
                case Phase.Ending:
                    SwitchTurn();
                    break;
                default:
                    throw new InvalidOperationException("Unknown phase.");
            }
        }

        public void PlayLand(string playerId, LandDTO landDTO)
        {
            Console.WriteLine($"Current Phase: {_gameState.CurrentPhase}");
            Console.WriteLine($"Current Player ID: {_gameState.GetCurrentPlayerId()}");
            Console.WriteLine($"Attempting to play land for player: {playerId}");
            Console.WriteLine($"Lands played this turn: {_gameState.LandsPlayedThisTurn[playerId]}");

            if (_gameState.CurrentPhase != Phase.MainPhase1 && _gameState.CurrentPhase != Phase.MainPhase2)
            {
                Console.WriteLine("Invalid phase for playing a land");
                throw new InvalidPhaseException("Can only play a land in first or second main phase");
            }

            var player = _gameState.GetPlayerById(playerId);
            if (_gameState.GetCurrentPlayerId() != playerId)
            {
                Console.WriteLine("Not the current player's turn");
                throw new InvalidMoveException("It's not your turn to play a land.");
            }

            if (!CanPlayLand(playerId))
            {
                Console.WriteLine("Player has already played a land this turn");
                throw new InvalidMoveException("You've already played a land this turn.");
            }

            var land = _cardConverter.ConvertToLand(landDTO);
            Console.WriteLine($"Land in player's hand: {player.Hand.Any(c => c.Id == land.Id)}");

            if (!player.Hand.Any(c => c.Id == land.Id))
            {
                Console.WriteLine("Land not found in player's hand");
                throw new InvalidMoveException("The land you're trying to play is not in your hand.");
            }

            player.PlayLand(land);
            _gameState.LandsPlayedThisTurn[playerId]++;
            Console.WriteLine("Land played successfully");
        }

        private void StartBeginningPhase()
        {
            _gameState.CurrentPhase = Phase.Beginning;
            _gameState.LandsPlayedThisTurn[_gameState.GetCurrentPlayerId()] = 0;
            _gameState.HasAttackedThisTurn = false;
            UntapPermanents(_gameState.GetCurrentPlayer());
            DrawCard(_gameState.GetCurrentPlayer());
        }

        private void StartMainPhase1() => _gameState.CurrentPhase = Phase.MainPhase1;
        private void StartCombatPhase() => _gameState.CurrentPhase = Phase.Combat;
        private void StartMainPhase2() => _gameState.CurrentPhase = Phase.MainPhase2;
        private void StartEndPhase() => _gameState.CurrentPhase = Phase.Ending;

        public void SwitchTurn()
        {
            _gameState.CurrentPlayerIndex = (_gameState.CurrentPlayerIndex + 1) % _gameState.Players.Count;
            StartBeginningPhase();
        }

        private void DrawStartingHand(Player player)
        {
            for (int i = 0; i < 7; i++)
            {
                player.DrawCard();
            }
        }

        private static void UntapPermanents(Player currentPlayer)
        {
            foreach (var card in currentPlayer.Battlefield)
            {
                card.IsTapped = false;
            }
        }

        private void DrawCard(Player player)
        {
            try
            {
                player.DrawCard();
            }
            catch (InvalidOperationException)
            {
                SetWinner(_gameState.Players.Find(p => p != player));
            }
        }

        private void SetWinner(Player player)
        {
            _gameState.IsGameOver = true;
            _gameState.Winner = _cardConverter.ConvertToPlayerDTO(player);
        }

        private bool CanPlayLand(string playerId)
        {
            return _gameState.LandsPlayedThisTurn[playerId] == 0;
        }
    }
}