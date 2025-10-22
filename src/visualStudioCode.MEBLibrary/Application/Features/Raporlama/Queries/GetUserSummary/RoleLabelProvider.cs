using Application.Authorization;
using System;
using System.Collections.Generic;

namespace Application.Features.Raporlama.Queries.GetUserSummary;

internal static class RoleLabelProvider
{
    private static readonly Dictionary<string, string> RoleLabels = new(StringComparer.OrdinalIgnoreCase)
    {
        [ApplicationRoles.BakanlikYetkilisi] = "Bakanlık Yetkilisi",
        [ApplicationRoles.IlYetkilisi] = "İl Yetkilisi",
        [ApplicationRoles.IlceYetkilisi] = "İlçe Yetkilisi",
        [ApplicationRoles.OkulKutuphaneYoneticisi] = "Okul Kütüphane Yöneticisi"
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
