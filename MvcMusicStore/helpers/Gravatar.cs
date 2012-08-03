using System.Security.Cryptography;
using System.Web.Mvc;
public static class GravatarHelper
{

    public static string Gravatar(this HtmlHelper helper, string email, int size)
    {
        var result = "<img src=\"{0}\" alt=\"Gravatar\" class=\"gravatar\" />";
        var url = GetGravatarURL(email, size);
        return string.Format(result, url);
    }

    static string GetGravatarURL(string email, int size)
    {
        return (string.Format("http://www.gravatar.com/avatar/{0}?s={1}&r=PG",
                    EncryptMD5(email), size.ToString()));
    }

    static string GetGravatarURL(string email, int size, string defaultImagePath)
    {
        return GetGravatarURL(email, size) + string.Format("&default={0}",
                   defaultImagePath);
    }

    static string EncryptMD5(string Value)
    {
        var md5 = new MD5CryptoServiceProvider();
        var valueArray = System.Text.Encoding.ASCII.GetBytes(Value);
        valueArray = md5.ComputeHash(valueArray);
        var encrypted = "";
        for (var i = 0; i < valueArray.Length; i++)
            encrypted += valueArray[i].ToString("x2").ToLower();
        return encrypted;
    }
}
