using System;
using System.Collections.Generic;
using System.Text;
using ParcelTracking.Domain.Enums;

namespace ParcelTracking.Domain.Rules;

public static class ParcelStatusTransitionRule
{
    private static readonly Dictionary<ParcelStatus, ParcelStatus[]> ValidTransitions =
        new()
        {
            { ParcelStatus.COLLECTED, new[] { ParcelStatus.SOURCE_SORT } },

            { ParcelStatus.SOURCE_SORT, new[] { ParcelStatus.DESTINATION_SORT } },

            { ParcelStatus.DESTINATION_SORT, new[] { ParcelStatus.DELIVERY_CENTRE } },

            { ParcelStatus.DELIVERY_CENTRE, new[] { ParcelStatus.READY_FOR_DELIVERY } },

            { ParcelStatus.READY_FOR_DELIVERY, new[] { ParcelStatus.DELIVERED, ParcelStatus.FAILED_TO_DELIVER } },

            { ParcelStatus.FAILED_TO_DELIVER, new[] { ParcelStatus.READY_FOR_DELIVERY, ParcelStatus.RETURNED } }
        };

    public static bool IsValid(ParcelStatus current, ParcelStatus next)
    {
        if (!ValidTransitions.ContainsKey(current))
            return false;

        return ValidTransitions[current].Contains(next);
    }
}


