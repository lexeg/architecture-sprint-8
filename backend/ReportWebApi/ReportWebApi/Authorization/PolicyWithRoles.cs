namespace ReportWebApi.Authorization;

public class PolicyWithRoles
{
    public string PolicyName { get; set; }

    public string[] Roles { get; set; }
}