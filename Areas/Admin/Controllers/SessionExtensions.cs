using Microsoft.AspNetCore.Http;
using System;

public static class SessionExtensions
{
    // Phương thức lưu decimal vào session
    public static void SetDecimal(this ISession session, string key, decimal value)
    {
        session.SetString(key, value.ToString());
    }

    // Phương thức lấy decimal từ session
    public static decimal? GetDecimal(this ISession session, string key)
    {
        var value = session.GetString(key);
        if (decimal.TryParse(value, out decimal result))
        {
            return result;
        }
        return null; // Hoặc trả về 0 tùy theo yêu cầu
    }
}
