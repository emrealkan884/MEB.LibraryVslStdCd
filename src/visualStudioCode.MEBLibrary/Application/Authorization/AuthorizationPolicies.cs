namespace Application.Authorization;

public static class AuthorizationPolicies
{
    public const string RequireMinistry = "Policy.RequireMinistry";
    public const string RequireProvinceOrAbove = "Policy.RequireProvinceOrAbove";
    public const string RequireDistrictOrAbove = "Policy.RequireDistrictOrAbove";
    public const string RequireSchoolOrAbove = "Policy.RequireSchoolOrAbove";
}
