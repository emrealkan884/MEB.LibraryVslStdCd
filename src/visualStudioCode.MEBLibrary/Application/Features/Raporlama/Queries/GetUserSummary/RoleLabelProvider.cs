using Application.Authorization;
using System;
using System.Collections.Generic;

namespace Application.Features.Raporlama.Queries.GetUserSummary;

internal static class RoleLabelProvider
{
    private static readonly Dictionary<string, string> RoleLabels = new(StringComparer.OrdinalIgnoreCase)
    {
        [ApplicationRoles.SistemYoneticisi] = "Sistem Yoneticisi",
        [ApplicationRoles.BakanlikYetkilisi] = "Bakanlik Yetkilisi",
        [ApplicationRoles.IlYetkilisi] = "Il Yetkilisi",
        [ApplicationRoles.IlceYetkilisi] = "Ilce Yetkilisi",
        [ApplicationRoles.OkulKutuphaneYoneticisi] = "Okul Kutuphane Yoneticisi"
    };

    public static string GetRoleLabel(string roleKey)
    {
        if (RoleLabels.TryGetValue(roleKey, out string? label))
            return label;

        return roleKey.StartsWith("Role.", StringComparison.OrdinalIgnoreCase)
            ? roleKey["Role.".Length..]
            : roleKey;
    }
}
