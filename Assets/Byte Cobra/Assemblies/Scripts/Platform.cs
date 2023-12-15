using System.ComponentModel;

namespace ByteCobra.Assemblies
{
    public enum Platform
    {
        [Description("Android")]
        Android,

        [Description("CloudRendering")]
        CloudRendering,

        [Description("Editor")]
        Editor,

        [Description("GameCoreScarlett")]
        GameCoreScarlett,

        [Description("GameCoreXboxOne")]
        GameCoreXboxOne,

        [Description("iOS")]
        iOS,

        [Description("LinuxStandalone64")]
        iOSLinuxStandalone64,

        [Description("Lumin")]
        Lumin,

        [Description("macOSStandalone")]
        macOSStandalone,

        [Description("PS4")]
        PS4,

        [Description("PS5")]
        PS5,

        [Description("Stadia")]
        Stadia,

        [Description("Switch")]
        Switch,

        [Description("tvOS")]
        tvOS,

        [Description("WSA")]
        WSA,

        [Description("WebGL")]
        WebGL,

        [Description("WindowsStandalone32")]
        WindowsStandalone32,

        [Description("WindowsStandalone64")]
        WindowsStandalone64,

        [Description("XboxOne")]
        XboxOne,
    }
}