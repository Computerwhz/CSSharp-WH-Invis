using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace CSSharpWHInvi;

public class Config : BasePluginConfig
{
    [JsonPropertyName("WallhackConfig")] public WallHackConfig WallHackConfig { get; set; } = new();
    [JsonPropertyName("SelfDamage")] public SelfDamageConfig SelfDamageConfig { get; set; } = new();
    [JsonPropertyName("CommandPermission")] public string AdminPermission { get; set; } = "@css/generic";
}

public class WallHackConfig
{
    [JsonPropertyName("DynamicColor")] public bool dynamicColor { get; set; } =  true;
    [JsonPropertyName("ColorR")] public byte R { get; set; } = 171;
    [JsonPropertyName("ColorG")] public byte G { get; set; } = 75;
    [JsonPropertyName("ColorB")] public byte B { get; set; } = 209;
}

public class SelfDamageConfig
{
    [JsonPropertyName("ak-47")] public int WeaponAk47 { get; set; } = 2;
    [JsonPropertyName("aug")] public int WeaponAug { get; set; } = 2;
    [JsonPropertyName("awp")] public int WeaponAwp { get; set; } = 5;
    [JsonPropertyName("cz75-auto")] public int WeaponCz75a { get; set; } = 2;
    [JsonPropertyName("desert eagle")] public int WeaponDeagle { get; set; } = 2;
    [JsonPropertyName("dual berettas")] public int WeaponElite { get; set; } = 2;
    [JsonPropertyName("famas")] public int WeaponFamas { get; set; } = 2;
    [JsonPropertyName("five-seven")] public int WeaponFiveseven { get; set; } = 2;
    [JsonPropertyName("g3sg1")] public int WeaponG3sg1 { get; set; } = 2;
    [JsonPropertyName("galil ar")] public int WeaponGalilar { get; set; } = 2;
    [JsonPropertyName("glock-18")] public int WeaponGlock { get; set; } = 2;
    [JsonPropertyName("m249")] public int WeaponM249 { get; set; } = 2;
    [JsonPropertyName("m4a1-s")] public int WeaponM4a1Silencer { get; set; } = 2;
    [JsonPropertyName("m4a4")] public int WeaponM4a1 { get; set; } = 2;
    [JsonPropertyName("mac-10")] public int WeaponMac10 { get; set; } = 2;
    [JsonPropertyName("mag-7")] public int WeaponMag7 { get; set; } = 8;
    [JsonPropertyName("mp5-sd")] public int WeaponMp5sd { get; set; } = 2;
    [JsonPropertyName("mp7")] public int WeaponMp7 { get; set; } = 2;
    [JsonPropertyName("mp9")] public int WeaponMp9 { get; set; } = 2;
    [JsonPropertyName("negev")] public int WeaponNegev { get; set; } = 2;
    [JsonPropertyName("nova")] public int WeaponNova { get; set; } = 8;
    [JsonPropertyName("p2000")] public int WeaponHkp2000 { get; set; } = 2;
    [JsonPropertyName("p250")] public int WeaponP250 { get; set; } = 2;
    [JsonPropertyName("p90")] public int WeaponP90 { get; set; } = 2;
    [JsonPropertyName("pp-bizon")] public int WeaponBizon { get; set; } = 2;
    [JsonPropertyName("r8 revolver")] public int WeaponRevolver { get; set; } = 2;
    [JsonPropertyName("sawed-off")] public int WeaponSawedoff { get; set; } = 8;
    [JsonPropertyName("scar-20")] public int WeaponScar20 { get; set; } = 2;
    [JsonPropertyName("sg 553")] public int WeaponSg556 { get; set; } = 2;
    [JsonPropertyName("ssg 08")] public int WeaponSsg08 { get; set; } = 5;
    [JsonPropertyName("tec-9")] public int WeaponTec9 { get; set; } = 2;
    [JsonPropertyName("ump-45")] public int WeaponUmp45 { get; set; } = 2;
    [JsonPropertyName("usp-s")] public int WeaponUspSilencer { get; set; } = 2;
    [JsonPropertyName("xm1014")] public int WeaponXm1014 { get; set; } = 8;
    [JsonPropertyName("zeus x27")] public int WeaponTaser { get; set; } = 2;
}