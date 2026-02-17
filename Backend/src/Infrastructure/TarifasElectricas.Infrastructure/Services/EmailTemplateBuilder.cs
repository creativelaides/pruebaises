using System.Text;

namespace TarifasElectricas.Infrastructure.Services;

/// <summary>
/// Plantillas simples para correos HTML.
/// </summary>
public static class EmailTemplateBuilder
{
    public static string WrapPlainText(string title, string body)
    {
        var safeTitle = System.Net.WebUtility.HtmlEncode(title);
        var safeBody = System.Net.WebUtility.HtmlEncode(body)
            .Replace("\r\n", "<br/>")
            .Replace("\n", "<br/>");

        var sb = new StringBuilder();
        sb.AppendLine("<!doctype html>");
        sb.AppendLine("<html lang=\"es\">");
        sb.AppendLine("<head>");
        sb.AppendLine("<meta charset=\"utf-8\"/>");
        sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"/>");
        sb.AppendLine("<style>");
        sb.AppendLine("body{margin:0;background:#f4f6fb;font-family:Arial,Helvetica,sans-serif;color:#111}");
        sb.AppendLine(".wrap{max-width:600px;margin:24px auto;padding:0 12px}");
        sb.AppendLine(".card{background:#fff;border-radius:12px;padding:24px;border:1px solid #e6e8ef}");
        sb.AppendLine(".title{font-size:18px;font-weight:700;margin:0 0 12px}");
        sb.AppendLine(".divider{height:1px;background:#eef1f6;margin:16px 0}");
        sb.AppendLine(".footer{font-size:12px;color:#6b7280;margin-top:16px}");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<div class=\"wrap\">");
        sb.AppendLine("<div class=\"card\">");
        sb.AppendLine($"<div class=\"title\">{safeTitle}</div>");
        sb.AppendLine("<div class=\"divider\"></div>");
        sb.AppendLine($"<div>{safeBody}</div>");
        sb.AppendLine("<div class=\"footer\">PruebaIses</div>");
        sb.AppendLine("</div>");
        sb.AppendLine("</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }
}
