using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation
{
    [JsonConverter(typeof(EnumRecordJsonConverter<ConversationLanguage>))]
    public record ConversationLanguage(string Value) : EnumRecord(Value)
    {
        public static readonly ConversationLanguage Afrikaans = new("AF");
        public static readonly ConversationLanguage Albanian = new("SQ");
        public static readonly ConversationLanguage Arabic = new("AR");
        public static readonly ConversationLanguage Azerbaijani = new("AZ");
        public static readonly ConversationLanguage Bengali = new("BN");
        public static readonly ConversationLanguage Bulgarian = new("BG");
        public static readonly ConversationLanguage Catalan = new("CA");
        public static readonly ConversationLanguage Chinese = new("ZH");
        public static readonly ConversationLanguage ChineseCHN = new("ZH_CN");
        public static readonly ConversationLanguage ChineseHKG = new("ZH_HK");
        public static readonly ConversationLanguage ChineseTAI = new("ZH_TW");
        public static readonly ConversationLanguage Croatian = new("HR");
        public static readonly ConversationLanguage Czech = new("CS");
        public static readonly ConversationLanguage Danish = new("DA");
        public static readonly ConversationLanguage Dutch = new("NL");
        public static readonly ConversationLanguage English = new("EN");
        public static readonly ConversationLanguage EnglishUK = new("EN_GB");
        public static readonly ConversationLanguage EnglishUS = new("EN_US");
        public static readonly ConversationLanguage Estonian = new("ET");
        public static readonly ConversationLanguage Filipino = new("FIL");
        public static readonly ConversationLanguage Finnish = new("FI");
        public static readonly ConversationLanguage French = new("FR");
        public static readonly ConversationLanguage German = new("DE");
        public static readonly ConversationLanguage Greek = new("EL");
        public static readonly ConversationLanguage Gujarati = new("GU");
        public static readonly ConversationLanguage Hausa = new("HA");
        public static readonly ConversationLanguage Hebrew = new("HE");
        public static readonly ConversationLanguage Hindi = new("HI");
        public static readonly ConversationLanguage Hungarian = new("HU");
        public static readonly ConversationLanguage Indonesian = new("ID");
        public static readonly ConversationLanguage Irish = new("GA");
        public static readonly ConversationLanguage Italian = new("IT");
        public static readonly ConversationLanguage Japanese = new("JA");
        public static readonly ConversationLanguage Kannada = new("KN");
        public static readonly ConversationLanguage Kazakh = new("KK");
        public static readonly ConversationLanguage Korean = new("KO");
        public static readonly ConversationLanguage Lao = new("LO");
        public static readonly ConversationLanguage Latvian = new("LV");
        public static readonly ConversationLanguage Lithuanian = new("LT");
        public static readonly ConversationLanguage Macedonian = new("MK");
        public static readonly ConversationLanguage Malay = new("MS");
        public static readonly ConversationLanguage Malayalam = new("ML");
        public static readonly ConversationLanguage Marathi = new("MR");
        public static readonly ConversationLanguage Norwegian = new("NB");
        public static readonly ConversationLanguage Persian = new("FA");
        public static readonly ConversationLanguage Polish = new("PL");
        public static readonly ConversationLanguage Portuguese = new("PT");
        public static readonly ConversationLanguage PortugueseBr = new("PT_BR");
        public static readonly ConversationLanguage PortuguesePt = new("PT_PT");
        public static readonly ConversationLanguage Punjabi = new("PA");
        public static readonly ConversationLanguage Romanian = new("RO");
        public static readonly ConversationLanguage Russian = new("RU");
        public static readonly ConversationLanguage Serbian = new("SR");
        public static readonly ConversationLanguage Slovak = new("SK");
        public static readonly ConversationLanguage Slovenian = new("SL");
        public static readonly ConversationLanguage Spanish = new("ES");
        public static readonly ConversationLanguage SpanishArg = new("ES_AR");
        public static readonly ConversationLanguage SpanishSpa = new("ES_ES");
        public static readonly ConversationLanguage SpanishMex = new("ES_MX");
        public static readonly ConversationLanguage Swahili = new("SW");
        public static readonly ConversationLanguage Swedish = new("SV");
        public static readonly ConversationLanguage Tamil = new("TA");
        public static readonly ConversationLanguage Telugu = new("TE");
        public static readonly ConversationLanguage Thai = new("TH");
        public static readonly ConversationLanguage Turkish = new("TR");
        public static readonly ConversationLanguage Ukrainian = new("UK");
        public static readonly ConversationLanguage Urdu = new("UR");
        public static readonly ConversationLanguage Uzbek = new("UZ");
        public static readonly ConversationLanguage Vietnamese = new("VI");
        public static readonly ConversationLanguage Zulu = new("ZU");
        public static readonly ConversationLanguage Unspecified = new("UNSPECIFIED");
    }
}
