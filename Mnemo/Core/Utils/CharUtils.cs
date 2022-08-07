namespace Mnemo.Core.Utils
{
    public static class CharUtils
    {
        public static bool IsLetter(this char self) => (self >= 'a' && self <= 'z') || (self >= 'A' && self <= 'Z');
        public static bool IsNumber(this char self) => self >= '0' && self <= '9';
        public static bool IsPartOfLiteral(this char self) => self.IsLetter() || self.IsNumber() || self == '_';
        public static bool IsSpace(this char self) => self == ' ';
        public static bool IsDoubleQuote(this char self) => self == '"';
        public static bool IsEnd(this char self) => self == '\n';
        public static bool IsInvalid(this char self) => self == '\r';
    }
}
