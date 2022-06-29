using System;
using SRML.Utils.Enum;

namespace ModdedIds
{
    [EnumHolder] //With this, you are indicating SRML that all the `public static readonly` variables are Enums
    public static class ModdedIds
    {
        public static readonly Identifiable.Id GLUE_SLIME; // Creates a new Identifiable.id with the first free value in Identifiable.Id with the name CUSTOM_IDENTIFIABLE

        public static readonly Identifiable.Id GLUE_PLORT;

        public static readonly PediaDirector.Id GLUE_ENTRY;
    }
}