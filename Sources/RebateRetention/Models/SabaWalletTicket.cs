namespace RebateRetention.Models;

public class SabaWalletTicket
{
	public int SId { get; set; }
	public int ProgramId { get; set; }

	public string ProgramName => "Euro";
	public int CustId { get; set; }

	public string Username { get; set; }

	public int AgentId { get; set; }

	public string AgentUsername { get; set; }

	public int MasterId { get; set; }

	public string MasterUsername { get; set; }

	public int SuperId { get; set; }

	public string SuperUsername { get; set; }
}