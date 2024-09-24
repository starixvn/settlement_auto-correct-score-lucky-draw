using Core.Constants;
using Core.Services;
using CorrectScoreLuckyDraw.Constants;

namespace CorrectScoreLuckyDraw.Models;

public class Ticket
{
	public RefDataService RefDataService { get; set; }

	public int DrawId { get; set; }

	public int MatchId { get; set; }

	public int LeagueId { get; set; }

	public int TypeId { get; set; }

	public DateTime KickOffTime { get; set; }

	public int HomeId { get; set; }

	public int AwayId { get; set; }

	public int CustId { get; set; }

	public string Username { get; set; }

	public int AgentId { get; set; }

	public string AgentUsername { get; set; }

	public int MasterId { get; set; }

	public string MasterUsername { get; set; }

	public int SuperId { get; set; }

	public string SuperUsername { get; set; }

	public string Site { get; set; }

	public string CurrencyName { get; set; }

	public decimal PrizeAmount { get; set; }

	public string VerificationCode { get; set; }

	public DateTime ExpiredDate { get; set; }

	public DateTime CreatedDate { get; set; }

	public bool IsTest { get; set; }

	public string TeamName => $"{RefDataService.GetTeamName(HomeId, ContentLanguage.English)} -vs- {RefDataService.GetTeamName(AwayId, ContentLanguage.English)}";

	public string TeamNameCh => GetTeamName(ContentLanguage.Cantonese);

	public string TeamNameCs => GetTeamName(ContentLanguage.Mandarin);

	public string TeamNameIndo => GetTeamName(ContentLanguage.Indo);

	public string LeagueName => RefDataService.GetLeagueName(LeagueId, ContentLanguage.English);

	public string LeagueNameCh => GetLeagueName(ContentLanguage.Cantonese);

	public string LeagueNameCs => GetLeagueName(ContentLanguage.Mandarin);

	public string LeagueNameIndo => GetLeagueName(ContentLanguage.Indo);

	private string GetTeamName(string languageCode)
	{
		var home = RefDataService.GetTeamName(HomeId, languageCode);
		var away = RefDataService.GetTeamName(AwayId, languageCode);

		return (string.IsNullOrEmpty(home) || string.IsNullOrEmpty(away)) ? TeamName : $"{home} -vs- {away}";
	}

	private string GetLeagueName(string languageCode)
	{
		var value = RefDataService.GetLeagueName(LeagueId, languageCode);

		return (string.IsNullOrEmpty(value)) ? LeagueName : value;
	}
}