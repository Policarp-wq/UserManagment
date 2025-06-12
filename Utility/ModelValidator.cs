namespace UserManagment.Utility
{
    public static class ModelValidator
    {
        public static bool IsPasswordValid(string password)
            => password.Length > 0 && password.All(c => char.IsDigit(c) || IsCharLatin(c));
        public static bool IsLoginValid(string login)
            => login.Length > 0 && login.All(c => char.IsDigit(c) || IsCharLatin(c));
        public static bool IsNameValid(string name)
            => name.Length > 0 && name.All(c => IsCharLatin(c) || IsCharRu(c));
        public static bool IsCharLatin(char c)
            => c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
        public static bool IsCharRu(char c)
            => c >= 'а' && c <= 'я' || c >= 'А' && c <= 'Я';
        public static bool IsGenderValid(int gender) => gender >= 0 && gender <= 2;
    }
}
