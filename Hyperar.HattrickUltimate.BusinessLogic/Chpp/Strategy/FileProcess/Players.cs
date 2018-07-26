﻿// -----------------------------------------------------------------------
// <copyright file="Players.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
// -----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.BusinessLogic.Chpp.Strategy.FileProcess
{
    using System;
    using System.Globalization;
    using System.Linq;
    using BusinessObjects.App;
    using BusinessObjects.Hattrick.Interface;
    using DataAccess.Database.Interface;
    using Interface;

    /// <summary>
    /// Provides functionality to process Players file.
    /// </summary>
    public class Players : IFileProcessStrategy
    {
        #region Private Fields

        /// <summary>
        /// Zero digit char constant.
        /// </summary>
        private const char Zero = '0';

        /// <summary>
        /// Current culture number decimal separator.
        /// </summary>
        private readonly string decimalSeparator;

        /// <summary>
        /// Database context.
        /// </summary>
        private IDatabaseContext context;

        /// <summary>
        /// Country repository.
        /// </summary>
        private IHattrickRepository<Country> countryRepository;

        /// <summary>
        /// League repository.
        /// </summary>
        private IHattrickRepository<League> leagueRepository;

        /// <summary>
        /// Global season number.
        /// </summary>
        private short seasonNumber;

        /// <summary>
        /// SeniorPlayerAvatar repository.
        /// </summary>
        private IRepository<SeniorPlayerAvatar> seniorPlayerAvatarRepository;

        /// <summary>
        /// Senior Player repository.
        /// </summary>
        private IHattrickRepository<SeniorPlayer> seniorPlayerRepository;

        /// <summary>
        /// Senior Player Season Goals repository.
        /// </summary>
        private IRepository<SeniorPlayerSeasonGoals> seniorPlayerSeasonGoalsRepository;

        /// <summary>
        /// Senior Player Skills repository.
        /// </summary>
        private IRepository<SeniorPlayerSkills> seniorPlayerSkillsRepository;

        /// <summary>
        /// Senior Team repository.
        /// </summary>
        private IHattrickRepository<SeniorTeam> seniorTeamRepository;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Players" /> class.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="countryRepository">Country repository.</param>
        /// <param name="leagueRepository">League repository.</param>
        /// <param name="seniorPlayerAvatarRepository">Senior Player Avatar repository.</param>
        /// <param name="seniorPlayerRepository">Senior Player repository.</param>
        /// <param name="seniorPlayerSeasonGoalsRepository">Senior Player Season Goals repository.</param>
        /// <param name="seniorPlayerSkillsRepository">Senior Player Skills repository.</param>
        /// <param name="seniorTeamRepository">Senior Team repository.</param>
        public Players(
                   IDatabaseContext context,
                   IHattrickRepository<Country> countryRepository,
                   IHattrickRepository<League> leagueRepository,
                   IRepository<SeniorPlayerAvatar> seniorPlayerAvatarRepository,
                   IHattrickRepository<SeniorPlayer> seniorPlayerRepository,
                   IRepository<SeniorPlayerSeasonGoals> seniorPlayerSeasonGoalsRepository,
                   IRepository<SeniorPlayerSkills> seniorPlayerSkillsRepository,
                   IHattrickRepository<SeniorTeam> seniorTeamRepository)
        {
            this.context = context;
            this.countryRepository = countryRepository;
            this.leagueRepository = leagueRepository;
            this.seniorPlayerAvatarRepository = seniorPlayerAvatarRepository;
            this.seniorPlayerRepository = seniorPlayerRepository;
            this.seniorPlayerSeasonGoalsRepository = seniorPlayerSeasonGoalsRepository;
            this.seniorPlayerSkillsRepository = seniorPlayerSkillsRepository;
            this.seniorTeamRepository = seniorTeamRepository;
            this.decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Process Players file.
        /// </summary>
        /// <param name="fileToProcess">File to process.</param>
        public void ProcessFile(IXmlEntity fileToProcess)
        {
            if (fileToProcess == null)
            {
                throw new ArgumentNullException(nameof(fileToProcess));
            }

            var file = fileToProcess as BusinessObjects.Hattrick.Players.Root;

            if (file == null)
            {
                throw new ArgumentException(Localization.Messages.UnexpectedObjectType, nameof(fileToProcess));
            }

            if (file.IsPlayingMatch)
            {
                throw new InvalidOperationException(
                          string.Format(
                                     Localization.Messages.TeamIsPlayingMatchCannotProcessPlayers,
                                     file.Team.TeamName));
            }

            this.seasonNumber = this.leagueRepository.Query()
                                                     .First().CurrentSeason;

            var seniorTeam = this.seniorTeamRepository.GetByHattrickId(file.Team.TeamId);

            var processingDate = DateTime.Now;

            foreach (var curPlayer in file.Team.PlayerList)
            {
                this.ProcessPlayer(curPlayer, seniorTeam.Id, processingDate);
            }

            // Gets the current Team's Players Hattrick ID.
            var fileSeniorPlayerIds = file.Team.PlayerList.Select(sp => sp.PlayerId);

            // Gets the stored Team's Players ID.
            var seniorPlayerIdsToDelete = this.seniorPlayerRepository.Query(sp => !fileSeniorPlayerIds.Contains(sp.HattrickId)
                                                                             && sp.SeniorTeam.HattrickId == file.Team.TeamId)
                                                                   .Select(sp => sp.Id);

            // If there are players to delete.
            if (seniorPlayerIdsToDelete.Any())
            {
                foreach (var curPlayerId in seniorPlayerIdsToDelete)
                {
                    var seniorPlayerToDelete = this.seniorPlayerRepository.GetById(curPlayerId);

                    this.seniorPlayerAvatarRepository.Delete(seniorPlayerToDelete.Avatar.Id);

                    var seasonGoalsIdsToDelete = seniorPlayerToDelete.SeasonGoals.Select(sg => sg.Id)
                                                                                  .ToArray();
                    var skillsIdsToDelete = seniorPlayerToDelete.Skills.Select(s => s.Id)
                                                                       .ToArray();

                    foreach (var curId in seasonGoalsIdsToDelete)
                    {
                        this.seniorPlayerSeasonGoalsRepository.Delete(curId);
                    }

                    foreach (var curId in skillsIdsToDelete)
                    {
                        this.seniorPlayerSkillsRepository.Delete(curId);
                    }

                    this.seniorPlayerRepository.Delete(curPlayerId);
                }

                this.context.Save();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Process Player.
        /// </summary>
        /// <param name="player">Player to process.</param>
        /// <param name="seniorTeamId">Senior Team Id.</param>
        /// <param name="processingDate">Processing date and time.</param>
        private void ProcessPlayer(BusinessObjects.Hattrick.Players.Player player, int seniorTeamId, DateTime processingDate)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            var seniorPlayer = this.seniorPlayerRepository.GetByHattrickId(player.PlayerId);

            if (seniorPlayer == null)
            {
                seniorPlayer = new SeniorPlayer();
            }

            seniorPlayer.Age = decimal.Parse($"{player.Age}{this.decimalSeparator}{player.AgeDays.ToString().PadLeft(3, Zero)}");
            seniorPlayer.Aggressiveness = player.Aggressiveness;
            seniorPlayer.Agreeability = player.Agreeability;
            seniorPlayer.BookingStatus = player.Cards;
            seniorPlayer.CareerGoals = player.CareerGoals;
            seniorPlayer.CareerHattricks = player.CareerHattricks;
            seniorPlayer.Category = player.PlayerCategoryId.HasValue && player.PlayerCategoryId.Value != 0 ? player.PlayerCategoryId : null;
            seniorPlayer.CountryId = this.countryRepository.GetByHattrickId(player.CountryId).Id;
            seniorPlayer.FirstName = player.FirstName;
            seniorPlayer.HasHomegrownBonus = player.MotherClubBonus;
            seniorPlayer.HattrickId = player.PlayerId;
            seniorPlayer.Honesty = player.Honesty;
            seniorPlayer.InjuryStatus = player.InjuryLevel > -1 ? (byte?)player.InjuryLevel : null;
            seniorPlayer.IsOnTransferMarket = player.TransferListed;
            seniorPlayer.LastName = player.LastName;
            seniorPlayer.Leadership = player.Leadership;
            seniorPlayer.MatchesOnJuniorNationalTeam = player.CapsU20;
            seniorPlayer.MatchesOnSeniorNationalTeam = player.Caps;
            seniorPlayer.NickName = string.IsNullOrWhiteSpace(player.NickName) ? null : player.NickName;
            seniorPlayer.PlaysOnNationalTeam = player.NationalTeamId > 0;
            seniorPlayer.SeniorTeamId = seniorTeamId;
            seniorPlayer.ShirtNumber = player.PlayerNumber == 100 ? (byte?)null : player.PlayerNumber;
            seniorPlayer.Specialty = player.Specialty;
            seniorPlayer.Statement = player.Statement;
            seniorPlayer.Wage = Convert.ToInt32(player.Salary);

            if (seniorPlayer.Id == 0)
            {
                this.seniorPlayerRepository.Insert(seniorPlayer);
            }
            else
            {
                this.seniorPlayerRepository.Update(seniorPlayer);
            }

            this.ProcessSkills(
                     player.PlayerForm,
                     player.StaminaSkill,
                     player.Experience,
                     player.Loyalty,
                     player.KeeperSkill.Value,
                     player.DefenderSkill.Value,
                     player.PlaymakerSkill.Value,
                     player.WingerSkill.Value,
                     player.PassingSkill.Value,
                     player.ScorerSkill.Value,
                     player.SetPiecesSkill.Value,
                     Convert.ToInt32(player.TSI),
                     seniorPlayer.Id,
                     processingDate);

            this.ProcessSeasonGoals(
                     player.LeagueGoals,
                     player.CupGoals,
                     player.FriendliesGoals,
                     Convert.ToByte(seniorPlayer.Country.League.CurrentSeason),
                     seniorPlayer.Id);

            this.context.Save();
        }

        /// <summary>
        /// Processes Player season goals.
        /// </summary>
        /// <param name="seriesGoals">Season series goals.</param>
        /// <param name="cupGoals">Season cup goals.</param>
        /// <param name="friendlyGoals">Season friendly goals.</param>
        /// <param name="seasonNumber">Season number.</param>
        /// <param name="seniorPlayerId">Senior Player Id.</param>
        private void ProcessSeasonGoals(byte seriesGoals, byte cupGoals, byte friendlyGoals, byte seasonNumber, int seniorPlayerId)
        {
            var seasonGoals = this.seniorPlayerSeasonGoalsRepository.Query(spsg => spsg.SeniorPlayerId == seniorPlayerId
                                                                                && spsg.Season == seasonNumber)
                                                                    .SingleOrDefault();

            if (seasonGoals == null)
            {
                seasonGoals = new SeniorPlayerSeasonGoals
                {
                    CupGoals = cupGoals,
                    FriendlyGoals = friendlyGoals,
                    Season = seasonNumber,
                    SeniorPlayerId = seniorPlayerId,
                    SeriesGoals = seriesGoals
                };

                this.seniorPlayerSeasonGoalsRepository.Insert(seasonGoals);
            }
            else
            {
                seasonGoals.CupGoals = cupGoals;
                seasonGoals.FriendlyGoals = friendlyGoals;
                seasonGoals.SeriesGoals = seriesGoals;

                this.seniorPlayerSeasonGoalsRepository.Update(seasonGoals);
            }

            this.context.Save();
        }

        /// <summary>
        /// Processes Player skills.
        /// </summary>
        /// <param name="form">Form level.</param>
        /// <param name="stamina">Stamina level.</param>
        /// <param name="experience">Experience level.</param>
        /// <param name="loyalty">Loyalty level.</param>
        /// <param name="keeper">Keeper level.</param>
        /// <param name="defending">Defending level.</param>
        /// <param name="playmaking">Playmaking level.</param>
        /// <param name="winger">Winger level.</param>
        /// <param name="passing">Passing level.</param>
        /// <param name="scoring">Scoring level.</param>
        /// <param name="setPieces">Set Pieces level.</param>
        /// <param name="totalSkillIndex">Total Skill Index.</param>
        /// <param name="seniorPlayerId">Senior Player Id.</param>
        /// <param name="processingDate">Processing date and time.</param>
        private void ProcessSkills(
                         byte form,
                         byte stamina,
                         byte experience,
                         byte loyalty,
                         byte keeper,
                         byte defending,
                         byte playmaking,
                         byte winger,
                         byte passing,
                         byte scoring,
                         byte setPieces,
                         int totalSkillIndex,
                         int seniorPlayerId,
                         DateTime processingDate)
        {
            var skills = this.seniorPlayerSkillsRepository.Query(sps => sps.SeniorPlayerId == seniorPlayerId)
                                                          .OrderByDescending(sps => sps.UpdatedOn)
                                                          .FirstOrDefault();

            bool shouldInsert = skills == null ||
                                (skills.Form != form ||
                                 skills.Stamina != stamina ||
                                 skills.Keeper != keeper ||
                                 skills.Defending != defending ||
                                 skills.Playmaking != playmaking ||
                                 skills.Winger != winger ||
                                 skills.Passing != passing ||
                                 skills.Scoring != scoring ||
                                 skills.SetPieces != setPieces ||
                                 skills.Loyalty != loyalty ||
                                 skills.Experience != experience ||
                                 skills.TotalSkillIndex != totalSkillIndex);

            if (shouldInsert)
            {
                skills = new SeniorPlayerSkills
                {
                    Defending = defending,
                    Experience = experience,
                    Form = form,
                    Keeper = keeper,
                    Loyalty = loyalty,
                    Passing = passing,
                    Playmaking = playmaking,
                    Scoring = scoring,
                    SetPieces = setPieces,
                    Stamina = stamina,
                    Winger = winger,
                    TotalSkillIndex = totalSkillIndex,
                    UpdatedOn = processingDate,
                    SeniorPlayerId = seniorPlayerId
                };

                this.seniorPlayerSkillsRepository.Insert(skills);

                this.context.Save();
            }
        }

        #endregion Private Methods
    }
}