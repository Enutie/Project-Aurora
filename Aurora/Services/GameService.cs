using Aurora.DTO;
using Aurora.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aurora.Services
{
    public class GameService : IGameService
    {
        private readonly ILogger<GameService> _logger;
        private readonly Dictionary<string, Game> _games = new Dictionary<string, Game>();

        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
        }

        public GameDTO StartGame(List<PlayerDTO> playerDTOs)
        {
            try
            {
                var players = playerDTOs.Select(p => new Player
                {
                    Id = p.Id,
                    Name = p.Name,
                    Life = p.Life
                }).ToList();

                var game = new Game(players);
                _games[game.Id] = game;

                return new GameDTO
                {
                    Id = game.Id,
                    Players = game.Players.Select(p => new PlayerDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Hand = p.Hand.Select(c => new CardDTO { Id = c.Id, Name = c.Name }).ToList(),
                        Battlefield = p.Battlefield.Select(c => new CardDTO { Id = c.Id, Name = c.Name }).ToList(),
                        Life = p.Life
                    }).ToList(),
                    CurrentPlayerIndex = game.currentPlayerIndex,
                    IsGameOver = game.IsGameOver,
                    Winner = game.Winner != null ? new PlayerDTO
                    {
                        Id = game.Winner.Id,
                        Name = game.Winner.Name,
                        Life = game.Winner.Life
                    } : null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start the game.");
                throw new InvalidOperationException("Failed to start the game.", ex);
            }
        }

        public GameDTO PlayLand(string gameId, string playerId, LandDTO landDTO)
        {
            try
            {
                if (!_games.TryGetValue(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }

                var player = game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    throw new PlayerNotFoundException($"Player with ID '{playerId}' not found in the game.");
                }

                var land = new Land((LandType)Enum.Parse(typeof(LandType), landDTO.Type.ToString()))
                {
                    Id = landDTO.Id,
                    IsTapped = landDTO.IsTapped
                };

                game.PlayLand(player, land);
                return GetGameState(gameId);
            }
            catch (GameNotFoundException ex)
            {
                _logger.LogError(ex, $"Game with ID '{gameId}' not found.");
                throw;
            }
            catch (PlayerNotFoundException ex)
            {
                _logger.LogError(ex, $"Player with ID '{playerId}' not found in the game.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid move. " + ex.Message);
                throw new InvalidMoveException("Invalid move. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while playing a land.");
                throw new InvalidOperationException("An error occurred while playing a land.", ex);
            }
        }

        public GameDTO CastCreature(string gameId, string playerId, CreatureDTO creatureDTO)
        {
            try
            {
                if (!_games.TryGetValue(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }

                var player = game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    throw new PlayerNotFoundException($"Player with ID '{playerId}' not found in the game.");
                }

                var creature = new Creature(creatureDTO.Name, creatureDTO.ManaCost, creatureDTO.Power, creatureDTO.Toughness)
                {
                    Id = creatureDTO.Id,
                    IsAttacking = creatureDTO.IsAttacking,
                    IsBlocked = creatureDTO.IsBlocked,
                    BlockedBy = creatureDTO.BlockedBy != null ? new Creature(creatureDTO.BlockedBy.Name, creatureDTO.BlockedBy.ManaCost, creatureDTO.BlockedBy.Power, creatureDTO.BlockedBy.Toughness)
                    {
                        Id = creatureDTO.BlockedBy.Id
                    } : null
                };

                game.CastCreature(player, creature);
                return GetGameState(gameId);
            }
            catch (GameNotFoundException ex)
            {
                _logger.LogError(ex, "Game not Found");
                throw;
            }
            catch (PlayerNotFoundException ex)
            {
                _logger.LogError(ex, "Player not Found");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid Move" + ex.Message);
                throw new InvalidMoveException("Invalid move. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while casting a creature.");
                throw new InvalidOperationException("An error occurred while casting a creature.", ex);
            }
        }

        public GameDTO Attack(string gameId, string attackingPlayerId, List<string> attackingCreatureIds)
        {
            try
            {
                if (!_games.TryGetValue(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }

                var attackingPlayer = game.Players.FirstOrDefault(p => p.Id == attackingPlayerId);
                if (attackingPlayer == null)
                {
                    throw new PlayerNotFoundException($"Attacking player with ID '{attackingPlayerId}' not found in the game.");
                }

                var attackingCreatures = attackingPlayer.Battlefield.OfType<Creature>()
                    .Where(c => attackingCreatureIds.Contains(c.Id)).ToList();

                game.DeclareAttackers(attackingPlayer, attackingCreatures);

                return GetGameState(gameId);
            }
            catch (GameNotFoundException ex)
            {
                _logger.LogError(ex, "Game not Found");
                throw;
            }
            catch (PlayerNotFoundException ex)
            {
                _logger.LogError(ex, "Player not Found");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid move. " + ex.Message);
                throw new InvalidMoveException("Invalid move. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attacking.");
                throw new InvalidOperationException("An error occurred while attacking.", ex);
            }
        }

        public GameDTO AssignBlockers(string gameId, string defendingPlayerId, Dictionary<string, string> blockerAssignments)
        {
            try
            {
                if (!_games.TryGetValue(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }

                var defendingPlayer = game.Players.FirstOrDefault(p => p.Id == defendingPlayerId);
                if (defendingPlayer == null)
                {
                    throw new PlayerNotFoundException($"Defending player with ID '{defendingPlayerId}' not found in the game.");
                }

                var blockingCreatures = new Dictionary<Creature, Creature>();
                foreach (var assignment in blockerAssignments)
                {
                    var attackingCreature = game.Players.SelectMany(p => p.Battlefield).OfType<Creature>().FirstOrDefault(c => c.Id == assignment.Key);
                    var blockingCreature = defendingPlayer.Battlefield.OfType<Creature>().FirstOrDefault(c => c.Id == assignment.Value);

                    if (attackingCreature != null && blockingCreature != null)
                    {
                        blockingCreatures[attackingCreature] = blockingCreature;
                    }
                }

                game.AssignBlockers(defendingPlayer, blockingCreatures);

                return GetGameState(gameId);
            }
            catch (GameNotFoundException ex)
            {
                _logger.LogError(ex, "Game not found");
                throw;
            }
            catch (PlayerNotFoundException ex)
            {
                _logger.LogError(ex, "Player not found");
                throw;
            }
        }

        public GameDTO SwitchTurn(string gameId)
        {
            try
            {
                if (!_games.TryGetValue(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }

                game.SwitchTurn();
                return GetGameState(gameId);
            }
            catch (GameNotFoundException ex)
            {
                _logger.LogError(ex, "Game not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while switching turns.");
                throw new InvalidOperationException("An error occurred while switching turns.", ex);
            }
        }

        public GameDTO GetGameState(string gameId)
        {
            try
            {
                if (!_games.TryGetValue(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }

                return new GameDTO
                {
                    Id = game.Id,
                    Players = game.Players.Select(p => new PlayerDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Hand = p.Hand.Select(c => new CardDTO { Id = c.Id, Name = c.Name }).ToList(),
                        Battlefield = p.Battlefield.Select(c => new CardDTO { Id = c.Id, Name = c.Name }).ToList(),
                        Life = p.Life
                    }).ToList(),
                    CurrentPlayerIndex = game.currentPlayerIndex,
                    IsGameOver = game.IsGameOver,
                    Winner = game.Winner != null ? new PlayerDTO
                    {
                        Id = game.Winner.Id,
                        Name = game.Winner.Name,
                        Life = game.Winner.Life
                    } : null
                };
            }
            catch (GameNotFoundException ex)
            {
                _logger.LogError(ex, "Game not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the game state.");
                throw new InvalidOperationException("An error occurred while retrieving the game state.", ex);
            }
        }

        public PlayerDTO GetPlayerInfo(string gameId, string playerId)
        {
            try
            {
                if (!_games.TryGetValue(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }

                var player = game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    throw new PlayerNotFoundException($"Player with ID '{playerId}' not found in the game.");
                }

                return new PlayerDTO
                {
                    Id = player.Id,
                    Name = player.Name,
                    Hand = player.Hand.Select(c => new CardDTO { Id = c.Id, Name = c.Name }).ToList(),
                    Battlefield = player.Battlefield.Select(c => new CardDTO { Id = c.Id, Name = c.Name }).ToList(),
                    Life = player.Life
                };
            }
            catch (GameNotFoundException ex)
            {
                _logger.LogError(ex, "Game not found");
                throw;
            }
            catch (PlayerNotFoundException ex)
            {
                _logger.LogError(ex, "Player not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving player information.");
                throw new InvalidOperationException("An error occurred while retrieving player information.", ex);
            }
        }
    }
}
